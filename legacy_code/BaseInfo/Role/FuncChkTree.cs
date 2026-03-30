using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace BaseInfo.Role
{
    public class FuncChkTree:BasePO
    {
        private int bizgrpid = 0;
        private int funcID = 0;
        private string funcname = "";
        private string fckstatus = "";
        public override string GetTableName()
        {
            return "";
        }

        public override String GetColumnNames()
        {
            return "BizGrpID,FuncID,FuncName,fckStatus";
        }
        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public int BizGrpID
        {
            set { bizgrpid = value; }
            get { return bizgrpid; }
        }

        public int FuncID
        {
            set { funcID = value; }
            get { return funcID; }
        }

        public string FuncName
        {
            set { funcname = value; }
            get { return funcname; }
        }

        public string fckStatus
        {
            set { fckstatus = value; }
            get { return fckstatus; }
        }
    }
}
