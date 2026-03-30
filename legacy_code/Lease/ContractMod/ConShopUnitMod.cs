using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Lease.ContractMod
{
    public class ConShopUnitMod:BasePO
    {
        private int shopModID;
        private int unitID;
        private int shopID;
        private decimal rentArea;
        private int rentInfo;
        private decimal rentLevel;
        private int rentStatus;

        public int ShopModID
        {
            get { return shopModID; }
            set { shopModID = value; }
        }

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
            return "ConShopUnitMod";
        }

        public override string GetColumnNames()
        {
            return "ShopModID,UnitID,ShopID,RentArea,RentInfo,RentLevel";
        }

        public override string GetInsertColumnNames()
        {
            return "ShopModID,UnitID,ShopID,RentArea,RentInfo,RentLevel";
        }

        public override string GetUpdateColumnNames()
        {
            return "UnitID,RentArea,RentInfo,RentLevel";
        }
    }
}
