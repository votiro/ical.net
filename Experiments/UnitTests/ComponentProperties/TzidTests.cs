using System.Collections.Generic;
using Experiments.ComponentProperties;
using Experiments.Utilities;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace UnitTests.ComponentProperties
{
    public class TzidTests
    {
        [Test, TestCaseSource(nameof(FormattingTestCases))]
        public string FormattingTests(string tzId)
        {
            var tzid = new Tzid(tzId);
            return tzid.ToString();
        }

        public static IEnumerable<ITestCaseData> FormattingTestCases()
        {
            var localTz = DateUtil.SystemTimeZone.Id;
            yield return new TestCaseData(localTz)
                .Returns("")
                .SetName("Local system time zone returns no value");

            const string siberia = "Europe/Volgograd";
            yield return new TestCaseData(siberia)
                .Returns($"TZID:{siberia}\r\n")
                .SetName("A Siberian time zone returns properly");
        }
    }
}