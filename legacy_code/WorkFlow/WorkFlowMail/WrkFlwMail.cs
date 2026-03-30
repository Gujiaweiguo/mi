using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace WorkFlow.WorkFlowMail
{
    public class WrkFlwMail:BasePO
    {
        private int wrkFlwMailID = 0;
        private int bizGrpID = 0;//¹¤×÷×éID
        private int createUserId = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;              
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;                 
        private int oprDeptID = 0;
        private string mailSubject = "";
        private string mailText = "";
        private int mailStatus = 0;
        private int pbizgrpID = 0;

        public static int WorkFlowMail_YES=1;
        public static int WorkFlowMail_NO=0;


        public override String GetTableName()
        {
            return "WrkFlwMail";
        }

        public override string GetInsertColumnNames()
        {
            return "WrkFlwMailID,CreateUserId,CreateTime,OprRoleID,OprDeptID,MailSubject,MailStatus,BizGrpID,MailText";
        }

        public override String GetColumnNames()
        {
            return "WrkFlwMailID,CreateUserId,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,MailSubject,MailText,MailStatus,PBizGrpID,BizGrpID";
        }
        public override String GetUpdateColumnNames()
        {
            return "WrkFlwMailID,CreateUserId,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,MailSubject,MailText,MailStatus,BizGrpID";
        }

        public override string GetQuerySql()
        {
            return @"select WrkFlwMailID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,MailSubject,MailText,MailStatus,PBizGrpID,BizGrpID from (select  bizgrpid as WrkFlwMailID,'' as CreateUserID,'' as CreateTime,'' as ModifyUserID,'' as ModifyTime,'' as OprRoleID,'' as OprDeptID,bizgrpname as MailSubject, '' as MailText,'1' as MailStatus ,'100' as PBizGrpID,BizGrpID from bizgrp 
union all
select  WrkFlwMailID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,MailSubject,MailText,MailStatus,bizgrpid as PBizGrpID,BizGrpID from WrkFlwMail) a ";    
        }

        public int WrkFlwMailID
        {
            set { wrkFlwMailID = value; }
            get { return wrkFlwMailID; }
        }
        public int BizGrpID
        {
            set { bizGrpID = value; }
            get { return bizGrpID; }
        }
        public int CreateUserId
        {
            get { return createUserId; }
            set { createUserId = value; }
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

        public string MailSubject
        {
            get { return mailSubject; }
            set { mailSubject = value; }
        }

        public string MailText
        {
            get { return mailText; }
            set { mailText = value; }
        }

        public int MailStatus
        {
            get { return mailStatus; }
            set { mailStatus = value; }
        }
        public int PBizGrpID
        {
            set { this.pbizgrpID = value; }
            get { return pbizgrpID; }
        }
    }
}
