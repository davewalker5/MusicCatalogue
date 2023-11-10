import pages from "@/helpers/navigation";

/**
 * Component to render a row containing the details for a single artist
 * @param {*} artist
 * @param {*} isWishList
 * @param {*} navigate
 * @returns
 */
const ArtistRow = ({ artist, isWishList, navigate }) => {
  return (
    <tr onClick={() => navigate(pages.albums, artist, null, isWishList)}>
      <td>{artist.name}</td>
      <td>{artist.albumCount}</td>
      <td>{artist.trackCount}</td>
    </tr>
  );
};

export default ArtistRow;
