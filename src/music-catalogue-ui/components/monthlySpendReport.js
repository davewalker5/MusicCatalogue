import React, { useCallback } from "react";
import styles from "./reports.module.css";
import useMonthlySpend from "@/hooks/useMonthlySpend";
import MonthlySpendReportRow from "./monthlySpendReportRow";
import ReportExportControls from "./reportExportControls";

/**
 * Component to display the monthly spending report page and its results
 * @param {*} logout
 * @returns
 */
const MonthlySpendReport = ({ logout }) => {
  const { records, setRecords } = useMonthlySpend(logout);

  /* Callback to export the report */
  const exportReportCallback = useCallback(async (e) => {
    // Prevent the default action associated with the click event
    e.preventDefault();
  }, []);

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Monthly Spending Report</h5>
      </div>
      <div className={styles.reportFormContainer}>
        <form className={styles.reportForm}>
          <div className="row" align="center">
            <div className="mt-6">
              <div className="d-inline-flex align-items-center">
                {records != null ? (
                  <ReportExportControls
                    isPrimaryButton={true}
                    exportReport={exportReportCallback}
                  />
                ) : (
                  <></>
                )}
              </div>
            </div>
          </div>
        </form>
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
