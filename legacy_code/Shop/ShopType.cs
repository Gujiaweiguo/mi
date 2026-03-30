using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Shop.ShopType
{
    public class ShopType:BasePO
    {
        private int shopTypeID = 0;
        private string shopTypeCode = "";
        private string shopTypeName = "";
        private int shopTypeStatus = 0;
        private string note = "";
        private string shopTypeStatusName = "";


        public int ShopTypeID
        {
            set { shopTypeID = value; }
            get { return shopTypeID; }
        }

        public string ShopTypeCode
        {
            set { shopTypeCode = value; }
            get { return shopTypeCode; }
        }

        public string ShopTypeName
        {
            set { shopTypeName = value; }
            get { return shopTypeName; }
        }

        public int ShopTypeStatus
        {
            set { shopTypeStatus = value; }
            get { return shopTypeStatus; }
        }

        public string Note
        {
            set { note = value; }
            get { return note; }
        }

        public string ShopTypeStatusName
        {
            set { shopTypeStatusName = value; }
            get { return shopTypeStatusName; }
        }


        public override String GetTableName()
        {
            return "ShopType";
        }

        public override String GetColumnNames()
        {
            return "ShopTypeID,ShopTypeCode,ShopTypeName,ShopTypeStatus,ShopTypeStatusName,Note";
        }

        public override String GetUpdateColumnNames()
        {
            return "ShopTypeCode,ShopTypeName,ShopTypeStatus,Note";
        }


        public override string GetInsertColumnNames()
        {
            return "ShopTypeID,ShopTypeCode,ShopTypeName,ShopTypeStatus,Note";
        }

        public override string GetQuerySql()
        {
            return "select ShopTypeID,ShopTypeCode,ShopTypeName,ShopTypeStatus, (case ShopTypeStatus when 1 then '有效' else '无效' end) as ShopTypeStatusName,Note from ShopType";
        }

        /**
         * 状态 - 无效
         */
        public static int SHOP_TYPE_STATUS_INVALID = 0;
        /**
         * 状态 - 有效
         */
        public static int SHOP_TYPE_STATUS_VALID = 1;


        public static int[] GetShopTypeStatus()
        {
            int[] shopTypeStatus = new int[2];
            shopTypeStatus[0] = SHOP_TYPE_STATUS_VALID;
            shopTypeStatus[1] = SHOP_TYPE_STATUS_INVALID;
            return shopTypeStatus;
        }

        public static String GetShopTypeStatusDesc(int shopTypeStatus)
        {
            if (shopTypeStatus == SHOP_TYPE_STATUS_INVALID)
            {
                return "BizGrp_NO";
            }
            if (shopTypeStatus == SHOP_TYPE_STATUS_VALID)
            {
                return "BizGrp_YES";
            }
            return "NULL";
        }
    }
}
