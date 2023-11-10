import { useCallback } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faRecordVinyl } from "@fortawesome/free-solid-svg-icons";
import { apiSetAlbumWishListFlag, apiFetchAlbumsByArtist } from "@/helpers/api";

const AddAlbumToWishList = ({ artistId, album, logout, setAlbums }) => {
  /* Callback to move an album from the wish list into the catalogue */
  const moveAlbumToCatalogue = useCallback(async () => {
    // Move the album to the catalogue
    const result = await apiSetAlbumWishListFlag(album, false, logout);
    if (result) {
      // Successful, so refresh the album list
      const fetchedAlbums = await apiFetchAlbumsByArtist(
        artistId,
        true,
        logout
      );
      setAlbums(fetchedAlbums);
    }
  }, [album, artistId, logout, setAlbums]);

  return (
    <FontAwesomeIcon icon={faRecordVinyl} onClick={moveAlbumToCatalogue} />
  );
};

export default AddAlbumToWishList;
