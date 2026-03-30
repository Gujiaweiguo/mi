using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Associator.Perform
{
    /*Ãñ×å*/
    public class Race:BasePO
    {
        private string raceNm = "";
        private string entryBy = "";
        private DateTime entryAt = DateTime.Now;

        public override string GetTableName()
        {
            return "Race";
        }

        public override string GetColumnNames()
        {
            return "RaceNm,EntryAt,EntryBy";
        }

        public override string GetInsertColumnNames()
        {
            return "RaceNm,EntryAt,EntryBy";
        }

        public override string GetUpdateColumnNames()
        {
            return "RaceNm,EntryAt,EntryBy";
        }

        public string RaceNm
        {
            get { return raceNm; }
            set { raceNm = value; }
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
