/**
 * Component to render a row containing the details of a single album that's part of a saved session
 * @param {*} sessionAlbum
 * @returns
 */
const SessionTrackRow = ({ track, album, artist, index }) => {
  return (
    <tr>
      <td>{index}</td>
      <td>{artist.name}</td>
      <td>{album.title}</td>
      <td>{track.title}</td>
      <td>{track.formattedDuration}</td>
    </tr>
  );
};

export default SessionTrackRow;
