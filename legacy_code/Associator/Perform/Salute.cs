using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Associator.Perform
{
    /*≥∆ŒΩ…Ë∂®*/
    public class Salute:BasePO
    {
        private string saluteNm = "";
        private string entryBy = "";
        private DateTime entryAt = DateTime.Now;

        public override string GetTableName()
        {
            return "Salute";
        }

        public override string GetColumnNames()
        {
            return "SaluteNm,EntryAt,EntryBy";
        }

        public override string GetInsertColumnNames()
        {
            return "SaluteNm,EntryAt,EntryBy";
        }

        public override string GetUpdateColumnNames()
        {
            return "SaluteNm,EntryAt,EntryBy";
        }

        public string SaluteNm
        {
            get { return saluteNm; }
            set { saluteNm = value; }
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
