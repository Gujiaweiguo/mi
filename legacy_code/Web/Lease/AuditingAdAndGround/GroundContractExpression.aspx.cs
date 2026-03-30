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
using Base.Util;

/// <summary>
/// Author:hesijian
/// Date:2009-11-19
/// Content:Created
/// </summary>

public partial class Lease_AuditingAdAndGround_GroundContractExpression : BasePage
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
    public string dateError = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {

            BindChargeType();
            BindFormulaType();

            GetConFormulaInfo();
            InitaExpressionsControls(false);
            BindGVType();

            int contractID = 0;
            if (Request.Cookies["Info"].Values["conID"] != "")
            {
                contractID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
            }


            emptyStr = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");
            dateError = (String)GetGlobalResourceObject("BaseInfo", "PublicMes_DateError");


        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Publicmessage", "Load();", true);
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

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        DataSet tempDs = new DataSet();

        baseBO.WhereClause = "FormulaID = '" + this.Hidden1.Value + "'";
        tempDs = baseBO.QueryDataSet(new ConFormulaH());
        if (tempDs.Tables[0].Rows.Count > 0)
        {
            cmbChargeTypeID.SelectedValue = tempDs.Tables[0].Rows[0]["ChargeTypeID"].ToString();

            SelectChargeType();

            cmbFormulaType.SelectedValue = tempDs.Tables[0].Rows[0]["FormulaType"].ToString();
            txtBeginDate.Text = Convert.ToDateTime(tempDs.Tables[0].Rows[0]["FStartDate"]).ToString("yyyy-MM-dd").ToString();
            txtOverDate.Text = Convert.ToDateTime(tempDs.Tables[0].Rows[0]["FEndDate"]).ToString("yyyy-MM-dd").ToString();

            txtBaseAmt.Text = tempDs.Tables[0].Rows[0]["BaseAmt"].ToString();


            txtFixedRental.Text = tempDs.Tables[0].Rows[0]["FixedRental"].ToString();

            if (tempDs.Tables[0].Rows[0]["RateType"].ToString() == ConFormulaH.RATETYPE_TYPE_MONTH)
            {
                rabMonthHire.Checked = true;
                rabDayHire.Checked = false;
            }
            if (tempDs.Tables[0].Rows[0]["RateType"].ToString() == ConFormulaH.RATETYPE_TYPE_DAY)
            {
                rabMonthHire.Checked = false;
                rabDayHire.Checked = true;

            }

            GVType.Enabled = true;

            ViewState["formulaHID"] = this.Hidden1.Value;
        }
    }

    //绑定费用类别
    protected void BindChargeType()
    {
        BaseBO baseBO = new BaseBO();
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");

        baseBO.WhereClause = "ChargeClass in (" + ChargeType.CHARGECLASS_LEASE + "," + ChargeType.CHARGECLASS_FANST + "," + ChargeType.CHARGECLASS_DEPOSIT + ")";
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
            if (status[i] != ConFormulaH.FORMULATYPE_TYPE_TWO)
            {
                cmbFormulaType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ConFormulaH.GetFormulaTypeStatusDesc(status[i])), status[i].ToString()));

            }
        }
    }
    #region 获取公式列表中的信息
    protected void GetConFormulaInfo()
    {
        int contractID = 0;
        if (Request.Cookies["Info"].Values["conID"] != "")
        {
            contractID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
        }

        ViewState["contractID"] = contractID;
        baseBo.WhereClause = "";
        baseBo.WhereClause = "ContractID = " + contractID;
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

            txtFixedRental.Text = formulaH.FixedRental.ToString();


            GVType.DataSource = rs;
            GVType.DataBind();
            GVType.Enabled = true;
        }
        else
            GVType.Enabled = false;
        //txtArea.Text = ViewState["shopArea"].ToString();
    }
    #endregion

    #region 公式的添加和修改
    protected void ExpressionAddAndModify()
    {
        JudgeExpression();
        BindGVType();
    }

    private void JudgeExpression()
    {
        ConFormulaH formula = new ConFormulaH();
        BaseBO baseBO = new BaseBO();
        Contract contract = new Contract();
        DateTime chargeStartDate = DateTime.Now;//费用开始日期


        DateTime conStartDate = DateTime.Now;//合同开始日期


        DateTime conEndDate = DateTime.Now;//合同终止日期

        /*获取费用开始日期和合同开始日期*/
        baseBO.WhereClause = "ContractID=" + Convert.ToInt32(ViewState["contractID"]);
        rs = baseBO.Query(contract);
        if (rs.Count == 1)
        {
            contract = rs.Dequeue() as Contract;
            chargeStartDate = contract.ChargeStartDate;
            conStartDate = contract.ConStartDate;
            conEndDate = contract.ConEndDate;
        }

        /*判断租金类别租金公式开始日期不能小于费用开始日期*/
        if (Convert.ToInt32(ViewState["ChargeClass"]) == CHARGECLASS_TRUE)
        {
            if (Convert.ToDateTime(chargeStartDate.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(txtBeginDate.Text) && Convert.ToDateTime(conEndDate.ToString("yyyy-MM-dd")) >= Convert.ToDateTime(txtOverDate.Text))
            {
                AddExpression();
            }
            else
            {
                /**/
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Lease_ChargeDateError") + "'", true);
                return;
            }
        }
        else
        {
            if (Convert.ToDateTime(conStartDate.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(txtBeginDate.Text) && Convert.ToDateTime(conEndDate.ToString("yyyy-MM-dd")) >= Convert.ToDateTime(txtOverDate.Text))
            {
                AddExpression();
            }
            else
            {
                /**/
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Lease_ChargeDateError") + "'", true);
                return;
            }
        }

    }
    private void AddExpression()
    {
        try
        {
            ConFormulaH formula = new ConFormulaH();
            BaseBO baseBO = new BaseBO();

            formula.ContractID = Convert.ToInt32(ViewState["contractID"]);
            formula.ChargeTypeID = Convert.ToInt32(cmbChargeTypeID.SelectedValue);
            formula.FormulaType = cmbFormulaType.SelectedValue;
            formula.FStartDate = Convert.ToDateTime(txtBeginDate.Text);
            formula.FEndDate = Convert.ToDateTime(txtOverDate.Text);
            formula.BaseAmt = Convert.ToDecimal(txtBaseAmt.Text);
            if (rabMonthHire.Checked == true)
                formula.RateType = ConFormulaH.RATETYPE_TYPE_MONTH;
            if (rabDayHire.Checked == true)
                formula.RateType = ConFormulaH.RATETYPE_TYPE_DAY;
            //if (rabFastness.Checked == true)
            //    formula.PcentOpt = ConFormulaH.PCENTOPT_TYPE_FAST;
            //if (rabMonopole.Checked == true)
            //    formula.PcentOpt = ConFormulaH.PCENTOPT_TYPE_S;
            //if (rabMultilevel.Checked == true)
            formula.PcentOpt = "F";
            //if (rabFastness2.Checked == true)
            formula.MinSumOpt = "F";
            //if (rabLevel.Checked == true)
            //    formula.MinSumOpt = ConFormulaH.MINSUMOPT_TYPE_T;
            //string ss = Hidden_txtArea.Value;
            formula.TotalArea = Convert.ToDecimal(ViewState["shopArea"]);

            formula.FixedRental = Convert.ToDecimal(txtFixedRental.Text);


            ViewState["FormulaID"] = formula.FormulaID;

            if (Convert.ToInt32(ViewState["ExpressionFlag"]) == 0)
            {
                formula.FormulaID = BaseApp.GetFormulaID();
                if (baseBo.Insert(formula) != -1)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                    return;
                    //提示保存失败
                }
            }
            if (Convert.ToInt32(ViewState["ExpressionFlag"]) == 1)
            {
                baseBo.WhereClause = "";
                baseBo.WhereClause = "FormulaID = " + Convert.ToInt32(ViewState["formulaHID"]);

                if (baseBo.Update(formula) != -1)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                    return;
                }
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加合同再签信息错误:", ex);
        }

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
            GVType.EmptyDataText = "";
            pds.AllowPaging = true;
            pds.PageSize = 8;
            lblTotalNumType.Text = "/" + pds.PageCount.ToString() + " page";
            pds.CurrentPageIndex = int.Parse(lblCurrentType.Text) - 1;
            if (pds.IsFirstPage)
            {
                btnBackType.Enabled = false;
                btnNextType.Enabled = true;
            }

            if (pds.IsLastPage)
            {
                btnBackType.Enabled = true;
                btnNextType.Enabled = false;
            }

            if (pds.IsFirstPage && pds.IsLastPage)
            {
                btnBackType.Enabled = false;
                btnNextType.Enabled = false;
            }

            if (!pds.IsLastPage && !pds.IsFirstPage)
            {
                btnBackType.Enabled = true;
                btnNextType.Enabled = true;
            }

            this.GVType.DataSource = pds;
            this.GVType.DataBind();
            spareRow = GVType.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
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
        txtFixedRental.Enabled = s;
    }
    #endregion

    private void setControlEnable(bool s)
    {
        cmbChargeTypeID.Enabled = s;
        cmbFormulaType.Enabled = s;
        txtBeginDate.Enabled = s;
        txtOverDate.Enabled = s;

        rabMonthHire.Enabled = s;
        rabDayHire.Enabled = s;
        txtFixedRental.Enabled = s;
        txtBaseAmt.Enabled = s;
    }
    private void SetFormulaType(bool s)
    {
        cmbChargeTypeID.Enabled = s;
        cmbFormulaType.Enabled = s;
        txtBeginDate.Enabled = s;
        txtOverDate.Enabled = s;
        txtBaseAmt.Enabled = s;

        rabMonthHire.Enabled = s;
        rabDayHire.Enabled = s;
        txtFixedRental.Enabled = s;
    }

    protected void btnNextType_Click(object sender, EventArgs e)
    {
        lblCurrentType.Text = Convert.ToString(int.Parse(lblCurrentType.Text) + 1);
        BindGVType();
    }
    protected void btnBackType_Click(object sender, EventArgs e)
    {
        lblCurrentType.Text = Convert.ToString(int.Parse(lblCurrentType.Text) - 1);
        BindGVType();
    }

    private void LoadBeginDate()
    {
        ConFormulaH conFormulaH = new ConFormulaH();
        Contract contract = new Contract();
        BaseBO baseBO = new BaseBO();

        DataTable dataTable = baseBO.QueryDataSet("Select max(FEndDate) + 1 From ConFormulaH Where ChargeTypeID = " + cmbChargeTypeID.SelectedValue + " And ContractID = " + Convert.ToInt32(ViewState["contractID"])).Tables[0];

        if (dataTable.Rows[0][0].ToString() != "")
        {

            txtBeginDate.Text = Convert.ToDateTime(dataTable.Rows[0][0]).ToString("yyyy-MM-dd");
        }
        else
        {
            baseBO.WhereClause = "ContractID=" + Convert.ToInt32(ViewState["contractID"]);
            rs = baseBO.Query(contract);
            if (rs.Count > 0)
            {
                contract = rs.Dequeue() as Contract;
                txtBeginDate.Text = contract.ChargeStartDate.ToString("yyyy-MM-dd");
                txtOverDate.Text = contract.ConEndDate.ToString("yyyy-MM-dd");
            }
        }
    }

    private void TextClear()
    {
        txtBaseAmt.Text = "0";
        txtFixedRental.Text = "0";
    }

    private void SelectChargeType()
    {
        BaseBO baseBO = new BaseBO();
        ChargeType chargeType = new ChargeType();
        baseBO.WhereClause = "ChargeTypeID = " + Convert.ToInt32(cmbChargeTypeID.SelectedValue);
        rs = baseBO.Query(chargeType);
        if (rs.Count > 0)
        {
            chargeType = rs.Dequeue() as ChargeType;
            if (chargeType.ChargeClass == ChargeType.CHARGECLASS_DEPOSIT || chargeType.ChargeClass == ChargeType.CHARGECLASS_PREDICT)
            {
                /*如果是押金和预付款费用类别-公式类别是一次性收取*/
                cmbFormulaType.Items.Clear();
                string[] status = ConFormulaH.GetFormulaTypeStatus();
                int s = status.Length;
                for (int i = 0; i < s; i++)
                {
                    if (status[i] == ConFormulaH.FORMULATYPE_TYPE_THREE)
                    {
                        cmbFormulaType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ConFormulaH.GetFormulaTypeStatusDesc(status[i])), status[i].ToString()));

                    }
                }

                SetFormulaType(false);
                cmbFormulaType.Enabled = false;
            }
            else if (chargeType.ChargeClass == ChargeType.CHARGECLASS_LEASE)
            {
                /*其他默认费用类别*/
                cmbFormulaType.Items.Clear();
                string[] status = ConFormulaH.GetFormulaTypeStatus();
                int s = status.Length;
                for (int i = 0; i < s; i++)
                {
                    if (status[i] != ConFormulaH.FORMULATYPE_TYPE_TWO)
                    {
                        cmbFormulaType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ConFormulaH.GetFormulaTypeStatusDesc(status[i])), status[i].ToString()));

                    }
                }
                setControlEnable(false);
            }
            else if (chargeType.ChargeClass == ChargeType.CHARGECLASS_FANST)
            {
                /*如果是每月固定费用费用类别-公式类别是固定和一次性收取*/
                cmbFormulaType.Items.Clear();
                string[] status = ConFormulaH.GetFormulaTypeStatus();
                int s = status.Length;
                for (int i = 0; i < s; i++)
                {
                    if (status[i] != ConFormulaH.FORMULATYPE_TYPE_TWO)
                    {
                        cmbFormulaType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ConFormulaH.GetFormulaTypeStatusDesc(status[i])), status[i].ToString()));

                    }
                }
                setControlEnable(false);

            }
            else if (chargeType.ChargeClass == ChargeType.CHARGECLASS_YEAREND)
            {
                /*如果为年终结算费用类别-公式类别是抽成和保底*/
                cmbFormulaType.Items.Clear();
                string[] status = ConFormulaH.GetFormulaTypeStatus();
                int s = status.Length;
                for (int i = 0; i < s; i++)
                {
                    if (status[i] == ConFormulaH.FORMULATYPE_TYPE_TWO)
                    {
                        cmbFormulaType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ConFormulaH.GetFormulaTypeStatusDesc(status[i])), status[i].ToString()));

                    }
                }
                setControlEnable(false);
                txtBaseAmt.Enabled = false;
                cmbFormulaType.Enabled = false;

                txtFixedRental.Text = "0";
            }
            else if (chargeType.ChargeClass == ChargeType.CHARGECLASS_UNION)
            {
                /*如果联营结算费用类别-公式类别是抽成和保底*/
                cmbFormulaType.Items.Clear();
                string[] status = ConFormulaH.GetFormulaTypeStatus();
                int s = status.Length;
                for (int i = 0; i < s; i++)
                {
                    if (status[i] == ConFormulaH.FORMULATYPE_TYPE_TWO)
                    {
                        cmbFormulaType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ConFormulaH.GetFormulaTypeStatusDesc(status[i])), status[i].ToString()));

                    }
                }
                setControlEnable(false);
                txtBaseAmt.Enabled = false;
                cmbFormulaType.Enabled = false;

                txtFixedRental.Text = "0";
            }
            else
            {
                cmbFormulaType.Items.Clear();
                BindFormulaType();
                cmbFormulaType.Enabled = false;
                setControlEnable(false);
                txtBaseAmt.Enabled = false;
            }
        }
    }
}
