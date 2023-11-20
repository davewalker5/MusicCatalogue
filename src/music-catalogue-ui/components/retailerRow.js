import pages from "@/helpers/navigation";

/**
 * Component to render a row containing the details for a single retailer
 * @param {*} mapsApiKey
 * @param {*} artist
 * @param {*} navigate
 * @returns
 */
const RetailerRow = ({ mapsApiKey, retailer, navigate }) => {
  return (
    <tr>
      <td
        onClick={() =>
          navigate({
            page: pages.retailerDetails,
            retailer: retailer,
            mapsApiKey: mapsApiKey,
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
            mapsApiKey: mapsApiKey,
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
            mapsApiKey: mapsApiKey,
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
            mapsApiKey: mapsApiKey,
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
    </tr>
  );
};

export default RetailerRow;
