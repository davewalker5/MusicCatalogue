import { apiGetToken } from "@/helpers/api/apiToken";
import { useState, useEffect } from "react";

/**
 * Hook to determine if the user is logged in or not
 * @returns
 */
const useIsLoggedIn = () => {
  const [isLoggedIn, setIsLoggedIn] = useState([]);

  useEffect(() => {
    const haveToken = apiGetToken();
    setIsLoggedIn(haveToken);
  }, []);

  return { isLoggedIn, setIsLoggedIn };
};

export default useIsLoggedIn;
