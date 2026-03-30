using System;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

using Invoice.MakePoolVoucher;
using Base.Biz;
using Base.DB;
using Base.Page;
using Lease.Subs;
using Invoice;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

public partial class Invoice_MakePoolVoucher_MakePoolVoucher : BasePage
{
    public string baseInfo = "";
    public string publicMes_DateError = "";
    public string beginEndDate = "";
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();
            int year = 0;

            baseBO.WhereClause = "SubsStatus=1";
            rs = baseBO.Query(new Subs());
            if (rs.Count >= 1)
            {
                foreach (Subs subsRs in rs)
                {
                    cmbSubs.Items.Add(new ListItem(subsRs.SubsName,subsRs.FinancialTypeID.ToString()));
                }
            }
            //凭证名称根据子公司绑定
            baseBO.WhereClause = "ParaType = " + AccountPara.ACCOUNTPARA_VOUCHER_TITLE + " and FinancialTypeID=" + Convert.ToInt32(this.cmbSubs.SelectedValue);
            rs = baseBO.Query(new AccountPara());
            if (rs.Count >= 1)
            {
                foreach (AccountPara accountPara in rs)
                {
                    cmbAccountParaID.Items.Add(new ListItem(accountPara.AccountName, accountPara.AccountParaID.ToString()));
                }
            }
            year = Convert.ToInt32(DateTime.Now.Year) - 10;

            for (int i = year; i < (year + 20); i++)
            {
                cmbFYear.Items.Add(new ListItem(i.ToString(),i.ToString()));
            }

