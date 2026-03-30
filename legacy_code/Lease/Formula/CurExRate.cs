using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Lease.Formula
{
    /// <summary>
    /// 汇率
    /// </summary>
    public class CurExRate:BasePO
    {
        private int exRateID;  //汇率ID
        private int curTypeID; //币种代码
        private int toCurTypeID;  //目标币种ID
        private decimal exRate;  //汇率
        private int status;  //汇率状态
        private string note; //备注
        private DateTime exRateDate = DateTime.Now;  //汇率日期
        private int createUserId = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserId = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        //币种状态
        public static int EXRATESTATUS_NO = 0;  //无效
        public static int EXRATESTATUS_YES = 1; //有效

        public static int[] GetExRateStatus()
        {
            int[] exRateStatus = new int[2];
            exRateStatus[0] = EXRATESTATUS_YES;
            exRateStatus[1] = EXRATESTATUS_NO;
            
            return exRateStatus;
        }

        public static String GetExRateStatusDesc(int exRateStatus)
        {
            if (exRateStatus == EXRATESTATUS_YES)
            {
                return "CUST_TYPE_STATUS_VALID";
            }
            if (exRateStatus == EXRATESTATUS_NO)
            {
                return "CUST_TYPE_STATUS_INVALID";
            }
            return "Public_Sealed";
        }

        public int ExRateID
        {
            get { return exRateID; }
            set { exRateID = value; }
        }

        public int CurTypeID
        {
            get { return curTypeID; }
            set { curTypeID = value; }
        }

        public int ToCurTypeID
        {
            get { return toCurTypeID; }
            set { toCurTypeID = value; }
        }

        public decimal ExRate
        {
            get { return exRate; }
            set { exRate = value; }
        }

        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public DateTime ExRateDate
        {
            get { return exRateDate; }
            set { exRateDate = value; }
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
            return "CurExRate";
        }

        public override string GetColumnNames()
        {
            return "ExRateID,CurTypeID,ToCurTypeID,ExRate,Status,Note,ExRateDate,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetInsertColumnNames()
        {
            return "ExRateID,CurTypeID,ToCurTypeID,ExRate,Status,Note,ExRateDate,CreateUserId,CreateTime";
        }

        public override string GetUpdateColumnNames()
        {
            return "CurTypeID,ToCurTypeID,ExRate,Status,Note,ExRateDate,ModifyUserId,ModifyTime";
        }
    }
}
