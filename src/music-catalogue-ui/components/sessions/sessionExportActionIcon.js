import { useCallback } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faFileExport } from "@fortawesome/free-solid-svg-icons";
import { apiExportSession } from "@/helpers/api/apiPlaylist";
import { Tooltip } from "react-tooltip";

/**
 * Icon and associated action to export a session
 * @param {*} session
 * @param {*} setMessage
 * @param {*} setError
 * @param {*} logout
 * @returns
 */
const SessionExportActionIcon = ({ session, setMessage, setError, logout }) => {
    // Callback to request an export via the API
    const exportSession = useCallback(
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
      <FontAwesomeIcon
        icon={faFileExport}
        data-tooltip-id="export-tooltip"
        data-tooltip-content="Export session"
        onClick={exportSession}
      />

      <Tooltip id="export-tooltip" />
    </>
  );
};

export default SessionExportActionIcon;
