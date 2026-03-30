using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Associator.Perform
{
    public class GiftStockQuery:BasePO
    {
        #region 私有属性

        /// <summary>
        /// 赠品ID
        /// </summary>
        private int giftID = 0;

        /// <summary>
        /// 服务台
        /// </summary>
        private string counterDesc = "";

        /// <summary>
        /// 赠品描述
        /// </summary>
        private string giftDesc = "";

        /// <summary>
        /// 数量
        /// </summary>
        private int stockCnt = 0;

        /// <summary>
        /// 金额
        /// </summary>
        private decimal refPrice = 0;


        #endregion

        #region 公共属性

        /// <summary>
        /// 赠品ID
        /// </summary>
        public int GiftID
        {
            get { return giftID; }
            set { giftID = value; }
        }

        /// <summary>
        /// 服务台
        /// </summary>
        public string CounterDesc
        {
            get { return counterDesc; }
            set { counterDesc = value; }
        }

        /// <summary>
        /// 赠品描述
        /// </summary>
        public string GiftDesc
        {
            get { return giftDesc; }
            set { giftDesc = value; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public int StockCnt
        {
            get { return stockCnt; }
            set { stockCnt = value; }
        }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal RefPrice
        {
            get { return refPrice; }
            set { refPrice = value; }
        }
        #endregion


        public override string GetTableName()
        {
            return "";
        }

        public override string GetColumnNames()
        {
            return "GiftID,CounterDesc,GiftDesc,StockCnt,RefPrice";
        }

        public override string GetInsertColumnNames()
        {
            return "";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public override string GetQuerySql()
        {
            return "Select GiftStock.GiftID,CounterDesc,GiftDesc,StockCnt,RefPrice From GiftStock Left Join Gift On GiftStock.GiftID = Gift.GiftID Left Join" +
                   " Counter On GiftStock.CounterID = Counter.CounterID";
        }
    }
}
