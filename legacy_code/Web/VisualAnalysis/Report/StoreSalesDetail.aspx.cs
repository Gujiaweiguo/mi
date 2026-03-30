using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Base.Biz;
using System.Data;
using Base.XML;
using BaseInfo.User;
using BaseInfo.Dept;
using System.IO;
public partial class VisualAnalysis_Report_StoreSalesDetail : System.Web.UI.Page
{
    protected string deptName = "";
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void PreLoad()
    {
         deptName = "项目销售与客流趋势分析";
         if (Request.QueryString["MallID"] != null)
         {
             BaseBO baseBo = new BaseBO();
             baseBo.WhereClause = "DeptID=" + Request.QueryString["MallID"].ToString();
             DataSet dt = baseBo.QueryDataSet(new Dept());
             if (dt.Tables[0].Rows.Count == 1)
             {
                 deptName = dt.Tables[0].Rows[0]["DeptName"].ToString() + deptName;
             }


             MSCol3DLineXML msxml = new MSCol3DLineXML();
             SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
             ChartLine2D line = new ChartLine2D();
             string strSql = @"select top 12 CONVERT(varchar(7), TransShopMth.Month, 120) '月份',isnull(SUM(TransShopMth.paidamt),0)  '本月销售',isnull(SUM(TransShopMth.lypaidamt),0) '环比销售' " +
                 "from TransShopMth where StoreId=" + Request.QueryString["MallID"].ToString() +
                 " group by TransShopMth.Month order by TransShopMth.Month";
             dt = baseBo.QueryDataSet(strSql);
             if (dt.Tables[0].Rows.Count > 0)
             {
                 string strFile = "E://work//mi_net//code//web//VisualAnalysis//VAMenu//" + sessionUser.UserID.ToString() + "//StoreSalesDetail1.xml";
                 if (File.Exists(strFile))
                     File.Delete(strFile);

                 msxml.parentYAxis = false;
                 msxml.Caption = "项目近12个月销售趋势图(单位:元）";
                 msxml.GetXml(strFile, dt.Tables[0]);
                 this.xmlFile.InnerText = "../../VisualAnalysis/VAMenu/" + sessionUser.UserID.ToString() + "/StoreSalesDetail1.xml";
             }

             strSql = "select tradename,sum(paidamt) saleamt from transshopmth " +
                      "where storeid="  + Request.QueryString["MallID"].ToString() +
                      " and month='" + DateTime.Now.Date.AddMonths(-1).ToString("yyyy-MM-01") + "' group by tradename";
             dt = baseBo.QueryDataSet(strSql);
             if (dt.Tables[0].Rows.Count > 0)
             {
                 string strFile = "E://work//mi_net//code//web//VisualAnalysis//VAMenu//" + sessionUser.UserID.ToString() + "//StoreSalesDetail2.xml";
                 if (File.Exists(strFile))
                     File.Delete(strFile);
                 Pie3DXML pie = new Pie3DXML();
                 pie.Caption = "项目" + DateTime.Now.Date.AddMonths(-1).ToString("yyyy年MM月") + "业态销售汇总";
                 pie.GetXml(strFile, dt.Tables[0]);
                 this.shopsale.InnerText = "../../VisualAnalysis/VAMenu/" + sessionUser.UserID.ToString() + "/StoreSalesDetail2.xml";
             }


             strSql = "select top 5 conshop.shopname,isnull(transshopmth.paidamt,0) '销售金额',isnull(aa.invamt,0) '应付租金'" +
                      " from transshopmth inner join conshop on (conshop.shopid=transshopmth.shopid)" +
                      " inner join contract on (contract.contractid=conshop.contractid) left join (" +
                        " select invoiceheader.contractid,sum(invoicedetail.invPayAmtl) InvAmt from invoiceheader" +
                        " inner join invoicedetail on (invoiceheader.invid=invoicedetail.invid) where invoicedetail.period='" + DateTime.Now.Date.AddMonths(-1).ToString("yyyy-MM-01") + "'" +
                        " group by invoiceheader.contractid) aa on (aa.contractid=contract.contractid)" +
                    " where transshopmth.month='" + DateTime.Now.Date.AddMonths(-1).ToString("yyyy-MM-01") + "'" +
                    " and conshop.storeid=" + Request.QueryString["MallID"].ToString() +
                    " order by transshopmth.paidamt desc";
             dt = baseBo.QueryDataSet(strSql);
             if (dt.Tables[0].Rows.Count > 0)
             {
                 string strFile = "E://work//mi_net//code//web//VisualAnalysis//VAMenu//" + sessionUser.UserID.ToString() + "//StoreSalesDetail3.xml";
                 if (File.Exists(strFile))
                     File.Delete(strFile);
                 StCol3D stcol = new StCol3D();
                 stcol.Caption = "项目" + DateTime.Now.Date.AddMonths(-1).ToString("yyyy年MM月") + "销售前5名";
                 stcol.GetXml(strFile, dt.Tables[0]);
                 this.xmlFile2.InnerText = "../../VisualAnalysis/VAMenu/" + sessionUser.UserID.ToString() + "/StoreSalesDetail3.xml";
             }
         }
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
}
