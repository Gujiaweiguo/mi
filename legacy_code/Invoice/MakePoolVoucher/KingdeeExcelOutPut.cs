using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using Base.Biz;
using Base.DB;
using Base;
namespace Invoice.MakePoolVoucher
{
    public class KingdeeExcelOutPut
    {
        private int accountParaID = 0;
        private DateTime beginDate = DateTime.Now;
        private DateTime endDate = DateTime.Now;
        private string customer = "";
        private string fYear = "";
        private string fPeriod = "";

        #region 共有属性
        public int AccountParaID
        {
            get { return accountParaID; }
            set { accountParaID = value; }
        }

        public DateTime BeginDate
        {
            get { return beginDate; }
            set { beginDate = value; }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        public string Customer
        {
            get { return customer; }
            set { customer = value; }
        }
        #endregion
        public KingdeeExcelOutPut()
        {
        }
        public KingdeeExcelOutPut(int accountParaID, DateTime beginDate, DateTime endDate, string customer, string fYear, string fPeriod)
        {
            this.accountParaID = accountParaID;
            this.beginDate = beginDate;
            this.endDate = endDate;
            this.customer = customer;
            this.fYear = fYear;
            this.fPeriod = fPeriod;
        }

        public Queue<KingdeeExcel> OutVoucher()
        {
            int j = 1;
            int x = 0;
            decimal FDebit = 0;
            decimal FCredit = 0;
            decimal SumFDebit = 0;
            decimal SumFCredit = 0;
            string itmeString = "";
            string customerString = "";
            KingdeeExcel kingdeeExcelend = null;

            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();
            Resultset rsAccountPara = new Resultset();
            AccountPara accountPara = new AccountPara();
            
            DataTable dt;
            DataTable dtItem;
            Queue<KingdeeExcel> kingdeeExcels = new Queue<KingdeeExcel>();

            string accountParaStr = "";
            int i = 0;
            /*查询凭证信息*/
            baseBO.WhereClause = "AccountParaID = " + accountParaID;
            rs = baseBO.Query(accountPara);

            if (rs.Count == 1)
            {
                accountPara = rs.Dequeue() as AccountPara;

                accountParaStr = accountPara.SQL;


                /*替换全部@item往来单位*/
                if(customer == "" || customer == null)
                {
                    customer = "";
                }
                accountParaStr = accountParaStr.Replace("@item", customer);
                accountParaStr = accountParaStr.Replace("@fromDate",beginDate.ToString());
                accountParaStr = accountParaStr.Replace("@endDate", endDate.ToString());

                /*查询合算总数*/
                dt = baseBO.QueryDataSet(accountParaStr).Tables[0];
                string item = "";
                string accountDesc = "";
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    baseBO.WhereClause = "PAccountParaID = " + accountParaID;
                    rsAccountPara = baseBO.Query(accountPara);

                    //KingdeeExcel[] kingdeeExcel = new KingdeeExcel[rsAccountPara.Count];

                    j = 1;
                    x = 0;
                    FDebit = 0;
                    FCredit = 0;
                    SumFDebit = 0;
                    SumFCredit = 0;
                    itmeString = "";
                    customerString ="";
                    kingdeeExcelend = null;
                    foreach (AccountPara accountParas in rsAccountPara)
                    {

                        //kingdeeExcel[j -1] = new KingdeeExcel();
                        
                        accountParaStr = accountParas.SQL;
                        if (accountParaStr != "none")
                        {
                            item = dt.Rows[i]["ITEM"].ToString();
                            if (item == "" || item == null || item.Length == 0)
                            {
                                item = "";
                            }
                            accountParaStr = accountParaStr.Replace("@item", item);
                            //accountParaStr = accountParaStr.Replace("@item", dt.Rows[i]["item"].ToString());
                            accountParaStr = accountParaStr.Replace("@fromDate", beginDate.ToString("yyyy-MM-dd"));
                            accountParaStr = accountParaStr.Replace("@endDate", endDate.ToString("yyyy-MM-dd"));

                            dtItem = baseBO.QueryDataSet(accountParaStr).Tables[0];
                            int xCount = dtItem.Rows.Count;
                            if (dtItem.Rows.Count > 0)
                            {
                                for (int l = 0; l < xCount; l++)
                                {
                                    if (Convert.ToDecimal(dtItem.Rows[l]["Amount"]) > 0)
                                    {
                                        KingdeeExcel kingdeeExcel = new KingdeeExcel();
                                        if (Convert.IsDBNull(dtItem.Rows[0]["Amount"]) != true)
                                        {
                                            kingdeeExcel.FDate = DateTime.Now.ToString("yyyy-MM-dd");
                                            kingdeeExcel.FYear = fYear;
                                            kingdeeExcel.FPeriod = fPeriod;
                                            kingdeeExcel.FGroupID = "'" + accountParas.AccountGp;
                                            kingdeeExcel.FNumber = i;
                                            kingdeeExcel.FAccountNum = "'" + accountParas.AccountNumber;
                                            kingdeeExcel.FAccountName = "'" + accountParas.AccountName;
                                            kingdeeExcel.FAmountFor = Convert.ToDecimal(dtItem.Rows[l]["Amount"]);
                                            if (accountParas.ParaType == AccountPara.ACCOUNTPARA_VOUCHER_DEBIT)
                                            {
                                                FDebit = Convert.ToDecimal(dtItem.Rows[l]["Amount"]);
                                                kingdeeExcel.FDebit = FDebit;
                                                kingdeeExcel.FCredit = 0;
                                                SumFDebit = SumFDebit + FDebit;
                                            }
                                            else if (accountParas.ParaType == AccountPara.ACCOUNTPARA_VOUCHER_LENDER)
                                            {
                                                FCredit = Convert.ToDecimal(dtItem.Rows[l]["Amount"]);
                                                kingdeeExcel.FCredit = FCredit;
                                                kingdeeExcel.FDebit = 0;
                                                SumFCredit = SumFCredit + FCredit;
                                            }
                                            
                                            
                                            //按约定初始化接口摘要日期
                                            accountDesc = accountParas.AccountDesc;
                                            accountDesc = accountDesc.Replace("YYYYMMDD-YYYYMMDD", beginDate.ToString("yyyyMMdd") + "-" + endDate.ToString("yyyyMMdd"));
                                            accountDesc = accountDesc.Replace("SYYYYMMDD", beginDate.ToString("yyyyMMdd"));
                                            accountDesc = accountDesc.Replace("EYYYYMMDD", endDate.ToString("yyyyMMdd"));
                                            accountDesc = accountDesc.Replace("SYYYYMM", beginDate.ToString("yyyyMM"));
                                            accountDesc = accountDesc.Replace("EYYYYMM", endDate.ToString("yyyyMM"));
                                            itmeString = dtItem.Rows[l]["itemcode"].ToString();
                                            accountDesc = accountDesc.Replace("ITEMCODE",itmeString );
                                            kingdeeExcel.FExplanation = "'" + accountDesc;
                                            kingdeeExcel.FEntryID = x;
                                            kingdeeExcel.FCurrencyNum = "'RMB";
                                            kingdeeExcel.FCurrencyName = "'人民币";
                                            if (accountParas.IsCustomer == Convert.ToString('Y'))
                                            {
                                                customerString = dtItem.Rows[l]["customer"].ToString();
                                                kingdeeExcel.FItem = "'" + customerString;
                                            }
                                            x++;

                                            kingdeeExcels.Enqueue(kingdeeExcel);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            kingdeeExcelend = new KingdeeExcel();
                            kingdeeExcelend.FDate = DateTime.Now.ToString("yyyy-MM-dd");
                            kingdeeExcelend.FYear = fYear;
                            kingdeeExcelend.FPeriod = fPeriod;
                            kingdeeExcelend.FGroupID = "'" + accountParas.AccountGp;
                            kingdeeExcelend.FNumber = i;
                            kingdeeExcelend.FAccountNum = "'" + accountParas.AccountNumber;
                            kingdeeExcelend.FAccountName = "'" + accountParas.AccountName;
                            if (accountParas.ParaType == AccountPara.ACCOUNTPARA_VOUCHER_DEBIT)
                            {
                                kingdeeExcelend.FDebit = 1;
                                kingdeeExcelend.FCredit = 0;
                            }
                            else if (accountParas.ParaType == AccountPara.ACCOUNTPARA_VOUCHER_LENDER)
                            {
                                kingdeeExcelend.FCredit = 1;
                                kingdeeExcelend.FDebit = 0;
                            }

                            //kingdeeExcelend.FEntryID = x;
                            kingdeeExcelend.FCurrencyNum = "'RMB";
                            kingdeeExcelend.FCurrencyName = "'人民币";
                            if (accountParas.IsCustomer == Convert.ToString('Y'))
                            {
                                kingdeeExcelend.FItem = "'" + customerString;
                            }
                            //按约定初始化接口摘要日期
                            accountDesc = accountParas.AccountDesc;
                            accountDesc = accountDesc.Replace("YYYYMMDD-YYYYMMDD", beginDate.ToString("yyyyMMdd") + "-" + endDate.ToString("yyyyMMdd"));
                            accountDesc = accountDesc.Replace("SYYYYMMDD", beginDate.ToString("yyyyMMdd"));
                            accountDesc = accountDesc.Replace("EYYYYMMDD", endDate.ToString("yyyyMMdd"));
                            accountDesc = accountDesc.Replace("SYYYYMM", beginDate.ToString("yyyyMM"));
                            accountDesc = accountDesc.Replace("EYYYYMM", endDate.ToString("yyyyMM"));
                            accountDesc = accountDesc.Replace("ITEMCODE", itmeString);
                            kingdeeExcelend.FExplanation = "'" + accountDesc;
                            //x++;
                        }
                    }

                    if (kingdeeExcelend != null)
                    {
                        if (SumFCredit != SumFDebit)
                        {
                            kingdeeExcelend.FEntryID = x;
                            if (SumFDebit > SumFCredit && kingdeeExcelend.FDebit == 1)
                            {
                                kingdeeExcelend.FDebit = SumFDebit - SumFCredit;
                                kingdeeExcelend.FAmountFor = kingdeeExcelend.FDebit;
                                kingdeeExcelend.FCredit = 0;
                            }
                            else if (SumFDebit < SumFCredit && kingdeeExcelend.FDebit == 1)
                            {
                                kingdeeExcelend.FDebit = SumFCredit - SumFDebit;
                                kingdeeExcelend.FAmountFor = kingdeeExcelend.FDebit;
                                kingdeeExcelend.FCredit = 0;
                            }
                            else if (SumFDebit > SumFCredit && kingdeeExcelend.FCredit == 1)
                            {
                                kingdeeExcelend.FCredit = SumFDebit - SumFCredit;
                                kingdeeExcelend.FAmountFor = kingdeeExcelend.FCredit;
                                kingdeeExcelend.FDebit = 0;
                            }
                            else if (SumFDebit < SumFCredit && kingdeeExcelend.FCredit == 1)
                            {
                                kingdeeExcelend.FCredit = SumFCredit - SumFDebit;
                                kingdeeExcelend.FAmountFor = kingdeeExcelend.FCredit;
                                kingdeeExcelend.FDebit = 0;
                            }
                        
                            kingdeeExcels.Enqueue(kingdeeExcelend);
                        }
                    }
                }
            }

            return kingdeeExcels;
        }


        public int OutDB()
        {
            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();
            Resultset rsAccountPara = new Resultset();
            AccountPara accountPara = new AccountPara();
            int accountID = 0;
            DataTable dt;
            DataTable dtItem;
            string accountParaStr = "";
            int i = 0;
            /*查询凭证信息*/
            baseBO.WhereClause = "AccountParaID = " + accountParaID;
            rs = baseBO.Query(accountPara);

            if (rs.Count == 1)
            {
                accountPara = rs.Dequeue() as AccountPara;
                accountParaStr = accountPara.SQL;

                /*替换全部@item往来单位*/
                if (customer == "" || customer == null)
                {
                    customer = "";
                }
                accountParaStr = accountParaStr.Replace("@item", customer);
                accountParaStr = accountParaStr.Replace("@fromDate", beginDate.ToString());
                accountParaStr = accountParaStr.Replace("@endDate", endDate.ToString());

                /*查询合算总数*/
                dt = baseBO.QueryDataSet(accountParaStr).Tables[0];
                AccountReport accountReport = new AccountReport();
                accountID = BaseApp.GetID(accountReport.GetTableName().ToString(), "AccountID");
                string item = "";
                string accountDesc = "";  //摘要
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    baseBO.WhereClause = "PAccountParaID = " + accountParaID;
                    rsAccountPara = baseBO.Query(accountPara);
                    foreach (AccountPara accountParas in rsAccountPara)
                    {
                        int paraType = accountParas.ParaType;
                        string accountNum = accountParas.AccountNumber;
                        string accountNm = accountParas.AccountName;
                        string itmeString = "";
                        accountParaStr = accountParas.SQL;
                        if (accountParaStr != "none")
                        {
                            item = dt.Rows[i]["ITEM"].ToString();
                            if (item == "" || item == null || item.Length == 0)
                            {
                                item = "";
                            }
                            accountParaStr = accountParaStr.Replace("@item", item);
                            accountParaStr = accountParaStr.Replace("@fromDate", beginDate.ToString("yyyy-MM-dd"));
                            accountParaStr = accountParaStr.Replace("@endDate", endDate.ToString("yyyy-MM-dd"));

                            dtItem = baseBO.QueryDataSet(accountParaStr).Tables[0];
                            int xCount = dtItem.Rows.Count;
                            if (dtItem.Rows.Count > 0)
                            {
                                for (int l = 0; l < xCount; l++)
                                {
                                    if (Convert.ToDecimal(dtItem.Rows[l]["Amount"]) > 0)
                                    {
                                        
                                        if (Convert.IsDBNull(dtItem.Rows[0]["Amount"]) != true)
                                        {
                                            accountReport = new AccountReport();
                                            accountReport.ParaType = paraType;
                                            accountReport.AccountNumber = accountNum;
                                            accountReport.AccountName = accountNm;
                                            accountReport.AccountID = accountID;
                                            accountReport.AccountReportID = BaseApp.GetID(accountReport.GetTableName().ToString(), "AccountReportID");
                                            if (paraType == accountReport.paratype_loan)  //借
                                            {
                                                accountReport.creditAmount = Convert.ToDecimal(dtItem.Rows[0]["Amount"]);
                                            }
                                            else
                                            { 
                                                accountReport.debitAmount=Convert.ToDecimal(dtItem.Rows[0]["Amount"]);                                            
                                            }

                                            //按约定初始化接口摘要日期
                                            accountDesc = accountParas.AccountDesc;
                                            accountDesc = accountDesc.Replace("YYYYMMDD-YYYYMMDD", beginDate.ToString("yyyyMMdd") + "-" + endDate.ToString("yyyyMMdd"));
                                            accountDesc = accountDesc.Replace("SYYYYMMDD", beginDate.ToString("yyyyMMdd"));
                                            accountDesc = accountDesc.Replace("EYYYYMMDD", endDate.ToString("yyyyMMdd"));
                                            accountDesc = accountDesc.Replace("SYYYYMM", beginDate.ToString("yyyyMM"));
                                            accountDesc = accountDesc.Replace("EYYYYMM", endDate.ToString("yyyyMM"));
                                            itmeString = dtItem.Rows[l]["itemcode"].ToString();
                                            accountDesc = accountDesc.Replace("ITEMCODE", itmeString);
                                            accountReport.AccountDesc =  accountDesc;
                                            baseBO.Insert(accountReport);
                                        }
                                    }
                                }
                            }
                        }
                       
                        //else
                        //{
                        //    kingdeeExcelend = new KingdeeExcel();
                        //    kingdeeExcelend.FDate = DateTime.Now.ToString("yyyy-MM-dd");
                        //    kingdeeExcelend.FYear = fYear;
                        //    kingdeeExcelend.FPeriod = fPeriod;
                        //    kingdeeExcelend.FGroupID = "'" + accountParas.AccountGp;
                        //    kingdeeExcelend.FNumber = i;
                        //    kingdeeExcelend.FAccountNum = "'" + accountParas.AccountNumber;
                        //    kingdeeExcelend.FAccountName = "'" + accountParas.AccountName;
                        //    if (accountParas.ParaType == AccountPara.ACCOUNTPARA_VOUCHER_DEBIT)
                        //    {
                        //        kingdeeExcelend.FDebit = 1;
                        //        kingdeeExcelend.FCredit = 0;
                        //    }
                        //    else if (accountParas.ParaType == AccountPara.ACCOUNTPARA_VOUCHER_LENDER)
                        //    {
                        //        kingdeeExcelend.FCredit = 1;
                        //        kingdeeExcelend.FDebit = 0;
                        //    }

                        //    //kingdeeExcelend.FEntryID = x;
                        //    kingdeeExcelend.FCurrencyNum = "'RMB";
                        //    kingdeeExcelend.FCurrencyName = "'人民币";
                        //    if (accountParas.IsCustomer == Convert.ToString('Y'))
                        //    {
                        //        kingdeeExcelend.FItem = "'" + customerString;
                        //    }
                        //    //按约定初始化接口摘要日期
                        //    accountDesc = accountParas.AccountDesc;
                        //    accountDesc = accountDesc.Replace("YYYYMMDD-YYYYMMDD", beginDate.ToString("yyyyMMdd") + "-" + endDate.ToString("yyyyMMdd"));
                        //    accountDesc = accountDesc.Replace("SYYYYMMDD", beginDate.ToString("yyyyMMdd"));
                        //    accountDesc = accountDesc.Replace("EYYYYMMDD", endDate.ToString("yyyyMMdd"));
                        //    accountDesc = accountDesc.Replace("SYYYYMM", beginDate.ToString("yyyyMM"));
                        //    accountDesc = accountDesc.Replace("EYYYYMM", endDate.ToString("yyyyMM"));
                        //    accountDesc = accountDesc.Replace("ITEMCODE", itmeString);
                        //    kingdeeExcelend.FExplanation = "'" + accountDesc;
                        //    //x++;
                        //}
                    }
                   

                }
            }

            return accountID;
        }
         
    }
}
