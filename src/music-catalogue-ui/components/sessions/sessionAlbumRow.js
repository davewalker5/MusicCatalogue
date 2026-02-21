import CurrencyFormatter from "../common/currencyFormatter";
import DateFormatter from "../common/dateFormatter";

/**
 * Component to render a row containing the details of a single album that's part of a saved session
 * @param {*} sessionAlbum
 * @returns
 */
const SessionAlbumRow = ({ sessionAlbum }) => {
  // Get the artist name
  const album = sessionAlbum.album;
  const artist = album["artist"];
  const artistName = artist != null ? artist["name"] : "";

  // Get the retailer name
  const retailer = album["retailer"];
  const retailerName = retailer != null ? retailer["name"] : "";

  // Get the genre
  const genre = album["genre"];
  const genreName = genre != null ? genre["name"] : "";

  return (
    <tr>
      <td>{sessionAlbum.position}</td>
      <td>{artistName}</td>
      <td>{album.title}</td>
      <td>{album.formattedPlayingTime}</td>
      <td>{album.tracks?.length}</td>
      <td>{genreName}</td>
      <td>{album.released}</td>
      <td>
        <DateFormatter value={album.purchased} />
      </td>
      <td>
        <CurrencyFormatter value={album.price} renderZeroAsBlank={true} />
      </td>
      <td>{retailerName}</td>
    </tr>
  );
};

export default SessionAlbumRow;
