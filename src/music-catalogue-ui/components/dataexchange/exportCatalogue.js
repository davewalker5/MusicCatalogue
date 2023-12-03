import styles from "./export.module.css";
import { useCallback, useState } from "react";
import { apiRequestCatalogueExport } from "@/helpers/api/apiDataExchange";
import FormInputField from "../common/formInputField";

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
    const isOK = await apiRequestCatalogueExport(fileName, logout);

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
          <div>
            <FormInputField
              label="File Name"
              name="fileName"
              value={fileName}
              setValue={setFileName}
            />
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
