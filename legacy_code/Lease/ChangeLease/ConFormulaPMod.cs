using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.ChangeLease
{
    public class ConFormulaPMod:BasePO
    {
        private int conFormulaPModID = 0;
        private int formulaID = 0;
        private decimal salesTo = 0;
        private decimal pcent = 0;

        public override string GetTableName()
        {
            return "ConFormulaPMod";
        }

        public override string GetColumnNames()
        {
            return "ConFormulaPModID,FormulaID,SalesTo,Pcent";
        }

        public override string GetInsertColumnNames()
        {
            return "ConFormulaPModID,FormulaID,SalesTo,Pcent";
        }

        public override string GetUpdateColumnNames()
        {
            return "FormulaID,SalesTo,Pcent";
        }

        public int ConFormulaPModID
        {
            get { return conFormulaPModID; }
            set { conFormulaPModID = value; }
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

    }
}
