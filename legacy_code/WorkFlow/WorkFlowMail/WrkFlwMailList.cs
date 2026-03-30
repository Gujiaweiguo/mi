using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace WorkFlow.WorkFlowMail
{
    public class WrkFlwMailList:BasePO
    {
        private int wrkFlwMailListID = 0;
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;
        private string mailSMTP = "";//服务器
        private string mailSMTPUser = "";//发件人
        private string mailSMTPPassword = "";//发件人密码
        public string addressee = "";//收件人地址
        public string userName = "";//收件人姓名
        public string subject = "";//邮件主题
        public string text = "";//邮件正文
        public string attachFile1 = "";//附件1
        public string attachFile2 = "";//附件2
        public string attachFile3 = "";//附件3 
        public int sendStatus = 0;

        public static int WorkFlowMailList_YES = 1;
        public static int WorkFlowMailList_NO = 0; 

        public override string GetTableName()
        {
            return "WrkFlwMailList";
        }
        public override string GetColumnNames()
        {
            return "WrkFlwMailListID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,MailSMTP,MailSMTPUser,MailSMTPPassword,Addressee,UserName,Subject,Text,AttachFile1,AttachFile2,AttachFile3,SendStatus";
        }
        public override string GetInsertColumnNames()
        {
            return "WrkFlwMailListID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,MailSMTP,MailSMTPUser,MailSMTPPassword,Addressee,UserName,Subject,Text,AttachFile1,AttachFile2,AttachFile3,SendStatus";
        }
        public override string GetUpdateColumnNames()
        {
            return "";
        }
        public int WrkFlwMailListID
        {
            set { this.wrkFlwMailListID = value; }
            get { return this.wrkFlwMailListID; }
        }
        public int CreateUserID
        {
            set { this.createUserID = value; }
            get { return createUserID; }
        }
        public DateTime CreateTime
        {
            set { this.createTime = value; }
            get { return this.createTime; }
        }
        public int ModifyUserID
        {
            set { this.modifyUserID = value; }
            get { return modifyUserID; }
        }
        public DateTime ModifyTime
        {
            set { this.modifyTime = value; }
            get { return modifyTime; }
        }
        public int OprRoleID
        {
            set { this.oprRoleID = value; }
            get { return oprRoleID; }
        }
        public int OprDeptID
        {
            set { this.oprDeptID = value; }
            get { return oprDeptID; }
        }
        public string MailSMTP
        {
            set { this.mailSMTP = value; }
            get { return this.mailSMTP; }
        }
        public string MailSMTPUser
        {
            set { this.mailSMTPUser = value; }
            get { return mailSMTPUser; }
        }
        public string MailSMTPPassword
        {
            set { this.mailSMTPPassword = value; }
            get { return mailSMTPPassword; }
        }
        public string Addressee
        {
            set { this.addressee = value; }
            get { return addressee; }
        }
        public string UserName
        {
            set { this.userName = value; }
            get { return userName; }
        }
        public string Subject
        {
            set { this.subject = value; }
            get { return subject; }
        }
        public string Text
        {
            set { this.text = value; }
            get { return text; }
        }
        public string AttachFile1
        {
            set { this.attachFile1 = value; }
            get { return attachFile1; }
        }
        public string AttachFile2
        {
            set { this.attachFile2 = value; }
            get { return attachFile2; }
        }
        public string AttachFile3
        {
            set { this.attachFile3 = value; }
            get { return attachFile3; }
        }
        public int SendStatus
        {
            set { this.sendStatus = value; }
            get { return sendStatus; }
        }
    }
}
