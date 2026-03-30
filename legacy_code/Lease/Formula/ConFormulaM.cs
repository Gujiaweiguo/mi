using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Lease.Formula
{
    /// <summary>
    /// 结算公式保底
    /// </summary>
    public class ConFormulaM:BasePO
    {
        private int conFormulaMID;
        private int formulaID;
        private decimal salesTo;
        private decimal minSum;

        public int ConFormulaMID
        {
            get { return conFormulaMID; }
            set { conFormulaMID = value; }
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

        public override string GetTableName()
        {
            return "ConFormulaM";
        }

        public override string GetColumnNames()
        {
            return "ConFormulaMID,FormulaID,SalesTo,MinSum";
        }

        public override string GetInsertColumnNames()
        {
            return "ConFormulaMID,FormulaID,SalesTo,MinSum";
        }

        public override string GetUpdateColumnNames()
        {
            return "SalesTo,MinSum";
        }
    }
}
