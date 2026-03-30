using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.ConOvertimeBill
{
    public class ConFormulaMOvtm:BasePO
    {
        private int conFormulaMOvtmID = 0;
        private int formulaID = 0;
        private decimal salesTo = 0;
        private decimal minSum = 0;

        public override string GetTableName()
        {
            return "ConFormulaMOvtm";
        }

        public override string GetColumnNames()
        {
            return "ConFormulaMOvtmID,FormulaID,SalesTo,MinSum";
        }

        public override string GetInsertColumnNames()
        {
            return "ConFormulaMOvtmID,FormulaID,SalesTo,MinSum";
        }

        public override string GetUpdateColumnNames()
        {
            return "FormulaID,SalesTo,MinSum";
        }

        public int ConFormulaMOvtmID
        {
            get { return conFormulaMOvtmID; }
            set { conFormulaMOvtmID = value; }
        }

        public int FormulaID
        {
            get { return formulaID; }
            set { formulaID = value; }
        }

        public decimal SalesTo
        {
            get { return salesTo; }
            set { salesTo = value; }
        }

        public decimal MinSum
        {
            get { return minSum; }
            set { minSum = value; }
        }
    }
}
