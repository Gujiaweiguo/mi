using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.AdContract
{
    /// <summary>
    /// 广告位
    /// </summary>
    public class AdBoard:BasePO
    {
        public static int BLANKOUT_STATUS_INVALID = 0;     // 未出租
        public static int BLANKOUT_STATUS_LEASEOUT = 1;    // 已出租
        public static int BLANKOUT_STATUS_VALID = 2;       // 已作废
       
        //是否作废
        public static int[] GetBlankOutStatus()
        {
            int[] blankOutStaus = new int[3];
            blankOutStaus[0] = BLANKOUT_STATUS_INVALID;
            blankOutStaus[1] = BLANKOUT_STATUS_LEASEOUT;
            blankOutStaus[2] = BLANKOUT_STATUS_VALID;
            return blankOutStaus;
        }

        public static String GetBlankOutStatusDesc(int blankOutStaus)
        {
            if (blankOutStaus == BLANKOUT_STATUS_INVALID)
            {
                return "RentableArea_NoLeaseOut";
            }
            if (blankOutStaus == BLANKOUT_STATUS_LEASEOUT)
            {
                return "Unit_NoLeaseOut";
            }
            if (blankOutStaus == BLANKOUT_STATUS_VALID)
            {
                return "Unit_Nonrentable";
            }
            return "未知";
        }



        private int adBoardID = 0;
        private string adBoardCode = "";
        private string adBoardName = "";
        private int adBoardStatus = 0; 

        public int AdBoardID
        {
            get { return adBoardID; }
            set { adBoardID = value; }
        }
        public string AdBoardCode
        {
            get { return adBoardCode; }
            set { adBoardCode = value; }
        }
        public string AdBoardName
        {
            get { return adBoardName; }
            set { adBoardName = value; }
        }
        public int AdBoardStatus
        {
            get { return adBoardStatus; }
            set { adBoardStatus = value; }
        }

        public override String GetTableName()
        {
            return "AdBoard";
        }

        public override String GetColumnNames()
        {
            return "AdBoardID,AdBoardCode,AdBoardName,AdBoardStatus";
        }

        public override String GetInsertColumnNames()
        {
            return "AdBoardID,AdBoardCode,AdBoardName,AdBoardStatus";
        }

        public override String GetUpdateColumnNames()
        {
            return "AdBoardCode,AdBoardName,AdBoardStatus";
        }
    }
}
