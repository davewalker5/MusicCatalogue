import pages from "@/helpers/navigation";
import DeleteAlbumActionIcon from "./deleteAlbumActionIcon";
import AlbumWishListActionIcon from "./albumWishListActionIcon";
import CurrencyFormatter from "./currencyFormatter";
import DateFormatter from "./dateFormatter";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCoins } from "@fortawesome/free-solid-svg-icons";
import { useCallback } from "react";

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

  // Get the genre
  const genre = album["genre"];
  const genreName = genre != null ? genre["name"] : "";

  // Callback for click events on the row (excluding the action icons)
  const rowClickCallback = useCallback(() => {
    navigate({
      page: pages.tracks,
      artist: artist,
      album: album,
      isWishList: isWishList,
    });
  }, [artist, album, isWishList, navigate]);

  return (
    <tr>
      <td onClick={rowClickCallback}>{artist.name}</td>
      <td onClick={rowClickCallback}>{album.title}</td>
      <td onClick={rowClickCallback}>{genreName}</td>
      <td onClick={rowClickCallback}>{album.released}</td>
      <td onClick={rowClickCallback}>
        <DateFormatter value={album.purchased} />
      </td>
      <td onClick={rowClickCallback}>
        <CurrencyFormatter value={album.price} renderZeroAsBlank={true} />
      </td>
      <td onClick={rowClickCallback}>{retailerName}</td>
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
            navigate({
              page: pages.albumPurchaseDetails,
              artist: artist,
              album: album,
            })
          }
        />
      </td>
    </tr>
  );
};

export default AlbumRow;
