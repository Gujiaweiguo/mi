using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Dept;


public partial class Sell_ComputerShopDaySales : BasePage
{
    public string baseInfo;
    private Hashtable dataSources = null;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        if (!this.IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_ShopDaySales");
            this.BindStore();
            this.txtStartDate.Text =DateTime.Now.ToString("yyyy-MM-dd");
            this.txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
    }
    /// <summary>
    /// 绑定商业项目
    /// </summary>
    private void BindStore()
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "depttype='" + Dept.DEPT_TYPE_MALL + "'";
        objBaseBo.OrderBy = " orderid ";
        this.ddlStore.Items.Add(new ListItem("",""));
        Resultset rs = objBaseBo.Query(new Dept());
        foreach (Dept objDept in rs)
        {
            this.ddlStore.Items.Add(new ListItem(objDept.DeptName,objDept.DeptID.ToString()));
        }
    }
    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        this.txtStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        this.txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        this.ddlStore.SelectedIndex = -1;
        
    }
    /// <summary>
    /// 确定(调用存储过程)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int i = 0;
        TimeSpan ts = DateTime.Parse(txtEndDate.Text).Subtract(DateTime.Parse(txtStartDate.Text));
        int bwt = ts.Days;
        if (bwt >= 0)
        {
            try
            {
                BaseBO objBaseBo = new BaseBO();
                for (i = 0; i <= bwt; i++)
                {
                    objBaseBo.ExecuteUpdate("EXEC	 [dbo].[SPMI_ComputerShopDaySales]@StoreID = '" + this.ddlStore.SelectedValue.ToString() + "',@BizDate = '" + DateTime.Parse(txtStartDate.Text.Trim()).AddDays(i) + "'");
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Succed") + "'", true);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + DateTime.Parse(txtStartDate.Text.Trim()).AddDays(i).ToString("yyyy-MM-dd") + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '结束日期不能小于开始日期。'", true);
        }
    }
}
