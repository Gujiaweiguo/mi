using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Lease.PayIn
{
    /// <summary>
    /// 代收款信息
    /// </summary>
    public class PayOut : BasePO
    {
        private int payOutID = 0;  //代收款返还ID
        private int payInID = 0;  //代收款ID
        private int createUserID = 0;  //创建用户代码
        private DateTime createTime = DateTime.Now;  //创建时间
        private int modifyUserID = 0;  //最后修改用户代码
        private DateTime modifyTime = DateTime.Now;  //最后修改时间
        private int oprRoleID = 0;  //操作用户的角色代码
        private int oprDeptID = 0;  //操作用户的机构代码
        private decimal payOutAmt = 0;  //返还金额
        private DateTime payOutDate = DateTime.Now;   //返还日期
        private int invPayID = 0;  //结算付款单ID
        private int payOutType = 0;  //返还类型
        private int payOutStatus = 0;  //返还状态
        private string note = null;  //备注

        public int PayOutID
        {
            get { return payOutID; }
            set { payOutID = value; }
        }

        public int PayInID
        {
            get { return payInID; }
            set { payInID = value; }
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

        public decimal PayOutAmt
        {
            get { return payOutAmt; }
            set { payOutAmt = value; }
        }

        public DateTime PayOutDate
        {
            get { return payOutDate; }
            set { payOutDate = value; }
        }

        public int InvPayID
        {
            get { return invPayID; }
            set { invPayID = value; }
        }

        public int PayOutType
        {
            get { return payOutType; }
            set { payOutType = value; }
        }

        public int PayOutStatus
        {
            get { return payOutStatus; }
            set { payOutStatus = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        //返还类型 PayOutType
        public static int PAYOUTTYPE_PAYOUT = 1;  //返还代收款
        public static int PAYOUTTYPE_DUDECT = 2;   //抵扣费用

        public static int[] GetPayOutType()
        {
            int[] payOutType = new int[2];
            payOutType[0] = PAYOUTTYPE_PAYOUT;
            payOutType[1] = PAYOUTTYPE_DUDECT;
            return payOutType;
        }

        public static string GetPayOutTypeDesc(int payoutType)
        {
            if (payoutType == PAYOUTTYPE_PAYOUT)
            {
                return "PayInput_Return";
            }
            if (payoutType == PAYOUTTYPE_DUDECT)
            {
                return "PayInput_OffsetPayment";
            }
            return "Unbeknown";
        }

        //返还状态 PayOutStatus
        public static int PAYOUTSTATUS_YES = 1;  //正常
        public static int PAYOUTSTATUS_CEL = 2;   //取消

        public static int[] GetPayOutStatus()
        {
            int[] payOutStatus = new int[2];
            payOutStatus[0] = PAYOUTSTATUS_YES;
            payOutStatus[1] = PAYOUTSTATUS_CEL;
            return payOutStatus;
        }

        public static string GetPayOutStatusDesc(int payoutStatus)
        {
            if (payoutStatus == PAYOUTSTATUS_YES)
            {
                return "正常";
            }
            if (payoutStatus == PAYOUTSTATUS_CEL)
            {
                return "取消";
            }
            return "未知";
        }

        public override string GetTableName()
        {
            return "PayOut";
        }

        public override string GetColumnNames()
        {
            return "PayOutID,PayInID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,PayOutAmt,PayOutDate,InvPayID,PayOutType,PayOutStatus,Note";
        }

        public override string GetUpdateColumnNames()
        {
            return "PayOutID,PayInID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,PayOutAmt,PayOutDate,InvPayID,PayOutType,PayOutStatus,Note";
        }
        public override string GetInsertColumnNames()
        {
            return "PayOutID,PayInID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,PayOutAmt,PayOutDate,PayOutType,PayOutStatus";
        }
    }
}
