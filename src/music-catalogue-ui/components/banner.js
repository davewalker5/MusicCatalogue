import { useContext } from "react";
import styles from "./banner.module.css";
import { navigationContext } from "./app";
import pages from "@/helpers/navigation";

const Banner = ({ children }) => {
  const { navigate } = useContext(navigationContext);
  return (
    <header className="row mb-4">
      <div className="col-2">
        <img
          src="./logo.png"
          alt="Music Catalogue"
          className={styles.logo}
          onClick={() => navigate(pages.home)}
        />
      </div>
      <div className="col-9 mt-5">
        <div className={styles.title}>{children}</div>
      </div>
    </header>
  );
};

export default Banner;
