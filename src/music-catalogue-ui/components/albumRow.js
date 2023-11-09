import pages from "@/helpers/navigation";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrashAlt } from "@fortawesome/free-solid-svg-icons";

/**
 * Component to render a row containing the details of a single album
 * @param {*} param0
 * @returns
 */
const AlbumRow = ({ artist, album, navigate, deleteAlbum }) => {
  return (
    <tr>
      <td onClick={() => navigate(pages.tracks, artist, album)}>
        {artist.name}
      </td>
      <td onClick={() => navigate(pages.tracks, artist, album)}>
        {album.title}
      </td>
      <td onClick={() => navigate(pages.tracks, artist, album)}>
        {album.genre}
      </td>
      <td onClick={() => navigate(pages.tracks, artist, album)}>
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
