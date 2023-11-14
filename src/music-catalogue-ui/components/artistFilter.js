import { useCallback } from "react";
import styles from "./artistFilter.module.css";
import { apiFetchArtists } from "../helpers/apiArtists";

const ArtistFilter = ({
  label,
  separator,
  filter,
  isWishList,
  setArtists,
  logout,
}) => {
  /* Callback to filter the artist list by this filter */
  const filterArtists = useCallback(async () => {
    try {
      // Get a list of artists matching the specified criteria
      var fetchedArtists = await apiFetchArtists(filter, isWishList, logout);
      setArtists(fetchedArtists);
    } catch {}
  }, [filter, isWishList, logout, setArtists]);

  return (
    <>
      {separator != "" ? <span>{separator}</span> : <></>}
      <span className={styles.artistFilterLabel} onClick={filterArtists}>
        {label}
      </span>
    </>
  );
};

export default ArtistFilter;
