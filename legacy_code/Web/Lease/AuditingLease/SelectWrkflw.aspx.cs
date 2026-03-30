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
using Base.Sys;
using Base.Util;
using Base;
using WorkFlow.WrkFlw;

public partial class Lease_AuditingLease_SelectWrkflw : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this.BindData(" and Nodestatus<>8 and  Nodestatus<>10");//绑定数据
            this.BinDDl();//绑定下拉列表框
        }
    }
    /// <summary>
    /// 绑定数据
    /// </summary>
    private void BindData(string strWhere)
    {
        string strSql = @"select a.wrkflwid,b.voucherid,(select wrkflwname from wrkflw b where b.wrkflwid=a.wrkflwid ) as wrkflwname
,b.voucherHints,Nodestatus,(select min(starttime) from wrkflwentity c where c.wrkflwid=a.wrkflwid and c.voucherid=b.voucherid) as  starttime,completedtime 
from wrkflw a,wrkflwentity b where wrkflwstatus=1 and 
 sequence=(select max(sequence) from wrkflwentity c where c.wrkflwid=a.wrkflwid and c.voucherid=b.voucherid)";
        if (strWhere != "")
            strSql += strWhere;
        strSql += " group by a.wrkflwid,b.voucherid,b.voucherHints,Nodestatus,starttime,completedtime order by a.wrkflwid";
        ViewState["strWhere"] = strWhere;
        BaseInfo.BaseCommon.BindGridView(strSql, this.GrdWrk);
    }
    /// <summary>
    /// 绑定下拉列表框
    /// </summary>
    private void BinDDl()
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        BaseBO objbaseBo = new BaseBO();
        objbaseBo.WhereClause = "WrkFlwStatus=1";
        Resultset rs = objbaseBo.Query(new WrkFlw());
        ddlWrkFlw.Items.Add(new ListItem(selected));
        foreach (WrkFlw objWrkFlw in rs)
            ddlWrkFlw.Items.Add(new ListItem(objWrkFlw.WrkFlwName.ToString(),objWrkFlw.WrkFlwID.ToString()));
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string strWhere = "";
        if (this.ddlWrkFlw.SelectedValue.Trim() != "---请选择---")
            strWhere += " and a.wrkflwid='" + this.ddlWrkFlw.SelectedValue + "'";
        if (this.txtStart.Text.Trim() != "")
            strWhere += " and starttime>='" + this.txtStart.Text.Trim() + "'";
        if (this.txtEnd.Text.Trim() != "")
            strWhere += " and completedtime<='" + this.txtEnd.Text.Trim() + "'";

        if(this.rbCom.Checked)//完成
            strWhere += " and Nodestatus=8";
        if(this.rbUnderWay.Checked)//流转中
            strWhere += " and Nodestatus<>8 and  Nodestatus<>10";
        if(this.rbBlankOut.Checked)//作废
            strWhere += " and Nodestatus=10";
        ViewState["strWhere"] = strWhere;
        this.BindData(strWhere);
    }
    protected void GrdWrk_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        this.BindData(ViewState["strWhere"].ToString());
    }
    protected void GrdWrk_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex >= 0)
        {
            if (e.Row.Cells[4].Text == "1")
                e.Row.Cells[4].Text = "正常流转待处理";
            if (e.Row.Cells[4].Text == "2")
                e.Row.Cells[4].Text = "上级审批待处理";
            if (e.Row.Cells[4].Text == "3")
                e.Row.Cells[4].Text = "驳回待处理";
            if (e.Row.Cells[4].Text == "4")
                e.Row.Cells[4].Text = "正常流转完成";
            if (e.Row.Cells[4].Text == "5")
                e.Row.Cells[4].Text = "上级审批完成";
            if(e.Row.Cells[4].Text == "6")
                e.Row.Cells[4].Text = "正常驳回处理完";
            if (e.Row.Cells[4].Text == "7")
                e.Row.Cells[4].Text = "上级驳回处理完";
            if (e.Row.Cells[4].Text == "8")
                e.Row.Cells[4].Text = "工作流正常流转完成";
            if (e.Row.Cells[4].Text == "9")
                e.Row.Cells[4].Text = "节点草稿状态";
            if (e.Row.Cells[4].Text == "10")
                e.Row.Cells[4].Text = "节点作废状态";
            if (e.Row.Cells[0].Text == "&nbsp;")
            {
                e.Row.Cells[7].Text = "";
            }
            else
                e.Row.Cells[7].Text = "<a href=javascript:window.showModalDialog('WrkFlwMessage.aspx?wrkFlwID='+encodeURI('" + e.Row.Cells[0].Text + "')+'&voucherID='+encodeURI('" + e.Row.Cells[1].Text + "'),'window','dialogWidth=600px;dialogHeight=320px');>" + (String)GetGlobalResourceObject("BaseInfo", "User_lblUserQuery") + "</a>";
        }
    }
    protected void GrdWrk_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        
    }
}
