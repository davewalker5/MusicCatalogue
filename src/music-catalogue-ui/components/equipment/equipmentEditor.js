import styles from "./equipmentEditor.module.css";
import pages from "@/helpers/navigation";
import FormInputField from "../common/formInputField";
import { useState, useCallback } from "react";
import {
  apiCreateEquipment,
  apiUpdateEquipment,
} from "@/helpers/api/apiEquipment";
import EquipmentTypeSelector from "../equipmentTypes/equipmentTypeSelector";
import ManufacturerSelector from "../manufacturers/manufacturerSelector";

/**
 * Component to render an album editor, excluding purchase details that are
 * maintained via their own component and the catalogue, which is maintained
 * via the album list
 * @param {*} album
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const EquipmentEditor = ({ equipment, isWishList, navigate, logout }) => {
  // Get the initial values for equipment properties
  const initialEquipmentType =
    equipment != null ? equipment.equipmentType : null;
  const initialManufacturer = equipment != null ? equipment.manufacturer : null;
  const initialDescription = equipment != null ? equipment.description : null;
  const initialModel = equipment != null ? equipment.model : null;
  const initialSerialNumber = equipment != null ? equipment.serialNumber : null;

  // Setup state
  const [equipmentType, setEquipmentType] = useState(initialEquipmentType);
  const [manufacturer, setManufacturer] = useState(initialManufacturer);
  const [description, setDescription] = useState(initialDescription);
  const [model, setModel] = useState(initialModel);
  const [serialNumber, setSerialNumber] = useState(initialSerialNumber);
  const [error, setError] = useState("");

  const saveEquipment = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Clear pre-existing errors
      setError("");

      try {
        // Get the equipment type and manufacturer IDs
        const equipmentTypeId = equipmentType != null ? equipmentType.id : null;
        const manufacturerId = manufacturer != null ? manufacturer.id : null;

        // Either add or update the equipment, depending on whether there's an
        // existing equipment or not
        let updatedEquipment = null;
        if (equipment == null) {
          // Create the equipment record
          updatedEquipment = await apiCreateEquipment(
            equipmentTypeId,
            manufacturerId,
            description,
            model,
            serialNumber,
            isWishList,
            null,
            null,
            null,
            logout
          );
        } else {
          // Update the existing equipment record
          updatedEquipment = await apiUpdateEquipment(
            equipment.id,
            equipmentTypeId,
            manufacturerId,
            description,
            model,
            serialNumber,
            isWishList,
            equipment.purchased,
            equipment.price,
            equipment.retailerId,
            logout
          );
        }

        // Go back to the equipment list, which should reflect the updated details
        navigate({
          page: pages.equipment,
          isWishList: isWishList,
        });
      } catch (ex) {
        setError(`Error saving the updated equipment details: ${ex.message}`);
      }
    },
    [
      equipment,
      description,
      equipmentType,
      manufacturer,
      model,
      serialNumber,
      isWishList,
      navigate,
      logout,
    ]
  );

  // Set the page title
  const pageTitle = equipment != null ? equipment.description : "New Equipment";

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{pageTitle}</h5>
      </div>
      <div className={styles.equipmentEditorFormContainer}>
        <form className={styles.equipmentEditorForm}>
          <div className="row">
            {error != "" ? (
              <div className={styles.equipmentEditorError}>{error}</div>
            ) : (
              <></>
            )}
          </div>
          <div className="row align-items-start">
            <FormInputField
              label="Description"
              name="description"
              value={description}
              setValue={setDescription}
            />
          </div>
          <div className="row align-items-start">
            <FormInputField
              label="Model"
              name="model"
              value={model}
              setValue={setModel}
            />
          </div>
          <div className="row align-items-start">
            <FormInputField
              label="Serial No."
              name="serialNumber"
              value={serialNumber}
              setValue={setSerialNumber}
            />
          </div>
          <div className="form-group mt-3">
            <label className={styles.equipmentEditorFormLabel}>
              Equipment Type
            </label>
            <div>
              <EquipmentTypeSelector
                initialEquipmentType={equipmentType}
                equipmentTypeChangedCallback={setEquipmentType}
                logout={logout}
              />
            </div>
          </div>
          <div className="form-group mt-3">
            <label className={styles.equipmentEditorFormLabel}>
              Manufacturer
            </label>
            <div>
              <ManufacturerSelector
                initialManufacturer={manufacturer}
                manufacturerChangedCallback={setManufacturer}
                logout={logout}
              />
            </div>
          </div>
          <div className="d-grid gap-2 mt-3"></div>
          <div className="d-grid gap-2 mt-3"></div>
          <div className={styles.equipmentEditorButton}>
            <button
              className="btn btn-primary"
              onClick={(e) => saveEquipment(e)}
            >
              Save
            </button>
          </div>
          <div className={styles.equipmentEditorButton}>
            <button
              className="btn btn-primary"
              onClick={() =>
                navigate({
                  page: pages.equipment,
                  isWishList: isWishList,
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

export default EquipmentEditor;
