import React, { useState, useCallback } from "react";
import styles from "@/components/reports/reports.module.css";
import useMonthlySpend from "@/hooks/useMonthlySpend";
import MonthlySpendReportRow from "./monthlySpendReportRow";
import ReportExportControls from "./reportExportControls";
import { apiRequestMonthlySpendingExport } from "@/helpers/api/apiDataExchange";

/**
 * Component to display the monthly spending report page and its results
 * @param {*} logout
 * @returns
 */
const MonthlySpendReport = ({ logout }) => {
  const { records, setRecords } = useMonthlySpend(logout);
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  /* Callback to export the report */
  const exportReportCallback = useCallback(
    async (e, fileName) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Clear pre-existing errors and messages
      setMessage("");
      setError("");

      // Request an export via the API
      const isOK = await apiRequestMonthlySpendingExport(fileName, logout);

      // If all's well, display a confirmation message. Otherwise, show an error
      if (isOK) {
        setMessage(
          `A background export of the monthly spending report to ${fileName} has been requested`
        );
      } else {
        setError(
          "An error occurred requesting an export of the monthly spending report"
        );
      }
    },
    [logout]
  );

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Monthly Spending Report</h5>
      </div>
      <div className={styles.reportFormContainer}>
        <form className={styles.reportForm}>
          {message != "" ? (
            <div className={styles.reportExportMessage}>{message}</div>
          ) : (
            <></>
          )}
          {error != "" ? (
            <div className={styles.reportExportError}>{error}</div>
          ) : (
            <></>
          )}
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
