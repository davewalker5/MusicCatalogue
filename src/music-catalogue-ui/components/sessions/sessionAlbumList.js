import SessionAlbumRow from "./sessionAlbumRow";

/**
 * Component to render the ordered list of albums in a saved session
 * @param {*} session 
 * @returns 
 */
const SessionAlbumList = ({ session }) => {
    return (
        <table className="table table-hover">
        <thead>
            <tr>
                <th>No.</th>
                <th>Artist</th>
                <th>Album</th>
                <th>Duration</th>
                <th>Tracks</th>
                <th>Genre</th>
                <th>Released</th>
                <th>Purchased</th>
                <th>Price</th>
                <th>Retailer</th>
            </tr>
        </thead>
        <tbody>
            {session.sessionAlbums.map((a) => (
                <SessionAlbumRow
                    key={a.id}
                    id={a.id}
                    sessionAlbum={a}
                />
            ))}
        </tbody>
    </table>
    );
};

export default SessionAlbumList;
