using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.BankCard
{
    public class BankTransSkuDet:BasePO
    {
        private int skuID = 0;
        private string bankTransID = "";
        private string bankCardID = "";
        private decimal bankAmt = 0;
        private decimal bankChgAmt = 0;
        private decimal bankNetAmt = 0;
        private int crtPayFlag = 0;


        public static int BANKTRANSSKUDET_NOT_PRODUCE = 0;     //未生成
        public static int BANKTRANSSKUDET_PRODUCE = 1;          //已生成

        public override string GetTableName()
        {
            return "BankTransSkuDet";
        }

        public override string GetColumnNames()
        {
            return "";
        }

        public override string GetInsertColumnNames()
        {
            return "BankTransID,BankCardID,BankAmt,BankChgAmt,BankNetAmt,CrtPayFlag";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }


        public int SkuID
        {
            get { return skuID; }
            set { skuID = value; }
        }

        public string BankTransID
        {
            get { return bankTransID; }
            set { bankTransID = value; }
        }

        public string BankCardID
        {
            get { return bankCardID; }
            set { bankCardID = value; }
        }

        public decimal BankAmt
        {
            get { return bankAmt; }
            set { bankAmt = value; }
        }

        public decimal BankChgAmt
        {
            get { return bankChgAmt; }
            set { bankChgAmt = value; }
        }

        public decimal BankNetAmt
        {
            get { return bankNetAmt; }
            set { bankNetAmt = value; }
        }

        public int CrtPayFlag
        {
            get { return crtPayFlag; }
            set { crtPayFlag = value; }
        }
    }
}
