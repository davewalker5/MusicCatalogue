import styles from "./buttonBar.module.css";

/**
 * Component to render the configurable button bar at the bottom of the page
 * @param {*} param0
 * @returns
 */
const ButtonBar = ({ navigateBack, lookup, logout }) => {
  const lookupStyles = navigateBack
    ? `btn btn-primary ${styles.lookup}`
    : "btn btn-primary";

  return (
    <div>
      <div className="float-start">
        {navigateBack ? (
          <button className="btn btn-primary" onClick={() => navigateBack()}>
            &lt; Back
          </button>
        ) : (
          <></>
        )}

        {lookup ? (
          <button className={lookupStyles} onClick={() => lookup()}>
            Lookup
          </button>
        ) : (
          <></>
        )}
      </div>
      <div className="float-end">
        <button className="btn btn-primary" onClick={() => logout()}>
          Logout
        </button>
      </div>
    </div>
  );
};

export default ButtonBar;
