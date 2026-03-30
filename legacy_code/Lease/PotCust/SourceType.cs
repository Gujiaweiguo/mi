using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
using Base.Biz;

namespace Lease.PotCust
{
    /// <summary>
    /// 商户来源类型 add by lcp at 2009-3-19
    /// </summary>
    public class SourceType:BasePO
    {
        private int sourceTypeId = 0;//
        private string sourceTypeName = "";//名称
        private int sourceTypeStatus = 0;//是否有效
        private string note = "";//备注
        private int createUserId = 0;//创建用户内码
        private DateTime createTime=DateTime.Now;//创建时间
        private int modifyUserId = 0;//修改用户内码
        private DateTime modifyTime=DateTime.Now;//修改时间
        private int oprRoleID = 0;//创建修改用户角色内码
        private int oprDeptID = 0;//创建修改用户部门内码


        public override string GetTableName()
        {
            return "SourceType";
        }
        public override string GetColumnNames()
        {
            return "SourceTypeId,SourceTypeName,SourceTypeStatus,Note,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override string GetUpdateColumnNames()
        {
            return "SourceTypeName,SourceTypeStatus,Note,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public int SourceTypeId
        {
            set { sourceTypeId = value; }
            get { return sourceTypeId; }
        }
        public string SourceTypeName
        {
            set { sourceTypeName = value; }
            get { return sourceTypeName; }
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
        public int SourceTypeStatus
        {
            set { sourceTypeStatus = value; }
            get { return sourceTypeStatus; }
        }
        public string Note
        {
            set { note = value; }
            get { return note; }
        }
    }
}
