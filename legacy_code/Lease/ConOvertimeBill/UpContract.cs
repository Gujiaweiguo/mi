using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Lease.ConOvertimeBill
{
    public class UpContract:BasePO
    {
        private DateTime conEndDate = DateTime.Now;
        private string additionalItem = "";
        private string eConURL = "";
        private int refID = 0;
        private string note = "";

        public override string GetTableName()
        {
            return "Contract";
        }

        public override string GetColumnNames()
        {
            return "";
        }

        public override string GetInsertColumnNames()
        {
            return "";
        }

        public override string GetUpdateColumnNames()
        {
            return "ConEndDate,AdditionalItem,EConURL,RefID,Note";
        }

        public DateTime ConEndDate
        {
            get { return conEndDate; }
            set { conEndDate = value; }
        }

        public string AdditionalItem
        {
            get { return additionalItem; }
            set { additionalItem = value; }
        }

        public string EConURL
        {
            get { return eConURL; }
            set { eConURL = value; }
        }

        public int RefID
        {
            get { return refID; }
            set { refID = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }
    }
}
