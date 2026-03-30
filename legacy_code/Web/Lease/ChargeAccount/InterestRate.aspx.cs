using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.Biz;
using Base.DB;
using Base;
using BaseInfo.User;
using Lease.Contract;
using Lease.PotBargain;
using Base.Page;
using Invoice;

public partial class Lease_ChargeAccount_InterestRate : BasePage
{
    public string baseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Lease_InterestRateModi");
            btnEdit.Enabled = false;
            btnBlankOut.Enabled = false;
            BindDrop();
            BindGrid();
        }
        btnSave.Attributes.Add("onclick", "return CheckData()");
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
    }

    private void BindDrop()
    {
        dropChargeType.Items.Clear();
        BaseBO baseBO = new BaseBO();
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        Resultset rs = baseBO.Query(new ChargeType());
        dropChargeType.Items.Add(new ListItem(selected, "-1"));
        foreach (ChargeType chargeType in rs)
            dropChargeType.Items.Add(new ListItem(chargeType.ChargeTypeName, chargeType.ChargeTypeID.ToString()));
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        if (txtContractCode.Text.Trim() != "" && dropChargeType.SelectedValue != "-1" && txtInterestRate.Text.Trim() != "")
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "ContractCode = '" + txtContractCode.Text + "'";
            Resultset rs = baseBO.Query(new Contract());
            
            if (rs.Count > 0)
            {
                Contract contract = rs.Dequeue() as Contract;
                decimal rate = InvoiceInterestPO.GetInterestRate(contract.ContractID, Convert.ToInt32(dropChargeType.SelectedValue));
                if (rate == -1)
                {
                    InterestRate interestRate = new InterestRate();
                    interestRate.InterestRateID = BaseApp.GetInterestRateID();
                    interestRate.ContractID = contract.ContractID;
                    interestRate.ChargeTypeID = Convert.ToInt32(dropChargeType.SelectedValue);
                    interestRate.IntRate = Convert.ToDecimal(txtInterestRate.Text) / 1000;
                    interestRate.OprDeptID = sessionUser.DeptID;
                    interestRate.OprRoleID = sessionUser.RoleID;
                    interestRate.CreateUserID = sessionUser.UserID;
                    int result = InterestRatePO.InsertInterestRate(interestRate);
                    if (result == 1)
                    {
                        clearText();
                    }
                }
                else
                {
                    //该合同的此费用类型已存在滞纳金利率
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Lease_ExistInterestRate") + "'", true);
                }
            }
            else
            {
                //合同号无效
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contract_No") + "'", true);
            }
        }
        else
        {
            //请输入信息
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtInterestRate.select()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '请选择费用类别。'", true);
        }
        BindGrid();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        if (txtContractCode.Text.Trim() != "" && dropChargeType.SelectedValue != "-1" && txtInterestRate.Text.Trim() != "")
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "ContractCode = '" + txtContractCode.Text + "'";
            Resultset rs = baseBO.Query(new Contract());
            if (rs.Count > 0)
            {
                Contract contract = rs.Dequeue() as Contract;
                InterestRate interestRate = new InterestRate();
                interestRate.InterestRateID = Convert.ToInt32(gvChargeType.SelectedRow.Cells[0].Text);
                interestRate.ContractID = contract.ContractID;
                interestRate.ChargeTypeID = Convert.ToInt32(dropChargeType.SelectedValue);
                interestRate.IntRate = Convert.ToDecimal(txtInterestRate.Text) / 1000;
                interestRate.ModifyUserID = sessionUser.UserID;
                interestRate.OprDeptID = sessionUser.DeptID;
                interestRate.OprRoleID = sessionUser.RoleID;
                int result = InterestRatePO.UpdateInterestRate(interestRate);                
                if (result == 1)
                {
                    clearText();
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "'", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }
            }
            else
            {
                //合同号无效
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contract_No") + "'", true);
            }
   
        }
        else
        {
            //请输入信息
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
        }
        btnEdit.Enabled = false;
        btnBlankOut.Enabled = false;
        btnSave.Enabled = true;
        BindGrid();
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        int interestRateID = Convert.ToInt32(gvChargeType.SelectedRow.Cells[0].Text);
        int result = InterestRatePO.DelInterestRate(interestRateID);
        if (result == 1)
        {
            clearText();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDelete") + "'", true);
            btnEdit.Enabled = false;
            btnBlankOut.Enabled = false;
            btnSave.Enabled = true;
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
        }
        
        BindGrid();
    }
    //protected void btnBack_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
    //    BindGrid();
    //}
    //protected void btnNext_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
    //    BindGrid();
    //}
    protected void gvChargeType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[4].Text = "";
            }
        }
    }
    protected void gvChargeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Resultset rs = InterestRatePO.GetInterestRateByID(Convert.ToInt32(gvChargeType.SelectedRow.Cells[0].Text));
        InterestRate intRate = rs.Dequeue() as InterestRate;
        txtContractCode.Text = gvChargeType.SelectedRow.Cells[1].Text;
        dropChargeType.SelectedItem.Text = gvChargeType.SelectedRow.Cells[2].Text;
        dropChargeType.SelectedValue = intRate.ChargeTypeID.ToString();
        txtInterestRate.Text = gvChargeType.SelectedRow.Cells[3].Text;

        btnBlankOut.Enabled = true;
        btnEdit.Enabled = true;
        btnSave.Enabled = false;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        BindGrid();
    }

    private void clearText()
    {
        txtContractCode.Text = "";
        txtInterestRate.Text = "";
        BindDrop();
    }

    private void BindGrid()
    {
        //BaseBO baseBo = new BaseBO();
        //PagedDataSource pds = new PagedDataSource();
        //int spareRow = 0;

        DataSet ds = InterestRatePO.GetInterestRate();
        //DataTable dt = ds.Tables[0];

        //int count = dt.Rows.Count;

        //pds.DataSource = dt.DefaultView;

        //if (pds.Count < 1)
        //{
        //    for (int i = 0; i < gvChargeType.PageSize; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    gvChargeType.DataSource = pds;
        //    gvChargeType.DataBind();
        //}
        //else
        //{
        //    gvChargeType.EmptyDataText = "";
        //    pds.AllowPaging = true;
        //    pds.PageSize = 10;
        //    lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
        //    pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;
        //    if (pds.IsFirstPage)
        //    {
        //        btnBack.Enabled = false;
        //        btnNext.Enabled = true;
        //    }

        //    if (pds.IsLastPage)
        //    {
        //        btnBack.Enabled = true;
        //        btnNext.Enabled = false;
        //    }

        //    if (pds.IsFirstPage && pds.IsLastPage)
        //    {
        //        btnBack.Enabled = false;
        //        btnNext.Enabled = false;
        //    }

        //    if (!pds.IsLastPage && !pds.IsFirstPage)
        //    {
        //        btnBack.Enabled = true;
        //        btnNext.Enabled = true;
        //    }

        //    this.gvChargeType.DataSource = pds;
        //    this.gvChargeType.DataBind();
        //    spareRow = gvChargeType.Rows.Count;
        //    for (int i = 0; i < pds.PageSize - spareRow; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    gvChargeType.DataSource = pds;
        //    gvChargeType.DataBind();
        //}

        int spareRow = 0;
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        //baseBO.WhereClause = "";
        //baseBO.OrderBy = "BrandName";
        //DataSet ds = baseBO.QueryDataSet(new InterestRate());
        dt = ds.Tables[0];
        pds.DataSource = dt.DefaultView;
        gvChargeType.DataSource = pds;
        gvChargeType.DataBind();
        spareRow = gvChargeType.Rows.Count;
        for (int i = 0; i < gvChargeType.PageSize - spareRow; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        gvChargeType.DataSource = pds;
        gvChargeType.DataBind();

    }

    protected void gvChargeType_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView theGrid = sender as GridView;
        int newPageIndex = 0;
        if (-2 == e.NewPageIndex)
        {
            TextBox txtNewPageIndex = null;
            GridViewRow pagerRow = theGrid.BottomPagerRow;
            if (null != pagerRow)
            {
                txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;
            }
            if (null != txtNewPageIndex)
            {
                newPageIndex = int.Parse(txtNewPageIndex.Text) - 1;
            }
        }
        else
        { newPageIndex = e.NewPageIndex; }
        newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
        newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;
        theGrid.PageIndex = newPageIndex;
        BindGrid();
    }


}
