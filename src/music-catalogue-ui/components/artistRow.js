import pages from "@/helpers/navigation";

/**
 * Component to render a row containing the details for a single artist
 * @param {*} param0
 * @returns
 */
const ArtistRow = ({ artist, navigate }) => {
  return (
    <tr onClick={() => navigate(pages.albums, artist, null)}>
      <td>{artist.name}</td>
      <td>{artist.albumCount}</td>
      <td>{artist.trackCount}</td>
    </tr>
  );
};

export default ArtistRow;
