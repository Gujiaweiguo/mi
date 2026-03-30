using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Associator.Perform
{
    /*职业设定*/
    public class Biz:BasePO
    {
        private string bizNm = "";
        private string entryBy = "";
        private DateTime entryAt = DateTime.Now;

        public override string GetTableName()
        {
            return "Biz";
        }

        public override string GetColumnNames()
        {
            return "BizNm,EntryAt,EntryBy";
        }

        public override string GetInsertColumnNames()
        {
            return "BizNm,EntryAt,EntryBy";
        }

        public override string GetUpdateColumnNames()
        {
            return "BizNm,EntryAt,EntryBy";
        }

        public string BizNm
        {
            get { return bizNm; }
            set { bizNm = value; }
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
