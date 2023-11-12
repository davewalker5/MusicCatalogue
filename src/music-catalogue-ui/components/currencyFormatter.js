import config from "../config.json";

const CurrencyFormatter = ({ value }) => {
  // Check there's a value to format
  if (value != null) {
    // Create a formatter to format the value
    const formatter = new Intl.NumberFormat(config.region.locale, {
      style: "currency",
      currency: config.region.currency,
    });

    // Format the value
    const formattedValue = formatter.format(value);

    return <>{formattedValue}</>;
  } else {
    return <></>;
  }
};

export default CurrencyFormatter;
