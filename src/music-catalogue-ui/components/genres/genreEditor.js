import styles from "./genreEditor.module.css";
import pages from "@/helpers/navigation";
import FormInputField from "../common/formInputField";
import { apiCreateGenre, apiUpdateGenre } from "@/helpers/api/apiGenres";
import { useState, useCallback } from "react";

/**
 * Component to render a genre editor
 * @param {*} genre
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const GenreEditor = ({ genre, navigate, logout }) => {
  // Setup state
  const initialName = genre != null ? genre.name : "";
  const [name, setName] = useState(initialName);
  const [error, setError] = useState("");

  const saveGenre = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Clear pre-existing errors
      setError("");

      try {
        // Either add or update the genre, depending on whether there's an
        // existing album or not
        let updatedGenre = null;
        if (genre == null) {
          // Create the genre
          updatedGenre = await apiCreateGenre(name, logout);
        } else {
          // Update the existing genre
          updatedGenre = await apiUpdateGenre(genre.id, name, logout);
        }

        // Go back to the genre, which should reflect the updated details
        navigate({
          page: pages.genres,
        });
      } catch (ex) {
        setError(`Error saving the updated genre details: ${ex.message}`);
      }
    },
    [genre, name, navigate, logout]
  );

  // Set the page title
  const pageTitle = genre != null ? `Genre - ${genre.name}` : `New Genre`;

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{pageTitle}</h5>
      </div>
      <div className={styles.genreEditorFormContainer}>
        <form className={styles.genreEditorForm}>
          <div className="row">
            {error != "" ? (
              <div className={styles.genreEditorError}>{error}</div>
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
          <div className={styles.genreEditorButton}>
            <button className="btn btn-primary" onClick={(e) => saveGenre(e)}>
              Save
            </button>
          </div>
          <div className={styles.genreEditorButton}>
            <button
              className="btn btn-primary"
              onClick={() =>
                navigate({
                  page: pages.genres,
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

export default GenreEditor;
