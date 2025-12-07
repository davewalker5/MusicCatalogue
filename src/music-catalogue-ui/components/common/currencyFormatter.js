import config from "@/config.json";

/**
 * Format a value as currency using the locale from the config file
 * @param {*} param0
 * @returns
 */
const CurrencyFormatter = ({ value, renderZeroAsBlank }) => {
  // Check there's a value to format
  if (value != null && (value > 0 || !renderZeroAsBlank)) {
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
