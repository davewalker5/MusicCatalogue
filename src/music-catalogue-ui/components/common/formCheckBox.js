/**
 * Component to render a form checkbox field
 * @param {*} param0
 * @returns
 */
const FormCheckBox = ({ label, name, value, setValue }) => {
  return (
    <div className="form-check d-flex align-items-center gap-2 mt-3">
      <input
        className="form-check-input m-0"
        type="checkbox"
        id={name}
        name={name}
        checked={!!value}
        onChange={(e) => setValue(e.target.checked)}
      />
      <label className="form-check-label m-0" htmlFor={name}>
        {label}
      </label>
    </div>
  );
};

export default FormCheckBox;
