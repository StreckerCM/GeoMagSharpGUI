using System;
using System.Runtime.Serialization;

namespace GeoMagSharp
{    
    /// <summary>
    /// Base class for all exceptions.
    /// </summary>
    [Serializable]
    public class GeoMagException : Exception
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

    /// <summary>
    /// Thrown when a magnetic model has not been loaded before performing calculations.
    /// </summary>
    [Serializable]
    public class GeoMagExceptionModelNotLoaded : GeoMagException
    {
        public GeoMagExceptionModelNotLoaded()
        {
        }

        public GeoMagExceptionModelNotLoaded(string message)
            : base(message)
        {
        }

        public GeoMagExceptionModelNotLoaded(string message, Exception inner)
            : base(message, inner)
        {
        }

        public GeoMagExceptionModelNotLoaded(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }


    /// <summary>
    /// Thrown when a coefficient file cannot be found at the specified path.
    /// </summary>
    [Serializable]
    public class GeoMagExceptionFileNotFound : GeoMagException
    {
        public GeoMagExceptionFileNotFound()
        {
        }

        public GeoMagExceptionFileNotFound(string message) 
            : base(message)
        {
        }

        public GeoMagExceptionFileNotFound(string message, Exception inner) 
            : base(message, inner)
        {
        }

        public GeoMagExceptionFileNotFound(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    /// <summary>
    /// Thrown when a coefficient file contains an invalid character in a data line.
    /// </summary>
    [Serializable]
    public class GeoMagExceptionBadCharacter : GeoMagException
    {
        public GeoMagExceptionBadCharacter()
        {
        }

        public GeoMagExceptionBadCharacter(string message)
            : base(message)
        {
        }

        public GeoMagExceptionBadCharacter(string message, Exception inner)
            : base(message, inner)
        {
        }

        public GeoMagExceptionBadCharacter(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }

    /// <summary>
    /// Thrown when a coefficient file contains an unexpected number of coefficients.
    /// </summary>
    [Serializable]
    public class GeoMagExceptionBadNumberOfCoefficients : GeoMagException
    {
        public GeoMagExceptionBadNumberOfCoefficients()
        {
        }

        public GeoMagExceptionBadNumberOfCoefficients(string message)
            : base(message)
        {
        }

        public GeoMagExceptionBadNumberOfCoefficients(string message, Exception inner)
            : base(message, inner)
        {
        }

        public GeoMagExceptionBadNumberOfCoefficients(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    /// <summary>
    /// Thrown when a coefficient file cannot be opened or read.
    /// </summary>
    [Serializable]
    public class GeoMagExceptionOpenError : GeoMagException
    {
        public GeoMagExceptionOpenError()
        {
        }

        public GeoMagExceptionOpenError(string message)
            : base(message)
        {
        }

        public GeoMagExceptionOpenError(string message, Exception inner)
            : base(message, inner)
        {
        }

        public GeoMagExceptionOpenError(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    /// <summary>
    /// Thrown when a calculation date or parameter is outside the valid range.
    /// </summary>
    [Serializable]
    public class GeoMagExceptionOutOfRange : GeoMagException
    {
        public GeoMagExceptionOutOfRange()
        {
        }

        public GeoMagExceptionOutOfRange(string message)
            : base(message)
        {
        }

        public GeoMagExceptionOutOfRange(string message, Exception inner)
            : base(message, inner)
        {
        }

        public GeoMagExceptionOutOfRange(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    /// <summary>
    /// Thrown when a calculation or model loading operation exceeds available memory.
    /// </summary>
    [Serializable]
    public class GeoMagExceptionOutOfMemory : GeoMagException
    {
        public GeoMagExceptionOutOfMemory()
        {
        }

        public GeoMagExceptionOutOfMemory(string message)
            : base(message)
        {
        }

        public GeoMagExceptionOutOfMemory(string message, Exception inner)
            : base(message, inner)
        {
        }

        public GeoMagExceptionOutOfMemory(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
    
}
