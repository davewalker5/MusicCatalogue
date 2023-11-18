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
    <tr
      onClick={() =>
        navigate({ page: pages.albums, artist: artist, isWishList: isWishList })
      }
    >
      <td>{artist.name}</td>
    </tr>
  );
};

export default ArtistRow;
