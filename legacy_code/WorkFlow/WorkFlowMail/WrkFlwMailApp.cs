using System;
using System.Collections.Generic;
using System.Text;
using jmail;
using System.Web.Mail;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Base.DB;
using Base.Biz;
namespace WorkFlow.WorkFlowMail
{
    public class WrkFlwMailApp
    {
        public void SendOneMail(WrkFlwMailList objWrkFlwMailList,BaseTrans objTrans)
        {
            BaseBO objBaseBo = new BaseBO();
            if (objTrans.Insert(objWrkFlwMailList) == -1)
            {
                objTrans.Rollback();
            }
        }
    }
}
