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
public partial class LeaseArea_Unit_CancelUnit : System.Web.UI.Page
{

    BaseBO bo = new BaseBO();
    Units unitInfo = new Units();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bo.WhereClause = "unitID=" + Convert.ToInt32(Request.QueryString["unitId"].ToString());

            Resultset rs = bo.Query(unitInfo);

            //显示 Code
            if (rs.Count != 0)
            {
                unitInfo = rs.Dequeue() as Units;
                this.txtUnitCode.Text = unitInfo.UnitCode;
            }
        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {  
        //修改数据
        string sql = "Update [Unit] Set unitStatus='" + Location.LOCATION_STATUS_INVALID + "' where unitID=" + Convert.ToInt32(Request.QueryString["unitId"].ToString());
        if (bo.ExecuteUpdate(sql) != -1)
        {
            Response.Write("<script language=javascript>alert('作废成功!');</script>");
        }
        else
        {
            Response.Write("<script language=javascript>alert('作废失败!');</script>");
        }
    }
}
