using System;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Runtime.Serialization;
using Ical.Net.Serialization.DataTypes;

namespace Ical.Net.DataTypes
{
    /// <summary>
    /// A class that represents the organizer of a VEVENT, VTODO, or VJOURNAL
    /// </summary>
    [DebuggerDisplay("{Value}")]
    public class Organizer : EncodableDataType
    {
        /// <summary>
        /// Represents an RFC-5545 ORGANIZER.
        /// </summary>
        /// <param name="commonName">The display name of the organizer</param>
        /// <param name="email">An optional email address for the organizer</param>
        /// <param name="uri">An optional URI for the organizer</param>
        /// <param name="sentBy">The individual acting on behalf of the calendar owner</param>
        public Organizer(string commonName, string email, string uri, string sentBy)
        {
            var mailAddress = GetEmailAddress(email);
            CommonName = commonName ?? mailAddress.DisplayName;
            SentBy = Uri.TryCreate(sentBy, UriKind.RelativeOrAbsolute, out var sentByResult)
                ? sentByResult
                : null;


        }
        

        public Organizer(string commonName)
            : this(commonName, null, null, null) { }

        private static MailAddress GetEmailAddress(string email)
        {
            try
            {
                return new MailAddress(email);
            }
            catch (Exception) { }
            return null;
        }

        public string CommonNameKey => "CN";
        public string CommonName { get; }

        public string DirectoryEntryKey => "DIR";
        public Uri DirectoryEntry { get; }

        public string SentByKey => "SENT-BY";
        public Uri SentBy { get; }

        public string EmailAddress { get; }

        public virtual Uri Value { get; set; }

        public static Organizer Parse(string rfcOrganizer)
        {
            throw new NotImplementedException();
            //if (string.IsNullOrWhiteSpace(rfcOrganizer))
            //{
            //    return null;
            //}

            //var serializer = new OrganizerSerializer();
            //CopyFrom(serializer.Deserialize(new StringReader(rfcOrganizer)) as ICopyable);
        }

        protected bool Equals(Organizer other) => Equals(Value, other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((Organizer) obj);
        }

        public override int GetHashCode() => Value?.GetHashCode() ?? 0;

        public override void CopyFrom(ICopyable obj)
        {
            base.CopyFrom(obj);

            var o = obj as Organizer;
            if (o != null)
            {
                Value = o.Value;
            }
        }
    }
}