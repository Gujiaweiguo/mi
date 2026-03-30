using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
public partial class MasterPage_mpReport : System.Web.UI.MasterPage 
{
    private string strTitle;
    private string strSql = "";
    protected string strJs;
    /// <summary>
    /// 页面标题
    /// </summary>
    public string PageTitle
    {
        get { return this.strTitle; }
        set { this.strTitle = value; }
    }
    /// <summary>
    /// 查询sql
    /// </summary>
    public string StrSql
    {
        get { return this.strSql; }
        set { this.strSql = value; }
    }   
    /* Page Load  */
    protected void Page_Load(object sender, EventArgs e)
    {
        //设置页面标题
        if(!String.IsNullOrEmpty(PageTitle))
        {
            Page.Header.Title = PageTitle + Page.Header.Title; 
        }  

        //设置sigmagrid
        SetSigmaGrid();
        strJs = GetSigmaGridStr("contentgrid");

    }
    /// <summary>
    /// 获得SigmaGrid的JS字符串
    /// </summary>
    /// <returns></returns>
    private string GetSigmaGridStr(string strDiv)
    {
        string strTemp = "";
        if (strSql != "")
        {
            strTemp = BaseInfo.SigmaGrid.GetGrid(strSql, strDiv);
        }
        return strTemp;
    }
    private void SetSigmaGrid()
    {
        if(!IsPostBack)
        {
            RegisterScript("gt_grid_all", "grid/gt_grid_all.js");
            RegisterScript("gt_msg_en", "grid/gt_msg_en.js");
            RegisterScript("gridFunc", "grid/gridFunc.js");

            RegisterCSSLink("grid/gt_grid.css");
            RegisterCSSLink("grid/skin/vista/skinstyle.css");
            RegisterCSSLink("grid/skin/china/skinstyle.css");
            RegisterCSSLink("grid/skin/mac/skinstyle.css");
        }
    }
    
    public void RegisterScript(string key, string url)
    {
        Page.ClientScript.RegisterClientScriptInclude(key, Page.ResolveUrl(url));
    }
    public void RegisterCSSLink(string url)
    {
        HtmlLink link = new HtmlLink();
        Page.Header.Controls.Add(link);
        link.EnableViewState = false;
        link.Attributes.Add("type", "text/css");
        link.Attributes.Add("rel", "stylesheet");
        link.Href = url;
    }

}