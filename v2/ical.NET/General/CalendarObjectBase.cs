using System;
using Ical.Net.Interfaces.General;

namespace Ical.Net.General
{
    public class CalendarObjectBase : ICopyable, ILoadable, ICloneable
    {
        private bool _mIsLoaded;

        public CalendarObjectBase()
        {
            // Objects that are loaded using a normal constructor
            // are "Loaded" by default.  Objects that are being
            // deserialized do not use the constructor.
            _mIsLoaded = true;
        }

        protected CalendarObjectBase(CalendarObjectBase cob)
        {
            _mIsLoaded = true;
        }

        /// <summary>
        /// Copies values from the target object to the
        /// current object.
        /// </summary>
        public virtual void CopyFrom(ICopyable c) {}

        /// <summary>
        /// Creates a copy of the object.
        /// </summary>
        /// <returns>The copy of the object.</returns>
        public virtual T Copy<T>()
        {
            var type = GetType();
            var obj = Activator.CreateInstance(type) as ICopyable;
            //try
            //{
            //    var clone = (T) Clone();
            //    return clone;
            //}
            //catch (Exception e)
            //{
            //    return default(T);
            //}

            // Duplicate our values
            if (obj is T)
            {
                obj.CopyFrom(this);
                return (T)obj;
            }
            return default(T);
        }

        public virtual bool IsLoaded => _mIsLoaded;

        [field: NonSerialized]
        public event EventHandler Loaded;

        public virtual void OnLoaded()
        {
            _mIsLoaded = true;
            Loaded?.Invoke(this, EventArgs.Empty);
        }

        public virtual object Clone() => new CalendarObjectBase();
    }
}