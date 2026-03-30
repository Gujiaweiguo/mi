using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Lease.ConShop
{
    public class HdwType:BasePO
    {
        private int hdwTypeID; //硬件类型ID
        private string hdwTypeName;  //硬件类型名称
        private int hdwClass;  //硬件类别
        private int hdwTypeStatus;  //硬件类型状态
        private string note;  //备注
        private int createUserId = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserId = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        /// <summary>
        /// 硬件类型ID
        /// </summary>
        public int HdwTypeID
        {
            get { return hdwTypeID; }
            set { hdwTypeID = value; }
        }

        /// <summary>
        /// 硬件类型名称
        /// </summary>
        public string HdwTypeName
        {
            get { return hdwTypeName; }
            set { hdwTypeName = value; }
        }

        /// <summary>
        /// 硬件类别
        /// </summary>
        public int HdwClass
        {
            get { return hdwClass; }
            set { hdwClass = value; }
        }

        /// <summary>
        /// 硬件类型状态
        /// </summary>
        public int HdwTypeStatus
        {
            get { return hdwTypeStatus; }
            set { hdwTypeStatus = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
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

        //硬件类型状态
        public static int HDWTYPESTATUS_EFFECTIVE = 1;  //有效
        public static int HDWTYPESTATUS_VAIN = 0;       //无效

        public static int[] GetHdwTypeStatus()
        {
            int[] hdwTypeStatus = new int[2];
            hdwTypeStatus[0] = HDWTYPESTATUS_EFFECTIVE;
            hdwTypeStatus[1] = HDWTYPESTATUS_VAIN;
            return hdwTypeStatus;
        }

        public static string GetHdwTypeStatusDesc(int hdwTypeStatus)
        {
            if (hdwTypeStatus == HDWTYPESTATUS_EFFECTIVE)
            {
                return "BizGrp_YES"; // "有效";
            }
            if (hdwTypeStatus == HDWTYPESTATUS_VAIN)
            {
                return "BizGrp_NO";// "无效";
            }
            return "NO";
        }

        //硬件类别
        public static int HDWCLASS_METER = 1;  //读表类
        public static int HDWCLASS_MONEY = 2;  //收款类
        public static int HDWCLASS_OTHER = 3;  //其它

        public static int[] GetHdwClass()
        {
            int[] hdwClass = new int[3];
            hdwClass[0] = HDWCLASS_METER;
            hdwClass[1] = HDWCLASS_MONEY;
            hdwClass[2] = HDWCLASS_OTHER;
            return hdwClass;
        }

        public static string GetHdwClassDesc(int hdwClass)
        {
            if (hdwClass == HDWCLASS_METER)
            {
                return "HdwClass_Meter"; // "读表类";
            }
            if (hdwClass == HDWCLASS_MONEY)
            {
                return "HdwClass_Money";// "收款类";
            }
            if (hdwClass == HDWCLASS_OTHER)
            {
                return "CUST_OTHER";// "其它";
            }
            return "NO";
        }



        public override string GetTableName()
        {
            return "HdwType";
        }

        public override string GetColumnNames()
        {
            return "HdwTypeID,HdwTypeName,HdwClass,HdwTypeStatus,Note,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetInsertColumnNames()
        {
            return "HdwTypeID,HdwTypeName,HdwClass,HdwTypeStatus,Note,CreateUserId,CreateTime";
        }

        public override string GetUpdateColumnNames()
        {
            return "HdwTypeName,HdwClass,HdwTypeStatus,Note,ModifyUserId,ModifyTime";
        }

    }
}
