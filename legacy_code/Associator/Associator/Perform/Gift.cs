using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Associator.Perform
{
    /*赠品信息*/
    public class Gift:BasePO
    {
        #region 私有属性
        /// <summary>
        /// 赠品ID
        /// </summary>
        private int giftID = 0;

        /// <summary>
        /// 赠品描述
        /// </summary>
        private string giftDesc = "";

        /// <summary>
        /// 是否可用积分兑换
        /// </summary>
        private int exByBonus = 0;

        /// <summary>
        /// 兑换所需积分
        /// </summary>
        private int bonusCost = 0;

        /// <summary>
        /// 是否可用小票兑换
        /// </summary>
        private int exByReceipt = 0;

        /// <summary>
        /// 兑换所需小票金额
        /// </summary>
        private decimal receiptMoney = 0;

        /// <summary>
        /// 是否每张小票只能兑换一次
        /// </summary>
        private int limitOne = 0;

        /// <summary>
        /// 是否可进行免费发放
        /// </summary>
        private int freeGift = 0;

        /// <summary>
        /// 开始时间
        /// </summary>
        private DateTime shopStartDate = DateTime.Now;

        /// <summary>
        /// 结束时间
        /// </summary>
        private DateTime shopEndDate = DateTime.Now;


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
        /// 赠品描述
        /// </summary>
        public string GiftDesc
        {
            get { return giftDesc; }
            set { giftDesc = value; }
        }

        /// <summary>
        /// 是否可用积分兑换
        /// </summary>
        public int ExByBonus
        {
            get { return exByBonus; }
            set { exByBonus = value; }
        }

        /// <summary>
        /// 兑换所需积分
        /// </summary>
        public int BonusCost
        {
            get { return bonusCost; }
            set { bonusCost = value; }
        }

        /// <summary>
        /// 是否可用小票兑换
        /// </summary>
        public int ExByReceipt
        {
            get { return exByReceipt; }
            set { exByReceipt = value; }
        }

        /// <summary>
        /// 兑换所需小票金额
        /// </summary>
        public decimal ReceiptMoney
        {
            get { return receiptMoney; }
            set { receiptMoney = value; }
        }

        /// <summary>
        /// 是否每张小票只能兑换一次
        /// </summary>
        public int LimitOne
        {
            get { return limitOne; }
            set { limitOne = value; }
        }

        /// <summary>
        /// 是否可进行免费发放
        /// </summary>
        public int FreeGift
        {
            get { return freeGift; }
            set { freeGift = value; }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime ShopStartDate
        {
            get { return shopStartDate; }
            set { shopStartDate = value; }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime ShopEndDate
        {
            get { return shopEndDate; }
            set { shopEndDate = value; }
        }

        /// <summary>
        /// 是否可用积分兑换--否
        /// </summary>
        public static int EXBYBONUS_NO = 0;

        /// <summary>
        /// 是否可用积分兑换--是
        /// </summary>
        public static int EXBYBONUS_YES = 1;


        /// <summary>
        /// 是否可用小票兑换--否
        /// </summary>
        public static int EXBYRECEIPT_NO = 0;

        /// <summary>
        /// 是否可用小票兑换--是
        /// </summary>
        public static int EXBYRECEIPT_YES = 1;

        /// <summary>
        /// 是否每张小票只能兑换一次--否
        /// </summary>
        public static int LIMITONE_NO = 0;

        /// <summary>
        /// 是否每张小票只能兑换一次--是
        /// </summary>
        public static int LIMITONE_YES = 1;

        /// <summary>
        /// 是否可进行免费发放--否
        /// </summary>
        public static int FREEGIFT_NO = 0;

        /// <summary>
        /// 是否可进行免费发放--是
        /// </summary>
        public static int FREEGIFT_YES = 1;

        #endregion


        public override string GetTableName()
        {
            return "Gift";
        }

        public override string GetColumnNames()
        {
            return "GiftID,GiftDesc,ExByBonus,BonusCost,ExByReceipt,ReceiptMoney,LimitOne,FreeGift,ShopStartDate,ShopEndDate";
        }

        public override string GetInsertColumnNames()
        {
            return "GiftID,GiftDesc,ExByBonus,BonusCost,ExByReceipt,ReceiptMoney,LimitOne,FreeGift,ShopStartDate,ShopEndDate";
        }

        public override string GetUpdateColumnNames()
        {
            return "GiftDesc,ExByBonus,BonusCost,ExByReceipt,ReceiptMoney,LimitOne,FreeGift,ShopStartDate,ShopEndDate";
        }

    }
}
