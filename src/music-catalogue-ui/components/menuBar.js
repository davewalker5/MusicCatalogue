import styles from "./menuBar.module.css";
import pages from "@/helpers/navigation";
import Image from "next/image";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCaretDown } from "@fortawesome/free-solid-svg-icons";

const MenuBar = ({ navigate, logout }) => (
  <header className={styles.header}>
    <nav className={styles.navbar} aria-label="Main navigation">
      <button
        className={styles.brand}
        onClick={() => navigate({ page: pages.artists })}
        aria-label="Music Catalogue home"
      >
        <span className={styles.brandMark}>
          <Image src="/logo.png" alt="" width={32} height={32} className={styles.logo} />
        </span>
        <span className={styles.brandCopy}>
          <strong>Music Catalogue</strong>
          <small>Personal music library</small>
        </span>
      </button>
      <div className={styles.links}>
        <NavMenu label="Music">
          <a onClick={() => navigate({ page: pages.artists, filter: "A" })}>Artists</a>
          <a onClick={() => navigate({ page: pages.playlistBuilder })}>Playlist Builder</a>
          <a onClick={() => navigate({ page: pages.savedSessions })}>Saved Sessions</a>
          <a onClick={() => navigate({ page: pages.genres })}>Genres</a>
          <a onClick={() => navigate({ page: pages.moods })}>Moods</a>
          <a onClick={() => navigate({ page: pages.artists, filter: "A", isWishList: true })}>Wish List</a>
          <a onClick={() => navigate({ page: pages.retailers })}>Retailers</a>
        </NavMenu>
        <NavMenu label="Equipment">
          <a onClick={() => navigate({ page: pages.equipment, isWishList: false })}>Equipment</a>
          <a onClick={() => navigate({ page: pages.equipment, isWishList: true })}>Wish List</a>
          <a onClick={() => navigate({ page: pages.equipmentTypes })}>Equipment Types</a>
          <a onClick={() => navigate({ page: pages.manufacturers })}>Manufacturers</a>
          <a onClick={() => navigate({ page: pages.retailers })}>Retailers</a>
        </NavMenu>
        <a onClick={() => navigate({ page: pages.lookup })}>Search</a>
        <a onClick={() => navigate({ page: pages.export })}>Export</a>
        <NavMenu label="Reports" alignRight>
          <a onClick={() => navigate({ page: pages.genreAlbumsReport })}>Albums By Genre</a>
          <a onClick={() => navigate({ page: pages.albumsByPurchaseDateReport })}>Albums By Purchase Date</a>
          <a onClick={() => navigate({ page: pages.artistStatisticsReport })}>Artist Statistics</a>
          <a onClick={() => navigate({ page: pages.genreStatisticsReport })}>Genre Statistics</a>
          <a onClick={() => navigate({ page: pages.jobStatusReport })}>Job Status</a>
          <a onClick={() => navigate({ page: pages.monthlySpendReport })}>Monthly Spend</a>
          <a onClick={() => navigate({ page: pages.retailerStatisticsReport })}>Retailer Statistics</a>
        </NavMenu>
        <a onClick={() => logout()}>Log Out</a>
      </div>
    </nav>
  </header>
);

const NavMenu = ({ label, alignRight = false, children }) => (
  <div className={styles.dropdown}>
    <button className={styles.dropbtn}>
      {label}
      <span className={styles.dropdownArrowContainer}>
        <FontAwesomeIcon icon={faCaretDown} />
      </span>
    </button>
    <div className={`${styles.dropdownContent} ${alignRight ? styles.dropdownRight : ""}`}>
      {children}
    </div>
  </div>
);

export default MenuBar;
