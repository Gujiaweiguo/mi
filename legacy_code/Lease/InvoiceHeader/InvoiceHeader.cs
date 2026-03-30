using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Lease.InvoiceHeader
{
    public class InvoiceHeader:BasePO
    {
        private int invID = 0;
        private int custID = 0;
        private int contractID = 0;
        private int curTypeID = 0;
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;
        private string invCode = "";
        private string custName = "";
        private DateTime invDate = DateTime.Now;
        private DateTime invPeriod = DateTime.Now;
        private int invStatus = 0;
        private int invType = 0;
        private int isFirst = 0;
        private int invCurTypeID = 0;
        private decimal invExRate = 0;
        private decimal invPayAmt = 0;
        private decimal invPayAmtL = 0;
        private decimal invAdjAmt = 0;
        private decimal invAdjAmtL = 0;
        private decimal invDiscAmt = 0;
        private decimal invDiscAmtL = 0;
        private decimal invChngAmt = 0;
        private decimal invChngAmtL = 0;
        private decimal invActPayAmt = 0;
        private decimal invActPayAmtL = 0;
        private decimal invPaidAmt = 0;
        private decimal invPaidAmtL = 0;
        private int printFlag = 0;
        private string note = "";

        public override string GetTableName()
        {
            return "InvoiceHeader";
        }

        public override string GetColumnNames()
        {
            return "";
        }

        public override string GetInsertColumnNames()
        {
            return "";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public int InvID
        {
            get { return invID; }
            set { invID = value; }
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

        public int CurTypeID
        {
            get { return curTypeID; }
            set { curTypeID = value; }
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

        public string InvCode
        {
            get { return invCode; }
            set { invCode = value; }
        }

        public string CustName
        {
            get { return custName; }
            set { custName = value; }
        }

        public DateTime InvDate
        {
            get { return invDate; }
            set { invDate = value; }
        }

        public DateTime InvPeriod
        {
            get { return invPeriod; }
            set { invPeriod = value; }
        }

        public int InvStatus
        {
            get { return invStatus; }
            set { invStatus = value; }
        }

        public int InvType
        {
            get { return invType; }
            set { invType = value; }
        }

        public int IsFirst
        {
            get { return isFirst; }
            set { isFirst = value; }
        }

        public int InvCurTypeID
        {
            get { return invCurTypeID; }
            set { invCurTypeID = value; }
        }

        public decimal InvExRate
        {
            get { return invExRate; }
            set { invExRate = value; }
        }

        public decimal InvPayAmt
        {
            get { return invPayAmt; }
            set { invPayAmt = value; }
        }

        public decimal InvPayAmtL
        {
            get { return invPayAmtL; }
            set { invPayAmtL = value; }
        }

        public decimal InvAdjAmt
        {
            get { return invAdjAmt; }
            set { invAdjAmt = value; }
        }

        public decimal InvAdjAmtL
        {
            get { return invAdjAmtL; }
            set { invAdjAmtL = value; }
        }

        public decimal InvDiscAmt
        {
            get { return invDiscAmt; }
            set { invDiscAmt = value; }
        }

        public decimal InvDiscAmtL
        {
            get { return invDiscAmtL; }
            set { invDiscAmtL = value; }
        }

        public decimal InvChngAmt
        {
            get { return invChngAmt; }
            set { invChngAmt = value; }
        }

        public decimal InvChngAmtL
        {
            get { return invChngAmtL; }
            set { invChngAmtL = value; }
        }

        public decimal InvActPayAmt
        {
            get { return invActPayAmt; }
            set { invActPayAmt = value; }
        }

        public decimal InvActPayAmtL
        {
            get { return invActPayAmtL; }
            set { invActPayAmtL = value; }
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

        public int PrintFlag
        {
            get { return printFlag; }
            set { printFlag = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }
    }
}
