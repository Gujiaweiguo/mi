using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.ChangeLease
{
    public class ConFormulaHMod:BasePO
    {
        private int conFormulaModID = 0;
        private int formulaID = 0;
        private int chargeTypeID = 0;
        private DateTime fStartDate = DateTime.Now;
        private DateTime fEndDate = DateTime.Now;
        private string formulaType="";
        private decimal totalArea = 0;
        private decimal unitPrice = 0;
        private decimal baseAmt = 0;
        private decimal fixedRental = 0;
        private string rateType ="";
        private string pcentOpt = "";
        private string minSumOpt = "";
        private string chargeTypeName = "";
        private string formulaTypeName = "";

        public override string GetTableName()
        {
            return "ConFormulaHMod";
        }

        public override string GetColumnNames()
        {
            return "ConFormulaModID,FormulaID,ChargeTypeID,FStartDate,FEndDate,FormulaType,TotalArea,UnitPrice,BaseAmt,FixedRental,RateType,PcentOpt,MinSumOpt";
        }

        public override string GetInsertColumnNames()
        {
            return "ConFormulaModID,FormulaID,ChargeTypeID,FStartDate,FEndDate,FormulaType,TotalArea,UnitPrice,BaseAmt,FixedRental,RateType,PcentOpt,MinSumOpt";
        }

        public override string GetUpdateColumnNames()
        {
            return "ChargeTypeID,FStartDate,FEndDate,FormulaType,TotalArea,UnitPrice,BaseAmt,FixedRental,RateType,PcentOpt,MinSumOpt";
        }

        public override string GetQuerySql()
        {
            return "select ConFormulaModID,FormulaID,ChargeTypeID,FStartDate,FEndDate,FormulaType,TotalArea,UnitPrice,BaseAmt,FixedRental,RateType,PcentOpt,MinSumOpt,"+
                   "'' as ChargeTypeName,'' as FormulaTypeName from ConFormulaHMod";
        }

        public int ConFormulaModID
        {
            get { return conFormulaModID; }
            set { conFormulaModID = value; }
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
    }
}
