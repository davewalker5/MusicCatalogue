import React, { useCallback, useState } from "react";
import Banner from "./banner";
import pages from "@/helpers/navigation";
import ComponentPicker from "./componentPicker";

const navigationContext = React.createContext(pages.artists);

const App = () => {
  const navigate = useCallback(
    (navTo, param) => setNav({ current: navTo, param, navigate }),
    []
  );

  const [nav, setNav] = useState({ current: pages.artists, navigate });

  return (
    <navigationContext.Provider value={nav}>
      <Banner>
        <div>Personal Music Catalogue</div>
      </Banner>
      <ComponentPicker currentNavLocation={nav.current} />
    </navigationContext.Provider>
  );
};

export { navigationContext };
export default App;
