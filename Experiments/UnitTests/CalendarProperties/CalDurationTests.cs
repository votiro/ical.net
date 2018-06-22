using System.Collections.Generic;
using Experiments.ComponentProperties;
using NodaTime;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace UnitTests.CalendarProperties
{
    public class CalDurationTests
    {
        [Test, TestCaseSource(nameof(ValueTestCases))]
        public void ValueTests(Duration duration, string expectedString, string expectedSerialized)
        {
            var calDuration = new CalDuration(duration);
            Assert.AreEqual(calDuration.Value, expectedString);
            Assert.AreEqual(calDuration.ToString(), expectedSerialized);
        }

        public static IEnumerable<ITestCaseData> ValueTestCases()
        {
            yield return new TestCaseData(Duration.FromHours(1), "PT1H", "DURATION:PT1H\r\n")
                .SetName("One hour, 0 minutes, 0 seconds");

            yield return new TestCaseData(Duration.FromMinutes(15), "PT15M", "DURATION:PT15M\r\n")
                .SetName("15 minutes");
        }
    }
}