            cmbFYear.SelectedValue = DateTime.Now.Year.ToString();

            
            for (int i=1; i <= 12; i++)
            {
                cmbFPeriod.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            cmbFPeriod.SelectedValue = DateTime.Now.Month.ToString();

            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "MakePoolVoucher_lblRentalInt");
            publicMes_DateError = (String)GetGlobalResourceObject("BaseInfo", "PublicMes_DateError");
            beginEndDate = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDateTime");
            btnQuery.Attributes.Add("onclick", "return InputValidator(form1)");
            strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh"); 
        }
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        //KillPross("EXCEL");
        if (this.cmbAccountParaID.Items.Count<=0)
        {
            lblMsg.Text = (String)GetGlobalResourceObject("BaseInfo", "MakePoolVoucher_NoPara");
            return;
        }

        KingdeeExcelOutPut KingdeeExcelOutPut = new KingdeeExcelOutPut(Convert.ToInt32(cmbAccountParaID.SelectedValue), Convert.ToDateTime(txtBeginDate.Text), Convert.ToDateTime(txtEndDate.Text), txtCustomer.Text.Trim(), cmbFYear.SelectedValue, cmbFPeriod.SelectedValue);

        lock (KingdeeExcelOutPut)
        {
           int ret= KingdeeExcelOutPut.OutDB();
           string str_sql = "select * from accountreport where accountid=" + ret.ToString();

           ParameterFields paraFields = new ParameterFields();
           ParameterField[] paraField = new ParameterField[2];
           ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[2];
           ParameterRangeValue rangeValue = new ParameterRangeValue();

           paraField[0] = new ParameterField();
           paraField[0].Name = "REXTitle";
           discreteValue[0] = new ParameterDiscreteValue();
           discreteValue[0].Value = "财务凭证";
           paraField[0].CurrentValues.Add(discreteValue[0]);

           paraField[1] = new ParameterField();
           paraField[1].Name = "REXMallTitle";
           discreteValue[1] = new ParameterDiscreteValue();
           discreteValue[1].Value = Session["MallTitle"].ToString();
           paraField[1].CurrentValues.Add(discreteValue[1]);


           foreach (ParameterField pf in paraField)
           {
               paraFields.Add(pf);
           }
           Session["paraFil"] = paraFields;
           Session["sql"] = str_sql;
           Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\RptAccountReport.rpt";
           this.Response.Redirect("../../ReportM/ReportShow.aspx");
        }


    }



    private void OutExcel(Queue<KingdeeExcel> KingdeeExcels)
    {
        if (KingdeeExcels.Count > 0)
        {
            string filePath = Server.MapPath("~/凭证模板.xls");//路径
            string tempPath = Server.MapPath("~/TempExcel/");
            string newPath = tempPath + cmbAccountParaID.SelectedItem.Text + DateTime.Now.ToLongDateString() + ".xls";
            string fileName = cmbAccountParaID.SelectedItem.Text + DateTime.Now.ToLongDateString() + ".xls";//客户端保存的文件名

            System.IO.FileInfo file = new System.IO.FileInfo(newPath);
            
            if (file.Exists)
            {
                file.Delete();
            }
            //FileStream fs = file.Create();
            //StreamWriter sw = file.CreateText();
            if (!Directory.Exists(tempPath))//判断文件夹是否存在，若不存在，则创建
            {
                //创建文件夹
                Directory.CreateDirectory(tempPath);//创建文件夹 
            }

            Excel.Application ThisApplication = new Excel.ApplicationClass();
            Excel.Workbook ThisWorkBook;
            object missing = System.Reflection.Missing.Value;
            try
            {
                //加载Excel模板文件
                ThisWorkBook = ThisApplication.Workbooks.Open(filePath, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
                Excel.Worksheet oExcel = (Excel.Worksheet)ThisWorkBook.Sheets[1];
                int i = 2;
                foreach (KingdeeExcel kingdeeExcel in KingdeeExcels)
                {
                    oExcel.Cells[i, 1] = kingdeeExcel.FDate;
                    oExcel.Cells[i, 2] = kingdeeExcel.FYear;
                    oExcel.Cells[i, 3] = kingdeeExcel.FPeriod;
                    oExcel.Cells[i, 4] = kingdeeExcel.FGroupID;
                    oExcel.Cells[i, 5] = kingdeeExcel.FNumber;
                    oExcel.Cells[i, 6] = kingdeeExcel.FAccountNum;
                    oExcel.Cells[i, 7] = kingdeeExcel.FAccountName;
                    oExcel.Cells[i, 8] = kingdeeExcel.FCurrencyNum;
                    oExcel.Cells[i, 9] = kingdeeExcel.FCurrencyName;
                    oExcel.Cells[i, 10] = kingdeeExcel.FAmountFor;
                    oExcel.Cells[i, 11] = kingdeeExcel.FDebit;
                    oExcel.Cells[i, 12] = kingdeeExcel.FCredit;
                    oExcel.Cells[i, 13] = kingdeeExcel.FPreparerID;
                    oExcel.Cells[i, 14] = kingdeeExcel.FCheckerID;
                    oExcel.Cells[i, 15] = kingdeeExcel.FApproveID;
                    oExcel.Cells[i, 16] = kingdeeExcel.FCashierID;
                    oExcel.Cells[i, 17] = kingdeeExcel.FHandler;
                    oExcel.Cells[i, 18] = kingdeeExcel.FSettleTypeID;
                    oExcel.Cells[i, 19] = kingdeeExcel.FSettleNo;
                    oExcel.Cells[i, 20] = kingdeeExcel.FExplanation;
                    oExcel.Cells[i, 21] = kingdeeExcel.FQuantity;
                    oExcel.Cells[i, 22] = kingdeeExcel.FMeasureUnitID;
                    oExcel.Cells[i, 23] = kingdeeExcel.FUnitPrice;
                    oExcel.Cells[i, 24] = kingdeeExcel.FReference;
                    oExcel.Cells[i, 25] = kingdeeExcel.FTransDate;
                    oExcel.Cells[i, 26] = kingdeeExcel.FTransNo;
                    oExcel.Cells[i, 27] = kingdeeExcel.FAttachments;
                    oExcel.Cells[i, 28] = kingdeeExcel.FSerialNum;
                    oExcel.Cells[i, 29] = kingdeeExcel.FObjectName;
                    oExcel.Cells[i, 30] = kingdeeExcel.FParameter;
                    oExcel.Cells[i, 31] = kingdeeExcel.FExchangeRate;
                    oExcel.Cells[i, 32] = kingdeeExcel.FEntryID;
                    oExcel.Cells[i, 33] = kingdeeExcel.FItem;
                    oExcel.Cells[i, 34] = kingdeeExcel.FPosted;
                    oExcel.Cells[i, 35] = kingdeeExcel.FInternalInd;
                    oExcel.Cells[i, 36] = kingdeeExcel.FCashFlow;

                    i++;
                }
                 
                 ThisApplication.Visible = false;
                 //更新数据后另存为新文件
                 oExcel.SaveAs(newPath, missing, missing, missing, missing, missing, missing, missing, missing, missing);

                 ThisWorkBook.Close(false, missing, missing);
                 ThisApplication.Quit();
                 //ThisWorkBook = null;
                 ThisApplication = null;

                 System.GC.Collect();
                 
                
                


                 //fs.Close();
                 FileInfo aFile = new FileInfo(newPath);
                 string na = Path.GetFileName(newPath);
                 Response.Clear();
                 Response.ClearHeaders();
                 Response.BufferOutput = false;
                 Response.ContentType = "application/octet-stream";
                 Response.AppendHeader("Content-disposition", "inline;filename=" + HttpUtility.UrlEncode(na, System.Text.Encoding.UTF8));
                 Response.AddHeader("Content-Length", aFile.Length.ToString());
                 Response.WriteFile(newPath,false);
                 Response.Flush();
                 //Response.End();
                 
                File.Delete(newPath);
                
                //KillPross("EXCEL");
            }
            catch(Exception ex)
            {
                throw ex;
            }         
        }
    }

    private void KillPross(string name)
    {
        try
        {
            Process[] myProcesses = Process.GetProcesses();
            foreach(Process myProcess in myProcesses)
            {
                if (name == myProcess.ProcessName)
                {
                    myProcess.Kill();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void cmbSubs_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = new Resultset();
        this.cmbAccountParaID.Items.Clear();
        if (this.cmbSubs.SelectedValue.ToString() != "-1")
        {
            baseBo.WhereClause = "ParaType = " + AccountPara.ACCOUNTPARA_VOUCHER_TITLE + " and FinancialTypeID=" + Convert.ToInt32(this.cmbSubs.SelectedValue);
            rs = baseBo.Query(new AccountPara());
            if (rs.Count >= 1)
            {
                foreach (AccountPara accountPara in rs)
                {
                    this.cmbAccountParaID.Items.Add(new ListItem(accountPara.AccountName, accountPara.AccountParaID.ToString()));
                }
            }
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (this.cmbAccountParaID.Items.Count <= 0)
        {
            lblMsg.Text = (String)GetGlobalResourceObject("BaseInfo", "MakePoolVoucher_NoPara");
            return;
        }

        KingdeeExcelOutPut KingdeeExcelOutPut = new KingdeeExcelOutPut(Convert.ToInt32(cmbAccountParaID.SelectedValue), Convert.ToDateTime(txtBeginDate.Text), Convert.ToDateTime(txtEndDate.Text), txtCustomer.Text.Trim(), cmbFYear.SelectedValue, cmbFPeriod.SelectedValue);

        lock (KingdeeExcelOutPut)
        {
            OutExcel(KingdeeExcelOutPut.OutVoucher());
        }

    }
}
