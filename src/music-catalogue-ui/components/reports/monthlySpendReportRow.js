import CurrencyFormatter from "../common/currencyFormatter";

/**
 * Component to render a row containing the details for a single monthly spending record
 * @param {*} record
 * @returns
 */
const MonthlySpendReportRow = ({ record }) => {
  return (
    <tr>
      <td>{record.year}</td>
      <td>{record.month}</td>
      <td>
        <CurrencyFormatter value={record.spend} renderZeroAsBlank={true} />
      </td>
    </tr>
  );
};

export default MonthlySpendReportRow;
