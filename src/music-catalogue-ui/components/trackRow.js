import DeleteTrackActionIcon from "./deleteTrackActionIcon";
import styles from "./trackRow.module.css";
import pages from "@/helpers/navigation";

/**
 * Component to render a row containing the details for a single track
 * @param {*} param0
 * @returns
 */
const TrackRow = ({
  artist,
  album,
  track,
  isWishList,
  navigate,
  logout,
  setTracks,
}) => {
  return (
    <tr>
      <td>{album.title}</td>
      <td
        className={styles.artist}
        onClick={() =>
          navigate({
            page: pages.albums,
            artist: artist,
            isWishList: isWishList,
          })
        }
      >
        {artist.name}
      </td>
      <td>{track.number}</td>
      <td>{track.title}</td>
      <td>{track.formattedDuration}</td>
      <td>
        <DeleteTrackActionIcon
          track={track}
          logout={logout}
          setTracks={setTracks}
        />
      </td>
    </tr>
  );
};

export default TrackRow;
