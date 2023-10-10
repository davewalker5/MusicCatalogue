import { useCallback } from "react";
import ButtonBar from "./buttonBar";

const LookupAlbum = ({ logout }) => {
  // Album lookup callback
  const lookup = useCallback(() => {}, []);

  return (
    <>
      <div className="row mb-2">
        <h5 className="themeFontColor text-center">Lookup Album</h5>
      </div>
      <ButtonBar navigateBack={null} lookup={lookup} logout={logout} />
    </>
  );
};

export default LookupAlbum;
