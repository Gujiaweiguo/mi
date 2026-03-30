using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.AdContract
{
    public class AreaContract:BasePO
    {
        private int adContractID = 0;
        private int custID = 0;
        private int areaTypeID = 0;
        private string adContractCode = "";
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
            return "AdContractID,AreaTypeID,CustID,AreaTypeID,AdContractCode,SignDate,StartDate,EndDate,AdType,AdDesc,TotalMoney,Prepayment," +
                    "PayingCycle,PaymentPerCycle,LastPaymentDate,AdContractType,AdContractStatus,AdMaker,Note,StoppingNote";
        }

        public override String GetInsertColumnNames()
        {
            return "AdContractID,AreaTypeID,CustID,AdContractCode,StartDate,EndDate,AdType,AdDesc,TotalMoney,Prepayment," +
                    "PayingCycle,PaymentPerCycle,LastPaymentDate,AdContractType,AdContractStatus,AdMaker,Note";
        }

        public override String GetUpdateColumnNames()
        {
            return "AreaTypeID,CustID,AdContractCode,StartDate,EndDate,AdType,AdDesc,TotalMoney,Prepayment," +
                   "PayingCycle,PaymentPerCycle,LastPaymentDate,AdMaker,Note";
        }
    }
}
