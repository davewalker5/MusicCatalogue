import pages from "@/helpers/navigation";
import DeleteAlbumActionIcon from "./deleteAlbumActionIcon";
import AlbumWishListActionIcon from "./albumWishListActionIcon";

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
        <DeleteAlbumActionIcon
          artistId={artist.id}
          album={album}
          isWishList={isWishList}
          logout={logout}
          setAlbums={setAlbums}
        />
      </td>
      <td>
        <AlbumWishListActionIcon
          artistId={artist.id}
          album={album}
          isWishList={isWishList}
          logout={logout}
          setAlbums={setAlbums}
        />
      </td>
    </tr>
  );
};

export default AlbumRow;
