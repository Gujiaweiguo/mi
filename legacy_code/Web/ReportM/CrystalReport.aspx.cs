using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Base.Page;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using CrystalDecisions.CrystalReports.Engine;
using System.Collections.Specialized;
using System.Text;
using Base.DB;
using Base.Util;
/// <summary>
/// http://192.168.1.229/JKLReportWeb/CrystalReport.aspx?rptPathName=aaa.rpt
/// &rptPath=\PurMon\NewGoodsDealIn\NewGoodsTestSaleAnalysis.rpt
/// 
/// </summary>
/// 
public partial class CrystalReport : BasePage
{

    private ReportDocument reportDoc;
    //排序标志
    string sortField = string.Empty;
    //rpt文件的相对路径

    string rptPathName = string.Empty;
    //rpt文件的相对路径，此参数用于存在链接的报表排序中(针对的问题是链接到其它报表后，返回到此报表出现的排序问题。问题原因是，链接前的reportDoc对象已经更新了)
    string rptPath = string.Empty;
    //表示是否排序
    string isSort = string.Empty;
    //报表标题
    string title = string.Empty;
    //报表标题Unicode
    string titleOfUnicode = string.Empty;
    //用于判断报表查询进行清除Session["rptPathName"]的标志：step＝"refresh"时，做Session["rptPathName"] = ""处理
    string strStep = string.Empty;
    //用于判断水晶报表链接过来进行清除Session["rptPathName"]的标志：linkASP="true"时，做Session["rptPathName"] = ""处理
    string strLink = string.Empty;
    //被链接的水晶报表的URL
    string newURL = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        // 在此处放置用户代码以初始化页面
        strStep = Request.QueryString["step"];
        strLink = Request.QueryString["linkASP"];
        newURL = Request.QueryString["newURL"];

        if (strStep != null && strStep == "refresh")
        {
            Session["rptPathName"] = "";
            return;
        }
        else if (strLink != null && strLink == "true")
        {
            Session["rptPathName"] = "";

            NameValueCollection coll_link = Request.QueryString;
            String[] arrParam = coll_link.AllKeys;
            for (int i = 0; i < arrParam.Length; i++)
            {
                string key = arrParam[i];
                if (key.IndexOf("linkASP") > -1)
                {
                    continue;
                }
                else if (key.IndexOf("newURL") > -1)
                {
                    continue;
                }
                string theValue = coll_link.Get(key);
                newURL += "&" + key + "=" + theValue;
            }

            Response.Redirect(newURL);

        }

        else

