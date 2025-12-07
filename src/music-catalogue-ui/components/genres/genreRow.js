import DeleteGenreActionIcon from "./deleteGenreActionIcon";
import pages from "@/helpers/navigation";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPenToSquare } from "@fortawesome/free-solid-svg-icons";

/**
 * Component to render a row containing the details for a single genre
 * @param {*} genre
 * @param {*} navigate
 * @param {*} logout
 * @param {*} setGenres
 * @param {*} setError
 * @returns
 */
const GenreRow = ({ genre, navigate, logout, setGenres, setError }) => {
  return (
    <tr>
      <td>{genre.name}</td>
      <td>
        <DeleteGenreActionIcon
          genre={genre}
          logout={logout}
          setGenres={setGenres}
          setError={setError}
        />
      </td>
      <td>
        <FontAwesomeIcon
          icon={faPenToSquare}
          onClick={() =>
            navigate({
              page: pages.genreEditor,
              genre: genre,
            })
          }
        />
      </td>
    </tr>
  );
};

export default GenreRow;
