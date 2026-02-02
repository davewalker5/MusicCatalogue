import styles from "./moodEditor.module.css";
import pages from "@/helpers/navigation";
import FormInputField from "../common/formInputField";
import { apiCreateMood, apiUpdateMood } from "@/helpers/api/apiMoods";
import { useState, useCallback } from "react";
import MoodWeightSliders from "./moodWeightSliders";

/**
 * Component to render a mood editor
 * @param {*} mood
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const MoodEditor = ({ mood, navigate, logout }) => {
  // Capture initial values
  const initialName = mood != null ? mood.name : "";
  const initialMorningWeight = mood != null ? mood.morningWeight : 0.0;
  const initialAfternoonWeight = mood != null ? mood.afternoonWeight : 0.0;
  const initialEveningWeight = mood != null ? mood.eveningWeight : 0.0;
  const initialLateWeight = mood != null ? mood.lateWeight : 0.0;

  // Setup state
  const [name, setName] = useState(initialName);
  const [weights, setWeights] = useState([
    initialMorningWeight,
    initialAfternoonWeight,
    initialEveningWeight,
    initialLateWeight
  ]);
  const [error, setError] = useState("");

  const sliderLabels = ["Morning Weight", "Afternoon Weight", "Evening Weight", "Lat Night Weight"]

  const saveMood = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Clear pre-existing errors
      setError("");

      try {
        // Before saving, the weights need to be scaled so they add up to 0
        // Either add or update the mood, depending on whether there's an
        // existing album or not
        let updatedMood = null;
        if (mood == null) {
          // Create the mood
          updatedMood = await apiCreateMood(name, weights[0], weights[1], weights[2], weights[3], logout);
        } else {
          // Update the existing mood
          updatedMood = await apiUpdateMood(mood.id, name, weights[0], weights[1], weights[2], weights[3], logout);
        }

        // Go back to the mood, which should reflect the updated details
        navigate({
          page: pages.moods,
        });
      } catch (ex) {
        setError(`Error saving the updated mood details: ${ex.message}`);
      }
    },
    [mood, name, weights, navigate, logout]
  );

  // Set the page title
  const pageTitle = mood != null ? `Mood - ${mood.name}` : `New Mood`;

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{pageTitle}</h5>
      </div>
      <div className={styles.moodEditorFormContainer}>
        <form className={styles.moodEditorForm}>
          <div className="row">
            {error != "" ? (
              <div className={styles.moodEditorError}>{error}</div>
            ) : (
              <></>
            )}
          </div>
          <div className="row align-items-start">
            <FormInputField
              label="Name"
              name="name"
              value={name}
              setValue={setName}
            />
          </div>
          <div className="row align-items-start">
            <>
              <MoodWeightSliders
                values={weights}
                onChange={setWeights}
                labels={sliderLabels}
                step={0.01}
              />
            </>
          </div>
          <div className="d-grid gap-2 mt-3"></div>
          <div className="d-grid gap-2 mt-3"></div>
          <div className={styles.moodEditorButton}>
            <button className="btn btn-primary" onClick={(e) => saveMood(e)}>
              Save
            </button>
          </div>
          <div className={styles.moodEditorButton}>
            <button
              className="btn btn-primary"
              onClick={() =>
                navigate({
                  page: pages.moods,
                })
              }
            >
              Cancel
            </button>
          </div>
        </form>
      </div>
    </>
  );
};

export default MoodEditor;
