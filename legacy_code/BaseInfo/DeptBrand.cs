using System;
using System.Collections.Generic;
using System.Text;
using Base.Biz;
using Base.DB;
namespace BaseInfo.Store
{
    public class DeptBrand:BasePO
    {
        private int deptId = 0;
        private int brandId = 0;
        private int createUserId = 0;
        private DateTime createTime;
        private int modifyUserId = 0;
        private DateTime modifyTime;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        public override string GetTableName()
        {
            return "DeptBrand";
        }
        public override string GetColumnNames()
        {
            return "DeptId,BrandId,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
       
        public override string GetUpdateColumnNames()
        {
            return "DeptId,BrandId,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }

        public int DeptId
        {
            set { deptId = value; }
            get { return deptId; }
        }
        public int BrandId
        {
            set { brandId = value; }
            get { return brandId; }
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
