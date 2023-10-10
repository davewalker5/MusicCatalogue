import { useCallback } from "react";
import ButtonBar from "./buttonBar";

/**
 * Component to render the album lookup page
 * @param {*} param0
 * @returns
 */
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
