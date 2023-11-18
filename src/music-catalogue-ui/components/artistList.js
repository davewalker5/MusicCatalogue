import pages from "../helpers/navigation";
import useArtists from "@/hooks/useArtists";
import ArtistRow from "./artistRow";
import ArtistFilterBar from "./artistFilterBar";

/**
 * Component to render a table listing all the artists in the catalogue
 * @param {*} filter
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const ArtistList = ({ filter, genre, isWishList, navigate, logout }) => {
  const { artists, setArtists } = useArtists(filter, genre, isWishList, logout);

  // Callback to pass to child components to set the artist list
  const setFilterCallback = (updatedFilter) => {
    navigate({
      page: pages.artists,
      filter: updatedFilter,
      genre: genre,
      isWishList: isWishList,
    });
  };

  // Set the page title to reflect whether we're viewing the wish list
  let title = isWishList ? "Wish List Artists" : "Artists";
  if (genre != null) {
    title = `${title} - ${genre.name}`;
  }

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{title}</h5>
      </div>
      <div className="row mb-2 pageTitle">
        <div align="center">
          <ArtistFilterBar setFilter={setFilterCallback} />
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
