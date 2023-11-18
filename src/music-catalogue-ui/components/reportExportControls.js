import styles from "./reports.module.css";
import { useState } from "react";

const ReportExportControls = ({ isPrimaryButton, exportReport }) => {
  const [fileName, setFileName] = useState("");

  // Export button may be the only button on the page, in which case it can
  // be made primary, or one of a number of buttons, in which case it's likely
  // secondary. Parent component decides.
  const buttonClass = isPrimaryButton ? "btn btn-primary" : "btn btn-secondary";

  return (
    <>
      <div className="col">
        <label className={styles.reportFormLabel}>File Name:</label>
      </div>
      <div className="col">
        <input
          className="form-control mt-1"
          placeholder="File name"
          name="fileName"
          value={fileName}
          onChange={(e) => setFileName(e.target.value)}
        />
      </div>
      <div className="col">
        <button
          className={buttonClass}
          onClick={(e) => exportReport(e, fileName)}
        >
          Export
        </button>
      </div>
    </>
  );
};

export default ReportExportControls;
