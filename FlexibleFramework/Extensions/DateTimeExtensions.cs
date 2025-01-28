using System;
using System.Threading;

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
        /// <param name="d">
        ///   The DateTime value to check.
        /// </param>
        /// <returns>
        ///   A boolean indicating whether the DateTime value has a non-zero time component.
        /// </returns>
        public static bool HasTime(this DateTime d)
        {
            return d.Hour > 0 || d.Minute > 0 || d.Second > 0 || d.Millisecond > 0;
        }

        /// <summary>
        /// Checks if the specified DateTime value has any non-zero time components (hours, minutes, seconds, or milliseconds).
        /// </summary>
        /// <param name="d">
        ///   The DateTime value to check.
        /// </param>
        /// <returns>
        ///   A boolean indicating whether the DateTime value has a non-zero time component.
        /// </returns>
        public static bool HasTime(this DateTime? d)
        {
            if (d == null)
                return false;
            return d.Value.HasTime();
        }

        #endregion

        #region Methods to Reset Time Components

        /// <summary>
        /// Returns a new DateTime value representing the start of the specified date.
        /// The time components (hours, minutes, seconds, milliseconds) are set to zero.
        /// </summary>
        /// <param name="dt">
        ///   The original DateTime value. If null, returns DateTime.MinValue.
        /// </param>
        /// <returns>
        ///   A new DateTime value with the date part preserved and time parts reset to zero.
        /// </returns>
        public static DateTime BeginDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, date.Kind);
        }

        /// <summary>
        /// Returns a new DateTime value representing the start of the specified date.
        /// The time components (hours, minutes, seconds, milliseconds) are set to zero.
        /// </summary>
        /// <param name="dt">
        ///   The original DateTime value. If null, returns DateTime.MinValue.
        /// </param>
        /// <returns>
        ///   A new DateTime value with the date part preserved and time parts reset to zero.
        /// </returns>
        public static DateTime? BeginDay(this DateTime? date)
        {
            if (date == null)
                return null;
            return date.Value.BeginDay();
        }

        #endregion

        #region Methods to Set Time Components

        /// <summary>
        /// Returns a new DateTime value representing the end of the specified date.
        /// The time components are set to their maximum possible values (23:59:59.999).
        /// </summary>
        /// <param name="dt">
        ///   The original DateTime value. If null, returns DateTime.MaxValue.
        /// </param>
        /// <returns>
        ///   A new DateTime value with the date part preserved and time parts set to their maximum values.
        /// </returns>
        public static DateTime EndDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999, date.Kind);
        }

        /// <summary>
        /// Returns a new DateTime value representing the end of the specified date.
        /// The time components are set to their maximum possible values (23:59:59.999).
        /// </summary>
        /// <param name="dt">
        ///   The original DateTime value. If null, returns DateTime.MaxValue.
        /// </param>
        /// <returns>
        ///   A new DateTime value with the date part preserved and time parts set to their maximum values.
        /// </returns>
        public static DateTime? EndDay(this DateTime? date)
        {
            if (date == null) 
                return null;
            return date.Value.EndDay();
        }

        #endregion

        #region Methods to Manipulate Month Components

        /// <summary>
        /// Returns a new DateTime value representing the start of the specified month.
        /// The date part is adjusted to the first day of the month, and time components are set to zero.
        /// </summary>
        /// <param name="dt">
        ///   The original DateTime value. If null, returns DateTime.MinValue.
        /// </param>
        /// <returns>
        ///   A new DateTime value with the date part set to the first day of the month and time parts reset.
        /// </returns>
        public static DateTime BeginMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1, 0, 0, 0, date.Kind);
        }



        /// <summary>
        /// Returns a new DateTime value representing the start of the specified month.
        /// The date part is adjusted to the first day of the month, and time components are set to zero.
        /// </summary>
        /// <param name="dt">
        ///   The original DateTime value. If null, returns DateTime.MinValue.
        /// </param>
        /// <returns>
        ///   A new DateTime value with the date part set to the first day of the month and time parts reset.
        /// </returns>
        public static DateTime? BeginMonth(this DateTime? date)
        {
            if (date == null) 
                return null;
            return date.Value.BeginMonth();
        }

        #endregion

        #region Methods to Manipulate Year Components

        /// <summary>
        /// Returns a new DateTime value representing the start of the specified year.
        /// The date part is adjusted to January 1st of the given year, and time components are set to zero.
        /// </summary>
        /// <param name="dt">
        ///   The original DateTime value. If null, returns DateTime.MinValue.
        /// </param>
        /// <returns>
        ///   A new DateTime value with the date part set to January 1st of the specified year and time parts reset.
        /// </returns>
        public static DateTime BeginYear(this DateTime date)
        {
            return new DateTime(date.Year, 1, 1, 0, 0, 0, date.Kind);
        }

        /// <summary>
        /// Returns a new DateTime value representing the start of the specified year.
        /// The date part is adjusted to January 1st of the given year, and time components are set to zero.
        /// </summary>
        /// <param name="dt">
        ///   The original DateTime value. If null, returns DateTime.MinValue.
        /// </param>
        /// <returns>
        ///   A new DateTime value with the date part set to January 1st of the specified year and time parts reset.
        /// </returns>
        public static DateTime? BeginYear(this DateTime? date)
        {
            if (date == null) 
                return null;
            return date.Value.BeginYear();
        }

        #endregion

        #region Methods to Calculate Date Ranges

        /// <summary>
        /// Returns a new DateTime value representing the start of the previous day.
        /// </summary>
        /// <param name="date">
        ///   The DateTime value to calculate the previous day for.
        /// </param>
        /// <returns>
        ///   A new DateTime value set to 00:00:00 of the preceding day.
        /// </returns>
        public static DateTime Yesterday(this DateTime date)
        {
            return date.AddDays(-1).BeginDay();
        }

        #endregion

        #region Thread-Safe Timestamp Handling

        private static long lastTimeStamp = DateTime.Now.Ticks;

        /// <summary>
        /// A thread-safe way to get the current timestamp with high precision.
        /// The value is updated in a thread-safe manner to avoid race conditions.
        /// </summary>
        public static long NowTicks
        {
            get
            {
                long original, newValue;
                do
                {
                    original = lastTimeStamp;
                    long now = DateTime.Now.Ticks;
                    newValue = Math.Max(now, original + 1);
                } while (Interlocked.CompareExchange(ref lastTimeStamp, newValue, original) != original);
                return newValue;
            }
        }

        #endregion

        #region Methods for Precise Time Handling

        /// <summary>
        /// Returns a DateTime instance with the precise current timestamp.
        /// </summary>
        public static DateTime ExactNow(this DateTime _)
        {
            return new DateTime(NowTicks);
        }

        /// <summary>
        /// Returns the current timestamp in terms of ticks.
        /// This provides a more precise representation than DateTime.Now.Ticks.
        /// </summary>
        public static long ExactTicks(this DateTime _)
        {
            return NowTicks;
        }

        #endregion
    }
}
