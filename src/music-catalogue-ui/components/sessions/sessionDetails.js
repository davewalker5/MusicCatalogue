import SessionAlbumList from "./sessionAlbumList";
import SessionSummary from "./sessionSummary";
import SessionTrackList from "./sessionTrackList";

/**
 * Component to display the summary, album and track details for a saved session
 * @param {*} session
 * @returns
 */
const SessionDetails = ({ session }) => {
    return (
        <>
            <div className="row mb-2 pageTitle">
            <   h5 className="themeFontColor text-center">Session Number {session.id}</h5>
            </div>
            <div className="row mb-2 pageTitle">
                <h6 className="themeFontColor text-center">Session Summary</h6>
            </div>
            <SessionSummary session={session} />
            <div className="row mb-2 pageTitle">
                <h6 className="themeFontColor text-center">Album List</h6>
            </div>
            <SessionAlbumList session={session} />
            <div className="row mb-2 pageTitle">
                <h6 className="themeFontColor text-center">Track List</h6>
            </div>
            <SessionTrackList session={session} />
        </>
    );
};

export default SessionDetails;
