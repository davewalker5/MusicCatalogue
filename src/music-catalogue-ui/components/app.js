import React, { useCallback, useState } from "react";
import Login from "./login";
import Banner from "./banner";
import pages from "@/helpers/navigation";
import ComponentPicker from "./componentPicker";
import { apiClearToken } from "@/helpers/api";
import useIsLoggedIn from "@/hooks/useIsLoggedIn";

const App = () => {
  const { isLoggedIn, setIsLoggedIn } = useIsLoggedIn();

  // Callbacks to set the logged in flag
  const login = useCallback(() => {
    setIsLoggedIn(true);
  }, [setIsLoggedIn]);

  const logout = useCallback(() => {
    apiClearToken();
    setIsLoggedIn(false);
  }, [setIsLoggedIn]);

  // Application-wide context
  const [context, setContext] = useState({
    page: pages.artists,
    artist: null,
    album: null,
  });

  // Callback to set the context
  const navigate = useCallback((page, artist, album) => {
    setContext({
      page: page,
      artist: artist,
      album: album,
    });
  }, []);

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
