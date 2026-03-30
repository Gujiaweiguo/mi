using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Sell
{
    public class ShopSellReceipt : BasePO
    {
        private int shopsellid = 0;//
        private int shopid = 0;//商铺编号
        private string shopname = "";//商铺名称
        private DateTime date = DateTime.Now;//日期
        private decimal cash = 0;//现金
        private decimal bankcard = 0;//银行卡
        private string posid = "";//POS机号
        private DateTime bizdate = DateTime.Now;//交易日期
        private string batchid = "";//批次号
        private string skuid = "";//商品编码
        private string skudesc = "";//商品描述
        private int mediamno = 0;//币制成员编码
        private string mediamdesc = "";//币制成员描述
        private decimal paidamt = 0;//实付金额
        public override String GetTableName()
        {
            return "ShopSellReceipt";
        }
        public override String GetColumnNames()
        {
            return "ShopSellID,ShopID,ShopName,Date,Cash,BankCard,PosID,BizDate,BatchID,SkuID,SkuDesc,MediaMNo,MediaMDesc,PaidAmt";
        }
        public override String GetUpdateColumnNames()
        {
            return "ShopID,ShopName,Date,Cash,BankCard,PosID,BizDate,BatchID,SkuID,SkuDesc,MediaMNo,MediaMDesc,PaidAmt";
        }
        public int ShopSellID
        {
            get { return shopsellid; }
            set { shopsellid = value; }
        }
        public int ShopID
        {
            get { return shopid; }
            set { shopid = value; }
        }
        public string ShopName
        {
            get { return shopname; }
            set { shopname = value; }
        }
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
        public decimal Cash
        {
            get { return cash; }
            set { cash = value; }
        }
        public decimal BankCard
        {
            get { return bankcard; }
            set { bankcard = value; }
        }
        public string PosID
        {
            get { return posid; }
            set { posid = value; }
        }
        public DateTime BizDate
        {
            get { return bizdate; }
            set { bizdate = value; }
        }
        public string BatchID
        {
            get { return batchid; }
            set { batchid = value; }
        }
        public string SkuID
        {
            get { return skuid; }
            set { skuid = value; }
        }
        public string SkuDesc
        {
            get { return skudesc; }
            set { skudesc = value; }
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
        public decimal PaidAmt
        {
            get { return paidamt; }
            set { paidamt = value; }
        }
    }
}