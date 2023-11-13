import CurrencyFormatter from "./currencyFormatter";

/**
 * Component to render a row containing the details for a single genre statistics record
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
