using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    public class InvDisc:BasePO
    {
        private int discID = 0;
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;
        private decimal discAmt = 0;
        private decimal discAmtL = 0;
        private DateTime discDate = DateTime.Now;
        private int discOpr = 0;
        private string discReason = "";
        private int discStatus = 0;

        public static int INVDISC_DRAFT = 1;//草稿
        public static int INVDISC_YES_PUT_IN_NO_UPDATE_LEASE_STATUS = 2;//已提交，待审批
        public static int INVDISC_UPDATE_LEASE_STATUS = 3;//审批通过
        public static int INVDISC_STATUS_OUT = 4;  //作废;

        public override string GetTableName()
        {
            return "InvDisc";
        }

        public override string GetColumnNames()
        {
            return "";
        }

        public override string GetInsertColumnNames()
        {
            return "DiscID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,DiscAmt,DiscAmtL,DiscDate,DiscOpr,DiscReason,DiscStatus";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }


        public int DiscID
        {
            get { return discID; }
            set { discID = value; }
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

        public decimal DiscAmt
        {
            get { return discAmt; }
            set { discAmt = value; }
        }

        public decimal DiscAmtL
        {
            get { return discAmtL; }
            set { discAmtL = value; }
        }

        public DateTime DiscDate
        {
            get { return discDate; }
            set { discDate = value; }
        }

        public int DiscOpr
        {
            get { return discOpr; }
            set { discOpr = value; }
        }

        public string DiscReason
        {
            get { return discReason; }
            set { discReason = value; }
        }

        public int DiscStatus
        {
            get { return discStatus; }
            set { discStatus = value; }
        }
    }
}
