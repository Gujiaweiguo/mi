using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    public class InvCancel:BasePO
    {
        private int invCelID = 0;
        private int invID = 0;
        private string celReason = "";
        private int invCelStatus = 0;
        private string note = "";
        private string custCode = "";
        private string custName = "";
        private string contractCode = "";
        private DateTime invPeriod = DateTime.Now;

        public static int INVCANCEL_DRAFT = 1;//草稿
        public static int INVCANCEL_YES_PUT_IN_NO_UPDATE_LEASE_STATUS = 2;//已提交，待审批
        public static int INVCANCEL_UPDATE_LEASE_STATUS = 3;//审批通过

        public override string GetTableName()
        {
            return "InvCancel";
        }

        public override string GetColumnNames()
        {
            return "InvCelID,InvID,CelReason,InvCelStatus,Note,CustCode,CustName,ContractCode,InvPeriod";
        }

        public override string GetInsertColumnNames()
        {
            return "InvCelID,InvID,CelReason,InvCelStatus,Note";
        }

        public override string GetUpdateColumnNames()
        {
            return "CelReason,Note";
        }

        public override string GetQuerySql()
        {
            return "select A.InvCelID,A.InvID,A.CelReason,A.InvCelStatus,A.Note,D.CustCode,D.CustName,C.ContractCode,B.InvPeriod from InvCancel A left join InvoiceHeader B on A.InvID=B.InvID " +
                    " left join Contract C on B.ContractID=C.ContractID left join Customer D on B.CustID=D.CustID ";
        }

        public int InvCelID
        {
            get { return invCelID; }
            set { invCelID = value; }
        }

        public int InvID
        {
            get { return invID; }
            set { invID = value; }
        }

        public string CelReason
        {
            get { return celReason; }
            set { celReason = value; }
        }

        public int InvCelStatus
        {
            get { return invCelStatus; }
            set { invCelStatus = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public string CustCode
        {
            get { return custCode; }
            set { custCode = value; }
        }

        public string CustName
        {
            get { return custName; }
            set { custName = value; }
        }

        public string ContractCode
        {
            get { return contractCode; }
            set { contractCode = value; }
        }

        public DateTime InvPeriod
        {
            get { return invPeriod; }
            set { invPeriod = value; }
        }

        
    }
}
