import { useCallback } from "react";
import useArtists from "@/hooks/useArtists";
import statuses from "@/helpers/status";
import ArtistRow from "./artistRow";
import StatusIndicator from "./statusIndicator";
import ButtonBar from "./buttonBar";
import pages from "@/helpers/navigation";

const ArtistList = ({ navigate, logout }) => {
  const { artists, setArtists, currentStatus } = useArtists(logout);

  // Callback to navigate to the lookup page
  const lookup = useCallback(() => {
    navigate(pages.lookup, null, null);
  }, [navigate]);

  if (currentStatus !== statuses.loaded)
    return <StatusIndicator currentStatus={currentStatus} />;

  return (
    <>
      <div className="row mb-2">
        <h5 className="themeFontColor text-center">Artists</h5>
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Name</th>
          </tr>
        </thead>
        <tbody>
          {artists.map((a) => (
            <ArtistRow key={a.id} artist={a} navigate={navigate} />
          ))}
        </tbody>
      </table>
      <ButtonBar navigateBack={null} lookup={lookup} logout={logout} />
    </>
  );
};

export default ArtistList;
