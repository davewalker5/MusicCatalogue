import pages from "@/helpers/navigation";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPenToSquare } from "@fortawesome/free-solid-svg-icons";
import DeleteManufacturerActionIcon from "./deleteManufacturerActionIcon";

/**
 * Component to render a row containing the details for a single manufacturer
 * @param {*} manufacturer
 * @param {*} navigate
 * @param {*} logout
 * @param {*} setManufacturers
 * @param {*} setError
 * @returns
 */
const ManufacturerRow = ({
  manufacturer,
  navigate,
  logout,
  setManufacturers,
  setError,
}) => {
  return (
    <tr>
      <td>{manufacturer.name}</td>
      <td>
        <DeleteManufacturerActionIcon
          manufacturer={manufacturer}
          logout={logout}
          setManufacturers={setManufacturers}
          setError={setError}
        />
      </td>
      <td>
        <FontAwesomeIcon
          icon={faPenToSquare}
          onClick={() =>
            navigate({
              page: pages.manufacturerEditor,
              manufacturer: manufacturer,
            })
          }
        />
      </td>
    </tr>
  );
};

export default ManufacturerRow;
