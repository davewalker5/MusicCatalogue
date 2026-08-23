import Select from "react-select";
import usePlaylistTypes from "@/hooks/usePlaylistTypes";
import { useState, useEffect, useMemo } from "react";

/**
 * Component to display the Playlist Type selector
 * @param {*} initialPlaylistType
 * @param {*} playlistTypeChangedCallback
 * @param {*} logout
 * @returns
 */
const PlaylistTypeSelector = ({ initialPlaylistType, playlistTypeChangedCallback, logout }) => {
  const { playlistTypes, setPlaylistTypes } = usePlaylistTypes(logout);

  const options = useMemo(() => (playlistTypes ?? []).map((playlistType) => ({
    value: playlistType.id,
    label: playlistType.name,
  })), [playlistTypes]);

  // Set up state
  const [playlistType, setPlaylistType] = useState(null);

  useEffect(() => {
    if ((playlistType == null) && (initialPlaylistType != null) && (options.length > 0)) {
      const match = options.find(o => o.value === initialPlaylistType.id);
      if (match) {
        setPlaylistType(match);
      }
    }
  }, [initialPlaylistType, playlistType, options]);

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
