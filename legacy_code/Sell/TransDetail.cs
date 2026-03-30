using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Sell
{
    /// <summary>
    /// 交易明细表
    /// </summary>
    public class TransDetail :BasePO 
    {
        private string transId = "";
        private string skuCd = "";
        private string pluCd = "";
        private string deptCd = "";
        private string catgCd = "";
        private string classCd = "";
        private string taxCd = "";
        private decimal qty = 0;
        private decimal orgPrice = 0;
        private decimal newPrice = 0;
        private string skuFlag = "";
        private string discCd = "";
        private decimal itemDisc = 0;
        private decimal allocDisc = 0;
        private decimal tax = 0;
        private decimal netAmt = 0;
        private string payComm = "";


        public string TransId
        {
            get { return transId; }
            set { transId = value; }
        }

        public string SkuCd
        {
            get { return skuCd; }
            set { skuCd = value; }
        }

        public string PluCd
        {
            get { return pluCd; }
            set { pluCd = value; }
        }

        public string DeptCd
        {
            get { return deptCd; }
            set { deptCd = value; }
        }

        public string CatgCd
        {
            get { return catgCd; }
            set { catgCd = value; }
        }

        public string ClassCd
        {
            get { return classCd; }
            set { classCd = value; }
        }

        public string TaxCd
        {
            get { return taxCd; }
            set { taxCd = value; }
        }

        public decimal Qty
        {
            get { return qty; }
            set { qty = value; }
        }

        public decimal OrgPrice
        {
            get { return orgPrice; }
            set { orgPrice = value; }
        }

        public decimal NewPrice
        {
            get { return newPrice; }
            set { newPrice = value; }
        }

        public string SkuFlag
        {
            get { return skuFlag; }
            set { skuFlag = value; }
        }

        public string DiscCd
        {
            get { return discCd; }
            set { discCd = value; }
        }

        public decimal ItemDisc
        {
            get { return itemDisc; }
            set { itemDisc = value; }
        }

        public decimal AllocDisc
        {
            get { return allocDisc; }
            set { allocDisc = value; }
        }

        public decimal Tax
        {
            get { return tax; }
            set { tax = value; }
        }

        public decimal NetAmt
        {
            get { return netAmt; }
            set { netAmt = value; }
        }

        public string PayComm
        {
            get { return payComm; }
            set { payComm = value; }
        }

 

        public static int TRANSTYPE_CONSUME = 1;  //消费交易
        public static int TRANSTYPE_BACK = 2;  //退货交易

        public static int DATASOURCE_POS = 1;  //POS系统
        public static int DATASOURCE_FILE = 2; //文件导入
        public static int DATASOURCE_WORK = 3; //手工录入

        public override string GetTableName()
        {
            return "TransDetail";
        }

        public override string GetColumnNames()
        {
            return "TransId,SkuCd,PluCd,DeptCd,CatgCd,ClassCd,TaxCd,Qty,OrgPrice,NewPrice,SkuFlag,DiscCd,ItemDisc,AllocDisc,Tax,NetAmt,PayComm";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }
        public override string GetInsertColumnNames()
        {
            return "TransId,SkuCd,PluCd,DeptCd,CatgCd,ClassCd,TaxCd,Qty,OrgPrice,NewPrice,SkuFlag,DiscCd,ItemDisc,AllocDisc,Tax,NetAmt,PayComm";
        }
    }
}

