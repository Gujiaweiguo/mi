using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.ConOvertimeBill
{
    public class ConFormulaPOvtm:BasePO
    {
        private int conFormulaPOvtmID = 0;
        private int formulaID = 0;
        private decimal salesTo = 0;
        private decimal pcent = 0;

        public override string GetTableName()
        {
            return "ConFormulaPOvtm";
        }

        public override string GetColumnNames()
        {
            return "ConFormulaPOvtmID,FormulaID,SalesTo,Pcent";
        }

        public override string GetInsertColumnNames()
        {
            return "ConFormulaPOvtmID,FormulaID,SalesTo,Pcent";
        }

        public override string GetUpdateColumnNames()
        {
            return "FormulaID,SalesTo,Pcent";
        }

        public int ConFormulaPOvtmID
        {
            get { return conFormulaPOvtmID; }
            set { conFormulaPOvtmID = value; }
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
