using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Util
{
    public class Utils
    {
        private static Char[] FIELD_TRIMED_CHARS ={ '[', ']', ' ' };

        /**
         * 用来取出字段中的界定符，如关键字的“[]”
         */
        public static String TrimField(String field)
        {

            if (field != null && field.Trim().Length > 0)
            {
                return field.Trim(FIELD_TRIMED_CHARS);
            }

            return field;
        }
    }
}
