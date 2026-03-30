using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    /*结算单优惠审批*/
    public class InvDiscAuditing:BasePO
    {
        private int invID = 0;
        private int invDetailID = 0;
        private int discDetID = 0;
        private string invCode = "";
        private DateTime invDate = DateTime.Now;
        private decimal invPayAmt = 0;
        private decimal invPaidAmt = 0;
        private decimal discRate = 0;
        private decimal discAmt = 0;
        private string discReason = "";
        private string shopName = "";
        private string custCode = "";
        private string custName = "";
        private string contractCode = "";
        private decimal invPayAmtDetail = 0;
        private decimal invExRate = 0;
        private decimal invAdjAmt = 0;
        private decimal invDiscAmt = 0;
        private decimal invActPayAmt = 0;
        private decimal invActPayAmtL = 0;

        public override string GetTableName()
        {
            return "";
        }

        public override string GetColumnNames()
        {
            return "InvID,InvDetailID,DiscDetID,InvCode,InvDate,InvPayAmt,InvPaidAmt,DiscRate,DiscAmt,DiscReason,ShopName,CustCode,CustName,ContractCode,InvPayAmtDetail,InvExRate,InvAdjAmt,InvDiscAmt,InvActPayAmt,InvActPayAmtL";
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
            return "select c.InvID,b.InvDetailID,DiscDetID,InvCode,InvDate,b.InvPayAmt,b.InvPaidAmt,DiscRate,DiscAmt,DiscReason,ShopName,CustCode,e.CustName,ContractCode,b.InvPayAmt as InvPayAmtDetail,b.InvExRate,b.InvAdjAmt,b.InvDiscAmt,b.InvActPayAmt,b.InvActPayAmtL " +
                     "from InvDiscDet a left join InvoiceDetail b on a.InvDetailID=b.InvDetailID left join " +
                     "InvoiceHeader c on b.InvID=c.InvID left join ConShop d on c.ContractID=d.ContractID left join " +
                     "Customer e on c.CustID=e.CustID left join Contract f on c.ContractID=f.ContractID";
        }

        public int InvID
        {
            get { return invID; }
            set { invID = value; }
        }

        public int InvDetailID
        {
            get { return invDetailID; }
            set { invDetailID = value; }
        }

        public int DiscDetID
        {
            get { return discDetID; }
            set { discDetID = value; }
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

        public decimal DiscRate
        {
            get { return discRate; }
            set { discRate = value; }
        }

        public decimal DiscAmt
        {
            get { return discAmt; }
            set { discAmt = value; }
        }

        public string DiscReason
        {
            get { return discReason; }
            set { discReason = value; }
        }

        public string ShopName
        {
            get { return shopName; }
            set { shopName = value; }
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

        public decimal InvPayAmtDetail
        {
            get { return invPayAmtDetail; }
            set { invPayAmtDetail = value; }
        }

        public decimal InvExRate
        {
            get { return invExRate; }
            set { invExRate = value; }
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

        public decimal InvActPayAmt
        {
            get { return invActPayAmt; }
            set { invActPayAmt = value; }
        }

        public decimal InvActPayAmtL
        {
            get { return invActPayAmtL; }
            set { invActPayAmtL = value; }
        }
    }
}
