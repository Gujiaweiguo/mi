using System;
using System.Collections.Generic;
using System.Text;

namespace WorkFlow.WrkFlw
{
    public class WrkFlwException : Exception
    {
        public WrkFlwException(String msg)
            : base(msg)
        {
        }
    }
}
