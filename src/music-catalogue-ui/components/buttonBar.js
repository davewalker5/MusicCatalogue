import CallbackButton from "./callbackButton";

/**
 * Component to render the configurable button bar at the bottom of the page
 * @param {*} param0
 * @returns
 */
const ButtonBar = ({ navigateBack, lookup, logout }) => {
  return (
    <div>
      <div className="float-start">
        <CallbackButton label="&lt; Back" callback={navigateBack} />
        {navigateBack != null ? " " : <></>}
        <CallbackButton label="Lookup" callback={lookup} />
      </div>
      <div className="float-end">
        <CallbackButton label="Logout" callback={logout} />
      </div>
    </div>
  );
};

export default ButtonBar;
