using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    public class DepBal:BasePO
    {
        private int depBalID = 0;
        private int custID = 0;
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;
        private int depBalCurID = 0;
        private decimal depBalExRate = 0;
        private decimal depBalAmt = 0;
        private decimal depBalAmtL = 0;
        private int depBalType = 0;
        private int invPayID = 0;
        private string note = "";

        //押金处理方式
        public static int DEPBAL_BACKING_OUT_SHOP = 1;//返还商铺
        public static int DEPBAL_BUCKLE_MONEY = 2;    //扣款
        public static int DEPBAL_MORTAGAGE = 3;       //诋付

        public static int[] GetDepBalType()
        {
            int[] depBalType = new int[2];
            depBalType[0] = DEPBAL_BACKING_OUT_SHOP;
            depBalType[1] = DEPBAL_BUCKLE_MONEY;
            return depBalType;
        }

        public static string GetDepBalTypeDesc(int depBalType)
        {
            if (depBalType == DEPBAL_BACKING_OUT_SHOP)
            {
                return "PayInput_Return";
            }
            if (depBalType == DEPBAL_BUCKLE_MONEY)
            {
                return "PayInput_DeductExpenses";
            }
            return "Unbeknown";
        }



        public override string GetTableName()
        {
            return "DepBal";
        }

        public override string GetColumnNames()
        {
            return "DepBalID,CustID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,DepBalCurID,DepBalExRate,DepBalAmt,DepBalAmtL,DepBalType,Note";
        }

        public override string GetInsertColumnNames()
        {
            return "DepBalID,CustID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,DepBalCurID,DepBalExRate,DepBalAmt,DepBalAmtL,DepBalType,Note";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }


        public int DepBalID
        {
            get { return depBalID; }
            set { depBalID = value; }
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

        public int DepBalCurID
        {
            get { return depBalCurID; }
            set { depBalCurID = value; }
        }

        public decimal DepBalExRate
        {
            get { return depBalExRate; }
            set { depBalExRate = value; }
        }

        public decimal DepBalAmt
        {
            get { return depBalAmt; }
            set { depBalAmt = value; }
        }

        public decimal DepBalAmtL
        {
            get { return depBalAmtL; }
            set { depBalAmtL = value; }
        }

        public int DepBalType
        {
            get { return depBalType; }
            set { depBalType = value; }
        }

        public int InvPayID
        {
            get { return invPayID; }
            set { invPayID = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }
    }
}
