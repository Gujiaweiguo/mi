using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Sell
{
    /// <summary>
    /// 交易付款方式表
    /// </summary>
    public class TransMedia : BasePO
    {
        private decimal transid = 0;
        private string mediatype = "";
        private int mediano = 0;
        private int mediamno = 0;
        private string mediamdesc = "";
        private string currcd = "";
        private decimal excrate = 0;
        private decimal amountt = 0;
        private decimal localamt = 0;
        private string remark1 = "";
        private string remark2 = "";
        private string remark3 = "";
        private decimal commrate = 0;
        private decimal commchg = 0;
        private string paymentstatus = "";
        private decimal paymentid = 0;

        public static int TRANSTYPE_CONSUME = 1;  //消费交易
        public static int TRANSTYPE_BACK = 2;  //退货交易

        public static int DATASOURCE_POS = 1;  //POS系统
        public static int DATASOURCE_FILE = 2; //文件导入
        public static int DATASOURCE_WORK = 3; //手工录入
        public override String GetTableName()
        {
            return "TransMedia";
        }
        public override String GetColumnNames()
        {
            return "TransId,MediaType,MediaNo,MediaMNo,MediaMDesc,CurrCd,ExcRate,Amountt,LocalAmt,Remark1,Remark2,Remark3,CommRate,CommChg,PaymentStatus,PaymentID";
        }
        public override String GetUpdateColumnNames()
        {
            return "TransId,MediaType,MediaNo,MediaMNo,MediaMDesc,CurrCd,ExcRate,Amountt,LocalAmt,Remark1,Remark2,Remark3,CommRate,CommChg,PaymentStatus,PaymentID";
        }
        public override string GetInsertColumnNames()
        {
            return "TransId,MediaType,MediaNo,MediaMNo,MediaMDesc,CurrCd,ExcRate,Amountt,LocalAmt,Remark1,Remark2,Remark3,CommRate,CommChg,PaymentStatus,PaymentID";
        }
        public decimal TransId
        {
            get { return transid; }
            set { transid = value; }
        }
        public string MediaType
        {
            get { return mediatype; }
            set { mediatype = value; }
        }
        public int MediaNo
        {
            get { return mediano; }
            set { mediano = value; }
        }
        public int MediaMNo
        {
            get { return mediamno; }
            set { mediamno = value; }
        }
        public string MediaMDesc
        {
            get { return mediamdesc; }
            set { mediamdesc = value; }
        }
        public string CurrCd
        {
            get { return currcd; }
            set { currcd = value; }
        }
        public decimal ExcRate
        {
            get { return excrate; }
            set { excrate = value; }
        }
        public decimal Amountt
        {
            get { return amountt; }
            set { amountt = value; }
        }
        public decimal LocalAmt
        {
            get { return localamt; }
            set { localamt = value; }
        }
        public string Remark1
        {
            get { return remark1; }
            set { remark1 = value; }
        }
        public string Remark2
        {
            get { return remark2; }
            set { remark2 = value; }
        }
        public string Remark3
        {
            get { return remark3; }
            set { remark3 = value; }
        }
        public decimal CommRate
        {
            get { return commrate; }
            set { commrate = value; }
        }
        public decimal CommChg
        {
            get { return commchg; }
            set { commchg = value; }
        }
        public string PaymentStatus
        {
            get { return paymentstatus; }
            set { paymentstatus = value; }
        }
        public decimal PaymentID
        {
            get { return paymentid; }
            set { paymentid = value; }
        }

    }
}
