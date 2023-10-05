import React, { useCallback, useState } from "react";
import Banner from "./banner";
import pages from "@/helpers/navigation";
import ComponentPicker from "./componentPicker";

// Create a navigation context for the current page to display
const navigationContext = React.createContext(pages.artists);

const App = () => {
  // Function used to change the current page. navTo is the page to navigate
  // to, a member of the pages object. param is a parameter passed to the
  // navigation function e.g. an artist id or an album id
  const navigate = useCallback(
    (navTo, param) => setCurrentPage({ current: navTo, param, navigate }),
    []
  );

  // Store the current page and the navigation function in state
  const [page, setCurrentPage] = useState({
    current: pages.artists,
    navigate,
  });

  // Note that the application provides a navigation context to all children
  // below it in the hierarchy
  return (
    <navigationContext.Provider value={page}>
      <Banner>
        <div>Personal Music Catalogue</div>
      </Banner>
      <ComponentPicker currentPage={page.current} />
    </navigationContext.Provider>
  );
};

export { navigationContext };
export default App;
