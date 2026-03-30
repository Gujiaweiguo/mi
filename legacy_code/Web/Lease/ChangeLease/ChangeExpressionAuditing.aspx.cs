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
using Lease.ConOvertimeBill;
using Base.Page;
using Lease.ChangeLease;
using Base.Util;

public partial class Lease_ChangeLease_ChangeExpressionAuditing : BasePage
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


    private static int CHARGECLASS_TRUE = 1;
    public string emptyStr;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {

            if (Request.Cookies["Info"].Values["conID"] != "")
            {
                ViewState["ContractID"] = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
            }

            if (Request.Cookies["Info"].Values["ConFormulaModID"] != "")
            {
                ViewState["ConFormulaModID"] = Convert.ToInt32(Request.Cookies["Info"].Values["ConFormulaModID"]);
            }

            BindChargeType();

            BindFormulaType();

            InitaExpressionsControls(false);

            BindGVType(Convert.ToInt32(ViewState["ConFormulaModID"]));

            BindGVDeductMoney();

            BindGVKeepMin();

            this.txtArea.Text = GetShopArea(Convert.ToInt32(ViewState["ContractID"])).ToString();
            ViewState["shopArea"] = GetShopArea(Convert.ToInt32(ViewState["ContractID"])).ToString();
            txtBaseAmt.Attributes.Add("onkeydown", "textleave()");
            txtUnitHire.Attributes.Add("onkeydown", "textleave()");
            txtUnitHire.Attributes.Add("onblur", "GetRental()");

            txtFixedRental.Attributes.Add("onkeydown", "textleave()");
            txtFore.Attributes.Add("onkeydown", "textleave()");
            txtForePer.Attributes.Add("onkeydown", "textleave()");
            txtForeKeepMin.Attributes.Add("onkeydown", "textleave()");
            txtForeKeep.Attributes.Add("onkeydown", "textleave()");
            emptyStr = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Publicmessage", "Load();", true);
        }
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        DataSet tempDs = new DataSet();
        baseBO.WhereClause = "FormulaID = '" + this.Hidden1.Value + "'";
        tempDs = baseBO.QueryDataSet(new ConFormulaHMod());
        if (tempDs.Tables[0].Rows.Count > 0)
        {
            cmbChargeTypeID.SelectedValue = tempDs.Tables[0].Rows[0]["ChargeTypeID"].ToString();
            cmbFormulaType.SelectedValue = tempDs.Tables[0].Rows[0]["FormulaType"].ToString();
            txtBeginDate.Text = Convert.ToDateTime(tempDs.Tables[0].Rows[0]["FStartDate"]).ToString("yyyy-MM-dd").ToString();
            txtOverDate.Text = Convert.ToDateTime(tempDs.Tables[0].Rows[0]["FEndDate"]).ToString("yyyy-MM-dd").ToString();
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

            baseBO.WhereClause = "";
            baseBO.WhereClause = "FormulaID = '" + this.Hidden1.Value + "'";
            tempDs.Clear();
            tempDs = baseBO.QueryDataSet(new ConFormulaPMod());
            DeductMoneyDT = tempDs.Tables[0];
            int count1 = DeductMoneyDT.Rows.Count;
            int ss1 = 5 - count1;
            for (int i = 0; i < ss1; i++)
            {
                DeductMoneyDT.Rows.Add(DeductMoneyDT.NewRow());
            }
            GVDeductMoney.DataSource = DeductMoneyDT;
            GVDeductMoney.DataBind();

            baseBO.WhereClause = "";
            baseBO.WhereClause = "FormulaID = '" + this.Hidden1.Value + "'";
            tempDs.Clear();
            tempDs = baseBO.QueryDataSet(new ConFormulaMMod());
            KeepMinDT = tempDs.Tables[0];
            int count2 = KeepMinDT.Rows.Count;
            int ss2 = 5 - count2;
            for (int j = 0; j < ss2; j++)
            {
                KeepMinDT.Rows.Add(KeepMinDT.NewRow());
            }
            GVKeepMin.DataSource = KeepMinDT;
            GVKeepMin.DataBind();

            /*编辑租金公式*/
            ViewState["ExpressionFlag"] = 1;
            GVType.Enabled = true;

            ViewState["formulaHID"] = this.Hidden1.Value;
        }
        else
        {
            txtFore.Enabled = false;
            txtForePer.Enabled = false;
            txtForeKeepMin.Enabled = false;
            txtForeKeep.Enabled = false;
            GVDeductMoney.Enabled = false;
            GVKeepMin.Enabled = false;
        }
    }
    protected void lBtnP_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "ConFormulaPModID = " + HiddenDeduct1.Value;
        DataSet tempDs = baseBO.QueryDataSet(new ConFormulaPMod());
        if (tempDs.Tables[0].Rows.Count > 0)
        {
            txtFore.Text = tempDs.Tables[0].Rows[0]["SalesTo"].ToString();
            txtForePer.Text = tempDs.Tables[0].Rows[0]["Pcent"].ToString();
            ViewState["deduct"] = HiddenDeduct1.Value;
        }
    }
    protected void lBtnM_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "ConFormulaMModID = " + HiddenKeepMin1.Value;
        DataSet tempDs = baseBO.QueryDataSet(new ConFormulaMMod());
        if (tempDs.Tables[0].Rows.Count > 0)
        {
            txtForeKeepMin.Text = tempDs.Tables[0].Rows[0]["SalesTo"].ToString();
            txtForeKeep.Text = tempDs.Tables[0].Rows[0]["MinSum"].ToString();
            ViewState["KeepMin"] = HiddenKeepMin1.Value;
        }
    }
    protected void GVType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //鼠标移动到每项时颜色交替效果

            e.Row.Attributes.Add("onmouseover", "if(this!=prevselitem){this.style.backgroundColor='#FFFFCD';this.style.color='#003399'}");//当鼠标停留时更改背景色 
            e.Row.Attributes.Add("onmouseout", "if(this!=prevselitem){this.style.backgroundColor='#ffffff';this.style.color='#000000'}");//当鼠标移开时还原背景色 
            //e.Row.Attributes.Add("onclick", e.Row.ClientID.ToString() + ".checked=true;selectx(this)"); 
            //e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor='White';this.style.color='#003399'");
            //e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor='#FFFFCD';this.style.color='#8C4510'");
            //单击事件
            e.Row.Attributes.Add("OnClick", "ClickEvent('" + e.Row.Cells[0].Text + "');");
        }

    }
    protected void GVDeductMoney_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //鼠标移动到每项时颜色交替效果
            e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor='White';this.style.color='#003399'");
            e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor='#FFFFCD';this.style.color='#8C4510'");

            //单击事件
            e.Row.Attributes.Add("OnClick", "ClickEventDeduct('" + e.Row.Cells[0].Text + "')");
        }
    }
    protected void GVKeepMin_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //鼠标移动到每项时颜色交替效果
            e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor='White';this.style.color='#003399'");
            e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor='#FFFFCD';this.style.color='#8C4510'");
            //单击事件
            e.Row.Attributes.Add("OnClick", "ClickEventKeepMin('" + e.Row.Cells[0].Text + "')");
        }
    }

    //绑定费用类别
    protected void BindChargeType()
    {
        BaseBO baseBO = new BaseBO();
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        baseBO.WhereClause = "ChargeClass <> " + ChargeType.CHARGECLASS_WATERORDLECT + " and ChargeClass <>" + ChargeType.CHARGECLASS_MAINTAIN + " and ChargeClass <> " + ChargeType.CHARGECLASS_OTHER;
        rs = baseBO.Query(new ChargeType());
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
        {
            cmbFormulaType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ConFormulaH.GetFormulaTypeStatusDesc(status[i])), status[i].ToString()));
        }
    }

    #region GridView绑定
    protected void BindGVType(int conFormulaModID)
    {
        ds.Clear();
        baseBo.WhereClause = "";

        baseBo.WhereClause = "ConFormulaModID = " + conFormulaModID;

        baseBo.OrderBy = "ChargeTypeID,FStartDate,FEndDate Desc";

        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;


        DataTable dt = baseBo.QueryDataSet(new ConFormulaHMod()).Tables[0];

        int count = dt.Rows.Count;

        //获取费用类别名字
        for (int j = 0; j < count; j++)
        {
            baseBo.WhereClause = "";
            baseBo.OrderBy = "";
            baseBo.WhereClause = "ChargeTypeID = " + dt.Rows[j]["ChargeTypeID"];
            DataSet tempDs = new DataSet();
            tempDs = baseBo.QueryDataSet(new ChargeType());

            dt.Rows[j]["ChargeTypeName"] = tempDs.Tables[0].Rows[0]["ChargeTypeName"].ToString();
        }

        //获取公式类别名称
        for (int j = 0; j < count; j++)
        {
            string formulaTypeName = (String)GetGlobalResourceObject("Parameter", ConFormulaH.GetFormulaTypeStatusDesc(dt.Rows[j]["FormulaType"].ToString()));
            dt.Rows[j]["FormulaTypeName"] = formulaTypeName;
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
            //pds.PageSize = 8;
            //lblTotalNumType.Text = "/" + pds.PageCount.ToString() + " page";
            //pds.CurrentPageIndex = int.Parse(lblCurrentType.Text) - 1;
            //if (pds.IsFirstPage)
            //{
            //    btnBackType.Enabled = false;
            //    btnNextType.Enabled = true;
            //}

            //if (pds.IsLastPage)
            //{
            //    btnBackType.Enabled = true;
            //    btnNextType.Enabled = false;
            //}

            //if (pds.IsFirstPage && pds.IsLastPage)
            //{
            //    btnBackType.Enabled = false;
            //    btnNextType.Enabled = false;
            //}

            //if (!pds.IsLastPage && !pds.IsFirstPage)
            //{
            //    btnBackType.Enabled = true;
            //    btnNextType.Enabled = true;
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


        DataTable dt = baseBo.QueryDataSet(new ConFormulaPMod()).Tables[0];
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
            //pds.PageSize = 5;
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


        DataTable dt = baseBo.QueryDataSet(new ConFormulaMMod()).Tables[0];
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
            //pds.PageSize = 5;
            //lblTotalNumT.Text = "/" + pds.PageCount.ToString() + " page";
            //pds.CurrentPageIndex = int.Parse(lblCurrentT.Text) - 1;
            //if (pds.IsFirstPage)
            //{
            //    btnBackT.Enabled = false;
            //    btnNextT.Enabled = true;
            //}

            //if (pds.IsLastPage)
            //{
            //    btnBackT.Enabled = true;
            //    btnNextT.Enabled = false;
            //}

            //if (pds.IsFirstPage && pds.IsLastPage)
            //{
            //    btnBackT.Enabled = false;
            //    btnNextT.Enabled = false;
            //}

            //if (!pds.IsLastPage && !pds.IsFirstPage)
            //{
            //    btnBackT.Enabled = true;
            //    btnNextT.Enabled = true;
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

    #region 初始化结算公式页面控件

    protected void InitaExpressionsControls(bool s)
    {
        //定义列表中的控件
        cmbChargeTypeID.Enabled = s;
        cmbFormulaType.Enabled = s;
        txtBeginDate.Enabled = s;
        txtOverDate.Enabled = s;

        //定义内容中的控件
        rabMonthHire.Enabled = s;
        rabDayHire.Enabled = s;
        txtUnitHire.Enabled = s;
        txtFixedRental.Enabled = s;
        rabFastness.Enabled = s;
        rabMonopole.Enabled = s;
        rabMultilevel.Enabled = s;
        rabFastness2.Enabled = s;
        rabLevel.Enabled = s;
        txtForePer.Enabled = s;
        txtFore.Enabled = s;
        txtForeKeep.Enabled = s;
        txtForeKeepMin.Enabled = s;
        //GVDeductMoney.Enabled = s;
        //GVKeepMin.Enabled = s;
    }
    #endregion

    private decimal GetShopArea(int conId)
    {
        decimal shopArea = 0m;
        baseBo.WhereClause = "";
        baseBo.WhereClause = "a.ShopTypeID=b.ShopTypeID and ContractID = " + conId;
        Resultset rs = baseBo.Query(new ConShop());
        if (rs.Count > 0)
        {
            foreach (ConShop conshop in rs)
            {
                shopArea += conshop.RentArea;
            }
        }
        return shopArea;
    }

    private void setControlEnable(bool s)
    {
        rabFastness.Enabled = s;
        rabMonopole.Enabled = s;
        rabMultilevel.Enabled = s;
        //GVDeductMoney.Enabled = s;
        txtFore.Enabled = s;
        txtForePer.Enabled = s;
        rabFastness2.Enabled = s;
        rabLevel.Enabled = s;
        //GVKeepMin.Enabled = s;
        txtForeKeepMin.Enabled = s;
        txtForeKeep.Enabled = s;
        cmbChargeTypeID.Enabled = true;
        cmbFormulaType.Enabled = true;
        txtBeginDate.Enabled = true;
        txtOverDate.Enabled = true;

        rabMonthHire.Enabled = !s;
        rabDayHire.Enabled = !s;
        txtUnitHire.Enabled = !s;
        txtFixedRental.Enabled = !s;

    }

    protected void GVDeductMoney_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVDeductMoney.PageIndex = e.NewPageIndex;
        BindGVDeductMoney();
        GVDeductMoney.DataBind();
    }
    protected void GVKeepMin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVKeepMin.PageIndex = e.NewPageIndex;
        BindGVKeepMin();
        GVKeepMin.DataBind();
    }
    //protected void btnNext_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
    //    BindGVDeductMoney();
    //}
    //protected void btnBack_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
    //    BindGVDeductMoney();
    //}
    //protected void btnBackT_Click(object sender, EventArgs e)
    //{
    //    lblCurrentT.Text = Convert.ToString(int.Parse(lblCurrentT.Text) - 1);
    //    BindGVKeepMin();
    //}
    //protected void btnNextT_Click(object sender, EventArgs e)
    //{
    //    lblCurrentT.Text = Convert.ToString(int.Parse(lblCurrentT.Text) + 1);
    //    BindGVKeepMin();
    //}
    //protected void btnNextType_Click(object sender, EventArgs e)
    //{
    //    lblCurrentType.Text = Convert.ToString(int.Parse(lblCurrentType.Text) + 1);
    //    BindGVType(Convert.ToInt32(ViewState["ConFormulaModID"]));
    //}
    //protected void btnBackType_Click(object sender, EventArgs e)
    //{
    //    lblCurrentType.Text = Convert.ToString(int.Parse(lblCurrentType.Text) - 1);
    //    BindGVType(Convert.ToInt32(ViewState["ConFormulaModID"]));
    //}

    protected void GVType_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        BindGVType(Convert.ToInt32(ViewState["ConFormulaModID"]));
        BindGVDeductMoney();
        BindGVKeepMin();
    }
}
