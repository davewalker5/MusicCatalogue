const CallbackButton = ({ label, callback }) => {
  return (
    <>
      {callback != null ? (
        <button className="btn btn-primary" onClick={() => callback()}>
          {label}
        </button>
      ) : (
        <></>
      )}
    </>
  );
};

export default CallbackButton;
