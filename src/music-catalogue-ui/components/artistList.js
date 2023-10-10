import useArtists from "@/hooks/useArtists";
import statuses from "@/helpers/status";
import ArtistRow from "./artistRow";
import StatusIndicator from "./statusIndicator";
import ButtonBar from "./buttonBar";

const ArtistList = ({ navigate, logout }) => {
  const { artists, setArtists, currentStatus } = useArtists();

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
      <ButtonBar navigateBack={null} logout={logout} />
    </>
  );
};

export default ArtistList;
