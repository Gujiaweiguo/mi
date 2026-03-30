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
using RentableArea;

public partial class LeaseArea_UnitType_CancelUnitType : System.Web.UI.Page
{

    BaseBO bo = new BaseBO();
    UnitTypes unitTypesInfo = new UnitTypes();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //填充Code 
            bo.WhereClause = "UnitTypeID=" + Convert.ToInt32(Request.QueryString["unitType"].ToString());

            Resultset rs = bo.Query(unitTypesInfo);

            if (rs.Count != 0)
            {
                unitTypesInfo = rs.Dequeue() as UnitTypes;
                this.txtUnitTypeCode.Text = unitTypesInfo.UnitTypeCode;
            }
        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        //修改
        string sql = "Update [unitType] Set unitTypeStatus='" + UnitTypes.UNITYEPE_STATUS_INVALID + "' where UnitTypeID=" + Convert.ToInt32(Request.QueryString["unitType"].ToString());

        if (bo.ExecuteUpdate(sql) != -1)
        {
            Response.Write("<script language=javascript>alert(\"作废成功!\");</script>");
        }
        else
        {
            Response.Write("<script language=javascript>alert(\"作废失败!\");</script>");
        }
    }
}
