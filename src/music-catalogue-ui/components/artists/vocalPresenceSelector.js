import Select from "react-select";
import useVocalPresences from "@/hooks/useVocalPresences";

/**
 * Component to display the Vocal Presence selector
 * @param {*} vocalPresence
 * @param {*} vocalPresenceChangedCallback
 * @param {*} logout
 * @returns
 */
const VocalPresenceSelector = ({ vocalPresence, vocalPresenceChangedCallback, logout }) => {
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
  if (vocalPresence != null) {
    selectedOption = options.find((x) => x.value === vocalPresence);
  }

  return (
    <Select value={selectedOption} onChange={(e) => vocalPresenceChangedCallback(e.value)} options={options} />
  );
};

export default VocalPresenceSelector;
