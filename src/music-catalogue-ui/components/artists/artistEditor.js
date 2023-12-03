import styles from "./artistEditor.module.css";
import pages from "@/helpers/navigation";
import FormInputField from "../common/formInputField";
import { useState, useCallback } from "react";
import { apiCreateArtist, apiUpdateArtist } from "@/helpers/api/apiArtists";

/**
 * Component to render the artist editor
 * @param {*} filter
 * @param {*} artist
 * @param {*} isWishList
 * @param {*} logout
 */
const ArtistEditor = ({ filter, artist, isWishList, navigate, logout }) => {
  // Set up state
  const initialName = artist != null ? artist.name : null;
  const [name, setName] = useState(initialName);
  const [error, setError] = useState("");

  const saveArtist = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Clear pre-existing errors
      setError("");

      try {
        // Either add or update the artist, depending on whether there's an
        // existing artist or not
        let updatedArtist = null;
        if (artist == null) {
          // Create the artist
          updatedArtist = await apiCreateArtist(name, logout);
        } else {
          // Update the existing artist
          updatedArtist = await apiUpdateArtist(artist.id, name, logout);
        }

        // Go back to the artist list, which should reflect the updated details
        navigate({
          filter: filter,
          page: pages.artists,
          isWishList: isWishList,
        });
      } catch (ex) {
        setError(`Error saving the updated artist details: ${ex.message}`);
      }
    },
    [filter, artist, isWishList, logout, name, navigate]
  );

  // Set the page title
  const pageTitle = artist != null ? artist.name : "New Artist";

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{pageTitle}</h5>
      </div>
      <div className={styles.artistEditorFormContainer}>
        <form className={styles.artistEditorForm}>
          <div className="row">
            {error != "" ? (
              <div className={styles.artistEditorError}>{error}</div>
            ) : (
              <></>
            )}
          </div>
          <div className="row align-items-start">
            <FormInputField
              label="Name"
              name="name"
              value={name}
              setValue={setName}
            />
          </div>
          <div className="d-grid gap-2 mt-3"></div>
          <div className="d-grid gap-2 mt-3"></div>
          <div className={styles.artistEditorButton}>
            <button className="btn btn-primary" onClick={(e) => saveArtist(e)}>
              Save
            </button>
          </div>
          <div className={styles.artistEditorButton}>
            <button
              className="btn btn-primary"
              onClick={() =>
                navigate({
                  filter: filter,
                  page: pages.artists,
                  isWishList: isWishList,
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

export default ArtistEditor;
