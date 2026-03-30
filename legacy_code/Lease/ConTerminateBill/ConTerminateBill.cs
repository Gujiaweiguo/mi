using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Lease.ConTerminateBill
{
    public class ConTerminateBill : BasePO
    {
        private int conTerID = 0;
        private int contractID = 0;
        private DateTime terDate = DateTime.Now;
        private string terReason = "";
        private string note = "";
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        private string additionalItem = "";
        private string eConURL = "";
        private int operators = 0;
        private string refID ="";
        private int tradeID = 0;
        private int contractStatus = 0;
        private string contractCode = "";
        private DateTime conStartDate = DateTime.Now;
        private DateTime conEndDate = DateTime.Now;
        private int penalty = 0;
        private int notice = 0;



        public int ConTerID
        {
            get { return conTerID; }
            set { conTerID = value; }
        }

        public int ContractID
        {
            get { return contractID; }
            set { contractID = value; }
        }

        public DateTime TerDate
        {
            get { return terDate; }
            set { terDate = value; }
        }

        public string TerReason
        {
            get { return terReason; }
            set { terReason = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
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


        public string AdditionalItem
        {
            get { return additionalItem; }
            set { additionalItem = value; }
        }

        public string EConURL
        {
            get { return eConURL; }
            set { eConURL = value; }
        }

        public int Operator
        {
            get { return operators; }
            set { operators = value; }
        }

        public string  RefID
        {
            get { return refID; }
            set { refID = value; }
        }

        public int TradeID
        {
            get { return tradeID; }
            set { tradeID = value; }
        }

        public int ContractStatus
        {
            get { return contractStatus; }
            set { contractStatus = value; }
        }

        public string ContractCode
        {
            get { return contractCode; }
            set { contractCode = value; }
        }

        public DateTime ConStartDate
        {
            get { return conStartDate; }
            set { conStartDate = value; }
        }

        public DateTime ConEndDate
        {
            get { return conEndDate; }
            set { conEndDate = value; }
        }

        public int Penalty
        {
            get { return penalty; }
            set { penalty = value; }
        }

        public int Notice
        {
            get { return notice; }
            set { notice = value; }
        }

        public override string GetTableName()
        {
            return "ConTerminateBill";
        }

        public override string GetColumnNames()
        {
            return "ConTerID,ContractID,TerDate,TerReason,AdditionalItem,EConURL,RefID,TradeID,ContractStatus,ContractCode,ConStartDate,ConEndDate,Penalty,Notice,Note";
        }

        public override string GetUpdateColumnNames()
        {
            return "ModifyUserID,ModifyTime,OprRoleID,OprDeptID,TerDate,TerReason,Note";
        }
        public override string GetInsertColumnNames()
        {
            return "ConTerID,ContractID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,TerDate,TerReason,Note";
        }

        public override string GetQuerySql()
        {
            return "Select ConTerID,a.ContractID,TerDate,TerReason,AdditionalItem,EConURL,a.RefID,TradeID,ContractStatus,ContractCode,ConStartDate,ConEndDate,Penalty,Notice,b.Note " +
                    "From Contract a Left Join ConTerminateBill b On a.ContractID=b.ContractID";
        }
    }
}
