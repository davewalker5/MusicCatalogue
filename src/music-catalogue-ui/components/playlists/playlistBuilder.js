import React, { useCallback, useState } from "react";
import styles from "./playlistBuilder.module.css";
import { apiGeneratePlaylist, apiSavePlaylist } from "@/helpers/api/apiPlaylist";
import Slider from "../common/slider";
import PlaylistTypeSelector from "./playlistTypeSelector";
import TimesOfDaySelector from "./timesOfDaySelector";
import PlaylistAlbumRow from "./playlistAlbumRow";
import GenreMultiSelectDropdownList from "../genres/genreMultiSelectDropdownList";
import ArtistSelector from "../artists/artistSelector";

/**
 * Component to pick a random album, optionally for a specified playlistType
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const PlaylistBuilder = ({ navigate, logout }) => {
  const [message, setMessage] = useState("");
  const [playlistType, setPlaylistType] = useState(null);
  const [timeOfDay, setTimeOfDay] = useState(null);
  const [numberOfEntries, setNumberOfEntries] = useState(3);
  const [playlist, setPlaylist] = useState(null);
  const [currentArtist, setCurrentArtist] = useState([]);
  const [includedGenres, setIncludedGenres] = useState([]);
  const [excludedGenres, setExcludedGenres] = useState([]);

  // Callback to request a playlist from the API
  const generatePlaylistCallback = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Clear pre-existing messages
      setMessage("");

      // Get the playlist builder criteria
      const playlistTypeId = playlistType != null ? playlistType.id : null;
      const timeOfDayId = timeOfDay != null ? timeOfDay.id : null;
      const currentArtistId = currentArtist != null ? currentArtist.id : null;

      // Extract the IDs for the included and excluded genres
      const includedGenreIds = includedGenres.map(item => item.id);
      const excludedGenreIds = excludedGenres.map(item => item.id);

      // Make sure they're all specified
      if ((playlistTypeId != null) && (timeOfDayId != null) && (numberOfEntries > 0)) {
        // Request a playlist built using the specified criteria
        const fetchedPlaylist = await apiGeneratePlaylist(
          playlistTypeId,
          timeOfDayId,
          numberOfEntries,
          currentArtistId,
          includedGenreIds,
          excludedGenreIds,
          logout);
        setPlaylist(fetchedPlaylist);
      }

    },
    [playlistType, timeOfDay, numberOfEntries, currentArtist, includedGenres, excludedGenres, logout]
  );

  // Callback to save a playlist as a saved session
  const savePlaylistCallback = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Clear pre-existing messages
      setMessage("");

      // Extract a list of album IDs from the current playlist and get the playlist type and ToD
      const albumIds = playlist.albums.map(item => item.id)
      const playlistTypeId = playlistType != null ? playlistType.id : null;
      const timeOfDayId = timeOfDay != null ? timeOfDay.id : null;

      // Make sure they're all specified
      const savedSession = await apiSavePlaylist(playlistTypeId, timeOfDayId, albumIds, logout);
      setMessage(`Playlist has been saved as session number ${savedSession.id}`);
    },
    [playlistType, timeOfDay, playlist, logout]
  );

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Playlist Builder</h5>
      </div>
      <div className={styles.playlistBuilderFormContainer}>
        <form className={styles.playlistBuilderForm}>
          <div>
            {message != "" ? (
              <div className={styles.savedMessage}>{message}</div>
            ) : (
              <></>
            )}
          </div>
          <div className="row d-flex justify-content-center">
            <div className="col-md-2">
              <div className="form-group mt-3">
                <label className={styles.playlistBuilderLabel}>Current Artist</label>
                <div>
                  <ArtistSelector
                    initialArtist={currentArtist}
                    artistChangedCallback={setCurrentArtist}
                    logout={logout}
                  />
                </div>
              </div>
            </div>
            <div className="col-auto">
              <div className="form-group mt-3">
                <label className={styles.playlistBuilderLabel}>
                  Include Genres{includedGenres.length > 0 && ` (${includedGenres.length})`}
                </label>
                <div>
                  <GenreMultiSelectDropdownList
                    label="Genres"
                    onSelectionChanged={setIncludedGenres}
                    logout={logout}
                  />
                </div>
              </div>
            </div>
            <div className="col-auto">
              <div className="form-group mt-3">
                <label className={styles.playlistBuilderLabel}>
                  Exclude Genres{excludedGenres.length > 0 && ` (${excludedGenres.length})`}
                </label>
                <div>
                  <GenreMultiSelectDropdownList
                    label="Genres"
                    onSelectionChanged={setExcludedGenres}
                    logout={logout}
                  />
                </div>
              </div>
            </div>
            <div className="col-auto">
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
            <div className="col-auto">
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
            <div className="col-auto">
              <div className="form-group mt-3">
                <label className={styles.playlistBuilderLabel}>Entries</label>
                <div>
                  <Slider
                    value={numberOfEntries}
                    minimum={3}
                    maximum={10}
                    step={1}
                    onChange={setNumberOfEntries}
                  />
                </div>
              </div>
            </div>
            <div className="col-auto">
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
      {playlist && playlist.albums && (
        <>
          <table className="table table-hover">
            <thead>
              <tr>
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
              {(playlist.albums ?? []).map((pa) => (
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
          <div className="row">
            <div className="col d-flex justify-content-center">
            <label className={styles.playlistBuilderLabel}>Total Playing Time:</label>
            <label>{playlist.formattedPlayingTime}</label>
            </div>
          </div>
          <div className={styles.playlistSaveButton}>
            <button
              className="btn btn-primary"
              onClick={(e) => savePlaylistCallback(e)}
            >
              Save
            </button>
          </div>
        </>
      )}
    </>
  );
};

export default PlaylistBuilder;
