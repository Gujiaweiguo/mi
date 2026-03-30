using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

/// <summary>
/// 编写人:hesijian
/// 编写时间：2009年6月18日
/// </summary>

namespace Associator.Perform
{
    public class BonusGp : BasePO
    {
        private int shopid = 0;
        private decimal bonusgpper = 0;
        public override String GetTableName()
        {
            return "BonusGp";
        }
        public override String GetColumnNames()
        {
            return "ShopID,BonusGpPer";
        }
        public override String GetUpdateColumnNames()
        {
            return "ShopID,BonusGpPer";
        }
        public int ShopID
        {
            get { return shopid; }
            set { shopid = value; }
        }
        public decimal BonusGpPer
        {
            get { return bonusgpper; }
            set { bonusgpper = value; }
        }
    }
}