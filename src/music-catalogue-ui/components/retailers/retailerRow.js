import pages from "@/helpers/navigation";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPenToSquare } from "@fortawesome/free-solid-svg-icons";
import DeleteRetailerActionIcon from "./deleteRetailerActionIcon";

/**
 * Component to render a row containing the details for a single retailer
 * @param {*} retailer
 * @param {*} navigate
 * @param {*} logout
 * @param {*} clearError
 * @param {*} setError
 * @param {*} setRetailers
 * @returns
 */
const RetailerRow = ({
  retailer,
  navigate,
  logout,
  clearError,
  setError,
  setRetailers,
}) => {
  return (
    <tr>
      <td
        onClick={() =>
          navigate({
            page: pages.retailerDetails,
            retailer: retailer,
          })
        }
      >
        {retailer.name}
      </td>
      <td
        onClick={() =>
          navigate({
            page: pages.retailerDetails,
            retailer: retailer,
          })
        }
      >
        {retailer.town}
      </td>
      <td
        onClick={() =>
          navigate({
            page: pages.retailerDetails,
            retailer: retailer,
          })
        }
      >
        {retailer.county}
      </td>
      <td
        onClick={() =>
          navigate({
            page: pages.retailerDetails,
            retailer: retailer,
          })
        }
      >
        {retailer.country}
      </td>
      <td>
        <a href={retailer.webSite} target="_blank">
          {retailer.webSite}
        </a>
      </td>
      <td>
        <DeleteRetailerActionIcon
          retailer={retailer}
          logout={logout}
          clearError={clearError}
          setError={setError}
          setRetailers={setRetailers}
        />
      </td>
      <td>
        <FontAwesomeIcon
          icon={faPenToSquare}
          onClick={() =>
            navigate({ page: pages.retailerEditor, retailer: retailer })
          }
        />
      </td>
    </tr>
  );
};

export default RetailerRow;
