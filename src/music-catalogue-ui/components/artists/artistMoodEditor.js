import styles from "./artistMoodEditor.module.css";
import MoodGrid from "../moods/moodGrid";
import { useState, useCallback, useEffect } from "react";
import { apiCreateArtistMood, apiDeleteArtistMood } from "@/helpers/api/apiArtistMoods";

/**
 * Component to render the artist mood editor
 * @param {*} artist
 * @param {*} logout
 */
const ArtistMoodEditor = ({ artist, logout }) => {
  const [error, setError] = useState("");
  const [selectedMoodIds, setSelectedMoodIds] = useState([]);

  useEffect(() => {
    setSelectedMoodIds((artist?.moods ?? []).map((am) => Number(am.moodId)));
  }, [artist]);

  const updateMoodMapping = useCallback(async (e, checkboxId, checked) => {
    try {
        // Clear pre-existing errors
        setError("");

        // Either add or delete the mapping
        const moodId = Number(checkboxId);
        if (checked) {
            await apiCreateArtistMood(artist.id, moodId, logout);
        } else {
            // Find the ID for the mapping, rather than the mood itself
            const artistMood = artist.moods.find(m => m.moodId === moodId);
            const mappingId = artistMood?.id;
            await apiDeleteArtistMood(mappingId, logout);
        }

        // Update state
        setSelectedMoodIds((prev) =>
            checked ? [...prev, moodId] : prev.filter((x) => x !== moodId)
        );

    } catch (ex) {
        setError(`Error saving the updated artist details: ${ex.message}`);
    }

  }, [logout]);

  const pageTitle = `Moods - ${artist.name}`;

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{pageTitle}</h5>
      </div>

      <div className={styles.artistMoodEditorFormContainer}>
        <form className={styles.artistMoodEditorForm}>
          <div className="row">
            {error !== "" ? (
              <div className={styles.artistMoodEditorError}>{error}</div>
            ) : null}
          </div>

          <MoodGrid
            selectedMoodIds={selectedMoodIds}
            toggleMood={updateMoodMapping}
            logout={logout}
          />
        </form>
      </div>
    </>
  );
};

export default ArtistMoodEditor;
