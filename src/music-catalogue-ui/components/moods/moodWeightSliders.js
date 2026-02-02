import styles from "./moodEditor.module.css";
import React, { useCallback, useMemo } from "react";
import Slider from "../common/slider";

/***
 * Clip a numeric value to a range
 * @param {*} x
 * @param {*} minimum
 * @param {*} maximum
 * @returns
 */
const clamp = (x, minimum, maximum) => Math.min(maximum, Math.max(minimum, x));

/***
 * The slider constrains values to a defined step but scaling so they all add up to 1
 * isn't quantised. Given a value, calculate how many steps it constitutes and round
 * to the nearest whole step
 * @param {*} x
 * @param {*} minimum
 * @param {*} maximum
 * @returns
 */
function quantizeToStep(x, step) {
  if (!step) return x;
  const inv = 1 / step;
  return Math.round(x * inv) / inv;
}

/***
 * Redistribute a fixed budget of 1.0 across the sliders
 * @param {*} values 
 * @param {*} i 
 * @param {*} newValue 
 * @param {*} step 
 * @returns 
 */
function redistribute(values, i, newValue, step) {
  const n = values.length;
  const next = [...values];

  // Quantise the value selected by the user
  let x = clamp(newValue, 0, 1);
  x = quantizeToStep(x, step);
  next[i] = x;

  // Calculate the remaining budget
  const R = quantizeToStep(1 - x, step);

  // Identify the other sliders
  const others = [];
  for (let k = 0; k < n; k++) {
    if (k !== i) {
        others.push(k);
    }
  }

  // Determine the sum of the original values for the other sliders
  const sumOthersOld = others.reduce((acc, k) => acc + values[k], 0);

  // Do a proportional redistribution of the remaining budget across the other sliders
  if (sumOthersOld > 0) {
    for (const k of others) {
      const v = (values[k] / sumOthersOld) * R;
      next[k] = quantizeToStep(v, step);
    }
  } else {
    // Edge case - only if the other sliders are 0
    const even = quantizeToStep(R / others.length, step);
    for (const k of others) next[k] = even;
  }

  // Fix rounding drift so the sum is exactly 1 (to step resolution)
  const sum = quantizeToStep(next.reduce((acc, v) => acc + v, 0), step);
  const drift = quantizeToStep(1 - sum, step);

  // If there's any remaining drift, absorb it into the largest value
  if (Math.abs(drift) > 0) {
    let absorb = others[0];
    for (const k of others) {
        if (next[k] > next[absorb]) {
            absorb = k;
        }
    }

    next[absorb] = quantizeToStep(clamp(next[absorb] + drift, 0, 1), step);
  }

  return next;
}

/**
 * Component to render the linked mood weightings sliders
 * @param {*} values
 * @param {*} onChange
 * @param {*} labels
 * @param {*} step
 * @param {*} showPercent
 * @returns
 */
const MoodWeightSliders = ({
  values,
  onChange,
  labels,
  step,
}) => {
  const onSliderChange = useCallback(
    (idx) => (newValue) => {
      const next = redistribute(values, idx, newValue, step);
      onChange(next);
    },
    [values, onChange, step]
  );

  return (
   <>
      {values.map((v, idx) => (
        <div key={idx} className="col">
          <div className="form-group mt-3">
            <label className={styles.retailerEditorInputLabel}>{labels[idx]}</label>
            <Slider
              value={v}
              minimum={0}
              maximum={1}
              step={step}
              onChange={onSliderChange(idx)}
            />
          </div>
        </div>
      ))}
    </>
  );
}

export default MoodWeightSliders;
