using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace BaseInfo.Store
{
    public class StoreLicense:BasePO
    {
        private int licenseId = 0;//主键
        private int storeId = 0;//关联
        private int licenseTypeId = 0;//证件类型内码
        private string licenseCode = "";//证件编码
        private string propertyName = "";//物业名称
        private string propertyOwner = "";//产权人
        private decimal area = 0;//权证面积
        private int usage = 0;//设计用途:非居住；居住；商住等
        private DateTime regDate;//发证日期
        private string files = "";//存档位置

        public static int USAGE_NO_HABITATION = 1;//非居住
        public static int USAGE_HABITATION = 2; //居住

        private int createUserId = 0;
        private DateTime createTime;
        private int modifyUserId = 0;
        private DateTime modifyTime;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        /// <summary>
        /// 居住类型
        /// </summary>
        /// <returns></returns>
        public static int[] GetUsageType()
        {
            int[] GetUsageType = new int[2];

            GetUsageType[0] = USAGE_NO_HABITATION;
            GetUsageType[1] = USAGE_HABITATION;
            return GetUsageType;
        }
        public static String GetUsageTypeDesc(int UsageType)
        {
            if (UsageType == USAGE_NO_HABITATION)
            {
                return "Store_USAGE_NO_HABITATION";
            }
            else if (UsageType == USAGE_HABITATION)
            {
                return "Store_USAGE_HABITATION";
            }
            else
                return "NO";
        }

        /// <summary>
        /// 表
        /// </summary>
        /// <returns></returns>
        public override string GetTableName()
        {
            return "StoreLicense";
        }
        /// <summary>
        /// 
        /// </summary>
        public override string GetColumnNames()
        {
            return "LicenseId,StoreId,LicenseTypeId,LicenseCode,PropertyName,PropertyOwner,Area,Usage,RegDate,Files,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override string GetUpdateColumnNames()
        {
            return "LicenseTypeId,LicenseCode,PropertyName,PropertyOwner,Area,Usage,RegDate,Files,ModifyUserId,ModifyTime,OprRoleID,OprDeptID"; 
        }
        public int LicenseId
        {
            set { this.licenseId = value; }
            get { return licenseId; }
        }
        public int StoreId
        {
            set { this.storeId = value; }
            get { return storeId; }
        }
        public int LicenseTypeId
        {
            set { this.licenseTypeId = value; }
            get { return licenseTypeId; }
        }
        public string LicenseCode
        {
            set { this.licenseCode = value; }
            get { return licenseCode; }
        }
        public string PropertyName
        {
            set { this.propertyName = value; }
            get { return propertyName; }
        }
        public string PropertyOwner
        {
            set { this.propertyOwner = value; }
            get { return propertyOwner; }
        }
        public decimal Area
        {
            set { this.area = value; }
            get { return area; }
        }
        public int Usage
        {
            set { this.usage = value; }
            get { return usage; }
        }
        public DateTime RegDate
        {
            set { this.regDate = value; }
            get { return regDate; }
        }
        public string Files
        {
            set { this.files = value; }
            get { return files; }
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
