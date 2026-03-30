using System;
using System.Collections.Generic;
using System.Text;
using Base.Biz;
using Base.DB;

namespace Lease.Subs
{
    public class Subs:BasePO
    {
        private int subsID = 0;
        private string subsCode = "";
        private string subsName = "";
        private string subsShortName = "";
        private string bankName = "";
        private string bankAcct = "";
        private int subsStatus = 0;
        private int financialTypeID = 0;
        private int createUserId = 0;
        private DateTime createTime=DateTime.Now;
        private int modifyUserId = 0;
        private DateTime modifyTime=DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        public override string GetColumnNames()
        {
            return "SubsID,SubsCode,SubsName,SubsShortName,BankName,BankAcct,SubsStatus,FinancialTypeID,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override string GetTableName()
        {
            return "Subsidiary";
        }
        public override string GetUpdateColumnNames()
        {
            return "SubsCode,SubsName,SubsShortName,BankName,BankAcct,SubsStatus,FinancialTypeID,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }

        public int SubsID
        {
            set { subsID = value; }
            get { return subsID; }
        }
        public string SubsCode
        {
            set { subsCode = value; }
            get { return subsCode; }
        }
        public string SubsName
        {
            set { subsName = value; }
            get { return subsName; }
        }
        public string SubsShortName
        {
            set { subsShortName = value; }
            get { return subsShortName; }
        }
        public int  SubsStatus
        {
            set { subsStatus = value; }
            get { return subsStatus; }
        }
        public int CreateUserId
        {
            set { createUserId = value; }
            get { return createUserId; }
        }
        public DateTime CreateTime
        {
            set { createTime = value; }
            get { return createTime; }
        }
        public int ModifyUserId
        {
            set { modifyUserId = value; }
            get { return modifyUserId; }
        }
        public DateTime ModifyTime
        {
            set { modifyTime = value; }
            get { return modifyTime; }
        }
        public int OprRoleID
        {
            set { oprRoleID = value; }
            get { return oprRoleID; }
        }
        public int OprDeptID
        {
            set { oprDeptID = value; }
            get { return oprDeptID; }
        }
        public int FinancialTypeID
        {
            set { financialTypeID = value; }
            get { return financialTypeID; }
        }
        public string BankName
        {
            set { bankName = value; }
            get { return bankName; }
        }
        public string BankAcct
        {
            set { bankAcct = value; }
            get { return bankAcct; }
        }
    }
}
