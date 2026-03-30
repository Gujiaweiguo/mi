using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace RentableArea
{
    public class UnitTypes : BasePO
    {
        public static int UNITYEPE_STATUS_INVALID = 0;
        public static int UNITYEPE_STATUS_VALID = 1;

        public static int[] GetUnitTypeStatus()
        {
            int[] unitTypeStaus = new int[2];
            unitTypeStaus[0] = UNITYEPE_STATUS_VALID;
            unitTypeStaus[1] = UNITYEPE_STATUS_INVALID;
            return unitTypeStaus;
        }

        public static String GetUnitTypeStatusDesc(int unitTypeStaus)
        {
            if (unitTypeStaus == UNITYEPE_STATUS_INVALID)
            {
                return "无效";
            }
            if (unitTypeStaus == UNITYEPE_STATUS_VALID)
            {
                return "有效";
            }
            return "未知";
        }

        public String UnitTypeStatusDesc
        {
            get { return GetUnitTypeStatusDesc(UnitTypeStatus); }
        }

        private int unitTypeID=0;
        private string unitTypeCode = null;
        private string unitTypeName = null;
        private int unitTypeStatus=1;
        private string note = null;

        private int createUserId = 0;
        private DateTime createTime=DateTime.Now;
        private int modifyUserId = 0;
        private DateTime modifyTime=DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        public override String GetTableName()
        {
            return "UnitType";
        }

        public override String GetColumnNames()
        {
            return "UnitTypeID,UnitTypeCode,UnitTypeName,UnitTypeStatus,Note,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }

        public override String GetUpdateColumnNames()
        {
            return "UnitTypeCode,UnitTypeName,UnitTypeStatus,Note,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override String GetInsertColumnNames()
        {
            return "UnitTypeCode,UnitTypeName,UnitTypeStatus,Note,CreateUserId,CreateTime,OprRoleID,OprDeptID";
        }
        public int UnitTypeID
        {
            get { return this.unitTypeID; }
            set { this.unitTypeID = value; }
        }
        public string UnitTypeCode
        {
            get { return this.unitTypeCode; }
            set { this.unitTypeCode = value; }
        }
        public string UnitTypeName
        {
            get { return this.unitTypeName; }
            set { this.unitTypeName = value; }
        }
        public int UnitTypeStatus
        {
            get { return this.unitTypeStatus; }
            set { this.unitTypeStatus = value; }
        }
        public string Note
        {
            get { return this.note; }
            set { this.note = value; }
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
