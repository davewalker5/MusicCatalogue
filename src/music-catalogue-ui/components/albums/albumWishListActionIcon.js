import { useCallback } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faHeartCirclePlus,
  faRecordVinyl,
} from "@fortawesome/free-solid-svg-icons";
import {
  apiSetAlbumWishListFlag,
  apiFetchAlbumsByArtist,
} from "@/helpers/api/apiAlbums";

/**
 * Icon and associated action to move an album between the catalogue and wish list
 * @param {*} album
 * @param {*} isWishList
 * @param {*} logout
 * @param {*} setAlbums
 * @returns
 */
const AlbumWishListActionIcon = ({ album, isWishList, logout, setAlbums }) => {
  // Set the icon depending on the direction in which the album will move
  const icon = isWishList ? faRecordVinyl : faHeartCirclePlus;

  /* Callback to move an album between the wish list and catalogue */
  const setAlbumWishListFlag = useCallback(async () => {
    // Move the album to the wish list
    const result = await apiSetAlbumWishListFlag(album, !isWishList, logout);
    if (result) {
      // Successful, so refresh the album list
      const fetchedAlbums = await apiFetchAlbumsByArtist(
        album.artistId,
        isWishList,
        logout
      );
      setAlbums(fetchedAlbums);
    }
  }, [album, isWishList, logout, setAlbums]);

  return <FontAwesomeIcon icon={icon} onClick={setAlbumWishListFlag} />;
};

export default AlbumWishListActionIcon;
