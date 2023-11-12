import pages from "@/helpers/navigation";
import DeleteAlbumActionIcon from "./deleteAlbumActionIcon";
import AlbumWishListActionIcon from "./albumWishListActionIcon";
import CurrencyFormatter from "./currencyFormatter";
import DateFormatter from "./dateFormatter";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCoins } from "@fortawesome/free-solid-svg-icons";

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
  // Get the retailer name
  const retailer = album["retailer"];
  const retailerName = retailer != null ? retailer["name"] : "";

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
      <td onClick={() => navigate(pages.tracks, artist, album, isWishList)}>
        <DateFormatter value={album.purchased} />
      </td>
      <td onClick={() => navigate(pages.tracks, artist, album, isWishList)}>
        <CurrencyFormatter value={album.price} renderZeroAsBlank={true} />
      </td>
      <td onClick={() => navigate(pages.tracks, artist, album, isWishList)}>
        {retailerName}
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
      <td>
        <FontAwesomeIcon
          icon={faCoins}
          onClick={() =>
            navigate(pages.albumPurchaseDetails, artist, album, false)
          }
        />
      </td>
    </tr>
  );
};

export default AlbumRow;
