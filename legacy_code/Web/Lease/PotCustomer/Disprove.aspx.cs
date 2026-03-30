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

using WorkFlow;
using WorkFlow.WrkFlw;
using WorkFlow.Uiltil;
using Base.Page;
using BaseInfo.User;

public partial class Test_Default3 : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            WrkFlwEntityDisprove[] objWrkFlwEntity = WrkFlwApp.GetWrkFlwEntitiesDisprove(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["Sequence"]), Convert.ToInt32(Request.QueryString["VoucherID"]));

            GridView1.DataSource = objWrkFlwEntity;
            GridView1.DataBind();
        }
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
        int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);
        int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);
        int toWrkFlwID = Convert.ToInt32(GridView1.SelectedRow.Cells[0].Text);
        int toNodeID = Convert.ToInt32(GridView1.SelectedRow.Cells[1].Text);
        int toSequence = Convert.ToInt32(GridView1.SelectedRow.Cells[9].Text);

        String voucherHints = GridView1.SelectedRow.Cells[7].Text;
        String voucherMemo = Request.QueryString["voucherMemo"];
        int voucherID = Convert.ToInt32(GridView1.SelectedRow.Cells[6].Text);
        int operatorID = objSessionUser.UserID;
        int deptID = objSessionUser.DeptID;
        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, operatorID);

        WrkFlwApp.RejectVoucher(wrkFlwID, nodeID, sequence, toWrkFlwID, toNodeID, vInfo);
        Response.Write("<script language=javascript>alert('" + hidAdd.Value + "');</script>");
        Response.Write("<script language=javascript>window.close();</script>");

    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#6699ff'");
        //    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
        //}
    }
}
