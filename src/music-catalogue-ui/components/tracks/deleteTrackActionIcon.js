import { apiDeleteTrack } from "@/helpers/api/apiTracks";
import { apiFetchAlbumById } from "@/helpers/api/apiAlbums";
import { useCallback } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrashAlt } from "@fortawesome/free-solid-svg-icons";
import { Tooltip } from "react-tooltip";

/**
 * Icon and associated action to delete a track
 * @param {*} track
 * @param {*} logout
 * @param {*} setTracks
 * @param {*} setError
 * @returns
 */
const DeleteTrackActionIcon = ({ track, logout, setTracks, setError }) => {
  /* Callback to prompt for confirmation and delete an album */
  const confirmDeleteTrack = useCallback(async (e, track) => {
    // Prevent the default action associated with the click event
    e.preventDefault();

    // Show a confirmation message and get the user response
    const message = `This will delete the track "${track.title}" - click OK to confirm`;
    const result = confirm(message);

    // If they've confirmed the deletion ...
    if (result) {
      try {
        // ... cdelete the track and confirm the API call was successful
        const result = await apiDeleteTrack(track.id, logout);
        if (result) {
          // Successful, so get the album from the service and set the tracks to
          // the appropriate member of the returned object
          var fetchedAlbum = await apiFetchAlbumById(track.albumId, logout);
          setTracks(fetchedAlbum.tracks);
        }
      } catch (ex) {
        setError(`Error deleting the track: ${ex.message}`);
      }
    }
  }, [logout, setTracks, setError]);

  return (
    <>
      <FontAwesomeIcon
        icon={faTrashAlt}
        data-tooltip-id="delete-tooltip"
        data-tooltip-content="Delete track"
        onClick={(e) => confirmDeleteTrack(e, track)}
      />

      <Tooltip id="delete-tooltip" />
    </>
  );
};

export default DeleteTrackActionIcon;
