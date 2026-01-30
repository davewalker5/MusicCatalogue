import Select from "react-select";
import useMoods from "@/hooks/useMoods";
import { useState } from "react";

/**
 * Component to display the Mood selector
 * @param {*} initialMood
 * @param {*} moodChangedCallback
 * @param {*} logout
 * @returns
 */
const MoodSelector = ({ initialMood, moodChangedCallback, logout }) => {
  const { moods, setMoods } = useMoods(logout);

  let options = [];
  if (moods.length > 0) {
    // Construct the options for the moods drop-down
    for (let i = 0; i < moods.length; i++) {
      options = [...options, { value: moods[i].id, label: moods[i].name }];
    }
  }

  // Determine the initial selection
  let selectedOption = null;
  if (initialMood != null) {
    selectedOption = options.find((x) => x.value === initialMood.id);
  }

  // Set up state
  const [mood, setMood] = useState(selectedOption);

  // Callback to update the mood state and notify the parent component
  // that the mood has changed
  const moodChanged = (e) => {
    // Update local state with the selection from the drop-down
    const updatedSelection = options.find((x) => x.value === e.value);
    setMood(updatedSelection);

    // Notify the parent component with a mood object
    moodChangedCallback({
      id: updatedSelection.value,
      name: updatedSelection.label,
    });
  };

  return (
    <Select value={selectedOption} onChange={moodChanged} options={options} />
  );
};

export default MoodSelector;
