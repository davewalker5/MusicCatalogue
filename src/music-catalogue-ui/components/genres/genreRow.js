/**
 * Component to render a row containing the details for a single genre
 * @param {*} genre
 * @param {*} setGenre
 * @returns
 */
const GenreRow = ({ genre, setGenre }) => {
  return (
    <tr>
      <td onClick={() => setGenre(genre)}>{genre.name}</td>
    </tr>
  );
};

export default GenreRow;
