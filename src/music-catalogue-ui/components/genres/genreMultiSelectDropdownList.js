import styles from "./genreMultiSelectDropdownList.module.css";
import useGenres from "@/hooks/useGenres";
import Multiselect from "multiselect-react-dropdown";
import React, { useState, useCallback } from "react";

/**
 * Component to display a multi-select list of Genres
 * @param {*} label
 * @param {*} onSelectionChanged
 * @param {*} logout
 * @returns
 */
const GenreMultiSelectDropdownList = ({ onSelectionChanged, logout }) => {
  const { genres, setGenres } = useGenres(logout);
  const [selectedGenres, setSelectedGenres] = useState([]);

  // Callback to update the selection state and notify the parent component that the selection has changed
  const genreSelectionChanged = (updatedSelection) => {
    // Update local state with the selection from the drop-down
    // const updatedSelection = options.find((x) => x.value === e.value);
    setSelectedGenres(updatedSelection);

    // Notify the parent component with a genre object
    onSelectionChanged(updatedSelection);
  };

  return (
    <div className={styles.noChips}>
      <Multiselect
        options={genres}
        selectedValues={selectedGenres}
        onSelect={genreSelectionChanged}
        onRemove={genreSelectionChanged}
        displayValue="name"
        showCheckbox="true"
        showArrow="true"
      />
    </div>
  );
};

export default GenreMultiSelectDropdownList;
