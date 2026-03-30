using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Generalize.Medium
{
    public class PrintSize:BasePO
    {
        private int printSizeID = 0;
        private string printSizeNm = "";
        private int printSizeStatus = 0;
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        /*有效*/
        public static int PRINTSIZE_STATUS_YES = 1;

        /*无效*/
        public static int PRINTSIZE_STATUS_NO = 0;


        public override string GetTableName()
        {
            return "PrintSize";
        }

        public override string GetColumnNames()
        {
            return "PrintSizeID,PrintSizeNm,PrintSizeStatus,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetInsertColumnNames()
        {
            return "PrintSizeID,PrintSizeNm,PrintSizeStatus,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetUpdateColumnNames()
        {
            return "PrintSizeNm,PrintSizeStatus,ModifyUserID,ModifyTime";
        }

        public int PrintSizeID
        {
            get { return printSizeID; }
            set { printSizeID = value; }
        }

        public string PrintSizeNm
        {
            get { return printSizeNm; }
            set { printSizeNm = value; }
        }

        public int PrintSizeStatus
        {
            get { return printSizeStatus; }
            set { printSizeStatus = value; }
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
    }
}
