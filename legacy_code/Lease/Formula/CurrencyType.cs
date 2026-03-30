using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Lease.Formula
{
    /// <summary>
    /// 币种
    /// </summary>
    public class CurrencyType:BasePO
    {
        private int curTypeID = 0;
        private string curTypeName = "";
        private int curTypeStatus = 0;
        private int isLocal = 0;
        private string note = "";
        private int createUserId = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserId = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        //币种状态
        public static int CURTYPESTATUS_NO = 0;  //无效
        public static int CURTYPESTATUS_YES = 1; //有效

        public static int[] GetCurTypeStatus()
        {
            int[] curTypeStatus = new int[2];
            curTypeStatus[0] = CURTYPESTATUS_YES;
            curTypeStatus[1] = CURTYPESTATUS_NO;            

            return curTypeStatus;
        }

        public static String GetCurTypeStatusDesc(int curTypeStatus)
        {
            if (curTypeStatus == CURTYPESTATUS_YES)
            {
                return "CUST_TYPE_STATUS_VALID";
            }
            if (curTypeStatus == CURTYPESTATUS_NO)
            {
                return "CUST_TYPE_STATUS_INVALID";
            }
            return "Public_Sealed";
        }

        //是否本币
        public static int ISLOCAL_NO = 0;  //否
        public static int ISLOCAL_YES = 1; //是

        public static int[] GetIsLocal()
        {
            int[] isLocal = new int[2];
            isLocal[0] = ISLOCAL_NO;
            isLocal[1] = ISLOCAL_YES;

            return isLocal;
        }

        public static String GetIsLocalDesc(int isLocal)
        {
            if (isLocal == ISLOCAL_YES)
            {
                return "IFPREPAY_TYPE_YES";
            }
            if (isLocal == ISLOCAL_NO)
            {
                return "IFPREPAY_TYPE_NO";
            }
            return "Public_Sealed";
        }

        public int CurTypeID
        {
            get { return curTypeID; }
            set { curTypeID = value; }
        }

        public string CurTypeName
        {
            get { return curTypeName; }
            set { curTypeName = value; }
        }

        public int CurTypeStatus
        {
            get { return curTypeStatus; }
            set { curTypeStatus = value; }
        }

        public int IsLocal
        {
            get { return isLocal; }
            set { isLocal = value; }
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

        public override string GetTableName()
        {
            return "CurrencyType";
        }

        public override string GetColumnNames()
        {
            return "CurTypeID,CurTypeName,CurTypeStatus,IsLocal,Note,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetUpdateColumnNames()
        {
            return "CurTypeName,CurTypeStatus,IsLocal,Note,ModifyUserId,ModifyTime";
        }
        public override string GetInsertColumnNames()
        {
            return "CurTypeID,CurTypeName,CurTypeStatus,IsLocal,Note,CreateUserId,CreateTime";
        }
    }
}
