using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    public class PayIn:BasePO
    {
        private int payInID = 0;
        private int shopID = 0;
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;
        private string payInCode = "";
        private DateTime payInPeriod = DateTime.Now;
        private int payInType = 0;
        private DateTime payInStartDate = DateTime.Now;
        private DateTime payInEndDate = DateTime.Now;
        private DateTime payInDate = DateTime.Now;
        private decimal payInAmt = 0;
        private decimal payOutAmtSum = 0;
        private int payInStatus = 0;
        private int payInDataSource = 0;
        private decimal paidAmt = 0;
        private decimal commRate = 0;


        public static int PAYIN_NO_BALANCE_IN_HAND = 1;//未结算
        public static int PAYIN_PART_BALANCE_IN_HAND = 2;//部分结算
        public static int PAYIN_BALANCE_IN_HAND = 3;//已结算
        public static int PAYIN_CANCEL = 4;//取消

        //public static int PAYINTYPE_TEMPORARILY = 1;//临时代收款
        //public static int PAYINTYPE_BANKCARD = 2;//银行卡代收款
        //public static int PAYINTYPE_OTHER = 3;//其他代收款

        public static int PAYINDATASOURCE_STSTEM = 1;//系统生成
        public static int PAYINDATASOURCE_HANDCRAFT = 2;//手工生成

        //代收款类型PayInType
        public static int PAYINTYPE_TEMPPUNIN = 3;  //临时代收款
        public static int PAYINTYPE_BANKCARD = 4;   //银行卡代收款
        public static int PAYINTYPE_OTHER = 5;      //其他代收款

        public static int[] GetPayInType()
        {
            int[] payInType = new int[3];
            payInType[0] = PAYINTYPE_TEMPPUNIN;
            payInType[1] = PAYINTYPE_BANKCARD;
            payInType[2] = PAYINTYPE_OTHER;
            return payInType;
        }

        public static string GetPayInTypeDesc(int payinType)
        {
            if (payinType == PAYINTYPE_TEMPPUNIN)
            {
                return "PayInput_Cash";
            }
            if (payinType == PAYINTYPE_BANKCARD)
            {
                return "PayInput_BankCard";
            }
            if (payinType == PAYINTYPE_OTHER)
            {
                return "PayInput_Others";
            }
            return "Unbeknown";
        }

        public override string GetTableName()
        {
            return "PayIn";
        }

        public override string GetColumnNames()
        {
            return "PayInID,ShopID,PayInAmt,PayOutAmtSum,PaidAmt,CommRate";
        }

        public override string GetInsertColumnNames()
        {
            return "PayInID,ShopID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,PayInCode,PayInPeriod,PayInType," +
                    "PayInStartDate,PayInEndDate,PayInDate,PayInAmt,PayOutAmtSum,PayInStatus,PayInDataSource,PaidAmt,CommRate";
        }

        public override string GetUpdateColumnNames()
        {
            return "ModifyUserID,ModifyTime,PayOutAmtSum,PayInStatus,PaidAmt,CommRate";
        }

        public override string GetQuerySql()
        {
            return "select PayInID,b.ShopID,PayInAmt,PayOutAmtSum,PaidAmt,CommRate from Contract a,ConShop b,PayIn c";
        }

        public int PayInID
        {
            get { return payInID; }
            set { payInID = value; }
        }

        public int ShopID
        {
            get { return shopID; }
            set { shopID = value; }
        }

        public int CreateUserID
        {
            get { return createUserID; }
            set { createUserID = value; }
        }

        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        public int ModifyUserID
        {
            get { return modifyUserID; }
            set { modifyUserID = value; }
        }

        public DateTime ModifyTime
        {
            get { return modifyTime; }
            set { modifyTime = value; }
        }

        public int OprRoleID
        {
            get { return oprRoleID; }
            set { oprRoleID = value; }
        }

        public int OprDeptID
        {
            get { return oprDeptID; }
            set { oprDeptID = value; }
        }

        public string PayInCode
        {
            get { return payInCode; }
            set { payInCode = value; }
        }

        public DateTime PayInPeriod
        {
            get { return payInPeriod; }
            set { payInPeriod = value; }
        }

        public int PayInType
        {
            get { return payInType; }
            set { payInType = value; }
        }

        public DateTime PayInStartDate
        {
            get { return payInStartDate; }
            set { payInStartDate = value; }
        }

        public DateTime PayInEndDate
        {
            get { return payInEndDate; }
            set { payInEndDate = value; }
        }

        public DateTime PayInDate
        {
            get { return payInDate; }
            set { payInDate = value; }
        }

        public decimal PayInAmt
        {
            get { return payInAmt; }
            set { payInAmt = value; }
        }

        public decimal PayOutAmtSum
        {
            get { return payOutAmtSum; }
            set { payOutAmtSum = value; }
        }

        public int PayInStatus
        {
            get { return payInStatus; }
            set { payInStatus = value; }
        }

        public int PayInDataSource
        {
            get { return payInDataSource; }
            set { payInDataSource = value; }
        }

        public decimal PaidAmt
        {
            get { return paidAmt; }
            set { paidAmt = value; }
        }

        public decimal CommRate
        {
            get { return commRate; }
            set { commRate = value; }
        }

    }
}
