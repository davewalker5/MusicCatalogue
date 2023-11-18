import pages from "@/helpers/navigation";

/**
 * Component to render a row containing the details for a single genre
 * @param {*} genre
 * @param {*} navigate
 * @returns
 */
const GenreRow = ({ genre, navigate }) => {
  return (
    <tr>
      <td onClick={() => navigate({ page: pages.artists, genre: genre })}>
        {genre.name}
      </td>
    </tr>
  );
};

export default GenreRow;
