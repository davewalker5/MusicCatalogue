import styles from "./slider.module.css";

const Slider = ({ value, minimum, maximum, step, onChange }) => {
  return (
    <div className={styles.sliderwrap}>
      <input
        type="range"
        min={minimum}
        max={maximum}
        step={step}
        value={value}
        onChange={(e) => onChange(e.target.value)}
        className="slider"
      />
      <span className={styles.slidervalue}>{value}</span>
    </div>
  );
};

export default Slider;