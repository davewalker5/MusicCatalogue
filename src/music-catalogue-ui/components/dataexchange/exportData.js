import styles from "./export.module.css";
import { useCallback, useState } from "react";
import {
  apiRequestCatalogueExport,
  apiRequestEquipmentExport,
} from "@/helpers/api/apiDataExchange";
import FormInputField from "../common/formInputField";
import Select from "react-select";

/**
 * Component to prompt for an export file name and request an export of the catalogue
 * or equipment register
 * @param {*} logout
 * @returns
 */
const ExportData = ({ logout }) => {
  const options = [
    { value: "catalogue", label: "Music Catalogue" },
    { value: "equipment", label: "Equipment Register" },
  ];

  const [exportSelection, setExportSelection] = useState(options[0]);
  const [fileName, setFileName] = useState("");
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  // Callback to request an export via the API
  const exportCallback = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Clear pre-existing errors and messages
      setMessage("");
      setError("");

      // Request an export via the API, using the export selection to choose what
      // to export
      let isOK = false;
      if (exportSelection.value == "catalogue") {
        isOK = await apiRequestCatalogueExport(fileName, logout);
      } else {
        isOK = await apiRequestEquipmentExport(fileName, logout);
      }

      // If all's well, display a confirmation message. Otherwise, show an error
      if (isOK) {
        setMessage(
          `A background export of the ${exportSelection.label} to ${fileName} has been requested`
        );
      } else {
        setError(
          `An error occurred requesting an export of the ${exportSelection.label}`
        );
      }
    },
    [exportSelection, fileName, logout]
  );

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Data Export</h5>
      </div>
      <div className={styles.exportFormContainer}>
        <form className={styles.exportForm}>
          <div>
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
          </div>
          <div className="form-group mt-3">
            <label className={styles.equipmentEditorFormLabel}>Export</label>
            <div>
              <Select
                value={exportSelection}
                onChange={setExportSelection}
                options={options}
              />
            </div>
          </div>
          <div>
            <FormInputField
              label="File Name"
              name="fileName"
              value={fileName}
              setValue={setFileName}
            />
          </div>
          <div className="d-grid gap-2 mt-3"></div>
          <div className="d-grid gap-2 mt-3"></div>
          <div className={styles.exportButton}>
            <button
              className="btn btn-primary"
              onClick={(e) => exportCallback(e)}
            >
              Export
            </button>
          </div>
        </form>
      </div>
    </>
  );
};

export default ExportData;
