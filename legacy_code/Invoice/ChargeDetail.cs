using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Invoice
{
    /// <summary>
    /// ·ÑÓÃÃ÷Ï¸
    /// </summary>
    public class ChargeDetail:BasePO
    {
        private int chgDetID = 0;
        private int chgID = 0;
        private int chargeTypeID = 0;
        private string chgName = "";
        private DateTime startDate = DateTime.Now;
        private DateTime endDate = DateTime.Now;
        private int hdwID = 0;
        private int refID = 0;
        private decimal chgAmt = 0;
        private int lastQty = 0;
        private int curQty = 0;
        private int costQty = 0;
        private int times = 0;
        private int freeQty = 0;
        private decimal price = 0;
        private string note = "";
        private DateTime chgPeriod = DateTime.Now;

        public int ChgDetID
        {
            get { return chgDetID; }
            set { chgDetID = value; }
        }

        public int ChgID
        {
            get { return chgID; }
            set { chgID = value; }
        }

        
        public int ChargeTypeID
        {
            get { return chargeTypeID; }
            set { chargeTypeID = value; }
        }
        public string ChgName
        {
            get { return chgName; }
            set { chgName = value; }
        }
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
        public int HdwID
        {
            get { return hdwID; }
            set { hdwID = value; }
        }
        public int RefID
        {
            get { return refID; }
            set { refID = value; }
        }
        public decimal ChgAmt
        {
            get { return chgAmt; }
            set { chgAmt = value; }
        }
        public int LastQty
        {
            get { return lastQty; }
            set { lastQty = value; }
        }
        public int CurQty
        {
            get { return curQty; }
            set { curQty = value; }
        }
        public int CostQty
        {
            get { return costQty; }
            set { costQty = value; }
        }
        public int Times
        {
            get { return times; }
            set { times = value; }
        }
        public int FreeQty
        {
            get { return freeQty; }
            set { freeQty = value; }
        }
        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }
        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public DateTime ChgPeriod
        {
            get { return chgPeriod; }
            set { chgPeriod = value; }
        }



        public override string GetTableName()
        {
            return "ChargeDetail";
        }

        public override string GetColumnNames()
        {
            return "ChgDetID,HdwID,ChgID,ChargeTypeID,HdwID,ChgName,StartDate,EndDate,RefID,ChgAmt,LastQty,CurQty,CostQty,Times,FreeQty,Price,Note,'' as HdwCode,'' as ChargeTypeName,ChgPeriod,'' as ShopID,'' as ShopName";
        }

        public override string GetInsertColumnNames()
        {
            return "ChgDetID,HdwID,ChgID,ChargeTypeID,ChgName,StartDate,EndDate,RefID,ChgAmt,LastQty,CurQty,CostQty,Times,FreeQty,Price,Note,ChgPeriod";
        }

        public override string GetUpdateColumnNames()
        {
            return "HdwID,ChargeTypeID,ChgName,StartDate,EndDate,ChgAmt,LastQty,CurQty,CostQty,Times,FreeQty,Price";
        }
    }
}
