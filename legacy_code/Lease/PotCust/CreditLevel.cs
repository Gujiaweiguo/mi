using System;
using System.Collections.Generic;
using System.Text;
using Base.Biz;
using Base.DB;

namespace Lease.PotCust
{
    /// <summary>
    /// 商户信用等级 add by lcp at 2009-3-19
    /// </summary>
    public class CreditLevel:BasePO
    {
        private int creditLevelId = 0;
        private string creditLevelName = "";
        private int creditLevelStatus = 0;
        private string note = "";
        private int createUserId = 0;//创建用户内码
        private DateTime createTime=DateTime.Now;//创建时间
        private int modifyUserId = 0;//修改用户内码
        private DateTime modifyTime=DateTime.Now;//修改时间
        private int oprRoleID = 0;//创建修改用户角色内码
        private int oprDeptID = 0;//创建修改用户部门内码

        public override string GetTableName()
        {
            return "CreditLevel";
        }
        public override string GetColumnNames()
        {
            return "CreditLevelId,CreditLevelName,Status,Note,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override string GetUpdateColumnNames()
        {
            return "CreditLevelName,Status,Note,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override string GetInsertColumnNames()
        {
            return "CreditLevelId,CreditLevelName,Status,Note,CreateUserId,CreateTime,OprRoleID,OprDeptID";
        }
        public int CreditLevelId
        {
            set { creditLevelId = value; }
            get { return creditLevelId; }
        }
        public string CreditLevelName
        {
            set { creditLevelName = value; }
            get { return creditLevelName; }
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
            set { creditLevelStatus = value; }
            get { return creditLevelStatus; }
        }
        public string Note
        {
            set { note = value; }
            get { return note; }
        }
    }
}
