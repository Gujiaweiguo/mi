using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Associator.Perform
{
    /*–À»§∞Æ∫√*/
    public class Recreation:BasePO
    {
        private string recreationNm = "";
        private string entryBy = "";
        private DateTime entryAt = DateTime.Now;

        public override string GetTableName()
        {
            return "Recreation";
        }

        public override string GetColumnNames()
        {
            return "RecreationNm,EntryAt,EntryBy";
        }

        public override string GetInsertColumnNames()
        {
            return "RecreationNm,EntryAt,EntryBy";
        }

        public override string GetUpdateColumnNames()
        {
            return "RecreationNm,EntryAt,EntryBy";
        }

        public string RecreationNm
        {
            get { return recreationNm; }
            set { recreationNm = value; }
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
