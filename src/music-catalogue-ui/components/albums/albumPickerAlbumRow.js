import pages from "@/helpers/navigation";
import CurrencyFormatter from "../common/currencyFormatter";
import DateFormatter from "../common/dateFormatter";
import React, { useCallback } from "react";

/**
 * Component to render a row containing the details of a single album in a
 * genre
 * @param {*} album
 * @param {*} artist
 * @returns
 */
const AlbumPickerAlbumRow = ({ match, navigate }) => {
  const album = match.album;
  const artist = match.album.artist;
  const purchaseDate = new Date(album.purchased);

  // Callback for click events on the row (excluding the action icons)
  const rowClickCallback = useCallback(() => {
    navigate({
      page: pages.tracks,
      artist: match.album.artist,
      album: match.album
    });
  }, [match, navigate]);

  return (
    <tr>
      <td onClick={rowClickCallback}>{match.similarity}</td>
      <td onClick={rowClickCallback}>{artist.name}</td>
      <td onClick={rowClickCallback}>{album.title}</td>
      <td onClick={rowClickCallback}>{album.genre.name}</td>
      {album.released > 0 ? <td onClick={rowClickCallback}>{album.released}</td> : <td onClick={rowClickCallback} />}
      {purchaseDate > 1900 ? (
        <td onClick={rowClickCallback} >
          <DateFormatter value={album.purchased} />
        </td>
      ) : (
        <td onClick={rowClickCallback} />
      )}
      {album.price > 0 ? (
        <td onClick={rowClickCallback}>
          <CurrencyFormatter value={album.price} />
        </td>
      ) : (
        <td onClick={rowClickCallback}/>
      )}
      {album.retailer != null ? <td onClick={rowClickCallback}>{album.retailer.name}</td> : <td onClick={rowClickCallback} />}
    </tr>
  );
};

export default AlbumPickerAlbumRow;
