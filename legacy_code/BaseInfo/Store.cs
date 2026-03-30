using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace BaseInfo.Store
{
    public class Store:BasePO
    {
        private int storeId = 0;//主键
        private string storeCode = "";
        private string storeName = "";//商业项目名称
        private string storeShortName = "";//商业项目简称
        private int storeType = 0;//商业项目定位类型
        private string storeAddr = "";//商业项目地址
        private string storeAmbit = "";//商业项目四至范围
        private int groundParking = 0;//地上车位
        private int underParking = 0;//地下车位
        private string officeAddr = "";//办公地址
        private string officeZip = "";//邮政编码
        private string officeTel = "";//办公电话
        private string propertyTel = "";//物业电话
        private string officeTel2 = "";//招商电话

        private int createUserId = 0;
        private DateTime createTime;
        private int modifyUserId = 0;
        private DateTime modifyTime;
        private int oprRoleID = 0;
        private int oprDeptID = 0;


        /// <summary>
        /// 表名
        /// </summary>
        /// <returns></returns>
        public override string GetTableName()
        {
            return "Store";
        }
        /// <summary>
        /// 字段
        /// </summary>
        /// <returns></returns>
        public override string GetColumnNames()
        {
            return "StoreId,StoreCode,StoreName,StoreShortName,StoreType,StoreAddr,StoreAmbit,GroundParking,UnderParking,OfficeAddr,OfficeZip,OfficeTel,PropertyTel,OfficeTel2,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override string GetUpdateColumnNames()
        {
            return "StoreCode,StoreName,StoreShortName,StoreType,StoreAddr,StoreAmbit,GroundParking,UnderParking,OfficeAddr,OfficeZip,OfficeTel,PropertyTel,OfficeTel2,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        

        public int StoreId//
        {
            set { this.storeId = value; }
            get { return this.storeId; }
        }
        public string StoreCode//
        {
            set { this.storeCode = value; }
            get { return storeCode; }
        }
        public string StoreName//
        {
            set { this.storeName = value; }
            get { return this.storeName; }
        }
        public string StoreShortName//
        {
            set { this.storeShortName = value; }
            get { return this.storeShortName; }
        }
        public int StoreType//
        {
            set { this.storeType = value; }
            get { return storeType; }
        }
        public  string StoreAddr//
        {
            set { this.storeAddr = value; }
            get { return this.storeAddr; }
        }
        public string StoreAmbit//
        {
            set { this.storeAmbit = value; }
            get { return storeAmbit; }
        }
        public int GroundParking//
        {
            set { this.groundParking = value; }
            get { return groundParking; }
        }
        public int  UnderParking//
        {
            set { this.underParking = value; }
            get { return underParking; }
        }
        public string OfficeAddr//
        {
            set { this.officeAddr = value; }
            get { return officeAddr; }
        }
        public string OfficeZip//
        {
            set { this.officeZip = value; }
            get { return officeZip; }
        }
        public string OfficeTel//
        {
            set { this.officeTel = value; }
            get { return officeTel; }
        }
        public string PropertyTel//
        {
            set { this.propertyTel = value; }
            get { return propertyTel; }
        }
        public string OfficeTel2//
        {
            set { this.officeTel2 = value; }
            get { return officeTel2; }
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
