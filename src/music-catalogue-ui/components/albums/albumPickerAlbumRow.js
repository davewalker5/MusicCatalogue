import CurrencyFormatter from "../common/currencyFormatter";
import DateFormatter from "../common/dateFormatter";

/**
 * Component to render a row containing the details of a single album in a
 * genre
 * @param {*} album
 * @param {*} artist
 * @returns
 */
const AlbumPickerAlbumRow = ({ album, artist }) => {
  const purchaseDate = new Date(album.purchased);

  return (
    <tr>
      <td>{artist.name}</td>
      <td>{album.title}</td>
      <td>{album.genre.name}</td>
      {album.released > 0 ? <td>{album.released}</td> : <td />}
      {purchaseDate > 1900 ? (
        <td>
          <DateFormatter value={album.purchased} />
        </td>
      ) : (
        <td />
      )}
      {album.price > 0 ? (
        <td>
          <CurrencyFormatter value={album.price} />
        </td>
      ) : (
        <td />
      )}
      {album.retailer != null ? <td>{album.retailer.name}</td> : <td />}
    </tr>
  );
};

export default AlbumPickerAlbumRow;
