using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Lease.ConShop
{
    /// <summary>
    /// 商铺硬件信息
    /// </summary>
    public class ShopHdw:BasePO
    {
        private int hdwID = 0;
        private int hdwTypeID = 0;
        private int shopID = 0;
        private string hdwCode = "";
        private string hdwName = "";
        private int hdwQty = 0;
        private string hdwUnit = "";
        private string hdwStd = "";
        private int hdwCond = 0;
        private int owner = 0;
        private string note = "";

        public int HdwID
        {
            get { return hdwID; }
            set { hdwID = value; }
        }

        public int HdwTypeID
        {
            get { return hdwTypeID; }
            set { hdwTypeID = value; }
        }

        public int ShopID
        {
            get { return shopID; }
            set { shopID = value; }
        }

        public string HdwCode
        {
            get { return hdwCode; }
            set { hdwCode = value; }
        }

        public string HdwName
        {
            get { return hdwName; }
            set { hdwName = value; }
        }

        public int HdwQty
        {
            get { return hdwQty; }
            set { hdwQty = value; }
        }

        public string HdwUnit
        {
            get { return hdwUnit; }
            set { hdwUnit = value; }
        }

        public string HdwStd
        {
            get { return hdwStd; }
            set { hdwStd = value; }
        }

        public int HdwCond
        {
            get { return hdwCond; }
            set { hdwCond = value; }
        }

        public int Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public override string GetTableName()
        {
            return "ShopHdw";
        }

        public override string GetColumnNames()
        {
            return "HdwID,HdwTypeID,ShopID,HdwCode,HdwName,HdwQty,HdwUnit,HdwStd,HdwCond,Owner,Note";
        }

        public override string GetInsertColumnNames()
        {
            return "HdwID,HdwTypeID,ShopID,HdwCode,HdwName,HdwQty,HdwUnit,HdwStd,HdwCond,Owner,Note";
        }

        public override string GetUpdateColumnNames()
        {
            return "HdwTypeID,ShopID,HdwCode,HdwName,HdwQty,HdwUnit,HdwStd,HdwCond,Owner,Note";
        }
        public override string GetQuerySql()
        {
            return "select HdwID,HdwTypeID,shophdw.ShopID,HdwCode,HdwName,HdwQty,HdwUnit,HdwStd,HdwCond,Owner,Note from shophdw inner join conshop on shophdw.shopid=conshop.shopid";
        }

        //硬件状况 1:正常 2:损坏
        public static int HDWCOND_GOOD = 1;
        public static int HDWCOND_BAD = 2;

        public static int[] GetHdwCond()
        {
            int[] HdwCond = new int[2];
            HdwCond[0] = HDWCOND_GOOD;
            HdwCond[1] = HDWCOND_BAD;
            return HdwCond;
        }

        public static string GetHdwCondDesc(int HdwCond)
        {
            if (HdwCond == HDWCOND_GOOD)
            {
                return "CONTRACTSTATUS_TYPE_INGEAR"; // "正常";
            }
            if (HdwCond == HDWCOND_BAD)
            {
                return "HdwCond_BAD";// "损坏";
            }
            return "NO";
        }


        //资产拥有者:Owner 1:购物中心 2:商铺
        public static int OWNER_MALL = 1;
        public static int OWNER_SHOP = 2;

        public static int[] GetOwner()
        {
            int[] owner = new int[2];
            owner[0] = OWNER_MALL;
            owner[1] = OWNER_SHOP;
            return owner;
        }

        public static string GetOwnerDesc(int owner)
        {
            if (owner == OWNER_MALL)
            {
                return "ShopHdw_Mall";  //购物中心
            }
            if (owner == OWNER_SHOP)
            {
                return "ShopHdw_Shop";  //商铺
            }
            return "NO";
        }

    }
}
