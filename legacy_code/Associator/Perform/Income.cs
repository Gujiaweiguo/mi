using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Associator.Perform
{
    /*收入设定*/
    public class Income:BasePO
    {
        private string incomeId = "";
        private int incomeLower = 0;
        private int incomeUpper = 0;
        private string entryBy = "";
        private DateTime entryAt = DateTime.Now;

        public override string GetTableName()
        {
            return "Income";
        }

        public override string GetColumnNames()
        {
            return "IncomeId,IncomeLower,IncomeUpper,EntryAt,EntryBy";
        }

        public override string GetInsertColumnNames()
        {
            return "IncomeId,IncomeLower,IncomeUpper,EntryAt,EntryBy";
        }

        public override string GetUpdateColumnNames()
        {
            return "IncomeId,IncomeLower,IncomeUpper,EntryAt,EntryBy";
        }

        public string IncomeId
        {
            get { return incomeId; }
            set { incomeId = value; }
        }

        public int IncomeLower
        {
            get { return incomeLower; }
            set { incomeLower = value; }
        }

        public int IncomeUpper
        {
            get { return incomeUpper; }
            set { incomeUpper = value; }
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
