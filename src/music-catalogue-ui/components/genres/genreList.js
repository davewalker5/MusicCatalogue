import styles from "./genreList.module.css";
import pages from "@/helpers/navigation";
import useGenres from "@/hooks/useGenres";
import GenreRow from "./genreRow";
import { useState } from "react";

/**
 * Component to render a table listing all the genres in the catalogue
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const GenreList = ({ navigate, logout }) => {
  const { genres, setGenres } = useGenres(false, logout);
  const [error, setError] = useState("");

  // Callback to pass to child components to set the list of genres
  const setGenresCallback = (genres) => {
    setGenres(genres);
  };

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Genres</h5>
      </div>
      <div className="row">
        {error != "" ? (
          <div className={styles.genreListError}>{error}</div>
        ) : (
          <></>
        )}
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Name</th>
          </tr>
        </thead>
        {genres != null && (
          <tbody>
            {genres.map((g) => (
              <GenreRow
                key={g.id}
                genre={g}
                navigate={navigate}
                logout={logout}
                setGenres={setGenresCallback}
                setError={setError}
              />
            ))}
          </tbody>
        )}
      </table>
      <div className={styles.genreListAddButton}>
        <button
          className="btn btn-primary"
          onClick={() =>
            navigate({
              page: pages.genreEditor,
              genre: null,
            })
          }
        >
          Add
        </button>
      </div>
    </>
  );
};

export default GenreList;
