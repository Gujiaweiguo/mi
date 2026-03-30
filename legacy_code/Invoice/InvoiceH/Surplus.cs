using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    public class Surplus:BasePO
    {
        private int surID = 0;
        private int custID = 0;
        private int billID = 0;
        private int billType = 0;
        private int surCurTypeID = 0;
        private decimal surExRate = 0;
        private decimal surAmt = 0;
        private decimal surAmtL = 0;
        private DateTime surDate = DateTime.Now;
        private int invPayType = 0;
        private decimal payOutAmtSum = 0;
        private string note = "";


        public override string GetTableName()
        {
            return "Surplus";
        }

        public override string GetColumnNames()
        {
            return "SurID,CustID,BillID,BillType,SurCurTypeID,SurExRate,SurAmt,SurAmtL,SurDate,InvPayType,PayOutAmtSum,Note";
        }

        public override string GetInsertColumnNames()
        {
            return "SurID,CustID,BillID,BillType,SurCurTypeID,SurExRate,SurAmt,SurAmtL,SurDate,InvPayType,PayOutAmtSum,Note";
        }

        public override string GetUpdateColumnNames()
        {
            return "PayOutAmtSum";
        }


        public int SurID
        {
            get { return surID; }
            set { surID = value; }
        }

        public int CustID
        {
            get { return custID; }
            set { custID = value; }
        }

        public int BillID
        {
            get { return billID; }
            set { billID = value; }
        }

        public int BillType
        {
            get { return billType; }
            set { billType = value; }
        }

        public int SurCurTypeID
        {
            get { return surCurTypeID; }
            set { surCurTypeID = value; }
        }

        public decimal SurExRate
        {
            get { return surExRate; }
            set { surExRate = value; }
        }

        public decimal SurAmt
        {
            get { return surAmt; }
            set { surAmt = value; }
        }

        public decimal SurAmtL
        {
            get { return surAmtL; }
            set { surAmtL = value; }
        }

        public DateTime SurDate
        {
            get { return surDate; }
            set { surDate = value; }
        }

        public int InvPayType
        {
            get { return invPayType; }
            set { invPayType = value; }
        }

        public decimal PayOutAmtSum
        {
            get { return payOutAmtSum; }
            set { payOutAmtSum = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }
    }
}
