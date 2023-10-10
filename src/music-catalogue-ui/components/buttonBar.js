const ButtonBar = ({ navigateBack, logout }) => {
  return (
    <div>
      <div class="float-start">
        {navigateBack ? (
          <button className="btn btn-primary " onClick={() => navigateBack()}>
            &lt; Back
          </button>
        ) : (
          <></>
        )}
      </div>
      <div class="float-end">
        <button className="btn btn-primary" onClick={() => logout()}>
          Logout
        </button>
      </div>
    </div>
  );
};

export default ButtonBar;
