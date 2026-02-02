namespace MusicCatalogue.Entities.Extensions
{
    public static class NumberRangeExtensions
    {
        /// <summary>
        /// Clamp a value to the range minimum - maximum
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public static double Clamp(double value, double minimum, double maximum)
            => (value < minimum) ? minimum : (value > maximum) ? maximum : value;

        /// <summary>
        /// Clamp an integer value to the range minimum - maximum
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public static int ClampInteger(int value, int minimum, int maximum)
            => (value < minimum) ? minimum : (value > maximum) ? maximum : value;
    }  
}
