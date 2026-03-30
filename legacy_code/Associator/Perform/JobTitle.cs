using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Associator.Perform
{
    /*职位设定*/
    public class JobTitle:BasePO
    {
        private string jobTitleNm = "";
        private string entryBy = "";
        private DateTime entryAt = DateTime.Now;

        public override string GetTableName()
        {
            return "JobTitle";
        }

        public override string GetColumnNames()
        {
            return "JobTitleNm,EntryAt,EntryBy";
        }

        public override string GetInsertColumnNames()
        {
            return "JobTitleNm,EntryAt,EntryBy";
        }

        public override string GetUpdateColumnNames()
        {
            return "JobTitleNm,EntryAt,EntryBy";
        }

        public string JobTitleNm
        {
            get { return jobTitleNm; }
            set { jobTitleNm = value; }
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
