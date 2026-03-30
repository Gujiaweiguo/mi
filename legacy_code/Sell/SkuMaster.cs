using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Sell
{
    /// <summary>
    /// 商品编码
    /// </summary>
    public class SkuMaster : BasePO
    {
        private string tenantId = "";
        private string skuId = "";
        private string skuDesc = "";
        private decimal unitPrice = 0;
        private decimal unitPrice2 = 0;
        private string deptId = "";
        private string catgId = "";
        private string classId = "";
        private string pcode = "";
        private string brand = "";
        private string spec = "";
        private string scolor = "";
        private string unit = "";
        private string produce = "";
        private string level = "";
        private string status = "";
        private string dataSource = "";

        private string isClassCode = "";
        private DateTime entryAt = DateTime.Now;
        private string entryBy = "";
        private DateTime modifyAt = DateTime.Now;
        private string modifyBy = "";
        private string component = "";
        private decimal dpriceMin = 0;
        private decimal dpriceMax = 0;
        private decimal dstock = 0;
        private DateTime deletetime = Convert.ToDateTime("1900-1-1");
        private string chlocked = "";
        private string isdiscountCode = "N";  //是否是降价商品：降价商品抽成同合同不同
        private decimal discountPcentRate = 0;
        private decimal bonusGpPer = 0;


        /// <summary>
        /// 不锁定
        /// </summary>
        public static string CHLOCKED_NOT_LOCK = "0";

        /// <summary>
        /// 锁定
        /// </summary>
        public static string CHLOCKED_LOCK = "1";

        public string TenantId
        {
            get { return tenantId; }
            set { tenantId = value; }
        }

        public string SkuId
        {
            get { return skuId; }
            set { skuId = value; }
        }

        public string SkuDesc
        {
            get { return skuDesc; }
            set { skuDesc = value; }
        }

        public decimal UnitPrice
        {
            get { return unitPrice; }
            set { unitPrice = value; }
        }

        public decimal UnitPrice2
        {
            get { return unitPrice2; }
            set { unitPrice2 = value; }
        }

        public string DeptId
        {
            get { return deptId; }
            set { deptId = value; }
        }

        public string CatgId
        {
            get { return catgId; }
            set { catgId = value; }
        }

        public string ClassId
        {
            get { return classId; }
            set { classId = value; }
        }

        public string Pcode
        {
            get { return pcode; }
            set { pcode = value; }
        }

        public string Brand
        {
            get { return brand; }
            set { brand = value; }
        }

        public string Spec
        {
            get { return spec; }
            set { spec = value; }
        }
        public string color
        {
            get { return scolor; }
            set { scolor = value; }
        }


        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        public string Produce
        {
            get { return produce; }
            set { produce = value; }
        }

        public string Level
        {
            get { return level; }
            set { level = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public string DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        public string IsClassCode
        {
            get { return isClassCode; }
            set { isClassCode = value; }
        }

        public DateTime EntryAt
        {
            get { return entryAt; }
            set { entryAt = value; }
        }

        public string EntryBy
        {
            get { return entryBy; }
            set { entryBy = value; }
        }

        public DateTime ModifyAt
        {
            get { return modifyAt; }
            set { modifyAt = value; }
        }

        public string ModifyBy
        {
            get { return modifyBy; }
            set { modifyBy = value; }
        }

        public string Component
        {
            get { return component; }
            set { component = value; }
        }

        public decimal dPriceMin
        {
            get { return dpriceMin; }
            set { dpriceMin = value; }
        }

        public decimal dPriceMax
        {
            get { return dpriceMax; }
            set { dpriceMax = value; }
        }

        public decimal dStock
        {
            get { return dstock; }
            set { dstock = value; }
        }

        public DateTime Deletetime
        {
            get { return deletetime; }
            set { deletetime = value; }
        }

        public string chLocked
        {
            get { return chlocked; }
            set { chlocked = value; }
        }

        public string isDiscountCode
        {
            get { return isdiscountCode; }
            set { isdiscountCode = value; }
        }

        public decimal DiscountPcentRate
        {
            get { return discountPcentRate; }
            set { discountPcentRate = value; }
        }

        public decimal BonusGpPer
        {
            get { return bonusGpPer; }
            set { bonusGpPer = value; }
        }

 

        public override string GetTableName()
        {
            return "SkuMaster";
        }

        public override string GetColumnNames()
        {
            return "TenantId,SkuId,SkuDesc,UnitPrice,UnitPrice2,DeptId,CatgId,ClassId,Pcode,Brand,Spec,color,Unit,Produce,Level,Status,DataSource,IsClassCode,EntryAt,EntryBy,ModifyAt,ModifyBy,Component,dPriceMin,dPriceMax,dStock,Deletetime,chLocked,isDiscountCode,DiscountPcentRate,BonusGpPer";
        }

        public override string GetUpdateColumnNames()
        {
            return "TenantId,SkuDesc,UnitPrice,UnitPrice2,DeptId,CatgId,ClassId,Pcode,Brand,Spec,color,Unit,Produce,Level,Status,DataSource,IsClassCode,EntryAt,EntryBy,ModifyAt,ModifyBy,Component,dPriceMin,dPriceMax,dStock,chLocked,isDiscountCode,DiscountPcentRate,BonusGpPer";
        }

        public override string GetInsertColumnNames()
        {
            return "TenantId,SkuId,SkuDesc,UnitPrice,UnitPrice2,DeptId,CatgId,ClassId,Pcode,Brand,Spec,color,Unit,Produce,Level,Status,DataSource,IsClassCode,EntryAt,EntryBy,ModifyAt,ModifyBy,Component,dPriceMin,dPriceMax,dStock,chLocked,isDiscountCode,DiscountPcentRate,BonusGpPer";
        }
    }
}
