using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace BaseInfo.Store
{
    public class StoreType: BasePO
    {
        private int storeTypeId = 0;
        private string storeTypeCode = "";
        private string storeTypeName = "";

        private int createUserId = 0;
        private DateTime createTime=DateTime.Now;
        private int modifyUserId = 0;
        private DateTime modifyTime=DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        /// <summary>
        /// 表
        /// </summary>
        /// <returns></returns>
        public override string GetTableName()
        {
            return "StoreType";
        }
        /// <summary>
        /// 表中的字段(必须与数据库中的大小写一致)
        /// </summary>
        /// <returns></returns>
        public override string GetColumnNames()
        {
            return "StoreTypeId,StoreTypeCode,StoreTypeName,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        /// <summary>
        /// 更新的字段
        /// </summary>
        /// <returns></returns>
        public override string GetUpdateColumnNames()
        {
            return "StoreTypeCode,StoreTypeName,ModifyUserId,ModifyTime";
        }

        public override String GetInsertColumnNames()
        {
            return "StoreTypeId,StoreTypeCode,StoreTypeName,CreateUserId,CreateTime";
        }
        public int StoreTypeId
        {
            set { storeTypeId = value; }
            get { return storeTypeId; }
        }
        public string StoreTypeCode
        {
            set { storeTypeCode = value; }
            get { return storeTypeCode; }
        }
        public string StoreTypeName
        {
            set { storeTypeName = value; }
            get { return storeTypeName; }
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
