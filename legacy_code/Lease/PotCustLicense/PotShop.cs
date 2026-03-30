using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.PotCustLicense
{
    public class PotShop:BasePO
    {
        private int potShopID = 0;
        private int custID = 0;
        private string potShopName = "";
        private string mainBrand = "";
        private DateTime shopStartDate = DateTime.Now;
        private DateTime shopEndDate = DateTime.Now;
        private int areaID = 0;
        private decimal rentalPrice = 0;
        private decimal rentArea = 0;
        private string note = null;
        private int shopTypeID = 0;
        private int bizmode = 0;

        private int storeid = 0;
        private string rentInc = "";
        private string pcent = "";
        private string unitId = "";

        private string highReg = "";//层高需求
        private string loadReg = "";//荷载要求
        private string waterReg = "";//上下水
        private string powerReg = "";//电量要求

        private int sequence = 0;//WrkFlwEntity 中的排序号，用来判断属于哪个商户
        private int shopSort = 0;//商户的商铺排序号
        private int shopStatus = 0;//商户的商铺状态

        //public static int POT_SHOP_MAIN = 1;
        //public static int POT_SHOP_ADMIRAL = 2;
        //public static int POT_SHOP_REFINEMENT = 3;

        //public static int[] GetShopTypeStatus()
        //{
        //    int[] shopTypeStatus = new int[3];
        //    shopTypeStatus[0] = POT_SHOP_MAIN;
        //    shopTypeStatus[1] = POT_SHOP_ADMIRAL;
        //    shopTypeStatus[2] = POT_SHOP_REFINEMENT;

        //    return shopTypeStatus;
        //}

        //public static String GetShopTypeStatusDesc(int shopTypeStatus)
        //{
        //    if (shopTypeStatus == POT_SHOP_MAIN)
        //    {
        //        return "POT_SHOP_MAIN";//主力店";
        //    }
        //    if (shopTypeStatus == POT_SHOP_ADMIRAL)
        //    {
        //        return "POT_SHOP_ADMIRAL";// "旗舰店";
        //    }
        //    if (shopTypeStatus == POT_SHOP_REFINEMENT)
        //    {
        //        return "POT_SHOP_REFINEMENT";// "精品店";
        //    }
        //    return "NO";
        //}

        //public String ShopTypeStatusDesc
        //{
        //    get { return GetShopTypeStatusDesc(shopTypeStatus); }
        //}

        public override String GetTableName()
        {
            return "PotShop";
        }

        public override string GetColumnNames()
        {
            return "PotShopID,PotShopName,MainBrand,ShopStartDate,ShopEndDate,AreaID,RentalPrice,RentArea,Note,ShopTypeID,BizMode,UnitId,Pcent,RentInc,StoreID,HighReg,LoadReg,WaterReg,PowerReg";
        }

        public override String GetInsertColumnNames()
        {
            return "PotShopID,CustID,PotShopName,MainBrand,ShopStartDate,ShopEndDate,AreaID,RentalPrice,RentArea,Note,ShopTypeID,BizMode,UnitId,Pcent,RentInc,StoreID,HighReg,LoadReg,WaterReg,PowerReg,Sequence,ShopSort,ShopStatus";
        }

        public override String GetUpdateColumnNames()
        {
            return "PotShopName,MainBrand,ShopStartDate,ShopEndDate,AreaID,RentalPrice,RentArea,Note,ShopTypeID,BizMode,UnitId,Pcent,RentInc,StoreID,HighReg,LoadReg,WaterReg,PowerReg";
        }


        public int PotShopID
        {
            get { return potShopID; }
            set { potShopID = value; }
        }

        public int CustID
        {
            get { return custID; }
            set { custID = value; }
        }

        public string PotShopName
        {
            get { return potShopName; }
            set { potShopName = value; }
        }

        public string MainBrand
        {
            get { return mainBrand; }
            set { mainBrand = value; }
        }

        public DateTime ShopStartDate
        {
            get { return shopStartDate; }
            set { shopStartDate = value; }
        }

        public DateTime ShopEndDate
        {
            get { return shopEndDate; }
            set { shopEndDate = value; }
        }

        public int AreaID
        {
            get { return areaID; }
            set { areaID = value; }
        }

        public decimal RentalPrice
        {
            get { return rentalPrice; }
            set { rentalPrice = value; }
        }

        public decimal RentArea
        {
            get { return rentArea; }
            set { rentArea = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public int ShopTypeID
        {
            get { return shopTypeID; }
            set { shopTypeID = value; }
        }

        public int BizMode
        {
            get { return bizmode; }
            set { bizmode = value; }
        }
        public int StoreID
        {
            set { storeid = value; }
            get { return storeid; }
        }
        public string RentInc
        {
            set { rentInc = value; }
            get { return rentInc; }
        }
        public string Pcent
        {
            set { pcent = value; }
            get { return pcent; }
        }
        public string UnitId
        {
            set { unitId = value; }
            get { return unitId; }
        }
        public string HighReg
        {
            set { highReg = value; }
            get { return highReg; }
        }
        public string LoadReg
        {
            set { loadReg = value; }
            get { return loadReg; }
        }
        public string WaterReg
        {
            set { waterReg = value; }
            get { return waterReg; }
        }
        public string PowerReg
        {
            set { powerReg = value; }
            get { return powerReg; }
        }
        public int Sequence
        {
            set { sequence = value; }
            get { return sequence; }
        }
        public int ShopSort
        {
            set { this.shopSort = value; }
            get { return this.shopSort; }
        }
        public int ShopStatus
        {
            set { this.shopStatus = value; }
            get { return this.shopStatus; }
        }
    }
}
