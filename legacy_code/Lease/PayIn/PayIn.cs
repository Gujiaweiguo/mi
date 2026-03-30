using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Lease.PayIn
{
    /// <summary>
    /// 代收款信息
    /// </summary>
    public class PayIn : BasePO
    {
        private int payInID = 0;  //代收款ID
        private int shopID = 0;  //商铺ID
        private int createUserID = 0;  //创建用户代码
        private DateTime createTime = DateTime.Now;  //创建时间
        private int modifyUserID = 0;  //最后修改用户代码
        private DateTime modifyTime = DateTime.Now;  //最后修改时间
        private int oprRoleID = 0;  //操作用户的角色代码
        private int oprDeptID = 0;  //操作用户的机构代码
        private string payInCode = null;   //代收款单号
        private DateTime payInPeriod = DateTime.Now;  //代收款记账月
        private int payInType = 0;  //代收款类型
        private DateTime payInStartDate = DateTime.Now;  //代收款费用发生开始日期
        private DateTime payInEndDate = DateTime.Now;  //代收款费用结束日期
        private DateTime payInDate = DateTime.Now;  //代收款日期
        private decimal payInAmt = 0;  //代收款金额
        private decimal payOutAmtSum = 0;  //累计返还金额
        private int payInStatus = 0;  //代收款状态
        private int payInDataSource = 0;  //数据来源
        private decimal paidAmt = 0;

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

        //代收款类型PayInType
        public static int PAYINTYPE_TEMPPUNIN = 1;  //临时代收款
        public static int PAYINTYPE_BANKCARD = 2;   //银行卡代收款
        public static int PAYINTYPE_OTHER = 3;      //其他代收款

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

        //代收款状态 PayInStatus
        public static int PAYINSTATRS_NOINV = 1;  //未结算
        public static int PAYINSTATRS_HALFINV = 2; //部分结算
        public static int PAYINSTATRS_YESINV = 3; //已结算
        public static int PAYINSTATRS_CEL = 4;  //取消

        public static int[] GetPayInStatus()
        {
            int[] payInStatus = new int[4];
            payInStatus[0] = PAYINSTATRS_NOINV;
            payInStatus[1] = PAYINSTATRS_HALFINV;
            payInStatus[2] = PAYINSTATRS_YESINV;
            payInStatus[3] = PAYINSTATRS_CEL;
            return payInStatus;
        }

        public static string GetPayInStatusDesc(int payinStatus)
        {
            if (payinStatus == PAYINSTATRS_NOINV)
            {
                return "PayInput_Non_Settlement";
            }
            if (payinStatus == PAYINSTATRS_HALFINV)
            {
                return "PayInput_PartialSettlement";
            }
            if (payinStatus == PAYINSTATRS_YESINV)
            {
                return "PayInput_Settlement";
            }
            if (payinStatus == PAYINSTATRS_CEL)
            {
                return "PayInput_Cancel";
            }
            return "Unbeknown";
        }

        //数据来源 PayInDataSource
        public static int PAYINDATASOURCE_SYS = 1;  //系统自动生成
        public static int PAYINDATASOURCE_HAND = 2;   //手工录入

        public static int[] GetPayInDataSource()
        {
            int[] payInDataSource = new int[2];
            payInDataSource[0] = PAYINDATASOURCE_SYS;
            payInDataSource[1] = PAYINDATASOURCE_HAND;
            return payInDataSource;
        }

        public static string GetPayInDataSourceDesc(int payinDataSource)
        {
            if (payinDataSource == PAYINDATASOURCE_SYS)
            {
                return "PayInput_SystemGenerate";
            }
            if (payinDataSource == PAYINDATASOURCE_HAND)
            {
                return "PayInput_ManualEntry";
            }
            return "Unbeknown";
        }

        public override string GetTableName()
        {
            return "PayIn";
        }

        public override string GetColumnNames()
        {
            return "PayInID,ShopID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,PayInCode,PayInPeriod,PayInType,PayInStartDate,PayInEndDate,PayInDate,PayInAmt,PayOutAmtSum,PayInStatus,PayInDataSource,'' as PayInStatusName,PaidAmt";
        }

        public override string GetUpdateColumnNames()
        {
            return "ModifyUserID,ModifyTime,PayInStartDate,PayInEndDate,PayInDate,PayInAmt,PayInStatus,PaidAmt";
        }
        public override string GetInsertColumnNames()
        {
            return "PayInID,ShopID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,PayInCode,PayInPeriod,PayInType,PayInStartDate,PayInEndDate,PayInDate,PayInAmt,PayOutAmtSum,PayInStatus,PayInDataSource";
        }
    }


}
