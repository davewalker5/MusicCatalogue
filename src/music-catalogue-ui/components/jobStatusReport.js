import React, { useCallback, useState } from "react";
import styles from "./jobStatusReport.module.css";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { apiJobStatusReport } from "@/helpers/api";
import JobStatusRow from "./jobStatusRow";

const JobStatusReport = ({ logout }) => {
  const [startDate, setStartDate] = useState(new Date());
  const [endDate, setEndDate] = useState(new Date());
  const [records, setRecords] = useState(null);

  // Callback to request the job status report from the API
  const getReportCallback = useCallback(async () => {
    const fetchedRecords = await apiJobStatusReport(startDate, endDate, logout);
    setRecords(fetchedRecords);
  }, [startDate, endDate, logout]);

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Job Status Report</h5>
      </div>
      <div className={styles.reportFormContainer}>
        <div className={styles.reportForm}>
          <div className="row" align="center">
            <div className="mt-3">
              <div className="d-inline-flex align-items-center">
                <label className={styles.reportFormLabel}>From</label>
                <DatePicker
                  selected={startDate}
                  onChange={(date) => setStartDate(date)}
                />
              </div>
              <div className="d-inline-flex align-items-center">
                <label className={styles.reportFormLabel}>To</label>
                <DatePicker
                  selected={endDate}
                  onChange={(date) => setEndDate(date)}
                />
              </div>
              <button
                className="btn btn-primary"
                onClick={() => getReportCallback()}
              >
                Search
              </button>
            </div>
          </div>
        </div>
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Job Name</th>
            <th>Parameters</th>
            <th>Started</th>
            <th>Completed</th>
            <th>Errors</th>
          </tr>
        </thead>
        {records != null && (
          <tbody>
            {records.map((r) => (
              <JobStatusRow key={r.id} record={r} />
            ))}
          </tbody>
        )}
      </table>
    </>
  );
};

export default JobStatusReport;
