using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace BaseInfo.Func
{
    public class BaseFunc:BasePO
    {
        public static int FUNC_STATUS_VALID = 1;
        public static int FUNC_TYPE = 1;

        private int funcID = 0;
        private string funcName = "";
        private string baseInfo = "";

        public override string GetTableName()
        {
            return "Func";
        }

        public override string GetColumnNames()
        {
            return "FuncID,FuncName,BaseInfo";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public int FuncID
        {
            get { return funcID; }
            set { funcID = value; }
        }

        public string FuncName
        {
            get { return funcName; }
            set { funcName = value; }
        }

        public string BaseInfo
        {
            get { return baseInfo; }
            set { baseInfo = value; }
        }

    }
}
