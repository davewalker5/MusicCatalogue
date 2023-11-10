import styles from "./trackRow.module.css";
import pages from "@/helpers/navigation";

/**
 * Component to render a row containing the details for a single track
 * @param {*} param0
 * @returns
 */
const TrackRow = ({ artist, album, track, isWishList, navigate }) => {
  return (
    <tr>
      <td>{album.title}</td>
      <td
        className={styles.artist}
        onClick={() => navigate(pages.albums, artist, null, isWishList)}
      >
        {artist.name}
      </td>
      <td>{track.number}</td>
      <td>{track.title}</td>
      <td>{track.formattedDuration}</td>
    </tr>
  );
};

export default TrackRow;
