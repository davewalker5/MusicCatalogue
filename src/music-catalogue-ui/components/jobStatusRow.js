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
      <td>{record.start}</td>
      <td>{record.end}</td>
      <td>{record.error}</td>
    </tr>
  );
};

export default JobStatusRow;
