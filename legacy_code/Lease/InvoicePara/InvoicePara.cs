using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Lease.InvoicePara
{
    public class InvoicePara:BasePO
    {
        private int invoiceParaID = 0;
        private int createUserID = 0;  //创建用户代码
        private DateTime createTime = DateTime.Now;  //创建时间
        private int modifyUserID = 0;  //最后修改用户代码
        private DateTime modifyTime = DateTime.Now;  //最后修改时间
        private int oprRoleID = 0;  //操作用户的角色代码
        private int oprDeptID = 0;  //操作用户的机构代码
        private string invoiceParaDesc = "";
        private int paraStatus = 0;
        private string invHeader = "";
        private string invSubhead = "";
        private string invH1 = "";
        private string invH2 = "";
        private string invH3 = "";
        private string invH4 = "";
        private string invH5 = "";
        private string invF1 = "";
        private string invF2 = "";
        private string invF3 = "";
        private string invF4 = "";
        private string invF5 = "";
        private string invF6 = "";
        private string invF7 = "";
        private int isDefault = 0;
        private string paraStatusName = "";
        private int subsid = 0;
        private string subsName = "";

        /*有效*/
        public static int INVOICEPARA_STATUS_YES = 1;
        /*无效*/
        public static int INVOICEPARA_STATUS_NO = 0;

        public static int[] GetInvoiceParaStatus()
        {
            int[] invoiceParaStatus = new int[2];
            invoiceParaStatus[0] = INVOICEPARA_STATUS_YES;
            invoiceParaStatus[1] = INVOICEPARA_STATUS_NO;
            return invoiceParaStatus;
        }

        public static String GetInvoiceParaStatusDesc(int invoiceParaStatus)
        {
            if (invoiceParaStatus == INVOICEPARA_STATUS_NO)
            {
                return "WrkFlw_Disabled";
            }
            if (invoiceParaStatus == INVOICEPARA_STATUS_YES)
            {
                return "WrkFlw_Enabled";
            }
            return "NULL";
        }

        public String InvoiceParaStatusDesc
        {
            get { return GetInvoiceParaStatusDesc(this.ParaStatus); }
        }

        public override string GetTableName()
        {
            return "InvoicePara";
        }

        public override string GetColumnNames()
        {
            return "InvoiceParaID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,InvoiceParaDesc,ParaStatus,IsDefault,ParaStatusName,InvHeader,InvSubhead,InvH1,InvH2,InvH3,InvH4,InvH5,InvF1,InvF2,InvF3,InvF4,InvF5,InvF6,InvF7,SubsID,SubsName";
        }

        public override string GetUpdateColumnNames()
        {
            return "ModifyUserID,ModifyTime,InvoiceParaDesc,ParaStatus,IsDefault,InvHeader,InvSubhead,InvH1,InvH2,InvH3,InvH4,InvH5,InvF1,InvF2,InvF3,InvF4,InvF5,InvF6,InvF7,SubsID";
        }

        public override string GetInsertColumnNames()
        {
            return "InvoiceParaID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,InvoiceParaDesc,ParaStatus,IsDefault,InvHeader,InvSubhead,InvH1,InvH2,InvH3,InvH4,InvH5,InvF1,InvF2,InvF3,InvF4,InvF5,InvF6,InvF7,SubsID";
        }

        public override string GetQuerySql()
        {
            return "Select InvoiceParaID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,InvoiceParaDesc,ParaStatus,IsDefault,'' as ParaStatusName,InvHeader,InvSubhead,InvH1,InvH2,InvH3,InvH4,InvH5,InvF1,InvF2,InvF3,InvF4,InvF5,InvF6,InvF7,SubsID,(select SubsName from Subsidiary where SubsID = InvoicePara.SubsID) as SubsName From InvoicePara";
        }

        public int InvoiceParaID
        {
            get { return invoiceParaID; }
            set { invoiceParaID = value; }
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

        public string InvoiceParaDesc
        {
            get { return invoiceParaDesc; }
            set { invoiceParaDesc = value; }
        }

        public int ParaStatus
        {
            get { return paraStatus; }
            set { paraStatus = value; }
        }

        public string InvHeader
        {
            get { return invHeader; }
            set { invHeader = value; }
        }

        public string InvSubhead
        {
            get { return invSubhead; }
            set { invSubhead = value; }
        }

        public string InvH1
        {
            get { return invH1; }
            set { invH1 = value; }
        }

        public string InvH2
        {
            get { return invH2; }
            set { invH2 = value; }
        }

        public string InvH3
        {
            get { return invH3; }
            set { invH3 = value; }
        }

        public string InvH4
        {
            get { return invH4; }
            set { invH4 = value; }
        }

        public string InvH5
        {
            get { return invH5; }
            set { invH5 = value; }
        }

        public string InvF1
        {
            get { return invF1; }
            set { invF1 = value; }
        }

        public string InvF2
        {
            get { return invF2; }
            set { invF2 = value; }
        }

        public string InvF3
        {
            get { return invF3; }
            set { invF3 = value; }
        }

        public string InvF4
        {
            get { return invF4; }
            set { invF4 = value; }
        }

        public string InvF5
        {
            get { return invF5; }
            set { invF5 = value; }
        }

        public string InvF6
        {
            get { return invF6; }
            set { invF6 = value; }
        }

        public string InvF7
        {
            get { return invF7; }
            set { invF7 = value; }
        }

        public int IsDefault
        {
            get { return isDefault; }
            set { isDefault = value; }
        }

        public string ParaStatusName
        {
            get { return paraStatusName; }
            set { paraStatusName = value; }
        }
        public int SubsID
        {
            set { subsid = value; }
            get { return subsid; }
        }
        public string SubsName
        {
            set { subsName = value; }
            get { return subsName; }
        }
    }
}
