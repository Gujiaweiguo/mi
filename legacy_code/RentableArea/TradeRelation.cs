using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;

using Base.DB;
namespace RentableArea
{
    public class TradeRelation:BasePO
    {
        public static int TRADERELATION_STATUS_INVALID = 0;
        public static int TRADERELATION_STATUS_VALID = 1;

        public static int[] GetTradeRelationStatus()
        {
            int[] aradeRelationStaus = new int[2];
            aradeRelationStaus[0] = TRADERELATION_STATUS_VALID;
            aradeRelationStaus[1] = TRADERELATION_STATUS_INVALID;
            return aradeRelationStaus;
        }

        public static String GetTradeRelationStatusDesc(int aradeRelationStaus)
        {
            if (aradeRelationStaus == TRADERELATION_STATUS_INVALID)
            {
                return "WrkFlw_Disabled";
            }
            if (aradeRelationStaus == TRADERELATION_STATUS_VALID)
            {
                return "WrkFlw_Enabled";
            }
            return "Null";
        }


        public static int TRADELEVEL_STATUS_ONE = 1;
        public static int TRADELEVEL_STATUS_TWO = 2;
        public static int TRADELEVEL_STATUS_THREE = 3;

        public static int[] GetTradeLevelStatus()
        {
            int[] tradeLevelStaus = new int[3];
            tradeLevelStaus[0] = TRADELEVEL_STATUS_ONE;
            tradeLevelStaus[1] = TRADELEVEL_STATUS_TWO;
            tradeLevelStaus[2] = TRADELEVEL_STATUS_THREE;
            return tradeLevelStaus;
        }

        public static String GetTradeLevelStatusDesc(int tradeLevelStaus)
        {
            if (tradeLevelStaus == TRADELEVEL_STATUS_ONE)
            {
                return "一级";
            }
            if (tradeLevelStaus == TRADELEVEL_STATUS_TWO)
            {
                return "二级";
            }
            if (tradeLevelStaus == TRADELEVEL_STATUS_THREE)
            {
                return "三级";
            }
            return "未知";
        }

        public String TradeRelationStatusDesc
        {
            get { return GetTradeRelationStatusDesc(TradeStatus); }
        }
        private int tradeID = 0;
        private string tradeCode = null;
        private string tradeName = null;
        private int pTradeID = 0;
        private int tradeLevel = 0;
        private int tradeStatus = 1;
        private int createUserId = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserId = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;
        private int rb = 0;
        private int gb = 0;
        private int bb = 0;

        public override String GetTableName()
        {
            return "TradeRelation";
        }

        public override String GetColumnNames()
        {
            return "TradeID,TradeCode,TradeName,PTradeID,TradeLevel,TradeStatus,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID,Rb,Gb,Bb";
        }

        public override String GetInsertColumnNames()
        {
            return "TradeID,TradeCode,TradeName,PTradeID,TradeLevel,TradeStatus,CreateUserId,CreateTime,Rb,Gb,Bb";
        }

        public override String GetUpdateColumnNames()
        {
            return "TradeName,TradeCode,TradeStatus,ModifyUserId,ModifyTime,Rb,Gb,Bb";
        }

        public int TradeID
        {
            get { return tradeID; }
            set { tradeID = value; }
        }
        public string TradeCode
        {
            get { return tradeCode; }
            set { tradeCode = value; }
        }
        public string TradeName
        {
            get { return tradeName; }
            set { tradeName = value; }
        }
        public int PTradeID
        {
            get { return pTradeID; }
            set { pTradeID = value; }
        }
        public int TradeLevel
        {
            get { return tradeLevel; }
            set { tradeLevel = value; }
        }
        public int TradeStatus
        {
            get { return tradeStatus; }
            set { tradeStatus = value; }
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
        public int Rb
        {
            set { rb = value; }
            get { return rb; }
        }
        public int Gb
        {
            set { gb = value; }
            get { return gb; }
        }
        public int Bb
        {
            set { bb = value; }
            get { return bb; }
        }
    }
}
