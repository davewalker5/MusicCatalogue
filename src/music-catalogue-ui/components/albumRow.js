import pages from "@/helpers/navigation";

/**
 * Component to render a row containing the details of a single album
 * @param {*} param0
 * @returns
 */
const AlbumRow = ({ artist, album, navigate }) => {
  return (
    <tr onClick={() => navigate(pages.tracks, artist, album, navigate)}>
      <td>{artist.name}</td>
      <td>{album.title}</td>
      <td>{album.genre}</td>
      <td>{album.released}</td>
    </tr>
  );
};

export default AlbumRow;
