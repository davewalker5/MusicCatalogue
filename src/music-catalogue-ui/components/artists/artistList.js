import pages from "@/helpers/navigation";
import styles from "./artistList.module.css";
import useArtists from "@/hooks/useArtists";
import ArtistRow from "./artistRow";
import ArtistFilterBar from "./artistFilterBar";
import { useState } from "react";

/**
 * Component to render a table listing all the artists in the catalogue
 * @param {*} filter
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const ArtistList = ({ filter, genre, isWishList, navigate, logout }) => {
  const { artists, setArtists } = useArtists(filter, genre, isWishList, logout);
  const [error, setError] = useState("");

  // Callback to pass to child components to set the artist list
  const setFilterCallback = (updatedFilter) => {
    navigate({
      page: pages.artists,
      filter: updatedFilter,
      genre: genre,
      isWishList: isWishList,
    });
  };

  // Set the page title to reflect whether we're viewing the wish list
  let title = isWishList ? "Wish List Artists" : "Artists";
  if (genre != null) {
    title = `${title} - ${genre.name}`;
  }

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{title}</h5>
      </div>
      <div className="row">
        {error != "" ? (
          <div className={styles.artistListError}>{error}</div>
        ) : (
          <></>
        )}
      </div>
      <div className="row mb-2 pageTitle">
        <div align="center">
          <ArtistFilterBar setFilter={setFilterCallback} />
        </div>
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Name</th>
          </tr>
        </thead>
        {artists != null && (
          <tbody>
            {artists.map((a) => (
              <ArtistRow
                key={a.id}
                filter={filter}
                genre={genre}
                artist={a}
                isWishList={isWishList}
                navigate={navigate}
                logout={logout}
                setArtists={setArtists}
                setError={setError}
              />
            ))}
          </tbody>
        )}
      </table>
      <div className={styles.artistListAddButton}>
        <button
          className="btn btn-primary"
          onClick={() =>
            navigate({
              filter: filter,
              page: pages.artistEditor,
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

export default ArtistList;
