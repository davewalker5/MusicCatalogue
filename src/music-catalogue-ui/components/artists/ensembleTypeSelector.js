import Select from "react-select";
import useEnsembleTypes from "@/hooks/useEnsembleTypes";

/**
 * Component to display the Ensemble Type selector
 * @param {*} ensembleType
 * @param {*} ensembleTypeChangedCallback
 * @param {*} logout
 * @returns
 */
const EnsembleTypeSelector = ({ ensembleType, ensembleTypeChangedCallback, logout }) => {
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
  if (ensembleType != null) {
    selectedOption = options.find((x) => x.value === ensembleType);
  }

  return (
    <Select value={selectedOption} onChange={(e) => ensembleTypeChangedCallback(e.value)} options={options} />
  );
};

export default EnsembleTypeSelector;
