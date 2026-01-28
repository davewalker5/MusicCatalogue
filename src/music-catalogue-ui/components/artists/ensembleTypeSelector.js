import Select from "react-select";
import useEnsembleTypes from "@/hooks/useEnsembleTypes";
import { useState } from "react";

/**
 * Component to display the Ensemble Type selector
 * @param {*} initialEnsembleType
 * @param {*} ensembleTypeChangedCallback
 * @param {*} logout
 * @returns
 */
const EnsembleTypeSelector = ({ initialEnsembleType, ensembleTypeChangedCallback, logout }) => {
  const { ensembleTypes, setEnsembleTypes } = useEnsembleTypes(logout);

  let options = [];
  if (ensembleTypes && ensembleTypes.length > 0) {
    // Construct the options for the ensemble type drop-down
    for (let i = 0; i < ensembleTypes.length; i++) {
      options = [...options, { value: ensembleTypes[i].id, label: ensembleTypes[i].name }];
    }
  }

  // Determine the initial selection
  let selectedOption = null;
  if (initialEnsembleType != null) {
    selectedOption = options.find((x) => x.value === initialEnsembleType);
  }

  // Set up state
  const [ensembleType, setEnsembleType] = useState(selectedOption);

  // Callback to update the ensemble type state and notify the parent component
  // that the ensemble type has changed
  const ensembleTypeChanged = (e) => {
    // Update local state with the selection from the drop-down
    const updatedSelection = options.find((x) => x.value === e.value);
    setEnsembleType(updatedSelection);

    // Notify the parent component with an ensemble type object
    ensembleTypeChangedCallback({
      id: updatedSelection.value,
      name: updatedSelection.label,
    });
  };

  return (
    <Select value={selectedOption} onChange={ensembleTypeChanged} options={options} />
  );
};

export default EnsembleTypeSelector;
