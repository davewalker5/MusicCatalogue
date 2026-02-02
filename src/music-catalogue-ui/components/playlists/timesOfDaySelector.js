import Select from "react-select";
import usePlaylistTypes from "@/hooks/usePlaylistTypes";
import { useState } from "react";

/**
 * Component to display the "time of day" selector
 * @param {*} initialTimeOfDay
 * @param {*} timeOfDayChangedCallback
 * @param {*} logout
 * @returns
 */
const TimesOfDaySelector = ({ initialTimeOfDay, timeOfDayChangedCallback, logout }) => {
  const { timesOfDay, setTimesOfDay } = useTimesOfDay(logout);

  let options = [];
  if (timesOfDay && timesOfDay.length > 0) {
    // Construct the options for the "time of day" drop-down
    for (let i = 0; i < timesOfDay.length; i++) {
      options = [...options, { value: timesOfDay[i].id, label: timesOfDay[i].name }];
    }
  }

  // Determine the initial selection
  let selectedOption = null;
  if (initialTimeOfDay != null) {
    selectedOption = options.find((x) => x.value === initialTimeOfDay);
  }

  // Set up state
  const [timeOfDay, setTimeOfDay] = useState(selectedOption);

  // Callback to update the playlist type state and notify the parent component
  // that the playlist type has changed
  const timeOfDayChanged = (e) => {
    // Update local state with the selection from the drop-down
    const updatedSelection = options.find((x) => x.value === e.value);
    setTimeOfDay(updatedSelection);

    // Notify the parent component with a time of day object
    timeOfDayChangedCallback({
      id: updatedSelection.value,
      name: updatedSelection.label,
    });
  };

  return (
    <Select value={selectedOption} onChange={timeOfDayChanged} options={options} />
  );
};

export default TimesOfDaySelector;
