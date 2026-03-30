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
using RentableArea;
using Base.DB;
public partial class RentableArea_TradeRelation_QueryTradeRelation : System.Web.UI.Page
{
    BaseBO baseBO = new BaseBO();
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            baseBO.WhereClause = "TradeDefStatus=" + TradeDef.TRADEDEF_STATUS_VALID;
            GridView1.DataSource = baseBO.Query(new TradeDef());
            GridView1.DataBind();
        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddTradeRelation.aspx");
    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
