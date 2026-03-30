using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Lease.PayIn
{
    /// <summary>
    /// 修改代收款信息中的累计返还金额
    /// </summary>
    public class UpdatePayOutAmtSum:BasePO
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

        public override string GetTableName()
        {
            return "PayIn";
        }

        public override string GetColumnNames()
        {
            return "PayInID,ShopID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,PayInCode,PayInPeriod,PayInType,PayInStartDate,PayInEndDate,PayInDate,PayInAmt,PayOutAmtSum,PayInStatus,PayInDataSource,'' as PayInStatusName";
        }

        public override string GetUpdateColumnNames()
        {
            return "PayInStatus,PayOutAmtSum";
        }
        public override string GetInsertColumnNames()
        {
            return "PayInID,ShopID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,PayInCode,PayInPeriod,PayInType,PayInStartDate,PayInEndDate,PayInDate,PayInAmt,PayOutAmtSum,PayInStatus,PayInDataSource";
        }
    }
}
