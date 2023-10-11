import styles from "./login.module.css";
import { React, useState, useCallback } from "react";
import { apiAuthenticate, apiSetToken } from "@/helpers/api";

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
  const tryLogin = useCallback(async () => {
    const token = await apiAuthenticate(username, password);
    if (token != null) {
      apiSetToken(token);
      login(true);
    }
  }, [username, password, login]);

  return (
    <>
      <div className={styles.authFormContainer}>
        <div className={styles.authForm}>
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
              <button className="btn btn-primary" onClick={tryLogin}>
                Log In
              </button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default Login;
