using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice
{
    public class BuilderAuditing:BasePO
    {
        #region Model
        private int _custid;
        private int _contractid;
        private string _chargetypename;
        private string _custname;
        private string _contractcode;
        private string _shopCode;
        private string _shopName;
        private string _custCode;
        private DateTime _invperiod;
        private int _isfirst;
        private decimal _invactpayamt;
        private decimal _invactpayamtl;
        private decimal _invpaidamt;
        private decimal _invpaidamtl;
        private decimal _balance;
        private decimal _balancel;
        private int _status;
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;
        private int _invID = 0;

        public override string GetTableName()
        {
            return "BuilderAuditing";
        }

        public override string GetColumnNames()
        {
            return "CustID,ContractID,ChargeTypeName,CustName,ContractCode,ShopCode,ShopName,CustCode,InvPeriod,IsFirst,InvActPayAmt,InvActPayAmtL,InvPaidAmt,InvPaidAmtL,Balance,BalanceL,Status,CreateUserID,CreateTime,InvID";
        }

        public override string GetInsertColumnNames()
        {
            return "CustID,ContractID,ChargeTypeName,CustName,ContractCode,ShopCode,ShopName,CustCode,InvPeriod,IsFirst,InvActPayAmt,InvActPayAmtL,InvPaidAmt,InvPaidAmtL,Balance,BalanceL,Status,CreateUserID,CreateTime,OprRoleID,OprDeptID,InvID";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }


        /// <summary>
        /// 
        /// </summary>
        public int CustID
        {
            set { _custid = value; }
            get { return _custid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ContractID
        {
            set { _contractid = value; }
            get { return _contractid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ChargeTypeName
        {
            set { _chargetypename = value; }
            get { return _chargetypename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CustName
        {
            set { _custname = value; }
            get { return _custname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ContractCode
        {
            set { _contractcode = value; }
            get { return _contractcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ShopCode
        {
            set { _shopCode = value; }
            get { return _shopCode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ShopName
        {
            set { _shopName = value; }
            get { return _shopName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CustCode
        {
            set { _custCode = value; }
            get { return _custCode; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime InvPeriod
        {
            set { _invperiod = value; }
            get { return _invperiod; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsFirst
        {
            set { _isfirst = value; }
            get { return _isfirst; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal InvActPayAmt
        {
            set { _invactpayamt = value; }
            get { return _invactpayamt; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal InvActPayAmtL
        {
            set { _invactpayamtl = value; }
            get { return _invactpayamtl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal InvPaidAmt
        {
            set { _invpaidamt = value; }
            get { return _invpaidamt; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal InvPaidAmtL
        {
            set { _invpaidamtl = value; }
            get { return _invpaidamtl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal Balance
        {
            set { _balance = value; }
            get { return _balance; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal BalanceL
        {
            set { _balancel = value; }
            get { return _balancel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
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
        public int InvID
        {
            get { return _invID; }
            set { _invID = value; }
        }
        #endregion Model
    }
}
