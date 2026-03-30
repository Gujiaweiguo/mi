using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Invoice
{
    /// <summary>
    /// 结算单主表
    /// </summary>
    public class InvoiceHeader:BasePO
    {
        private int invID = 0;
        private int custID = 0;
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;
        private int curTypeID = 1;
        private int contractID = 0;
        private string invCode = "";
        private string custName = "";
        private DateTime invDate = DateTime.Now;  //结算单生成日
        private DateTime invPeriod = DateTime.Now; //结算单记账月
        private int invStatus = 1; //结算单状态
        private int invType = 1; //结算类型
        private int isFirst = 1; //是否首期
        private int invCurTypeID = 1; //结算币种
        private decimal invExRate = 0; //结算汇率
        private decimal invPayAmt = 0; //结算金额
        private decimal invPayAmtL = 0; //结算本币金额
        private decimal invAdjAmt = 0; //调整金额
        private decimal invAdjAmtL = 0; //调整本币金额
        private decimal invDiscAmt = 0; //优惠金额
        private decimal invDiscAmtL = 0; //优惠本币金额
        private decimal invChngAmt = 0; //其它变动金额
        private decimal invChngAmtL = 0; //其它变动本币金额
        private decimal invActPayAmt = 0; //实际应结金额
        private decimal invActPayAmtL = 0; //实际应结本币金额
        private decimal invPaidAmt = 0; //已结金额
        private decimal invPaidAmtL = 0; //已结本币金额
        private int printFlag = 0;
        private string note = ""; //备注
        private string bancthID = "";

        public static int INVSTATUS_VALID = 1;   //有效（未结算）
        public static int INVSTATUS_PART_CLOSING = 2;  //部分结算
        public static int INVSTATUS_CLOSING = 3;  //已结算
        public static int INVSTATUS_CANCEL = 4;   //取消

        //结算类型
        public static int INVTYPE_LEASE = 1;   //租赁
        public static int INVTYPE_UNION = 2;   //联营
        public static int INVTYPE_LEASE_AD = 3;   //租赁广告
        public static int INVTYPE_LEASE_AREA = 4;   //租赁场地

        public int InvID
        {
            get { return invID; }
            set { invID = value; }
        }

        public int CustID
        {
            get { return custID; }
            set { custID = value; }
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

        public int ContractID
        {
            get { return contractID; }
            set { contractID = value; }
        }

        public int CurTypeID
        {
            get { return curTypeID; }
            set { curTypeID = value; }
        }

        public string InvCode
        {
            get { return invCode; }
            set { invCode = value; }
        }

        public string CustName
        {
            get { return custName; }
            set { custName = value; }
        }

        public DateTime InvDate
        {
            get { return invDate; }
            set { invDate = value; }
        }

        public DateTime InvPeriod
        {
            get { return invPeriod; }
            set { invPeriod = value; }
        }

        public int InvStatus
        {
            get { return invStatus; }
            set { invStatus = value; }
        }

        public int InvType
        {
            get { return invType; }
            set { invType = value; }
        }

        public int IsFirst
        {
            get { return isFirst; }
            set { isFirst = value; }
        }

        public int InvCurTypeID
        {
            get { return invCurTypeID; }
            set { invCurTypeID = value; }
        }

        public decimal InvExRate
        {
            get { return invExRate; }
            set { invExRate = value; }
        }

        public decimal InvPayAmt
        {
            get { return invPayAmt; }
            set { invPayAmt = value; }
        }

        public decimal InvPayAmtL
        {
            get { return invPayAmtL; }
            set { invPayAmtL = value; }
        }

        public decimal InvAdjAmt
        {
            get { return invAdjAmt; }
            set { invAdjAmt = value; }
        }

        public decimal InvAdjAmtL
        {
            get { return invAdjAmtL; }
            set { invAdjAmtL = value; }
        }

        public decimal InvDiscAmt
        {
            get { return invDiscAmt; }
            set { invDiscAmt = value; }
        }

        public decimal InvDiscAmtL
        {
            get { return invDiscAmtL; }
            set { invDiscAmtL = value; }
        }

        public decimal InvChngAmt
        {
            get { return invChngAmt; }
            set { invChngAmt = value; }
        }

        public decimal InvChngAmtL
        {
            get { return invChngAmtL; }
            set { invChngAmtL = value; }
        }

        public decimal InvActPayAmt
        {
            get { return invActPayAmt; }
            set { invActPayAmt = value; }
        }

        public decimal InvActPayAmtL
        {
            get { return invActPayAmtL; }
            set { invActPayAmtL = value; }
        }

        public decimal InvPaidAmt
        {
            get { return invPaidAmt; }
            set { invPaidAmt = value; }
        }

        public decimal InvPaidAmtL
        {
            get { return invPaidAmtL; }
            set { invPaidAmtL = value; }
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

        public string BancthID
        {
            get { return bancthID; }
            set { bancthID = value; }
        }


        public override string GetTableName()
        {
            return "InvoiceHeader";
        }

        public override string GetColumnNames()
        {
            return "InvID,CurTypeID,ContractID,InvCode,CustName,InvDate,InvPeriod,InvStatus,InvType,IsFirst,InvCurTypeID,InvExRate,InvPayAmt,InvPayAmtL," +
                    "InvAdjAmt,InvAdjAmtL,InvDiscAmt,InvDiscAmtL,InvChngAmt,InvChngAmtL,InvActPayAmt,InvActPayAmtL,InvPaidAmt,InvPaidAmtL,Note,BancthID";
        }

        public override string GetInsertColumnNames()
        {
            return "InvID,CustID,ContractID,CurTypeID,CreateUserID,CreateTime,OprRoleID,OprDeptID,InvCode,CustName,InvDate,InvPeriod,InvStatus,InvType,IsFirst,InvCurTypeID,InvExRate,InvPayAmt,InvPayAmtL," +
                   "InvAdjAmt,InvAdjAmtL,InvDiscAmt,InvDiscAmtL,InvChngAmt,InvChngAmtL,InvActPayAmt,InvActPayAmtL,InvPaidAmt,InvPaidAmtL,PrintFlag,Note,BancthID";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

        //是否首其
        public static int ISFIRST_NO = 0;
        public static int ISFIRST_YES = 1;

        //结算单状态 InvStatus
        public static int INVSTATUS_NOINV = 1; //有效(未结算)
        public static int INVSTATUS_HALFINV = 2; //部分结算
        public static int INVSTATUS_INV = 3; //已结算
        public static int INVSTATUS_CEL = 4; //取消

        public static int[] GetInvStatus()
        {
            int[] invStatus = new int[4];
            invStatus[0] = INVSTATUS_NOINV;
            invStatus[1] = INVSTATUS_HALFINV;
            invStatus[2] = INVSTATUS_INV;
            invStatus[3] = INVSTATUS_CEL;
            return invStatus;
        }

        public static string GetInvStatusDesc(int invStatus)
        {
            if (invStatus == INVSTATUS_NOINV)
            {
                return "INVSTATUS_NOINV";
            }
            if (invStatus == INVSTATUS_HALFINV)
            {
                return "INVSTATUS_HALFINV";
            }
            if (invStatus == INVSTATUS_INV)
            {
                return "INVSTATUS_INV";
            }
            if (invStatus == INVSTATUS_CEL)
            {
                return "INVSTATUS_CEL";
            }
            return "NO";
        }
    }
}
