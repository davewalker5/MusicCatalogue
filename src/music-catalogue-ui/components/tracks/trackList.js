import { useState, useCallback } from "react";
import useTracks from "@/hooks/useTracks";
import TrackRow from "./trackRow";
import pages from "@/helpers/navigation";
import styles from "./trackList.module.css";

/**
 * Component to render the list of tracks for the specified album
 * @param {*} artist
 * @param {*} album
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const TrackList = ({ artist, album, isWishList, navigate, logout }) => {
  const { tracks, setTracks } = useTracks(album.id, logout);
  const [error, setError] = useState("");

  // Set the page title to reflect whether we're viewing the wish list
  const title = isWishList
    ? `${artist.name} - ${album.title} (Wish List)`
    : `${artist.name} - ${album.title}`;

  // Set the back button text to indicate whether we're viewing the wish list
  const backButtonText = isWishList
    ? `Back to Wish List for ${artist.name}`
    : `Back to Albums by ${artist.name}`;

  // Backwards navigation callback
  const navigateBack = useCallback(() => {
    navigate({ page: pages.albums, artist: artist, isWishList: isWishList });
  }, [navigate, artist, isWishList]);

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{title}</h5>
      </div>
      <div className="row">
        {error != "" ? (
          <div className={styles.trackListError}>{error}</div>
        ) : (
          <></>
        )}
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Album Title</th>
            <th>Artist</th>
            <th>No.</th>
            <th>Track</th>
            <th>Duration</th>
            <th />
          </tr>
        </thead>
        <tbody>
          {tracks.map((t) => (
            <TrackRow
              key={t.id}
              artist={artist}
              album={album}
              track={t}
              isWishList={isWishList}
              navigate={navigate}
              logout={logout}
              setTracks={setTracks}
              setError={setError}
            />
          ))}
        </tbody>
      </table>
      <button className="btn btn-primary" onClick={() => navigateBack()}>
        &lt; {backButtonText}
      </button>
      <div className={styles.trackListAddButton}>
        <button
          className="btn btn-primary"
          onClick={() =>
            navigate({
              page: pages.trackEditor,
              artist: artist,
              album: album,
            })
          }
        >
          Add
        </button>
      </div>
    </>
  );
};

export default TrackList;
