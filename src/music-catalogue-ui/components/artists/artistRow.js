import pages from "@/helpers/navigation";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBullseye, faMasksTheater, faPenToSquare } from "@fortawesome/free-solid-svg-icons";
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
 * @param {*} vocalPresences
 * @param {*} ensembleTypes
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
  vocalPresences,
  ensembleTypes
}) => {
  const vocalPresence = vocalPresences.find(v => v.id === artist.vocals)?.name;
  const ensembleType = ensembleTypes.find(v => v.id === artist.ensemble)?.name;
  const moods = artist.moods.map(m => m.mood.name).sort().join(", ");

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
      <td
        onClick={() =>
          navigate({
            page: pages.albums,
            artist: artist,
            isWishList: isWishList,
          })
        }
      >
        {artist.energy}
      </td><td
        onClick={() =>
          navigate({
            page: pages.albums,
            artist: artist,
            isWishList: isWishList,
          })
        }
      >
        {artist.intimacy}
      </td><td
        onClick={() =>
          navigate({
            page: pages.albums,
            artist: artist,
            isWishList: isWishList,
          })
        }
      >
        {artist.warmth}
      </td>
      <td
        onClick={() =>
          navigate({
            page: pages.albums,
            artist: artist,
            isWishList: isWishList,
          })
        }
      >
        {vocalPresence}
      </td>
      <td
        onClick={() =>
          navigate({
            page: pages.albums,
            artist: artist,
            isWishList: isWishList,
          })
        }
      >
        {ensembleType}
      </td><td
        onClick={() =>
          navigate({
            page: pages.albums,
            artist: artist,
            isWishList: isWishList,
          })
        }
      >
        {moods}
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
      <td>
        <FontAwesomeIcon
          icon={faMasksTheater}
          onClick={() =>
            navigate({
              filter: filter,
              page: pages.artistMoodEditor,
              artist: artist,
              isWishList: isWishList,
            })
          }
        />
      </td>
      <td>
        <FontAwesomeIcon
          icon={faBullseye}
          onClick={() =>
            navigate({
              filter: filter,
              page: pages.closestArtists,
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
