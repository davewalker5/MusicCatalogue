import pages from "@/helpers/navigation";

/**
 * Component to render a row containing the details for a single retailer
 * @param {*} artist
 * @param {*} navigate
 * @returns
 */
const RetailerRow = ({ retailer, navigate }) => {
  return (
    <tr>
      <td
        onClick={() =>
          navigate({ page: pages.retailerDetails, retailer: retailer })
        }
      >
        {retailer.name}
      </td>
      <td
        onClick={() =>
          navigate({ page: pages.retailerDetails, retailer: retailer })
        }
      >
        {retailer.town}
      </td>
      <td
        onClick={() =>
          navigate({ page: pages.retailerDetails, retailer: retailer })
        }
      >
        {retailer.county}
      </td>
      <td
        onClick={() =>
          navigate({ page: pages.retailerDetails, retailer: retailer })
        }
      >
        {retailer.country}
      </td>
      <td>
        <a href={retailer.webSite} target="_blank">
          {retailer.webSite}
        </a>
      </td>
    </tr>
  );
};

export default RetailerRow;
