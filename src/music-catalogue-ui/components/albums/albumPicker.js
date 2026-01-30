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
const AlbumPicker = ({ logout }) => {
  const [genre, setGenre] = useState(null);
  const [mood, setMood] = useState(null);
  const [energy, setEnergy] = useState(3);
  const [intimacy, setIntimacy] = useState(3);
  const [warmth, setWarmth] = useState(3);
  const [albums, setAlbums] = useState(null);

  // Callback to request a random album from the API
  const pickAlbumCallback = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Request a set of random albums matching the specified criteria
      const genreId = genre != null ? genre.id : null;
      const moodId = mood != null ? mood.id : null;
      const fetchedAlbums = await apiFetchRandomAlbum(genreId, moodId, energy, intimacy, warmth, logout);
      setAlbums(fetchedAlbums);
    },
    [genre, energy, intimacy, warmth, logout]
  );

      console.log(albums);
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
                        initialValue={energy}
                        minimum={0}
                        maximum={5}
                        step={1}
                        sliderChangedCallback={setEnergy}
                      />
                </div>
              </div>
            </div>
            <div className="col-md-2">
              <div className="form-group mt-3">
                <label className={styles.albumPickerLabel}>Intimacy</label>
                <div>
                  <Slider
                        initialValue={intimacy}
                        minimum={0}
                        maximum={5}
                        step={1}
                        sliderChangedCallback={setIntimacy}
                      />
                </div>
              </div>
            </div>
            <div className="col-md-2">
              <div className="form-group mt-3">
                <label className={styles.albumPickerLabel}>Warmth</label>
                <div>
                  <Slider
                        initialValue={warmth}
                        minimum={0}
                        maximum={5}
                        step={1}
                        sliderChangedCallback={setWarmth}
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
        <tbody>
          {(albums ?? []).map((a) => (
            <AlbumPickerAlbumRow
              key={a.id}
              id={a.id}
              album={a}
              artist={a.artist}
            />
          ))}
        </tbody>
      </table>
    </>
  );
};

export default AlbumPicker;
