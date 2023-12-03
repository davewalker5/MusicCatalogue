import CurrencyFormatter from "../common/currencyFormatter";

/**
 * Component to render a row containing the details for a single retailer statistics record
 * @param {*} record
 * @returns
 */
const RetailerStatisticsRow = ({ record }) => {
  return (
    <tr>
      <td>{record.name}</td>
      <td>{record.artists}</td>
      <td>{record.albums}</td>
      <td>{record.tracks}</td>
      <td>
        <CurrencyFormatter value={record.spend} renderZeroAsBlank={true} />
      </td>
    </tr>
  );
};

export default RetailerStatisticsRow;
