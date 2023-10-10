import styles from "./trackRow.module.css";
import pages from "@/helpers/navigation";

const TrackRow = ({ artist, album, track, navigate }) => {
  return (
    <tr>
      <td>{album.title}</td>
      <td
        className={styles.artist}
        onClick={() => navigate(pages.albums, artist, null)}
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
