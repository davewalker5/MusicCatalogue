import React, { useCallback, useEffect, useState } from "react";
import Login from "./login";
import pages from "@/helpers/navigation";
import ComponentPicker from "./componentPicker";
import { apiClearToken } from "@/helpers/apiToken";
import useIsLoggedIn from "@/hooks/useIsLoggedIn";
import MenuBar from "./menuBar";
import { apiFetchSecret } from "@/helpers/apiSecrets";

/**
 * Default application state:
 */
const defaultContext = {
  // Current page
  page: pages.artists,

  // Artist, album and retailer context
  artist: null,
  album: null,
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

  // Google Maps API key
  const [mapsApiKey, setMapsApiKey] = useState(null);

  // Callback to set the context
  const navigate = ({
    page = pages.artists,
    artist = null,
    album = null,
    retailer = null,
    genre = null,
    filter = "A",
    isWishList = false,
  } = {}) => {
    // Set the context, applying defaults to any values that are undefined
    const updatedContext = {
      page: page,
      artist: typeof artist != "undefined" ? artist : null,
      album: typeof album != "undefined" ? album : null,
      retailer: typeof retailer != "undefined" ? retailer : null,
      genre: typeof genre != "undefined" ? genre : null,
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
    apiClearToken();
    setIsLoggedIn(false);
  }, [setIsLoggedIn]);

  // Get the Google Maps API key and store it in state
  useEffect(() => {
    const fetchMapsApiKey = async () => {
      try {
        var fetchedKey = await apiFetchSecret("Maps API Key", logout);
        setMapsApiKey(fetchedKey);
      } catch {}
    };

    fetchMapsApiKey();
  }, [logout]);

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
              mapsApiKey={mapsApiKey}
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
