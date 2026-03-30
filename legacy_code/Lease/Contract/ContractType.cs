using System;
using System.Collections.Generic;
using System.Text;
using Base.Biz;
using Base.DB;
using Base.Util;

namespace Lease.Contract
{
    public class ContractType:BasePO
    {
        private int contractTypeID = 0;
        private string contractTypeCode = "";
        private string contractTypeName = "";
        private int contractTypeStatus = 0;
        private string note = "";

        private int createUserId = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserId = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        public override string GetTableName()
        {
            return "ContractType";
        }
        public override string GetColumnNames()
        {
            return "ContractTypeID,ContractTypeCode,ContractTypeName,ContractTypeStatus,Note,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override string GetUpdateColumnNames()
        {
            return "ContractTypeCode,ContractTypeName,ContractTypeStatus,Note,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override string GetInsertColumnNames()
        {
            return "ContractTypeID,ContractTypeCode,ContractTypeName,ContractTypeStatus,Note,CreateUserId,CreateTime,OprRoleID,OprDeptID";
        }
        public int ContractTypeID
        {
            set { contractTypeID = value; }
            get { return contractTypeID; }
        }
        public string ContractTypeCode
        {
            set { contractTypeCode = value; }
            get { return contractTypeCode; }
        }
        public string ContractTypeName
        {
            set { contractTypeName = value; }
            get { return contractTypeName; }
        }
        public int ContractTypeStatus
        {
            set { contractTypeStatus = value; }
            get { return contractTypeStatus; }
        }
        public string Note
        {
            set { note = value; }
            get { return note; }
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
    }
}
