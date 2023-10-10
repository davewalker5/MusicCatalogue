import React, { useCallback, useState } from "react";
import Banner from "./banner";
import pages from "@/helpers/navigation";
import ComponentPicker from "./componentPicker";

const App = () => {
  const [context, setContext] = useState({
    page: pages.artists,
    artist: null,
    album: null,
  });

  const navigate = useCallback((page, artist, album) => {
    console.log(page);
    console.log(artist);
    console.log(album);
    setContext({
      page: page,
      artist: artist,
      album: album,
    });
  }, []);

  return (
    <>
      <Banner navigate={navigate} />
      <ComponentPicker context={context} navigate={navigate} />
    </>
  );
};

export default App;
