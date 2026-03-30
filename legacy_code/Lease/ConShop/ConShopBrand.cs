using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Lease.ConShop
{
    public class ConShopBrand:BasePO
    {
        private int brandID;
        private string brandName;
        private int brandLevel;
        private string brandRegDesc;
        private string brandProduce;
        private string brandTargetCust;

        public int BrandId
        {
            get { return brandID; }
            set { brandID = value; }
        }

        public string BrandName
        {
            get { return brandName; }
            set { brandName = value; }
        }

        public int BrandLevel
        {
            get { return brandLevel; }
            set { brandLevel = value; }
        }

        public string BrandRegDesc
        {
            get { return brandRegDesc; }
            set { brandRegDesc = value; }
        }

        public string BrandProduce
        {
            get { return brandProduce; }
            set { brandProduce = value; }
        }

        public string BrandTargetCust
        {
            get { return brandTargetCust; }
            set { brandTargetCust = value; }
        }

        public override string GetTableName()
        {
            return "ConShopBrand";
        }

        public override string GetColumnNames()
        {
            return "BrandId,BrandName,BrandLevel,BrandRegDesc,BrandProduce,BrandTargetCust";
        }

        public override string GetQuerySql()
        {
            return "Select BrandId,BrandName,BrandLevel,BrandRegDesc,BrandProduce,BrandTargetCust,'' as BrandLevelName from ConShopBrand";
        }

        public override string GetInsertColumnNames()
        {
            return "BrandId,BrandName,BrandLevel,BrandRegDesc,BrandProduce,BrandTargetCust";
        }

        public override string GetUpdateColumnNames()
        {
            return "BrandName,BrandLevel,BrandRegDesc,BrandProduce,BrandTargetCust";
        }

        //Æ·ÅÆµÈ¼¶ BrandLevel
        public static int BRANDLEVEL_INTERNATIONAL = 1;
        public static int BRANDLEVEL_CONTRY = 2;
        public static int BRANDLEVEL_PROVINCE = 3;

        public static int[] GetBrandLevel()
        {
            int [] brandLevel = new int[3];
            brandLevel[0] = BRANDLEVEL_INTERNATIONAL;
            brandLevel[1] = BRANDLEVEL_CONTRY;
            brandLevel[2] = BRANDLEVEL_PROVINCE;
            return brandLevel;
        }

        public static string GetBrandLevelDesc(int brandLevel)
        {
            if (brandLevel == BRANDLEVEL_INTERNATIONAL)
            {
                return "ConShopBrand_International";
            }
            if (brandLevel == BRANDLEVEL_CONTRY)
            {
                return "ConShopBrand_Domestic";
            }
            if (brandLevel == BRANDLEVEL_PROVINCE)
            {
                return "ConShopBrand_Province";
            }
            return "Null";
        }

    }
}
