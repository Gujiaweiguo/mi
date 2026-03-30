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
public partial class Lease_ConOvertimeBill_ConOverTimeExpression : System.Web.UI.Page
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
            BindChargeType();
            BindFormulaType();

            GetConFormulaInfo();
            InitaExpressionsControls(false);
            BindGVType();
            BindGVDeductMoney();
            BindGVKeepMin();

            int contractID = 0;
            if (Request.Cookies["Info"].Values["conID"] != "")
            {
                contractID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
            }

            this.txtArea.Text = GetShopArea(contractID).ToString();
            ViewState["shopArea"] = GetShopArea(contractID).ToString();
            txtBaseAmt.Attributes.Add("onkeydown", "textleave()");
            txtUnitHire.Attributes.Add("onkeydown", "textleave()");
            txtFixedRental.Attributes.Add("onkeydown", "textleave()");
            txtFore.Attributes.Add("onkeydown", "textleave()");
            txtForePer.Attributes.Add("onkeydown", "textleave()");
            txtForeKeepMin.Attributes.Add("onkeydown", "textleave()");
            txtForeKeep.Attributes.Add("onkeydown", "textleave()");

            txtFixedRental.Attributes.Add("onFocus", "GetRental()");
            emptyStr = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");
            IBtnSave.Attributes.Add("onclick", "return FormulaValidator(form1)");


        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Publicmessage", "Load();", true);
        }
    }
    protected void cmbFormulaType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbFormulaType.SelectedValue == ConFormulaH.FORMULATYPE_TYPE_ONE)
        {
            setControlEnable(false);
            txtBaseAmt.Enabled = false;

        }
        if (cmbFormulaType.SelectedValue == ConFormulaH.FORMULATYPE_TYPE_TWO)
        {
            setControlEnable(true);
            txtBaseAmt.Enabled = false;
        }
        if (cmbFormulaType.SelectedValue == ConFormulaH.FORMULATYPE_TYPE_THREE)
        {
            SetFormulaType(false);
        }
    }
    protected void IBtnSave_Click(object sender, EventArgs e)
    {
        ExpressionAddAndModify();
        txtBaseAmt.Text = "0";
        IBtnSave.Enabled = false;
        //BindGVType();
        InitaExpressionsControls(false);
    }
    protected void IBtnAdd_Click(object sender, EventArgs e)
    {
        ViewState["ExpressionFlag"] = 0;
        InitaExpressionsControls(true);
        GVType.Enabled = true;
        IBtnAdd.Enabled = true;
        IBtnDel.Enabled = true;
        IBtnModify.Enabled = true;
        IBtnSave.Enabled = true;

        LoadBeginDate();
        rabMonthHire.Checked = true;
        rabDayHire.Checked = false;
        rabFastness.Checked = true;
        rabMonopole.Checked = false;
        rabMultilevel.Checked = false;
        rabFastness2.Checked = true;
        rabLevel.Checked = false;
        setControlEnable(false);
    }
    protected void IBtnModify_Click(object sender, EventArgs e)
    {
        ViewState["ExpressionFlag"] = 1;
        //InitaExpressionsControls(true);

        if (cmbFormulaType.SelectedValue == ConFormulaH.FORMULATYPE_TYPE_ONE)
        {
            setControlEnable(false);
        }
        if (cmbFormulaType.SelectedValue == ConFormulaH.FORMULATYPE_TYPE_TWO)
        {
            setControlEnable(true);
        }
        GVType.Enabled = true;
        IBtnAdd.Enabled = true;
        IBtnDel.Enabled = true;
        IBtnModify.Enabled = true;
        IBtnSave.Enabled = true;
    }
    protected void IBtnDel_Click(object sender, EventArgs e)
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = " + Convert.ToInt32(ViewState["formulaHID"]);
        //rs = baseBo.Query(new ConFormulaP());
        //if (rs.Count > 0)
        //{
        //    //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('该记录存在抽成关系，不能删除！')", true);
        //}
        //rs = baseBo.Query(new ConFormulaM());
        //if (rs.Count > 0)
        //{
        //    //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('该记录存在保底关系，不能删除！')", true);
        //}
        //else 
        if (Convert.ToInt32(ViewState["formulaHID"]) != 0)
        {
            baseBo.WhereClause = "";
            baseBo.WhereClause = "FormulaID = " + Convert.ToInt32(ViewState["formulaHID"]);
            if (baseBo.Delete(new ConFormulaH()) == -1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
                return;
            }
            if (baseBo.Delete(new ConFormulaM()) == -1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
                return;
            }
            if (baseBo.Delete(new ConFormulaP()) == -1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
                return;
            }
        }

        BindGVType();
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
            tempDs = baseBO.QueryDataSet(new ConFormulaP());
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
            tempDs = baseBO.QueryDataSet(new ConFormulaM());
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
            //InitaExpressionsControls(true);

            if (cmbFormulaType.SelectedValue == ConFormulaH.FORMULATYPE_TYPE_ONE)
            {
                setControlEnable(false);
            }
            if (cmbFormulaType.SelectedValue == ConFormulaH.FORMULATYPE_TYPE_TWO)
            {
                setControlEnable(true);
            }
            GVType.Enabled = true;
            IBtnAdd.Enabled = true;
            IBtnDel.Enabled = true;
            IBtnModify.Enabled = true;
            IBtnSave.Enabled = true;

            ViewState["formulaHID"] = this.Hidden1.Value;
        }
        else
        {
            txtFore.Enabled = false;
            txtForePer.Enabled = false;
            txtForeKeepMin.Enabled = false;
            txtForeKeep.Enabled = false;
            BtnDeductAdd.Enabled = false;
            BtnDeductDel.Enabled = false;
            BtnKeepDel.Enabled = false;
            BtnKeepAdd.Enabled = false;
            GVDeductMoney.Enabled = false;
            GVKeepMin.Enabled = false;
        }

        txtFore.Text = "";
        txtForePer.Text = "";
        txtForeKeepMin.Text = "";
        txtForeKeep.Text = "";
    }
    protected void lBtnP_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "ConFormulaPID = " + HiddenDeduct1.Value;
        DataSet tempDs = baseBO.QueryDataSet(new ConFormulaP());
        if (tempDs.Tables[0].Rows.Count > 0)
        {
            txtFore.Text = tempDs.Tables[0].Rows[0]["SalesTo"].ToString();
            txtForePer.Text = tempDs.Tables[0].Rows[0]["Pcent"].ToString();
            BtnDeductDel.Enabled = true;
            ViewState["deduct"] = HiddenDeduct1.Value;
        }
    }
    protected void lBtnM_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "ConFormulaMID = " + HiddenKeepMin1.Value;
        DataSet tempDs = baseBO.QueryDataSet(new ConFormulaM());
        if (tempDs.Tables[0].Rows.Count > 0)
        {
            txtForeKeepMin.Text = tempDs.Tables[0].Rows[0]["SalesTo"].ToString();
            txtForeKeep.Text = tempDs.Tables[0].Rows[0]["MinSum"].ToString();
            BtnKeepDel.Enabled = true;
            ViewState["KeepMin"] = HiddenKeepMin1.Value;
        }
    }
    protected void BtnDeductAdd_Click(object sender, EventArgs e)
    {
        ConFormulaP formulaP = new ConFormulaP();
        ConFormulaH conFormulaH = new ConFormulaH();
        string Pcentopt_Type = "";

        if (Convert.ToInt32(ViewState["formulaHID"]) == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_FormulaSave") + "'", true);
            return;
        }

        formulaP.ConFormulaPID = BaseApp.GetConFormulaPID();
        formulaP.FormulaID = Convert.ToInt32(ViewState["formulaHID"]);
        formulaP.SalesTo = Convert.ToDecimal(txtFore.Text);
        formulaP.Pcent = Convert.ToDecimal(txtForePer.Text) / 100;

        baseTrans.BeginTrans();

        if (baseTrans.Insert(formulaP) == -1)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            baseTrans.Rollback();
            return;
        }

        /*固定*/
        if (rabFastness.Checked == true)
        {
            Pcentopt_Type = ConFormulaH.PCENTOPT_TYPE_FAST;
        }
        /*单级*/
        if (rabMonopole.Checked == true)
        {
            Pcentopt_Type = ConFormulaH.PCENTOPT_TYPE_S;
        }
        /*多级*/
        if (rabMultilevel.Checked == true)
        {
            Pcentopt_Type = ConFormulaH.PCENTOPT_TYPE_M;
        }

        if (baseTrans.ExecuteUpdate("Update ConFormulaH Set PcentOpt= '" + Pcentopt_Type + "' Where FormulaID = " + ViewState["formulaHID"]) == -1)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            baseTrans.Rollback();
            return;
        }
        baseTrans.Commit();
        BindGVDeductMoney();
        txtFore.Text = "";
        txtForePer.Text = "";
    }
    protected void BtnDeductDel_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "";
        baseBO.WhereClause = "ConFormulaPID = " + Convert.ToInt32(ViewState["deduct"]);
        if (baseBO.Delete(new ConFormulaP()) != -1)
        {
            //删除成功！
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            return;
        }
        txtFore.Text = "";
        txtForePer.Text = "";
        BindGVDeductMoney();
    }
    protected void BtnKeepAdd_Click(object sender, EventArgs e)
    {
        ConFormulaM formulaM = new ConFormulaM();
        ConFormulaH conFormulaH = new ConFormulaH();
        string Minsumopt_Type = "";

        if (Convert.ToInt32(ViewState["formulaHID"]) == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_FormulaSave") + "'", true);
            return;
        }

        formulaM.ConFormulaMID = BaseApp.GetConFormulaMID();
        formulaM.FormulaID = Convert.ToInt32(ViewState["formulaHID"]);
        formulaM.SalesTo = Convert.ToDecimal(txtForeKeepMin.Text);
        formulaM.MinSum = Convert.ToDecimal(txtForeKeep.Text);

        baseTrans.BeginTrans();

        if (baseTrans.Insert(formulaM) == -1)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            baseTrans.Rollback();
            return;
        }

        if (rabFastness2.Checked == true)
        {
            Minsumopt_Type = ConFormulaH.MINSUMOPT_TYPE_FAST;
        }
        if (rabLevel.Checked == true)
        {
            Minsumopt_Type = ConFormulaH.MINSUMOPT_TYPE_T;
        }

        if (baseTrans.ExecuteUpdate("Update ConFormulaH Set MinSumOpt= '" + Minsumopt_Type + "' Where FormulaID = " + ViewState["formulaHID"]) == -1)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            baseTrans.Rollback();
            return;
        }
        baseTrans.Commit();

        BindGVKeepMin();
        txtForeKeep.Text = "";
        txtForeKeepMin.Text = "";
    }
    protected void BtnKeepDel_Click(object sender, EventArgs e)
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "ConFormulaMID = " + Convert.ToInt32(ViewState["KeepMin"]);
        if (baseBo.Delete(new ConFormulaM()) != -1)
        {
            //删除成功！
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            return;
        }
        txtForeKeepMin.Text = "";
        txtForeKeep.Text = "";
        BindGVKeepMin();
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
        if (rabFastness.Checked == true)
            formula.PcentOpt = ConFormulaH.PCENTOPT_TYPE_FAST;
        if (rabMonopole.Checked == true)
            formula.PcentOpt = ConFormulaH.PCENTOPT_TYPE_S;
        if (rabMultilevel.Checked == true)
            formula.PcentOpt = ConFormulaH.PCENTOPT_TYPE_M;
        if (rabFastness2.Checked == true)
            formula.MinSumOpt = ConFormulaH.MINSUMOPT_TYPE_FAST;
        if (rabLevel.Checked == true)
            formula.MinSumOpt = ConFormulaH.MINSUMOPT_TYPE_T;
        //string ss = Hidden_txtArea.Value;
        formula.TotalArea = Convert.ToDecimal(ViewState["shopArea"]);
        formula.UnitPrice = Convert.ToDecimal(txtUnitHire.Text);
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
            }
        }
        if (Convert.ToInt32(ViewState["ExpressionFlag"]) == 1)
        {
            baseBo.WhereClause = "";
            baseBo.WhereClause = "FormulaID = " + Convert.ToInt32(ViewState["formulaHID"]);

            if (baseBo.Update(formula) != -1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            }
        }
    }

    #endregion

    #region GridView绑定
    protected void BindGVType()
    {
        ds.Clear();
        baseBo.WhereClause = "";
        baseBo.OrderBy = "FormulaType,FStartDate Desc";
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

    protected void BindGVDeductMoney()
    {
        baseBo.WhereClause = "";
        baseBo.OrderBy = "";
        baseBo.WhereClause = "FormulaID = " + Convert.ToInt32(ViewState["formulaHID"]);
        //DeductMoneyDS = baseBo.QueryDataSet();
        //DeductMoneyDT = DeductMoneyDS.Tables[0];
        //int count = DeductMoneyDT.Rows.Count;

        //int ss = 4 - count;
        //for (int i = 0; i < ss; i++)
        //{
        //    DeductMoneyDT.Rows.Add(DeductMoneyDT.NewRow());
        //}
        //GVDeductMoney.DataSource = DeductMoneyDT;
        //GVDeductMoney.DataBind();

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
            GVDeductMoney.EmptyDataText = "";
            pds.AllowPaging = true;
            pds.PageSize = 4;
            lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
            pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;
            if (pds.IsFirstPage)
            {
                btnBack.Enabled = false;
                btnNext.Enabled = true;
            }

            if (pds.IsLastPage)
            {
                btnBack.Enabled = true;
                btnNext.Enabled = false;
            }

            if (pds.IsFirstPage && pds.IsLastPage)
            {
                btnBack.Enabled = false;
                btnNext.Enabled = false;
            }

            if (!pds.IsLastPage && !pds.IsFirstPage)
            {
                btnBack.Enabled = true;
                btnNext.Enabled = true;
            }

            this.GVDeductMoney.DataSource = pds;
            this.GVDeductMoney.DataBind();
            spareRow = GVDeductMoney.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
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
        //KeepMinDS = baseBo.QueryDataSet(new ConFormulaM());
        //KeepMinDT = KeepMinDS.Tables[0];
        //int count = KeepMinDT.Rows.Count;

        //int ss = 4 - count;
        //for (int i = 0; i < ss; i++)
        //{
        //    KeepMinDT.Rows.Add(KeepMinDT.NewRow());
        //}
        //GVKeepMin.DataSource = KeepMinDT;
        //GVKeepMin.DataBind();
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
            GVKeepMin.EmptyDataText = "";
            pds.AllowPaging = true;
            pds.PageSize = 4;
            lblTotalNumT.Text = "/" + pds.PageCount.ToString() + " page";
            pds.CurrentPageIndex = int.Parse(lblCurrentT.Text) - 1;
            if (pds.IsFirstPage)
            {
                btnBackT.Enabled = false;
                btnNextT.Enabled = true;
            }

            if (pds.IsLastPage)
            {
                btnBackT.Enabled = true;
                btnNextT.Enabled = false;
            }

            if (pds.IsFirstPage && pds.IsLastPage)
            {
                btnBackT.Enabled = false;
                btnNextT.Enabled = false;
            }

            if (!pds.IsLastPage && !pds.IsFirstPage)
            {
                btnBackT.Enabled = true;
                btnNextT.Enabled = true;
            }

            this.GVKeepMin.DataSource = pds;
            this.GVKeepMin.DataBind();
            spareRow = GVKeepMin.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
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
        IBtnAdd.Enabled = !s;
        IBtnDel.Enabled = !s;
        IBtnModify.Enabled = !s;
        //IBtnSave.Enabled = !s;

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
        GVDeductMoney.Enabled = s;
        GVKeepMin.Enabled = s;
        BtnDeductAdd.Enabled = s;
        BtnDeductDel.Enabled = s;
        BtnKeepDel.Enabled = s;
        BtnKeepAdd.Enabled = s;
    }
    #endregion

    private decimal GetShopArea(int conId)
    {
        decimal shopArea = 0m;
        baseBo.WhereClause = "";
        baseBo.WhereClause = "ContractID = " + conId;
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
        GVDeductMoney.Enabled = s;
        txtFore.Enabled = s;
        txtForePer.Enabled = s;
        rabFastness2.Enabled = s;
        rabLevel.Enabled = s;
        GVKeepMin.Enabled = s;
        txtForeKeepMin.Enabled = s;
        txtForeKeep.Enabled = s;
        BtnDeductAdd.Enabled = s;
        BtnDeductDel.Enabled = s;
        BtnKeepAdd.Enabled = s;
        BtnKeepDel.Enabled = s;
        cmbChargeTypeID.Enabled = true;
        cmbFormulaType.Enabled = true;
        txtBeginDate.Enabled = true;
        txtOverDate.Enabled = true;

        rabMonthHire.Enabled = !s;
        rabDayHire.Enabled = !s;
        txtUnitHire.Enabled = !s;
        txtFixedRental.Enabled = !s;
    }
    private void SetFormulaType(bool s)
    {
        rabFastness.Enabled = s;
        rabMonopole.Enabled = s;
        rabMultilevel.Enabled = s;
        GVDeductMoney.Enabled = s;
        txtFore.Enabled = s;
        txtForePer.Enabled = s;
        rabFastness2.Enabled = s;
        rabLevel.Enabled = s;
        GVKeepMin.Enabled = s;
        txtForeKeepMin.Enabled = s;
        txtForeKeep.Enabled = s;
        BtnDeductAdd.Enabled = s;
        BtnDeductDel.Enabled = s;
        BtnKeepAdd.Enabled = s;
        BtnKeepDel.Enabled = s;
        cmbChargeTypeID.Enabled = true;
        cmbFormulaType.Enabled = true;
        txtBeginDate.Enabled = true;
        txtOverDate.Enabled = true;
        txtBaseAmt.Enabled = true;

        rabMonthHire.Enabled = s;
        rabDayHire.Enabled = s;
        txtUnitHire.Enabled = s;
        txtFixedRental.Enabled = s;
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
    protected void btnNext_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        BindGVDeductMoney();
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        BindGVDeductMoney();
    }
    protected void btnBackT_Click(object sender, EventArgs e)
    {
        lblCurrentT.Text = Convert.ToString(int.Parse(lblCurrentT.Text) - 1);
        BindGVKeepMin();
    }
    protected void btnNextT_Click(object sender, EventArgs e)
    {
        lblCurrentT.Text = Convert.ToString(int.Parse(lblCurrentT.Text) + 1);
        BindGVKeepMin();
    }
    protected void cmbChargeTypeID_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectChargeType();
        LoadBeginDate();
    }
    protected void GVType_SelectedIndexChanged(object sender, EventArgs e)
    {

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
                /*如果是其他费用类别和水电费费用类别-公式类别是一次性收取*/
                cmbFormulaType.SelectedValue = ConFormulaH.FORMULATYPE_TYPE_THREE;
                SetFormulaType(false);
                cmbFormulaType.Enabled = false;
            }
            else if (chargeType.ChargeClass == ChargeType.CHARGECLASS_LEASE)
            {
                /*其他默认费用类别*/
                ViewState["ChargeClass"] = CHARGECLASS_TRUE;
                cmbFormulaType.Items.Clear();
                BindFormulaType();
                cmbFormulaType.Enabled = true;
                setControlEnable(false);
                txtBaseAmt.Enabled = false;
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
            else
            {
                cmbFormulaType.Items.Clear();
                BindFormulaType();
                cmbFormulaType.Enabled = true;
                setControlEnable(false);
                txtBaseAmt.Enabled = false;
            }
        }
    }
}
