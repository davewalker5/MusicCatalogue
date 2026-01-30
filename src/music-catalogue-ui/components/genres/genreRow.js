import DeleteGenreActionIcon from "./deleteGenreActionIcon";
import pages from "@/helpers/navigation";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPenToSquare } from "@fortawesome/free-solid-svg-icons";
import { Tooltip } from "react-tooltip";

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
        <>
          <FontAwesomeIcon
            icon={faPenToSquare}
            data-tooltip-id="edit-tooltip"
            data-tooltip-content="Edit genre"
            onClick={() =>
              navigate({
                page: pages.genreEditor,
                genre: genre,
              })
            }
          />

          <Tooltip id="edit-tooltip" />
        </>
      </td>
    </tr>
  );
};

export default GenreRow;
