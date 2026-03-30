using System;
using System.Collections.Generic;
using System.Text;

using Base.Biz;
using Base.DB;

namespace BaseInfo.Dept
{
    public class DeptBO : BaseBO
    {
        public Resultset GetAllDepts()
        {
            OrderBy = "DeptID";
            return Query(new Dept());
        }
    }
}
