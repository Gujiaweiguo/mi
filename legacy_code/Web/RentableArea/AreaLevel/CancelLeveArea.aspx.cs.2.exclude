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

public partial class LeaseArea_Area_CancelArea : System.Web.UI.Page
{

    BaseBO bo = new BaseBO();
    Area areaInfo = new Area();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //添充代码
            bo.WhereClause = "AreaID=" + Convert.ToInt32(Request.QueryString["areaId"].ToString());

            Resultset rs = bo.Query(areaInfo);

            if (rs.Count != 0)
            {
                areaInfo = rs.Dequeue() as Area;
                this.txtAreaCode.Text = areaInfo.AreaCode;
            }
        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        //更新状态
        string sql = "Update [Area] Set AreaStatus='"+ Area.AREA_STATUS_INVALID +"' where AreaId="+ Convert.ToInt32(Request.QueryString["areaId"].ToString());

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
