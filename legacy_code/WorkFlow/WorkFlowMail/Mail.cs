using System;
using System.Collections.Generic;
using System.Text;
using jmail;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;

namespace WorkFlow.WorkFlowMail
{
    public class Mail
    {
        public string MailSMTP = "";//服务器
        public string MailSMTPUser = "";//发件人
        public string MailSMTPPassword = "";//发件人密码
        public string addressee = "";//收件人
        public string MailSubject = "";//邮件主题
        public string MailText = "";//邮件正文
        public string AttachFile = "";//附件
        public Mail()
        {

        }
        public Mail(string MailSMTP, string MailSMTPUser, string MailSMTPPassword)
        {
            this.MailSMTP = MailSMTP;
            this.MailSMTPUser = MailSMTPUser;
            this.MailSMTPPassword = MailSMTPPassword;
        }
    }
}
