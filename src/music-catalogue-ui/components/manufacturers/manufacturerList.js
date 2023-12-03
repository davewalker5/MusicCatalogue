import styles from "./manufacturerList.module.css";
import pages from "@/helpers/navigation";
import useManufacturers from "@/hooks/useManufacturers";
import { useState } from "react";
import ManufacturerRow from "./manufacturerRow";

/**
 * Component to render a table listing all the manufacturers in the register
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const ManufacturerList = ({ navigate, logout }) => {
  const { manufacturers, setManufacturers } = useManufacturers(logout);
  const [error, setError] = useState("");

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Manufacturers</h5>
      </div>
      <div className="row">
        {error != "" ? (
          <div className={styles.manufacturerListError}>{error}</div>
        ) : (
          <></>
        )}
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Name</th>
          </tr>
        </thead>
        {manufacturers != null && (
          <tbody>
            {manufacturers.map((m) => (
              <ManufacturerRow
                key={m.id}
                manufacturer={m}
                navigate={navigate}
                logout={logout}
                setManufacturers={setManufacturers}
                setError={setError}
              />
            ))}
          </tbody>
        )}
      </table>
      <div className={styles.manufacturerListAddButton}>
        <button
          className="btn btn-primary"
          onClick={() =>
            navigate({
              page: pages.manufacturerEditor,
            })
          }
        >
          Add
        </button>
      </div>
    </>
  );
};

export default ManufacturerList;
