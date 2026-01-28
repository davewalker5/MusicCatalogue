#!/usr/bin/env python3
"""
Load artist moods from a CSV and populate ARTIST_MOODS in SQLite.

Steps:
1) Extract artist name
2) Extract moods string
3) Split moods by ';' and for each mood:
   - replace '_' with ' '
   - convert to Title Case
4) Lookup ArtistId in ARTISTS
5) Lookup MoodId in MOODS (by Name)
6) If both found, insert into ARTIST_MOODS (avoid duplicates)

Supports:
- --dry-run (no writes)
- --report-missing (print artists/moods not found)
- --overwrite (optional: wipe existing links for matching artists first)

CSV expected headers (case-insensitive):
- artist (or name)
- default_moods (or moods)

For example:

artist,default_moods
Julie London,late_night;intimate;smoky
"""

from __future__ import annotations

import argparse
import csv
import re
import sqlite3
import unicodedata
from dataclasses import dataclass
from pathlib import Path
from typing import Dict, List, Set, Tuple


# ---------- Normalization helpers ----------

def normalize_name(s: str) -> str:
    """
    Normalize names for matching (artist names primarily).
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

    s = unicodedata.normalize("NFKD", s)
    s = "".join(ch for ch in s if not unicodedata.combining(ch))

    s = s.replace("’", "'").replace("`", "'").replace("“", '"').replace("”", '"')
    s = s.replace("&", " and ")

    s = re.sub(r"\s+", " ", s)
    s = re.sub(r"[^a-zA-Z0-9 ]+", "", s)

    return s.lower().strip()


def mood_to_db_name(raw: str) -> str:
    """
    Transform CSV mood token to MOODS.Name format:
      - replace '_' with single space
      - strip
      - Title Case
    Example: 'late_night' -> 'Late Night'
    """
    s = (raw or "").strip()
    s = re.sub(r"\s+", " ", s.replace("_", " ")).strip()
    # Title case, but keep small words natural-ish (optional). Simple title() is fine.
    return s.title()


# ---------- SQLite helpers ----------

def connect(db_path: Path) -> sqlite3.Connection:
    conn = sqlite3.connect(str(db_path))
    conn.row_factory = sqlite3.Row
    conn.execute("PRAGMA foreign_keys = ON;")
    return conn


def load_artist_index(conn: sqlite3.Connection) -> Dict[str, int]:
    """
    Map normalized Name/SearchableName -> ArtistId
    """
    cur = conn.execute("SELECT Id, Name, SearchableName FROM ARTISTS")
    idx: Dict[str, int] = {}
    for row in cur.fetchall():
        if row["Name"]:
            idx[normalize_name(row["Name"])] = row["Id"]
        if row["SearchableName"]:
            idx[normalize_name(row["SearchableName"])] = row["Id"]
    return idx


def load_mood_index(conn: sqlite3.Connection) -> Dict[str, int]:
    """
    Map normalized mood Name -> MoodId
    (Assumes MOODS has a Name column.)
    """
    cur = conn.execute("SELECT Id, Name FROM MOODS")
    idx: Dict[str, int] = {}
    for row in cur.fetchall():
        if row["Name"]:
            idx[normalize_name(row["Name"])] = row["Id"]
    return idx


def existing_links(conn: sqlite3.Connection) -> Set[Tuple[int, int]]:
    cur = conn.execute("SELECT ArtistId, MoodId FROM ARTIST_MOODS")
    return {(int(r["ArtistId"]), int(r["MoodId"])) for r in cur.fetchall()}


# ---------- CSV reading ----------

@dataclass(frozen=True)
class ArtistMoodRow:
    artist: str
    moods_raw: str


def read_csv_rows(csv_path: Path) -> List[ArtistMoodRow]:
    with csv_path.open("r", encoding="utf-8-sig", newline="") as f:
        reader = csv.DictReader(f)
        if not reader.fieldnames:
            raise ValueError("CSV has no header row.")

        headers = {h.lower().strip(): h for h in reader.fieldnames}

        def col(row: dict, key: str) -> str:
            h = headers.get(key)
            return (row.get(h, "") if h else "") or ""

        out: List[ArtistMoodRow] = []
        for r in reader:
            artist = (col(r, "artist") or col(r, "name")).strip()
            moods = (col(r, "default_moods") or col(r, "moods")).strip()
            if not artist or not moods:
                continue
            out.append(ArtistMoodRow(artist=artist, moods_raw=moods))
        return out


def split_moods(moods_raw: str) -> List[str]:
    # main format: semicolon separated
    parts = [p.strip() for p in moods_raw.split(";")]
    # drop empties
    return [p for p in parts if p]


# ---------- Main logic ----------

def main() -> None:
    ap = argparse.ArgumentParser(description="Populate ARTIST_MOODS from a CSV mapping between artists and moods")
    ap.add_argument("--db", required=True, help="Path to SQLite database file")
    ap.add_argument("--csv", required=True, help="Path to CSV containing artist moods")
    ap.add_argument("--dry-run", action="store_true", help="Don't write anything; just print actions")
    ap.add_argument("--report-missing", action="store_true", help="Print missing artists/moods")
    ap.add_argument(
        "--overwrite-artists",
        action="store_true",
        help="For artists present in the CSV, delete existing ARTIST_MOODS rows first.",
    )
    args = ap.parse_args()

    db_path = Path(args.db)
    csv_path = Path(args.csv)

    rows = read_csv_rows(csv_path)
    if not rows:
        print("No usable rows found in CSV (need artist + moods).")
        return

    conn = connect(db_path)
    try:
        artist_idx = load_artist_index(conn)
        mood_idx = load_mood_index(conn)
        links = existing_links(conn)

        # If overwriting, we delete per-artist (only those found)
        artists_to_overwrite: Set[int] = set()

        inserts: List[Tuple[int, int]] = []
        missing_artists: Set[str] = set()
        missing_moods: Set[str] = set()

        for row in rows:
            a_key = normalize_name(row.artist)
            artist_id = artist_idx.get(a_key)

            # small extra: try stripping leading "the "
            if artist_id is None and a_key.startswith("the "):
                artist_id = artist_idx.get(a_key[4:])

            if artist_id is None:
                missing_artists.add(row.artist)
                continue

            if args.overwrite_artists:
                artists_to_overwrite.add(int(artist_id))

            for mood_token in split_moods(row.moods_raw):
                mood_name = mood_to_db_name(mood_token)
                m_key = normalize_name(mood_name)
                mood_id = mood_idx.get(m_key)

                if mood_id is None:
                    # Try the token itself normalized:
                    alt_key = normalize_name(mood_token.replace("_", " "))
                    mood_id = mood_idx.get(alt_key)

                if mood_id is None:
                    missing_moods.add(mood_name)
                    continue

                pair = (int(artist_id), int(mood_id))
                if pair in links:
                    continue

                inserts.append(pair)
                links.add(pair)

        # Apply changes
        if args.dry_run:
            if args.overwrite_artists and artists_to_overwrite:
                print(f"[DRY] Would delete existing moods for {len(artists_to_overwrite)} artist(s).")
            for a_id, m_id in inserts[:200]:
                print(f"[DRY] INSERT ArtistId={a_id}, MoodId={m_id}")
            if len(inserts) > 200:
                print(f"[DRY] ... plus {len(inserts) - 200} more inserts")
        else:
            if args.overwrite_artists and artists_to_overwrite:
                conn.executemany(
                    "DELETE FROM ARTIST_MOODS WHERE ArtistId = ?",
                    [(a_id,) for a_id in artists_to_overwrite],
                )

            conn.executemany(
                "INSERT INTO ARTIST_MOODS (ArtistId, MoodId) VALUES (?, ?)",
                inserts,
            )
            conn.commit()

        print(f"Prepared {len(inserts)} new ARTIST_MOODS link(s).")

        if args.report_missing:
            if missing_artists:
                print("\nArtists not found in ARTISTS:")
                for a in sorted(missing_artists):
                    print(f" - {a}")
            if missing_moods:
                print("\nMoods not found in MOODS (by Name):")
                for m in sorted(missing_moods):
                    print(f" - {m}")

    finally:
        conn.close()


if __name__ == "__main__":
    main()
