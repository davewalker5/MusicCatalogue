import styles from "./menuBar.module.css";
import pages from "../helpers/navigation";

const MenuBar = ({ navigate, logout }) => {
  return (
    <div className={styles.titleBarContainer}>
      <div className={styles.titleElement}>
        <img
          src="./logo.png"
          alt="Music Catalogue"
          className={styles.logo}
          onClick={() => navigate(pages.artists, null, null)}
        />
      </div>
      <div className={styles.title}>Music Catalogue</div>
      <div className={styles.menuContainer}>
        <span className={styles.menuItem} onClick={() => logout()}>
          Log Out
        </span>
        <span className={styles.menuItem}>Export</span>
        <span className={styles.menuItem}>Import</span>
        <span
          className={styles.menuItem}
          onClick={() => navigate(pages.lookup, null, null)}
        >
          Search
        </span>
        <span
          className={styles.menuItem}
          onClick={() => navigate(pages.artists, null, null)}
        >
          Artists
        </span>
      </div>
    </div>
  );
};

export default MenuBar;
