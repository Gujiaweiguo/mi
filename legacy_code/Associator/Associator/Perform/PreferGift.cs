using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Associator.Perform
{
    /*œ≤∫√‘˘∆∑*/
    public class PreferGift:BasePO
    {
        private string preferGiftNm = "";
        private string entryBy = "";
        private DateTime entryAt = DateTime.Now;

        public override string GetTableName()
        {
            return "PreferGift";
        }

        public override string GetColumnNames()
        {
            return "PreferGiftNm,EntryAt,EntryBy";
        }

        public override string GetInsertColumnNames()
        {
            return "PreferGiftNm,EntryAt,EntryBy";
        }

        public override string GetUpdateColumnNames()
        {
            return "PreferGiftNm,EntryAt,EntryBy";
        }

        public string PreferGiftNm
        {
            get { return preferGiftNm; }
            set { preferGiftNm = value; }
        }

        public DateTime EntryAt
        {
            get { return entryAt; }
            set { entryAt = value; }
        }

        public string EntryBy
        {
            get { return entryBy; }
            set { entryBy = value; }
        }
    }
}
