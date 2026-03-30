using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    public class SurBal:BasePO
    {
        private int surBalID = 0;
        private int custID = 0;
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;
        private int surBalType = 0;
        private int surBalInvID = 0;
        private decimal surBalAmt = 0;
        private decimal surBalAmtL = 0;
        private DateTime surBalDate = DateTime.Now;
        private int surBalStatus = 0;
        private string note = "";

        //余款处理方式
        public static int SURBAL_BACKING_OUT_SHOP = 1;//返还商铺
        public static int SURBAL_BUCKLE_MONEY = 2;    //扣款
        public static int SURBAL_MORTAGAGE = 3;       //诋付

        public static int[] GetSurBalType()
        {
            int[] surBalType = new int[2];
            surBalType[0] = SURBAL_BACKING_OUT_SHOP;
            surBalType[1] = SURBAL_BUCKLE_MONEY;
            return surBalType;
        }

        public static string GetSurBalTypeDesc(int surBalType)
        {
            if (surBalType == SURBAL_BACKING_OUT_SHOP)
            {
                return "PayInput_Return";
            }
            if (surBalType == SURBAL_BUCKLE_MONEY)
            {
                return "PayInput_DeductExpenses";
            }
            return "未知";
        }

        public static int SURBAL_UP_TO_SNUFF = 1;     //正常
        public static int SURBAL_CANCEL = 2;          //取消

        public override string GetTableName()
        {
            return "SurBal";
        }

        public override string GetColumnNames()
        {
            return "SurBalID,CustID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,SurBalType,SurBalInvID,SurBalAmt,SurBalAmtL,SurBalDate,SurBalStatus,Note";
        }

        public override string GetInsertColumnNames()
        {
            return "SurBalID,CustID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,SurBalType,SurBalInvID,SurBalAmt,SurBalAmtL,SurBalDate,SurBalStatus,Note";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public int SurBalID
        {
            get { return surBalID; }
            set { surBalID = value; }
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

        public int SurBalType
        {
            get { return surBalType; }
            set { surBalType = value; }
        }

        public int SurBalInvID
        {
            get { return surBalInvID; }
            set { surBalInvID = value; }
        }

        public decimal SurBalAmt
        {
            get { return surBalAmt; }
            set { surBalAmt = value; }
        }

        public decimal SurBalAmtL
        {
            get { return surBalAmtL; }
            set { surBalAmtL = value; }
        }

        public DateTime SurBalDate
        {
            get { return surBalDate; }
            set { surBalDate = value; }
        }

        public int SurBalStatus
        {
            get { return surBalStatus; }
            set { surBalStatus = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }
    }
}
