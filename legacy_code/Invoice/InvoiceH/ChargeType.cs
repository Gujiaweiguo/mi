using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    public class ChargeType:BasePO
    {
        private int chargeTypeID = 0;
        private string chargeTypeCode = "";
        private string chargeTypeName = "";
        private int chargeClass = 0;
        private int isChargeCross = 0;
        private string note = "";


        public override string GetTableName()
        {
            return "ChargeType";
        }

        public override string GetColumnNames()
        {
            return "ChargeTypeID,ChargeTypeCode,ChargeTypeName,ChargeClass,IsChargeCross,Note";
        }

        public override string GetInsertColumnNames()
        {
            return "";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public int ChargeTypeID
        {
            get { return chargeTypeID; }
            set { chargeTypeID = value; }
        }

        public string ChargeTypeCode
        {
            get { return chargeTypeCode; }
            set { chargeTypeCode = value; }
        }

        public string ChargeTypeName
        {
            get { return chargeTypeName; }
            set { chargeTypeName = value; }
        }

        public int ChargeClass
        {
            get { return chargeClass; }
            set { chargeClass = value; }
        }

        public int IsChargeCross
        {
            get { return isChargeCross; }
            set { isChargeCross = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }
    }
}
