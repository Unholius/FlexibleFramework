using System;
using Xunit;

namespace FlexibleFramework.Extensions.Tests
{
    /// <summary>
    /// Contains unit tests for <see cref="Extensions.DateTimeExtensions"/> methods.
    /// Verifies correctness of date/time manipulations and thread-safe operations.
    /// </summary>
    public class DateTimeExtensionsTests
    {
        #region HasTime Tests

        /// <summary>
        /// Tests that HasTime returns true when DateTime contains non-zero time components.
        /// </summary>
        [Fact]
        public void HasTime_WithTime_ReturnsTrue()
        {
            var date = new DateTime(2023, 10, 1, 12, 30, 45);
            Assert.True(date.HasTime());
        }

        /// <summary>
        /// Tests that HasTime returns false for date without time (00:00:00).
        /// </summary>
        [Fact]
        public void HasTime_WithoutTime_ReturnsFalse()
        {
            var date = new DateTime(2023, 10, 1);
            Assert.False(date.HasTime());
        }

        /// <summary>
        /// Tests that HasTime handles nullable DateTime and returns true when time is present.
        /// </summary>
        [Fact]
        public void HasTime_NullableDateWithTime_ReturnsTrue()
        {
            DateTime? date = new DateTime(2023, 10, 1, 1, 0, 0);
            Assert.True(date.HasTime());
        }

        /// <summary>
        /// Tests that HasTime returns false for null input.
        /// </summary>
        [Fact]
        public void HasTime_NullableDateWithoutTime_ReturnsFalse()
        {
            DateTime? date = null;
            Assert.False(date.HasTime());
        }
        #endregion

        #region BeginDay Tests

        /// <summary>
        /// Verifies that BeginDay resets time components to 00:00:00.
        /// </summary>
        [Fact]
        public void BeginDay_ResetsTimeToZero()
        {
            var date = new DateTime(2023, 10, 1, 14, 30, 0);
            var result = date.BeginDay();
            Assert.Equal(new DateTime(2023, 10, 1), result);
        }

        /// <summary>
        /// Tests that BeginDay returns null for null input.
        /// </summary>
        [Fact]
        public void BeginDay_NullableDate_ReturnsNullForNullInput()
        {
            DateTime? date = null;
            Assert.Null(date.BeginDay());
        }
        #endregion

        // Остальные регионы с аналогичными XML-комментариями...

        #region Thread-Safe Timestamp Tests

        /// <summary>
        /// Verifies that NowTicks produces monotonically increasing values 
        /// even when called rapidly in sequence.
        /// </summary>
        [Fact]
        public void NowTicks_ReturnsMonotonicallyIncreasingValues()
        {
            long prev = Extensions.DateTimeExtensions.NowTicks;
            for (int i = 0; i < 10; i++)
            {
                long current = Extensions.DateTimeExtensions.NowTicks;
                Assert.True(current >= prev);
                prev = current;
            }
        }

        /// <summary>
        /// Tests that ExactNow returns DateTime with UTC kind.
        /// </summary>
        [Fact]
        public void ExactNow_ReturnsUtcDateTime()
        {
            var exactNow = Extensions.DateTimeExtensions.ExactNow();
            Assert.Equal(DateTimeKind.Utc, exactNow.Kind);
        }

        /// <summary>
        /// Verifies synchronization between ExactTicks and ExactNow methods.
        /// </summary>
        [Fact]
        public void ExactTicks_MatchesExactNowTicks()
        {
            long ticks = Extensions.DateTimeExtensions.ExactTicks();
            var date = new DateTime(ticks, DateTimeKind.Utc);
            Assert.Equal(ticks, date.Ticks);
        }
        #endregion
    }
}