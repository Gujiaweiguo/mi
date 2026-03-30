using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
using Base.Biz;

namespace Lease.Brand
{
    /// <summary>
    /// 品牌经营方式
    /// </summary>
    public class BrandOperateType:BasePO
    {
        private int operateTypeId = 0;//主键
        private string operateName = "";//品牌经营方式名称
        private int operateStatus = 0;//是否有效
        private string note = "";//备注
        private int createUserId = 0;//创建用户内码
        private DateTime createTime;//创建时间
        private int modifyUserId = 0;//修改用户内码
        private DateTime modifyTime;//修改时间
        private int oprRoleID = 0;//创建修改用户角色内码
        private int oprDeptID = 0;//创建修改用户部门内码

        public override string GetTableName()
        {
            return "BrandOperateType";
        }
        public override string GetColumnNames()
        {
            return "OperateTypeId,OperateName,Status,Note,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override string GetUpdateColumnNames()
        {
            return "OperateName,Status,Note,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }

        public int OperateTypeId
        {
            set { operateTypeId = value; }
            get { return operateTypeId; }
        }
        public string OperateName
        {
            set { operateName = value; }
            get { return operateName; }
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
            set { operateStatus = value; }
            get { return operateStatus; }
        }
        public string Note
        {
            set { note = value; }
            get { return note; }
        }
    }
}
