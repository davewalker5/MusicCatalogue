/**
 * Convert a name that may be mixed case and with spaces to a local storage key
 * @param {*} name
 * @returns
 */
const getStorageKey = (name) => {
  const key = name.toLowerCase().replace(/ /g, "-");
  return key;
};

/**
 * Store a named value
 * @param {*} token
 * @param {*} value
 */
const setStorageValue = (name, value) => {
  const key = getStorageKey(name);
  localStorage.setItem(key, value);
};

/**
 * Retrieve a stored value
 * @param {*} name
 * @returns
 */
const getStorageValue = (name) => {
  try {
    const key = getStorageKey(name);
    const token = localStorage.getItem(key);
    return token;
  } catch {
    return null;
  }
};

/**
 * Clear a named storage value
 * @param {*} name
 */
const clearStorageValue = (name) => {
  const key = getStorageKey(name);
  localStorage.removeItem(key);
};

export { setStorageValue, getStorageValue, clearStorageValue };
