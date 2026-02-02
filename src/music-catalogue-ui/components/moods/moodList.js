import styles from "./moodList.module.css";
import pages from "@/helpers/navigation";
import useMoods from "@/hooks/useMoods";
import MoodRow from "./moodRow";
import { useState } from "react";

/**
 * Component to render a table listing all the moods in the catalogue
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const MoodList = ({ navigate, logout }) => {
  const { moods, setMoods } = useMoods(logout);
  const [error, setError] = useState("");

  // Callback to pass to child components to set the list of moods
  const setMoodsCallback = (moods) => {
    setMoods(moods);
  };

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Moods</h5>
      </div>
      <div className="row">
        {error != "" ? (
          <div className={styles.moodListError}>{error}</div>
        ) : (
          <></>
        )}
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Name</th>
            <th>Morning Weight</th>
            <th>Afternoon Weight</th>
            <th>Evening Weight</th>
            <th>Late Night Weight</th>
          </tr>
        </thead>
        {moods != null && (
          <tbody>
            {moods.map((m) => (
              <MoodRow
                key={m.id}
                mood={m}
                navigate={navigate}
                logout={logout}
                setMoods={setMoodsCallback}
                setError={setError}
              />
            ))}
          </tbody>
        )}
      </table>
      <div className={styles.moodListAddButton}>
        <button
          className="btn btn-primary"
          onClick={() =>
            navigate({
              page: pages.moodEditor,
              mood: null,
            })
          }
        >
          Add
        </button>
      </div>
    </>
  );
};

export default MoodList;
