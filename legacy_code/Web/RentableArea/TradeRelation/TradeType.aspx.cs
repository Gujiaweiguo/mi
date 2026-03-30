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

using Base.DB;
using Base.Biz;
using Base;
using RentableArea;

public partial class RentableArea_TradeRelation_TradeType : System.Web.UI.Page
{
    private static int OPR_ADD = 1;
    private static int OPR_EDIT = 2;
    private static int YESORNOTRADE = 0;
    BaseTrans baseTrans = new BaseTrans();
    TradeRelation tradeRelation = new TradeRelation();
    TradeDef tradeDef = new TradeDef();
    BaseBO baseBO = new BaseBO();

    protected void Page_Load(object sender, EventArgs e)
    {
        showtreenode();
        if (!this.IsPostBack)
        {
            BindTradeStatus();
            BindTradeLevel();
            SetControlStatus();
        }
    }

    //绑定经营类别状态
    private void BindTradeStatus()
    {
        int[] tradeLevel = TradeRelation.GetTradeRelationStatus();
        int s = tradeLevel.Length;
        for (int i = 0; i < s; i++)
        {
            cmbTradeTypeStatus.Items.Add(new ListItem(TradeRelation.GetTradeRelationStatusDesc(tradeLevel[i]), tradeLevel[i].ToString()));
        }
    }

    //绑定经营类别级别
    private void BindTradeLevel()
    {
        int[] tradeLevel = TradeRelation.GetTradeLevelStatus();
        int s = tradeLevel.Length;
        for (int i = 0; i < s; i++)
        {
            cmbTradeLevel.Items.Add(new ListItem(TradeRelation.GetTradeLevelStatusDesc(tradeLevel[i]), tradeLevel[i].ToString()));
        }
    }

    private void SetControlStatus()
    {
        btnAdd.Enabled = true;
        btnEdit.Enabled = false;
        btnCancel.Enabled = false;
        btnSave.Enabled = false;

        this.txtTradeCode.ReadOnly = true;
        this.txtTradeName.ReadOnly = true;
        this.cmbTradeLevel.Enabled = false;
        this.cmbTradeTypeStatus.Enabled = false;

        txtTradeCode.BackColor = System.Drawing.Color.FromName("#F5F5F4");
        txtTradeName.BackColor = System.Drawing.Color.FromName("#F5F5F4");
        cmbTradeLevel.BackColor = System.Drawing.Color.FromName("#F5F5F4");
        cmbTradeTypeStatus.BackColor = System.Drawing.Color.FromName("#F5F5F4");
    }

