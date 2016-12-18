using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Ical.Net.DataTypes;
using Ical.Net.Interfaces.DataTypes;
using NUnit.Framework;

namespace Ical.Net.UnitTests
{
    internal class GetOccurrenceTests
    {
        public static CalendarCollection GetCalendars(string incoming) =>
            Calendar.LoadFromStream(new StringReader(incoming)) as CalendarCollection;

        [Test]
        public void WrongDurationTest()
        {
            var firstStart = new CalDateTime(DateTime.Parse("2016-01-01"));
            var firstEnd = new CalDateTime(DateTime.Parse("2016-01-05"));
            var vEvent = new Event
            {
                DtStart = firstStart,
                DtEnd = firstEnd,
            };

            var secondStart = new CalDateTime(DateTime.Parse("2016-03-01"));
            var secondEnd = new CalDateTime(DateTime.Parse("2016-03-05"));
            var vEvent2 = new Event
            {
                DtStart = secondStart,
                DtEnd = secondEnd,
            };

            var calendar = new Calendar();
            calendar.Events.Add(vEvent);
            calendar.Events.Add(vEvent2);

            var searchStart = DateTime.Parse("2015-12-29");
            var searchEnd = DateTime.Parse("2017-02-10");
            var occurrences = calendar.GetOccurrences(searchStart, searchEnd).OrderBy(o => o.Period.StartTime).ToList();

            var firstOccurrence = occurrences.First();
            var firstStartCopy = firstStart.Copy<CalDateTime>();
            var firstEndCopy = firstEnd.Copy<CalDateTime>();
            Assert.AreEqual(firstStartCopy, firstOccurrence.Period.StartTime);
            Assert.AreEqual(firstEndCopy, firstOccurrence.Period.EndTime);

            var secondOccurrence = occurrences.Last();
            var secondStartCopy = secondStart.Copy<CalDateTime>();
            var secondEndCopy = secondEnd.Copy<CalDateTime>();
            Assert.AreEqual(secondStartCopy, secondOccurrence.Period.StartTime);
            Assert.AreEqual(secondEndCopy, secondOccurrence.Period.EndTime);
        }

        [Test]
        public void EnumerationChangedException()
        {
            const string ical = @"BEGIN:VCALENDAR
PRODID:-//Google Inc//Google Calendar 70.9054//EN
VERSION:2.0
CALSCALE:GREGORIAN
METHOD:PUBLISH
X-WR-CALNAME:name
X-WR-TIMEZONE:America/New_York
BEGIN:VTIMEZONE
TZID:America/New_York
X-LIC-LOCATION:America/New_York
BEGIN:DAYLIGHT
TZOFFSETFROM:-0500
TZOFFSETTO:-0400
TZNAME:EDT
DTSTART:19700308T020000
RRULE:FREQ=YEARLY;BYMONTH=3;BYDAY=2SU
END:DAYLIGHT
BEGIN:STANDARD
TZOFFSETFROM:-0400
TZOFFSETTO:-0500
TZNAME:EST
DTSTART:19701101T020000
RRULE:FREQ=YEARLY;BYMONTH=11;BYDAY=1SU
END:STANDARD
END:VTIMEZONE

BEGIN:VEVENT
DTSTART;TZID=America/New_York:20161011T170000
DTEND;TZID=America/New_York:20161011T180000
DTSTAMP:20160930T115710Z
UID:blablabla
RECURRENCE-ID;TZID=America/New_York:20161011T170000
CREATED:20160830T144559Z
DESCRIPTION:
LAST-MODIFIED:20160928T142659Z
LOCATION:Location1
SEQUENCE:0
STATUS:CONFIRMED
SUMMARY:Summary1
TRANSP:OPAQUE
END:VEVENT

END:VCALENDAR";

            var calendar = GetCalendars(ical);
            var date = new DateTime(2016, 10, 11);
            var occurrences = calendar[0].GetOccurrences(date);

            //We really want to make sure this doesn't explode
            Assert.AreEqual(1, occurrences.Count);
        }

