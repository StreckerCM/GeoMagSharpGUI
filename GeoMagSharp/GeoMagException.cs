using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoMagSharp
{
    class GeoMagException : Exception
    {
        public GeoMagException()
            : base() { }

        public GeoMagException(string message)
            : base(message) { }

        public GeoMagException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public GeoMagException(string message, Exception innerException)
            : base(message, innerException) { }

        public GeoMagException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }
    }
}
