using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace BaseInfo.Role
{
    public class FuncTree : BasePO
    {
        private int bizgrpid = 0;
        private int funcID = 0;
        private string funcname = "";


        public static int FUNC_TYPE_FUNCTION = 0;
        public static int FUNC_TYPE_FUNCTION1 = 1;
        public static int FUNC_TYPE_FUNCTION2 = 2;
        public static int FUNC_TYPE_FUNCTION3 = 3;


        public override string GetTableName()
        {
            return "";
        }

        public override String GetColumnNames()
        {
            return "BizGrpID,FuncID,FuncName";
        }
        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public override string GetQuerySql()
        {
            return "select a.BizGrpID,FuncID,FuncName from BizGrp a,Func b ";
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
    }
}
