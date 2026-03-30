using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Sell
{
    /// <summary>
    /// 交易表头
    /// </summary>
    public class TransHeader: BasePO
    {
        private decimal transId = 0;
        private string tenantId = "";
        private string posId = "";
        private DateTime bizDate = DateTime.Now;
        private string batchId = "";
        private string receiptId = "";
        private string shiftNo = "";
        private DateTime transDate = DateTime.Now;
        private string userId = "";
        private string mReceiptId = "";
        private string rReceiptId = "";
        private string reason = "";
        private string salesMan = "";
        private int guestCount = 0;
        private string training = "";
        private string tranStatus = "";
        private DateTime entryAt = DateTime.Now;
        private string entryBy = "";

        public decimal TransId
        {
            get { return transId; }
            set { transId = value; }
        }

        public string TenantId
        {
            get { return tenantId; }
            set { tenantId = value; }
        }

        public string PosId
        {
            get { return posId; }
            set { posId = value; }
        }

        public DateTime BizDate
        {
            get { return bizDate; }
            set { bizDate = value; }
        }

        public string BatchId
        {
            get { return batchId; }
            set { batchId = value; }
        }

        public string ReceiptId
        {
            get { return receiptId; }
            set { receiptId = value; }
        }

        public string ShiftNo
        {
            get { return shiftNo; }
            set { shiftNo = value; }
        }

        public DateTime TransDate
        {
            get { return transDate; }
            set { transDate = value; }
        }

        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public string MReceiptId
        {
            get { return mReceiptId; }
            set { mReceiptId = value; }
        }

        public string RReceiptId
        {
            get { return rReceiptId; }
            set { rReceiptId = value; }
        }

        public string Reason
        {
            get { return reason; }
            set { reason = value; }
        }

        public string SalesMan
        {
            get { return salesMan; }
            set { salesMan = value; }
        }


        public int GuestCount
        {
            get { return guestCount; }
            set { guestCount = value; }
        }

        public string Training
        {
            get { return training; }
            set { training = value; }
        }

        public string TranStatus
        {
            get { return tranStatus; }
            set { tranStatus = value; }
        }

        public DateTime EntryAt
        {
            get { return entryAt; }
            set { entryAt = value; }
        }

        public string EntryBy
        {
            get { return entryBy; }
            set { entryBy = value; }
        }
 
        public static int TRANSTYPE_CONSUME = 1;  //消费交易
        public static int TRANSTYPE_BACK = 2;  //退货交易

        public static int DATASOURCE_POS = 1;  //POS系统
        public static int DATASOURCE_FILE = 2; //文件导入
        public static int DATASOURCE_WORK = 3; //手工录入

        public override string GetTableName()
        {
            return "TransHeader";
        }

        public override string GetColumnNames()
        {
            return "TransId,TenantId,PosId,BizDate,BatchId,ReceiptId,ShiftNo,TransDate,UserId,MReceiptId,RReceiptId,Reason,SalesMan,GuestCount,Training,TranStatus,EntryAt,EntryBy";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }
        public override string GetInsertColumnNames()
        {
            return "TransId,TenantId,PosId,BizDate,BatchId,ReceiptId,ShiftNo,TransDate,UserId,MReceiptId,RReceiptId,Reason,SalesMan,GuestCount,Training,TranStatus,EntryAt,EntryBy";
        }
    }
}
