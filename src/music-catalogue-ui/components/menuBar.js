import styles from "./menuBar.module.css";
import pages from "../helpers/navigation";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCaretDown } from "@fortawesome/free-solid-svg-icons";

const MenuBar = ({ navigate, logout }) => {
  return (
    <>
      <div className={styles.navbar}>
        <div className={styles.titleElement}>
          <img
            src="./logo.png"
            alt="Music Catalogue"
            className={styles.logo}
            onClick={() => navigate(pages.artists, null, null)}
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
            <a onClick={() => navigate(pages.jobStatusReport, null, null)}>
              Job Status
            </a>
          </div>
        </div>
        <a onClick={() => navigate(pages.export, null, null)}>Export</a>
        <a href="#">Import</a>
        <a onClick={() => navigate(pages.lookup, null, null)}>Search</a>
        <a onClick={() => navigate(pages.artists, null, null)}>Artists</a>
      </div>
    </>
  );
};

export default MenuBar;
