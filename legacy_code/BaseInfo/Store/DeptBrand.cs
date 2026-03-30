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

        private int brandLevel =0;
        private int status = 0;
        private string brandName = "";

        public string strDeptID = "";

        public override string GetTableName()
        {
            return "DeptBrand";
        }
        public override string GetColumnNames()
        {
            //return "DeptId,BrandId,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
            return "DeptId,BrandId,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID,BrandLevel,BrandName,Status";
        }
       
        public override string GetUpdateColumnNames()
        {
            return "DeptId,BrandId,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override string GetInsertColumnNames()
        {
            return "DeptId,BrandId,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        //public override string GetQuerySql()
        //{
        //    return "select DeptId,ConShopBrand.BrandId,0 as CreateUserId,getdate() as CreateTime,0 as ModifyUserId,getdate() as ModifyTime,0 as OprRoleID,0 as OprDeptID,BrandLevel,BrandName,(case when  (deptid ='" + strDeptID + "' and DeptBrand.deptid=DeptId)  then 1 else 0 end) as Status from ConShopBrand left join DeptBrand on ConShopBrand.brandid=DeptBrand.brandid";
        //}
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
        public int BrandLevel
        {
            set { brandLevel = value; }
            get { return brandLevel; }
        }
        public int Status
        {
            set { status = value; }
            get { return status; }
        }
        public string BrandName
        {
            set { brandName = value; }
            get { return brandName; }
        }
    }
}
