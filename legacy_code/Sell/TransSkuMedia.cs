using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Sell
{
    /// <summary>
    /// 交易金额流水
    /// </summary>
    public class TransSkuMedia:BasePO
    {
        private int transSkuMedID = 0;  //交易ID
        private string transID = "";  //原交易号
        private int shopID = 0;         //商铺ID
        private string shopName = ""; //商铺名称
        private string posID = "";   //POS机ID
        private int userID = 0;         //收银员ID
        private DateTime bizDate = DateTime.Now;  //记帐日期
        private DateTime transTime = DateTime.Now; //交易时间
        private string receiptID = "";  //POS交易号
        private string cardID = "";     //卡号
        private string eFTID = "";      //完整银行POS终端号
        private int mediaMNo = 0;         //币制成员编码
        private string mediaMDesc = ""; //币制成员描述
        private string skuID = "";            //商品ID
        private string skuDesc = "";    //商品描述
        private decimal paidAmt = 0;      //实付金额
        private decimal commRate = 0;     //手续费率
        private decimal commChg = 0;      //手续费
        private decimal netAmt = 0;       //消费净额
        private int paymentID = 0;        //总收银ID
        private int paymentStatus = 0;    //是否已做总收
        private int transType = 1;        //交易类型
        private int dataSource = 1;       //数据来源
        private string batchID = "";     //批次号
        private int stroeID = 0; //商业项目id
        private int mainlocationID = 0; //大区


        public int TransSkuMedID
        {
            get { return transSkuMedID; }
            set { transSkuMedID = value; }
        }

        public string TransID
        {
            get { return transID; }
            set { transID = value; }
        }

        public int ShopID
        {
            get { return shopID; }
            set { shopID = value; }
        }

        public string ShopName
        {
            get { return shopName; }
            set { shopName = value; }
        }

        public string PosID
        {
            get { return posID; }
            set { posID = value; }
        }

        public int UserID
        {
            get { return userID; }
            set { userID = value; }
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

        public string ReceiptID
        {
            get { return receiptID; }
            set { receiptID = value; }
        }

        public string CardID
        {
            get { return cardID; }
            set { cardID = value; }
        }

        public string EFTID
        {
            get { return eFTID; }
            set { eFTID = value; }
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

        public string SkuID
        {
            get { return skuID; }
            set { skuID = value; }
        }

        public string SkuDesc
        {
            get { return skuDesc; }
            set { skuDesc = value; }
        }

        public decimal PaidAmt
        {
            get { return paidAmt; }
            set { paidAmt = value; }
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

        public int PaymentID
        {
            get { return paymentID; }
            set { paymentID = value; }
        }

        public int PaymentStatus
        {
            get { return paymentStatus; }
            set { paymentStatus = value; }
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

        public string BatchID
        {
            get { return batchID; }
            set { batchID = value; }
        }

        public int StoreID
        {
            get { return stroeID; }
            set { stroeID = value; }
        }

        public int MainLocationID
        {
            get { return mainlocationID; }
            set { mainlocationID = value; }
        }

        public static int TRANSTYPE_CONSUME = 1;  //消费交易
        public static int TRANSTYPE_BACK = 2;  //退货交易

        public static int DATASOURCE_POS = 1;  //POS系统
        public static int DATASOURCE_FILE = 2; //文件导入
        public static int DATASOURCE_WORK = 3; //手工录入

        public override string GetTableName()
        {
            return "TransSkuMedia";
        }

        public override string GetColumnNames()
        {
            return "TransSkuMedID,TransID,ShopID,ShopName,PosID,UserID,BizDate,TransTime,ReceiptID,CardID,EFTID,MediaMNo,MediaMDesc,SkuID,SkuDesc," +
                    "PaidAmt,CommRate,CommChg,NetAmt,PaymentID,PaymentStatus,TransType,DataSource,BatchID,StoreID,MainLocationID";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }
        public override string GetInsertColumnNames()
        {
            return "TransID,ShopID,ShopName,PosID,UserID,BizDate,TransTime,ReceiptID,CardID,EFTID,MediaMNo,MediaMDesc,SkuID,SkuDesc," +
                   "PaidAmt,CommRate,CommChg,NetAmt,PaymentID,PaymentStatus,TransType,DataSource,BatchID,StoreID,MainLocationID";
        }
    }
}
