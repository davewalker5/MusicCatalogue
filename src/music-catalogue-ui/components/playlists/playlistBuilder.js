import React, { useCallback, useState } from "react";
import styles from "./playlistBuilder.module.css";
import { apiGeneratePlaylist } from "@/helpers/api/apiPlaylist";
import Slider from "../common/slider";
import PlaylistTypeSelector from "./playlistTypeSelector";
import TimesOfDaySelector from "./timesOfDaySelector";
import PlaylistAlbumRow from "./playlistAlbumRow";

/**
 * Component to pick a random album, optionally for a specified playlistType
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const PlaylistBuilder = ({ navigate, logout }) => {
  const [playlistType, setPlaylistType] = useState(null);
  const [timeOfDay, setTimeOfDay] = useState(null);
  const [numberOfEntries, setNumberOfEntries] = useState(3);
  const [playlistAlbums, setPlaylistAlbums] = useState(null);

  // Callback to request a playlist from the API
  const generatePlaylistCallback = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Request a playlist built using the specified criteria
      const playlistTypeId = playlistType != null ? playlistType.id : null;
      const timeOfDayId = timeOfDay != null ? timeOfDay.id : null;
      const fetchedAlbums = await apiGeneratePlaylist(playlistTypeId, timeOfDayId, numberOfEntries, logout);
      setPlaylistAlbums(fetchedAlbums);
    },
    [playlistType, timeOfDay, numberOfEntries, logout]
  );

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Playlist Builder</h5>
      </div>
      <div className={styles.playlistBuilderFormContainer}>
        <form className={styles.playlistBuilderForm}>
          <div className="row d-flex justify-content-center">
            <div className="col-md-2">
              <div className="form-group mt-3">
                <label className={styles.playlistBuilderLabel}>Playlist Type</label>
                <div>
                  <PlaylistTypeSelector
                        initialPlaylistType={playlistType}
                        playlistTypeChangedCallback={setPlaylistType}
                        logout={logout}
                      />
                </div>
              </div>
            </div>
            <div className="col-md-2">
              <div className="form-group mt-3">
                <label className={styles.playlistBuilderLabel}>Time Of Day</label>
                <div>
                  <TimesOfDaySelector
                        initialTimeOfDay={timeOfDay}
                        timeOfDayChangedCallback={setTimeOfDay}
                        logout={logout}
                      />
                </div>
              </div>
            </div>
            <div className="col-md-2">
              <div className="form-group mt-3">
                <label className={styles.playlistBuilderLabel}>Number Of Entries</label>
                <div>
                  <Slider
                        initialValue={numberOfEntries}
                        minimum={3}
                        maximum={10}
                        step={1}
                        sliderChangedCallback={setNumberOfEntries}
                      />
                </div>
              </div>
            </div>
            <div className="col-md-2">
              <div className="form-group mt-3">
                <label className={styles.playlistBuilderLabel}></label>
                <div>
                  <button
                    className="btn btn-primary"
                    onClick={(e) => generatePlaylistCallback(e)}
                  >
                    Build
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
            <th>Album Title</th>
            <th>Genre</th>
            <th>Released</th>
            <th>Purchased</th>
            <th>Price</th>
            <th>Retailer</th>
          </tr>
        </thead>
        <tbody>
          {(playlistAlbums ?? []).map((pa) => (
            <PlaylistAlbumRow
              key={pa.id}
              id={pa.id}
              artist={pa.artist}
              album={pa}
              navigate={navigate}
            />
          ))}
        </tbody>
      </table>
    </>
  );
};

export default PlaylistBuilder;
