import Select from "react-select";
import useArtists from "@/hooks/useArtists";
import { useState } from "react";

/**
 * Component to display the Artist selector
 * @param {*} initialArtist
 * @param {*} artistChangedCallback
 * @param {*} logout
 * @returns
 */
const ArtistSelector = ({ initialArtist, artistChangedCallback, logout }) => {
  const { artists, setArtists } = useArtists(logout);

  let options = [{value: null, label: ""}];
  if (artists.length > 0) {
    // Construct the options for the artists drop-down
    for (let i = 0; i < artists.length; i++) {
      options = [...options, { value: artists[i].id, label: artists[i].name }];
    }
  }

  // Determine the initial selection
  let selectedOption = null;
  if (initialArtist != null) {
    selectedOption = options.find((x) => x.value === initialArtist.id);
  }

  // Set up state
  const [artist, setArtist] = useState(selectedOption);

  // Callback to update the artist state and notify the parent component
  // that the artist has changed
  const artistChanged = (e) => {
    // Update local state with the selection from the drop-down
    const updatedSelection = options.find((x) => x.value === e.value);
    setArtist(updatedSelection);

    // Notify the parent component with a artist object
    artistChangedCallback({
      id: updatedSelection.value,
      name: updatedSelection.label,
    });
  };

  return (
    <Select value={selectedOption} onChange={artistChanged} options={options} />
  );
};

export default ArtistSelector;
