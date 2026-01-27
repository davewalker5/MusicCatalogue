import Select from "react-select";
import useEquipmentTypes from "@/hooks/useEquipmentTypes";
import { useState } from "react";

/**
 * Component to display the equipment type selector
 * @param {*} initialEquipmentType
 * @param {*} equipmentTypeChangedCallback
 * @param {*} logout
 * @returns
 */
const EquipmentTypeSelector = ({
  initialEquipmentType,
  equipmentTypeChangedCallback,
  logout,
}) => {
  const { equipmentTypes, setEquipmentTypes } = useEquipmentTypes(logout);

  let options = [];
  if (equipmentTypes.length > 0) {
    // Construct the options for the drop-down
    for (let i = 0; i < equipmentTypes.length; i++) {
      options = [
        ...options,
        { value: equipmentTypes[i].id, label: equipmentTypes[i].name },
      ];
    }
  }

  // Determine the initial selection
  let selectedOption = null;
  if (initialEquipmentType != null) {
    selectedOption = options.find((x) => x.value === initialEquipmentType.id);
  }

  // Set up state
  const [equipmentType, setEquipmentType] = useState(selectedOption);

  // Callback to update state and notify the parent component
  // that the selection has changed
  const equipmentTypeChanged = (e) => {
    // Update local state with the selection from the drop-down
    const updatedSelection = options.find((x) => x.value === e.value);
    setEquipmentType(updatedSelection);

    // Notify the parent component with an equipment type object
    equipmentTypeChangedCallback({
      id: updatedSelection.value,
      name: updatedSelection.label,
    });
  };

  return (
    <Select
      value={selectedOption}
      onChange={equipmentTypeChanged}
      options={options}
    />
  );
};

export default EquipmentTypeSelector;
