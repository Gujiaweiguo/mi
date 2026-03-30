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
using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.User;

public partial class Disktop1 : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.InitLabel();
        this.BindData();
    }

    protected void InitLabel()
    {
        BaseBO objBaseB0 = new BaseBO();

        SessionUser sessionuser = (SessionUser)Session["UserAccessInfo"];
        objBaseB0.WhereClause = "userid=" + sessionuser.UserID;
        Resultset rs = objBaseB0.Query(new Users());
        if (rs.Count == 1)
        {
            Users objUserInfo = rs.Dequeue() as Users;
            this.lblname.Text = objUserInfo.UserName;
        }
        this.lblDate.Text = this.GetLastDate(sessionuser.UserID.ToString());
    }

    private void BindData()
    {
        SessionUser sessionuser = (SessionUser)Session["UserAccessInfo"];
//        string sqlStr = @"Select WrkFlw.WrkFlwName,WrkFlwentity.StartTime,WrkFlwentity.CompletedTime,WrkFlwentity.UserID,WrkFlwentity.VoucherID, WrkFlwentity.VoucherHints,WrkFlwentity.VoucherMemo	
//        From WrkFlwentity,WrkFlwNode,WrkFlw 
//        Where WrkFlwentity.WrkFlwID=WrkFlwNode.WrkFlwID And WrkFlwentity.NodeID=WrkFlwNode.NodeID And 
//        WrkFlwNode.WrkFlwID = WrkFlw.WrkFlwID And WrkFlwNode.RoleID= "+sessionuser.RoleID+" And  WrkFlw.WrkFlwStatus=1	And WrkFlwentity.NodeStatus In (1,3) Order By StartTime Desc";

        string sqlStr = @"Select WrkFlw.WrkFlwName,WrkFlwentity.StartTime,WrkFlwentity.CompletedTime,WrkFlwentity.UserID,WrkFlwentity.VoucherID, WrkFlwentity.VoucherHints,WrkFlwentity.VoucherMemo,Func.FuncUrl + '?'
		+ 'WrkFlwID=' + CAST(WrkFlwentity.WrkFlwID AS varchar(10)) + '&NodeID=' +CAST(WrkFlwentity.NodeID AS varchar(10)) + '&Sequence=' + CAST(WrkFlwentity.Sequence AS varchar(10)) + 
		+ '&VoucherID=' + CAST(WrkFlwentity.VoucherID AS varchar(10)) + '&Type=Old' As FuncUrl
        From WrkFlwentity,WrkFlwNode,WrkFlw,Func
        Where WrkFlwentity.WrkFlwID=WrkFlwNode.WrkFlwID And WrkFlwentity.NodeID=WrkFlwNode.NodeID And WrkFlwNode.FuncID = Func.FuncID And
        WrkFlwNode.WrkFlwID = WrkFlw.WrkFlwID And WrkFlwNode.RoleID= " + sessionuser.RoleID + " And  WrkFlw.WrkFlwStatus=1	And WrkFlwentity.NodeStatus In (1,3) Order By StartTime Desc";



        string sqlStr1 = @"Select WrkFlw.WrkFlwName,WrkFlwentity.StartTime,WrkFlwentity.CompletedTime,WrkFlwentity.UserID,WrkFlwentity.VoucherID, WrkFlwentity.VoucherHints,WrkFlwentity.VoucherMemo,WrkFlwentity.VoucherMemo,Func.FuncUrl + '?'
		+ 'WrkFlwID=' + CAST(WrkFlwentity.WrkFlwID AS varchar(10)) + '&NodeID=' +CAST(WrkFlwentity.NodeID AS varchar(10)) + '&Sequence=' + CAST(WrkFlwentity.Sequence AS varchar(10)) + 
		+ '&VoucherID=' + CAST(WrkFlwentity.VoucherID AS varchar(10)) + '&Type=Old' As FuncUrl		
        From WrkFlwentity,WrkFlwNode,WrkFlw,Func
        Where WrkFlwentity.WrkFlwID=WrkFlwNode.WrkFlwID And WrkFlwentity.NodeID=WrkFlwNode.NodeID And WrkFlwNode.FuncID = Func.FuncID And
        WrkFlwNode.WrkFlwID = WrkFlw.WrkFlwID And WrkFlwNode.RoleID= " + sessionuser.RoleID + "  And  WrkFlw.WrkFlwStatus=1	And WrkFlwentity.NodeStatus In (1,3) and DateDiff(dd,StartTime,GetDate())<=4 And DateDiff(dd,StartTime,GetDate())>=0  Order By StartTime Desc";
        BaseBO basebo = new BaseBO();
        DataSet ds = basebo.QueryDataSet(sqlStr);
        DataSet ds1 = basebo.QueryDataSet(sqlStr1);
        int k = 0;
        int l = 0;
        if (ds.Tables[0].Rows.Count == 0)
        {
            k = GridView1.PageSize;
        }
        else if (ds.Tables[0].Rows.Count % GridView1.PageSize != 0)
        {
            k = (GridView1.PageSize - ds.Tables[0].Rows.Count % GridView1.PageSize);
        }
        if (ds1.Tables[0].Rows.Count == 0)
        {
            l = GridView2.PageSize;
        }
        else if (ds1.Tables[0].Rows.Count % GridView2.PageSize != 0)
        {
            l = (GridView2.PageSize - ds1.Tables[0].Rows.Count % GridView2.PageSize);
        }
        if (k>0)
        {
            for (int i = 0; i < k; i++)
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());                
            }
        }
        if (l>0)
        {
            for (int i = 0; i < l; i++)
            {
                ds1.Tables[0].Rows.Add(ds1.Tables[0].NewRow());                
            }
        }

        GridView1.DataSource = ds.Tables[0].DefaultView;
        GridView1.DataBind();
        GridView2.DataSource = ds1.Tables[0].DefaultView;
        GridView2.DataBind();
        BindGV3();
        BindGV4();
    }

    private void BindGV3()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PageUrl");
        dt.Columns.Add("MenuName");
        for (int i=0; i < GridView3.PageSize; i++)
        {
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
        }
        dt.Rows[0]["PageUrl"] = "Sell/ShopSellDifferenceReceipt.aspx";
        dt.Rows[0]["MenuName"] = (String)GetGlobalResourceObject("BaseInfo", "Menu_ShopSellDifferenceReceipt");
        dt.Rows[1]["PageUrl"] = "ReportM/RptSale/RptSalesDetails.aspx";
        dt.Rows[1]["MenuName"] = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblSalesDetails");
        GridView3.DataSource = dt.DefaultView;
        GridView3.DataBind();
    }

    private void BindGV4()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PageUrl");
        dt.Columns.Add("MenuName");
        for (int i=0; i < GridView4.PageSize; i++)
        {
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
        }
        dt.Rows[0]["PageUrl"] = "Lease/ChargeAccount/ChargeAccount.aspx";
        dt.Rows[0]["MenuName"] = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblLeaseAccount");
        dt.Rows[1]["PageUrl"] = "ReportM/RptSale/RptMallSalesSum.aspx";
        dt.Rows[1]["MenuName"] = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblMallSalesSum");
        dt.Rows[2]["PageUrl"] = "ReportM/RptBase/RptContractAlert.aspx";
        dt.Rows[2]["MenuName"] = (String)GetGlobalResourceObject("BaseInfo", "Menu_ContractAlert");
        GridView4.DataSource = dt.DefaultView;
        GridView4.DataBind();
    }

    private string GetLastDate(string strUserID)
    {
        string strDate = "0000-00-00 00:00:00";
        string strSql = "select top 1 createtime from ( select top 2 createtime from openpagelog where createuserid=" + strUserID + 
                        " and pagepath in ('/Web/Login.aspx','Login.aspx') order by createtime desc) as a" +
                        " order by a.createtime";
        BaseBO basebo = new BaseBO();
        DataSet ds = basebo.QueryDataSet(strSql);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            strDate = ds.Tables[0].Rows[0][0].ToString();
        }
        return strDate;
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[0].Text != "&nbsp;")
            {
                //DateTime dt = DateTime.Now.Date;
                //try { dt = DateTime.Parse(e.Row.Cells[1].Text.ToString()); }
                //catch { }
                string strUrl = "";
                if (e.Row.Cells[0].Text.Trim().StartsWith("~"))
                    strUrl = e.Row.Cells[0].Text.Trim().Substring(2);
                else
                    strUrl = e.Row.Cells[0].Text.Trim();


                e.Row.Cells[1].Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "<a href=" + strUrl + ">" + "<img src='pic/arrow.jpg' border=0/></a>" + "&nbsp;&nbsp;&nbsp;&nbsp;" + " 新增 " + e.Row.Cells[1].Text + "  \"" + this.OpenUrl(strUrl, e.Row.Cells[6].Text) + " 需要您的处理.";//e.Row.Cells[3].Text时间
                //e.Row.Cells[1].Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "<img src='pic/arrow.jpg'/>" + "&nbsp;&nbsp;&nbsp;&nbsp;" + dt.ToShortDateString() + " 新增 " + e.Row.Cells[1].Text + "  \"" + e.Row.Cells[6].Text + "\"" + " 需要您的处理.";
                //e.Row.Cells[1].Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "<a href=" + strUrl + ">" + "<img src='pic/arrow.jpg' border=0/></a>" + "&nbsp;&nbsp;&nbsp;&nbsp;" + dt.ToShortDateString() + " 新增 " + e.Row.Cells[1].Text + "  \"" + this.OpenUrl(strUrl, e.Row.Cells[6].Text) + " 需要您的处理.";
            }
        }
    }
    private string OpenUrl(string strUrl, string strContent)
    {
        return "<a href=" + strUrl + ">"+strContent+"</a>";
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView theGrid = sender as GridView;
        int newPageIndex = 0;
        if (-2 == e.NewPageIndex)
        {
            TextBox txtNewPageIndex = null;
            GridViewRow pagerRow = theGrid.BottomPagerRow;
            if (null != pagerRow)
            {
                txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;
            }
            if (null != txtNewPageIndex)
            {
                newPageIndex = int.Parse(txtNewPageIndex.Text) - 1;
            }
        }
        else
        { newPageIndex = e.NewPageIndex; }
        newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
        newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;
        theGrid.PageIndex = newPageIndex;
        this.BindData();

    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Response.Redirect(GridView1.SelectedRow.Cells[0].Text);
       
    }
    protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Response.Redirect(GridView2.SelectedRow.Cells[0].Text);
    }
    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[0].Text != "&nbsp;")
            {
                //DateTime dt = DateTime.Now.Date;
                //try { dt = DateTime.Parse(e.Row.Cells[1].Text.ToString()); }
                //catch { }
                e.Row.Cells[1].Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "<img src='pic/arrow.jpg'/>" + "&nbsp;&nbsp;&nbsp;&nbsp;" + " 您提交的关于 " + "  \"" + e.Row.Cells[6].Text + "\"" + " 的申请已经处理。";// dt.ToShortDateString() 时间
            }
        }
    }
    protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[0].Text != "&nbsp;")
            {
                e.Row.Cells[1].Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "<a href=" + e.Row.Cells[0].Text + ">" + "<img src='pic/arrow.jpg' border=0/></a>" + "&nbsp;&nbsp;&nbsp;&nbsp;" + this.OpenUrl(e.Row.Cells[0].Text, e.Row.Cells[1].Text);
            }
        }
    }
}
