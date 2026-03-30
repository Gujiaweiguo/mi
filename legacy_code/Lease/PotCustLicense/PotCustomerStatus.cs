using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.PotCustLicense
{
    public class PotCustomerStatus:BasePO
    {
        private int customerStatus = 0;

        public int CustomerStatus
        {
            get { return customerStatus; }
            set { customerStatus = value; }
        }
        public override string GetTableName()
        {
            return "PotCustomer";
        }

        public override string GetColumnNames()
        {
            return "CustomerStatus";
        }

        public override string GetUpdateColumnNames()
        {
            return "CustomerStatus";
        }
    }
}
