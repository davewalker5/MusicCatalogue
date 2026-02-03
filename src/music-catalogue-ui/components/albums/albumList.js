import styles from "./albumList.module.css";
import pages from "@/helpers/navigation";
import useAlbums from "@/hooks/useAlbums";
import AlbumRow from "./albumRow";
import { useState } from "react";

/**
 * Component to render the table of all albums by the specified artist
 * @param {*} artist
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const AlbumList = ({ artist, isWishList, navigate, logout }) => {
  const { albums, setAlbums } = useAlbums(artist.id, isWishList, logout);
  const [error, setError] = useState("");

  // Set the page title to reflect whether we're viewing the wish list
  const title = isWishList
    ? `Wish List for ${artist.name}`
    : `Albums by ${artist.name}`;

  // Callback to pass to child components to set the album list
  const setAlbumsCallback = (albums) => {
    setAlbums(albums);
  };

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{title}</h5>
      </div>
      <div className="row">
        {error != "" ? (
          <div className={styles.albumListError}>{error}</div>
        ) : (
          <></>
        )}
      </div>
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
            <th />
            <th />
            <th />
          </tr>
        </thead>
        <tbody>
          {(albums ?? []).map((a) => (
            <AlbumRow
              key={a.id}
              id={a.id}
              artist={artist}
              album={a}
              isWishList={isWishList}
              navigate={navigate}
              logout={logout}
              setAlbums={setAlbumsCallback}
              setError={setError}
            />
          ))}
        </tbody>
      </table>
      <div className={styles.albumListAddButton}>
        <button
          className="btn btn-primary"
          onClick={() =>
            navigate({
              page: pages.albumEditor,
              artist: artist,
              isWishList: isWishList,
            })
          }
        >
          Add
        </button>
      </div>
    </>
  );
};

export default AlbumList;
