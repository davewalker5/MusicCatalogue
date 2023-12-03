import pages from "@/helpers/navigation";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPenToSquare } from "@fortawesome/free-solid-svg-icons";
import DeleteArtistActionIcon from "./deleteArtistActionIcon";

/**
 * Component to render a row containing the details for a single artist
 * @param {*} filter
 * @param {*} genre
 * @param {*} artist
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} logout
 * @param {*} setArtists
 * @param {*} setError
 * @returns
 */
const ArtistRow = ({
  filter,
  genre,
  artist,
  isWishList,
  navigate,
  logout,
  setArtists,
  setError,
}) => {
  return (
    <tr>
      <td
        onClick={() =>
          navigate({
            page: pages.albums,
            artist: artist,
            isWishList: isWishList,
          })
        }
      >
        {artist.name}
      </td>
      <td>
        <DeleteArtistActionIcon
          filter={filter}
          genre={genre}
          artist={artist}
          isWishList={isWishList}
          logout={logout}
          setArtists={setArtists}
          setError={setError}
        />
      </td>
      <td>
        <FontAwesomeIcon
          icon={faPenToSquare}
          onClick={() =>
            navigate({
              filter: filter,
              page: pages.artistEditor,
              artist: artist,
              isWishList: isWishList,
            })
          }
        />
      </td>
    </tr>
  );
};

export default ArtistRow;
