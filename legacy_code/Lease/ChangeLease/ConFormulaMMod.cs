using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.ChangeLease
{
    public class ConFormulaMMod:BasePO
    {
        private int conFormulaMModID = 0;
        private int formulaID = 0;
        private decimal salesTo = 0;
        private decimal minSum = 0;

        public override string GetTableName()
        {
            return "ConFormulaMMod";
        }

        public override string GetColumnNames()
        {
            return "ConFormulaMModID,FormulaID,SalesTo,MinSum";
        }

        public override string GetInsertColumnNames()
        {
            return "ConFormulaMModID,FormulaID,SalesTo,MinSum";
        }

        public override string GetUpdateColumnNames()
        {
            return "FormulaID,SalesTo,MinSum";
        }

        public int ConFormulaMModID
        {
            get { return conFormulaMModID; }
            set { conFormulaMModID = value; }
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
