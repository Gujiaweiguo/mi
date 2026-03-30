using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Generalize.Medium
{
    public class Contents:BasePO
    {
        private int contentsID = 0;
        private string contentsNm = "";
        private int contentsStatus = 0;
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        /*有效*/
        public static int CONTENTS_STATUS_YES = 1;

        /*无效*/
        public static int CONTENTS_STATUS_NO = 0;


        public override string GetTableName()
        {
            return "Contents";
        }

        public override string GetColumnNames()
        {
            return "ContentsID,ContentsNm,ContentsStatus,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetInsertColumnNames()
        {
            return "ContentsID,ContentsNm,ContentsStatus,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetUpdateColumnNames()
        {
            return "ContentsNm,ContentsStatus,ModifyUserID,ModifyTime";
        }

        public int ContentsID
        {
            get { return contentsID; }
            set { contentsID = value; }
        }

        public string ContentsNm
        {
            get { return contentsNm; }
            set { contentsNm = value; }
        }

        public int  ContentsStatus
        {
            get { return contentsStatus; }
            set { contentsStatus = value; }
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
