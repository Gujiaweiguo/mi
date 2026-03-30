using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.MakePoolVoucher
{
    public class AccountPara:BasePO
    {
        private int accountParaID = 0;
        private int paraType = 0;
        private int pAccountParaID = 0;
        private string accountGp = "";
        private string accountNumber = "";
        private string accountName = "";
        private string accountDesc = "";
        private string exNumber = "";
        private string isDept = "";
        private string isCustomer = "";
        private string sQL = "";
        private int ftypeId = 0;


        public int AccountParaID
        {
            get { return accountParaID; }
            set { accountParaID = value; }
        }

        public int ParaType
        {
            get { return paraType; }
            set { paraType = value; }
        }

        public int PAccountParaID
        {
            get { return pAccountParaID; }
            set { pAccountParaID = value; }
        }

        public string AccountGp
        {
            get { return accountGp; }
            set { accountGp = value; }
        }

        public string AccountNumber
        {
            get { return accountNumber; }
            set { accountNumber = value; }
        }

        public string AccountName
        {
            get { return accountName; }
            set { accountName = value; }
        }

        public string AccountDesc
        {
            get { return accountDesc; }
            set { accountDesc = value; }
        }

        public string ExNumber
        {
            get { return exNumber; }
            set { exNumber = value; }
        }

        public string IsDept
        {
            get { return isDept; }
            set { isDept = value; }
        }

        public string IsCustomer
        {
            get { return isCustomer; }
            set { isCustomer = value; }
        }

        public string SQL
        {
            get { return sQL; }
            set { sQL = value; }
        }

        public int FinancialTypeId
        {
            get { return ftypeId; }
            set { ftypeId = value; }
        }

        /// <summary>
        /// 凭证头
        /// </summary>
        public static int ACCOUNTPARA_VOUCHER_TITLE = 0;
        /// <summary>
        /// 借方科目
        /// </summary>
        public static int ACCOUNTPARA_VOUCHER_DEBIT = 1;
        /// <summary>
        /// 贷方科目
        /// </summary>
        public static int ACCOUNTPARA_VOUCHER_LENDER = 2;

        public override string GetTableName()
        {
            return "AccountPara";
        }

        public override string GetColumnNames()
        {
            return "AccountParaID,ParaType,PAccountParaID,AccountGp,AccountNumber,AccountName,AccountDesc,ExNumber,IsDept,IsCustomer,SQL,FinancialTypeId";
        }

        public override string GetInsertColumnNames()
        {
            return "AccountParaID,ParaType,PAccountParaID,AccountGp,AccountNumber,AccountName,AccountDesc,ExNumber,IsDept,IsCustomer,SQL,FinancialTypeId";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

    }
}
