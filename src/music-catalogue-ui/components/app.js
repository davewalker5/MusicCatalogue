import React, { useCallback, useState } from "react";
import Login from "./login";
import Banner from "./banner";
import pages from "@/helpers/navigation";
import ComponentPicker from "./componentPicker";
import { apiClearToken } from "@/helpers/api";
import useIsLoggedIn from "@/hooks/useIsLoggedIn";

const defaultContext = {
  page: pages.artists,
  artist: null,
  album: null,
};

const App = () => {
  // Hook to determine the current logged in state, and a method to change it
  const { isLoggedIn, setIsLoggedIn } = useIsLoggedIn();

  // Application-wide context
  const [context, setContext] = useState(defaultContext);

  // Callback to set the context
  const navigate = useCallback((page, artist, album) => {
    setContext({
      page: page,
      artist: artist,
      album: album,
    });
  }, []);

  // Callbacks to set the logged in flag
  const login = useCallback(() => {
    setIsLoggedIn(true);
    setContext(defaultContext);
  }, [setIsLoggedIn]);

  const logout = useCallback(() => {
    apiClearToken();
    setIsLoggedIn(false);
  }, [setIsLoggedIn]);

  // If the user's logged in, show the banner and current component. Otherwise,
  // show the login page
  return (
    <>
      {isLoggedIn ? (
        <>
          <Banner navigate={navigate} />
          <ComponentPicker
            context={context}
            navigate={navigate}
            logout={logout}
          />
        </>
      ) : (
        <Login login={login} />
      )}
    </>
  );
};

export default App;
