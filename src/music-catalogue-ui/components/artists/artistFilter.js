import styles from "./artistFilter.module.css";

const ArtistFilter = ({ label, separator, filter, setFilter }) => {
  return (
    <>
      {separator != "" ? <span>{separator}</span> : <></>}
      <span
        className={styles.artistFilterLabel}
        onClick={() => setFilter(filter)}
      >
        {label}
      </span>
    </>
  );
};

export default ArtistFilter;
