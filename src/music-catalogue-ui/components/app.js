import React, { useCallback, useState } from "react";
import Login from "./login";
import Banner from "./banner";
import pages from "@/helpers/navigation";
import ComponentPicker from "./componentPicker";
import { apiClearToken } from "@/helpers/api";

const App = () => {
  // Flag indicating the user's logged in
  const [loggedIn, setLoggedIn] = useState(false);

  // Callbacks to set the logged in flag
  const login = useCallback(() => {
    setLoggedIn(true);
  }, []);

  const logout = useCallback(() => {
    apiClearToken();
    setLoggedIn(false);
  }, []);

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
      {loggedIn ? (
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
