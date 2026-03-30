using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Lease
{
    public class ConLease:BasePO
    {
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
            return "ConLease";
        }

        public override string GetColumnNames()
        {
            return "ContractID,BillCycle,CurTypeID,SettleMode,RentInc,PayTypeID,LatePayInt,IntDay,TaxRate,MonthSettleDays,BalanceMonth,TaxType,IfPrepay";
        }

        public override string GetUpdateColumnNames()
        {
            return "BillCycle,CurTypeID,SettleMode,MonthSettleDays,RentInc,PayTypeID,LatePayInt,IntDay,TaxRate,IfPrepay,BalanceMonth,TaxType";
        }
        public override string GetInsertColumnNames()
        {
            return "ContractID,BillCycle,CurTypeID,SettleMode,MonthSettleDays,RentInc,PayTypeID,LatePayInt,IntDay,TaxRate,IfPrepay,BalanceMonth,TaxType";
        }

        public static int BILLCYCLE_TYPE_MONTH = 0;
        public static int BILLCYCLE_TYPE_YEAR = 1;
        public static int BILLCYCLE_TYPE_QUARTER = 2;


        public static int[] GetBillCycleTypeStatus()
        {
            int[] billCycleTypeStatus = new int[3];
            billCycleTypeStatus[0] = BILLCYCLE_TYPE_MONTH;
            billCycleTypeStatus[1] = BILLCYCLE_TYPE_YEAR;
            billCycleTypeStatus[2] = BILLCYCLE_TYPE_QUARTER;

            return billCycleTypeStatus;
        }

        public static String GetBillCycleTypeStatusDesc(int billCycleTypeStatus)
        {
            if (billCycleTypeStatus == BILLCYCLE_TYPE_MONTH)
            {
                return "月";
            }
            if (billCycleTypeStatus == BILLCYCLE_TYPE_YEAR)
            {
                return "年";
            }
            if (billCycleTypeStatus == BILLCYCLE_TYPE_QUARTER)
            {
                return "季度";
            }
            return "未知";
        }
        //结算

        public static int CURRENCY_TYPE_RMB = 1;
        public static int CURRENCY_TYPE_SINGAPORE = 2;
        public static int CURRENCY_TYPE_US = 3;
        public static int CURRENCY_TYPE_EUROPE = 4;
    //    public static int BILLCYCLE_TYPE_QUARTER = 2;


        public static int[] GetCurrencyTypeTypeStatus()
        {
            int[] currencyTypeTypeStatus = new int[4];
            currencyTypeTypeStatus[0] = CURRENCY_TYPE_RMB;
            currencyTypeTypeStatus[1] = CURRENCY_TYPE_SINGAPORE;
            currencyTypeTypeStatus[2] = CURRENCY_TYPE_US;
            currencyTypeTypeStatus[3] = CURRENCY_TYPE_EUROPE;

            return currencyTypeTypeStatus;
        }

        public static String GetCurrencyTypeTypeStatusDesc(int currencyTypeTypeStatus)
        {
            if (currencyTypeTypeStatus == CURRENCY_TYPE_RMB)
            {
                return "人民币";
            }
            if(currencyTypeTypeStatus == CURRENCY_TYPE_SINGAPORE)
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

            return "未知";
        }

        //结算处理方式SettleMode

        public static int SETTLEMODE_TYPE_FIRST = 1;
        public static int SETTLEMODE_TYPE_LAST = 2;
        public static int SETTLEMODE_TYPE_NO = 3;


        public static int[] GetSettleModeTypeStatus()
        {
            int[] settleModeTypeStatus = new int[3];
            settleModeTypeStatus[0] = SETTLEMODE_TYPE_FIRST;
            settleModeTypeStatus[1] = SETTLEMODE_TYPE_LAST;
            settleModeTypeStatus[2] = SETTLEMODE_TYPE_NO;

            return settleModeTypeStatus;
        }

        public static String GetSettleModeTypeStatusDesc(int settleModeTypeStatus)
        {
            if (settleModeTypeStatus == SETTLEMODE_TYPE_FIRST)
            {
                return "SETTLEMODE_TYPE_FIRST";// "首月对齐";
            }
            if (settleModeTypeStatus == SETTLEMODE_TYPE_LAST)
            {
                return "SETTLEMODE_TYPE_LAST";// "次月对齐";
            }
            if (settleModeTypeStatus == SETTLEMODE_TYPE_NO)
            {
                return "SETTLEMODE_TYPE_NO";// "不做调整";
            }
            return "NO";
        }

        //租金增长率

        ////PayTypeId

        //public static int PAYTYPEID_TYPE_FOREGIFONE = 1;
        //public static int PAYTYPEID_TYPE_FOREGIFTWO = 2;
        //public static int PAYTYPEID_TYPE_FOREGIFTHREE = 3;
        //public static int PAYTYPEID_TYPE_FOREGIFRIVET = 4;
        //public static int PAYTYPEID_TYPE_FOREGIFONETHREE = 5;

        //public static int[] GetPayTypeIdTypeStatus()
        //{
        //    int[] payTypeIdTypeStatus = new int[5];
        //    payTypeIdTypeStatus[0] = PAYTYPEID_TYPE_FOREGIFONE;
        //    payTypeIdTypeStatus[1] = PAYTYPEID_TYPE_FOREGIFTWO;
        //    payTypeIdTypeStatus[2] = PAYTYPEID_TYPE_FOREGIFTHREE;
        //    payTypeIdTypeStatus[3] = PAYTYPEID_TYPE_FOREGIFRIVET;
        //    payTypeIdTypeStatus[4] = PAYTYPEID_TYPE_FOREGIFONETHREE;

        //    return payTypeIdTypeStatus;
        //}

        //public static String GetPayTypeIdTypeStatusDesc(int payTypeIdTypeStatus)
        //{
        //    if (payTypeIdTypeStatus == PAYTYPEID_TYPE_FOREGIFONETHREE)
        //    {
        //        return "PAYTYPEID_TYPE_FOREGIFONETHREE"; //押一付三
        //    }
        //    if (payTypeIdTypeStatus == PAYTYPEID_TYPE_FOREGIFONE)
        //    {
        //        return "PAYTYPEID_TYPE_FOREGIFONE"; //押一付一
        //    }
        //    if (payTypeIdTypeStatus == PAYTYPEID_TYPE_FOREGIFTWO)
        //    {
        //        return "PAYTYPEID_TYPE_FOREGIFTWO";  //"押二付一";
        //    }
        //    if (payTypeIdTypeStatus == PAYTYPEID_TYPE_FOREGIFTHREE)
        //    {
        //        return "PAYTYPEID_TYPE_FOREGIFTHREE"; // "押三付一";
        //    }
        //    if (payTypeIdTypeStatus == PAYTYPEID_TYPE_FOREGIFRIVET)
        //    {
        //        return "PAYTYPEID_TYPE_FOREGIFRIVET"; //固定押金";
        //    }

        //    return "NO";
        //}


        //是否预收保底
        public static int IFPREPAY_TYPE_YES = 1;
        public static int IFPREPAY_TYPE_NO = 0;

        public static int[] GetIfPrepayStatus()
        {
            int[] IfPrepayStatus = new int[2];
            IfPrepayStatus[0] = IFPREPAY_TYPE_YES;
            IfPrepayStatus[1] = IFPREPAY_TYPE_NO;
            return IfPrepayStatus;
        }

        public static string GetIfPrepayStatusDesc(int IfPrepayStatus)
        {
            if (IfPrepayStatus == IFPREPAY_TYPE_YES)
            {
                return "IFPREPAY_TYPE_YES";
            }
            if (IfPrepayStatus == IFPREPAY_TYPE_NO)
            {
                return "IFPREPAY_TYPE_NO";
            }
            return "NO";
        }

        //结算周期FirstSetAcountMon
        //public static int FIRSTSETACOUNTMON_TYPE_ZERO = 0;
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
            int[] FirstSetAcountMonStatus = new int[12];
           // FirstSetAcountMonStatus[0] = FIRSTSETACOUNTMON_TYPE_ZERO;
            FirstSetAcountMonStatus[0] = FIRSTSETACOUNTMON_TYPE_ONE;
            FirstSetAcountMonStatus[1] = FIRSTSETACOUNTMON_TYPE_TWO;
            FirstSetAcountMonStatus[2] = FIRSTSETACOUNTMON_TYPE_THREE;
            FirstSetAcountMonStatus[3] = FIRSTSETACOUNTMON_TYPE_FOUR;
            FirstSetAcountMonStatus[4] = FIRSTSETACOUNTMON_TYPE_FIVE;
            FirstSetAcountMonStatus[5] = FIRSTSETACOUNTMON_TYPE_SIX;
            FirstSetAcountMonStatus[6] = FIRSTSETACOUNTMON_TYPE_SEVENT;
            FirstSetAcountMonStatus[7] = FIRSTSETACOUNTMON_TYPE_EIGHT;
            FirstSetAcountMonStatus[8] = FIRSTSETACOUNTMON_TYPE_NINE;
            FirstSetAcountMonStatus[9] = FIRSTSETACOUNTMON_TYPE_TEN;
            FirstSetAcountMonStatus[10] = FIRSTSETACOUNTMON_TYPE_ELEVEN;
            FirstSetAcountMonStatus[11] = FIRSTSETACOUNTMON_TYPE_TWELVE;
            return FirstSetAcountMonStatus;
        }

        public static string GetFirstSetAcountMonStatusDesc(int FirstSetAcountMonStatus)
        {
            //if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_ZERO)
            //{
            //    return "半月";
            //}
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_ONE)
            {
                return "1";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_TWO)
            {
                return "2";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_THREE)
            {
                return "3";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_FOUR)
            {
                return "4";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_FIVE)
            {
                return "5";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_SIX)
            {
                return "6";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_SEVENT)
            {
                return "7";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_EIGHT)
            {
                return "8";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_NINE)
            {
                return "9";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_TEN)
            {
                return "10";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_ELEVEN)
            {
                return "11";
            }
            if (FirstSetAcountMonStatus == FIRSTSETACOUNTMON_TYPE_TWELVE)
            {
                return "12";
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
                return "FAXTYPE_TYPE_INCRE";  //"增值税";
            }
            if (TaxTypeStatus == FAXTYPE_TYPE_COMM)
            {
                return "FAXTYPE_TYPE_COMM"; //普通发票";
            }
            if (TaxTypeStatus == FAXTYPE_TYPE_OTHER)
            {
                return "FAXTYPE_TYPE_OTHER"; // "其它";
            }
            return "NO";
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

        public int ContractID
        {
            get { return contractID; }
            set { contractID = value; }
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
    }
}
