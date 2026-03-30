using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace BaseInfo.Role
{
    public class BizGrpTree:BasePO
    {
        private int bizgrpid = 0;
        private int menuid = 0;
        private string bizgrpname = "";

        public override string GetTableName()
        {
            return "";
        }

        public override String GetColumnNames()
        {
            return "BizGrpID,MenuID,BizGrpName";
        }
        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public override string GetQuerySql()
        {
            return "select a.BizGrpID,BizGrpName,MenuID from BizGrp a,Func b where a.BizGrpID=b.BizGrpID group by a.BizGrpID,BizGrpName,MenuID";
        }


        public int BizGrpID
        {
            set { bizgrpid = value; }
            get { return bizgrpid; }
        }

        public int MenuID
        {
            set { menuid = value; }
            get { return menuid; }
        }

        public string BizGrpName
        {
            set { bizgrpname = value; }
            get { return bizgrpname; }
        }

    }
}
