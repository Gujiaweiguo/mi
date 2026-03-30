using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    public class SurBalDel:BasePO
    {
        private int surBalDelID = 0;
        private int surID = 0;
        private int surBalID = 0;
        private int surCurID = 0;
        private decimal surExRate = 0;
        private decimal surBalAmt = 0;
        private decimal surBalAmtL = 0;
        private string note = "";

        public override string GetTableName()
        {
            return "SurBalDel";
        }

        public override string GetColumnNames()
        {
            return "SurBalDelID,SurID,SurBalID,SurCurID,SurExRate,SurBalAmt,SurBalAmtL";
        }

        public override string GetInsertColumnNames()
        {
            return "SurBalDelID,SurID,SurBalID,SurCurID,SurExRate,SurBalAmt,SurBalAmtL";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public int SurBalDelID
        {
            get { return surBalDelID; }
            set { surBalDelID = value; }
        }

        public int SurID
        {
            get { return surID; }
            set { surID = value; }
        }

        public int SurBalID
        {
            get { return surBalID; }
            set { surBalID = value; }
        }

        public int SurCurID
        {
            get { return surCurID; }
            set { surCurID = value; }
        }

        public decimal SurExRate
        {
            get { return surExRate; }
            set { surExRate = value; }
        }

        public decimal SurBalAmt
        {
            get { return surBalAmt; }
            set { surBalAmt = value; }
        }

        public decimal SurBalAmtL
        {
            get { return surBalAmtL; }
            set { surBalAmtL = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }
    }
}
