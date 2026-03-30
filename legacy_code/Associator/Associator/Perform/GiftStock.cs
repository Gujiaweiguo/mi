using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Associator.Perform
{
    public class GiftStock:BasePO
    {
        /// <summary>
        /// 库存点ID
        /// </summary>
        private int stockID = 0;

        /// <summary>
        /// 赠品
        /// </summary>
        private int giftID = 0;

        /// <summary>
        /// 服务台ID
        /// </summary>
        private int counterID = 0;

        /// <summary>
        /// 库存数量
        /// </summary>
        private int stockCnt = 0;

        /// <summary>
        /// 参考价格
        /// </summary>
        private decimal refPrice = 0;



        /// <summary>
        /// 库存点ID
        /// </summary>
        public int StockID
        {
            get { return stockID; }
            set { stockID = value; }
        }

        /// <summary>
        /// 赠品
        /// </summary>
        public int GiftID
        {
            get { return giftID; }
            set { giftID = value; }
        }

        /// <summary>
        /// 服务台ID
        /// </summary>
        public int CounterID
        {
            get { return counterID; }
            set { counterID = value; }
        }


        /// <summary>
        /// 库存数量
        /// </summary>
        public int StockCnt
        {
            get { return stockCnt; }
            set { stockCnt = value; }
        }


        /// <summary>
        /// 参考价格
        /// </summary>
        public decimal RefPrice
        {
            get { return refPrice; }
            set { refPrice = value; }
        }


        public override string GetTableName()
        {
            return "GiftStock";
        }

        public override string GetColumnNames()
        {
            return "GiftID,StockID,StockCnt,RefPrice,CounterID";
        }

        public override string GetInsertColumnNames()
        {
            return "GiftID,StockID,StockCnt,RefPrice,CounterID";
        }

        public override string GetUpdateColumnNames()
        {
            return "GiftID,StockID,StockCnt,RefPrice,CounterID";
        }
    }
}
