import Select from "react-select";
import { useState } from "react";
import useManufacturers from "@/hooks/useManufacturers";

/**
 * Component to display the manufacturer selector
 * @param {*} initialManufacturer
 * @param {*} manufacturerChangedCallback
 * @param {*} logout
 * @returns
 */
const ManufacturerSelector = ({
  initialManufacturer,
  manufacturerChangedCallback,
  logout,
}) => {
  const { manufacturers, setManufacturers } = useManufacturers(logout);

  let options = [];
  if (manufacturers.length > 0) {
    // Construct the options for the drop-down
    for (let i = 0; i < manufacturers.length; i++) {
      options = [
        ...options,
        { value: manufacturers[i].id, label: manufacturers[i].name },
      ];
    }
  }

  // Determine the initial selection
  let selectedOption = null;
  if (initialManufacturer != null) {
    selectedOption = options.find((x) => x.value === initialManufacturer.id);
  }

  // Set up state
  const [manufacturer, setManufacturer] = useState(selectedOption);

  // Callback to update state and notify the parent component
  // that the selection has changed
  const manufacturerChanged = (e) => {
    // Update local state with the selection from the drop-down
    const updatedSelection = options.find((x) => x.value === e.value);
    setManufacturer(updatedSelection);

    // Notify the parent component with a manufacturer object
    manufacturerChangedCallback({
      id: updatedSelection.value,
      name: updatedSelection.label,
    });
  };

  return (
    <Select
      value={selectedOption}
      onChange={manufacturerChanged}
      options={options}
    />
  );
};

export default ManufacturerSelector;