        [Test]
        public void GetOccurrencesShouldEnumerate()
        {
            const string ical =
   @"BEGIN:VCALENDAR
PRODID:-//github.com/rianjs/ical.net//NONSGML ical.net 2.2//EN
VERSION:2.0
BEGIN:VTIMEZONE
TZID:W. Europe Standard Time
BEGIN:STANDARD
DTSTART:16010101T030000
RRULE:FREQ=YEARLY;BYDAY=SU;BYMONTH=10;BYSETPOS=-1
TZNAME:Mitteleuropäische Zeit
TZOFFSETFROM:+0200
TZOFFSETTO:+0100
END:STANDARD
BEGIN:DAYLIGHT
DTSTART:00010101T020000
RRULE:FREQ=YEARLY;BYDAY=SU;BYMONTH=3;BYSETPOS=-1
TZNAME:Mitteleuropäische Sommerzeit
TZOFFSETFROM:+0100
TZOFFSETTO:+0200
END:DAYLIGHT
END:VTIMEZONE
BEGIN:VEVENT
BACKGROUND:BUSY
DESCRIPTION:Backup Daten
DTEND;TZID=W. Europe Standard Time:20150305T043000
DTSTAMP:20161122T120652Z
DTSTART;TZID=W. Europe Standard Time:20150305T000100
RESOURCES:server
RRULE:FREQ=WEEKLY;BYDAY=MO
SUMMARY:Server
UID:a30ed847-8000-4c53-9e58-99c8f9cf7c4b
X-LIGHTSOUT-ACTION:START=WakeUp\;END=Reboot\,Force
X-LIGHTSOUT-MODE:TimeSpan
X-MICROSOFT-CDO-BUSYSTATUS:BUSY
END:VEVENT
BEGIN:VEVENT
BACKGROUND:BUSY
DESCRIPTION:Backup Daten
DTEND;TZID=W. Europe Standard Time:20161128T043000
DTSTAMP:20161122T120652Z
DTSTART;TZID=W. Europe Standard Time:20161128T000100
RECURRENCE-ID:20161128T000100
RESOURCES:server
SEQUENCE:0
SUMMARY:Server
UID:a30ed847-8000-4c53-9e58-99c8f9cf7c4b
X-LIGHTSOUT-ACTION:START=WakeUp\;END=Reboot\,Force
X-LIGHTSOUT-MODE:TimeSpan
X-MICROSOFT-CDO-BUSYSTATUS:BUSY
END:VEVENT
END:VCALENDAR
";

            var collection = Calendar.LoadFromStream(new StringReader(ical));
            var startCheck = new DateTime(2016, 11, 11);
            var occurrences = collection.GetOccurrences<Event>(startCheck, startCheck.AddMonths(1));

            Assert.IsTrue(occurrences.Count == 4);
        }

        [Test]
        public void MissingOccurrence()
        {
            var eventStart = new DateTime(2012, 10, 12, 7, 00, 00);
            
            var duration = TimeSpan.FromHours(10.5);

            var r = new RecurrencePattern(FrequencyType.Weekly, 1)
            {
                Until = DateTime.Parse("2013-04-30 23:59:59"),
                ByDay = new List<IWeekDay> {new WeekDay(DayOfWeek.Friday)}
            };

            var evt2 = new Event
            {
                Start = new CalDateTime(eventStart),
                Duration = duration,
                RecurrenceRules = new List<IRecurrencePattern> { r },
            };

            // Both statements below return the same results, but they skip Jan 4 2013. Happens in 2.2.14 and in 2.2.15
            //var occurences2 = Utility.RecurrenceUtil.GetOccurrences(evt2, new CalDateTime(startdate.AddHours(-1)),
            //    new CalDateTime(until), false);

            var searchStart = eventStart.AddHours(-1);
            var searchEnd = new DateTime(2013, 4, 30, 17, 30, 00);
            var occurrences = evt2.GetOccurrences(searchStart, searchEnd);
            Assert.AreEqual(29, occurrences.Count);

            var missingPeriod = new Period(new CalDateTime(DateTime.Parse("2013-01-04 07:00:00")));
            Assert.IsTrue(occurrences.Select(o => o.Period).Contains(missingPeriod));

            //occurences2.All(x =>
            //{
            //    Debug.WriteLine(x.Period.StartTime.Value);
            //    return true;
            //});

            //Debug.Assert(occurences2.Count == 29);
            //start = new DateTime(2012, 10, 12, 7, 00, 00);
            //foreach (var rec in occurences2.OrderBy(x => x.Period.StartTime.Value))
            //{
            //    Debug.Assert(DayOfWeek.Friday == rec.Period.StartTime.DayOfWeek);
            //    Debug.Assert(start == rec.Period.StartTime.Value);
            //    Debug.Assert(start.AddMinutes(630) == rec.Period.EndTime.Value);
            //    start = start.AddDays(7);
            //}
        }
    }
}
