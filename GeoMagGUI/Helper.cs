﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoMagGUI
{
    public static class Helper
    {
        public static bool IsNumeric(Object expression)
        {
            if (expression == null || expression is DateTime)
                return false;

            if (expression is Int16 || expression is Int32 || expression is Decimal || expression is Single || expression is Double || expression is Boolean)
                return true;

            try
            {
                if (expression is string)
                    Double.Parse(expression as string);
                else
                    Double.Parse(expression.ToString());
                return true;
            }
            catch (Exception)
            { } // just dismiss errors but return false
            return false;
        }

    }
}
