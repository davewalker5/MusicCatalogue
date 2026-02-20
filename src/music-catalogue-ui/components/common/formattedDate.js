import React from "react";

const formatterLocal = new Intl.DateTimeFormat("en-GB", {
  day: "2-digit",
  month: "short",
  year: "numeric",
  hour: "2-digit",
  minute: "2-digit",
  hour12: false,
});

const formatterUTC = new Intl.DateTimeFormat("en-GB", {
  day: "2-digit",
  month: "short",
  year: "numeric",
  hour: "2-digit",
  minute: "2-digit",
  hour12: false,
  timeZone: "UTC",
});

const FormattedDate = ({
  value,
  utc = false,
  className = "",
}) => {
  const date = value instanceof Date ? value : new Date(value);
  const formatter = utc ? formatterUTC : formatterLocal;

  return (
    <time
      dateTime={date.toISOString()}
      className={className}
    >
      {formatter.format(date).replace(",", "")}
    </time>
  );
}

export default FormattedDate;
