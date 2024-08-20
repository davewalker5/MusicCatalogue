import CurrencyFormatter from "../common/currencyFormatter";
import DateFormatter from "../common/dateFormatter";

/**
 * Component to render a row containing the details of a single album in a
 * genre
 * @param {*} record
 * @returns
 */
const GenreAlbumRow = ({ record }) => {
  const purchaseDate = new Date(record.purchased);
  console.log(purchaseDate.getFullYear());
  return (
    <tr>
      <td>{record.artist}</td>
      <td>{record.title}</td>
      <td>{record.genre}</td>
      {record.released > 0 ? <td>{record.released}</td> : <td />}
      {purchaseDate > 1900 ? (
        <td>
          <DateFormatter value={record.purchased} />
        </td>
      ) : (
        <td />
      )}
      {record.price > 0 ? (
        <td>
          <CurrencyFormatter value={record.price} />
        </td>
      ) : (
        <td />
      )}
      <td>{record.retailer}</td>
    </tr>
  );
};

export default GenreAlbumRow;
