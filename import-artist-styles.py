"""
Apply artist style properties from a CSV to the SQLite ARTISTS table.

CSV expected columns (case-insensitive):
  - artist
  - energy
  - intimacy
  - warmth
  - vocals_presence   (Unknown/Instrumental/Mixed/VocalLed OR 0..3)
  - ensemble_bias     (Unknown/Band/BigBand/Choir/Orchestra/Quartet/SmallCombo/Solo/Studio/Trio OR 0..9)

For example:

artist,energy,intimacy,warmth,vocals_presence,ensemble_bias
Julie London,2,5,4,vocal_led,small_combo

DB table:
CREATE TABLE ARTISTS (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    SearchableName TEXT NULL,
    Energy INTEGER NOT NULL DEFAULT 0,
    Ensemble INTEGER NOT NULL DEFAULT 0,
    Intimacy INTEGER NOT NULL DEFAULT 0,
    Vocals INTEGER NOT NULL DEFAULT 0,
    Warmth INTEGER NOT NULL DEFAULT 0
);
"""

from __future__ import annotations

import argparse
import csv
import re
import sqlite3
import unicodedata
from dataclasses import dataclass
from pathlib import Path
from typing import Any, Dict, Optional, Tuple


# ---- Enum mappings (match C# enums) ----

VOCALS_MAP = {
    "UNKNOWN": 0,
    "INSTRUMENTAL": 1,
    "MIXED": 2,
    "VOCALLED": 3,
    "VOCAL_LED": 3,
    "VOCAL-LED": 3,
    "VOCAL LED": 3,
}

ENSEMBLE_MAP = {
    "UNKNOWN": 0,
    "BAND": 1,
    "BIGBAND": 2,
    "BIG_BAND": 2,
    "BIG-BAND": 2,
    "BIG BAND": 2,
    "CHOIR": 3,
    "ORCHESTRA": 4,
    "QUARTET": 5,
    "SMALLCOMBO": 6,
    "SMALL_COMBO": 6,
    "SMALL-COMBO": 6,
    "SMALL COMBO": 6,
    "SOLO": 7,
    "STUDIO": 8,
    "TRIO": 9,
}


# ---- Normalization helpers ----

def normalize_name(s: str) -> str:
    """
    Normalize artist names to improve matching:
    - strip/condense whitespace
    - remove accents
    - unify quotes/apostrophes
    - replace '&' with 'and'
    - remove most punctuation
    - lowercase
    """
    if s is None:
        return ""

    s = s.strip()

    # normalize unicode (accents)
    s = unicodedata.normalize("NFKD", s)
    s = "".join(ch for ch in s if not unicodedata.combining(ch))

    # unify apostrophes/quotes
    s = s.replace("’", "'").replace("`", "'").replace("“", '"').replace("”", '"')

    # common substitutions
    s = s.replace("&", " and ")

    # collapse whitespace
    s = re.sub(r"\s+", " ", s)

    # drop punctuation (keep letters/numbers/spaces)
    s = re.sub(r"[^a-zA-Z0-9 ]+", "", s)

    return s.lower().strip()


def parse_int(value: Any, default: int = 0) -> int:
    try:
        if value is None:
            return default
        if isinstance(value, int):
            return value
        s = str(value).strip()
        if s == "":
            return default
        return int(float(s))
    except Exception:
        return default


def parse_enum(value: Any, mapping: Dict[str, int], default: int = 0) -> int:
    """
    Accept either numeric strings ('2') or enum names ('VocalLed').
    """
    if value is None:
        return default
    s = str(value).strip()
    if s == "":
        return default

    # numeric?
    if re.fullmatch(r"-?\d+", s):
        return int(s)

    key = re.sub(r"\s+", "_", s).upper()
    key = key.replace("-", "_")
    return mapping.get(key, mapping.get(key.replace("_", ""), default))


@dataclass
class StyleRow:
    artist: str
    energy: int
    intimacy: int
    warmth: int
    vocals: int
    ensemble: int


def read_style_csv(csv_path: Path) -> list[StyleRow]:
    with csv_path.open("r", encoding="utf-8-sig", newline="") as f:
        reader = csv.DictReader(f)
        if not reader.fieldnames:
            raise ValueError("CSV has no header row.")

        # map headers case-insensitively
        headers = {h.lower().strip(): h for h in reader.fieldnames}

        def get(row: dict, key: str) -> str:
            h = headers.get(key)
            return row.get(h, "") if h else ""

        rows: list[StyleRow] = []
        for r in reader:
            artist = (get(r, "artist") or get(r, "name") or "").strip()
            if not artist:
                continue

            rows.append(
                StyleRow(
                    artist=artist,
                    energy=parse_int(get(r, "energy"), 0),
                    intimacy=parse_int(get(r, "intimacy"), 0),
                    warmth=parse_int(get(r, "warmth"), 0),
                    vocals=parse_enum(get(r, "vocals_presence"), VOCALS_MAP, 0),
                    ensemble=parse_enum(get(r, "ensemble_bias"), ENSEMBLE_MAP, 0),
                )
            )
        return rows


