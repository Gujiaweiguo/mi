using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    public class CurExRate:BasePO
    {
        private int curTypeID = 0;
        private int toCurTypeID = 0;
        private decimal exRate = 0;
        private int status = 0;
        private string note ="";

        public override string GetTableName()
        {
            return "CurExRate";
        }

        public override string GetColumnNames()
        {
            return "CurTypeID,ToCurTypeID,ExRate,Status,Note";
        }

        public override string GetInsertColumnNames()
        {
            return "CurTypeID,ToCurTypeID,ExRate,Status,Note";
        }

        public override string GetUpdateColumnNames()
        {
            return "CurTypeID,ToCurTypeID,ExRate,Status,Note";
        }

        public int CurTypeID
        {
            get { return curTypeID; }
            set { curTypeID = value; }
        }

        public int ToCurTypeID
        {
            get { return toCurTypeID; }
            set { toCurTypeID = value; }
        }

        public decimal ExRate
        {
            get { return exRate; }
            set { exRate = value; }
        }

        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }
    }
}
