using System;
using System.Collections.Generic;
using System.Text;

using Base.Biz;

namespace WorkFlow.WrkFlw
{
    public class WrkFlwBO : BaseBO
    {
        public WrkFlw GetWrkFlw(int wrkFlwID)
        {
            this.WhereClause = "WrkFlwID=" + wrkFlwID;
            return Query(new WrkFlw()).Dequeue() as WrkFlw;
        }

    }
}
