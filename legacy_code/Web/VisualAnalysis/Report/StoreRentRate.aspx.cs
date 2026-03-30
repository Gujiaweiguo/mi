using System;
using System.Data;
using Base.Biz;
using BaseInfo.Dept;
using Base.XML;
using BaseInfo.User;
public partial class VisualAnalysis_Report_StoreRentRate : System.Web.UI.Page
{
    public string deptName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        string strSql = "select unit.unitcode,conshop.shopname,unitrent.floorarea,unitrent.usearea,unitrent.rentarea from unitrent" +
                        " inner join unit on (unit.unitid=unitrent.unitid) left join conshop on (conshop.shopid=unitrent.shopid)" +
                        " where unit.unitstatus!=2 and unitrent.period='" + DateTime.Now.Date.ToString("yyyy-MM-01") + "' and unit.storeid=" +
                        Request.QueryString["MallID"].ToString(); 
        if (!IsPostBack)
        {
            Session["getdatasql"] = strSql;
        }
        Session["getdatasql"] = strSql;
    }

    #region Web Form Designer generated code
    override protected void OnInit(EventArgs e)
    {
        InitializeComponent();
        base.OnInit(e);
    }

    private void InitializeComponent()
    {
        PreLoad();
    }
    #endregion

    private void PreLoad()
    {
        deptName = "项目商铺本月出租情况分析(单位:平米)";
        if (Request.QueryString["MallID"] != null)
        {
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = "DeptID=" + Request.QueryString["MallID"].ToString();
            DataSet dt = baseBo.QueryDataSet(new Dept());
            if (dt.Tables[0].Rows.Count == 1)
            {
                deptName = dt.Tables[0].Rows[0]["DeptName"].ToString() + deptName;
            }

            string strSql = @"select CONVERT(varchar(7), unitrent.period, 120) 'month',sum(floorarea)-sum(usearea) '公摊面积',sum(usearea)-sum(rentarea) '空置面积',sum(rentarea) '签约面积' from unitrent " +
                         "where unitrent.StoreId= " + Request.QueryString["MallID"].ToString() +
                         " and year(period)=" + DateTime.Now.Date.Year.ToString() +
                         " group by unitrent.StoreId,period ";
            dt = baseBo.QueryDataSet(strSql);
            if (dt.Tables[0].Rows.Count > 0)
            {
                StCol3D stcol = new StCol3D();
                SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
                string strFile = "E://work//mi_net//code//web//VisualAnalysis//VAMenu//" + sessionUser.UserID.ToString() + "//StoreRentRate.xml";
                stcol.Caption = "";
                stcol.xAxisName = DateTime.Now.Date.Year.ToString() + "年度各月份";
                stcol.yAxisName = "面积";
                stcol.ShowValues = true;
                stcol.GetXml(strFile, dt.Tables[0]);
                this.xmlFile.InnerText = "../../VisualAnalysis/VAMenu/" + sessionUser.UserID.ToString() + "/StoreRentRate.xml";
            }
        }
    }
}