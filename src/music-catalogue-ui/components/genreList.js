import pages from "../helpers/navigation";
import useGenres from "@/hooks/useGenres";
import GenreRow from "./genreRow";

/**
 * Component to render a table listing all the genres in the catalogue
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const GenreList = ({ navigate, logout }) => {
  const { genres, setGenres } = useGenres(false, logout);

  // Callback to pass to child components to set the genre
  const setGenreCallback = (genre) => {
    navigate({
      page: pages.artists,
      genre: genre,
      filter: "*",
    });
  };

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Genres</h5>
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Name</th>
          </tr>
        </thead>
        {genres != null && (
          <tbody>
            {genres.map((g) => (
              <GenreRow key={g.id} genre={g} setGenre={setGenreCallback} />
            ))}
          </tbody>
        )}
      </table>
    </>
  );
};

export default GenreList;
