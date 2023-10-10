import styles from "./banner.module.css";
import pages from "@/helpers/navigation";

const Banner = ({ navigate }) => {
  return (
    <header className="row mb-4">
      <div className="col-2">
        <img
          src="./logo.png"
          alt="Music Catalogue"
          className={styles.logo}
          onClick={() => navigate(pages.artists, null, null)}
        />
      </div>
      <div className="col-9 mt-5">
        <div className={styles.title}>
          <div>Music Catalogue</div>
        </div>
      </div>
    </header>
  );
};

export default Banner;
