using System;
using System.Collections.Generic;
using System.Text;
using Base.Biz;
using Base.DB;

namespace Lease.PotCust
{
    /// <summary>
    /// 招商进程  add by lcp at 2009-3-19
    /// </summary>
    public class ProcessType : BasePO
    {
        private int processTypeId = 0;//主键
        private string processTypeCode = "";//进名编码
        private string processTypeName = "";//进名称程
        private int processTypeStatus = 0;//进程状态
        private string note = "";//备注
        private int createUserId = 0;//创建用户内码
        private DateTime createTime = DateTime.Now;//创建时间
        private int modifyUserId = 0;//修改用户内码
        private DateTime modifyTime = DateTime.Now;//修改时间
        private int oprRoleID = 0;//创建修改用户角色内码
        private int oprDeptID = 0;//创建修改用户部门内码


        public override string GetTableName()
        {
            return "ProcessType";
        }
        public override string GetColumnNames()
        {
            return "ProcessTypeId,ProcessTypeCode,ProcessTypeName,Status,Note,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override string GetUpdateColumnNames()
        {
            return "ProcessTypeCode,ProcessTypeName,Status,Note,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public int ProcessTypeId
        {
            set { processTypeId = value; }
            get { return processTypeId; }
        }
        public string ProcessTypeCode
        {
            set { this.processTypeCode = value; }
            get { return this.processTypeCode; }
        }
        public string ProcessTypeName
        {
            set { processTypeName = value; }
            get { return processTypeName; }
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
        public int Status
        {
            set { processTypeStatus = value; }
            get { return processTypeStatus; }
        }
        public string Note
        {
            set { note = value; }
            get { return note; }
        }
    }
}
