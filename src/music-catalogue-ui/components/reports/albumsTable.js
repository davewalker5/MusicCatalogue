import AlbumRow from "./albumRow";

/**
 * Component to render a table containing the details for a set of albums
 * @param {*} records
 * @returns
 */
const AlbumsTable = ({ records }) => {
  return (
    <table className="table table-hover">
      <thead>
        <tr>
          <th>Artist</th>
          <th>Title</th>
          <th>Genre</th>
          <th>Released</th>
          <th>Purchased</th>
          <th>Price</th>
          <th>Retailer</th>
        </tr>
      </thead>
      {records != null && (
        <tbody>
          {records.map((r) => (
            <AlbumRow key={r.id} record={r} />
          ))}
        </tbody>
      )}
    </table>
  );
};

export default AlbumsTable;
