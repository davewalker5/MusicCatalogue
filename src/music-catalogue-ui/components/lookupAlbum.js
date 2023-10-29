import styles from "./lookupAlbum.module.css";
import pages from "@/helpers/navigation";
import { useCallback, useState } from "react";
import { apiFetchArtistById, apiLookupAlbum } from "@/helpers/api";

/**
 * Component to render the album lookup page
 * @param {*} param0
 * @returns
 */
const LookupAlbum = ({ navigate, logout }) => {
  // Configure state for the controlled fields
  const [artistName, setArtistName] = useState("");
  const [albumTitle, setAlbumTitle] = useState("");
  const [errorMessage, setErrorMessage] = useState("");

  // Lookup navigation callback
  const lookup = useCallback(async () => {
    // Lookup the album - this will preferentially use the local database via the
    // REST API and fallback to the external API if needed
    const album = await apiLookupAlbum(artistName, albumTitle, logout);
    if (album != null) {
      // The album only contains the artist ID, not the full artist details, but
      // they will now be stored locally, so fetch them
      const artist = await apiFetchArtistById(album.artistId, logout);
      if (artist != null) {
        // Navigate to the track list
        navigate(pages.tracks, artist, album);
      } else {
        setErrorMessage(`Artist with id ${album.artistId} not found`);
      }
    } else {
      setErrorMessage(`Album "${albumTitle}" by "${artistName}" not found`);
    }
  }, [artistName, albumTitle, navigate, logout]);

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Lookup Album</h5>
      </div>
      <div className={styles.lookupFormContainer}>
        <div className={styles.lookupForm}>
          <div>
            <div className="form-group mt-3">
              <label className={styles.lookupFormLabel}>Artist Name</label>
              <input
                className="form-control mt-1"
                placeholder="Artist"
                name="artistName"
                value={artistName}
                onChange={(e) => setArtistName(e.target.value)}
              />
            </div>
            <div className="form-group mt-3">
              <label className={styles.lookupFormLabel}>Album Title</label>
              <input
                className="form-control mt-1"
                placeholder="Album"
                name="albumTitle"
                value={albumTitle}
                onChange={(e) => setAlbumTitle(e.target.value)}
              />
            </div>
            <div className="d-grid gap-2 mt-3">
              <span className={styles.lookupError}>{errorMessage}</span>
            </div>
            <div className={styles.lookupButton}>
              <button className="btn btn-primary" onClick={() => lookup()}>
                Lookup
              </button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default LookupAlbum;
