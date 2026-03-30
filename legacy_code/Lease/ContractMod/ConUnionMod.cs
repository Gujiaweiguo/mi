using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Lease.ContractMod
{
    public class ConUnionMod:BasePO
    {
        private int conModID = 0;
        private int contractID;
        private int billCycle;
        private string rentInc;
        private int accountCycle;
        private decimal taxRate;
        private int taxType;
        private int curTypeID;
        private decimal inTaxRate = 0;
        private decimal outTaxRate = 0;

        public int ConModID
        {
            get { return conModID; }
            set { conModID = value; }
        }

        public int ContractID
        {
            get { return contractID; }
            set { contractID = value; }
        }

        public int BillCycle
        {
            get { return billCycle; }
            set { billCycle = value; }
        }

        public string RentInc
        {
            get { return rentInc; }
            set { rentInc = value; }
        }

        public int AccountCycle
        {
            get { return accountCycle; }
            set { accountCycle = value; }
        }

        public decimal TaxRate
        {
            get { return taxRate; }
            set { taxRate = value; }
        }

        public int TaxType
        {
            get { return taxType; }
            set { taxType = value; }
        }

        public int CurTypeID
        {
            get { return curTypeID; }
            set { curTypeID = value; }
        }

        public decimal InTaxRate
        {
            get { return inTaxRate; }
            set { inTaxRate = value; }
        }

        public decimal OutTaxRate
        {
            get { return outTaxRate; }
            set { outTaxRate = value; }
        }

        public override string GetTableName()
        {
            return "ConUnionMod";
        }

        public override string GetColumnNames()
        {
            return "ConModID,ContractID,BillCycle,RentInc,AccountCycle,TaxRate,TaxType,CurTypeID,InTaxRate,OutTaxRate";
        }

        public override string GetInsertColumnNames()
        {
            return "ConModID,ContractID,BillCycle,RentInc,AccountCycle,TaxRate,TaxType,CurTypeID,InTaxRate,OutTaxRate";
        }

        public override string GetUpdateColumnNames()
        {
            return "BillCycle,RentInc,AccountCycle,TaxRate,TaxType,CurTypeID,InTaxRate,OutTaxRate";
        }

    }
}
