import Select from "react-select";
import usePlaylistTypes from "@/hooks/usePlaylistTypes";
import { useState, useEffect } from "react";

/**
 * Component to display the Playlist Type selector
 * @param {*} initialPlaylistType
 * @param {*} playlistTypeChangedCallback
 * @param {*} logout
 * @returns
 */
const PlaylistTypeSelector = ({ initialPlaylistType, playlistTypeChangedCallback, logout }) => {
  const { playlistTypes, setPlaylistTypes } = usePlaylistTypes(logout);

  let options = [];
  if (playlistTypes && playlistTypes.length > 0) {
    // Construct the options for the playlist type drop-down
    for (let i = 0; i < playlistTypes.length; i++) {
      options = [...options, { value: playlistTypes[i].id, label: playlistTypes[i].name }];
    }
  }

  // Set up state
  const [playlistType, setPlaylistType] = useState(null);

  useEffect(() => {
    if ((playlistType == null) && (initialPlaylistType != null) && (options.length > 0)) {
      const match = options.find(o => o.value === initialPlaylistType.id);
      if (match) {
        setPlaylistType(match);
      }
    }
  }, [initialPlaylistType, options]);

  // Callback to update the playlist type state and notify the parent component
  // that the playlist type has changed
  const playlistTypeChanged = (e) => {
    // Update local state with the selection from the drop-down
    const updatedSelection = options.find((x) => x.value === e.value);
    setPlaylistType(updatedSelection);

    // Notify the parent component with an playlist type object
    playlistTypeChangedCallback({
      id: updatedSelection.value,
      name: updatedSelection.label,
    });
  };

  return (
    <Select value={playlistType} onChange={playlistTypeChanged} options={options} />
  );
};

export default PlaylistTypeSelector;
