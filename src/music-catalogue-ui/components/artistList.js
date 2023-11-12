import useArtists from "@/hooks/useArtists";
import ArtistRow from "./artistRow";

/**
 * Component to render a table listing all the artists in the catalogue
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const ArtistList = ({ isWishList, navigate, logout }) => {
  const { artists, setArtists } = useArtists(isWishList, logout);

  // Set the page title to reflect whether we're viewing the wish list
  const title = isWishList ? "Wish List Artists" : "Artists";

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{title}</h5>
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Name</th>
            <th>Albums</th>
            <th>Tracks</th>
            <th>Total Spend</th>
          </tr>
        </thead>
        {artists != null && (
          <tbody>
            {artists.map((a) => (
              <ArtistRow
                key={a.id}
                artist={a}
                isWishList={isWishList}
                navigate={navigate}
              />
            ))}
          </tbody>
        )}
      </table>
    </>
  );
};

export default ArtistList;
