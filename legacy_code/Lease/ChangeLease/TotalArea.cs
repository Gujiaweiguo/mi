using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.ChangeLease
{
    public class TotalArea:BasePO
    {
        private decimal rentArea = 0;

        public decimal RentArea
        {
            get { return rentArea; }
            set { rentArea = value; }
        }

        public override String GetTableName()
        {
            return "ConShop";
        }

        public override String GetColumnNames()
        {
            return "RentArea";
        }

        public override string GetQuerySql()
        {
            return "select sum(RentArea) as RentArea from ConShop";
        }
        public override String GetInsertColumnNames()
        {
            return "";
        }

        public override String GetUpdateColumnNames()
        {
            return "";
        }
    }
}
