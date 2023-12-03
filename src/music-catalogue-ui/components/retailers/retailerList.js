import styles from "./retailerList.module.css";
import pages from "@/helpers/navigation";
import useRetailers from "@/hooks/useRetailers";
import RetailerRow from "./retailerRow";
import { useCallback, useState } from "react";

/**
 * Component to render a table listing all the retailers in the catalogue
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const RetailerList = ({ navigate, logout }) => {
  const { retailers: retailers, setRetailers } = useRetailers(logout);
  const [errorMessage, setError] = useState(null);

  // Callback passed to child components to set the error message
  const setErrorCallback = useCallback((message) => {
    setError(message);
  }, []);

  // Callback passed to child components to clear the error message
  const clearErrorCallback = useCallback(() => {
    setError(null);
  }, []);

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Retailers</h5>
      </div>
      {errorMessage != null ? (
        <div className={styles.retailerListErrorContainer}>{errorMessage}</div>
      ) : (
        <></>
      )}
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Name</th>
            <th>Town</th>
            <th>County</th>
            <th>Country</th>
            <th>Web Site</th>
            <th />
          </tr>
        </thead>
        {retailers != null && (
          <tbody>
            {retailers.map((r) => (
              <RetailerRow
                key={r.id}
                retailer={r}
                navigate={navigate}
                logout={logout}
                setRetailers={setRetailers}
                clearError={clearErrorCallback}
                setError={setErrorCallback}
              />
            ))}
          </tbody>
        )}
      </table>
      <div className={styles.retailerListButton}>
        <button
          className="btn btn-primary"
          onClick={() =>
            navigate({
              page: pages.retailerEditor,
              retailer: {
                id: 0,
                name: null,
                address1: null,
                address2: null,
                town: null,
                county: null,
                postCode: null,
                country: null,
                webSite: null,
                latitude: null,
                longitude: null,
              },
            })
          }
        >
          Add
        </button>
      </div>
    </>
  );
};

export default RetailerList;
