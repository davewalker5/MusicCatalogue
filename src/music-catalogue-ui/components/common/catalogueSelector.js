import catalogues from "@/helpers/catalogues";
import styles from "./catalogueSelector.module.css";
import Select from "react-select";
import { useState } from "react";

/**
 * Component to render the catalogue selector
 * @param {*} selectedCatalogue
 * @param {*} catalogueChangedCallback
 * @returns
 */
const CatalogueSelector = ({ selectedCatalogue, catalogueChangedCallback }) => {
  // Construct a list of select list options for the catalogue
  const options = [
    { value: catalogues.main, label: "Main Catalogue" },
    { value: catalogues.wishlist, label: "Wish List" },
  ];

  // Determine the initial selection and set up state
  const selectedOption = options.find((x) => x.value === selectedCatalogue);
  const [catalogue, setCatalogue] = useState(selectedOption);

  // Callback to update the catalogue state and notify the parent component
  // that the catalogue has changed
  const catalogueChanged = (e) => {
    const updatedSelection = options.find((x) => x.value === e.value);
    setCatalogue(updatedSelection);
    catalogueChangedCallback(e.value);
  };

  return (
    <Select
      className={styles.catalogueSelector}
      value={catalogue}
      onChange={catalogueChanged}
      options={options}
    />
  );
};

export default CatalogueSelector;
