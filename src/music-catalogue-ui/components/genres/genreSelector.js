import Select from "react-select";
import useGenres from "@/hooks/useGenres";
import { useState } from "react";

/**
 * Component to display the Genre selector
 * @param {*} initialGenre
 * @param {*} genreChangedCallback
 * @param {*} logout
 * @returns
 */
const GenreSelector = ({ initialGenre, genreChangedCallback, logout }) => {
  const { genres, setGenres } = useGenres(logout);

  let options = [];
  if (genres.length > 0) {
    // Construct the options for the genres drop-down
    for (let i = 0; i < genres.length; i++) {
      options = [...options, { value: genres[i].id, label: genres[i].name }];
    }
  }

  // Determine the initial selection
  let selectedOption = null;
  if (initialGenre != null) {
    selectedOption = options.find((x) => x.value === initialGenre.id);
  }

  // Set up state
  const [genre, setGenre] = useState(selectedOption);

  // Callback to update the genre state and notify the parent component
  // that the genre has changed
  const genreChanged = (e) => {
    // Update local state with the selection from the drop-down
    const updatedSelection = options.find((x) => x.value === e.value);
    setGenre(updatedSelection);

    // Notify the parent component with a genre object
    genreChangedCallback({
      id: updatedSelection.value,
      name: updatedSelection.label,
    });
  };

  return (
    <Select value={selectedOption} onChange={genreChanged} options={options} />
  );
};

export default GenreSelector;
