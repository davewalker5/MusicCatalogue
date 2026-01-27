import styles from "./manufacturerEditor.module.css";
import pages from "@/helpers/navigation";
import FormInputField from "../common/formInputField";
import { useState, useCallback } from "react";
import {
  apiCreateManufacturer,
  apiUpdateManufacturer,
} from "@/helpers/api/apiManufacturers";

/**
 * Component to render the manufacturer editor
 * @param {*} manufacturer
 * @param {*} navigate
 * @param {*} logout
 */
const EquipmentTypeEditor = ({ manufacturer, navigate, logout }) => {
  // Set up state
  const initialName = manufacturer != null ? manufacturer.name : null;
  const [name, setName] = useState(initialName);
  const [error, setError] = useState("");

  const saveManufacturer = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Clear pre-existing errors
      setError("");

      try {
        // Either add or update the manufacturer, depending on whether there's an
        // existing manufacturer or not
        let updatedEquipmentType = null;
        if (manufacturer == null) {
          // Create the manufacturer
          updatedEquipmentType = await apiCreateManufacturer(name, logout);
        } else {
          // Update the existing manufacturer
          updatedEquipmentType = await apiUpdateManufacturer(
            manufacturer.id,
            name,
            logout
          );
        }

        // Go back to the manufacturer list, which should reflect the updated details
        navigate({
          page: pages.manufacturers,
        });
      } catch (ex) {
        setError(
          `Error saving the updated manufacturer details: ${ex.message}`
        );
      }
    },
    [manufacturer, logout, name, navigate]
  );

  // Set the page title
  const pageTitle =
    manufacturer != null ? manufacturer.name : "New Manufacturer";

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{pageTitle}</h5>
      </div>
      <div className={styles.manufacturerEditorFormContainer}>
        <form className={styles.manufacturerEditorForm}>
          <div className="row">
            {error != "" ? (
              <div className={styles.manufacturerEditorError}>{error}</div>
            ) : (
              <></>
            )}
          </div>
          <div className="row align-items-start">
            <FormInputField
              label="Name"
              name="name"
              value={name}
              setValue={setName}
            />
          </div>
          <div className="d-grid gap-2 mt-3"></div>
          <div className="d-grid gap-2 mt-3"></div>
          <div className={styles.manufacturerEditorButton}>
            <button
              className="btn btn-primary"
              onClick={(e) => saveManufacturer(e)}
            >
              Save
            </button>
          </div>
          <div className={styles.manufacturerEditorButton}>
            <button
              className="btn btn-primary"
              onClick={() =>
                navigate({
                  page: pages.manufacturers,
                })
              }
            >
              Cancel
            </button>
          </div>
        </form>
      </div>
    </>
  );
};

export default EquipmentTypeEditor;
