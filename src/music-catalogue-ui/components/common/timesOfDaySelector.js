import Select from "react-select";
import useTimesOfDay from "@/hooks/useTimesOfDay";
import { useState, useEffect, useMemo } from "react";

/**
 * Component to display the "time of day" selector
 * @param {*} initialTimeOfDay
 * @param {*} timeOfDayChangedCallback
 * @param {*} logout
 * @returns
 */
const TimesOfDaySelector = ({ initialTimeOfDay, timeOfDayChangedCallback, logout }) => {
  const { timesOfDay, setTimesOfDay } = useTimesOfDay(logout);

  const options = useMemo(() => (timesOfDay ?? []).map((timeOfDay) => ({
    value: timeOfDay.id,
    label: timeOfDay.name,
  })), [timesOfDay]);

  // Set up state
  const [timeOfDay, setTimeOfDay] = useState(null);

  useEffect(() => {
    if ((timeOfDay == null) && (initialTimeOfDay != null) && (options.length > 0)) {
      const match = options.find(o => o.value === initialTimeOfDay.id);
      if (match) {
        setTimeOfDay(match);
      }
    }
  }, [initialTimeOfDay, timeOfDay, options]);

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
    <Select value={timeOfDay} onChange={timeOfDayChanged} options={options} />
  );
};

export default TimesOfDaySelector;
