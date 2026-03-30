using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Associator.Perform
{
    /*¹ú¼®*/
    public class Nationality:BasePO
    {
        private string natNm = "";
        private string entryBy = "";
        private DateTime entryAt = DateTime.Now;

        public override string GetTableName()
        {
            return "Nationality";
        }

        public override string GetColumnNames()
        {
            return "NatNm,EntryAt,EntryBy";
        }

        public override string GetInsertColumnNames()
        {
            return "NatNm,EntryAt,EntryBy";
        }

        public override string GetUpdateColumnNames()
        {
            return "NatNm,EntryAt,EntryBy";
        }

        public string NatNm
        {
            get { return natNm; }
            set { natNm = value; }
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
