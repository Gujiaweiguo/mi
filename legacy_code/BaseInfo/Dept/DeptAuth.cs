using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace BaseInfo.Dept
{
    public class DeptAuth:BasePO
    {
        private int deptAuthID = 0;
        private int deptID = 0;
        //private string deptAuthDesc = null;
        private string concessionAuth = null;
        private string contractAuth = null;
        private string tradeAuth = null;
        private string feeAuth = null;
        private string otherAuth = null;
        private string deptAuthName = null;
        public int DeptAuthID
        {
            get {return deptAuthID; }
            set { deptAuthID = value; }
        }

        public int DeptID
        {
            get { return deptID; }
            set { deptID = value; }
        }

        public string DeptAuthName
        {
            get { return deptAuthName; }
            set { deptAuthName = value; }
        }

        //public string DeptAuthDesc
        //{
        //    get { return deptAuthDesc; }
        //    set { deptAuthDesc = value; }
        //}

        public string ConcessionAuth
        {
            get { return concessionAuth; }
            set { concessionAuth = value; }
        }

        public string ContractAuth
        {
            get { return contractAuth; }
            set { contractAuth = value; }
        }

        public string TradeAuth
        {
            get { return tradeAuth; }
            set { tradeAuth = value; }
        }

        public string FeeAuth
        {
            get { return feeAuth; }
            set { feeAuth = value; }
        }

        public string OtherAuth
        {
            get { return otherAuth; }
            set { otherAuth = value; }
        }

        public override string GetTableName()
        {
            return "Deptauth";
        }

        public override string GetColumnNames()
        {
            return "DeptAuthID,DeptID,DeptAuthName,ConcessionAuth,ContractAuth,TradeAuth,OtherAuth";
        }

        public override string GetUpdateColumnNames()
        {
            return "ConcessionAuth,ContractAuth,TradeAuth,OtherAuth";
        }
    }
}
