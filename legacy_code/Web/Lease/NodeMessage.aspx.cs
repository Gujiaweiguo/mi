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
using Base;
using Base.Page;
using Lease;
using WorkFlow.WrkFlw;

public partial class Lease_NodeMessage : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            string wrkFlwID = Request.QueryString["wrkFlwID"].ToString();
            string voucherID = Request.QueryString["voucherID"].ToString();
            BindData(wrkFlwID, voucherID);
        }
    }

    private void BindData(string wrkFlwID, string voucherID)
    {
        DataSet ds = CheckApproveMessage.GetCheckApproveMessage(wrkFlwID, voucherID);
        DataTable dt = ds.Tables[0];
        int count = dt.Rows.Count;
        int ss = 0;

        for (int j = 0; j < count; j++)
        {
            string nodeStatusName = WrkFlwEntity.GetNodeStatusDesc(Convert.ToInt32(dt.Rows[j]["NodeStatus"].ToString()));
            dt.Rows[j]["NodeStatusName"] = nodeStatusName;
        }

        if (count < 1)
        {
            for (int i = 0; i < 15; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
        }
        else
        {
            for (int i = 0; i < 15 - count; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
        }
        GVMessage.DataSource = dt;
        GVMessage.DataBind();
    }
}
