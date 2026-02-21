import SessionAlbumList from "./sessionAlbumList";
import SessionSummary from "./sessionSummary";
import SessionTrackList from "./sessionTrackList";
import styles from "./sessionDetails.module.css";
import { useCallback, useState } from "react";
import { apiExportSession } from "@/helpers/api/apiPlaylist";

/**
 * Component to display the summary, album and track details for a saved session
 * @param {*} session
 * @returns
 */
const SessionDetails = ({ session, logout }) => {
    const [message, setMessage] = useState("");
    const [error, setError] = useState("");

    // Callback to request an export via the API
    const exportCallback = useCallback(
        async (e) => {
            // Prevent the default action associated with the click event
            e.preventDefault();

            // Clear pre-existing messages and errors
            setMessage("");
            setError("");

            // Construct the file name
            const fileName = `${session.createdAt.split('T')[0]} ${String(session.id).padStart(4, '0')}.csv`;

            // Request an export via the API
            const isOK = await apiExportSession(session.id, fileName, logout);

            // Display a confirmation message. Otherwise, show an error
            if (isOK) {
                setMessage(`Export of session ${session.id} to ${fileName} has been requested`);
            } else {
                setError(`An error occurred requesting export of the session`);
            }

            // With a long playlist, the page can be quite long so scroll to the top so the user sees the confirmation
            // message
            window.scrollTo({ top: 0, behavior: "smooth" });
        },
        [session, logout]
    );

    return (
        <>
            <div className="row mb-2 pageTitle">
            <   h5 className="themeFontColor text-center">Session Number {session.id}</h5>
            </div>
            <div className="row">
                {message != "" ? (
                    <div className={styles.sessionDetailsMessage}>{message}</div>
                ) : (
                    <></>
                )}
                {error != "" ? (
                    <div className={styles.sessionDetailsError}>{error}</div>
                ) : (
                    <></>
                )}
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
            <div className={styles.sessionDetailsSaveButton}>
                <button className="btn btn-primary" onClick={(e) => exportCallback(e)}>
                    Export
                </button>
            </div>
        </>
    );
};

export default SessionDetails;
