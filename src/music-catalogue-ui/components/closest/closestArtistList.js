import pages from "@/helpers/navigation";
import styles from "./artistList.module.css";
import useArtists from "@/hooks/useArtists";
import ClosestArtistRow from "./closestArtistRow";
import ArtistFilterBar from "./artistFilterBar";
import { useState } from "react";
import useVocalPresences from "@/hooks/useVocalPresences";
import useEnsembleTypes from "@/hooks/useEnsembleTypes";

/**
 * Component to render a table listing all the artists in the catalogue
 * @param {*} filter
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const ClosestArtistList = ({ artist, navigate, logout }) => {
  const { artists, setArtists } = useArtists(filter, genre, isWishList, logout);
  const { vocalPresences, setVocalPresences } = useVocalPresences(logout);
  const { ensembleTypes, setEnsembleTypes } = useEnsembleTypes(logout);

  // Set the page title to reflect whether we're viewing the wish list
  let title = `Closest Artists to ${artist.name}`

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{title}</h5>
      </div>
      <div className="row">
        {error != "" ? (
          <div className={styles.artistListError}>{error}</div>
        ) : (
          <></>
        )}
      </div>
      <div className="row mb-2 pageTitle">
        <div align="center">
          <ArtistFilterBar setFilter={setFilterCallback} />
        </div>
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Name</th>
            <th>Similarity</th>
            <th>Energy</th>
            <th>Intimacy</th>
            <th>Warmth</th>
            <th>Vocals</th>
            <th>Ensemble</th>
            <th>Moods</th>
          </tr>
        </thead>
        {artists != null && (
          <tbody>
            {artists.map((a) => (
              <ClosestArtistRow
                key={a.id}
                filter={filter}
                genre={genre}
                artist={a}
                isWishList={isWishList}
                navigate={navigate}
                logout={logout}
                setArtists={setArtists}
                setError={setError}
                vocalPresences={vocalPresences}
                ensembleTypes={ensembleTypes}
              />
            ))}
          </tbody>
        )}
      </table>
      <div className={styles.artistListAddButton}>
        <button
          className="btn btn-primary"
          onClick={() =>
            navigate({
              filter: filter,
              page: pages.artistEditor,
              isWishList: isWishList,
            })
          }
        >
          Add
        </button>
      </div>
    </>
  );
};

export default ArtistList;
