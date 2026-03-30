using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Associator.Perform
{
    /// <summary>
    /// 免费发放记录
    /// </summary>
    public class FreeGiftTrans:BasePO
    {
        /// <summary>
        /// 赠品发放ID
        /// </summary>
        private int giftTransID = 0;

        /// <summary>
        /// 赠品ID
        /// </summary>
        private int giftID = 0;

        /// <summary>
        /// 活动ID
        /// </summary>
        private int actID = 0;

        /// <summary>
        /// 会员ID
        /// </summary>
        private int membID = 0;

        /// <summary>
        /// 发放日期
        /// </summary>
        private DateTime actDate = DateTime.Now;

        /// <summary>
        /// 发放数量
        /// </summary>
        private int giftQty = 0;

        /// <summary>
        /// 赠品发放ID
        /// </summary>
        public int GiftTransID
        {
            get { return giftTransID; }
            set { giftTransID = value; }
        }

        /// <summary>
        /// 赠品ID
        /// </summary>
        public int GiftID
        {
            get { return giftID; }
            set { giftID = value; }
        }

        /// <summary>
        /// 活动ID
        /// </summary>
        public int ActID
        {
            get { return actID; }
            set { actID = value; }
        }

        /// <summary>
        /// 会员ID
        /// </summary>
        public int MembID
        {
            get { return membID; }
            set { membID = value; }
        }


        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime ActDate
        {
            get { return actDate; }
            set { actDate = value; }
        }

        /// <summary>
        /// 发放数量
        /// </summary>
        public int GiftQty
        {
            get { return giftQty; }
            set { giftQty = value; }
        }

        public override string GetTableName()
        {
            return "FreeGiftTrans";
        }

        public override string GetColumnNames()
        {
            return "GiftTransID,GiftID,ActID,MembID,ActDate,GiftQty";
        }

        public override string GetInsertColumnNames()
        {
            return "GiftTransID,GiftID,ActID,MembID,ActDate,GiftQty";
        }

        public override string GetUpdateColumnNames()
        {
            return "GiftID,ActID,MembID,ActDate,GiftQty";
        }
    }
}
