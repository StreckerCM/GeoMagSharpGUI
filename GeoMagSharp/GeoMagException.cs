using System;
using System.Runtime.Serialization;

namespace GeoMagSharp
{    
    /// <summary>
    /// Base class for all exceptions.
    /// </summary>
    [Serializable]
    class GeoMagException : Exception
    {
        /// <summary>
        /// Creates a new instance of <seealso cref="GeoMagException"/>.
        /// </summary>
        protected GeoMagException()
        {
        }

        /// <summary>
        /// Creates a new instance of <seealso cref="GeoMagException"/>.
        /// </summary>
        /// <param name="message">error message</param>
        protected GeoMagException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of <seealso cref="GeoMagException"/>.
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="inner">inner exception</param>
        protected GeoMagException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Creates a new instance of <seealso cref="GeoMagException"/>.
        /// </summary>
        /// <param name="info">serialization information</param>
        /// <param name="context">streaming context</param>
        protected GeoMagException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
