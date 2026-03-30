using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Associator.Perform
{
    /*œ≤∫√…Ã∆∑*/
    public class PreferMer:BasePO
    {
        private string preferMerNm = "";
        private string entryBy = "";
        private DateTime entryAt = DateTime.Now;

        public override string GetTableName()
        {
            return "PreferMer";
        }

        public override string GetColumnNames()
        {
            return "PreferMerNm,EntryAt,EntryBy";
        }

        public override string GetInsertColumnNames()
        {
            return "PreferMerNm,EntryAt,EntryBy";
        }

        public override string GetUpdateColumnNames()
        {
            return "PreferMerNm,EntryAt,EntryBy";
        }

        public string PreferMerNm
        {
            get { return preferMerNm; }
            set { preferMerNm = value; }
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
