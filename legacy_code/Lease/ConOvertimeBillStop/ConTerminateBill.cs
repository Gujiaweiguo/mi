using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Lease.ConOvertimeBillStop
{
    public class ConTerminateBill:BasePO
    {
        private int conTermD = 0;
        private int contractID = 0;
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;
        private DateTime termDate = DateTime.Now;
        private int operators = 0;
        private int refID = 0;
        private string note = "";

        public override string GetTableName()
        {
            return "ConTerminateBill";
        }

        public override string GetColumnNames()
        {
            return "ContractID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,TermDate,Operator,RefID,Note";
        }

        public override string GetInsertColumnNames()
        {
            return "ConTermD,ContractID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,TermDate,Operator,RefID,Note";
        }

        public override string GetUpdateColumnNames()
        {
            return "TermDate";
        }

        public int ConTermD
        {
            get { return conTermD; }
            set { conTermD = value; }
        }

        public int ContractID
        {
            get { return contractID; }
            set { contractID = value; }
        }

        public int CreateUserID
        {
            get { return createUserID; }
            set { createUserID = value; }
        }

        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        public int ModifyUserID
        {
            get { return modifyUserID; }
            set { modifyUserID = value; }
        }

        public DateTime ModifyTime
        {
            get { return modifyTime; }
            set { modifyTime = value; }
        }

        public int OprRoleID
        {
            get { return oprRoleID; }
            set { oprRoleID = value; }
        }

        public int OprDeptID
        {
            get { return oprDeptID; }
            set { oprDeptID = value; }
        }

        public DateTime TermDate
        {
            get { return termDate; }
            set { termDate = value; }
        }

        public int Operator
        {
            get { return operators; }
            set { operators = value; }
        }

        public int RefID
        {
            get { return refID; }
            set { refID = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }
    }
}
