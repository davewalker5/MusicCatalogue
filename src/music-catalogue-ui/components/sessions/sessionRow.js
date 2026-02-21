import pages from "@/helpers/navigation";
import DateFormatter from "../common/dateFormatter";
import { useCallback } from "react";

/**
 * Component to render a session row containing the top-level details of a single session
 * @param {*} session
 * @param {*} navigate
 * @returns
 */
const SessionRow = ({
  session,
  navigate
}) => {
  // Callback for click events on the row
  const rowClickCallback = useCallback(() => {
    navigate({
      page: pages.savedSessionDetails,
      session: session
    });
  }, [session, navigate]);

  return (
    <tr>
      <td onClick={rowClickCallback}>
        <DateFormatter value={session.createdAt} />
      </td>
      <td onClick={rowClickCallback}>{session.playlistTypeName}</td>
      <td onClick={rowClickCallback}>{session.timeOfDayName}</td>
      <td onClick={rowClickCallback}>{session.artists}</td>
      <td onClick={rowClickCallback}>{session.sessionAlbums.length}</td>
    </tr>
  );
};

export default SessionRow;
