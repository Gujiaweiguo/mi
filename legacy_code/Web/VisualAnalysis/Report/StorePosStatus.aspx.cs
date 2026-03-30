using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Base.Biz;
using BaseInfo.Dept;
public partial class VisualAnalysis_Report_StorePosStatus : System.Web.UI.Page
{
    protected string strStoreName = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        string strSql = @"select ROW_NUMBER() OVER(order by conshop.shopcode) as no,posstatus.shopid ShopID,posstatus.POSid,conshop.shopcode ,
                        conshop.shopname,posstatus.ip,posstatus.tpusrid,posstatus.poslasttime,
                        case posstatus.posstatus when 0 then '关机' when 1 then '更新成功' when 2 then '开机' when 3 then '签到'
                        when 4 then '交易中' when 6 then '正常'  when 5 then '最终签退' when 9 then '断网' end posstatus
                        from posstatus
                        left join conshop on (conshop.shopid=posstatus.shopid)
                        where conshop.storeid=" + Request.QueryString["MallID"].ToString();
        if (!IsPostBack)
        {
            Session["getdatasql"] = strSql;
        }
        Session["getdatasql"] = strSql;
        strStoreName = "项目POS机状态监控";
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "DeptID=" + Request.QueryString["MallID"].ToString();
        DataSet dt = baseBo.QueryDataSet(new Dept());
        if (dt.Tables[0].Rows.Count == 1)
        {
            strStoreName = dt.Tables[0].Rows[0]["DeptName"].ToString() + strStoreName;
        }
    }
}
