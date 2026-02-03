import pages from "@/helpers/navigation";
import CurrencyFormatter from "../common/currencyFormatter";
import DateFormatter from "../common/dateFormatter";
import { useCallback } from "react";

/**
 * Component to render a playlist row containing the details of a single album
 * @param {*} artist
 * @param {*} album
 * @param {*} navigate
 * @returns
 */
const PlaylistAlbumRow = ({
  artist,
  album,
  navigate
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
    });
  }, [artist, album, navigate]);

  return (
    <tr>
      <td onClick={rowClickCallback}>{artist.name}</td>
      <td onClick={rowClickCallback}>{album.title}</td>
      <td onClick={rowClickCallback}>{album.formattedPlayingTime}</td>
      <td onClick={rowClickCallback}>{genreName}</td>
      <td onClick={rowClickCallback}>{album.released}</td>
      <td onClick={rowClickCallback}>
        <DateFormatter value={album.purchased} />
      </td>
      <td onClick={rowClickCallback}>
        <CurrencyFormatter value={album.price} renderZeroAsBlank={true} />
      </td>
      <td onClick={rowClickCallback}>{retailerName}</td>
    </tr>
  );
};

export default PlaylistAlbumRow;
