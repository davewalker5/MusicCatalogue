import styles from "./menuBar.module.css";
import pages from "@/helpers/navigation";
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
            onClick={() => navigate({ page: pages.artists })}
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
            <a onClick={() => navigate({ page: pages.artistStatisticsReport })}>
              Artist Statistics
            </a>
            <a onClick={() => navigate({ page: pages.genreStatisticsReport })}>
              Genre Statistics
            </a>
            <a
              onClick={() => navigate({ page: pages.retailerStatisticsReport })}
            >
              Retailer Statistics
            </a>
            <a onClick={() => navigate({ page: pages.jobStatusReport })}>
              Job Status
            </a>
            <a onClick={() => navigate({ page: pages.monthlySpendReport })}>
              Monthly Spend
            </a>
          </div>
        </div>
        <a onClick={() => navigate({ page: pages.export })}>Export</a>
        <a href="#">Import</a>
        <a onClick={() => navigate({ page: pages.lookup })}>Search</a>
        <div className={styles.dropdown}>
          <button className={styles.dropbtn}>
            Equipment
            <div className={styles.dropdownArrowContainer}>
              <FontAwesomeIcon icon={faCaretDown} />
            </div>
          </button>
          <div className={styles.dropdownContent}>
            <a onClick={() => navigate({ page: pages.equipment })}>Equipment</a>
            <a>Wish List</a>
            <a onClick={() => navigate({ page: pages.equipmentTypes })}>
              Equipment Types
            </a>
            <a onClick={() => navigate({ page: pages.manufacturers })}>
              Manufacturers
            </a>
            <a onClick={() => navigate({ page: pages.retailers })}>Retailers</a>
          </div>
        </div>
        <div className={styles.dropdown}>
          <button className={styles.dropbtn}>
            Music
            <div className={styles.dropdownArrowContainer}>
              <FontAwesomeIcon icon={faCaretDown} />
            </div>
          </button>
          <div className={styles.dropdownContent}>
            <a onClick={() => navigate({ page: pages.artists, filter: "A" })}>
              Artists
            </a>
            <a onClick={() => navigate({ page: pages.genres })}>Genres</a>
            <a
              onClick={() =>
                navigate({
                  page: pages.artists,
                  filter: "A",
                  isWishList: true,
                })
              }
            >
              Wish List
            </a>
            <a onClick={() => navigate({ page: pages.retailers })}>Retailers</a>
          </div>
        </div>
      </div>
    </>
  );
};

export default MenuBar;
