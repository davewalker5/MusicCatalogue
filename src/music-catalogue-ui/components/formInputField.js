import styles from "./formInputField.module.css";

/**
 * Component to render a form input field
 * @param {*} param0
 * @returns
 */
const FormInputField = ({ label, name, value, setValue }) => {
  return (
    <div className="form-group mt-3">
      <label className={styles.retailerEditorInputLabel}>{label}</label>
      <input
        className="form-control"
        placeholder={label}
        name={name}
        value={value}
        onChange={(e) => setValue(e.target.value)}
      />
    </div>
  );
};

export default FormInputField;
