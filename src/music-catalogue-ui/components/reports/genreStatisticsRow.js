import CurrencyFormatter from "../common/currencyFormatter";

/**
 * Component to render a row containing the details for a single genre statistics record
 * @param {*} record
 * @returns
 */
const GenreStatisticsRow = ({ record }) => {
  return (
    <tr>
      <td>{record.genre}</td>
      <td>{record.artists}</td>
      <td>{record.albums}</td>
      <td>{record.tracks}</td>
      <td>
        <CurrencyFormatter value={record.spend} renderZeroAsBlank={true} />
      </td>
    </tr>
  );
};

export default GenreStatisticsRow;
