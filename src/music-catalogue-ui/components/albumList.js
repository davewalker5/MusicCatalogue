import useAlbums from "@/hooks/useAlbums";
import AlbumRow from "./albumRow";

/**
 * Component to render the table of all albums by the specified artist
 * @param {*} artist
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const AlbumList = ({ artist, isWishList, navigate, logout }) => {
  const { albums, setAlbums } = useAlbums(artist.id, isWishList, logout);

  // Set the page title to reflect whether we're viewing the wish list
  const title = isWishList
    ? `Wish List for ${artist.name}`
    : `Albums by ${artist.name}`;

  // Callback to pass to child components to set the album list
  const setAlbumsCallback = (albums) => {
    setAlbums(albums);
  };

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{title}</h5>
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Artist</th>
            <th>Album Title</th>
            <th>Genre</th>
            <th>Released</th>
            <th>Purchased</th>
            <th>Price</th>
            <th>Retailer</th>
            <th />
            <th />
            <th />
          </tr>
        </thead>
        <tbody>
          {(albums ?? []).map((a) => (
            <AlbumRow
              key={a.id}
              id={a.id}
              artist={artist}
              album={a}
              isWishList={isWishList}
              navigate={navigate}
              logout={logout}
              setAlbums={setAlbumsCallback}
            />
          ))}
        </tbody>
      </table>
    </>
  );
};

export default AlbumList;
