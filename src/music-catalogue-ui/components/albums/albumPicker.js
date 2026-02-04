import React, { useCallback, useState } from "react";
import styles from "./albumPicker.module.css";
import { apiFetchRandomAlbum } from "@/helpers/api/apiAlbums";
import AlbumPickerAlbumRow from "./albumPickerAlbumRow";
import GenreSelector from "../genres/genreSelector";
import MoodSelector from "../moods/moodSelector";
import Slider from "../common/slider";

/**
 * Component to pick a random album, optionally for a specified genre
 * @param {*} logout
 * @returns
 */
const AlbumPicker = ({ navigate, logout }) => {
  const [genre, setGenre] = useState(null);
  const [mood, setMood] = useState(null);
  const [energy, setEnergy] = useState(3);
  const [intimacy, setIntimacy] = useState(3);
  const [warmth, setWarmth] = useState(3);
  const [pickedAlbums, setPickedAlbums] = useState(null);

  // Callback to request a random album from the API
  const pickAlbumCallback = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Request a set of random albums matching the specified criteria
      const genreId = genre != null ? genre.id : null;
      const moodId = mood != null ? mood.id : null;
      const fetchedAlbums = await apiFetchRandomAlbum(genreId, moodId, energy, intimacy, warmth, logout);
      setPickedAlbums(fetchedAlbums);
    },
    [genre, mood, energy, intimacy, warmth, logout]
  );

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Album Picker</h5>
      </div>
      <div className={styles.albumPickerFormContainer}>
        <form className={styles.albumPickerForm}>
          <div className="row d-flex justify-content-center">
            <div className="col-md-2">
              <div className="form-group mt-3">
                <label className={styles.albumPickerLabel}>Genre</label>
                <div>
                  <GenreSelector
                        initialGenre={genre}
                        genreChangedCallback={setGenre}
                      />
                </div>
              </div>
            </div>
            <div className="col-md-2">
              <div className="form-group mt-3">
                <label className={styles.albumPickerLabel}>Mood</label>
                <div>
                  <MoodSelector
                        initialMood={mood}
                        moodChangedCallback={setMood}
                      />
                </div>
              </div>
            </div>
            <div className="col-md-2">
              <div className="form-group mt-3">
                <label className={styles.albumPickerLabel}>Energy</label>
                <div>
                  <Slider
                    value={energy}
                    minimum={0}
                    maximum={5}
                    step={1}
                    onChange={setEnergy}
                  />
                </div>
              </div>
            </div>
            <div className="col-md-2">
              <div className="form-group mt-3">
                <label className={styles.albumPickerLabel}>Intimacy</label>
                <div>
                  <Slider
                    value={intimacy}
                    minimum={0}
                    maximum={5}
                    step={1}
                    onChange={setIntimacy}
                  />
                </div>
              </div>
            </div>
            <div className="col-md-2">
              <div className="form-group mt-3">
                <label className={styles.albumPickerLabel}>Warmth</label>
                <div>
                  <Slider
                    value={warmth}
                    minimum={0}
                    maximum={5}
                    step={1}
                    onChange={setWarmth}
                  />
                </div>
              </div>
            </div>
            <div className="col-md-2">
              <div className="form-group mt-3">
                <label className={styles.albumPickerLabel}></label>
                <div>
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
      {pickedAlbums && (
        <table className="table table-hover">
          <thead>
            <tr>
              <th>Match Strength</th>
              <th>Artist</th>
              <th>Album Title</th>
              <th>Playing Time</th>
              <th>Genre</th>
              <th>Released</th>
              <th>Purchased</th>
              <th>Price</th>
              <th>Retailer</th>
            </tr>
          </thead>
          <tbody>
            {(pickedAlbums ?? []).map((pa) => (
              <AlbumPickerAlbumRow
                key={pa.album.id}
                id={pa.album.id}
                match={pa}
                navigate={navigate}
              />
            ))}
          </tbody>
        </table>
      )}
    </>
  );
};

export default AlbumPicker;
