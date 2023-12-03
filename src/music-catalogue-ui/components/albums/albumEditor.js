import styles from "./albumEditor.module.css";
import pages from "@/helpers/navigation";
import FormInputField from "../common/formInputField";
import { apiCreateAlbum, apiUpdateAlbum } from "@/helpers/api/apiAlbums";
import { useState, useCallback } from "react";
import GenreSelector from "../genres/genreSelector";

/**
 * Component to render an album editor, excluding purchase details that are
 * maintained via their own component and the catalogue, which is maintained
 * via the album list
 * @param {*} artist
 * @param {*} album
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const AlbumEditor = ({ artist, album, isWishList, navigate, logout }) => {
  // Get the initial genre selection
  let initialGenre = null;
  if (album != null) {
    initialGenre = album.genre;
  }

  // Get initial values for the remaining album properties
  const initialTitle = album != null ? album.title : null;
  const initialReleased = album != null ? album.released : null;
  const initialCoverUrl = album != null ? album.coverUrl : null;

  // Setup state
  const [title, setTitle] = useState(initialTitle);
  const [genre, setGenre] = useState(initialGenre);
  const [released, setReleased] = useState(initialReleased);
  const [coverUrl, setCoverUrl] = useState(initialCoverUrl);
  const [error, setError] = useState("");

  const saveAlbum = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Clear pre-existing errors
      setError("");

      try {
        // Get the genre ID
        const genreId = genre != null ? genre.id : null;

        // Either add or update the album, depending on whether there's an
        // existing album or not
        let updatedAlbum = null;
        if (album == null) {
          // Create the album
          updatedAlbum = await apiCreateAlbum(
            artist.id,
            genreId,
            title,
            released,
            coverUrl,
            isWishList,
            null,
            null,
            null,
            logout
          );
        } else {
          // Update the existing album
          updatedAlbum = await apiUpdateAlbum(
            album.id,
            artist.id,
            genreId,
            title,
            released,
            coverUrl,
            album.isWishListItem,
            album.purchased,
            album.price,
            album.retailerId,
            logout
          );
        }

        // Go back to the album list for the artist, which should reflect the
        // updated details
        navigate({
          page: pages.albums,
          artist: artist,
          isWishList: isWishList,
        });
      } catch (ex) {
        setError(`Error saving the updated album details: ${ex.message}`);
      }
    },
    [
      album,
      artist,
      title,
      genre,
      released,
      coverUrl,
      isWishList,
      navigate,
      logout,
    ]
  );

  // Set the page title
  const pageTitle =
    album != null
      ? `${album.title} - ${artist.name}`
      : `New Album - ${artist.name}`;

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{pageTitle}</h5>
      </div>
      <div className={styles.albumEditorFormContainer}>
        <form className={styles.albumEditorForm}>
          <div className="row">
            {error != "" ? (
              <div className={styles.albumEditorError}>{error}</div>
            ) : (
              <></>
            )}
          </div>
          <div className="row align-items-start">
            <FormInputField
              label="Title"
              name="title"
              value={title}
              setValue={setTitle}
            />
          </div>
          <div className="form-group mt-3">
            <label className={styles.albumEditorFormLabel}>Genre</label>
            <div>
              <GenreSelector
                initialGenre={genre}
                genreChangedCallback={setGenre}
                logout={logout}
              />
            </div>
          </div>
          <div className="row align-items-start">
            <FormInputField
              label="Released"
              name="released"
              value={released}
              setValue={setReleased}
            />
          </div>
          <div className="row align-items-start">
            <FormInputField
              label="Cover URL"
              name="coverUrl"
              value={coverUrl}
              setValue={setCoverUrl}
            />
          </div>
          <div className="d-grid gap-2 mt-3"></div>
          <div className="d-grid gap-2 mt-3"></div>
          <div className={styles.albumEditorButton}>
            <button className="btn btn-primary" onClick={(e) => saveAlbum(e)}>
              Save
            </button>
          </div>
          <div className={styles.albumEditorButton}>
            <button
              className="btn btn-primary"
              onClick={() =>
                navigate({
                  page: pages.albums,
                  artist: artist,
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

export default AlbumEditor;
