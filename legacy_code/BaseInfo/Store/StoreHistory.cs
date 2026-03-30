using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace BaseInfo.Store
{
    public class StoreHistory:BasePO
    {
        private int historyId = 0;//主键
        private int storeId = 0;//关联
        private DateTime historyDate;//发生日期
        private string historyDesc = "";//描述

        private int createUserId = 0;
        private DateTime createTime;
        private int modifyUserId = 0;
        private DateTime modifyTime;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        /// <summary>
        /// 表
        /// </summary>
        /// <returns></returns>
        public override string GetTableName()
        {
            return "StoreHistory";
        }
        /// <summary>
        /// 表中的字段(必须与数据库中的大小写一致)
        /// </summary>
        /// <returns></returns>
        public override string GetColumnNames()
        {
            return "HistoryId,StoreId,HistoryDate,HistoryDesc,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        /// <summary>
        /// 更新的字段
        /// </summary>
        /// <returns></returns>
        public override string GetUpdateColumnNames()
        {
            return "HistoryDate,HistoryDesc,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }


        public int HistoryId
        {
            set { this.historyId = value; }
            get { return historyId; }
        }
        public int StoreId
        {
            set { this.storeId = value; }
            get { return storeId; }
        }
        public DateTime HistoryDate
        {
            set { this.historyDate = value; }
            get { return historyDate; }
        }
        public string HistoryDesc
        {
            set { this.historyDesc = value; }
            get { return historyDesc; }
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