    private void showtreenode()
    {
        string jsdept = "";
        BaseBO baseBO = new BaseBO();

        Resultset rs = baseBO.Query(new TradeRelation());

        if (rs.Count > 0)
        {

            foreach (TradeRelation tradeRelation in rs)
            {
                jsdept += tradeRelation.TradeID + "|" + tradeRelation.PTradeID + "|" + tradeRelation.TradeName + "|" + "" + "^";
            }
            depttxt.Value = jsdept;
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        btnAdd.Enabled = false;
        btnEdit.Enabled = false;
        btnCancel.Enabled = true;
        btnSave.Enabled = true;

        this.txtTradeCode.ReadOnly = false;
        this.txtTradeName.ReadOnly = false;
        this.cmbTradeLevel.Enabled = true;
        this.cmbTradeTypeStatus.Enabled = true;

        this.txtTradeCode.Text = "";
        this.txtTradeName.Text = "";

        Session["Flag"] = OPR_ADD;
        showtreenode();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        btnAdd.Enabled = false;
        btnEdit.Enabled = false;
        btnCancel.Enabled = true;
        btnSave.Enabled = true;

        this.txtTradeCode.ReadOnly = false;
        this.txtTradeName.ReadOnly = false;
        this.cmbTradeLevel.Enabled = true;
        this.cmbTradeTypeStatus.Enabled = true;

        Session["Flag"] = OPR_EDIT;
        showtreenode();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        SetControlStatus();
        showtreenode();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int oprFlag = Convert.ToInt32(Session["Flag"]);
        if (Convert.ToString(oprFlag) == "")
        {
            return;
        }
    
        GetControlValue();
        if (OPR_ADD == oprFlag)
        {
            tradeRelation.TradeID = BaseApp.GetTradeID();
            baseBO.WhereClause = "";
            baseBO.WhereClause = "TradeCode = '" + txtTradeCode.Text.Trim() + "'";
            Resultset rs = baseBO.Query(new TradeRelation());
            if (rs.Count == 0)
            {
                if (YESORNOTRADE == 0)
                {
                    if (baseBO.Insert(tradeRelation) != -1)
                    {
                        /*提示添加成功信息*/
                    }
                }
                else
                {
                    tradeRelation.PTradeID = Convert.ToInt32(Session["TradeID"]);
                    baseTrans.BeginTrans();
                    try
                    {
                        baseTrans.Insert(tradeRelation);
                        tradeDef.Trade2ID = tradeRelation.TradeID;
                        baseTrans.Insert(tradeDef);
                    }
                    catch (Exception ex)
                    {
                        baseTrans.Rollback();
                    }
                    baseTrans.Commit();
                }
            }
        }
        else if (OPR_EDIT == oprFlag)
        {
            
            if (YESORNOTRADE == 0)
            {
                baseBO.WhereClause = "";
                baseBO.WhereClause = "TradeID=" + Session["TradeID"];
                if (baseBO.Update(tradeRelation) != -1)
                {
                    /*提示添加成功信息*/
                }
            }
            else
            {
                baseTrans.BeginTrans();
                try
                {
                    baseTrans.WhereClause = "";
                    baseTrans.WhereClause = "TradeID=" + Session["TradeID"];
                    baseTrans.Update(tradeRelation);
                    baseTrans.WhereClause = "";
                    baseTrans.WhereClause = "Trade2ID=" + Session["TradeID"];
                    baseTrans.Update(tradeDef);
                }
                catch (Exception ex)
                {
                    baseTrans.Rollback();
                }
                baseTrans.Commit();
            }
        }
        showtreenode();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }

    private void GetControlValue()
    {
        tradeRelation.TradeCode = this.txtTradeCode.Text;
        tradeRelation.TradeName = this.txtTradeName.Text;
        tradeRelation.TradeLevel = Convert.ToInt32(this.cmbTradeLevel.SelectedValue);
        tradeRelation.TradeStatus = Convert.ToInt32(this.cmbTradeTypeStatus.SelectedValue);
        if (this.cmbTradeLevel.SelectedValue == TradeRelation.TRADELEVEL_STATUS_ONE.ToString())
        {
            tradeRelation.PTradeID = 0;
            YESORNOTRADE = 0;
        }
        if (this.cmbTradeLevel.SelectedValue == TradeRelation.TRADELEVEL_STATUS_TWO.ToString())
        {
            //tradeRelation.PTradeID = Convert.ToInt32(Session["TradeID"]);
            YESORNOTRADE = 1;
            //查询一级经营类别信息
            baseBO.WhereClause = "";
            baseBO.WhereClause = "TradeID = " + Convert.ToInt32(Session["TradeID"]);
            Resultset tempRs = baseBO.Query(tradeRelation);
            //添加二级经营类别
            if (tempRs.Count == 1)
            {
                 TradeRelation tempTrade = tempRs.Dequeue() as TradeRelation;
                //一级经营类别信息
                tradeDef.Trade1ID = tempTrade.TradeID;
                tradeDef.Trade1Code = tempTrade.TradeCode;
                tradeDef.Trade1Name = tempTrade.TradeName;
                //二级经营类别信息
                tradeDef.Trade2Code = this.txtTradeCode.Text;
                tradeDef.Trade2Name = this.txtTradeName.Text;
                tradeDef.TradeDefStatus = Convert.ToInt32(this.cmbTradeTypeStatus.SelectedValue);
            }
        }
    }

    protected void treeClick_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        TradeRelation tradeRelation = new TradeRelation();
        Session["TradeID"] = deptid.Value;
        baseBO.WhereClause = "";
        baseBO.WhereClause = "TradeID = " + deptid.Value;
        Resultset rs = baseBO.Query(tradeRelation);
        if (rs.Count == 1)
        {
            tradeRelation = rs.Dequeue() as TradeRelation;
            this.txtTradeCode.Text = tradeRelation.TradeCode;
            this.txtTradeName.Text = tradeRelation.TradeName;
            this.cmbTradeLevel.SelectedValue = tradeRelation.TradeLevel.ToString();
            this.cmbTradeTypeStatus.SelectedValue = tradeRelation.TradeStatus.ToString();
        }
        SetControlStatus();
        btnAdd.Enabled = true;
        btnEdit.Enabled = true;
        btnCancel.Enabled = false;
        btnSave.Enabled = false;
        showtreenode();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
}