using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.BankCard
{
    /*银行卡交易明细*/
    public class BankTransDet:BasePO
    {
        private int bankTransID = 0;
        private int payInID = 0;
        private int shopID = 0;
        private int bankInputBatchID = 0;
        private string bankEFTID = "";
        private string bankTransBNo = "";
        private string bankTransNo = "";
        private DateTime bankTransTime = DateTime.Now;
        private string bankCardID = "";
        private decimal bankAmt = 0;
        private decimal bankChgAmt = 0;
        private decimal bankNetAmt = 0;
        private int reconcType = 0;
        private decimal reconcID = 0;
        private int dataSource = 0;

        public static int BANKTRANSDET_RECONCTYPE_NOT_ANTITHESES = 0;//未对账
        public static int BANKTRANSDET_RECONCTYPE_SUCCEED_ANTITHESES = 1;//对账成功
        public static int BANKTRANSDET_RECONCTYPE_HANDCRAFT_ADJUST = 2;//手工调整

        public static int BANKTRANSDET_DATASOURCE_SYSTEM = 1;//系统
        public static int BANKTRANSDET_DATASOURCE_HANDCRAFT = 2;//手工

        public override string GetTableName()
        {
            return "BankTransDet";
        }

        public override string GetColumnNames()
        {
            return "BankTransID,BankEFTID,BankTransTime,BankCardID,BankAmt,BankChgAmt,BankNetAmt,ReconcType,DataSource";
        }

        public override string GetInsertColumnNames()
        {
            return "BankTransID,PayInID,BankEFTID,BankTransTime,BankCardID,BankAmt,BankChgAmt,BankNetAmt,ReconcType,DataSource,ReconcID";
        }

        public override string GetUpdateColumnNames()
        {
            return "ReconcType,ReconcID,ShopID";
        }

        public int BankTransID
        {
            get { return bankTransID; }
            set { bankTransID = value; }
        }

        public int PayInID
        {
            get { return payInID; }
            set { payInID = value; }
        }

        public int ShopID
        {
            get { return shopID; }
            set { shopID = value; }
        }

        public int BankInputBatchID
        {
            get { return bankInputBatchID; }
            set { bankInputBatchID = value; }
        }

        public string BankEFTID
        {
            get { return bankEFTID; }
            set { bankEFTID = value; }
        }

        public string BankTransBNo
        {
            get { return bankTransBNo; }
            set { bankTransBNo = value; }
        }

        public string BankTransNo
        {
            get { return bankTransNo; }
            set { bankTransNo = value; }
        }

        public DateTime BankTransTime
        {
            get { return bankTransTime; }
            set { bankTransTime = value; }
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

        public int ReconcType
        {
            get { return reconcType; }
            set { reconcType = value; }
        }

        public decimal ReconcID
        {
            get { return reconcID; }
            set { reconcID = value; }
        }

        public int DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }
    }
}
