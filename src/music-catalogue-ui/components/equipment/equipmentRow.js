import pages from "@/helpers/navigation";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPenToSquare, faCoins } from "@fortawesome/free-solid-svg-icons";
import DeleteEquipmentActionIcon from "./deleteEquipmentActionIcon";
import DateFormatter from "../common/dateFormatter";
import CurrencyFormatter from "../common/currencyFormatter";
import EquipmentWishListActionIcon from "./equipmentWishListActionIcon";
import { Tooltip } from "react-tooltip";

/**
 * Component to render a row containing the details for a single item of equipment
 * @param {*} equipment
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} logout
 * @param {*} setManufacturers
 * @param {*} setError
 * @returns
 */
const EquipmentRow = ({
  equipment,
  isWishList,
  navigate,
  logout,
  setEquipment,
  setError,
}) => {
  // Get the equipment type name
  const equipmentType = equipment["equipmentType"];
  const equipmentTypeName = equipmentType != null ? equipmentType["name"] : "";

  // Get the manufacturer name
  const manufacturer = equipment["manufacturer"];
  const manufacturerName = manufacturer != null ? manufacturer["name"] : "";

  // Get the retailer name
  const retailer = equipment["retailer"];
  const retailerName = retailer != null ? retailer["name"] : "";

  return (
    <tr>
      <td>{equipment.description}</td>
      <td>{equipment.model}</td>
      <td>{equipment.serialNumber}</td>
      <td>{equipmentTypeName}</td>
      <td>{manufacturerName}</td>
      <td>
        <DateFormatter value={equipment.purchased} />
      </td>
      <td>
        <CurrencyFormatter value={equipment.price} renderZeroAsBlank={true} />
      </td>
      <td>{retailerName}</td>
      <td>
        <DeleteEquipmentActionIcon
          equipment={equipment}
          isWishList={isWishList}
          logout={logout}
          setEquipment={setEquipment}
          setError={setError}
        />
      </td>
      <td>
        <>
          <FontAwesomeIcon
            icon={faPenToSquare}
            data-tooltip-id="edit-tooltip"
            data-tooltip-content="Edit equipment"
            onClick={() =>
              navigate({
                page: pages.equipmentEditor,
                equipment: equipment,
              })
            }
          />

          <Tooltip id="edit-tooltip" />
        </>
      </td>
      <td>
        <EquipmentWishListActionIcon
          equipment={equipment}
          isWishList={isWishList}
          logout={logout}
          setEquipment={setEquipment}
        />
      </td>
      <td>
        <>
          <FontAwesomeIcon
            icon={faCoins}
            data-tooltip-id="purchase-tooltip"
            data-tooltip-content="Edit equipment purchase details"
            onClick={() =>
              navigate({
                page: pages.equipmentPurchaseDetails,
                equipment: equipment,
              })
            }
          />

          <Tooltip id="purchase-tooltip" />
        </>
      </td>
    </tr>
  );
};

export default EquipmentRow;
