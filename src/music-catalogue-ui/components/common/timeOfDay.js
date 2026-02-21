const DEFAULT_BANDS = [
  { name: "Morning",   start: "05:00", end: "12:00", playlistType: "Normal" },
  { name: "Afternoon", start: "12:00", end: "17:00", playlistType: "Normal"  },
  { name: "Evening",   start: "17:00", end: "19:00", playlistType: "Normal"  },
  { name: "Late",      start: "19:00", end: "05:00", playlistType: "Curated"  }
];

/**
 * Given a string representing a time in HH:MM format, convert that time to a "minutes since midnight"
 * @param {*} hhmm 
 * @returns 
 */
const hhmmToMinutes = (hhmm) => {
  const parts = hhmm.split(":");
  const hours = parseInt(parts[0], 10);
  const minutes = parseInt(parts[1], 10);
  return hours * 60 + minutes;
}

/**
 * Function to determine whether a 
 * @param {*} nowMin 
 * @param {*} startMin 
 * @param {*} endMin 
 * @returns 
 */
const isInBand = (nowMin, startMin, endMin) => {
  // For normal bands, the start time is less than the end time
  if (startMin < endMin) {
    return nowMin >= startMin && nowMin < endMin;
  }

  // Handle the case that wraps-over-midnight
  return nowMin >= startMin || nowMin < endMin;
}

/***
 * Function to return the band name for the current time
 * @param {*} nowMin 
 * @param {*} startMin 
 * @param {*} endMin 
 * @returns 
 */
const getCurrentBandName = (date = new Date(), bands = DEFAULT_BANDS) => {
  const nowMin = date.getHours() * 60 + date.getMinutes();

  for (const band of bands) {
    const startMin = hhmmToMinutes(band.start);
    const endMin = hhmmToMinutes(band.end);

    if (isInBand(nowMin, startMin, endMin)) {
      return band.name;
    }
  }

  throw new Error("No matching time band found");
}

/**
 * Given a band name, identify and return the time of day with that name
 * @param {*} timesOfDay 
 * @param {*} date 
 * @param {*} bands 
 * @returns 
 */
export const getCurrentTimeOfDay = (timesOfDay, date = new Date(), bands = DEFAULT_BANDS) => {
  const bandName = getCurrentBandName(date, bands);
  return timesOfDay.find(e => e.name === bandName);
}

/**
 * Given a time of day, identify and return the default playlist type for that time of day
 * @param {*} playlistTypes 
 * @param {*} timeOfDay 
 * @param {*} bands 
 * @returns 
 */
export const getPlaylistTypeForTimeOfDay = (playlistTypes, timeOfDay, bands = DEFAULT_BANDS) => {
  const band = bands.find(b => b.name === timeOfDay.name);
  return playlistTypes.find(t => t.name === band.playlistType);
}
