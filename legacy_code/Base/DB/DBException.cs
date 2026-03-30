using System;
using System.Collections.Generic;
using System.Text;

namespace Base.DB
{
    public class DBException : Exception
    {
        
        public DBException()
            : base()
        {
        }

        public DBException(String msg)
            : base(msg)
        {
        }
    }
}
