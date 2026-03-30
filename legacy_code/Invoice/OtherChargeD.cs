using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Invoice
{
    public class OtherChargeD:BasePO
    {
        private int otherChargeDID = 0;
        private int otherChargeHID = 0;
        private int chargeTypeID = 0;
        private string chgName = "";
        private string refID = "";
        private DateTime startDate = DateTime.Now;
        private DateTime endDate = DateTime.Now;
        private decimal chgAmt = 0;
        private string note = "";
        private DateTime chgPeriod = DateTime.Now;

        public int OtherChargeDID
        {
            get { return otherChargeDID; }
            set { otherChargeDID = value; }
        }

        public int OtherChargeHID
        {
            get { return otherChargeHID; }
            set { otherChargeHID = value; }
        }

        public int ChargeTypeID
        {
            get { return chargeTypeID; }
            set { chargeTypeID = value; }
        }

        public string ChgName
        {
            get { return chgName; }
            set { chgName = value; }
        }

        public string RefID
        {
            get { return refID; }
            set { refID = value; }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        public decimal ChgAmt
        {
            get { return chgAmt; }
            set { chgAmt = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public DateTime ChgPeriod
        {
            get { return chgPeriod; }
            set { chgPeriod = value; }
        }

        public override string GetTableName()
        {
            return "OtherChargeD";
        }

        public override string GetColumnNames()
        {
            return "OtherChargeDID,OtherChargeHID,ChargeTypeID,ChgName,ChgPeriod,StartDate,EndDate,RefID,ChgAmt,Note";
        }

        public override string GetInsertColumnNames()
        {
            return "OtherChargeDID,OtherChargeHID,ChargeTypeID,ChgName,ChgPeriod,StartDate,EndDate,RefID,ChgAmt,Note";
        }

        public override string GetUpdateColumnNames()
        {
            return "OtherChargeHID,ChargeTypeID,ChgName,ChgPeriod,StartDate,EndDate,RefID,ChgAmt,Note";
        }
    }
}
