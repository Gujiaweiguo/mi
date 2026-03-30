using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    /*调整结算明细表操作*/
    public class InvAdjDetIns:BasePO
    {
        private int invAdjDetID = 0;
        private int invDetailID = 0;
        private int invAdjID = 0;
        private decimal adjAmt = 0;
        private decimal adjAmtL = 0;
        private string adjReason = "";
        private DateTime adjDate = DateTime.Now;
        private int adjOpr = 0;
        private string chargeTypeName = "";
        private string contractCode = "";
        private DateTime period = DateTime.Now;
        private string custName = "";
        private string invCode = "";
        private decimal invPayAmt = 0;
        private decimal invAdjAmt = 0;
        private decimal invAdjAmtL = 0;
        private string userName = "";

        public override string GetTableName()
        {
            return "InvAdjDet";
        }

        public override string GetColumnNames()
        {
            return "InvAdjDetID,AdjDate,InvAdjAmt,AdjAmt,InvAdjAmtL,AdjAmtL,AdjOpr,AdjReason,InvCode,ChargeTypeName,ContractCode,Period,CustName,InvPayAmt,InvDetailID";
        }

        public override string GetInsertColumnNames()
        {
            return "InvAdjDetID,InvDetailID,InvAdjID,AdjAmt,AdjAmtL,AdjReason";
        }

        public override string GetUpdateColumnNames()
        {
            return "AdjAmt,AdjAmtL,AdjReason";
        }

        public override string GetQuerySql()
        {
            return "select c.InvAdjDetID,AdjDate,a.AdjAmt as InvAdjAmt,c.AdjAmt,a.AdjAmtL as InvAdjAmtL,c.AdjAmtL,AdjOpr,c.AdjReason,InvCode,ChargeTypeName,ContractCode,Period,CustName,d.InvPayAmt,d.InvDetailID,'' as UserName,d.InvActPayAmt,d.InvPaidAmt " +
                    "from InvAdj a left join InvoiceHeader b on a.InvID=b.InvID left join InvAdjDet c on a.InvAdjID=c.InvAdjID " +
                     "left join  InvoiceDetail d on c.InvDetailID=d.InvDetailID left join  ChargeType e " +
                    "on d.ChargeTypeID=e.ChargeTypeID left join Contract f on b.ContractID = f.ContractID";
        }

        public int InvAdjDetID
        {
            get { return invAdjDetID; }
            set { invAdjDetID = value; }
        }

        public int InvDetailID
        {
            get { return invDetailID; }
            set { invDetailID = value; }
        }

        public int InvAdjID
        {
            get { return invAdjID; }
            set { invAdjID = value; }
        }

        public decimal AdjAmt
        {
            get { return adjAmt; }
            set { adjAmt = value; }
        }

        public decimal AdjAmtL
        {
            get { return adjAmtL; }
            set { adjAmtL = value; }
        }

        public string AdjReason
        {
            get { return adjReason; }
            set { adjReason = value; }
        }

        public DateTime AdjDate
        {
            get { return adjDate; }
            set { adjDate = value; }
        }

        public int AdjOpr
        {
            get { return adjOpr; }
            set { adjOpr = value; }
        }

        public string ChargeTypeName
        {
            get { return chargeTypeName; }
            set { chargeTypeName = value; }
        }
        
        public string ContractCode
        {
            get { return contractCode; }
            set { contractCode = value; }
        }

        public DateTime Period
        {
            get { return period; }
            set { period = value; }
        }

        public string CustName
        {
            get { return custName; }
            set { custName = value; }
        }

        public string InvCode
        {
            get { return invCode; }
            set { invCode = value; }
        }

        public decimal InvPayAmt
        {
            get { return invPayAmt; }
            set { invPayAmt = value; }
        }

        public decimal InvAdjAmtL
        {
            get { return invAdjAmtL; }
            set { invAdjAmtL = value; }
        }

        public decimal InvAdjAmt
        {
            get { return invAdjAmt; }
            set { invAdjAmt = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
    }
}
