using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    public class DepBalDel:BasePO
    {
        private int depBalDetID = 0;
        private int invPayDetID = 0;
        private int depBalID = 0;
        private int depBalCurID = 0;
        private decimal depBalExRate = 0;
        private decimal depBalAmt = 0;
        private decimal depBalAmtL = 0;

        public override string GetTableName()
        {
            return "DepBalDel";
        }

        public override string GetColumnNames()
        {
            return "DepBalDetID,InvPayDetID,DepBalID,DepBalCurID,DepBalExRate,DepBalAmt,DepBalAmtL";
        }

        public override string GetInsertColumnNames()
        {
            return "DepBalDetID,InvPayDetID,DepBalID,DepBalCurID,DepBalExRate,DepBalAmt,DepBalAmtL";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public int DepBalDetID
        {
            get { return depBalDetID; }
            set { depBalDetID = value; }
        }

        public int InvPayDetID
        {
            get { return invPayDetID; }
            set { invPayDetID = value; }
        }

        public int DepBalID
        {
            get { return depBalID; }
            set { depBalID = value; }
        }

        public int DepBalCurID
        {
            get { return depBalCurID; }
            set { depBalCurID = value; }
        }

        public decimal DepBalExRate
        {
            get { return depBalExRate; }
            set { depBalExRate = value; }
        }

        public decimal DepBalAmt
        {
            get { return depBalAmt; }
            set { depBalAmt = value; }
        }

        public decimal DepBalAmtL
        {
            get { return depBalAmtL; }
            set { depBalAmtL = value; }
        }
    }
}
