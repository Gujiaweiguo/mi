using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Invoice
{
    /// <summary>
    /// 费用单
    /// </summary>
    public class Charge:BasePO
    {
        private int chgID = 0;
        private int shopID = 0;
        private DateTime printDate = DateTime.Now;
        private int chgStatus = 0;
        private int printFlag = 0;
        private string note = "";
        private string invCode = "0";
        private int rangeCode = 0;
        public int ChgID
        {
            get { return chgID; }
            set { chgID = value; }
        }

        public int ShopID
        {
            get { return shopID; }
            set { shopID = value; }
        }

        public DateTime PrintDate
        {
            get { return printDate; }
            set { printDate = value; }
        }

        public int ChgStatus
        {
            get { return chgStatus; }
            set { chgStatus = value; }
        }

        public int PrintFlag
        {
            get { return printFlag; }
            set { printFlag = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public string InvCode
        {
            get { return invCode; }
            set { invCode = value; }
        }
        public int RangeCode
        {
            set { rangeCode = value; }
            get { return rangeCode; }
        }

        //费用单状态
        public static int CHGSTATUS_TYPE_TEMP = 1;  //草搞状态
        public static int CHGSTATUS_TYPE_INGEAR = 2; //待审批
        public static int CHGSTATUS_TYPE_ATTREM = 3; //审批通过
        public static int CHGSTATUS_TYPE_END = 4;  //作废

        public override string GetTableName()
        {
            return "Charge";
        }

        public override string GetColumnNames()
        {
            return "ChgID,ShopID,PrintDate,ChgStatus,PrintFlag,Note,InvCode,RangeCode";
        }

        public override string GetInsertColumnNames()
        {
            return "ChgID,ShopID,PrintDate,ChgStatus,PrintFlag,Note,InvCode,RangeCode";
        }

        public override string GetUpdateColumnNames()
        {
            return "ShopID,PrintDate,ChgStatus,PrintFlag,Note,InvCode";
        }
    }
}
