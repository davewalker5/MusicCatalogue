import { useContext } from "react";
import { navigationContext } from "./app";
import pages from "@/helpers/navigation";

const ArtistRow = ({ artist }) => {
  const { navigate } = useContext(navigationContext);

  return (
    <tr onClick={() => navigate(pages.albums, artist)}>
      <td>{artist.name}</td>
    </tr>
  );
};

export default ArtistRow;
