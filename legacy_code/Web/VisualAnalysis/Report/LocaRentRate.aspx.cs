using System;
using System.Data;

using Base.Biz;
using BaseInfo.Dept;
public partial class VisualAnalysis_Report_LocaRentRate : System.Web.UI.Page
{
    public string baseInfo;
    public string strPath;
    public string deptName = "";
    public string strmonth = "";
    public string locaID = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            deptName = "测试大区";
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_BackHome");
            strmonth = DateTime.Now.AddMonths(-1).ToString("yyyy年MM月份");  //查询上月的出租率
            if (Request.QueryString["LocaID"] != null)
            {
                locaID = Request.QueryString["LocaID"].ToString();
                strPath = "../Disktop.aspx?LocaID=" + locaID;
                BaseBO baseBo = new BaseBO();
                baseBo.WhereClause = "DeptID=" + locaID.ToString();
                DataSet dt = baseBo.QueryDataSet(new Dept());

                if (dt.Tables[0].Rows.Count == 1)
                {
                    deptName = dt.Tables[0].Rows[0]["DeptName"].ToString().Trim();
                }
            }
            else
            {
                locaID = "0";
                strPath = "../Disktop.aspx";
            }
        }
    }
    /// <summary>
    /// 返回项目整体出租率分析html
    /// </summary>
    /// <returns></returns>
    public string GetChartHtml()
    {
        string strHtml;
        string strSql = "select t.storeid,dept.deptname,b.deptid,b.deptname ddname,t.period,unitstatus,sum(T.floorarea) AS totalfloorarea " +
                        "from( select storeid,period,case when shopid=0 then 0 else 1 end  as unitstatus,floorarea " +
                        "FROM unitrent where unitrent.period='" + DateTime.Now.Date.AddMonths(-1).ToString("yyyy-MM-01") + "' ) t " +
                        "inner join dept on (t.storeid=dept.deptid) " +
                        "left join (select deptid,deptname,pdeptid from dept where depttype=5) a on (dept.pdeptid=a.deptid) " +  //城市
                        "left join (select deptid,deptname from dept where depttype=4) b on (a.pdeptid=b.deptid) " +  //区域
                        "where b.deptid=" + locaID +
                        " group by t.storeid,t.unitstatus,t.period,dept.deptname,b.deptid,b.deptname order by t.storeid";


        strHtml = "<div id='chartdiv5' align='center'>FusionCharts. </div>" +
                  "<script type='text/javascript'>" +
                  "var chart5 = new FusionCharts('../../FusionCharts/SWF/MSColumn3DLineDY.swf', 'ChartId', '100%', '100%', '0', '0');" +
                  "chart5.setDataURL('" + "../../FusionCharts/XML/LocaCZL.xml" + "');" +
                  "chart5.setTransparent(true);chart5.render('chartdiv5');" +
                  "</script>" ;
        return strHtml; 
    }
    /// <summary>
    /// 根据项目生成
    /// </summary>
    /// <param name="storeid"></param>
    public void GetChartSql(int storeid)
    {
 
    }
    /// <summary>
    /// 生成FusionChart的类型Session对象
    /// </summary>
    /// <param name="strType">FusionChart名称</param>
    public void SetChartType(string strType)
    {
        Session["ChartType"] = strType;
    }
}
