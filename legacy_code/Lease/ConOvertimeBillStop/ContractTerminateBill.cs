using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Lease.ConOvertimeBillStop
{
    public class ContractTerminateBill:BasePO
    {
        private DateTime conEndDate = DateTime.Now;

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
            return "ConEndDate";
        }

        public DateTime ConEndDate
        {
            get { return conEndDate; }
            set { conEndDate = value; }
        }
    }
}
