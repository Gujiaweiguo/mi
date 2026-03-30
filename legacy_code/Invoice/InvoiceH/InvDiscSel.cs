using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    /*Ω·À„”≈ª›*/
    public class InvDiscSel:BasePO
    {
        private int invDetailID = 0;
        private string custCode = "";
        private string custName = "";
        private string contractCode = "";
        private string shopName = "";
        private string invCode = "";
        private DateTime invDate = DateTime.Now;
        private decimal invPayAmt = 0;
        private decimal invPaidAmt = 0;
        private decimal invAdjAmt = 0;
        private decimal invDiscAmt = 0;
        private string tradeName = "";


        public override string GetTableName()
        {
            return "";
        }

        public override string GetColumnNames()
        {
            return "InvDetailID,CustCode,CustName,ContractCode,ShopName,InvCode,InvDate,InvPayAmt,InvPaidAmt,InvAdjAmt,InvDiscAmt,TradeName";
        }

        public override string GetInsertColumnNames()
        {
            return "";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public override string GetQuerySql()
        {
            return "select InvDetailID,CustCode,c.CustName,ContractCode,ShopName,InvCode,InvDate,b.InvPayAmt,b.InvAdjAmt,b.InvDiscAmt,b.InvPaidAmt,TradeName,a.InvExRate,b.InvActPayAmt " + 
                    " from InvoiceHeader a left join InvoiceDetail b on a.InvID=b.InvID left join Customer c on " +
                    "a.CustID = c.CustID left join Contract d on a.ContractID=d.ContractID left join ConShop e on d.ContractID=e.ContractID " +
                     "left join Floors f on e.FloorID=f.FloorID left join Building g on e.BuildingID=g.BuildingID left join ChargeType h " +
                     "on b.ChargeTypeID=h.ChargeTypeID left join TradeRelation i on d.TradeID=i.TradeID";
        }

        public int InvDetailID
        {
            get { return invDetailID; }
            set { invDetailID = value; }
        }

        public string CustCode
        {
            get { return custCode; }
            set { custCode = value; }
        }

        public string CustName
        {
            get { return custName; }
            set { custName = value; }
        }

        public string ContractCode
        {
            get { return contractCode; }
            set { contractCode = value; }
        }

        public string ShopName
        {
            get { return shopName; }
            set { shopName = value; }
        }

        public string InvCode
        {
            get { return invCode; }
            set { invCode = value; }
        }

        public DateTime InvDate
        {
            get { return invDate; }
            set { invDate = value; }
        }

        public decimal InvPayAmt
        {
            get { return invPayAmt; }
            set { invPayAmt = value; }
        }

        public decimal InvPaidAmt
        {
            get { return invPaidAmt; }
            set { invPaidAmt = value; }
        }

        public string TradeName
        {
            get { return tradeName; }
            set { tradeName = value; }
        }

        public decimal InvAdjAmt
        {
            get { return invAdjAmt; }
            set { invAdjAmt = value; }
        }

        public decimal InvDiscAmt
        {
            get { return invDiscAmt; }
            set { invDiscAmt = value; }
        }
    }
}