# ---- SQLite update logic ----

def connect(db_path: Path) -> sqlite3.Connection:
    conn = sqlite3.connect(str(db_path))
    conn.row_factory = sqlite3.Row
    return conn


def build_artist_index(conn: sqlite3.Connection) -> Dict[str, Tuple[int, sqlite3.Row]]:
    """
    Index artists by normalized Name and SearchableName for quick matching.
    """
    cur = conn.execute("SELECT Id, Name, SearchableName, Energy, Ensemble, Intimacy, Vocals, Warmth FROM ARTISTS")
    index: Dict[str, Tuple[int, sqlite3.Row]] = {}
    for row in cur.fetchall():
        for key in (row["Name"], row["SearchableName"]):
            if key:
                index[normalize_name(key)] = (row["Id"], row)
    return index


def update_artist_style(
    conn: sqlite3.Connection,
    artist_id: int,
    new_values: StyleRow,
    only_fill_blanks: bool = True,
) -> None:
    """
    Update style columns. If only_fill_blanks=True, only overwrite columns where existing value is 0.
    """
    cur = conn.execute(
        "SELECT Energy, Ensemble, Intimacy, Vocals, Warmth FROM ARTISTS WHERE Id = ?",
        (artist_id,),
    )
    existing = cur.fetchone()
    if existing is None:
        return

    to_set: Dict[str, int] = {}

    def maybe_set(col: str, new_val: int) -> None:
        if new_val is None:
            return
        if only_fill_blanks:
            if int(existing[col]) == 0 and int(new_val) != 0:
                to_set[col] = int(new_val)
        else:
            if int(new_val) != 0:
                to_set[col] = int(new_val)

    maybe_set("Energy", new_values.energy)
    maybe_set("Intimacy", new_values.intimacy)
    maybe_set("Warmth", new_values.warmth)
    maybe_set("Vocals", new_values.vocals)
    maybe_set("Ensemble", new_values.ensemble)

    if not to_set:
        return

    sets = ", ".join([f"{col} = ?" for col in to_set.keys()])
    params = list(to_set.values()) + [artist_id]
    conn.execute(f"UPDATE ARTISTS SET {sets} WHERE Id = ?", params)


def main() -> None:
    ap = argparse.ArgumentParser(description="Apply artist style properties from a CSV file to the ARTISTS database table")
    ap.add_argument("--db", required=True, help="Path to SQLite database")
    ap.add_argument("--csv", required=True, help="Path to CSV file with artist style properties")
    ap.add_argument(
        "--overwrite",
        action="store_true",
        help="Overwrite existing non-zero values (default: only fill blanks where DB value is 0).",
    )
    ap.add_argument(
        "--dry-run",
        action="store_true",
        help="Don't write changes; just report what would change.",
    )
    ap.add_argument(
        "--report-missing",
        action="store_true",
        help="Print artists from CSV that weren't found in the database.",
    )
    args = ap.parse_args()

    db_path = Path(args.db)
    csv_path = Path(args.csv)

    rows = read_style_csv(csv_path)

    conn = connect(db_path)
    try:
        index = build_artist_index(conn)

        updated = 0
        missing: list[str] = []

        for r in rows:
            key = normalize_name(r.artist)
            hit = index.get(key)

            # small extra trick: try stripping leading "the "
            if hit is None and key.startswith("the "):
                hit = index.get(key[4:])

            if hit is None:
                missing.append(r.artist)
                continue

            artist_id, _ = hit

            if args.dry_run:
                # simulate by checking which cols would be set
                cur = conn.execute(
                    "SELECT Energy, Ensemble, Intimacy, Vocals, Warmth FROM ARTISTS WHERE Id = ?",
                    (artist_id,),
                )
                ex = cur.fetchone()
                before = dict(ex) if ex else {}
                # naive simulation: call update in a savepoint and roll back
                conn.execute("SAVEPOINT dryrun")
                update_artist_style(conn, artist_id, r, only_fill_blanks=not args.overwrite)
                cur2 = conn.execute(
                    "SELECT Energy, Ensemble, Intimacy, Vocals, Warmth FROM ARTISTS WHERE Id = ?",
                    (artist_id,),
                )
                after = dict(cur2.fetchone())
                conn.execute("ROLLBACK TO dryrun")
                conn.execute("RELEASE dryrun")

                if before != after:
                    updated += 1
                    print(f"[DRY] {r.artist}: {before} -> {after}")
            else:
                before_changes = conn.total_changes
                update_artist_style(conn, artist_id, r, only_fill_blanks=not args.overwrite)
                if conn.total_changes > before_changes:
                    updated += 1

        if not args.dry_run:
            conn.commit()

        print(f"Updated {updated} artist(s).")

        if args.report_missing and missing:
            print("\nMissing (in CSV but not in DB):")
            for m in missing:
                print(f" - {m}")

    finally:
        conn.close()


if __name__ == "__main__":
    main()