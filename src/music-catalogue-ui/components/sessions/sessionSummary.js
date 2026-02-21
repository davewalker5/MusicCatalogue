import FormattedDate from "../common/formattedDate";

/**
 * Component to tabulate summary details for a saved session
 * @param {*} session
 * @returns
 */
const SessionSummary = ({ session }) => {
  return (
    <table className="table table-hover">
        <thead>
            <tr>
                <th>Created</th>
                <th>Type</th>
                <th>Time Of Day</th>
                <th>Albums</th>
                <th>Playing Time</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td><FormattedDate value={session.createdAt} /></td>
                <td>{session.playlistTypeName}</td>
                <td>{session.timeOfDayName}</td>
                <td>{session.sessionAlbums.length}</td>
                <td>{session.formattedPlayingTime}</td>
            </tr>
        </tbody>
    </table>
  );
};

export default SessionSummary;
