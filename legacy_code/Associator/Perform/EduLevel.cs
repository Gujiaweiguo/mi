using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Associator.Perform
{
    /*教育水平*/
    public class EduLevel:BasePO
    {
        private string eduLevelNm = "";
        private string entryBy = "";
        private DateTime entryAt = DateTime.Now;

        public override string GetTableName()
        {
            return "EduLevel";
        }

        public override string GetColumnNames()
        {
            return "EduLevelNm,EntryAt,EntryBy";
        }

        public override string GetInsertColumnNames()
        {
            return "EduLevelNm,EntryAt,EntryBy";
        }

        public override string GetUpdateColumnNames()
        {
            return "EduLevelNm,EntryAt,EntryBy";
        }

        public string EduLevelNm
        {
            get { return eduLevelNm; }
            set { eduLevelNm = value; }
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
