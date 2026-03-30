using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.PotCust
{
    public class CustType:BasePO
    {
        private int custTypeID = 0;
        private string custTypeCode = "";
        private string custTypeName = "";
        private int custTypeStatus = 0;
        private string note = "";
        private int createUserId = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserId = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        public static int CUST_TYPE_STATUS_INVALID = 0;
        public static int CUST_TYPE_STATUS_VALID = 1;

        public static int[] GetCustTypeStatus()
        {
            int[] custTypeStatus = new int[2];
            custTypeStatus[0] = CUST_TYPE_STATUS_VALID;
            custTypeStatus[1] = CUST_TYPE_STATUS_INVALID;
            return custTypeStatus;
        }

        public static String GetCustTypeStatusDesc(int custTypeStatus)
        {
            if (custTypeStatus == CUST_TYPE_STATUS_INVALID)
            {
                return "WrkFlw_Disabled";// "无效";
            }
            if (custTypeStatus == CUST_TYPE_STATUS_VALID)
            {
                return "WrkFlw_Enabled";//"有效";
            }
            return "NO";
        }

        public String CustTypeStatusDesc
        {
            get { return GetCustTypeStatusDesc(custTypeStatus); }
        }

        public override String GetTableName()
        {
            return "CustType";
        }

        public override String GetColumnNames()
        {
            return "CustTypeID,CustTypeCode,CustTypeName,CustTypeStatus,Note";
        }

        public override string GetQuerySql()
        {
            return "select CustTypeID,CustTypeCode,CustTypeName,CustTypeStatus,Note,CustTypeStatus from CustType";
        }

        public override String GetInsertColumnNames()
        {
            return "CustTypeID,CustTypeCode,CustTypeName,CustTypeStatus,Note,CreateUserId,CreateTime";
        }

        public override String GetUpdateColumnNames()
        {
            return "CustTypeCode,CustTypeName,CustTypeStatus,Note,ModifyUserId,ModifyTime";
        }

        public int CustTypeID
        {
            get { return custTypeID; }
            set { custTypeID = value; }
        }

        public string CustTypeCode
        {
            get { return custTypeCode; }
            set { custTypeCode = value; }
        }

        public string CustTypeName
        {
            get { return custTypeName; }
            set { custTypeName = value; }
        }

        public int CustTypeStatus
        {
            get { return custTypeStatus; }
            set { custTypeStatus = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
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

