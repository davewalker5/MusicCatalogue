import React, { useCallback, useState, useEffect, useMemo } from "react";
import styles from "./sessionList.module.css";
import { apiSearchForSessions } from "@/helpers/api/apiPlaylist";
import PlaylistTypeSelector from "../common/playlistTypeSelector";
import TimesOfDaySelector from "../common/timesOfDaySelector";
import DatePicker from "react-datepicker";
import SessionRow from "./sessionRow";
import useTimesOfDay from "@/hooks/useTimesOfDay";
import usePlaylistTypes from "@/hooks/usePlaylistTypes";
import { getCurrentTimeOfDay, getPlaylistTypeForTimeOfDay } from "../common/timeOfDay";

/**
 * Component to search for and display saved sessions
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const SessionList = ({ navigate, logout }) => {
  const { timesOfDay, setTimesOfDay } = useTimesOfDay(logout);
  const { playlistTypes, setPlaylistTypes } = usePlaylistTypes(logout);

  const currentTimeOfDay = useMemo(() => getCurrentTimeOfDay(timesOfDay), [timesOfDay]);

  const [fromDate, setFromDate] = useState(null);
  const [toDate, setToDate] = useState(null);
  const [playlistType, setPlaylistType] = useState(null);
  const [timeOfDay, setTimeOfDay] = useState(null);
  const [sessions, setSessions] = useState(null);

  // On initially rendering, currentTimeOfDay won't be set because the times of day won't have
  // been retrieved from the API yet. So we can't use currentTimeOfDay to initialise state, above
  useEffect(() => {
    if (currentTimeOfDay) {
      // Set the time of day
      setTimeOfDay(currentTimeOfDay);

      // Identify the default playlist type
      const playlistType = getPlaylistTypeForTimeOfDay(playlistTypes, currentTimeOfDay);
      setPlaylistType(playlistType);
    }
  }, [currentTimeOfDay, playlistTypes]);

  // Callback to retrieve saved sessions
  const loadSessionsCallback = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Make sure the dates span the start of the from date to the end of the to date and get the playlist type and ToD
      const fromDateAndTime = fromDate != null ? new Date(new Date(fromDate).setHours(0,0,0,0)) : null;
      const toDateAndTime = toDate != null ? new Date(new Date(toDate).setHours(23,59,59,999)) : null;
      const playlistTypeId = playlistType != null ? playlistType.id : null;
      const timeOfDayId = timeOfDay != null ? timeOfDay.id : null;

      // Load matching sessions
      const fetchedSessions = await apiSearchForSessions(fromDateAndTime, toDateAndTime, playlistTypeId, timeOfDayId, logout);

      // Generate the artist list for each session and assign the names of the playlist type and time of day
      fetchedSessions?.forEach(s => {
        s.artists = s.sessionAlbums.map(sa => sa.album.artist.name).join(", ");
        s.playlistTypeName = playlistTypes.find(t => t.id === s.type)?.name;
        s.timeOfDayName = timesOfDay.find(t => t.id === s.timeOfDay)?.name;
      });

      // Store the sessions
      setSessions(fetchedSessions);
    },
    [playlistTypes, timesOfDay, fromDate, toDate, playlistType, timeOfDay, logout]
  );

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Saved Sessions</h5>
      </div>
      <div className={styles.sessionListFormContainer}>
        <form className={styles.sessionListForm}>
          <div className="row d-flex justify-content-center">
            <div className="col-auto">
              <div className="form-group mt-3">
                <label className={styles.sessionListLabel}>From</label>
                <div>
                    <DatePicker
                      selected={fromDate}
                      onChange={(date) => setFromDate(date)}
                    />
                </div>
              </div>
            </div>
            <div className="col-auto">
              <div className="form-group mt-3">
                <label className={styles.sessionListLabel}>To</label>
                <div>
                    <DatePicker
                      selected={toDate}
                      onChange={(date) => setToDate(date)}
                    />
                </div>
              </div>
            </div>
            <div className="col-auto">
              <div className="form-group mt-3">
                <label className={styles.sessionListLabel}>Playlist Type</label>
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
                <label className={styles.sessionListLabel}>Time Of Day</label>
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
                <label className={styles.sessionListLabel}></label>
                <div>
                  <button
                    className="btn btn-primary"
                    onClick={(e) => loadSessionsCallback(e)}
                  >
                    Search
                  </button>
                </div>
              </div>
            </div>
          </div>
        </form>
      </div>
      {sessions && (
        <>
          <table className="table table-hover">
            <thead>
              <tr>
                <th>Created</th>
                <th>Type</th>
                <th>Time Of Day</th>
                <th>Artists</th>
                <th>Albums</th>
              </tr>
            </thead>
            <tbody>
              {sessions.map((s) => (
                <SessionRow
                  key={s.id}
                  id={s.id}
                  session={s}
                  navigate={navigate}
                />
              ))}
            </tbody>
          </table>
        </>
      )}
    </>
  );
};

export default SessionList;
