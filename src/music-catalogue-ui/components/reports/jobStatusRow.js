import DateAndTimeFormatter from "../common/dateAndTimeFormatter";

/**
 * Component to render a row containing the details of a single job status record
 * @param {*} record
 * @returns
 */
const JobStatusRow = ({ record }) => {
  return (
    <tr>
      <td>{record.name}</td>
      <td>{record.parameters}</td>
      <td>
        <DateAndTimeFormatter value={record.start} />
      </td>
      <td>
        <DateAndTimeFormatter value={record.end} />
      </td>
      <td>{record.error}</td>
    </tr>
  );
};

export default JobStatusRow;
