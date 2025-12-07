import React, { useCallback, useState } from "react";
import styles from "./albumPicker.module.css";
import { apiFetchRandomAlbum } from "@/helpers/api/apiAlbums";
import AlbumPickerAlbumRow from "./albumPickerAlbumRow";
import GenreSelector from "../genres/genreSelector";
import { apiFetchArtistById } from "@/helpers/api/apiArtists";

/**
 * Component to pick a random album, optionally for a specified genre
 * @param {*} logout
 * @returns
 */
const AlbumPicker = ({ logout }) => {
  const [genre, setGenre] = useState(null);
  const [details, setDetails] = useState({ album: null, artist: null });

  // Callback to request a random album from the API
  const pickAlbumCallback = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Request a random album, optionally filtering by the selected genre, and
      // retrieve the artist details
      const genreId = genre != null ? genre.id : null;
      const fetchedAlbum = await apiFetchRandomAlbum(genreId, logout);
      if (fetchedAlbum != null) {
        const fetchedArtist = await apiFetchArtistById(
          fetchedAlbum.artistId,
          logout
        );
        setDetails({ album: fetchedAlbum, artist: fetchedArtist });
      } else {
        setDetails({ album: null, artist: null });
      }
    },
    [genre, logout]
  );

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Album Picker</h5>
      </div>
      <div className={styles.albumPickerFormContainer}>
        <form className={styles.albumPickerForm}>
          <div className="row" align="center">
            <div className="mt-3">
              <div className="d-inline-flex align-items-center">
                <div className="col">
                  <label className={styles.albumPickerLabel}>
                    Genre to pick from:
                  </label>
                </div>
                <div className="col">
                  <div className={styles.alunmPickerGenreSelector}>
                    <GenreSelector
                      initialGenre={genre}
                      genreChangedCallback={setGenre}
                    />
                  </div>
                </div>
                <div className="col">
                  <button
                    className="btn btn-primary"
                    onClick={(e) => pickAlbumCallback(e)}
                  >
                    Pick
                  </button>
                </div>
              </div>
            </div>
          </div>
        </form>
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Artist</th>
            <th>Title</th>
            <th>Genre</th>
            <th>Released</th>
            <th>Purchased</th>
            <th>Price</th>
            <th>Retailer</th>
          </tr>
        </thead>
        {details.album != null && (
          <tbody>
            {
              <AlbumPickerAlbumRow
                key={details.album.id}
                album={details.album}
                artist={details.artist}
              />
            }
          </tbody>
        )}
      </table>
    </>
  );
};

export default AlbumPicker;
