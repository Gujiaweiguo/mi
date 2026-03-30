using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;

using Base.DB;
namespace RentableArea
{
    public class ProdRelation:BasePO
    {
        public static int PRODRELATION_STATUS_INVALID = 0;
        public static int PRODRELATION_STATUS_VALID = 1;

        public static int[] GetProdRelationStatus()
        {
            int[] prodRelationStatus = new int[2];
            prodRelationStatus[0] = PRODRELATION_STATUS_VALID;
            prodRelationStatus[1] = PRODRELATION_STATUS_INVALID;
            return prodRelationStatus;
        }

        public static String GetProdRelationStatusDesc(int prodRelationStatus)
        {
            if (prodRelationStatus == PRODRELATION_STATUS_INVALID)
            {
                return "WrkFlw_Disabled";
            }
            if (prodRelationStatus == PRODRELATION_STATUS_VALID)
            {
                return "WrkFlw_Enabled";
            }
            return "Null";
        }

        public static int PRODLEVEL_STATUS_ONE = 1;
        public static int PRODLEVEL_STATUS_TWO = 2;
        public static int PRODLEVEL_STATUS_THREE = 3;
        public static int PRODLEVEL_STATUS_FOUR = 4;

        public static int[] GetProdLevelStatus()
        {
            int[] prodLevelStatus = new int[4];
            prodLevelStatus[0] = PRODLEVEL_STATUS_ONE;
            prodLevelStatus[1] = PRODLEVEL_STATUS_TWO;
            prodLevelStatus[2] = PRODLEVEL_STATUS_THREE;
            prodLevelStatus[3] = PRODLEVEL_STATUS_FOUR;
            return prodLevelStatus;
        }

        public static String GetProdLevelStatusDesc(int prodLevelStatus)
        {
            if (prodLevelStatus == PRODLEVEL_STATUS_ONE)
            {
                return "一级";
            }
            if (prodLevelStatus == PRODLEVEL_STATUS_TWO)
            {
                return "二级";
            }
            if (prodLevelStatus == PRODLEVEL_STATUS_THREE)
            {
                return "三级";
            }
            if (prodLevelStatus == PRODLEVEL_STATUS_FOUR)
            {
                return "三级";
            }
            return "未知";
        }

        public String ProdRelationStatusDesc
        {
            get { return GetProdRelationStatusDesc(ProdStatus); }
        }
        private int prodID = 0;
        private string prodCode = null;
        private string prodName = null;
        private int pProdID = 0;
        private int prodLevel = 0;
        private int prodStatus = 1;

        public override String GetTableName()
        {
            return "ProdRelation";
        }

        public override String GetColumnNames()
        {
            return "ProdID,ProdCode,ProdName,PProdID,ProdLevel,ProdStatus";
        }

        public override String GetInsertColumnNames()
        {
            return "ProdID,ProdCode,ProdName,PProdID,ProdLevel,ProdStatus";
        }

        public override String GetUpdateColumnNames()
        {
            return "ProdName,ProdCode,ProdStatus";
        }

        public int ProdID
        {
            get { return prodID; }
            set { prodID = value; }
        }
        public string ProdCode
        {
            get { return prodCode; }
            set { prodCode = value; }
        }
        public string ProdName
        {
            get { return prodName; }
            set { prodName = value; }
        }
        public int PProdID
        {
            get { return pProdID; }
            set { pProdID = value; }
        }
        public int ProdLevel
        {
            get { return prodLevel; }
            set { prodLevel = value; }
        }
        public int ProdStatus
        {
            get { return prodStatus; }
            set { prodStatus = value; }
        }
    }
}
