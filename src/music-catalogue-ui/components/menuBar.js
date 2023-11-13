import styles from "./menuBar.module.css";
import pages from "../helpers/navigation";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCaretDown } from "@fortawesome/free-solid-svg-icons";

/**
 * Component to render the menu bar
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const MenuBar = ({ navigate, logout }) => {
  return (
    <>
      <div className={styles.navbar}>
        <div className={styles.titleElement}>
          <img
            src="./logo.png"
            alt="Music Catalogue"
            className={styles.logo}
            onClick={() => navigate(pages.artists, null, null, false)}
          />
        </div>
        <div className={styles.title}>Music Catalogue</div>
        <a onClick={() => logout()}>Log Out</a>
        <div className={styles.dropdown}>
          <button className={styles.dropbtn}>
            Reports
            <div className={styles.dropdownArrowContainer}>
              <FontAwesomeIcon icon={faCaretDown} />
            </div>
          </button>
          <div className={styles.dropdownContent}>
            <a
              onClick={() =>
                navigate(pages.genreStatisticsReport, null, null, false)
              }
            >
              Genre Statistics
            </a>
            <a
              onClick={() => navigate(pages.jobStatusReport, null, null, false)}
            >
              Job Status
            </a>
          </div>
        </div>
        <a onClick={() => navigate(pages.export, null, null, false)}>Export</a>
        <a href="#">Import</a>
        <a onClick={() => navigate(pages.lookup, null, null, false)}>Search</a>
        <a onClick={() => navigate(pages.artists, null, null, true)}>
          Wish List
        </a>
        <a onClick={() => navigate(pages.artists, null, null, false)}>
          Artists
        </a>
      </div>
    </>
  );
};

export default MenuBar;
