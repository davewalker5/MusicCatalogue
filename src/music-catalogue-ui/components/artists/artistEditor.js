import styles from "./artistEditor.module.css";
import pages from "@/helpers/navigation";
import FormInputField from "../common/formInputField";
import { useState, useCallback } from "react";
import { apiCreateArtist, apiUpdateArtist } from "@/helpers/api/apiArtists";
import VocalPresenceSelector from "./vocalPresenceSelector";
import EnsembleTypeSelector from "./ensembleTypeSelector";
import Slider from "../common/slider";

/**
 * Component to render the artist editor
 * @param {*} filter
 * @param {*} artist
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} logout
 */
const ArtistEditor = ({ filter, artist, isWishList, navigate, logout }) => {
  // Get initial values for artist properties
  const initialName = artist != null ? artist.name : null;
  const initialVocalPresence = artist != null ? artist.vocals : null;
  const initialEnsembleType = artist != null ? artist.ensemble : null;
  const initialEnergy = artist != null ? artist.energy : null;
  const initialIntimacy = artist != null ? artist.intimacy : null;
  const initialWarmth = artist != null ? artist.warmth : null;

  // Set up state
  const [name, setName] = useState(initialName);
  const [vocalPresence, setVocalPresence] = useState(initialVocalPresence);
  const [ensembleType, setEnsembleType] = useState(initialEnsembleType);
  const [energy, setEnergy] = useState(initialEnergy);
  const [intimacy, setIntimacy] = useState(initialIntimacy);
  const [warmth, setWarmth] = useState(initialWarmth);
  const [error, setError] = useState("");

  const saveArtist = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Clear pre-existing errors
      setError("");

      try {
        // Either add or update the artist, depending on whether there's an
        // existing artist or not
        let updatedArtist = null;
        if (artist == null) {
          // Create the artist
          updatedArtist = await apiCreateArtist(name, energy, intimacy, warmth, vocalPresence.id, ensembleType.id, logout);
        } else {
          // Update the existing artist
          updatedArtist = await apiUpdateArtist(artist.id, name, energy, intimacy, warmth, vocalPresence.id, ensembleType.id, logout);
        }

        // Go back to the artist list, which should reflect the updated details
        navigate({
          filter: filter,
          page: pages.artists,
          isWishList: isWishList,
        });
      } catch (ex) {
        setError(`Error saving the updated artist details: ${ex.message}`);
      }
    },
    [filter,
      artist,
      isWishList,
      logout,
      name,
      energy,
      intimacy,
      warmth,
      vocalPresence,
      ensembleType,
      navigate]
  );

  // Set the page title
  const pageTitle = artist != null ? artist.name : "New Artist";

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{pageTitle}</h5>
      </div>
      <div className={styles.artistEditorFormContainer}>
        <form className={styles.artistEditorForm}>
          <div className="row">
            {error != "" ? (
              <div className={styles.artistEditorError}>{error}</div>
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
            <div className="col">
              <div className="form-group mt-3">
                <label className={styles.artistEditorFormLabel}>Energy</label>
                <div>
                  <Slider
                    initialValue={energy}
                    minimum={0}
                    maximum={5}
                    step={1}
                    sliderChangedCallback={setEnergy}
                  />
                </div>
              </div>
            </div>
            <div className="col">
              <div className="form-group mt-3">
                <label className={styles.artistEditorFormLabel}>Intimacy</label>
                <div>
                  <Slider
                    initialValue={intimacy}
                    minimum={0}
                    maximum={5}
                    step={1}
                    sliderChangedCallback={setIntimacy}
                  />
                </div>
              </div>
            </div>
            <div className="col">
              <div className="form-group mt-3">
                <label className={styles.artistEditorFormLabel}>Warmth</label>
                <div>
                  <Slider
                    initialValue={warmth}
                    minimum={0}
                    maximum={5}
                    step={1}
                    sliderChangedCallback={setWarmth}
                  />
                </div>
              </div>
            </div>
          </div>
          <div className="form-group mt-3">
            <label className={styles.artistEditorFormLabel}>Vocal Presence</label>
            <div>
              <VocalPresenceSelector
                initialVocalPresence={vocalPresence}
                vocalPresenceChangedCallback={setVocalPresence}
                logout={logout}
              />
            </div>
          </div>
          <div className="form-group mt-3">
            <label className={styles.artistEditorFormLabel}>Ensemble Type</label>
            <div>
              <EnsembleTypeSelector
                initialEnsembleType={ensembleType}
                ensembleTypeChangedCallback={setEnsembleType}
                logout={logout}
              />
            </div>
          </div>
          <div className="d-grid gap-2 mt-3"></div>
          <div className="d-grid gap-2 mt-3"></div>
          <div className={styles.artistEditorButton}>
            <button className="btn btn-primary" onClick={(e) => saveArtist(e)}>
              Save
            </button>
          </div>
          <div className={styles.artistEditorButton}>
            <button
              className="btn btn-primary"
              onClick={() =>
                navigate({
                  filter: filter,
                  page: pages.artists,
                  isWishList: isWishList,
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

export default ArtistEditor;
