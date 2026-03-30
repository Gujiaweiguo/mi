using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.ConOvertimeBill
{
    public class ConFormulaHOvtm:BasePO
    {
        private int conOverTimeID = 0;
        private int formulaID = 0;
        private int chargeTypeID = 0;
        private DateTime fStartDate = DateTime.Now;
        private DateTime fEndDate = DateTime.Now;
        private string formulaType = "";
        private decimal totalArea = 0;
        private decimal unitPrice = 0;
        private decimal baseAmt = 0;
        private decimal fixedRental = 0;
        private string rateType = "";
        private string pcentOpt = "";
        private string minSumOpt = "";
        private string chargeTypeName = "";
        private string formulaTypeName = "";
        private int contractID = 0;
        public override string GetTableName()
        {
            return "ConFormulaHOvtm";
        }

        public override string GetColumnNames()
        {
            return "ConOverTimeID,FormulaID,ChargeTypeID,FStartDate,FEndDate,FormulaType,TotalArea,UnitPrice,BaseAmt,FixedRental,RateType,PcentOpt,MinSumOpt,ContractID";
        }

        public override string GetInsertColumnNames()
        {
            return "ConOverTimeID,FormulaID,ContractID,ChargeTypeID,FStartDate,FEndDate,FormulaType,TotalArea,UnitPrice,BaseAmt,FixedRental,RateType,PcentOpt,MinSumOpt";
        }

        public override string GetUpdateColumnNames()
        {
            return "ChargeTypeID,FStartDate,FEndDate,FormulaType,TotalArea,UnitPrice,BaseAmt,FixedRental,RateType,PcentOpt,MinSumOpt";
        }

        public override string GetQuerySql()
        {
            return "select ConOverTimeID,FormulaID,ChargeTypeID,FStartDate,FEndDate,FormulaType,TotalArea,UnitPrice,BaseAmt,FixedRental,RateType,PcentOpt,MinSumOpt,ContractID," +
                   "'' as ChargeTypeName,'' as FormulaTypeName from ConFormulaHOvtm";
        }

        public int ConOverTimeID
        {
            get { return conOverTimeID; }
            set { conOverTimeID = value; }
        }

        public int FormulaID
        {
            get { return formulaID; }
            set { formulaID = value; }
        }

        public int ChargeTypeID
        {
            get { return chargeTypeID; }
            set { chargeTypeID = value; }
        }

        public DateTime FStartDate
        {
            get { return fStartDate; }
            set { fStartDate = value; }
        }

        public DateTime FEndDate
        {
            get { return fEndDate; }
            set { fEndDate = value; }
        }

        public string FormulaType
        {
            get { return formulaType; }
            set { formulaType = value; }
        }

        public decimal TotalArea
        {
            get { return totalArea; }
            set { totalArea = value; }
        }

        public decimal UnitPrice
        {
            get { return unitPrice; }
            set { unitPrice = value; }
        }

        public decimal BaseAmt
        {
            get { return baseAmt; }
            set { baseAmt = value; }
        }

        public decimal FixedRental
        {
            get { return fixedRental; }
            set { fixedRental = value; }
        }

        public string RateType
        {
            get { return rateType; }
            set { rateType = value; }
        }

        public string PcentOpt
        {
            get { return pcentOpt; }
            set { pcentOpt = value; }
        }

        public string MinSumOpt
        {
            get { return minSumOpt; }
            set { minSumOpt = value; }
        }

        public string ChargeTypeName
        {
            get { return chargeTypeName; }
            set { chargeTypeName = value; }
        }

        public string FormulaTypeName
        {
            get { return formulaTypeName; }
            set { formulaTypeName = value; }
        }

        public int ContractID
        {
            get { return contractID; }
            set { contractID = value; }
        }
    }
}
