using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Associator.Perform
{
    /*»éÒö×´¿ö*/
    public class Mstatus:BasePO
    {
        private string mstatusNm = "";
        private string entryBy = "";
        private DateTime entryAt = DateTime.Now;

        public override string GetTableName()
        {
            return "Mstatus";
        }

        public override string GetColumnNames()
        {
            return "MstatusNm,EntryAt,EntryBy";
        }

        public override string GetInsertColumnNames()
        {
            return "MstatusNm,EntryAt,EntryBy";
        }

        public override string GetUpdateColumnNames()
        {
            return "MstatusNm,EntryAt,EntryBy";
        }

        public string MstatusNm
        {
            get { return mstatusNm; }
            set { mstatusNm = value; }
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
