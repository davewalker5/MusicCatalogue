import styles from "./exportCatalogue.module.css";
import { useCallback, useState } from "react";
import { apiRequestExport } from "@/helpers/apiDataExchange";

/**
 * Component to prompt for an export file name and request an export of the catalogue
 * @param {*} logout
 * @returns
 */
const ExportCatalogue = ({ logout }) => {
  const [fileName, setFileName] = useState("");
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  // Callback to request an export via the API
  const exportCallback = useCallback(async () => {
    // Clear pre-existing errors and messages
    setMessage("");
    setError("");

    // Request an export via the API
    const isOK = await apiRequestExport(fileName, logout);

    // If all's well, display a confirmation message. Otherwise, show an error
    if (isOK) {
      setMessage(
        `A background export of the music catalogue to ${fileName} has been requested`
      );
    } else {
      setError("An error occurred requesting an export of the music catalogue");
    }
  }, [fileName, logout]);

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Export Catalogue</h5>
      </div>
      <div className={styles.exportFormContainer}>
        <div className={styles.exportForm}>
          {message != "" ? (
            <div className={styles.exportMessage}>{message}</div>
          ) : (
            <></>
          )}
          {error != "" ? (
            <div className={styles.exportError}>{error}</div>
          ) : (
            <></>
          )}
          <div></div>
          <div>
            <div className="form-group mt-3">
              <label className={styles.exportFormLabel}>File Name</label>
              <input
                className="form-control mt-1"
                placeholder="File name"
                name="fileName"
                value={fileName}
                onChange={(e) => setFileName(e.target.value)}
              />
            </div>
            <br />
            <div className={styles.exportButton}>
              <button
                className="btn btn-primary"
                onClick={() => exportCallback()}
              >
                Export
              </button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default ExportCatalogue;
