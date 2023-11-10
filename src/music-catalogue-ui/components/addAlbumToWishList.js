import { useCallback } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faHeartCirclePlus } from "@fortawesome/free-solid-svg-icons";
import { apiSetAlbumWishListFlag, apiFetchAlbumsByArtist } from "@/helpers/api";

const AddAlbumToWishList = ({ artistId, album, logout, setAlbums }) => {
  /* Callback to prompt for confirmation and delete an album */
  const moveAlbumToWishList = useCallback(async () => {
    // Move the album to the wish list
    const result = await apiSetAlbumWishListFlag(album, true, logout);
    if (result) {
      // Successful, so refresh the album list
      const fetchedAlbums = await apiFetchAlbumsByArtist(
        artistId,
        false,
        logout
      );
      setAlbums(fetchedAlbums);
    }
  }, [album, artistId, logout, setAlbums]);

  return (
    <FontAwesomeIcon icon={faHeartCirclePlus} onClick={moveAlbumToWishList} />
  );
};

export default AddAlbumToWishList;
