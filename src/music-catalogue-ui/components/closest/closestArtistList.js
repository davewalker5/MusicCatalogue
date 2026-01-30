import config from "@/config.json";
import useClosestArtists from "@/hooks/useClosestArtists";
import ClosestArtistRow from "./closestArtistRow";
import useVocalPresences from "@/hooks/useVocalPresences";
import useEnsembleTypes from "@/hooks/useEnsembleTypes";

/**
 * Component to render a table listing all the artists in the catalogue
 * @param {*} artist
 * @param {*} filter
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const ClosestArtistList = ({ artist, filter, isWishList, navigate, logout }) => {
  const { closestArtists, setClosestArtists } = useClosestArtists(
      artist.id,
      config.matching.numberOfMatches,
      config.matching.energyWeight,
      config.matching.intimacyWeight,
      config.matching.warmthWeight,
      config.matching.moodWeight,
      logout);
  const { vocalPresences, setVocalPresences } = useVocalPresences(logout);
  const { ensembleTypes, setEnsembleTypes } = useEnsembleTypes(logout);

  // Set the page title to reflect whether we're viewing the wish list
  let title = `Closest Artists to ${artist.name}`

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{title}</h5>
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
        {closestArtists != null && (
          <tbody>
            {closestArtists.map((ca) => (
              <ClosestArtistRow
                key={ca.artist.id}
                artist={ca.artist}
                similarity={ca.similarity}
                filter={filter}
                isWishList={isWishList}
                navigate={navigate}
                vocalPresences={vocalPresences}
                ensembleTypes={ensembleTypes}
              />
            ))}
          </tbody>
        )}
      </table>
    </>
  );
};

export default ClosestArtistList;
