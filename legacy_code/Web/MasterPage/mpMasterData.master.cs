using System;
using System.Web.UI;

public partial class MasterPage_mpMasterData : System.Web.UI.MasterPage
{
    private string strTitle;
    public  string strFresh;
    private string strSql = "";
    public string strPage;
    protected string strJs;
    /// <summary>
    /// 页面选项卡名称
    /// </summary>
    public string PageFresh
    {
        get { return this.strFresh; }
        set { this.strFresh = value; }
    }
    /// <summary>
    /// 页面选项卡路径
    /// </summary>
    public string PagePath
    {
        get { return this.strPage; }
        set { this.strPage = value; }
    }
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
            Page.Header.Title = PageTitle ; 
        }  

        //设置sigmagrid
        if(this.StrSql!="")
            strJs = GetSigmaGridStr("datagrid");

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

    
}
