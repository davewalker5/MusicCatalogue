import React, { useCallback, useState } from "react";
import Login from "./login/login";
import pages from "@/helpers/navigation";
import ComponentPicker from "./componentPicker";
import { apiClearToken } from "@/helpers/api/apiToken";
import useIsLoggedIn from "@/hooks/useIsLoggedIn";
import MenuBar from "./menuBar";
import { clearStorageValue } from "@/helpers/storage";
import secrets from "@/helpers/secrets";

/**
 * Default application state:
 */
const defaultContext = {
  // Current page
  page: pages.artists,

  // Artist, album, track and retailer context
  artist: null,
  album: null,
  track: null,
  retailer: null,

  // Data retrieval/filering criteria
  genre: null,
  filter: "A",
  isWishList: false,
};

const App = () => {
  // Hook to determine the current logged in state, and a method to change it
  const { isLoggedIn, setIsLoggedIn } = useIsLoggedIn();

  // Application state
  const [context, setContext] = useState(defaultContext);

  // Callback to set the context
  const navigate = ({
    page = pages.artists,
    artist = null,
    album = null,
    track = null,
    retailer = null,
    genre = null,
    equipmentType = null,
    filter = "A",
    isWishList = false,
  } = {}) => {
    // Set the context, applying defaults to any values that are undefined
    const updatedContext = {
      page: page,
      artist: typeof artist != "undefined" ? artist : null,
      album: typeof album != "undefined" ? album : null,
      track: typeof track != "undefined" ? track : null,
      retailer: typeof retailer != "undefined" ? retailer : null,
      genre: typeof genre != "undefined" ? genre : null,
      equipmentType: typeof equipmentType != "undefined" ? equipmentType : null,
      filter: typeof filter != "undefined" ? filter : "A",
      isWishList: typeof isWishList != "undefined" ? isWishList : false,
    };
    setContext(updatedContext);
  };

  // Callbacks to set the logged in flag
  const login = useCallback(() => {
    setIsLoggedIn(true);
    setContext(defaultContext);
  }, [setIsLoggedIn]);

  const logout = useCallback(() => {
    clearStorageValue(secrets.mapsApiKey);
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
