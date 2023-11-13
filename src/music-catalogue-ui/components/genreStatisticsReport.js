import React, { useCallback, useState } from "react";
import styles from "./reports.module.css";
import Select from "react-select";
import "react-datepicker/dist/react-datepicker.css";
import { apiGenreStatisticsReport } from "@/helpers/apiReports";
import GenreStatisticsRow from "./genreStatisticsRow";

/**
 * Component to display the job status search page and its results
 * @param {*} logout
 * @returns
 */
const GenreStatisticsReport = ({ logout }) => {
  const [catalogue, setCatalogue] = useState(0);
  const [records, setRecords] = useState(null);

  // Callback to request the genre statistics report from the API
  const getReportCallback = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Set the wishlist flag from the drop-down selection
      const forWishList = catalogue.value == "wishlist";

      // Fetch the report
      const fetchedRecords = await apiGenreStatisticsReport(
        forWishList,
        logout
      );
      setRecords(fetchedRecords);
    },
    [catalogue, logout]
  );

  // Construct a list of select list options for the directory
  const options = [
    { value: "catalogue", label: "Main Catalogue" },
    { value: "wishlist", label: "Wish List" },
  ];

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Genre Statistics Report</h5>
      </div>
      <div className={styles.reportFormContainer}>
        <form className={styles.reportForm}>
          <div className="row" align="center">
            <div className="mt-3">
              <div className="d-inline-flex align-items-center">
                <label className={styles.reportFormLabel}>
                  Generate Report For
                </label>
                <Select
                  className={styles.reportCatalogueSelector}
                  defaultValue={catalogue}
                  onChange={setCatalogue}
                  options={options}
                />
              </div>
              <button
                className="btn btn-primary"
                onClick={(e) => getReportCallback(e)}
              >
                Search
              </button>
            </div>
          </div>
        </form>
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Genre</th>
            <th>Artists</th>
            <th>Albums</th>
            <th>Tracks</th>
            <th>Total Spend</th>
          </tr>
        </thead>
        {records != null && (
          <tbody>
            {records.map((r) => (
              <GenreStatisticsRow key={r.id} record={r} />
            ))}
          </tbody>
        )}
      </table>
    </>
  );
};

export default GenreStatisticsReport;