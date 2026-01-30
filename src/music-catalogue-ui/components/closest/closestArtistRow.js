import pages from "@/helpers/navigation";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBullseye } from "@fortawesome/free-solid-svg-icons";

/**
 * Component to render a row containing the details for a single artist
 * @param {*} artist
 * @param {*} similarity
 * @param {*} filter
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} vocalPresences
 * @param {*} ensembleTypes
 * @returns
 */
const ClosestArtistRow = ({
  artist,
  similarity,
  filter,
  isWishList,
  navigate,
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
        {similarity}
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

export default ClosestArtistRow;
