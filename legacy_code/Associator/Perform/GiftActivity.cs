using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Associator.Perform
{
    /*赠品发放活动*/
    public class GiftActivity:BasePO
    {
        /// <summary>
        /// 活动ID
        /// </summary>
        private int actID = 0;

        /// <summary>
        /// 赠品ID
        /// </summary>
        private int giftID = 0;

        /// <summary>
        /// 活动描述
        /// </summary>
        private string actDesc = "";

        /// <summary>
        /// 开始时间
        /// </summary>
        private DateTime shopStartDate = DateTime.Now;

        /// <summary>
        /// 结束时间
        /// </summary>
        private DateTime shopEndDate = DateTime.Now;

        /// <summary>
        /// 赠品发放方式
        /// </summary>
        private int giftOption = 0;


        /// <summary>
        /// 活动ID
        /// </summary>
        public int ActID
        {
            get { return actID; }
            set { actID = value; }
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
        /// 活动描述
        /// </summary>
        public string ActDesc
        {
            get { return actDesc; }
            set { actDesc = value; }
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
        /// 赠品发放方式
        /// </summary>
        public int GiftOption
        {
            get { return giftOption; }
            set { giftOption = value; }
        }

        public static int GIFTACTIVITY_ONCE = 1;
        public static int GIFTACTIVITY_DAY = 2;


        public override string GetTableName()
        {
            return "GiftActivity";
        }

        public override string GetColumnNames()
        {
            return "ActID,GiftID,ActDesc,ShopStartDate,ShopEndDate,GiftOption";
        }

        public override string GetInsertColumnNames()
        {
            return "ActID,GiftID,ActDesc,ShopStartDate,ShopEndDate,GiftOption";
        }

        public override string GetUpdateColumnNames()
        {
            return "GiftID,ActDesc,ShopStartDate,ShopEndDate,GiftOption";
        }
    }
}
