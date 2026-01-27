import styles from "./login.module.css";
import secrets from "@/helpers/secrets";
import { React, useState, useCallback } from "react";
import { apiAuthenticate } from "@/helpers/api/apiAuthenticate";
import { apiSetToken } from "@/helpers/api/apiToken";
import { apiFetchSecret } from "@/helpers/api/apiSecrets";
import { setStorageValue } from "@/helpers/storage";

/**
 * Component to render the login page
 * @param {*} param0
 * @returns
 */
const Login = ({ login }) => {
  // Configure state for the controlled values
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  // Callback to attempt to login
  const tryLogin = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Attempt to authenticate using the service
      const token = await apiAuthenticate(username, password);
      if (token != null) {
        // Successful, so store the token
        apiSetToken(token);

        // Now attempt to get the Google Maps API key
        const apiKey = await apiFetchSecret(secrets.mapsApiKey);
        if (apiKey != null) {
          // Successful, so store it
          setStorageValue(secrets.mapsApiKey, apiKey);
        }

        login(true);
      }
    },
    [username, password, login]
  );

  return (
    <>
      <div className={styles.authFormContainer}>
        <form className={styles.authForm}>
          <div className={styles.authFormContent}>
            <h3 className={styles.authFormTitle}>Log In</h3>
            <div className="form-group mt-3">
              <label className={styles.authFormLabel}>Username</label>
              <input
                className="form-control mt-1"
                placeholder="Username"
                name="username"
                autoComplete="username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
              />
            </div>
            <div className="form-group mt-3">
              <label className={styles.authFormLabel}>Password</label>
              <input
                type="password"
                className="form-control mt-1"
                placeholder="Password"
                name="password"
                autoComplete="current-password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </div>
            <div className="d-grid gap-2 mt-3">
              <button className="btn btn-primary" onClick={(e) => tryLogin(e)}>
                Log In
              </button>
            </div>
          </div>
        </form>
      </div>
    </>
  );
};

export default Login;
