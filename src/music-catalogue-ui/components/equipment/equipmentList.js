import styles from "./equipmentList.module.css";
import pages from "@/helpers/navigation";
import useEquipment from "@/hooks/useEquipment";
import { useState } from "react";
import EquipmentRow from "./equipmentRow";

/**
 * Component to render a table listing all the equipment in the register
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const EquipmentList = ({ isWishList, navigate, logout }) => {
  const { equipment, setEquipment } = useEquipment(isWishList, logout);
  const [error, setError] = useState("");

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Equipment</h5>
      </div>
      <div className="row">
        {error != "" ? (
          <div className={styles.equipmentListError}>{error}</div>
        ) : (
          <></>
        )}
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Description</th>
            <th>Model</th>
            <th>Serial No.</th>
            <th>Type</th>
            <th>Manufacturer</th>
            <th>Purchased</th>
            <th>Price</th>
            <th>Retailer</th>
            <th></th>
            <th></th>
            <th></th>
            <th></th>
          </tr>
        </thead>
        {equipment != null && (
          <tbody>
            {equipment.map((e) => (
              <EquipmentRow
                key={e.id}
                equipment={e}
                isWishList={isWishList}
                navigate={navigate}
                logout={logout}
                setEquipment={setEquipment}
                setError={setError}
              />
            ))}
          </tbody>
        )}
      </table>
      <div className={styles.equipmentListAddButton}>
        <button
          className="btn btn-primary"
          onClick={() =>
            navigate({
              page: pages.equipmentEditor,
            })
          }
        >
          Add
        </button>
      </div>
    </>
  );
};

export default EquipmentList;
