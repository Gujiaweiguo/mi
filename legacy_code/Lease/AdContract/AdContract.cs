using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.AdContract
{
    /// <summary>
    /// 广告位租用合同
    /// </summary>
    public class AdContract:BasePO
    {
        public static int ADCONTRACT_SETTLE_UP = 1;//一次付清
        public static int ADCONTRACT_MONTH = 2;//月付
        public static int ADCONTRACT_QUARTER = 3;//季付
        public static int ADCONTRACT_SEMIYEARLY = 4;//半年付
        public static int ADCONTRACT_YEAR = 5;//年付

        public static int[] GetPayingCycleStatus()
        {
            int[] payingCycleStatus = new int[5];
            payingCycleStatus[0] = ADCONTRACT_SETTLE_UP;
            payingCycleStatus[1] = ADCONTRACT_MONTH;
            payingCycleStatus[2] = ADCONTRACT_QUARTER;
            payingCycleStatus[3] = ADCONTRACT_SEMIYEARLY;
            payingCycleStatus[4] = ADCONTRACT_YEAR;
            return payingCycleStatus;
        }

        public static String GetPayingCycleStatusDesc(int payingCycleStatus)
        {
            if (payingCycleStatus == ADCONTRACT_SETTLE_UP)
            {
                return "ADCONTRACT_SETTLE_UP";
            }
            if (payingCycleStatus == ADCONTRACT_MONTH)
            {
                return "ADCONTRACT_MONTH";
            }
            if (payingCycleStatus == ADCONTRACT_QUARTER)
            {
                return "ADCONTRACT_QUARTER";
            }
            if (payingCycleStatus == ADCONTRACT_SEMIYEARLY)
            {
                return "ADCONTRACT_SEMIYEARLY";
            }
            if (payingCycleStatus == ADCONTRACT_YEAR)
            {
                return "ADCONTRACT_YEAR";
            }
            return "NO";
        }

        /// <summary>
        /// 广告位类型
        /// </summary>
        public static int ADTYPE_FAST = 1;   //固定广告位
        public static int ADTYPE_COMM = 2;

        public static int[] GetAdType()
        {
            int[] adType = new int[2];
            adType[0] = ADTYPE_FAST;
            adType[1] = ADTYPE_COMM;
            return adType;
        }

        public static string GetAdTypeDesc(int adType)
        {
            if (adType == ADTYPE_FAST)
                return "ADTYPE_FAST";
            if (adType == ADTYPE_COMM)
                return "ADTYPE_COMM";
            return "未知";
        }

        //合同类型
        public static int ADCONTRACTTYPE_ADBOARD = 1;   //广告
        public static int ADCONTRACTTYPE_AREATYPE = 2;  //场地

        //合同状态
        public static int ADCONTRACTSTATUS_TEMP = 0;       //草稿
        public static int ADCONTRACTSTATUS_NO_CHECK = 1;   //未审批
        public static int ADCONTRACTSTATUS_YES_CHECK = 2;  //未审批
        public static int ADCONTRACTSTATUS_NO_PASS = 3;    //驳回


        private int adContractID = 0;
        private int adBoardID = 0;
        private int custID = 0;
        private int areaTypeID = 0;
        private string adContractCode ="";
        private DateTime signDate = DateTime.Now;
        private DateTime startDate = DateTime.Now;
        private DateTime endDate = DateTime.Now;
        private int adType = 0;
        private string adDesc = "";
        private decimal totalMoney = 0;
        private decimal prepayment = 0;
        private int payingCycle = 0;
        private decimal paymentPerCycle = 0;
        private DateTime lastPaymentDate = DateTime.Now;
        private int adContractType = 0;
        private int adContractStatus = 0;
        private string adMaker = "";
        private string note = "";
        private string stoppingNote = "";

        public int AdContractID
        {
            get { return adContractID; }
            set { adContractID = value; }
        }

        public int AdBoardID
        {
            get { return adBoardID; }
            set { adBoardID = value; }
        }

        public int CustID
        {
            get { return custID; }
            set { custID = value; }
        }

        public int AreaTypeID
        {
            get { return areaTypeID; }
            set { areaTypeID = value; }
        }

        public string AdContractCode
        {
            get { return adContractCode; }
            set { adContractCode = value; }
        }

        public DateTime SignDate
        {
            get { return signDate; }
            set { signDate = value; }
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

        public int AdType
        {
            get { return adType; }
            set { adType = value; }
        }

        public string AdDesc
        {
            get { return adDesc; }
            set { adDesc = value; }
        }

        public decimal TotalMoney
        {
            get { return totalMoney; }
            set { totalMoney = value; }
        }

        public decimal Prepayment
        {
            get { return prepayment; }
            set { prepayment = value; }
        }

        public int PayingCycle
        {
            get { return payingCycle; }
            set { payingCycle = value; }
        }

        public decimal PaymentPerCycle
        {
            get { return paymentPerCycle; }
            set { paymentPerCycle = value; }
        }

        public DateTime LastPaymentDate
        {
            get { return lastPaymentDate; }
            set { lastPaymentDate = value; }
        }

        public int AdContractType
        {
            get { return adContractType; }
            set { adContractType = value; }
        }

        public int AdContractStatus
        {
            get { return adContractStatus; }
            set { adContractStatus = value; }
        }

        public string AdMaker
        {
            get { return adMaker; }
            set { adMaker = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public string StoppingNote
        {
            get { return stoppingNote; }
            set { stoppingNote = value; }
        }

        public override String GetTableName()
        {
            return "AdContract";
        }

        public override String GetColumnNames()
        {
            return "AdContractID,AdBoardID,CustID,AreaTypeID,AdContractCode,SignDate,StartDate,EndDate,AdType,AdDesc,TotalMoney,Prepayment," +
                    "PayingCycle,PaymentPerCycle,LastPaymentDate,AdContractType,AdContractStatus,AdMaker,Note,StoppingNote";
        }

        public override String GetInsertColumnNames()
        {
            return "AdContractID,AdBoardID,CustID,AdContractCode,StartDate,EndDate,AdType,AdDesc,TotalMoney,Prepayment," +
                    "PayingCycle,PaymentPerCycle,LastPaymentDate,AdContractType,AdContractStatus,AdMaker,Note";
        }

        public override String GetUpdateColumnNames()
        {
            return "AdBoardID,CustID,AdContractCode,StartDate,EndDate,AdType,AdDesc,TotalMoney,Prepayment," +
                   "PayingCycle,PaymentPerCycle,LastPaymentDate,AdMaker,Note";
        }
    }
}
