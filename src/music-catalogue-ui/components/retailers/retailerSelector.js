import Select from "react-select";
import useRetailers from "@/hooks/useRetailers";
import { useState } from "react";

/**
 * Component to display the retailer selector
 * @param {*} initialRetailer
 * @param {*} retailerChangedCallback
 * @param {*} logout
 * @returns
 */
const RetailerSelector = ({
  initialRetailer,
  retailerChangedCallback,
  logout,
}) => {
  const { retailers, setRetailers } = useRetailers(logout);

  let options = [];
  if (retailers.length > 0) {
    // Construct the options for the retailers drop-down
    for (let i = 0; i < retailers.length; i++) {
      options = [
        ...options,
        { value: retailers[i].id, label: retailers[i].name },
      ];
    }
  }

  // Determine the initial selection
  let selectedOption = null;
  if (initialRetailer != null) {
    selectedOption = options.find((x) => x.value === initialRetailer.id);
  }

  // Set up state
  const [retailer, setRetailer] = useState(selectedOption);

  // Callback to update the genre state and notify the parent component
  // that the genre has changed
  const retailerChanged = (e) => {
    // Update local state with the selected option
    const updatedSelection = options.find((x) => x.value === e.value);
    setRetailer(updatedSelection);

    // Notify the parent component with a partial retailer object
    retailerChangedCallback({
      id: updatedSelection.value,
      name: updatedSelection.label,
    });
  };

  return (
    <Select
      value={selectedOption}
      onChange={retailerChanged}
      options={options}
    />
  );
};

export default RetailerSelector;
