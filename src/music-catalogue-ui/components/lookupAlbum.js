import styles from "./lookupAlbum.module.css";
import pages from "@/helpers/navigation";
import { useCallback, useState } from "react";
import { apiFetchArtistById } from "@/helpers/apiArtists";
import { apiLookupAlbum } from "@/helpers/apiAlbums";
import Select from "react-select";

/**
 * Component to render the album lookup page
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const LookupAlbum = ({ navigate, logout }) => {
  // Configure state for the controlled fields
  const [artistName, setArtistName] = useState("");
  const [albumTitle, setAlbumTitle] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const [catalogue, setCatalogue] = useState("wishlist");

  // Lookup navigation callback
  const lookup = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Determine where to store new albums based on the target drop-down
      const storeInWishList = catalogue.value == "wishlist";

      // Lookup the album - this will preferentially use the local database via the
      // REST API and fallback to the external API if needed
      const album = await apiLookupAlbum(
        artistName,
        albumTitle,
        storeInWishList,
        logout
      );
      if (album != null) {
        // The album only contains the artist ID, not the full artist details, but
        // they will now be stored locally, so fetch them
        const artist = await apiFetchArtistById(album.artistId, logout);
        if (artist != null) {
          // Navigate to the track list
          navigate(pages.tracks, artist, album, storeInWishList);
        } else {
          setErrorMessage(`Artist with id ${album.artistId} not found`);
        }
      } else {
        setErrorMessage(`Album "${albumTitle}" by "${artistName}" not found`);
      }
    },
    [artistName, albumTitle, catalogue, navigate, logout]
  );

  // Construct a list of select list options for the target directory
  const options = [
    { value: "wishlist", label: "Wish List" },
    { value: "catalogue", label: "Main Catalogue" },
  ];

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Lookup Album</h5>
      </div>
      <div className={styles.lookupFormContainer}>
        <form className={styles.lookupForm}>
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
            <div className="form-group mt-3">
              <label className={styles.lookupFormLabel}>Save Album To</label>
              <Select
                className={styles.lookupCatalogueSelector}
                defaultValue={catalogue}
                onChange={setCatalogue}
                options={options}
              />
            </div>
            <div className="d-grid gap-2 mt-3">
              <span className={styles.lookupError}>{errorMessage}</span>
            </div>
            <div className="d-grid gap-2 mt-3"></div>
            <div className={styles.lookupButton}>
              <button className="btn btn-primary" onClick={() => lookup()}>
                Lookup
              </button>
            </div>
          </div>
        </form>
      </div>
    </>
  );
};

export default LookupAlbum;
