import config from "@/config.json";

/**
 * Format a value as a date and time using the locale from the config file
 * @param {*} param0
 * @returns
 */
const DateAndTimeFormatter = ({ value }) => {
  // Check there's a value to format
  if (value != null) {
    // Create a formatter to format the value
    const formatter = new Intl.DateTimeFormat(config.region.locale, {
      dateStyle: "short",
      timeStyle: "medium",
    });

    // Format the value
    const dateToFormat = new Date(value);
    const formattedValue = formatter.format(dateToFormat);

    return <>{formattedValue}</>;
  } else {
    return <></>;
  }
};

export default DateAndTimeFormatter;
