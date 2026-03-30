using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Store;

public partial class ReportM_RptSale_RptDataTransLog : BasePage
{
    public string baseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            pageTitle = (String)GetGlobalResourceObject("BaseInfo", "Rpt_DateTransLog");
            BindDdl();
        }
    }

    private void BindDdl()
    {
        BaseBO baseBo = new BaseBO();
        //项目 
        Resultset rs = baseBo.Query(new Store());
        ddlBizproject.Items.Add(new ListItem("", ""));
        foreach (Store store in rs)
        {
            ddlBizproject.Items.Add(new ListItem(store.StoreName, store.StoreId.ToString()));
        }

        //数据类型
        //rs = baseBo.Query(new ServerResource());
        ddlDataType.Items.Add(new ListItem("", ""));
        ddlDataType.Items.Add(new ListItem("客流", "0"));
        ddlDataType.Items.Add(new ListItem("销售", "1"));
        ddlDataType.Items.Add(new ListItem("商品", "2"));
        ddlDataType.Items.Add(new ListItem("收银员", "3"));
        ddlDataType.Items.Add(new ListItem("商铺信息", "4"));
        ddlDataType.Items.Add(new ListItem("付款方式", "5"));
        ddlDataType.Items.Add(new ListItem("积分规则", "6"));
        ddlDataType.Items.Add(new ListItem("项目信息", "7"));
        ddlDataType.Items.Add(new ListItem("POS状态", "8"));
        //foreach (ServerResource SResource in rs)
        //{
        //    ddlResource.Items.Add(new ListItem(SResource.ResourceName, SResource.ResourceID.ToString()));
        //}

        //传输类型
        //rs = baseBo.Query(new TransTask());
        ddlTransType.Items.Add(new ListItem("", ""));
        ddlTransType.Items.Add(new ListItem("下传", "0"));
        ddlTransType.Items.Add(new ListItem("上传", "1"));
        //foreach (TransTask transTask in rs)
        //{
        //    ddlTask.Items.Add(new ListItem(transTask.TaskName, transTask.TaskID.ToString()));
        //}

        //传输状态
        //int[] getTransFlag = DataTransLog.GetTransFlag();
        ddlTransFlag.Items.Add(new ListItem("", ""));
        ddlTransFlag.Items.Add(new ListItem("不成功", "0"));
        ddlTransFlag.Items.Add(new ListItem("成功", "1"));
        //for(int i=0;i<getTransFlag.Length;i++)
        //{
        //    ddlTransStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo",DataTransLog.GetTransFlagDec(getTransFlag[i])), getTransFlag[i].ToString()));
        //}
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[8];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[8];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXStoreName";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptStoreInvSum_StoreName");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXServerResourceName";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Store_ServerResourceName");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXTaskName";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "Store_TaskName");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXStartDate";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "Master_lblStart");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXEndDate";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_EndTime");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXTransFlag";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_TransFlag");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXTitle";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_DateTransLog");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXMallTitle";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = Session["MallTitle"].ToString();
        paraField[7].CurrentValues.Add(discreteValue[7]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string str_sql = @"select datatranslog.storeid,dept.deptname,datatranslog.bizdate,case datatypeid when 0 then '客流' when 1 then '销售' when 2 then '商品'
                            when 3 then '收银员' when 4 then '商铺信息' when 5 then '付款方式' when 6 then '积分规则' when 7 then '项目信息'
                            when 8 then 'POS状态' end datatype,datatranslog.datacount,case datatranslog.transtypeid when 0 then '下传' when 1 then '上传' end transtype,
                            case datatranslog.transflag when 0 then '不成功' when 1 then '成功' end transflag,
                            datatranslog.transstartdate,datatranslog.transenddate
                            from datatranslog
                            inner join dept on dept.deptid=datatranslog.storeid
                            where 1=1";

        if (ddlBizproject.Text != "")
        {
            str_sql = str_sql + " AND datatranslog.storeid = '" + ddlBizproject.SelectedValue + "'";
        }
        if (ddlDataType.Text != "")
        {
            str_sql = str_sql + " AND datatranslog.datatypeid = '" + ddlDataType.SelectedValue + "'";
        }
        if (ddlTransType.Text != "")
        {
            str_sql = str_sql + " AND datatranslog.transtypeid = '" + ddlTransType.SelectedValue + "'";
        }
        if (ddlTransFlag.Text != "")
        {
            str_sql = str_sql + " AND DataTransLog.TransFlag = '" + ddlTransFlag.SelectedValue + "'";
        }
        if (txtExecuteDate.Text != "")
        {
            str_sql = str_sql + " AND DataTransLog.BizDate = '" + txtExecuteDate.Text + "'";
        }
        str_sql = str_sql + " order by BizDate desc";

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\DataTransLog.rpt";

    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptSale/RptDataTransLog.aspx");

    }
}
