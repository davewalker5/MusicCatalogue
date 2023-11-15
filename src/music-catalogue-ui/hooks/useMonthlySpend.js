import { useState, useEffect } from "react";
import { apiMonthlySpendReport } from "@/helpers/apiReports";

/**
 * Hook that uses the API helpers to retrieve a list of artists from the
 * Music Catalogue REST API
 * @param {*} logout
 * @returns
 */
const useMonthlySpend = (logout) => {
  // Current list of artists and the method to change it
  const [records, setRecords] = useState([]);

  useEffect(() => {
    const fetchReport = async () => {
      try {
        // Get a list of records via the service, store it in state
        var fetchedRecords = await apiMonthlySpendReport(logout);
        setRecords(fetchedRecords);
      } catch {}
    };

    fetchReport();
  }, [logout]);

  return { records, setRecords };
};

export default useMonthlySpend;
