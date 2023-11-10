import pages from "@/helpers/navigation";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrashAlt } from "@fortawesome/free-solid-svg-icons";

/**
 * Component to render a row containing the details of a single album
 * @param {*} artist
 * @param {*} album
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} deleteAlbum
 * @returns
 */
const AlbumRow = ({ artist, album, isWishList, navigate, deleteAlbum }) => {
  return (
    <tr>
      <td onClick={() => navigate(pages.tracks, artist, album, isWishList)}>
        {artist.name}
      </td>
      <td onClick={() => navigate(pages.tracks, artist, album, isWishList)}>
        {album.title}
      </td>
      <td onClick={() => navigate(pages.tracks, artist, album, isWishList)}>
        {album.genre}
      </td>
      <td onClick={() => navigate(pages.tracks, artist, album, isWishList)}>
        {album.released}
      </td>
      <td>
        <FontAwesomeIcon
          icon={faTrashAlt}
          onClick={(e) => deleteAlbum(e, album)}
        />
      </td>
    </tr>
  );
};

export default AlbumRow;
