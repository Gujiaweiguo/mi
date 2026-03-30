using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Lease.PotBargain
{
    public class ChargeType : BasePO
    {
        private int chargeTypeID;
        private string chargeTypeCode;
        private string chargeTypeName;
        private int chargeClass;
        private int isChargeCross;
        private string note;
        private string accountNumber;
        private int createUserId = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserId = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        //收费时间交叉标志　IsChargeCross
        public static int ISCHARGECROSS_YES = 1;
        public static int ISCHARGECROSS_NO = 0;

        public static int[] GetIsChargeCross()
        {
            int[] isChargeCross = new int[2];
            isChargeCross[0] = ISCHARGECROSS_NO;
            isChargeCross[1] = ISCHARGECROSS_YES;
            return isChargeCross;
        }

        public static string GetIsChargeCrossDesc(int isChargeCross)
        {
            if (isChargeCross == ISCHARGECROSS_NO)
            {
                return "ISCHARGECROSS_NO";// "不允许交叉";
            }
            if (isChargeCross == ISCHARGECROSS_YES)
            {
                return "ISCHARGECROSS_YES";//"允许交叉";
            }
            return "NO";
        }

        //费用分类 ChargeClass 
        public static int CHARGECLASS_LEASE = 1;
        public static int CHARGECLASS_DEPOSIT = 2;
        public static int CHARGECLASS_FANST = 3;
        public static int CHARGECLASS_APPORTION = 4;
        public static int CHARGECLASS_WATERORDLECT = 5;
        public static int CHARGECLASS_PREDICT = 6;
        public static int CHARGECLASS_MAINTAIN = 7;
        public static int CHARGECLASS_YEAREND = 8;
        public static int CHARGECLASS_OTHER = 9;
        public static int CHARGECLASS_UNION = 10;
        public static int CHARGECLASS_INTEREST = 11;
        public static int CHARGECLASS_UNION_INNERCARDRATE = 12;
        public static int CHARGECLASS_UNION_OUTERCARDRATE = 13;

        public static int[] GetChargeClass()
        {
            int[] chargeClass = new int[13];
            chargeClass[0] = CHARGECLASS_LEASE;
            chargeClass[1] = CHARGECLASS_DEPOSIT;
            chargeClass[2] = CHARGECLASS_FANST;
            chargeClass[3] = CHARGECLASS_APPORTION;
            chargeClass[4] = CHARGECLASS_WATERORDLECT;
            chargeClass[5] = CHARGECLASS_PREDICT;
            chargeClass[6] = CHARGECLASS_MAINTAIN;
            chargeClass[7] = CHARGECLASS_YEAREND;
            chargeClass[8] = CHARGECLASS_OTHER;
            chargeClass[9] = CHARGECLASS_UNION;
            chargeClass[10] = CHARGECLASS_INTEREST;
            chargeClass[11] = CHARGECLASS_UNION_INNERCARDRATE;
            chargeClass[12] = CHARGECLASS_UNION_OUTERCARDRATE;
            return chargeClass;
        }

        public static string GetChargeClassDesc(int chargeClass)
        {
            if (chargeClass == CHARGECLASS_LEASE)
            {
                return "CHARGECLASS_LEASE";//租金";
            }
            if (chargeClass == CHARGECLASS_DEPOSIT)
            {
                return "CHARGECLASS_DEPOSIT";// "押金";
            }
            if (chargeClass == CHARGECLASS_FANST)
            {
                return "CHARGECLASS_FANST";// "每月固定费用";
            }
            if (chargeClass == CHARGECLASS_APPORTION)
            {
                return "CHARGECLASS_APPORTION";// "分摊费用";
            }
            if (chargeClass == CHARGECLASS_WATERORDLECT)
            {
                return "CHARGECLASS_WATERORDLECT";// "计表费用(水电费)";
            }
            if (chargeClass == CHARGECLASS_PREDICT)
            {
                return "CHARGECLASS_PREDICT";//预付款";
            }
            if (chargeClass == CHARGECLASS_MAINTAIN)
            {
                return "CHARGECLASS_MAINTAIN";// "维修费";
            }
            if (chargeClass == CHARGECLASS_YEAREND)
            {
                return "CHARGECLASS_YEAREND";// "年终结算";
            }
            if (chargeClass == CHARGECLASS_OTHER)
            {
                return "CHARGECLASS_OTHER";// "其他费用";
            }
            if (chargeClass == CHARGECLASS_UNION)
            {
                return "CHARGECLASS_UNION";// "联营结算";
            }
            if (chargeClass == CHARGECLASS_INTEREST)
            {
                return "CHARGECLASS_INTEREST";// "滞纳金";
            }
            if (chargeClass == CHARGECLASS_UNION_INNERCARDRATE)
            {
                return "CHARGECLASS_UNION_INNERCARDRATE";// "联营内卡手续费";
            }
            if (chargeClass == CHARGECLASS_UNION_OUTERCARDRATE)
            {
                return "CHARGECLASS_UNION_OUTERCARDRATE";// "联营外卡手续费";
            }
            return "NO";
        }

        
        public override string GetTableName()
        {
            return "ChargeType";
        }

        public override string GetColumnNames()
        {
            return "ChargeTypeID,ChargeTypeCode,ChargeTypeName,ChargeClass,IsChargeCross,Note,AccountNumber";
        }

        public override string GetUpdateColumnNames()
        {
            return "ChargeTypeCode,ChargeTypeName,ChargeClass,IsChargeCross,Note,AccountNumber,ModifyUserId,ModifyTime";
        }

        public override string GetInsertColumnNames()
        {
            return "ChargeTypeID,ChargeTypeCode,ChargeTypeName,ChargeClass,IsChargeCross,Note,AccountNumber,CreateUserId,CreateTime";
        }

        public int ChargeTypeID
        {
            get {return chargeTypeID; }
            set {chargeTypeID = value; }
        }

        public string ChargeTypeCode
        {
            get { return chargeTypeCode; }
            set { chargeTypeCode = value; }
        }

        public string ChargeTypeName
        {
            get { return chargeTypeName; }
            set { chargeTypeName = value; }
        }

        public int ChargeClass
        {
            get { return chargeClass; }
            set { chargeClass = value; }
        }

        public int IsChargeCross
        {
            get { return isChargeCross; }
            set { isChargeCross = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public string AccountNumber
        {
            get { return accountNumber; }
            set { accountNumber = value; }
        }

        public int CreateUserId
        {
            set { createUserId = value; }
            get { return createUserId; }
        }
        public DateTime CreateTime
        {
            set { createTime = value; }
            get { return createTime; }
        }
        public int ModifyUserId
        {
            set { modifyUserId = value; }
            get { return modifyUserId; }
        }
        public DateTime ModifyTime
        {
            set { modifyTime = value; }
            get { return modifyTime; }
        }
        public int OprRoleID
        {
            set { oprRoleID = value; }
            get { return oprRoleID; }
        }
        public int OprDeptID
        {
            set { oprDeptID = value; }
            get { return oprDeptID; }
        }
    }
}
