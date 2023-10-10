const TrackRow = ({ artist, album, track }) => {
  return (
    <tr>
      <td>{album.title}</td>
      <td>{artist.name}</td>
      <td>{track.number}</td>
      <td>{track.title}</td>
      <td>{track.duration}</td>
    </tr>
  );
};

export default TrackRow;
