import Select from "react-select";
import useVocalPresences from "@/hooks/useVocalPresences";
import { useState } from "react";

/**
 * Component to display the Vocal Presence selector
 * @param {*} initialVocalPresence
 * @param {*} vocalPresenceChangedCallback
 * @param {*} logout
 * @returns
 */
const VocalPresenceSelector = ({ initialVocalPresence, vocalPresenceChangedCallback, logout }) => {
  const { vocalPresences, setVocalPresences } = useVocalPresences(logout);

  let options = [];
  if (vocalPresences && vocalPresences.length > 0) {
    // Construct the options for the vocal presence drop-down
    for (let i = 0; i < vocalPresences.length; i++) {
      options = [...options, { value: vocalPresences[i].id, label: vocalPresences[i].name }];
    }
  }

  // Determine the initial selection
  let selectedOption = null;
  if (initialVocalPresence != null) {
    selectedOption = options.find((x) => x.value === initialVocalPresence);
  }

  // Set up state
  const [vocalPresence, setVocalPresence] = useState(selectedOption);

  // Callback to update the vocal presence state and notify the parent component
  // that the vocal presence has changed
  const vocalPresenceChanged = (e) => {
    // Update local state with the selection from the drop-down
    const updatedSelection = options.find((x) => x.value === e.value);
    setVocalPresence(updatedSelection);

    // Notify the parent component with a vocal presence object
    vocalPresenceChangedCallback({
      id: updatedSelection.value,
      name: updatedSelection.label,
    });
  };

  return (
    <Select value={selectedOption} onChange={vocalPresenceChanged} options={options} />
  );
};

export default VocalPresenceSelector;
