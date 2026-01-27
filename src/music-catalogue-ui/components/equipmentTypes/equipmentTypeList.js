import styles from "./equipmentTypeList.module.css";
import pages from "@/helpers/navigation";
import useEquipmentTypes from "@/hooks/useEquipmentTypes";
import EquipmentTypeRow from "./equipmentTypeRow";
import { useState } from "react";

/**
 * Component to render a table listing all the equipment types in the register
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const EquipmentTypeList = ({ navigate, logout }) => {
  const { equipmentTypes, setEquipmentTypes } = useEquipmentTypes(logout);
  const [error, setError] = useState("");

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Equipment Types</h5>
      </div>
      <div className="row">
        {error != "" ? (
          <div className={styles.equipmentTypeListError}>{error}</div>
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
        {equipmentTypes != null && (
          <tbody>
            {equipmentTypes.map((et) => (
              <EquipmentTypeRow
                key={et.id}
                equipmentType={et}
                navigate={navigate}
                logout={logout}
                setEquipmentTypes={setEquipmentTypes}
                setError={setError}
              />
            ))}
          </tbody>
        )}
      </table>
      <div className={styles.equipmentTypeListAddButton}>
        <button
          className="btn btn-primary"
          onClick={() =>
            navigate({
              page: pages.equipmentTypeEditor,
            })
          }
        >
          Add
        </button>
      </div>
    </>
  );
};

export default EquipmentTypeList;
