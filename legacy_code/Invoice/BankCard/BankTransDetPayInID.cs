using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.BankCard
{
    public class BankTransDetPayInID:BasePO
    {
        private int payInID = 0;

        public int PayInID
        {
            get { return payInID; }
            set { payInID = value; }
        }

        public override string GetTableName()
        {
            return "BankTransDet";
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
            return "PayInID";
        }
    }
}
