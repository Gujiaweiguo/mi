using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Lease.ConOvertimeBill
{
    public class ConOvertimeBill:BasePO
    {
        private int conOverTimeID = 0;
        private int contractID = 0;
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;
        private DateTime prvEndDate = DateTime.Now;
        private DateTime newConStartDate = DateTime.Now;
        private DateTime newConEndDate = DateTime.Now;
        private string additionalItem = "";
        private string eConURL = "";
        private int operators = 0;
        private string  refID = "";
        private string note = "";

        private string custName = "";
        private string custShortName = "";
        private int tradeID = 0;
        private int contractStatus = 0;
        private string contractCode = "";
        private DateTime conStartDate = DateTime.Now;
        private DateTime conEndDate = DateTime.Now;
        private int penalty = 0;
        private int notice = 0;
        private string  bRefID = "";

        public override string GetTableName()
        {
            return "ConOvertimeBill";
        }

        public override string GetColumnNames()
        {
            return "ConOverTimeID,ContractID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,PrvEndDate,NewConStartDate,NewConEndDate,AdditionalItem,EConURL," +
                    "Operator,RefID,Note,CustName,CustShortName,TradeID,ContractStatus,ContractCode,BRefID,ConStartDate,ConEndDate,Penalty,Notice";
        }

        public override string GetInsertColumnNames()
        {
            return "ConOverTimeID,ContractID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,PrvEndDate,NewConStartDate,NewConEndDate,AdditionalItem,EConURL," +
                    "Operator,RefID,Note";
        }

        public override string GetQuerySql()
        {
            return "select ConOverTimeID,a.ContractID,a.CreateUserID,a.CreateTime,a.ModifyUserID,a.ModifyTime,a.OprRoleID,a.OprDeptID,a.PrvEndDate,a.NewConStartDate,NewConEndDate,a.AdditionalItem,a.EConURL,"+
                   " Operator,a.RefID,a.Note,CustName,CustShortName,TradeID,ContractStatus,ContractCode,"+
                    "b.RefID as BRefID,ConStartDate,ConEndDate,Penalty,Notice from ConOvertimeBill a,Contract b,Customer c";
        }

        public override string GetUpdateColumnNames()
        {
            return "CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,PrvEndDate,NewConStartDate,NewConEndDate,AdditionalItem,EConURL," +
                    "Operator,RefID,Note";
        }

        public int ConOverTimeID
        {
            get { return conOverTimeID; }
            set { conOverTimeID = value; }
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

        public DateTime PrvEndDate
        {
            get { return prvEndDate; }
            set { prvEndDate = value; }
        }

        public DateTime NewConStartDate
        {
            get { return newConStartDate; }
            set { newConStartDate = value; }
        }

        public DateTime NewConEndDate
        {
            get { return newConEndDate; }
            set { newConEndDate = value; }
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

        public string RefID
        {
            get { return refID; }
            set { refID = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public string CustName
        {
            get { return custName; }
            set { custName = value; }
        }

        public string CustShortName
        {
            get { return custShortName; }
            set { custShortName = value; }
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

        public string  BRefID
        {
            get { return bRefID; }
            set { bRefID = value; }
        }
    }
}
