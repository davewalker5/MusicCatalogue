import { useEffect, useState } from "react";
import styles from "./slider.module.css";

const Slider = ({ initialValue, minimum, maximum, step, sliderChangedCallback }) => {
  const [value, setValue] = useState(initialValue);

  // Keep in sync if the parent changes the initial value later
  useEffect(() => {
    setValue(initialValue);
  }, [initialValue]);

  const handleChange = (e) => {
    const newValue = Number(e.target.value);
    setValue(newValue);
    sliderChangedCallback(newValue);
  };

  return (
    <div className={styles.sliderwrap}>
      <input
        type="range"
        min={minimum}
        max={maximum}
        step={step}
        value={value}
        onChange={handleChange}
        className="slider"
      />
      <span className={styles.slidervalue}>{value}</span>
    </div>
  );
};

export default Slider;