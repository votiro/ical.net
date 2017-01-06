using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Ical.Net.ExtensionMethods;
using Ical.Net.Interfaces.General;
using Ical.Net.Utility;

namespace Ical.Net.General
{
    /// <summary>
    /// A class that represents a property of the <see cref="Calendar"/>
    /// itself or one of its components.  It can also represent non-standard
    /// (X-) properties of an iCalendar component, as seen with many
    /// applications, such as with Apple's iCal.
    /// X-WR-CALNAME:US Holidays
    /// </summary>
    /// <remarks>
    /// Currently, the "known" properties for an iCalendar are as
    /// follows:
    /// <list type="bullet">
    ///     <item>ProdID</item>
    ///     <item>Version</item>
    ///     <item>CalScale</item>
    ///     <item>Method</item>
    /// </list>
    /// There may be other, custom X-properties applied to the calendar,
    /// and X-properties may be applied to calendar components.
    /// </remarks>
    [DebuggerDisplay("{Name}:{Value}")]
    public class CalendarProperty : CalendarObject, ICalendarProperty
    {
        private List<object> _values = new List<object>(128);

        /// <summary>
        /// Returns a list of parameters that are associated with the iCalendar object.
        /// </summary>
        public virtual IParameterCollection Parameters { get; protected set; } = new ParameterList();

        public CalendarProperty() {}

        public CalendarProperty(string name) : base(name) {}

        public CalendarProperty(string name, object value) : base(name)
        {
            var asClonable = value as ICloneable;
            if (asClonable == null)
            {
                return;
            }
            _values.Add(asClonable);
        }

        public CalendarProperty(int line, int col) : base(line, col) {}

        protected CalendarProperty(CalendarProperty other) : base(other)
        {
            Value = other.Value;
            var clonedParameters = CollectionHelpers.Clone(other.Parameters);
            var parameterList = new ParameterList();
            foreach (var parameter in clonedParameters)
            {
                parameterList.Add(parameter);
            }
            Parameters = parameterList;

            if (other._values == null || !other._values.Any())
            {
                return;
            }

            if (other._values.First() is ValueType)
            {
                var list = new List<object>(other._values);
                _values = list;
            }
            else
            {
                var clonableValues = other._values.Cast<ICloneable>().Where(v => v != null).ToList();
                _values = CollectionHelpers.Clone(clonableValues).Cast<object>().ToList();
            }
        }

        /// <summary>
        /// Adds a parameter to the iCalendar object.
        /// </summary>
        public virtual void AddParameter(string name, string value)
        {
            var p = new CalendarParameter(name, value);
            Parameters.Add(p);
        }

        /// <summary>
        /// Adds a parameter to the iCalendar object.
        /// </summary>
        public virtual void AddParameter(CalendarParameter p)
        {
            Parameters.Add(p);
        }

        public override void CopyFrom(ICopyable obj)
        {
            base.CopyFrom(obj);

            var p = obj as ICalendarProperty;
            if (p == null)
            {
                return;
            }

            // Copy/clone the object if possible (deep copy)
            if (p.Values is ICopyable)
            {
                SetValue(((ICopyable)p.Values).Copy<object>());
            }
            //else if (p.Values is List<object>)
            //{
            //    //Do stuff
            //    var foo = true;
            //    var newValues = p.Values.Select(i => i.Copy())
            //}
            else
            {
                SetValue(p.Values);
            }

            // Copy parameters
            foreach (var parm in p.Parameters)
            {
                this.AddChild(parm.Copy<CalendarParameter>());
            }
            SetValue(p.Values);
        }

        public override object Clone()
        {
            return new CalendarProperty(this);
            //var clone = base.Clone() as CalendarProperty;
            //if (clone == null)
            //{
            //    return null;
            //}

            //clone.Value = Value;
            //var clonedParameters = CollectionHelpers.Clone(Parameters);
            //var parameterList = new ParameterList();
            //foreach (var parameter in clonedParameters)
            //{
            //    parameterList.Add(parameter);
            //}
            //clone.Parameters = parameterList;
            //var clonableValues = _values.Cast<ICloneable>().Where(p => p != null).ToList();
            //clone._values = CollectionHelpers.Clone(clonableValues).Cast<object>().ToList();
            //return clone;
        }

        public virtual IEnumerable<object> Values => _values;

        public object Value
        {
            get
            {
                return _values?.FirstOrDefault();
            }
            set
            {
                if (value == null)
                {
                    _values = null;
                    return;
                }

                if (_values != null && _values.Count > 0)
                {
                    _values[0] = value;
                }
                else
                {
                    _values?.Clear();
                    _values?.Add(value);
                }
            }
        }

        public virtual bool ContainsValue(object value)
        {
            return _values.Contains(value);
        }

        public virtual int ValueCount => _values?.Count ?? 0;

        public virtual void SetValue(object value)
        {
            if (_values.Count == 0)
            {
                _values.Add(value);
            }
            else if (value != null)
            {
                // Our list contains values.  Let's set the first value!
                _values[0] = value;
            }
            else
            {
                _values.Clear();
            }
        }

        public virtual void SetValue(IEnumerable<object> values)
        {
            // Remove all previous values
            _values.Clear();
            _values.AddRange(values);
        }

        public virtual void AddValue(object value)
        {
            if (value == null)
            {
                return;
            }

            _values.Add(value);
        }

        public virtual void RemoveValue(object value)
        {
            if (value == null)
            {
                return;
            }
            _values.Remove(value);
        }
    }
}