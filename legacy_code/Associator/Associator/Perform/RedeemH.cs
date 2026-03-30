using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Associator.Perform
{
    /// <summary>
    /// 积分兑换记录主表
    /// </summary>
    public class RedeemH : BasePO
    {
        private int redeemID = 0;  //赠品兑换ID
        private int membID = 0;      //会员ID
        private DateTime redeemDate = DateTime.Now;  //兑换日期
        private decimal bonusPrev = 0;  //兑换前积分
        private decimal redeemAmt = 0;  //赠品兑换总积分
        private decimal bonusCurr = 0;  //兑换后积分
        private int giftID = 0;

        public int RedeemID
        {
            get { return redeemID; }
            set { redeemID = value; }
        }

        public int MembID
        {
            get { return membID; }
            set { membID = value; }
        }

        public int GiftID
        {
            get { return giftID; }
            set { giftID = value; }
        }

        public DateTime RedeemDate
        {
            get { return redeemDate; }
            set { redeemDate = value; }
        }

        public decimal BonusPrev
        {
            get { return bonusPrev; }
            set { bonusPrev = value; }
        }

        public decimal RedeemAmt
        {
            get { return redeemAmt; }
            set { redeemAmt = value; }
        }

        public decimal BonusCurr
        {
            get { return bonusCurr; }
            set { bonusCurr = value; }
        }

        public override string GetTableName()
        {
            return "RedeemH";
        }

        public override string GetColumnNames()
        {
            return "RedeemID,MembID,GiftID,RedeemDate,BonusPrev,RedeemAmt,BonusCurr";
        }

        public override string GetInsertColumnNames()
        {
            return "RedeemID,MembID,GiftID,RedeemDate,BonusPrev,RedeemAmt,BonusCurr";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }
    }
}
