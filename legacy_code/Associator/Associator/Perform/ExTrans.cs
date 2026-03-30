using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Associator.Perform
{
    /// <summary>
    /// Ð¡Æ±¶Ò»»¼ÇÂ¼
    /// </summary>
    public class ExTrans : BasePO
    {
        private int exID;
        private int membID;
        private DateTime exDate = DateTime.Now;
        private string receiptNum = "";
        private decimal transAmt = 0;
        private int giftQty = 0;
        private int giftID = 0;

        public override string GetTableName()
        {
            return "ExTrans";
        }

        public override string GetColumnNames()
        {
            return "ExID,MembID,ExDate,ReceiptNum,TransAmt,GiftQty,GiftID";
        }

        public override string GetInsertColumnNames()
        {
            return "ExID,MembID,ExDate,ReceiptNum,TransAmt,GiftQty,GiftID";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public int ExID
        {
            get { return exID; }
            set { exID = value; }
        }

        public int MembID
        {
            get { return membID; }
            set { membID = value; }
        }

        public DateTime ExDate
        {
            get { return exDate; }
            set { exDate = value; }
        }

        public string ReceiptNum
        {
            get { return receiptNum; }
            set { receiptNum = value; }
        }

        public decimal TransAmt
        {
            get { return transAmt; }
            set { transAmt = value; }
        }

        public int GiftQty
        {
            get { return giftQty; }
            set { giftQty = value; }
        }

        public int GiftID
        {
            get { return giftID; }
            set { giftID = value; }
        }
    }
}
