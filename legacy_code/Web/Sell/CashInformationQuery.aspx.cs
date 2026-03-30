using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Base.DB;
using Base.Biz;
using Base.Page;

public partial class Sell_CashInformationQuery :BasePage
{
#region 定义
    public string baseInfo;
#endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtBizDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            BindGrdCashInfoNull();
            this.Form.DefaultButton = "btnQuery";
            
        }
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "CashInformation_Query");
    }
    protected void page()
    {
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        string sql = "Select UserID,TPUsrNm,BizDate,SUM(Amountt) as Amountt,SUM(PayAmt) as  PayAmt " +
                     "From CasherPayment Inner Join Tpusr On CasherPayment.UserID = Tpusr.TPUsrId " +
                     " Where BizDate= '" + txtBizDate.Text.Trim().ToString()+"'"+
                     " Group By BizDate,UserID,TPUsrNm";
        DataSet ds = baseBO.QueryDataSet(sql);
        dt = ds.Tables[0];
        pds.DataSource = dt.DefaultView;
        GrdCashInfo.DataSource = pds;
        GrdCashInfo.DataBind();
        spareRow = GrdCashInfo.Rows.Count;
        for (int i = 0; i < GrdCashInfo.PageSize - spareRow; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        GrdCashInfo.DataSource = pds;
        GrdCashInfo.DataBind();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
    
        

    }
    private void BindGrdCashInfoNull()
    {
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        dt.Columns.Add("UserID");
        dt.Columns.Add("TPUsrNm");
        dt.Columns.Add("BizDate");
        dt.Columns.Add("Amountt");
        dt.Columns.Add("PayAmt");


        for (int i = 0; i < 10; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        GrdCashInfo.DataSource = dt;
        GrdCashInfo.DataBind();
        GrdCashInfoClear();
    }
    private void GrdCashInfoClear()
    {
        foreach (GridViewRow gvr in GrdCashInfo.Rows)
        {
            if (gvr.Cells[0].Text == "&nbsp;")
            {
                gvr.Cells[5].Text = "";
            }
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (txtBizDate.Text != "")
        {
            page();
            GrdCashInfoClear();
        }
    }

    protected void GrdCashInfo_SelectedIndexChanged(object sender, EventArgs e)
    {

            /*把点击的合同号存入Cookies*/
            HttpCookie cookies = new HttpCookie("Info");
            cookies.Expires = System.DateTime.Now.AddHours(1);
            cookies.Values.Add("UserID", GrdCashInfo.SelectedRow.Cells[0].Text);
            cookies.Values.Add("BizDate", GrdCashInfo.SelectedRow.Cells[2].Text);
            Response.AppendCookie(cookies);
            Response.Redirect("CashInformation.aspx?modify=1");
        

    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("CashInformation.aspx?modify=0");
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        txtBizDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        BindGrdCashInfoNull();
    }


}
