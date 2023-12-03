import styles from "./equipmentTypeEditor.module.css";
import pages from "@/helpers/navigation";
import FormInputField from "../common/formInputField";
import { useState, useCallback } from "react";
import {
  apiCreateEquipmentType,
  apiUpdateEquipmentType,
} from "@/helpers/api/apiEquipmentTypes";

/**
 * Component to render the equipment type editor
 * @param {*} equipmentType
 * @param {*} navigate
 * @param {*} logout
 */
const EquipmentTypeEditor = ({ equipmentType, navigate, logout }) => {
  // Set up state
  const initialName = equipmentType != null ? equipmentType.name : null;
  const [name, setName] = useState(initialName);
  const [error, setError] = useState("");

  const saveEquipmentType = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Clear pre-existing errors
      setError("");

      try {
        // Either add or update the equipment type, depending on whether there's an
        // existing equipment type or not
        let updatedEquipmentType = null;
        if (equipmentType == null) {
          // Create the equipment type
          updatedEquipmentType = await apiCreateEquipmentType(name, logout);
        } else {
          // Update the existing equipment type
          updatedEquipmentType = await apiUpdateEquipmentType(
            equipmentType.id,
            name,
            logout
          );
        }

        // Go back to the artist list, which should reflect the updated details
        navigate({
          page: pages.equipmentTypes,
        });
      } catch (ex) {
        setError(
          `Error saving the updated equipment type details: ${ex.message}`
        );
      }
    },
    [equipmentType, logout, name, navigate]
  );

  // Set the page title
  const pageTitle =
    equipmentType != null ? equipmentType.name : "New Equipment Type";

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{pageTitle}</h5>
      </div>
      <div className={styles.equipmentTypeEditorFormContainer}>
        <form className={styles.equipmentTypeEditorForm}>
          <div className="row">
            {error != "" ? (
              <div className={styles.equipmentTypeEditorError}>{error}</div>
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
          <div className={styles.equipmentTypeEditorButton}>
            <button
              className="btn btn-primary"
              onClick={(e) => saveEquipmentType(e)}
            >
              Save
            </button>
          </div>
          <div className={styles.equipmentTypeEditorButton}>
            <button
              className="btn btn-primary"
              onClick={() =>
                navigate({
                  page: pages.equipmentTypes,
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
