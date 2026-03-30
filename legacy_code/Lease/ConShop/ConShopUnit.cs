using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Lease.ConShop
{
    public class ConShopUnit:BasePO
    {
        private int unitID;
        private int shopID;
        private decimal rentArea;
        private int rentInfo;
        private decimal rentLevel;
        private int rentStatus;

        public int UnitID
        {
            get { return unitID; }
            set { unitID = value; }
        }

        public int ShopID
        {
            get { return shopID; }
            set { shopID = value; }
        }

        public decimal RentArea
        {
            get { return rentArea; }
            set { rentArea = value; }
        }

        public int RentInfo
        {
            get { return rentInfo; }
            set { rentInfo = value; }
        }

        public decimal RentLevel
        {
            get { return rentLevel; }
            set { rentLevel = value; }
        }

        public int RentStatus
        {
            get { return rentStatus; }
            set { rentStatus = value; }
        }

        public override string GetTableName()
        {
            return "ConShopUnit";
        }

        public override string GetColumnNames()
        {
            return "UnitID,ShopID,RentArea,RentInfo,RentLevel";
        }

        public override string GetInsertColumnNames()
        {
            return "UnitID,ShopID,RentArea,RentInfo,RentLevel";
        }

        public override string GetUpdateColumnNames()
        {
            return "UnitID,ShopID,RentArea,RentInfo,RentLevel";
        }

        //出租状态　RentStatus
        public static int RENTSTATUS_TYPE_NO = 0;
        public static int RENTSTATUS_TYPE_YES = 1;

        public static int[] GetRentStatusStatus()
        {
            int[] RentStatus = new int[2];
            RentStatus[0] = RENTSTATUS_TYPE_NO;
            RentStatus[1] = RENTSTATUS_TYPE_YES;
            return RentStatus;
        }

        public static string GetRentStatusStatusDesc(int RentStatus)
        {
            if (RentStatus == RENTSTATUS_TYPE_NO)
            {
                return "无效";
            }
            if (RentStatus == RENTSTATUS_TYPE_NO)
            {
                return "有效";
            }
            return "未知";
        }
    }
}
