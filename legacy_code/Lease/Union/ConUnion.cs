using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Lease.Union
{
    /// <summary>
    /// 联营合同
    /// </summary>
    public class ConUnion:BasePO
    {
        private int contractID;
        private int billCycle;
        private string rentInc;
        private int accountCycle;
        private decimal taxRate;
        private int taxType;
        private int curTypeID;
        private decimal inTaxRate=0;
        private decimal outTaxRate = 0;
        private float monthSettleDays = 0;
        private decimal latePayInt = 0;
        private int intDay = 0;

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

        public float MonthSettleDays
        {
            get { return monthSettleDays; }
            set { monthSettleDays = value; }
        }

        public decimal LatePayInt
        {
            get { return latePayInt; }
            set { latePayInt = value; }
        }

        public int IntDay
        {
            get { return intDay; }
            set { intDay = value; }
        }

        public override string GetTableName()
        {
            return "ConUnion";
        }

        public override string GetColumnNames()
        {
            return "ContractID,BillCycle,RentInc,AccountCycle,TaxRate,TaxType,CurTypeID,InTaxRate,OutTaxRate,MonthSettleDays,LatePayInt,IntDay";
        }

        public override string GetInsertColumnNames()
        {
            return "ContractID,BillCycle,RentInc,AccountCycle,TaxRate,TaxType,CurTypeID,InTaxRate,OutTaxRate,MonthSettleDays,LatePayInt,IntDay";
        }

        public override string GetUpdateColumnNames()
        {
            return "BillCycle,RentInc,AccountCycle,TaxRate,TaxType,CurTypeID,InTaxRate,OutTaxRate,MonthSettleDays,LatePayInt,IntDay";
        }

        //币种
        public static int CURRENCY_TYPE_RMB = 1;
        public static int CURRENCY_TYPE_SINGAPORE = 2;
        public static int CURRENCY_TYPE_US = 3;
        public static int CURRENCY_TYPE_EUROPE = 4;
        public static int BILLCYCLE_TYPE_JAPAN = 5;


        public static int[] GetCurrencyTypeTypeStatus()
        {
            int[] currencyTypeTypeStatus = new int[5];
            currencyTypeTypeStatus[0] = CURRENCY_TYPE_RMB;
            currencyTypeTypeStatus[1] = CURRENCY_TYPE_SINGAPORE;
            currencyTypeTypeStatus[2] = CURRENCY_TYPE_US;
            currencyTypeTypeStatus[3] = CURRENCY_TYPE_EUROPE;
            currencyTypeTypeStatus[4] = BILLCYCLE_TYPE_JAPAN;

            return currencyTypeTypeStatus;
        }

        public static String GetCurrencyTypeTypeStatusDesc(int currencyTypeTypeStatus)
        {
            if (currencyTypeTypeStatus == CURRENCY_TYPE_RMB)
            {
                return "人民币";
            }
            if (currencyTypeTypeStatus == CURRENCY_TYPE_SINGAPORE)
            {
                return "新加坡元";
            }
            if (currencyTypeTypeStatus == CURRENCY_TYPE_US)
            {
                return "美圆";
            }
            if (currencyTypeTypeStatus == CURRENCY_TYPE_EUROPE)
            {
                return "欧元";
            }
            if (currencyTypeTypeStatus == BILLCYCLE_TYPE_JAPAN)
            {
                return "日元";
            }

            return "未知";
        }

        //结算周期FirstSetAcountMon
        public static int FIRSTSETACOUNTMON_TYPE_ZERO = 0;
        public static int FIRSTSETACOUNTMON_TYPE_ONE = 1;
        public static int FIRSTSETACOUNTMON_TYPE_TWO = 2;
        public static int FIRSTSETACOUNTMON_TYPE_THREE = 3;
        public static int FIRSTSETACOUNTMON_TYPE_FOUR = 4;
        public static int FIRSTSETACOUNTMON_TYPE_FIVE = 5;
        public static int FIRSTSETACOUNTMON_TYPE_SIX = 6;
        public static int FIRSTSETACOUNTMON_TYPE_SEVENT = 7;
        public static int FIRSTSETACOUNTMON_TYPE_EIGHT = 8;
        public static int FIRSTSETACOUNTMON_TYPE_NINE = 9;
        public static int FIRSTSETACOUNTMON_TYPE_TEN = 10;
        public static int FIRSTSETACOUNTMON_TYPE_ELEVEN = 11;
        public static int FIRSTSETACOUNTMON_TYPE_TWELVE = 12;

        public static int[] GetFirstSetAcountMonStatus()
        {
            int[] FirstSetAcountMonStatus = new int[13];
            FirstSetAcountMonStatus[0] = FIRSTSETACOUNTMON_TYPE_ZERO;
            FirstSetAcountMonStatus[1] = FIRSTSETACOUNTMON_TYPE_ONE;
            FirstSetAcountMonStatus[2] = FIRSTSETACOUNTMON_TYPE_TWO;
            FirstSetAcountMonStatus[3] = FIRSTSETACOUNTMON_TYPE_THREE;
            FirstSetAcountMonStatus[4] = FIRSTSETACOUNTMON_TYPE_FOUR;
            FirstSetAcountMonStatus[5] = FIRSTSETACOUNTMON_TYPE_FIVE;
            FirstSetAcountMonStatus[6] = FIRSTSETACOUNTMON_TYPE_SIX;
            FirstSetAcountMonStatus[7] = FIRSTSETACOUNTMON_TYPE_SEVENT;
            FirstSetAcountMonStatus[8] = FIRSTSETACOUNTMON_TYPE_EIGHT;
            FirstSetAcountMonStatus[9] = FIRSTSETACOUNTMON_TYPE_NINE;
            FirstSetAcountMonStatus[10] = FIRSTSETACOUNTMON_TYPE_TEN;
            FirstSetAcountMonStatus[11] = FIRSTSETACOUNTMON_TYPE_ELEVEN;
            FirstSetAcountMonStatus[12] = FIRSTSETACOUNTMON_TYPE_TWELVE;
            return FirstSetAcountMonStatus;
        }

        public static string GetFirstSetAcountMonStatusDesc(int FirstSetAcountMonStatus)
        {
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_ZERO)
            {
                return "半月";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_ONE)
            {
                return "1月";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_TWO)
            {
                return "2月";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_THREE)
            {
                return "3月";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_FOUR)
            {
                return "4月";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_FIVE)
            {
                return "5月";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_SIX)
            {
                return "6月";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_SEVENT)
            {
                return "7月";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_EIGHT)
            {
                return "8月";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_NINE)
            {
                return "9月";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_TEN)
            {
                return "10月";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_ELEVEN)
            {
                return "11月";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_TWELVE)
            {
                return "12月";
            }
            return "未知";

        }
        //发票类型　TaxType
        public static int FAXTYPE_TYPE_INCRE = 1;
        public static int FAXTYPE_TYPE_COMM = 2;
        public static int FAXTYPE_TYPE_OTHER = 3;

        public static int[] GetTaxTypeStatus()
        {
            int[] TaxTypeStatus = new int[3];
            TaxTypeStatus[0] = FAXTYPE_TYPE_INCRE;
            TaxTypeStatus[1] = FAXTYPE_TYPE_COMM;
            TaxTypeStatus[2] = FAXTYPE_TYPE_OTHER;
            return TaxTypeStatus;
        }

        public static string GetTaxTypeStatusDesc(int TaxTypeStatus)
        {
            if (TaxTypeStatus == FAXTYPE_TYPE_INCRE)
            {
                return "增值税";
            }
            if (TaxTypeStatus == FAXTYPE_TYPE_COMM)
            {
                return "普通发票";
            }
            if (TaxTypeStatus == FAXTYPE_TYPE_OTHER)
            {
                return "其它";
            }
            return "未知";
        }

        //帐期AccountCycle
        public static int ACCOUNTCYCLE_TYPE_HALFM = 1;
        public static int ACCOUNTCYCLE_TYPE_ONEM = 2;
        public static int ACCOUNTCYCLE_TYPE_ONEHALFM = 3;
        public static int ACCOUNTCYCLE_TYPE_TWOM = 4;
        public static int ACCOUNTCYCLE_TYPE_THREEM = 5;

        public static int[] GetAccountCycleStatus()
        {
            int[] AccountCycleStatus = new int[5];
            AccountCycleStatus[0] = ACCOUNTCYCLE_TYPE_HALFM;
            AccountCycleStatus[1] = ACCOUNTCYCLE_TYPE_ONEM;
            AccountCycleStatus[2] = ACCOUNTCYCLE_TYPE_ONEHALFM;
            AccountCycleStatus[3] = ACCOUNTCYCLE_TYPE_TWOM;
            AccountCycleStatus[4] = ACCOUNTCYCLE_TYPE_THREEM;
            return AccountCycleStatus;
        }

        public static string GetAccountCycleStatusDesc(int AccountCycleStatus)
        {
            if (AccountCycleStatus == ACCOUNTCYCLE_TYPE_HALFM)
            {
                return "15天";
            }
            if (AccountCycleStatus == ACCOUNTCYCLE_TYPE_ONEM)
            {
                return "30天";
            }
            if (AccountCycleStatus == ACCOUNTCYCLE_TYPE_ONEHALFM)
            {
                return "45天";
            }
            if (AccountCycleStatus == ACCOUNTCYCLE_TYPE_TWOM)
            {
                return "60天";
            }
            if (AccountCycleStatus == ACCOUNTCYCLE_TYPE_THREEM)
            {
                return "90天";
            }
            return "未知";
        }

    }
}