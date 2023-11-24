import styles from "./trackEditor.module.css";
import pages from "@/helpers/navigation";
import FormInputField from "./formInputField";
import { useState, useCallback } from "react";
import { apiCreateTrack, apiUpdateTrack } from "@/helpers/apiTracks";

const TrackEditor = ({ track, album, artist, navigate, logout }) => {
  // Split the track's formatted duration on the ":"
  let initialMinutes = null;
  let initialSeconds = null;
  if (track != null && track.formattedDuration != null) {
    const elements = track.formattedDuration.split(":");
    if (elements.length > 0) {
      initialMinutes = elements[0];
      initialSeconds = elements[1];
    }
  }

  // Get initial values for the other properties
  const initialTitle = track != null ? track.title : null;
  const initialNumber = track != null ? track.number : null;

  const [title, setTitle] = useState(initialTitle);
  const [number, setNumber] = useState(initialNumber);
  const [minutes, setMinutes] = useState(initialMinutes);
  const [seconds, setSeconds] = useState(initialSeconds);
  const [error, setError] = useState("");

  /* Callback to save retailer details */
  const saveTrack = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Clear pre-existing errors
      setError("");

      try {
        // Calculate the duration from the indiviidual minutes and seconds
        // inputs
        const durationMinutes = Number(minutes);
        const durationSeconds = Number(seconds);
        const duration = 1000 * (60 * durationMinutes + durationSeconds);

        // Either add or update the track, depending on whether there's an
        // existing track or not
        let updatedTrack = null;
        if (track == null) {
          updatedTrack = await apiCreateTrack(
            title,
            number,
            duration,
            album.id,
            logout
          );
        } else {
          updatedTrack = await apiUpdateTrack(
            track.id,
            title,
            number,
            duration,
            album.id,
            logout
          );
        }

        // If all's well, navigate back to the track list page. Otherwise, show an error
        if (updatedTrack == null) {
          const action = track.Id <= 0 ? "adding" : "updating";
          setError(`An error occurred ${action} the track`);
        } else {
          navigate({
            page: pages.tracks,
            artist: artist,
            album: album,
          });
        }
      } catch (e) {
        setError(
          e.message
          //"Error converting the supplied minutes and seconds to a duration"
        );
      }
    },
    [album, artist, track, title, number, minutes, seconds, navigate, logout]
  );

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{title}</h5>
      </div>
      <div className={styles.trackEditorFormContainer}>
        <form className={styles.trackEditorForm}>
          <div className="row">
            {error != "" ? (
              <div className={styles.trackEditorError}>{error}</div>
            ) : (
              <></>
            )}
          </div>
          <div className="row align-items-start">
            <div className="col">
              <FormInputField
                label="Title"
                name="title"
                value={title}
                setValue={setTitle}
              />
            </div>
          </div>
          <div className="row align-items-start">
            <div className="col">
              <FormInputField
                label="Number"
                name="number"
                value={number}
                setValue={setNumber}
              />
            </div>
          </div>
          <div className="row align-items-start">
            <div className="col">
              <FormInputField
                label="Minutes"
                name="minutes"
                value={minutes}
                setValue={setMinutes}
              />
            </div>
            <div className="col">
              <FormInputField
                label="Seconds"
                name="seconds"
                value={seconds}
                setValue={setSeconds}
              />
            </div>
          </div>
          <div className="d-grid gap-2 mt-3"></div>
          <div className="d-grid gap-2 mt-3"></div>
          <div className={styles.trackEditorButton}>
            <button className="btn btn-primary" onClick={(e) => saveTrack(e)}>
              Save
            </button>
          </div>
          <div className={styles.trackEditorButton}>
            <button
              className="btn btn-primary"
              onClick={() =>
                navigate({
                  page: pages.tracks,
                  artist: artist,
                  album: album,
                })
              }
            >
              Cancel
            </button>
          </div>
        </form>
      </div>
    </>
  );
};

export default TrackEditor;
