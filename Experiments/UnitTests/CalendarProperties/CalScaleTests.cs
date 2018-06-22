using System;
using System.Linq;
using Experiments.CalendarProperties;
using Experiments.Utilities;
using NUnit.Framework;

namespace UnitTests.CalendarProperties
{
    public class CalScaleTests
    {
        private static readonly StringComparer _defaultComparer = StringComparer.Ordinal;
        private static readonly string[] _properties = {"foo", "foo", "bar", "baz"};
        private static readonly string[] _expectedProperties = _properties.Distinct(_defaultComparer).OrderBy(w => w, _defaultComparer).ToArray();
        private static readonly string _expectedPropertyString = string.Join(";", _expectedProperties);

        [Test]
        public void CalScaleTesting()
        {
            const string calScale = "JULIAN";
            var simpleScale = new CalScale(calScale);
            Assert.AreEqual($"CALSCALE:{calScale}" + FormattingCodepoints.LineBreak, simpleScale.ToString());
            Assert.AreEqual(calScale, simpleScale.Value);
            Assert.IsTrue(simpleScale.Properties == null);

            var withProperties = new CalScale(calScale, _properties);
            CollectionAssert.AreEqual(withProperties.Properties, _expectedProperties);
            var expectedPropertyString = $"CALSCALE;{_expectedPropertyString}:JULIAN{FormattingCodepoints.LineBreak}";
            Assert.AreEqual(withProperties.ToString(), expectedPropertyString);

            var defaultScale = new CalScale();
            Assert.AreEqual("CALSCALE:GREGORIAN" + FormattingCodepoints.LineBreak, defaultScale.ToString());
        }
    }
}
