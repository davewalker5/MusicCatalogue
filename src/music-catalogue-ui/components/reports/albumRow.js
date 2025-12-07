import CurrencyFormatter from "../common/currencyFormatter";
import DateFormatter from "../common/dateFormatter";

/**
 * Component to render a row containing the details of a single album
 * @param {*} record
 * @returns
 */
const AlbumRow = ({ record }) => {
  const purchaseDate = new Date(record.purchased);

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

export default AlbumRow;
