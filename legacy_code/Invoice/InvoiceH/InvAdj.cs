using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    public class InvAdj:BasePO
    {
        private int invAdjID = 0;
        private int invID = 0;
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;
        private decimal adjAmt = 0;
        private decimal adjAmtL = 0;
        private DateTime adjDate = DateTime.Now;
        private int adjOpr = 0;
        private string adjReason = "";
        private int adjStatus = 0;
        private string note = "";
        private decimal invPayAmt = 0;
        private string invCode = "";
        private string chargeTypeName = "";
        private string contractCode = "";
        private DateTime invPeriod = DateTime.Now;
        private string custName = "";

        public static int INVADJ_DRAFT = 1;//草稿
        public static int INVADJ_YES_PUT_IN_NO_UPDATE_LEASE_STATUS = 2;//已提交，待审批
        public static int INVADJ_UPDATE_LEASE_STATUS = 3;//审批通过
        public static int INVADJ_BLANK_OUT = 4;//作废

        public override string GetTableName()
        {
            return "InvAdj";
        }

        public override string GetColumnNames()
        {
            return "AdjDate,AdjAmt,AdjOpr,AdjReason,InvPayAmt,InvCode,ChargeTypeName,ContractCode,InvPeriod,CustName";
        }

        public override string GetInsertColumnNames()
        {
            return "InvAdjID,InvID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,AdjAmt,AdjAmtL,AdjDate,AdjOpr,AdjReason,AdjStatus";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public override string GetQuerySql()
        {
            return "select AdjDate,AdjAmt,AdjOpr,AdjReason,b.InvPayAmt,a.InvCode,ChargeTypeName,ContractCode,InvPeriod,CustName from InvoiceHeader a left join InvoiceDetail b on a.InvID=b.InvID " +
                    "right join InvAdj c on b.InvDetailID=c.InvDetailID left join ChargeType d on b.ChargeTypeID=d.ChargeTypeID left join Contract e on a.ContractID = e.ContractID";
        }

        public int InvAdjID
        {
            get { return invAdjID; }
            set { invAdjID = value; }
        }

        public int InvID
        {
            get { return invID; }
            set { invID = value; }
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

        public decimal AdjAmt
        {
            get { return adjAmt; }
            set { adjAmt = value; }
        }

        public decimal AdjAmtL
        {
            get { return adjAmtL; }
            set { adjAmtL = value; }
        }

        public DateTime AdjDate
        {
            get { return adjDate; }
            set { adjDate = value; }
        }

        public int AdjOpr
        {
            get { return adjOpr; }
            set { adjOpr = value; }
        }

        public string AdjReason
        {
            get { return adjReason; }
            set { adjReason = value; }
        }

        public int AdjStatus
        {
            get { return adjStatus; }
            set { adjStatus = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public decimal InvPayAmt
        {
            get { return invPayAmt; }
            set { invPayAmt = value; }
        }

        public string InvCode
        {
            get { return invCode; }
            set { invCode = value; }
        }

        public string ChargeTypeName
        {
            get { return chargeTypeName; }
            set { chargeTypeName = value; }
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

        public string CustName
        {
            get { return custName; }
            set { custName = value; }
        }
    }
}
