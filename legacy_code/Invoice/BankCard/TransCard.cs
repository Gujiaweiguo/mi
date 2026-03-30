using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.BankCard
{
    /*银行卡交易*/
    public class TransCard:BasePO
    {
        private int transCrdID = 0;
        private string transID = "";
        private string eFTID = "";
        private string cardID = "";
        private DateTime bizDate = DateTime.Now;
        private DateTime transTime = DateTime.Now;
        private int mediaMNo = 0;
        private string mediaMDesc = "";
        private decimal amt = 0;
        private decimal commRate = 0;
        private decimal commChg = 0;
        private decimal netAmt = 0;
        private decimal reconcID = 0;
        private int reconcType = 0;
        private int transType = 0;
        private int dataSource = 0;
        private string mediaType = "";

        /*是否已对账*/
        public static int TRANSCARD_TRANSTYPE_NO = 0;//未对账
        public static int TRANSCARD_TRANSTYPE_YES = 1;//已对账

        /*交易类型*/
        public static int TRANSCARD_TRANSTYPE_CONSUMED = 1;//消费
        public static int TRANSCARD_TRANSTYPE_UNTREAD = 2;//退货

        /*数据来源*/
        public static int TRANSCARD_DATASOURCE_POS = 1;//POS
        public static int TRANSCARD_DATASOURCE_FILE = 2;//文件
        public static int TRANSCARD_DATASOURCE_HANDCRAFT = 3;//手工

        public static string MEDIATYPE_PLUS = "T";//正数
        public static string MEDIATYPE_NEGATIVE = "C";//负

        public override string GetTableName()
        {
            return "TransCard";
        }

        public override string GetColumnNames()
        {
            return "TransCrdID,TransID,EFTID,CardID,BizDate,TransTime,MediaMNo,MediaMDesc,MediaType,Amt,CommRate,CommChg,NetAmt,ReconcID,ReconcType,TransType,DataSource";
        }

        public override string GetInsertColumnNames()
        {
            return "";
        }

        public override string GetUpdateColumnNames()
        {
            return "ReconcID,ReconcType";
        }

        public int TransCrdID
        {
            get { return transCrdID; }
            set { transCrdID = value; }
        }

        public string TransID
        {
            get { return transID; }
            set { transID = value; }
        }

        public string EFTID
        {
            get { return eFTID; }
            set { eFTID = value; }
        }

        public string CardID
        {
            get { return cardID; }
            set { cardID = value; }
        }

        public DateTime BizDate
        {
            get { return bizDate; }
            set { bizDate = value; }
        }

        public DateTime TransTime
        {
            get { return transTime; }
            set { transTime = value; }
        }

        public int MediaMNo
        {
            get { return mediaMNo; }
            set { mediaMNo = value; }
        }

        public string MediaMDesc
        {
            get { return mediaMDesc; }
            set { mediaMDesc = value; }
        }

        public decimal Amt
        {
            get { return amt; }
            set { amt = value; }
        }

        public decimal CommRate
        {
            get { return commRate; }
            set { commRate = value; }
        }

        public decimal CommChg
        {
            get { return commChg; }
            set { commChg = value; }
        }

        public decimal NetAmt
        {
            get { return netAmt; }
            set { netAmt = value; }
        }

        public decimal ReconcID
        {
            get { return reconcID; }
            set { reconcID = value; }
        }

        public int ReconcType
        {
            get { return reconcType; }
            set { reconcType = value; }
        }

        public int TransType
        {
            get { return transType; }
            set { transType = value; }
        }

        public int DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        public string MediaType
        {
            get { return mediaType; }
            set { mediaType = value; }
        }
    }
}
