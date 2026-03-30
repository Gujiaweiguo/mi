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

using Base.Biz;
using Base.DB;
using Base;
using Lease;
using Lease.PotCustLicense;
using Lease.ConShop;
using RentableArea;
using Lease.PotBargain;
using Lease.Formula;
using Lease.Customer;
using Lease.Contract;
using WorkFlow.WrkFlw;
using WorkFlow;
using WorkFlow.Uiltil;
using BaseInfo.User;
using System.Text;
using Base.Page;

public partial class Lease_ExpressionPalaver : BasePage
{
    BaseBO baseBo = new BaseBO();
    Resultset rs = new Resultset();
    BaseTrans baseTrans = new BaseTrans();
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    DataSet DeductMoneyDS = new DataSet();
    DataTable DeductMoneyDT = new DataTable();
    DataSet KeepMinDS = new DataSet();
    DataTable KeepMinDT = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (Request.Cookies["Info"].Values["conID"] != "")
            {
                 ViewState["contractID"] = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
            }
            else
            {
                 ViewState["contractID"] = "0";
            }

            BindChargeType();
            BindFormulaType();

            GetConFormulaInfo();
            BindGVType();
            BindGVDeductMoney();
            BindGVKeepMin();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Publicmessage", "Load();", true);
        }
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        DataSet tempDs = new DataSet();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = '" + this.Hidden1.Value + "'";
        tempDs = baseBo.QueryDataSet(new ConFormulaH());
        if (tempDs.Tables[0].Rows.Count > 0)
        {
            cmbChargeTypeID.SelectedValue = tempDs.Tables[0].Rows[0]["ChargeTypeID"].ToString();
            cmbFormulaType.SelectedValue = tempDs.Tables[0].Rows[0]["FormulaType"].ToString();
            txtBeginDate.Text = tempDs.Tables[0].Rows[0]["FStartDate"].ToString();
            txtOverDate.Text = tempDs.Tables[0].Rows[0]["FEndDate"].ToString();
            txtBaseAmt.Text = tempDs.Tables[0].Rows[0]["BaseAmt"].ToString();
            txtArea.Text = tempDs.Tables[0].Rows[0]["TotalArea"].ToString();
            txtUnitHire.Text = tempDs.Tables[0].Rows[0]["UnitPrice"].ToString();
            txtFixedRental.Text = tempDs.Tables[0].Rows[0]["FixedRental"].ToString();
            rabMonthHire.Checked = false;
            rabDayHire.Checked = false;
            rabFastness.Checked = false;
            rabMultilevel.Checked = false;
            rabMonopole.Checked = false;
            rabFastness2.Checked = false;
            rabLevel.Checked = false;
            if (tempDs.Tables[0].Rows[0]["RateType"].ToString() == ConFormulaH.RATETYPE_TYPE_MONTH)
                rabMonthHire.Checked = true;
            if (tempDs.Tables[0].Rows[0]["RateType"].ToString() == ConFormulaH.RATETYPE_TYPE_DAY)
                rabDayHire.Checked = true;
            if (tempDs.Tables[0].Rows[0]["PcentOpt"].ToString() == ConFormulaH.PCENTOPT_TYPE_FAST)
                rabFastness.Checked = true;
            if (tempDs.Tables[0].Rows[0]["PcentOpt"].ToString() == ConFormulaH.PCENTOPT_TYPE_S)
                rabMonopole.Checked = true;
            if (tempDs.Tables[0].Rows[0]["PcentOpt"].ToString() == ConFormulaH.PCENTOPT_TYPE_M)
                rabMultilevel.Checked = true;
            if (tempDs.Tables[0].Rows[0]["MinSumOpt"].ToString() == ConFormulaH.MINSUMOPT_TYPE_FAST)
                rabFastness2.Checked = true;
            if (tempDs.Tables[0].Rows[0]["MinSumOpt"].ToString() == ConFormulaH.MINSUMOPT_TYPE_T)
                rabLevel.Checked = true;

            baseBo.WhereClause = "";
            baseBo.WhereClause = "FormulaID = '" + this.Hidden1.Value + "'";
            tempDs.Clear();
            tempDs = baseBo.QueryDataSet(new ConFormulaP());
            DeductMoneyDT = tempDs.Tables[0];
            int count1 = DeductMoneyDT.Rows.Count;
            int ss1 = 10 - count1;
            for (int i = 0; i < ss1; i++)
            {
                DeductMoneyDT.Rows.Add(DeductMoneyDT.NewRow());
            }
            GVDeductMoney.DataSource = DeductMoneyDT;
            GVDeductMoney.DataBind();

            baseBo.WhereClause = "";
            baseBo.WhereClause = "FormulaID = '" + this.Hidden1.Value + "'";
            tempDs.Clear();
            tempDs = baseBo.QueryDataSet(new ConFormulaM());
            KeepMinDT = tempDs.Tables[0];
            int count2 = KeepMinDT.Rows.Count;
            int ss2 = 10 - count2;
            for (int j = 0; j < ss2; j++)
            {
                KeepMinDT.Rows.Add(KeepMinDT.NewRow());
            }
            GVKeepMin.DataSource = KeepMinDT;
            GVKeepMin.DataBind();

            GVDeductMoney.Enabled = true;
            GVKeepMin.Enabled = true;

            ViewState["formulaHID"] = this.Hidden1.Value;
        }
        BindGVDeductMoney();
        BindGVKeepMin();
    }
    protected void btnDeduct_Click(object sender, EventArgs e)
    {

    }
    protected void btnKeepMin_Click(object sender, EventArgs e)
    {

    }
    protected void GVType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            string gIntro = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //鼠标移动到每项时颜色交替效果
                e.Row.Attributes.Add("onmouseover", "if(this!=prevselitem){this.style.backgroundColor='#FFFFCD';this.style.color='#003399'}");//当鼠标停留时更改背景色 
                e.Row.Attributes.Add("onmouseout", "if(this!=prevselitem){this.style.backgroundColor='#ffffff';this.style.color='#000000'}");//当鼠标移开时还原背景色 
                //单击事件
                e.Row.Attributes.Add("OnClick", "ClickEvent('" + e.Row.Cells[0].Text + "')");
            }
            //gIntro = e.Row.Cells[2].Text.ToString();
            //e.Row.Cells[2].Text = SubStr(gIntro, 10);
            //gIntro = e.Row.Cells[3].Text.ToString();
            //e.Row.Cells[3].Text = SubStr(gIntro, 10);
        }
    }

    protected void GVType_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void GVDeductMoney_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //鼠标移动到每项时颜色交替效果
                e.Row.Attributes.Add("onmouseover", "if(this!=prevselitem){this.style.backgroundColor='#FFFFCD';this.style.color='#003399'}");//当鼠标停留时更改背景色 
                e.Row.Attributes.Add("onmouseout", "if(this!=prevselitem){this.style.backgroundColor='#ffffff';this.style.color='#000000'}");//当鼠标移开时还原背景色 
            }
        }
    }
    protected void GVKeepMin_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //鼠标移动到每项时颜色交替效果
                e.Row.Attributes.Add("onmouseover", "if(this!=prevselitem){this.style.backgroundColor='#FFFFCD';this.style.color='#003399'}");//当鼠标停留时更改背景色 
                e.Row.Attributes.Add("onmouseout", "if(this!=prevselitem){this.style.backgroundColor='#ffffff';this.style.color='#000000'}");//当鼠标移开时还原背景色 
            }
        }
    }

    //绑定费用类别
    protected void BindChargeType()
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        rs = baseBo.Query(new ChargeType());
        cmbChargeTypeID.Items.Add(new ListItem(selected, "-1"));
        foreach (ChargeType chargeType in rs)
        {
            cmbChargeTypeID.Items.Add(new ListItem(chargeType.ChargeTypeName, chargeType.ChargeTypeID.ToString()));
        }
    }

    //绑定公式类别
    protected void BindFormulaType()
    {
        string[] status = ConFormulaH.GetFormulaTypeStatus();
        int s = status.Length;
        for (int i = 0; i < s; i++)
            cmbFormulaType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ConFormulaH.GetFormulaTypeStatusDesc(status[i])), status[i].ToString()));
    }

    #region 获取公式列表中的信息
    protected void GetConFormulaInfo()
    {
        baseBo.WhereClause = "";
        baseBo.OrderBy = "ChargeTypeID,FStartDate";
        if (Request.Cookies["Info"].Values["conID"] != "")
        {
            baseBo.WhereClause = "ContractID = " + ViewState["contractID"];
        }
        rs = baseBo.Query(new ConFormulaH());
        ds = baseBo.QueryDataSet(new ConFormulaH());
        if (rs.Count > 0)
        {
            ConFormulaH formulaH = rs.Dequeue() as ConFormulaH;
            cmbChargeTypeID.SelectedValue = formulaH.ChargeTypeID.ToString();
            cmbFormulaType.SelectedValue = formulaH.FormulaType.ToString();
            txtBeginDate.Text = formulaH.FStartDate.ToString("yyyy-MM-dd");
            txtOverDate.Text = formulaH.FEndDate.ToString("yyyy-MM-dd");
            txtBaseAmt.Text = formulaH.BaseAmt.ToString();
            rabMonthHire.Checked = false;
            rabDayHire.Checked = false;
            if (formulaH.RateType == ConFormulaH.RATETYPE_TYPE_MONTH)
                rabMonthHire.Checked = true;
            if (formulaH.RateType == ConFormulaH.RATETYPE_TYPE_DAY)
                rabDayHire.Checked = true;
            //txtArea.Text = formulaH.TotalArea.ToString();
            txtUnitHire.Text = formulaH.UnitPrice.ToString();
            txtFixedRental.Text = formulaH.FixedRental.ToString();
            rabFastness.Checked = false;
            rabMultilevel.Checked = false;
            rabMonopole.Checked = false;
            rabFastness2.Checked = false;
            rabLevel.Checked = false;
            if (formulaH.PcentOpt == ConFormulaH.PCENTOPT_TYPE_FAST)
                rabFastness.Checked = true;
            if (formulaH.PcentOpt == ConFormulaH.PCENTOPT_TYPE_M)
                rabMultilevel.Checked = true;
            if (formulaH.PcentOpt == ConFormulaH.PCENTOPT_TYPE_S)
                rabMonopole.Checked = true;
            if (formulaH.MinSumOpt == ConFormulaH.MINSUMOPT_TYPE_FAST)
                rabFastness2.Checked = true;
            if (formulaH.MinSumOpt == ConFormulaH.MINSUMOPT_TYPE_T)
                rabLevel.Checked = true;
            //GVType.DataSource = ds.Tables[0];
            //GVType.DataBind();
            GVType.Enabled = true;
        }
        else
            GVType.Enabled = false;
        //txtArea.Text = ViewState["shopArea"].ToString();
    }
    #endregion

    #region GridView绑定
    protected void BindGVType()
    {
        ds.Clear();
        baseBo.WhereClause = "";
        baseBo.OrderBy = "ChargeTypeID,FStartDate,FEndDate Desc";
        baseBo.WhereClause = "ContractID = '" + ViewState["contractID"] + "'";

        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;


        DataTable dt = baseBo.QueryDataSet(new ConFormulaH()).Tables[0];
        dt.Columns.Add("StartDate");
        dt.Columns.Add("EndDate");

        int count = dt.Rows.Count;

        //获取费用类别名字
        for (int j = 0; j < count; j++)
        {
            baseBo.WhereClause = "";
            baseBo.OrderBy = "";
            baseBo.WhereClause = "ChargeTypeID = " + dt.Rows[j]["ChargeTypeID"];
            DataSet tempDs = new DataSet();
            tempDs = baseBo.QueryDataSet(new ChargeType());

            //dt.Rows[j]["ChargeTypeName"] = (String)GetGlobalResourceObject("Parameter", aaa);
            dt.Rows[j]["ChargeTypeName"] = tempDs.Tables[0].Rows[0]["ChargeTypeName"].ToString();
        }

        //获取公式类别名称
        for (int j = 0; j < count; j++)
        {
            string formulaTypeName = (String)GetGlobalResourceObject("Parameter", ConFormulaH.GetFormulaTypeStatusDesc(dt.Rows[j]["FormulaType"].ToString()));
            dt.Rows[j]["FormulaTypeName"] = formulaTypeName;
            dt.Rows[j]["StartDate"] = Convert.ToDateTime(dt.Rows[j]["FStartDate"]).ToString("yyyy-MM-dd");
            dt.Rows[j]["EndDate"] = Convert.ToDateTime(dt.Rows[j]["FEndDate"]).ToString("yyyy-MM-dd");
        }

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GVType.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GVType.DataSource = pds;
            GVType.DataBind();
        }
        else
        {
            //GVType.EmptyDataText = "";
            //pds.AllowPaging = true;
            //pds.PageSize = 12;
            //lblTotalNumType.Text = "/" + pds.PageCount.ToString() + " page";
            //pds.CurrentPageIndex = int.Parse(lblCurrentType.Text) - 1;
            //if (pds.IsFirstPage)
            //{
            //    bt1Back.Enabled = false;
            //    bt1Next.Enabled = true;
            //}

            //if (pds.IsLastPage)
            //{
            //    bt1Back.Enabled = true;
            //    bt1Next.Enabled = false;
            //}

            //if (pds.IsFirstPage && pds.IsLastPage)
            //{
            //    bt1Back.Enabled = false;
            //    bt1Next.Enabled = false;
            //}

            //if (!pds.IsLastPage && !pds.IsFirstPage)
            //{
            //    bt1Back.Enabled = true;
            //    bt1Next.Enabled = true;
            //}

            this.GVType.DataSource = pds;
            this.GVType.DataBind();
            spareRow = GVType.Rows.Count;
            for (int i = 0; i < GVType.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GVType.DataSource = pds;
            GVType.DataBind();
        }
    }

    public string SubStr(string sString, int nLeng)
    {
        if (sString.Length <= nLeng)
        {
            return sString;
        }
        string sNewStr = sString.Substring(0, nLeng);
        return sNewStr;
    }

    protected void BindGVDeductMoney()
    {
        baseBo.WhereClause = "";
        baseBo.OrderBy = "";
        baseBo.WhereClause = "FormulaID = " + Convert.ToInt32(ViewState["formulaHID"]);
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;


        DataTable dt = baseBo.QueryDataSet(new ConFormulaP()).Tables[0];
        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GVDeductMoney.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GVDeductMoney.DataSource = pds;
            GVDeductMoney.DataBind();
        }
        else
        {
            //GVDeductMoney.EmptyDataText = "";
            //pds.AllowPaging = true;
            //pds.PageSize = 10;
            //lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
            //pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;
            //if (pds.IsFirstPage)
            //{
            //    btnBack.Enabled = false;
            //    btnNext.Enabled = true;
            //}

            //if (pds.IsLastPage)
            //{
            //    btnBack.Enabled = true;
            //    btnNext.Enabled = false;
            //}

            //if (pds.IsFirstPage && pds.IsLastPage)
            //{
            //    btnBack.Enabled = false;
            //    btnNext.Enabled = false;
            //}

            //if (!pds.IsLastPage && !pds.IsFirstPage)
            //{
            //    btnBack.Enabled = true;
            //    btnNext.Enabled = true;
            //}

            this.GVDeductMoney.DataSource = pds;
            this.GVDeductMoney.DataBind();
            spareRow = GVDeductMoney.Rows.Count;
            for (int i = 0; i < GVDeductMoney.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GVDeductMoney.DataSource = pds;
            GVDeductMoney.DataBind();
        }
    }

    protected void BindGVKeepMin()
    {
        baseBo.WhereClause = "";
        baseBo.OrderBy = "";
        baseBo.WhereClause = "FormulaID = " + Convert.ToInt32(ViewState["formulaHID"]);

        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;


        DataTable dt = baseBo.QueryDataSet(new ConFormulaM()).Tables[0];
        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GVKeepMin.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GVKeepMin.DataSource = pds;
            GVKeepMin.DataBind();
        }
        else
        {
            //GVKeepMin.EmptyDataText = "";
            //pds.AllowPaging = true;
            //pds.PageSize = 10;
            //lblTotalNumT.Text = "/" + pds.PageCount.ToString() + " page";
            //pds.CurrentPageIndex = int.Parse(lblCurrentT.Text) - 1;
            //if (pds.IsFirstPage)
            //{
            //    bt2Back.Enabled = false;
            //    bt2Next.Enabled = true;
            //}

            //if (pds.IsLastPage)
            //{
            //    bt2Back.Enabled = true;
            //    bt2Next.Enabled = false;
            //}

            //if (pds.IsFirstPage && pds.IsLastPage)
            //{
            //    bt2Back.Enabled = false;
            //    bt2Next.Enabled = false;
            //}

            //if (!pds.IsLastPage && !pds.IsFirstPage)
            //{
            //    bt2Back.Enabled = true;
            //    bt2Next.Enabled = true;
            //}

            this.GVKeepMin.DataSource = pds;
            this.GVKeepMin.DataBind();
            spareRow = GVKeepMin.Rows.Count;
            for (int i = 0; i < GVKeepMin.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GVKeepMin.DataSource = pds;
            GVKeepMin.DataBind();
        }
    }
    #endregion

    protected void GrdBrandOperateType_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        this.BindGVType();
        BindGVDeductMoney();
        BindGVKeepMin();
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        //foreach (GridViewRow grv in GrdBrandOperateType.Rows)
        //{
        //    grv.BackColor = Color.White;
        //}
    }
}
