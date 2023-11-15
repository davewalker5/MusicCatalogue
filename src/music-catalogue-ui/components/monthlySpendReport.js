import useMonthlySpend from "@/hooks/useMonthlySpend";
import MonthlySpendReportRow from "./monthlySpendReportRow";

/**
 * Component to display the monthly spending report page and its results
 * @param {*} logout
 * @returns
 */
const MonthlySpendReport = ({ logout }) => {
  const { records, setRecords } = useMonthlySpend(logout);

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Monthly Spending Report</h5>
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Year</th>
            <th>Month</th>
            <th>Total Spend</th>
          </tr>
        </thead>
        {records != null && (
          <tbody>
            {records.map((r) => (
              <MonthlySpendReportRow key={`${r.year}.${r.month}`} record={r} />
            ))}
          </tbody>
        )}
      </table>
    </>
  );
};

export default MonthlySpendReport;
