using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Lease.ContractMod
{
    public class ConLeaseMod:BasePO
    {
        private int conModID;
        private int contractID;
        private int billCycle;
        private int curTypeID;
        private int settleMode;
        private float monthSettleDays;
        private string rentInc;
        private int payTypeId;
        private decimal latePayInt;
        private int intDay;
        private decimal taxRate;
        private int ifPrepay;
        private string additionalItem;
        private string eConURL;
        private string note;
        private DateTime balanceMonth;
        private int taxType;

        public override string GetTableName()
        {
            return "ConLeaseMod";
        }

        public override string GetColumnNames()
        {
            return "ConModID,ContractID,BillCycle,CurTypeID,SettleMode,MonthSettleDays,RentInc,PayTypeID,LatePayInt,IntDay,TaxRate,IfPrepay,BalanceMonth,TaxType";
        }

        public override string GetUpdateColumnNames()
        {
            return "BillCycle,CurTypeID,SettleMode,MonthSettleDays,RentInc,PayTypeID,LatePayInt,IntDay,TaxRate,IfPrepay,BalanceMonth,TaxType";
        }
        public override string GetInsertColumnNames()
        {
            return "ConModID,ContractID,BillCycle,CurTypeID,SettleMode,MonthSettleDays,RentInc,PayTypeID,LatePayInt,IntDay,TaxRate,IfPrepay,BalanceMonth,TaxType";
        }

        public int BillCycle
        {
            get { return billCycle; }
            set { billCycle = value; }
        }
        public int CurTypeID
        {
            get { return curTypeID; }
            set { curTypeID = value; }
        }
        public int SettleMode
        {
            get { return settleMode; }
            set { settleMode = value; }
        }
        public float MonthSettleDays
        {
            get { return monthSettleDays; }
            set { monthSettleDays = value; }
        }
        public string RentInc
        {
            get { return rentInc; }
            set { rentInc = value; }
        }
        public int PayTypeID
        {
            get { return payTypeId; }
            set { payTypeId = value; }
        }
        public decimal LatePayInt
        {
            get { return latePayInt; }
            set { latePayInt = value; }
        }
        public string Note
        {
            get { return note; }
            set { note = value; }
        }
        public decimal TaxRate
        {
            get { return taxRate; }
            set { taxRate = value; }
        }
        public string AdditionalItem
        {
            get { return additionalItem; }
            set { additionalItem = value; }
        }
        public string EConURL
        {
            get { return eConURL; }
            set { eConURL = value; }
        }

        public int IntDay
        {
            get { return intDay; }
            set { intDay = value; }
        }

        public int ConModID
        {
            get { return conModID; }
            set { conModID = value; }
        }

        public int IfPrepay
        {
            get { return ifPrepay; }
            set { ifPrepay = value; }
        }

        public DateTime BalanceMonth
        {
            get { return balanceMonth; }
            set { balanceMonth = value; }
        }

        public int TaxType
        {
            get { return taxType; }
            set { taxType = value; }
        }

        public int ContractID
        {
            get { return contractID; }
            set { contractID = value; }
        }
    }
}
