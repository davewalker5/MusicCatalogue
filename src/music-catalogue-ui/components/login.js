import styles from "./login.module.css";
import { React, useState, useCallback } from "react";
import { apiAuthenticate, apiSetToken } from "@/helpers/api";

/**
 * Component to render the login page
 * @param {*} param0
 * @returns
 */
const Login = ({ login }) => {
  const submit = (e) => {
    // Prevent event propagation
    e.preventDefault();
    // e.nativeEvent.stopPropagation();

    // Get the form elements
    const userName = e.target.username.value;
    const password = e.target.password.value;

    // Attempt to login
    apiAuthenticate(userName, password).then(function (token) {
      apiSetToken(token);
      login(true);
    });
  };

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
