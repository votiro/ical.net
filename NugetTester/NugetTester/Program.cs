using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Ical.Net;
using Ical.Net.DataTypes;
using Ical.Net.Interfaces.Components;
using Ical.Net.Interfaces.DataTypes;
using Ical.Net.Serialization.iCalendar.Serializers;
using NodaTime;
using Calendar = Ical.Net.Calendar;

namespace NugetTester
{
    class Program
    {
        private static DateTime _now = DateTime.UtcNow;
        private static DateTime _later = _now.AddHours(1);

        static void Main(string[] args)
        {
            //var calendarEvent = new Event
            //{
            //    Start = new CalDateTime(_now, "UTC"),
            //    End = new CalDateTime(_later, "UTC"),
            //}

            var timeZone = DateTimeZoneProviders.Tzdb["America/Toronto"];
            var localTime = LocalDateTime.FromDateTime(DateTime.Parse("2016-11-06 02:30"));
            var zonedTime = localTime.InZoneLeniently(timeZone);

            var foo = new DirectoryInfo(@"C:\git\legacyplant\src\Direct\WindowsUI\Deploy\temp\bin\Debug");

            Console.ReadLine();
        }

        private static Event DeserializeCalendarEvent(string ical)
        {
            var calendar = DeserializeCalendar(ical);
            var calendarEvent = calendar.First().Events.First() as Event;
            return calendarEvent;
        }

        private static CalendarCollection DeserializeCalendar(string ical)
        {
            using (var reader = new StringReader(ical))
            {
                return Calendar.LoadFromStream(reader) as CalendarCollection;
            }
        }

        private static string SerializeToString(IEvent calendarEvent) => SerializeToString(new Calendar { Events = { calendarEvent } });

        private static string SerializeToString(Calendar iCalendar) => new CalendarSerializer().SerializeToString(iCalendar);
    }
}
