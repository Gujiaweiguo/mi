using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    public class InvoicePay:BasePO
    {
        private int invPayID = 0;
        private int custID = 0;
        private int contractID = 0;
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;
        private int refID = 0;
        private DateTime invPayDate = DateTime.Now;
        private DateTime invPayTime = DateTime.Now;
        private int invPayType = 0;
        private int invPayCurID = 0;
        private decimal invPayExRate = 0;
        private decimal invPaidAmt = 0;
        private decimal invPaidAmtL = 0;
        private decimal invPaidAmtSum = 0;
        private decimal invPaidAmtSumL = 0;
        private decimal invSurAmt = 0;
        private decimal invSurAmtL = 0;
        private int surProcType = 0;
        private int invPayStatus = 0;
        private string note = "";

        public static int INVPAYTYPE_CASH = 1;
        public static int INVPAYTYPE_P_O = 2;
        public static int INVPAYTYPE_CHECK = 3;
        public static int INVPAYTYPE_REPLACE_FUND = 5;
        public static int INVPAYTYPE_OTHER = 6;
        //public static int INVPAYTYPE_DEPOSIT_MORTAGAGE= 4;
        
        //public static int INVPAYTYPE_RESIDUAL_MORTAGAGE = 6;

        public static int INVPAYTYPE_SURPROCTYPE_BACKING_OUT_CUST = 1;//客户返还
        public static int INVPAYTYPE_SURPROCTYPE_CARRY_FORWARD_BALANCE = 2;//结转余额

        public static int SURBAL_UP_TO_SNUFF = 1;     //正常
        public static int SURBAL_CANCEL = 2;          //取消

        public static int[] GetInvPayType()
        {
            int[] getInvPayType = new int[5];
            getInvPayType[0] = INVPAYTYPE_CASH;//现金
            getInvPayType[1] = INVPAYTYPE_P_O; //汇票
            getInvPayType[2] = INVPAYTYPE_CHECK; //支票
            getInvPayType[3] = INVPAYTYPE_REPLACE_FUND;      //代收款诋扣
            getInvPayType[4] = INVPAYTYPE_OTHER;      //代收款诋扣
            //getInvPayType[3] = INVPAYTYPE_DEPOSIT_MORTAGAGE; //押金诋付
            
            //getInvPayType[5] = INVPAYTYPE_RESIDUAL_MORTAGAGE; //余款诋付
            return getInvPayType;
        }

        public static String GetInvPayTypeDesc(int getInvPayType)
        {
            if (getInvPayType == INVPAYTYPE_CASH)
            {
                return "InvoicePay_INVPAYTYPE_CASH";
            }
            if (getInvPayType == INVPAYTYPE_P_O)
            {
                return "InvoicePay_INVPAYTYPE_P_O";
            }
            if (getInvPayType == INVPAYTYPE_CHECK)
            {
                return "InvoicePay_INVPAYTYPE_CHECK";
            }
            //if (getInvPayType == INVPAYTYPE_DEPOSIT_MORTAGAGE)
            //{
            //    return "InvoicePay_INVPAYTYPE_DEPOSIT_MORTAGAGE";
            //}
            if (getInvPayType == INVPAYTYPE_REPLACE_FUND)
            {
                return "InvoicePay_INVPAYTYPE_REPLACE_FUND";
            }
            if (getInvPayType == INVPAYTYPE_OTHER)
            {
                return "INVPAYTYPE_OTHER";
            }
            //if (getInvPayType == INVPAYTYPE_RESIDUAL_MORTAGAGE)
            //{
            //    return "InvoicePay_INVPAYTYPE_RESIDUAL_MORTAGAGE";
            //}

            return "未知";

        }

        public override string GetTableName()
        {
            return "InvoicePay";
        }

        public override string GetColumnNames()
        {
            return "InvPayID,CustID,ContractID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,RefID,InvPayDate,InvPayType," +
                "InvPayCurID,InvPayExRate,InvPaidAmt,InvPaidAmtL,InvPaidAmtSum,InvPaidAmtSumL,InvSurAmt,InvSurAmtL,SurProcType,InvPayStatus,Note";
        }

        public override string GetInsertColumnNames()
        {
            return "InvPayID,CustID,ContractID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,RefID,InvPayDate,InvPayType," +
                "InvPayCurID,InvPayExRate,InvPaidAmt,InvPaidAmtL,InvPaidAmtSum,InvPaidAmtSumL,InvSurAmt,InvSurAmtL,SurProcType,InvPayStatus,Note";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public int InvPayID
        {
            get { return invPayID; }
            set { invPayID = value; }
        }

        public int CustID
        {
            get { return custID; }
            set { custID = value; }
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

        public int RefID
        {
            get { return refID; }
            set { refID = value; }
        }

        public DateTime InvPayDate
        {
            get { return invPayDate; }
            set { invPayDate = value; }
        }

        public DateTime InvPayTime
        {
            get { return invPayTime; }
            set { invPayTime = value; }
        }

        public int InvPayType
        {
            get { return invPayType; }
            set { invPayType = value; }
        }

        public int InvPayCurID
        {
            get { return invPayCurID; }
            set { invPayCurID = value; }
        }

        public decimal InvPayExRate
        {
            get { return invPayExRate; }
            set { invPayExRate = value; }
        }

        public decimal InvPaidAmt
        {
            get { return invPaidAmt; }
            set { invPaidAmt = value; }
        }

        public decimal InvPaidAmtL
        {
            get { return invPaidAmtL; }
            set { invPaidAmtL = value; }
        }

        public decimal InvPaidAmtSum
        {
            get { return invPaidAmtSum; }
            set { invPaidAmtSum = value; }
        }

        public decimal InvPaidAmtSumL
        {
            get { return invPaidAmtSumL; }
            set { invPaidAmtSumL = value; }
        }

        public decimal InvSurAmt
        {
            get { return invSurAmt; }
            set { invSurAmt = value; }
        }

        public decimal InvSurAmtL
        {
            get { return invSurAmtL; }
            set { invSurAmtL = value; }
        }

        public int SurProcType
        {
            get { return surProcType; }
            set { surProcType = value; }
        }

        public int InvPayStatus
        {
            get { return invPayStatus; }
            set { invPayStatus = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }
    }
}
