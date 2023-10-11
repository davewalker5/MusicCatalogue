import styles from "./login.module.css";
import { React, useState, useCallback } from "react";
import { apiAuthenticate, apiSetToken } from "@/helpers/api";

/**
 * Component to render the login page
 * @param {*} param0
 * @returns
 */
const Login = ({ login }) => {
  // Configure state items for controlled form fields
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");

  const submit = useCallback(
    async (e) => {
      // Prevent default handling of the submit button press
      e.preventDefault();

      // Attempt to login
      const token = await apiAuthenticate(userName, password);
      if (token != null) {
        apiSetToken(token);
        login(true);
      }
    },
    [userName, password, login]
  );

  return (
    <>
      <div className={styles.authFormContainer}>
        <form className={styles.authForm} onSubmit={submit}>
          <div className={styles.authFormContent}>
            <h3 className={styles.authFormTitle}>Log In</h3>
            <div className="form-group mt-3">
              <label className={styles.authFormLabel}>Username</label>
              <input
                className="form-control mt-1"
                placeholder="Username"
                name="username"
                autoComplete="username"
                value={userName}
                onChange={(e) => setUserName(e.target.value)}
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
              <button type="submit" className="btn btn-primary">
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
