using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Dept;

public partial class Sell_ComputerShopMonthSales : BasePage
{
    public string baseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        if (!this.IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_ShopMonthSales");
            this.BindStore();
            this.txtDate.Text = DateTime.Now.ToString("yyyy-MM-01");
        }
    }
    /// <summary>
    /// 绑定商业项目
    /// </summary>
    private void BindStore()
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "depttype='" + Dept.DEPT_TYPE_MALL + "'";
        objBaseBo.OrderBy = "orderid";
        this.ddlStore.Items.Add(new ListItem("", ""));
        Resultset rs = objBaseBo.Query(new Dept());
        foreach (Dept objDept in rs)
        {
            this.ddlStore.Items.Add(new ListItem(objDept.DeptName, objDept.DeptID.ToString()));
        }
    }
    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        this.txtDate.Text = DateTime.Now.ToString("yyyy-MM-01");
        this.ddlStore.SelectedIndex = -1;
    }
    /// <summary>
    /// 确定（执行存储过程）
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            BaseBO objBaseBo = new BaseBO();
            objBaseBo.ExecuteUpdate("EXEC	 [dbo].[SPMI_ComputerShopMonthSales]@StoreID = '" + this.ddlStore.SelectedValue.ToString() + "',@BizMth = '" + this.txtDate.Text.Trim() + "'");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Succed") + "'", true);
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
        }
    }
}
