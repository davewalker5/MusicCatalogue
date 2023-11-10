import pages from "@/helpers/navigation";
import DeleteAlbum from "./deleteAlbum";
import AddAlbumToWishList from "./addAlbumToWishList";
import AddAlbumToCatalogue from "./addAlbumToCatalogue";

/**
 * Component to render a row containing the details of a single album
 * @param {*} artist
 * @param {*} album
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} logout
 * @param {*} setAlbums
 * @returns
 */
const AlbumRow = ({
  artist,
  album,
  isWishList,
  navigate,
  logout,
  setAlbums,
}) => {
  return (
    <tr>
      <td onClick={() => navigate(pages.tracks, artist, album, isWishList)}>
        {artist.name}
      </td>
      <td onClick={() => navigate(pages.tracks, artist, album, isWishList)}>
        {album.title}
      </td>
      <td onClick={() => navigate(pages.tracks, artist, album, isWishList)}>
        {album.genre}
      </td>
      <td onClick={() => navigate(pages.tracks, artist, album, isWishList)}>
        {album.released}
      </td>
      <td>
        <DeleteAlbum
          album={album}
          isWishList={isWishList}
          logout={logout}
          setAlbums={setAlbums}
        />
      </td>
      <td>
        {isWishList == false ? (
          <AddAlbumToWishList
            artistId={artist.id}
            album={album}
            logout={logout}
            setAlbums={setAlbums}
          />
        ) : (
          <AddAlbumToCatalogue
            artistId={artist.id}
            album={album}
            logout={logout}
            setAlbums={setAlbums}
          />
        )}
      </td>
    </tr>
  );
};

export default AlbumRow;
