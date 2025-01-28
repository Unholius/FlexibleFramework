using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FlexibleFramework.Extensions
{
    /// <summary>
    /// DateTime extensions
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Указано ли время в переменной
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool HasTime(this DateTime d)
        {
            return d.Hour > 0 || d.Minute > 0 || d.Second > 0 || d.Millisecond > 0;
        }

        /// <summary>
        ///     Вернуть дату с временем 0:0:0
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime BeginDay(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day);
        }

        /// <summary>
        ///     Вернуть дату с временем 0:0:0
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime BeginDay(this DateTime? date)
        {
            if (date == null) return DateTime.MinValue;
            DateTime dt = (DateTime)date;
            return new DateTime(dt.Year, dt.Month, dt.Day);
        }

        /// <summary>
        ///     Вернуть дату с временем 23:59:59.999
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime EndDay(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59, 999);
        }

        /// <summary>
        ///     Вернуть дату с временем 23:59:59.999
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime EndDay(this DateTime? date)
        {
            if (date == null) return DateTime.MaxValue;
            DateTime dt = (DateTime)date;
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59, 999);
        }

        /// <summary>
        ///     Вернуть дату с первым днем месяца и временем 0:0:0
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime BeginMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        /// <summary>
        ///     Вернуть дату с первым днем месяца и временем 0:0:0
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime BeginMonth(this DateTime? date)
        {
            if (date == null) return DateTime.MinValue;
            DateTime dt = (DateTime)date;
            return new DateTime(dt.Year, dt.Month, 1);
        }

        /// <summary>
        ///     Вернуть дату с последнем днем месяца и временем 23:59:59.999
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime EndMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month), 23, 59, 59, 999);
        }

        /// <summary>
        ///     Вернуть дату с последнем днем месяца и временем 23:59:59.999
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime EndMonth(this DateTime? date)
        {
            if (date == null) return DateTime.MaxValue;
            DateTime dt = (DateTime)date;
            return new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month), 23, 59, 59, 999);
        }

        /// <summary>
        ///     Вернуть дату с первым днем первого месяца и временем 0:0:0
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime BeginYear(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }

        /// <summary>
        ///     Вернуть дату с первым днем первого месяца и временем 0:0:0
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime BeginYear(this DateTime? date)
        {
            if (date == null) return DateTime.MinValue;
            DateTime dt = (DateTime)date;
            return new DateTime(dt.Year, 1, 1);
        }

        /// <summary>
        ///     Вернуть дату с последнем днем последнего месяца и временем 0:0:0
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime EndYear(this DateTime dt)
        {
            return new DateTime(dt.Year, 12, DateTime.DaysInMonth(dt.Year, 12), 23, 59, 59, 999);
        }

        /// <summary>
        ///     Вернуть дату с последнем днем последнего месяца и временем 0:0:0
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime EndYear(this DateTime? date)
        {
            if (date == null) return DateTime.MaxValue;
            DateTime dt = (DateTime)date;
            return new DateTime(dt.Year, 12, DateTime.DaysInMonth(dt.Year, 12), 23, 59, 59, 999);
        }


        /// <summary>
        ///     Вернуть начало вчерашнего дня
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime Yesterday(this DateTime date)
        {
            return date.AddDays(-1).BeginDay();
        }

        /// <summary>
        ///     Вернуть начало вчерашнего дня
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime Yesterday(this DateTime? date)
        {
            return date?.AddDays(-1).BeginDay() ?? throw new NullReferenceException("DateTimeExtensions.Yesterday: Дата не должна быть <null>!");
        }

        private static long lastTimeStamp = DateTime.Now.Ticks;

        /// <summary>
        ///     Более точный <see cref="DateTime.Ticks"/>
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

        /// <summary>
        ///     Более точный <see cref="DateTime.Now"/>
        /// </summary>
        public static DateTime ExactNow(this DateTime _)
        {
            return new DateTime(NowTicks);
        }

        /// <summary>
        ///     Более точный <see cref="DateTime.Ticks"/>
        /// </summary>
        public static long ExactTicks(this DateTime _)
        {
            return NowTicks;
        }
    }
}
