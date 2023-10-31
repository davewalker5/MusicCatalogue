import { useCallback, useState } from "react";

const JobStatusReport = ({ navigate, logout }) => {
  const [startDate, setStartDate] = useState(new Date());

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Job Status Report</h5>
      </div>
    </>
  );
};

export default JobStatusReport;