        {
            displayReport();

        }
    }

    private void displayReport()
    {

        rptPathName = Request.QueryString["rptPathName"];

        sortField = Request.QueryString["sort"];
        
        title = Request.QueryString["title"];
        
        rptPath = Request.QueryString["rptPath"];
        
        //参数titleOfUnicode保存了title的Unicode码

        
        //titleOfUnicode = Request.QueryString["titleOfUnicode"];

        if (sortField == null && ((string)Session["rptPathName"] != rptPathName))       //((string)Session["rptPathName"] != rptPathName) 添加此条件是为了翻页时，不重新生成reportDoc
        
        {
            //第一次登录此报表时，经由此路径
            ParameterFields paramFields = new ParameterFields();
            NameValueCollection coll = Request.QueryString;
            String[] arr1 = coll.AllKeys;

            string reportPath1 = Server.MapPath("Report");
            string rptName = reportPath1 + rptPathName;
            reportDoc = new ReportDocument();
          
            reportDoc.Load(rptName);
            SetDBLogonForReport(reportDoc);

            for (int i = 0; i < arr1.Length; i++)
            {
                string key = arr1[i];
                if (key.IndexOf("sort") > -1)
                {
                    continue;
                }
                else if (key.IndexOf("rptPathName") > -1)
                {
                    continue;
                }
                else if (key.IndexOf("link") > -1)
                {
                    continue;
                }
                string theValue = coll.Get(key);
                ParameterField paramField = new ParameterField();
                ParameterDiscreteValue discreteVal = new ParameterDiscreteValue();
                paramField.ParameterFieldName = key;
                discreteVal.Value = theValue;
                paramField.CurrentValues.Add(discreteVal);
                paramFields.Add(paramField);

                //涂长东改动 ，加入以下方法

                ParameterValues currentParameterValues = new ParameterValues();
                ParameterDiscreteValue parameterDiscreteValue = new ParameterDiscreteValue();
                parameterDiscreteValue.Value = theValue;
                currentParameterValues.Add(parameterDiscreteValue);
                ParameterFieldDefinitions parameterFieldDefinitions = reportDoc.DataDefinition.ParameterFields;
                ParameterFieldDefinition parameterFieldDefinition = parameterFieldDefinitions[key];
                parameterFieldDefinition.ApplyCurrentValues(currentParameterValues);
            }

          



            crView.ParameterFieldInfo = paramFields;
            crView.EnableDatabaseLogonPrompt = false;




            Session["reportDOC"] = reportDoc;
            Session["sortField"] = "";
            Session["rptPathName"] = rptPathName;
            Session["title"] = title;

            crView.DisplayToolbar = true;
            crView.DisplayGroupTree = false;
            crView.EnableParameterPrompt = false;
            crView.EnterpriseLogon = false;
            crView.HasCrystalLogo = false;
            crView.HasToggleGroupTreeButton = false;
            crView.BestFitPage = true;
            
            crView.ReportSource = reportDoc;
        }
        else if (sortField != null && rptPath != null && ((string)Session["rptPathName"] != rptPath))
        {
            //链接后返回父报表时，进行排序时，经由此路径

            ParameterFields paramFields = new ParameterFields();
            NameValueCollection coll = Request.QueryString;
            String[] arr1 = coll.AllKeys;

            reportDoc = new ReportDocument();
            Session["reportDOC"] = reportDoc;
            Session["sortField"] = "";
            Session["rptPathName"] = rptPath;
            //Session["title"] = titleOfUnicode;
            Session["title"] = title;
            string reportPath1 = Server.MapPath("Report");
            string rptName = reportPath1 + rptPath;
            reportDoc.Load(rptName);
            SetDBLogonForReport(reportDoc);
            setReportSort(reportDoc);

            for (int i = 0; i < arr1.Length; i++)
            {
                string key = arr1[i];
                if (key.IndexOf("sort") > -1)
                {
                    continue;
                }
                else if (key.IndexOf("rptPathName") > -1)
                {
                    continue;
                }
                else if (key.IndexOf("link") > -1)
                {
                    continue;
                }

                string theValue = coll.Get(key);

                //if (key.IndexOf("title") > -1)
                //{
                //    theValue = titleOfUnicode;
                //}
                ParameterField paramField = new ParameterField();
                ParameterDiscreteValue discreteVal = new ParameterDiscreteValue();
                paramField.ParameterFieldName = key;
                discreteVal.Value = theValue;
                paramField.CurrentValues.Add(discreteVal);
                paramFields.Add(paramField);

                //涂长东改动 ，加入以下方法

                ParameterValues currentParameterValues = new ParameterValues();
                ParameterDiscreteValue parameterDiscreteValue = new ParameterDiscreteValue();
                parameterDiscreteValue.Value = theValue;
                currentParameterValues.Add(parameterDiscreteValue);
                ParameterFieldDefinitions parameterFieldDefinitions = reportDoc.DataDefinition.ParameterFields;
                ParameterFieldDefinition parameterFieldDefinition = parameterFieldDefinitions[key];
                parameterFieldDefinition.ApplyCurrentValues(currentParameterValues);
            }

            crView.ParameterFieldInfo = paramFields;
            
            crView.EnableDatabaseLogonPrompt = false;



        
            crView.DisplayToolbar = true;
            crView.DisplayGroupTree = false;
            crView.EnableParameterPrompt = false;
            crView.EnterpriseLogon = false;
            crView.HasCrystalLogo = false;
            crView.HasToggleGroupTreeButton = false;
            crView.BestFitPage = true;
            
            crView.ReportSource = reportDoc;


            Session["reportDOC"] = reportDoc;
            Session["sortField"] = "";
            Session["rptPathName"] = rptPathName;
            Session["title"] = title;
        }
        else
        {
            if (sortField != (string)Session["sortField"])
            {
                //经由此路径，一是可以避免导出报表时汉字出现乱码，二是避免点击不同排序字段排序

                Session["sortField"] = sortField;

                ParameterFields paramFields = new ParameterFields();
                NameValueCollection coll = Request.QueryString;
                String[] arr1 = coll.AllKeys;
                reportDoc = (ReportDocument)Session["reportDOC"];
                for (int i = 0; i < arr1.Length; i++)
                {
                    string key = arr1[i];
                    if (key.IndexOf("sort") > -1)
                    {
                        continue;
                    }
                    else if (key.IndexOf("rptPathName") > -1)
                    {
                        continue;
                    }
                    else if (key.IndexOf("link") > -1)
                    {
                        continue;
                    }
                    string theValue = coll.Get(key);

                    if (key.IndexOf("title") > -1)
                    {
                        theValue = (String)Session["title"];
                    }
                    ParameterField paramField = new ParameterField();
                    ParameterDiscreteValue discreteVal = new ParameterDiscreteValue();
                    paramField.ParameterFieldName = key;
                    discreteVal.Value = theValue;
                    paramField.CurrentValues.Add(discreteVal);
                    paramFields.Add(paramField);

                    //涂长东改动 ，加入以下方法

                    ParameterValues currentParameterValues = new ParameterValues();
                    ParameterDiscreteValue parameterDiscreteValue = new ParameterDiscreteValue();
                    parameterDiscreteValue.Value = theValue;
                    currentParameterValues.Add(parameterDiscreteValue);
                    ParameterFieldDefinitions parameterFieldDefinitions = reportDoc.DataDefinition.ParameterFields;
                    ParameterFieldDefinition parameterFieldDefinition = parameterFieldDefinitions[key];
                    parameterFieldDefinition.ApplyCurrentValues(currentParameterValues);
                }

                crView.ParameterFieldInfo = paramFields;
               
                crView.DisplayToolbar = true;
                crView.DisplayGroupTree = false;
                crView.EnableParameterPrompt = false;
                crView.EnterpriseLogon = false;
                crView.HasCrystalLogo = false;
                crView.HasToggleGroupTreeButton = false;
                crView.BestFitPage = true;
                
                crView.ReportSource = reportDoc;

            }
            else if (sortField == (string)Session["sortField"])
            {
                reportDoc = (ReportDocument)Session["reportDOC"];
                setReportSort(reportDoc);
                crView.DisplayToolbar = true;
                crView.DisplayGroupTree = false;
                crView.EnableParameterPrompt = false;
                crView.EnterpriseLogon = false;
                crView.HasCrystalLogo = false;
                crView.HasToggleGroupTreeButton = false;
                crView.BestFitPage = true;
                
                crView.ReportSource = reportDoc;
            }
        }
    }

    private string bytesToUnicode(string strChinese)
    {
        if (strChinese == null || strChinese.Length == 0)
        {
            return "";
        }

        StringBuilder strUnicode = new StringBuilder();
        string s1;
        string s2;

        for (int index = 0; index < strChinese.Length; index++)
        {
            byte[] bt = System.Text.Encoding.Unicode.GetBytes(strChinese.Substring(index, 1));
            if (bt.Length > 1)//判断是否汉字
            {
                s1 = Convert.ToString((short)(bt[1] - '\0'), 16);//转化为16进制字符串

                s2 = Convert.ToString((short)(bt[0] - '\0'), 16);//转化为16进制字符串

                s1 = (s1.Length == 1 ? "0" : "") + s1;//不足位补0
                s2 = (s2.Length == 1 ? "0" : "") + s2;//不足位补0
                strUnicode.Append("%u" + s1 + s2);
            }
        }

        strUnicode.Append("%u0020");

        return strUnicode.ToString();
    }


    private void SetDBLogonForReport(ReportDocument reportDocument)
    {
        ConnectionInfo connectionInfo = new ConnectionInfo();
        DBParam dbp = ParamManager.GetDBParam("mi_net");

        connectionInfo.ServerName = dbp.DataSource;
        connectionInfo.DatabaseName = dbp.ID;
        connectionInfo.UserID = dbp.UserID;
        connectionInfo.Password = dbp.Password;
 
        //设置数据源

        //connectionInfo.LogonProperties = DataSourceManager 

        Tables tables = reportDocument.Database.Tables;
        foreach (CrystalDecisions.CrystalReports.Engine.Table table in tables)
        {
            TableLogOnInfo tableLogonInfo = table.LogOnInfo;
            tableLogonInfo.ConnectionInfo = connectionInfo;
            
            table.ApplyLogOnInfo(tableLogonInfo);
        }
    
    }

    private void setReportSort(ReportDocument reportDocument)
    {
        foreach (SortField crSortField in reportDocument.DataDefinition.SortFields)
        {
            if (crSortField.Field.Name == "SortField")
            {
                if (crSortField.SortDirection == CrystalDecisions.Shared.SortDirection.AscendingOrder)
                {
                    crSortField.SortDirection = CrystalDecisions.Shared.SortDirection.DescendingOrder;
                }
                else
                {
                    crSortField.SortDirection = CrystalDecisions.Shared.SortDirection.AscendingOrder;
                }
            }
        }

    }

    private void Page_Unload(object sender, EventArgs e)
    {
        reportDoc.Dispose();
        crView.Dispose();
    }

    #region Web 窗体设计器生成的代码
    override protected void OnInit(EventArgs e)
    {
        //
        // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。


        //
        InitializeComponent();
        base.OnInit(e);
    }

    /// <summary>
    /// 设计器支持所需的方法 - 不要使用代码编辑器修改


    /// 此方法的内容。


    /// </summary>
    private void InitializeComponent()
    {
        this.Load += new System.EventHandler(this.Page_Load);

    }
    #endregion

}
