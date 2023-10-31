import React, { useCallback, useState } from "react";
import styles from "./jobStatusReport.module.css";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

const JobStatusReport = ({ navigate, logout }) => {
  const [startDate, setStartDate] = useState(new Date());
  const [endDate, setEndDate] = useState(new Date());

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Job Status Report</h5>
      </div>
      <div className={styles.reportFormContainer}>
        <div className={styles.reportForm}>
          <div class="row" align="center">
            <div class="mt-3">
              <div class="d-inline-flex align-items-center">
                <label className={styles.reportFormLabel}>From</label>
                <DatePicker
                  selected={startDate}
                  onChange={(date) => setStartDate(date)}
                />
              </div>
              <div class="d-inline-flex align-items-center">
                <label className={styles.reportFormLabel}>To</label>
                <DatePicker
                  selected={endDate}
                  onChange={(date) => setEndDate(date)}
                />
              </div>
              <button className="btn btn-primary">Search</button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default JobStatusReport;
