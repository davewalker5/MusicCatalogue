import pages from "@/helpers/navigation";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPenToSquare } from "@fortawesome/free-solid-svg-icons";
import DeleteEquipmentTypeActionIcon from "./deleteEquipmentTypeActionIcon";

/**
 * Component to render a row containing the details for a single equipment type
 * @param {*} equipmentType
 * @param {*} navigate
 * @param {*} logout
 * @param {*} setEquipmentTypes
 * @param {*} setError
 * @returns
 */
const EquipmentTypeRow = ({
  equipmentType,
  navigate,
  logout,
  setEquipmentTypes,
  setError,
}) => {
  return (
    <tr>
      <td>{equipmentType.name}</td>
      <td>
        <DeleteEquipmentTypeActionIcon
          equipmentType={equipmentType}
          logout={logout}
          setEquipmentTypes={setEquipmentTypes}
          setError={setError}
        />
      </td>
      <td>
        <FontAwesomeIcon
          icon={faPenToSquare}
          onClick={() =>
            navigate({
              page: pages.equipmentTypeEditor,
              equipmentType: equipmentType,
            })
          }
        />
      </td>
    </tr>
  );
};

export default EquipmentTypeRow;
