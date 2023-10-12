import { useCallback } from "react";
import useArtists from "@/hooks/useArtists";
import ArtistRow from "./artistRow";
import pages from "@/helpers/navigation";

/**
 * Component to render a table listing all the artists in the catalogue
 * @param {*} param0
 * @returns
 */
const ArtistList = ({ navigate, logout }) => {
  const { artists, setArtists } = useArtists(logout);

  // Callback to navigate to the lookup page
  const lookup = useCallback(() => {
    navigate(pages.lookup, null, null);
  }, [navigate]);

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Artists</h5>
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Name</th>
            <th>Albums</th>
            <th>Tracks</th>
          </tr>
        </thead>
        <tbody>
          {artists.map((a) => (
            <ArtistRow key={a.id} artist={a} navigate={navigate} />
          ))}
        </tbody>
      </table>
    </>
  );
};

export default ArtistList;
