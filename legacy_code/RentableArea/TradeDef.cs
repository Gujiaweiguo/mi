using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace RentableArea
{
    public class TradeDef:BasePO
    {
        public static int TRADEDEF_STATUS_INVALID = 0;
        public static int TRADEDEF_STATUS_VALID = 1;

        public static int[] GetTradeDefStatus()
        {
            int[] tradeDefStaus = new int[2];
            tradeDefStaus[0] = TRADEDEF_STATUS_INVALID;
            tradeDefStaus[1] = TRADEDEF_STATUS_VALID;
            return tradeDefStaus;
        }

        public static String GetTradeDefStatusDesc(int tradeDefStaus)
        {
            if (tradeDefStaus == TRADEDEF_STATUS_INVALID)
            {
                return "无效";
            }
            if (tradeDefStaus == TRADEDEF_STATUS_VALID)
            {
                return "有效";
            }
            return "未知";
        }


        private int trade3ID = 0;
        private string trade3Code = null;
        private string trade3Name = null;

        private int trade2ID = 0;
        private string trade2Code = null;
        private string trade2Name = null;

        private int trade1ID = 0;
        private string trade1Code = null;
        private string trade1Name = null;

        private int tradeDefStatus = 1;
        private string note = null;

        public override String GetTableName()
        {
            return "TradeDef";
        }

        public override String GetColumnNames()
        {
            return "Trade2ID,Trade2Code,Trade2Name,Trade1ID,Trade1Code,Trade1Name,TradeDefStatus,Note";
        }

        public override String GetInsertColumnNames()
        {
            return "Trade2ID,Trade2Code,Trade2Name,Trade1ID,Trade1Code,Trade1Name,TradeDefStatus";
        }

        public override String GetUpdateColumnNames()
        {
            return "Trade2Code,Trade2Name,Trade1Code,Trade1Name,TradeDefStatus";
        }

        //public int Trade3ID
        //{
        //    get { return trade3ID; }
        //    set { trade3ID = value; }
        //}
        //public string Trade3Code
        //{
        //    get { return trade3Code; }
        //    set { trade3Code = value; }
        //}
        //public string Trade3Name
        //{
        //    get { return trade3Name; }
        //    set { trade3Name = value; }
        //}

        public int Trade2ID
        {
            get { return trade2ID; }
            set { trade2ID = value; }
        }
        public string Trade2Code
        {
            get { return trade2Code; }
            set { trade2Code = value; }
        }
        public string Trade2Name
        {
            get { return trade2Name; }
            set { trade2Name = value; }
        }

        public int Trade1ID
        {
            get { return trade1ID; }
            set { trade1ID = value; }
        }
        public string Trade1Code
        {
            get { return trade1Code; }
            set { trade1Code = value; }
        }
        public string Trade1Name
        {
            get { return trade1Name; }
            set { trade1Name = value; }
        }

        public int TradeDefStatus
        {
            get { return tradeDefStatus; }
            set { tradeDefStatus = value; }
        }
        public string Note
        {
            get { return note; }
            set { note = value; }
        }
    }
}
