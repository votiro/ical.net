using System.Collections.Generic;

namespace Ical.Net
{
    internal static class KnownTokens
    {
       private static readonly HashSet<string> Tokens = new HashSet<string>(new []
       {
           "BEGIN",
           "END",
           "VCALENDAR",
           "VEVENT",
           "VTODO",
           "VJOURNAL",
           "VFREEBUSY",
           "VTIMEZONE",
           "VALARM",
           "STANDARD",
           "DAYLIGHT",
           "VAVAILABILITY",
           "AVAILABLE",
           "CALSCALE",
           "METHOD",
           "PRODID",
           "VERSION",
           "ATTACH",
           "CATEGORIES",
           "CATEGORY",
           "CLASS",
           "COMMENT",
           "DESCRIPTION",
           "GEO",
           "LOCATION",
           "PERCENT-COMPLETE",
           "PRIORITY",
           "RESOURCES",
           "STATUS",
           "SUMMARY",
           "COMPLETED",
           "DTEND",
           "DUE",
           "DTSTART",
           "DURATION",
           "FREEBUSY",
           "TRANSP",
           "TZID",
           "TZNAME",
           "TZOFFSETFROM",
           "TZOFFSETTO",
           "TZURL",
           "ATTENDEE",
           "CONTACT",
           "ORGANIZER",
           "RECURRENCE-ID",
           "RELATED-TO",
           "URL",
           "UID",
           "EXDATE",
           "EXRULE",
           "RDATE",
           "RRULE",
           "RULE",
           "ACTION",
           "REPEAT",
           "TRIGGER",
           "CREATED",
           "DTSTAMP",
           "LAST-MODIFIED",
           "SEQUENCE",
           "REQUEST-STATUS",
           "XML",
           "TZUNTIL",
           "TZID-ALIAS-OF",
           "BUSYTYPE",
           "NAME",
           "REFRESH-INTERVAL",
           "SOURCE",
           "COLOR",
           "IMAGE",
           "CONFERENCE"
       });

       public static bool IsKnownToken(string token)
       {
           string normalized = token.ToUpper();

           if (normalized.StartsWith("X-") || normalized.StartsWith(" ") || normalized.StartsWith("\t"))
               return true;

           foreach (string knownToken in Tokens)
           {
               if (normalized.StartsWith(knownToken))
                   return true;
           }

           return false;
       }
    }
}
