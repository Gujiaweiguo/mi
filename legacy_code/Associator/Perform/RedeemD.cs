using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Associator.Perform
{
    /// <summary>
    /// 积分兑换记录明细表
    /// </summary>
    public class RedeemD : BasePO
    {
        private int redeemID = 0;  //赠品兑换ID
        private int giftID = 0;  //赠品ID
        private int giftQty = 0;  //赠品数量
        private int giftPoint = 0;  //赠品单位积分
        private int totalPoint = 0;  //赠品总积分

        public int RedeemID
        {
            get { return redeemID; }
            set { redeemID = value; }
        }

        public int GiftID
        {
            get { return giftID; }
            set { giftID = value; }
        }

        public int GiftQty
        {
            get { return giftQty; }
            set { giftQty = value; }
        }

        public int GiftPoint
        {
            get { return giftPoint; }
            set { giftPoint = value; }
        }

        public int TotalPoint
        {
            get { return totalPoint; }
            set { totalPoint = value; }
        }

        public override string GetTableName()
        {
            return "RedeemD";
        }

        public override string GetColumnNames()
        {
            return "RedeemID,GiftID,GiftQty,GiftPoint,TotalPoint";
        }

        public override string GetInsertColumnNames()
        {
            return "RedeemID,GiftID,GiftQty,GiftPoint,TotalPoint";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }
    }
}
