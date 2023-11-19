import React, { useCallback, useState } from "react";
import Login from "./login";
import pages from "@/helpers/navigation";
import ComponentPicker from "./componentPicker";
import { apiClearToken } from "@/helpers/apiToken";
import useIsLoggedIn from "@/hooks/useIsLoggedIn";
import MenuBar from "./menuBar";
import config from "../config.json";
import { Helmet } from "react-helmet";

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

  // Set the script source for Google Maps
  const googleMapsScript = `https://maps.googleapis.com/maps/api/js?key=${config.api.mapsApiKey}&callback=console.debug&libraries=maps,marker&v=beta`;

  // If the user's logged in, show the banner and current component. Otherwise,
  // show the login page
  return (
    <>
      {isLoggedIn ? (
        <>
          <Helmet>
            <script
              src={googleMapsScript}
              crossorigin="anonymous"
              async
            ></script>
          </Helmet>
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
