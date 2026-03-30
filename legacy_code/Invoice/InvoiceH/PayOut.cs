using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    public class PayOut:BasePO
    {
        private int payOutID = 0;
        private int payInID = 0;
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;
        private decimal payOutAmt = 0;
        private DateTime payOutDate = DateTime.Now;
        private int invPayID = 0;
        private int payOutType = 0;
        private int payOutStatus = 0;

        public static int PAYOUT_UP_TO_SNUFF = 1;     //正常
        public static int PAYOUT_CANCEL = 2;          //取消

        public static int PAYOUT_BACKING_OUT_SHOP = 1;//返还商铺
        public static int PAYOUT_MORTAGAGE = 2;       //诋付

        public override string GetTableName()
        {
            return "PayOut";
        }

        public override string GetColumnNames()
        {
            return "PayOutID,PayInID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,PayOutAmt,PayOutDate,InvPayID,PayOutType,PayOutStatus";
        }

        public override string GetInsertColumnNames()
        {
            return "PayOutID,PayInID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,PayOutAmt,PayOutDate,InvPayID,PayOutType,PayOutStatus";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public int PayOutID
        {
            get { return payOutID; }
            set { payOutID = value; }
        }

        public int PayInID
        {
            get { return payInID; }
            set { payInID = value; }
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

        public decimal PayOutAmt
        {
            get { return payOutAmt; }
            set { payOutAmt = value; }
        }

        public DateTime PayOutDate
        {
            get { return payOutDate; }
            set { payOutDate = value; }
        }

        public int InvPayID
        {
            get { return invPayID; }
            set { invPayID = value; }
        }

        public int PayOutType
        {
            get { return payOutType; }
            set { payOutType = value; }
        }

        public int PayOutStatus
        {
            get { return payOutStatus; }
            set { payOutStatus = value; }
        }

    }
}
