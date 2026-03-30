using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
using Base.Util;
using Base.Biz;
using BaseInfo;

namespace BaseInfo.Dept
{
    public class DeptTypePO : BasePO
    {
        private int deptType = 0;
        private string deptTypeName = "";


        public override string GetTableName()
        {
            return "DeptType";
        }


        public override string GetColumnNames()
        {
            return "DeptType,DeptTypeName";
        }


        public override string GetUpdateColumnNames()
        {
            return "DeptTypeName";
        }


        public override String GetInsertColumnNames()
        {
            return "DeptType,DeptTypeName";
        }


        public int DeptType
        {
            set { deptType = value; }
            get { return deptType; }
        }


        public string DeptTypeName
        {
            set { deptTypeName = value; }
            get { return deptTypeName; }
        }
    }
}
