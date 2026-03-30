using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.PotBargain
{
    public class PayType : BasePO
    {
        private int paytypeid = 0;
        private string paytypecode = "";
        private string paytypename = "";
        private int paytypestatus = 0;
        private string node = "";

        public static int ISPAYTYPESTATUS_YES = 1;
        public static int ISPAYTYPESTATUS_NO = 0;

        public static int[] GetIsPayType()
        {
            int[] isPayType = new int[2];
            isPayType[0] = ISPAYTYPESTATUS_YES;
            isPayType[1] = ISPAYTYPESTATUS_NO;
            return isPayType;
        }

        public static string GetIsPayTypeDesc(int isPayType)
        {
            if (isPayType == ISPAYTYPESTATUS_NO)
            {
                return "BizGrp_NO";
            }
            if (isPayType == ISPAYTYPESTATUS_YES)
            {
                return "BizGrp_YES";
            }
            return "Memb_MembCardNoUse";
        }

        public override String GetTableName()
        {
            return "PayType";
        }
        public override String GetColumnNames()
        {
            return "PayTypeID,PayTypeCode,PayTypeName,PayTypeStatus,Node";
        }
        public override String GetUpdateColumnNames()
        {
            return "PayTypeCode,PayTypeName,PayTypeStatus,Node";
        }
        public override string GetInsertColumnNames()
        {
            return "PayTypeID,PayTypeCode,PayTypeName,PayTypeStatus,Node";
        }
        public int PayTypeID
        {
            get { return paytypeid; }
            set { paytypeid = value; }
        }
        public string PayTypeCode
        {
            get { return paytypecode; }
            set { paytypecode = value; }
        }
        public string PayTypeName
        {
            get { return paytypename; }
            set { paytypename = value; }
        }
        public int PayTypeStatus
        {
            get { return paytypestatus; }
            set { paytypestatus = value; }
        }
        public string Node
        {
            get { return node; }
            set { node = value; }
        }
    }
}