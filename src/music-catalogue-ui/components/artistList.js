import styles from "./artistList.module.css";
import useArtists from "@/hooks/useArtists";
import ArtistRow from "./artistRow";
import ArtistFilterBar from "./artistFilterBar";

/**
 * Component to render a table listing all the artists in the catalogue
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const ArtistList = ({ filter, isWishList, navigate, logout }) => {
  const { artists, setArtists } = useArtists(filter, isWishList, logout);

  // Callback to pass to child components to set the artist list
  const setArtistsCallback = (artists) => {
    setArtists(artists);
  };

  // Set the page title to reflect whether we're viewing the wish list
  const title = isWishList ? "Wish List Artists" : "Artists";

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{title}</h5>
      </div>
      <div className="row mb-2 pageTitle">
        <div align="center">
          <ArtistFilterBar
            isWishList={isWishList}
            logout={logout}
            setArtists={setArtistsCallback}
          />
        </div>
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Name</th>
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
