import DeleteMoodActionIcon from "./deleteMoodActionIcon";
import pages from "@/helpers/navigation";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPenToSquare } from "@fortawesome/free-solid-svg-icons";
import { Tooltip } from "react-tooltip";

/**
 * Component to render a row containing the details for a single mood
 * @param {*} mood
 * @param {*} navigate
 * @param {*} logout
 * @param {*} setMoods
 * @param {*} setError
 * @returns
 */
const MoodRow = ({ mood, navigate, logout, setMoods, setError }) => {
  return (
    <tr>
      <td>{mood.name}</td>
      <td>
        <DeleteMoodActionIcon
          mood={mood}
          logout={logout}
          setMoods={setMoods}
          setError={setError}
        />
      </td>
      <td>
        <>
          <FontAwesomeIcon
            icon={faPenToSquare}
            data-tooltip-id="edit-tooltip"
            data-tooltip-content="Edit mood"
            onClick={() =>
              navigate({
                page: pages.moodEditor,
                mood: mood,
              })
            }
          />

          <Tooltip id="edit-tooltip" />
        </>
      </td>
    </tr>
  );
};

export default MoodRow;
