import ArtistFilter from "./artistFilter";

const ArtistFilterBar = ({ isWishList, logout, setArtists }) => {
  // Construct the filtering , starting with "All"
  let options = [{ id: 0, label: "All", filter: "*", separator: "" }];

  // Add the alphabet filters
  const alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
  for (let i = 0; i < alphabet.length; i++) {
    options = [
      ...options,
      { id: i + 1, label: alphabet[i], filter: alphabet[i], separator: "|" },
    ];
  }

  return (
    <>
      {options.map((o) => (
        <ArtistFilter
          key={o.id}
          label={o.label}
          separator={o.separator}
          filter={o.filter}
          isWishList={isWishList}
          setArtists={setArtists}
          logout={logout}
        />
      ))}
    </>
  );
};

export default ArtistFilterBar;
