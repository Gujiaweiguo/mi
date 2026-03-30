using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Lease.Formula
{
    /// <summary>
    /// 结算公式抽成
    /// </summary>
    public class ConFormulaP:BasePO
    {
        private int conFormulaPID;
        private int formulaID;
        private decimal salesTo;
        private decimal pcent;

        public int ConFormulaPID
        {
            get { return conFormulaPID; }
            set { conFormulaPID = value; }
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

        public decimal Pcent
        {
            get { return pcent; }
            set { pcent = value; }
        }

        public override string GetTableName()
        {
            return "ConFormulaP";
        }

        public override string GetColumnNames()
        {
            return "ConFormulaPID,FormulaID,SalesTo,Pcent";
        }

        public override string  GetQuerySql()
        {
            return "select ConFormulaPID,FormulaID,SalesTo,Pcent*100 as Pcent from  ConFormulaP";
        }

        public override string GetInsertColumnNames()
        {
            return "ConFormulaPID,FormulaID,SalesTo,Pcent";
        }

        public override string GetUpdateColumnNames()
        {
            return "FormulaID,SalesTo,Pcent";
        }
    }
}
