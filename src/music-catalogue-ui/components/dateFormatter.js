import config from "../config.json";

const DateFormatter = ({ value }) => {
  // Check there's a value to format
  if (value != null) {
    // Create a formatter to format the value
    const formatter = new Intl.DateTimeFormat(config.region.locale);

    // Format the value
    const dateToFormat = new Date(value);
    const formattedValue = formatter.format(dateToFormat);

    return <>{formattedValue}</>;
  } else {
    return <></>;
  }
};

export default DateFormatter;
