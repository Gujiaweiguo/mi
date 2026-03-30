using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Lease.ConTerminateBill
{
    public class UpdateContractStopDate:BasePO
    {
        private int conTerID = 0;
        private int contractID = 0;
        private DateTime terDate = DateTime.Now;
        private DateTime conEndDate = DateTime.Now;
        private int contractStatus = 0;
        private DateTime stopDate = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        public int ContractID
        {
            get { return contractID; }
            set { contractID = value; }
        }

        public int ConTerID
        {
            get { return conTerID; }
            set { conTerID = value; }
        }

        public DateTime TerDate
        {
            get { return terDate; }
            set { terDate = value; }
        }

        public DateTime ConEndDate
        {
            get { return conEndDate; }
            set { conEndDate = value; }
        }

        public int ContractStatus
        {
            get { return contractStatus; }
            set { contractStatus = value; }
        }

        public DateTime StopDate
        {
            get { return stopDate; }
            set { stopDate = value; }
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

        public override string GetTableName()
        {
            return "Contract";
        }

        public override string GetColumnNames()
        {
            return "ConTerID,ContractID,TerDate,ConEndDate";
        }

        public override string GetUpdateColumnNames()
        {
            return "StopDate,ContractStatus";
        }
        public override string GetInsertColumnNames()
        {
            return "";
        }

        public override string GetQuerySql()
        {
            return "Select ConTerID,a.ContractID,TerDate,ConEndDate " +
                    "From ConTerminateBill a Left Join Contract b On a.ContractID=b.ContractID";
        }
    }
}
