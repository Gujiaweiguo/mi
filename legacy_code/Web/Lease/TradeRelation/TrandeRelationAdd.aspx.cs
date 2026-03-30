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

using RentableArea;
using Base.Biz;
using Base.DB;
using Base;
using Base.Page;
using BaseInfo.User;
using System.Drawing;

public partial class Lease_TradeRelation_TrandeRelationAdd : BasePage
{
    public string baseInfo;
    public string selectTradeLevel;
    private static int OPR_ADD = 1;
    private static int OPR_EDIT = 2;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_lblTradeAdd");
            ShowTree();
            selectTradeLevel = (String)GetGlobalResourceObject("BaseInfo", "Prompt_LeaseTradeID");

            int[] status = TradeRelation.GetTradeRelationStatus();
            for (int i = 0; i < status.Length; i++)
            {
                cmbTradePStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", TradeRelation.GetTradeRelationStatusDesc(status[i])), status[i].ToString()));
            }
        }
        btnSave.Attributes.Add("onclick", "return CheckData();");
        btnEdit.Attributes.Add("onclick", "return CheckData();");

    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {

        btnEdit.Enabled = false;
        btnSave.Enabled = true;
        btnAdd.Enabled = false;

        txtTradePCode.ReadOnly = false;
        txtTradePName.ReadOnly = false;
        cmbTradePStatus.Enabled = true;

        ClearText();

        ViewState["Flag"] = OPR_ADD;

        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void treeClick_Click(object sender, EventArgs e)
    {

        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        TradeRelation tradeRelation = new TradeRelation();

        string tradeID = deptid.Value;

        if (tradeID.Length == 3)
        {
            //textlock();
            ViewState["TradeID"] = tradeID;
            baseBO.WhereClause = "TradeID=" + tradeID;
            rs = baseBO.Query(tradeRelation);
            if (rs.Count == 1)
            {
                tradeRelation = rs.Dequeue() as TradeRelation;
                txtTradePCode.Text = tradeRelation.TradeCode;
                txtTradePName.Text = tradeRelation.TradeName;
                txtTradePCode.ReadOnly = true;
                txtTradePName.ReadOnly = true;
                cmbTradePStatus.SelectedValue = tradeRelation.TradeStatus.ToString();
                cmbTradePStatus.Enabled = false;
            }
        }
        else if (tradeID.Length == 6)
        {
            //textlock();
            ViewState["TradeID"] = tradeID;
            baseBO.WhereClause = "TradeID=" + tradeID.Substring(tradeID.Length - 3);
            rs = baseBO.Query(tradeRelation);
            if (rs.Count == 1)
            {
                tradeRelation = rs.Dequeue() as TradeRelation;
                txtTradePCode.Text = tradeRelation.TradeCode;
                txtTradePName.Text = tradeRelation.TradeName;
                txtTradePCode.ReadOnly = true;
                txtTradePName.ReadOnly = true;
                cmbTradePStatus.SelectedValue = tradeRelation.TradeStatus.ToString();
                cmbTradePStatus.Enabled = false;
            }
        }
        
       // 16制进转rgb
        Color myColor = Color.FromArgb(tradeRelation.Rb, tradeRelation.Gb, tradeRelation.Bb);
        int rgb = myColor.ToArgb() & 0xFFFFFF;
        this.txtbgcolor.BackColor = ColorTranslator.FromHtml(rgb.ToString());
        this.hidbgcolor.Value = ColorTranslator.ToHtml(ColorTranslator.FromHtml(rgb.ToString()));
       // this.NEW.BackColor = ColorTranslator.FromHtml(rgb.ToString());
        //this.txtbgcolor.Text = ColorTranslator.ToHtml(ColorTranslator.FromHtml(rgb.ToString()));




        btnEdit.Enabled = true;
        btnAdd.Enabled = true;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        Session["editLog"] = txtTradePCode.Text;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
       
            btnEdit.Enabled = false;
            btnSave.Enabled = true;
            btnAdd.Enabled = false;

            txtTradePCode.ReadOnly = false;
            txtTradePName.ReadOnly = false;
            cmbTradePStatus.Enabled = true;

            ViewState["Flag"] = OPR_EDIT;
        
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtTradePCode.Text != "" && txtTradePName.Text != "")
        {
            TradeRelation tradeRelation = new TradeRelation();
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet("select TradeCode from TradeRelation where TradeCode='" + txtTradePCode.Text.Trim() + "'");

                if (Convert.ToInt32(ViewState["Flag"]) == OPR_ADD)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "RentableArea_lblTradeCode") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtTradePCode.select()", true);
                    }
                    else
                    {
                        if (ViewState["TradeID"].ToString().Length == 6)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_NotNode") + "'", true);
                        }
                        else if (ViewState["TradeID"].ToString().Length == 1)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_NotNode") + "'", true);
                        }
                        else if (ViewState["TradeID"].ToString().Length == 3)
                        {
                            if (Convert.ToInt32(ViewState["TradeID"]) == 100)
                            {
                                SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
                                tradeRelation.CreateUserId = sessionUser.UserID;
                                tradeRelation.TradeID = BaseApp.GetTradeID();
                                tradeRelation.TradeCode = txtTradePCode.Text.Trim();
                                tradeRelation.TradeName = txtTradePName.Text.Trim();
                                tradeRelation.PTradeID = 100;
                                tradeRelation.TradeLevel = TradeRelation.TRADELEVEL_STATUS_ONE;
                                tradeRelation.TradeStatus = Convert.ToInt32(cmbTradePStatus.SelectedValue);
                                if (hidbgcolor.Value != "")
                                {
                                    int r = Convert.ToInt32("0x" + hidbgcolor.Value.Substring(1, 2), 16);
                                    int g = Convert.ToInt32("0x" + hidbgcolor.Value.Substring(3, 2), 16);
                                    int b = Convert.ToInt32("0x" + hidbgcolor.Value.Substring(5, 2), 16);
                                    tradeRelation.Rb = Convert.ToInt32(r);
                                    tradeRelation.Gb = Convert.ToInt32(g);
                                    tradeRelation.Bb = Convert.ToInt32(b);
                                }


                            }
                            else
                            {

                                SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
                                tradeRelation.CreateUserId = sessionUser.UserID;
                                tradeRelation.TradeID = BaseApp.GetTradeID();
                                tradeRelation.TradeCode = txtTradePCode.Text.Trim();
                                tradeRelation.TradeName = txtTradePName.Text.Trim();
                                tradeRelation.PTradeID = Convert.ToInt32(ViewState["TradeID"]);
                                tradeRelation.TradeLevel = TradeRelation.TRADELEVEL_STATUS_TWO;
                                tradeRelation.TradeStatus = Convert.ToInt32(cmbTradePStatus.SelectedValue);
                                if (hidbgcolor.Value != "")
                                {
                                    int r = Convert.ToInt32("0x" + hidbgcolor.Value.Substring(1, 2), 16);
                                    int g = Convert.ToInt32("0x" + hidbgcolor.Value.Substring(3, 2), 16);
                                    int b = Convert.ToInt32("0x" + hidbgcolor.Value.Substring(5, 2), 16);
                                    tradeRelation.Rb = Convert.ToInt32(r);
                                    tradeRelation.Gb = Convert.ToInt32(g);
                                    tradeRelation.Bb = Convert.ToInt32(b);
                                }

                            }

                            if (baseBO.Insert(tradeRelation) == -1)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                            }
                        }
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                        //selectdeptid.Value = "";
                        ShowTree();
                        ClearText();

                        this.btnSave.Enabled = false;
                    }
                }
                else if (Convert.ToInt32(ViewState["Flag"]) == OPR_EDIT)
                {
                    if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0][0].ToString() == Session["editLog"].ToString())
                    {
                        if (ViewState["TradeID"].ToString().Length == 1)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_NotNode") + "'", true);
                        }

                        if (ViewState["TradeID"].ToString().Length == 6)
                        {
                            ViewState["TradeID"] = ViewState["TradeID"].ToString().Substring(3, 3);
                        }

                        if (ViewState["TradeID"].ToString().Length == 3)
                        {
                            if (Convert.ToInt32(ViewState["TradeID"]) == 100)
                            {

                                SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
                                tradeRelation.ModifyUserId = sessionUser.UserID;
                                tradeRelation.TradeCode = txtTradePCode.Text.Trim();
                                tradeRelation.TradeName = txtTradePName.Text.Trim();
                                tradeRelation.TradeStatus = Convert.ToInt32(cmbTradePStatus.SelectedValue);
                                if (hidbgcolor.Value != "")
                                {
                                    int r = Convert.ToInt32("0x" + hidbgcolor.Value.Substring(1, 2), 16);
                                    int g = Convert.ToInt32("0x" + hidbgcolor.Value.Substring(3, 2), 16);
                                    int b = Convert.ToInt32("0x" + hidbgcolor.Value.Substring(5, 2), 16);
                                    tradeRelation.Rb = Convert.ToInt32(r);
                                    tradeRelation.Gb = Convert.ToInt32(g);
                                    tradeRelation.Bb = Convert.ToInt32(b);
                                }

                            }
                            else
                            {

                                SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
                                tradeRelation.ModifyUserId = sessionUser.UserID;
                                tradeRelation.TradeCode = txtTradePCode.Text.Trim();
                                tradeRelation.TradeName = txtTradePName.Text.Trim();
                                tradeRelation.TradeStatus = Convert.ToInt32(cmbTradePStatus.SelectedValue);
                                if (hidbgcolor.Value != "")
                                {
                                    int r = Convert.ToInt32("0x" + hidbgcolor.Value.Substring(1, 2), 16);
                                    int g = Convert.ToInt32("0x" + hidbgcolor.Value.Substring(3, 2), 16);
                                    int b = Convert.ToInt32("0x" + hidbgcolor.Value.Substring(5, 2), 16);
                                    tradeRelation.Rb = Convert.ToInt32(r);
                                    tradeRelation.Gb = Convert.ToInt32(g);
                                    tradeRelation.Bb = Convert.ToInt32(b);
                                }

                            }

                            baseBO.WhereClause = "TradeID = " + Convert.ToInt32(ViewState["TradeID"]);
                            if (baseBO.Update(tradeRelation) == -1)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "'", true);
                            }
                        }
                        //selectdeptid.Value = "";
                        ShowTree();
                        ClearText();

                        this.btnSave.Enabled = false;
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "RentableArea_lblTradeCode") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtTradePCode.select()", true);
                    }
                }
                
            
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "select", "select()", true);
        }
            
        
    }

    private void ShowTree()
    {
        BaseBO baseBO = new BaseBO();
        string jsdept = "";

        baseBO.WhereClause = "TradeStatus = " + TradeRelation.TRADERELATION_STATUS_VALID;
        baseBO.OrderBy = "TradeLevel,PTradeID";

        Resultset rs = baseBO.Query(new TradeRelation());

        jsdept = "100" + "|" + "0" + "|" + (String)GetGlobalResourceObject("BaseInfo", "LeaseholdContract_labTradeID") + "^";
        if (rs.Count > 0)
        {
            foreach (TradeRelation tradeRelation in rs)
            {
                if (tradeRelation.TradeLevel == 1)
                {
                    jsdept += tradeRelation.TradeID + "|" + "100" + "|" + tradeRelation.TradeName + "^";
                }
                else
                {
                    jsdept += tradeRelation.PTradeID.ToString() + tradeRelation.TradeID.ToString() + "|" + tradeRelation.PTradeID + "|" + tradeRelation.TradeName + "^";
                }
            }
        }
        depttxt.Value = jsdept;
    }

    private void ClearText()
    {
        txtTradePCode.Text = "";
        txtTradePName.Text = "";
        cmbTradePStatus.SelectedIndex = 0;
        txtbgcolor.BackColor = Color.White;
    }
    //protected void IBtnDel_Click(object sender, EventArgs e)
    //{
    //    TradeRelation tradeRelation = new TradeRelation();
    //    BaseBO baseBO = new BaseBO();
    //    if (ViewState["TradeID"].ToString().Length == 1)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_NotNode") + "'", true);
    //    }

    //    if (ViewState["TradeID"].ToString().Length == 6)
    //    {
    //        ViewState["TradeID"] = ViewState["TradeID"].ToString().Substring(3, 3);
    //    }
        
    //    if (ViewState["TradeID"].ToString().Length == 3)
    //    {

    //        baseBO.WhereClause = "TradeID = " + Convert.ToInt32(ViewState["TradeID"]);
    //        if (baseBO.Delete(tradeRelation) == -1)
    //        {
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
    //        }
    //        ShowTree();
    //    }
    //        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
    //    selectdeptid.Value = "";
    //    ClearText();
    //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    //}
    protected void btnCel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/Lease/TradeRelation/TrandeRelationAdd.aspx");
    }
    //protected void txtbgcolor_TextChanged(object sender, EventArgs e)
    //{
    //    if (txtbgcolor.Text != "" && txtbgcolor.Text != null)
    //    {
    //        this.txtbgcolor.BackColor = System.Drawing.ColorTranslator.FromHtml(txtbgcolor.Text);
    //    }
    //}



}
