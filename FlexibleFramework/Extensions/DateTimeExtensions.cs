using System;

namespace FlexibleFramework.Extensions
{
    /// <summary>
    /// Provides utility methods for manipulating DateTime values.
    /// </summary>
    public static class DateTimeExtensions
    {
        #region Methods to Check Time Components

        /// <summary>
        /// Checks if the specified DateTime value has any non-zero time components (hours, minutes, seconds, or milliseconds).
        /// </summary>
        /// <param name="date">The DateTime value to check.</param>
        /// <returns>A boolean indicating whether the DateTime value has a non-zero time component.</returns>
        public static bool HasTime(this DateTime date)
        {
            return date.TimeOfDay.Ticks > 0;
        }

        /// <summary>
        /// Checks if the specified DateTime value has any non-zero time components (hours, minutes, seconds, or milliseconds).
        /// </summary>
        /// <param name="date">The nullable DateTime value to check.</param>
        /// <returns>A boolean indicating whether the DateTime value has a non-zero time component.</returns>
        public static bool HasTime(this DateTime? date)
        {
            return date?.HasTime() ?? false;
        }

        #endregion

        #region Methods to Reset Time Components

        /// <summary>
        /// Returns a new DateTime value representing the start of the specified date (00:00:00.0000000).
        /// </summary>
        /// <param name="date">The original DateTime value.</param>
        /// <returns>A new DateTime value with time components reset.</returns>
        public static DateTime BeginDay(this DateTime date)
        {
            return date.Date;
        }

        /// <summary>
        /// Returns a new DateTime value representing the start of the specified date (00:00:00.0000000).
        /// </summary>
        /// <param name="date">The original nullable DateTime value.</param>
        /// <returns>A new DateTime value with time components reset, or null if input is null.</returns>
        public static DateTime? BeginDay(this DateTime? date)
        {
            return date?.Date;
        }

        #endregion

        #region Methods to Set Time Components

        /// <summary>
        /// Returns a new DateTime value representing the end of the specified date (23:59:59.9999999).
        /// </summary>
        /// <param name="date">The original DateTime value.</param>
        /// <returns>A new DateTime value with time set to end of day.</returns>
        public static DateTime EndDay(this DateTime date)
        {
            return date.Date.AddDays(1).AddTicks(-1);
        }

        /// <summary>
        /// Returns a new DateTime value representing the end of the specified date (23:59:59.9999999).
        /// </summary>
        /// <param name="date">The original nullable DateTime value.</param>
        /// <returns>A new DateTime value with time set to end of day, or null if input is null.</returns>
        public static DateTime? EndDay(this DateTime? date)
        {
            return date?.EndDay();
        }

        #endregion

        #region Methods to Manipulate Month Components

        /// <summary>
        /// Returns a new DateTime value representing the start of the specified month.
        /// </summary>
        /// <param name="date">The original DateTime value.</param>
        /// <returns>A new DateTime value set to the first day of the month at 00:00:00.</returns>
        public static DateTime BeginMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1, 0, 0, 0, date.Kind);
        }

        /// <summary>
        /// Returns a new DateTime value representing the start of the specified month.
        /// </summary>
        /// <param name="date">The original nullable DateTime value.</param>
        /// <returns>A new DateTime value set to the first day of the month, or null if input is null.</returns>
        public static DateTime? BeginMonth(this DateTime? date)
        {
            return date?.BeginMonth();
        }

        #endregion

        #region Methods to Manipulate Year Components

        /// <summary>
        /// Returns a new DateTime value representing the start of the specified year.
        /// </summary>
        /// <param name="date">The original DateTime value.</param>
        /// <returns>A new DateTime value set to January 1st of the year at 00:00:00.</returns>
        public static DateTime BeginYear(this DateTime date)
        {
            return new DateTime(date.Year, 1, 1, 0, 0, 0, date.Kind);
        }

        /// <summary>
        /// Returns a new DateTime value representing the start of the specified year.
        /// </summary>
        /// <param name="date">The original nullable DateTime value.</param>
        /// <returns>A new DateTime value set to January 1st of the year, or null if input is null.</returns>
        public static DateTime? BeginYear(this DateTime? date)
        {
            return date?.BeginYear();
        }

        #endregion

        #region Methods to Calculate Date Ranges

        /// <summary>
        /// Returns a DateTime value representing 00:00:00 of the previous day.
        /// </summary>
        /// <param name="date">The DateTime value to calculate from.</param>
        public static DateTime Yesterday(this DateTime date)
        {
            return date.AddDays(-1).BeginDay();
        }

        #endregion

        #region Thread-Safe Timestamp Handling

        private static long _lastTimestamp = DateTime.UtcNow.Ticks;
        private static readonly object _timestampLock = new object();

        /// <summary>
        /// Gets a thread-safe timestamp with microsecond precision.
        /// Note: May return duplicate values on systems with low-resolution clocks.
        /// </summary>
        public static long NowTicks
        {
            get
            {
                lock (_timestampLock)
                {
                    var current = DateTime.UtcNow.Ticks;
                    _lastTimestamp = current > _lastTimestamp ? current : _lastTimestamp + 1;
                    return _lastTimestamp;
                }
            }
        }

        #endregion

        #region Methods for Precise Time Handling

        /// <summary>
        /// Returns a DateTime instance with a thread-safe high-precision timestamp.
        /// </summary>
        public static DateTime ExactNow()
        {
            return new DateTime(NowTicks, DateTimeKind.Utc);
        }

        /// <summary>
        /// Returns a thread-safe high-precision timestamp in ticks.
        /// </summary>
        public static long ExactTicks()
        {
            return NowTicks;
        }

        #endregion
    }
}

