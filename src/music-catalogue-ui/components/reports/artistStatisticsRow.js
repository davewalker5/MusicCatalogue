import CurrencyFormatter from "../common/currencyFormatter";

/**
 * Component to render a row containing the details for a single artist statistics record
 * @param {*} record
 * @returns
 */
const ArtistStatisticsRow = ({ record }) => {
  return (
    <tr>
      <td>{record.name}</td>
      <td>{record.albums}</td>
      <td>{record.tracks}</td>
      <td>
        <CurrencyFormatter value={record.spend} renderZeroAsBlank={true} />
      </td>
    </tr>
  );
};

export default ArtistStatisticsRow;
