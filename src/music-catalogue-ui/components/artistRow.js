import pages from "@/helpers/navigation";

const ArtistRow = ({ artist, navigate }) => {
  return (
    <tr onClick={() => navigate(pages.albums, artist, null)}>
      <td>{artist.name}</td>
    </tr>
  );
};

export default ArtistRow;
