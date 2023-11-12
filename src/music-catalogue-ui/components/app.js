import React, { useCallback, useState } from "react";
import Login from "./login";
import pages from "@/helpers/navigation";
import ComponentPicker from "./componentPicker";
import { apiClearToken } from "@/helpers/apiToken";
import useIsLoggedIn from "@/hooks/useIsLoggedIn";
import MenuBar from "./menuBar";

const defaultContext = {
  page: pages.artists,
  artist: null,
  album: null,
  isWishList: false,
};

const App = () => {
  // Hook to determine the current logged in state, and a method to change it
  const { isLoggedIn, setIsLoggedIn } = useIsLoggedIn();

  // Application-wide context
  const [context, setContext] = useState(defaultContext);

  // Callback to set the context
  const navigate = useCallback((page, artist, album, isWishList) => {
    setContext({
      page: page,
      artist: artist,
      album: album,
      isWishList: isWishList,
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
          <div>
            <MenuBar navigate={navigate} logout={logout} />
          </div>
          <div>
            <ComponentPicker
              context={context}
              navigate={navigate}
              logout={logout}
            />
          </div>
        </>
      ) : (
        <Login login={login} />
      )}
    </>
  );
};

export default App;
