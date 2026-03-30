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
public partial class Lease_AuditingLease_AuditingExpressionPalaver : BasePage
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
            BindChargeType();
            BindFormulaType();

            GetConFormulaInfo();
            BindGVType();
            BindGVDeductMoney();
            BindGVKeepMin();
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
            int ss1 = 5 - count1;
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
            int ss2 = 5 - count2;
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
    }
    protected void btnDeduct_Click(object sender, EventArgs e)
    {

    }
    protected void btnKeepMin_Click(object sender, EventArgs e)
    {

    }
    protected void GVType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //鼠标移动到每项时颜色交替效果
            e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor='White';this.style.color='#003399'");
            e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor='#FFFFCD';this.style.color='#8C4510'");
            //单击事件
            e.Row.Attributes.Add("OnClick", "ClickEvent('" + e.Row.Cells[0].Text + "')");
        }
    }
    protected void GVType_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void GVDeductMoney_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void GVKeepMin_RowDataBound(object sender, GridViewRowEventArgs e)
    {

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
        baseBo.WhereClause = "ContractID = " + Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
        baseBo.OrderBy = "ChargeTypeID,FStartDate";
        rs = baseBo.Query(new ConFormulaH());
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
            GVType.DataSource = rs;
            GVType.DataBind();
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
        baseBo.OrderBy = "FEndDate Desc";
        baseBo.WhereClause = "ContractID = '" + ViewState["contractID"] + "'";
        ds = baseBo.QueryDataSet(new ConFormulaH());
        int dsCount = ds.Tables[0].Rows.Count;

        dt = ds.Tables[0];
        int count = dt.Rows.Count;

        //获取费用类别名字
        for (int j = 0; j < count; j++)
        {
            baseBo.WhereClause = "";
            baseBo.OrderBy = "";
            baseBo.WhereClause = "ChargeTypeID = " + dt.Rows[j]["ChargeTypeID"];
            DataSet tempDs = new DataSet();
            tempDs = baseBo.QueryDataSet(new ChargeType());
            dt.Rows[j]["ChargeTypeName"] = (String)GetGlobalResourceObject("Parameter", tempDs.Tables[0].Rows[0]["ChargeTypeName"].ToString());
        }

        //获取公式类别名称
        for (int j = 0; j < count; j++)
        {
            string formulaTypeName = (String)GetGlobalResourceObject("Parameter", ConFormulaH.GetFormulaTypeStatusDesc(dt.Rows[j]["FormulaType"].ToString()));
            dt.Rows[j]["FormulaTypeName"] = formulaTypeName;
        }

        //补空行



        int ss = 10 - count;
        for (int i = 0; i < ss; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        GVType.DataSource = dt;
        GVType.DataBind();

        for (int l = 0; l < count; l++)
        {
            string gIntro;
            if (GVType.PageIndex == 0)
            {
                gIntro = GVType.Rows[l].Cells[1].Text.ToString();
                GVType.Rows[l].Cells[1].Text = SubStr(gIntro, 2);
                gIntro = GVType.Rows[l].Cells[4].Text.ToString();
                GVType.Rows[l].Cells[4].Text = SubStr(gIntro, 4);
            }
        }


    }

    public string SubStr(string sString, int nLeng)
    {
        if (sString.Length <= nLeng)
        {
            return sString;
        }
        string sNewStr = sString.Substring(0, nLeng);
        sNewStr = sNewStr;
        return sNewStr;
    }

    protected void BindGVDeductMoney()
    {
        baseBo.WhereClause = "";
        baseBo.OrderBy = "";
        baseBo.WhereClause = "FormulaID = " + Convert.ToInt32(ViewState["formulaHID"]);
        DeductMoneyDS = baseBo.QueryDataSet(new ConFormulaP());
        DeductMoneyDT = DeductMoneyDS.Tables[0];
        int count = DeductMoneyDT.Rows.Count;

        int ss = 5 - count;
        for (int i = 0; i < ss; i++)
        {
            DeductMoneyDT.Rows.Add(DeductMoneyDT.NewRow());
        }
        GVDeductMoney.DataSource = DeductMoneyDT;
        GVDeductMoney.DataBind();

    }

    protected void BindGVKeepMin()
    {
        baseBo.WhereClause = "";
        baseBo.OrderBy = "";
        baseBo.WhereClause = "FormulaID = " + Convert.ToInt32(ViewState["formulaHID"]);
        KeepMinDS = baseBo.QueryDataSet(new ConFormulaM());
        KeepMinDT = KeepMinDS.Tables[0];
        int count = KeepMinDT.Rows.Count;

        int ss = 5 - count;
        for (int i = 0; i < ss; i++)
        {
            KeepMinDT.Rows.Add(KeepMinDT.NewRow());
        }
        GVKeepMin.DataSource = KeepMinDT;
        GVKeepMin.DataBind();
    }
    #endregion
}
