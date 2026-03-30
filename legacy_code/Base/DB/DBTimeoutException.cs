using System;
using System.Collections.Generic;
using System.Text;

namespace Base.DB
{
    public class TimeoutException : Exception
    {
        public TimeoutException()
            : base()
        {
        }

        public TimeoutException(String msg)
            : base(msg)
        {
        }


    }
}
