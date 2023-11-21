import useRetailers from "@/hooks/useRetailers";
import RetailerRow from "./retailerRow";

/**
 * Component to render a table listing all the retailers in the catalogue
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const RetailerList = ({ navigate, logout }) => {
  const { retailers: retailers, setRetailers } = useRetailers(logout);

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Retailers</h5>
      </div>
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
              <RetailerRow key={r.id} retailer={r} navigate={navigate} />
            ))}
          </tbody>
        )}
      </table>
    </>
  );
};

export default RetailerList;
