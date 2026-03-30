using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.Biz;
using Base.Page;
using BaseInfo.User;

public partial class Sell_MediaInfo : BasePage 
{
    #region 定义
    public string baseInfo;
    public static int strUserCode;
    public static string strUserName;
    public static bool flag = false;
    #endregion
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_MediaInfo");
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        strUserCode = objSessionUser.UserID;
        strUserName = objSessionUser.UserName;
        showtree();
        this.btnSave.Attributes.Add("onclick", "return CheckIsNull()");
        this.bt1Save.Attributes.Add("onclick", "return CheckISNum()");

    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!ExistColumn("MediaNo", "Media", txtMediaNo.Text.Trim()))
        {
            if (!ExistColumn("MediaDesc", "Media", txtMediaDesc.Text.Trim()))
            {
                int strAuto = 0;
                try
                {
                    if (txtMediaNo.Text != "")
                    {
                        BaseBO bo = new BaseBO();
                        if (ChkAuto.Checked)
                        {
                            strAuto = 1;
                        }
                        else
                        {
                            strAuto = 0;
                        }
                        string sql = "INSERT INTO Media (MediaNo,MediaDesc,Overpaid,PayType,deletetime,EntryAt,EntryBy) VALUES " +
                                        "('" + txtMediaNo.Text + "','" + txtMediaDesc.Text + "','" + strAuto + "','" + txtMediaNo.Text + "',null,'" + DateTime.Now + "','" + strUserCode + "') ";
                        bo.ExecuteUpdate(sql);
                        //lblErr1.Text = (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success");
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "treearray()", true);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                        this.txtMediaNo.Text = "";
                        this.txtMediaDesc.Text = "";
                    }
                    else
                    {
                        //lblErr1.Text = (String)GetGlobalResourceObject("BaseInfo", "BankCard_NotSelect");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + Label4.Text + (String)GetGlobalResourceObject("BaseInfo", "BankCard_NotSelect") + "'", true);
                    }
                }
                catch (Exception ex)
                {
                    //lblErr1.Text = (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }
            }
            else
            {
                txtMediaDesc.Text = "";
                lblErr1.Text = Label20.Text + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + lblErr1.Text + "'", true);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "treearray()", true);
            }
        }
        else
        {
            string str = Label4.Text + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist");
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "s", "parent.document.all.txtWroMessage.value =  '" + str + "'", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "treearray()", true);
            txtMediaNo.Text = "";
            //lblErr1.Text = Label4.Text + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist");
            
        }
        showtree();
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "Load()", true);
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        int strAuto = 0;
        if (txtMediaNo.Text != "")
        {
            try
            {
                BaseBO bo = new BaseBO();
                if (ChkAuto.Checked)
                {
                    strAuto = 1;
                }
                else
                {
                    strAuto = 0;
                }
                string sql = "Update Media Set MediaDesc='" + txtMediaDesc.Text + "',Overpaid='" + strAuto + "',EntryAt='" + DateTime.Now + "',EntryBy='" + strUserCode + "',Deletetime=null " +
                                " WHERE MediaNo='" + txtMediaNo.Text + "'";

                bo.ExecuteUpdate(sql);
                //lblErr1.Text = (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success");
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            }
            catch (Exception ex)
            {
                //lblErr1.Text = (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
        }
        else
        {
            //lblErr1.Text = (String)GetGlobalResourceObject("BaseInfo", "BankCard_NotSelect");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + Label4.Text + (String)GetGlobalResourceObject("BaseInfo", "BankCard_NotSelect") + "'", true);
        }
        showtree();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "treearray()", true);

    }
    protected void BtnDel_Click(object sender, EventArgs e)
    {
        if (txtMediaNo.Text != "")
        {
            try
            {
                
                BaseBO bo = new BaseBO();
                string sql = "Update Media Set Deletetime='" + DateTime.Now + "',EntryAt='" + DateTime.Now + "'" +
                                " WHERE MediaNo='" + txtMediaNo.Text + "'";

                bo.ExecuteUpdate(sql);
                //lblErr1.Text = (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success");
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            }
            catch (Exception ex)
            {
                //lblErr1.Text = (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
        }
        else
        {
            //lblErr1.Text = (String)GetGlobalResourceObject("BaseInfo", "BankCard_NotSelect");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + Label4.Text + (String)GetGlobalResourceObject("BaseInfo", "BankCard_NotSelect") + "'", true);
        }
        showtree();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "treearray()", true);
    }
    protected void btnQuit_Click(object sender, EventArgs e)
    {
        txtMediaNo.Text = "";
        txtMediaDesc.Text = "";
        lblErr1.Text = "";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  ''", true);
    }



    private void BindDropList()
    {
        BaseBO baseBO = new BaseBO();
        string sql = "  SELECT MediaNo,MediaDesc FROM Media WHERE Deletetime is null  order by MediaNo";
        DataSet myDS = baseBO.QueryDataSet(sql);
        int count = myDS.Tables[0].Rows.Count;
        ddlMedia.Items.Clear();
        ddlMediaC.Items.Clear();
        ddlMediaR.Items.Clear();

        ddlMedia.Items.Add(new ListItem("", ""));
        ddlMediaC.Items.Add(new ListItem("", ""));
        ddlMediaR.Items.Add(new ListItem("", ""));

        for (int i = 0; i < count; i++)
        {
            //绑定Media

            ddlMedia.Items.Add(new ListItem(myDS.Tables[0].Rows[i]["MediaNo"].ToString() + " " + myDS.Tables[0].Rows[i]["MediaDesc"].ToString(), myDS.Tables[0].Rows[i]["MediaNo"].ToString()));
            ddlMediaC.Items.Add(new ListItem(myDS.Tables[0].Rows[i]["MediaNo"].ToString() + " " + myDS.Tables[0].Rows[i]["MediaDesc"].ToString(), myDS.Tables[0].Rows[i]["MediaNo"].ToString()));

            ddlMediaR.Items.Add(new ListItem(myDS.Tables[0].Rows[i]["MediaNo"].ToString() + " " + myDS.Tables[0].Rows[i]["MediaDesc"].ToString(), myDS.Tables[0].Rows[i]["MediaNo"].ToString()));
                   
        }

        BaseBO baseB1 = new BaseBO();

        string sql1 = "  SELECT MediaMNo,MediaMDesc FROM MediaM WHERE Deletetime is null  order by MediaMNo";
        DataSet myDS1 = baseB1.QueryDataSet(sql1);
        int count1 = myDS1.Tables[0].Rows.Count;
        ddlMediaMC.Items.Clear();
        ddlMediaMR.Items.Clear();
        ddlMediaMC.Items.Add(new ListItem("",""));
        ddlMediaMR.Items.Add(new ListItem("", ""));
        for (int i = 0; i < count1; i++)
        {
            //绑定MediaM
            ddlMediaMC.Items.Add(new ListItem(myDS1.Tables[0].Rows[i]["MediaMNo"].ToString() + " " + myDS1.Tables[0].Rows[i]["MediaMDesc"].ToString(), myDS1.Tables[0].Rows[i]["MediaMNo"].ToString()));
            ddlMediaMR.Items.Add(new ListItem(myDS1.Tables[0].Rows[i]["MediaMNo"].ToString() + " " + myDS1.Tables[0].Rows[i]["MediaMDesc"].ToString(), myDS1.Tables[0].Rows[i]["MediaMNo"].ToString())); 
        }
    }
    private bool ExistColumn(string Column, string tableName, string ColumnValue)
    {
        BaseBO baseBO = new BaseBO();
        DataTable tempTb = new DataTable();
        string sql = "Select " + Column + " FROM " + tableName + " WHERE " + Column + " ='" + ColumnValue + "'";
        tempTb = baseBO.QueryDataSet(sql).Tables[0];

        if (tempTb.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    protected void btnAddM_Click(object sender, EventArgs e)
    {
        if (!ExistColumn("MediaMNo", "MediaM", txtMediaMNo.Text.Trim()))
        {
            int strAuto = 0;
            string strCard="";
            try
            {
                if (txtMediaMNo.Text != "")
                {
                    BaseBO bo = new BaseBO();
                    if (ChkAutoM.Checked)
                    {
                        strAuto = 1;
                    }
                    else
                    {
                        strAuto = 0;
                    }
                    if (rdoD.Checked)
                    {
                        strCard = "D";
                    }

                    if (rdoF.Checked)
                    {
                        strCard = "F";
                    }

                    if (txtComTenant.Text=="")
                    {
                        txtComTenant.Text = "0";
                    }

                    if (txtComMall.Text == "")
                    {
                        txtComMall.Text = "0";
                    }
                    string sql = "INSERT INTO MediaM (MediaMNo,MediaMDesc,MediaNo,CMediaNo,CMediaMNo,RMediaNo,RMediaMNo,ChangeAuto,CardType,ComTenant,ComMall,deletetime,EntryAt,EntryBy,MediaMEDesc) VALUES " +
                                    "('" + txtMediaMNo.Text + "'," +
                                    "'" + txtMediaMDesc.Text + "'," +
                                    "'" + ddlMedia.SelectedValue + "'," +
                                    "'" + ddlMediaC.SelectedValue + "'," +
                                    "'" + ddlMediaMC.SelectedValue + "'," +
                                    "'" + ddlMediaR.SelectedValue + "'," +
                                    "'" + ddlMediaMR.SelectedValue + "'," +
                                    "'" + strAuto + "'," +
                                    "'" + strCard + "'," +
                                    "'" + txtComTenant.Text.Trim() + "'," +
                                    "'" + txtComMall.Text.Trim() + "'," +
                                    " null," +
                                    "'" + DateTime.Now + "'," +
                                    "'" + strUserCode + "'," +
                                    "'') ";
                    bo.ExecuteUpdate(sql);
                    //lblErr2.Text = (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success");
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                }
                else
                {
                    //lblErr2.Text = (String)GetGlobalResourceObject("BaseInfo", "BankCard_NotSelect");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + Label16.Text + (String)GetGlobalResourceObject("BaseInfo", "BankCard_NotSelect") + "'", true);
                }
            }
            catch (Exception ex)
            {
                //lblErr2.Text = (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
        }
        else
        {
            txtMediaMNo.Text = "";
            //lblErr2.Text = Label4.Text + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist"); 
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + Label4.Text + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
        }
        showtree();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "treearray()", true);
    }
    protected void btnEditM_Click(object sender, EventArgs e)
    {
        int strAuto = 0;
        string strCard = "";
        try
        {
            if (txtMediaMNo.Text != "")
            {
                BaseBO bo = new BaseBO();
                if (ChkAutoM.Checked)
                {
                    strAuto = 1;
                }
                else
                {
                    strAuto = 0;
                }
                if (rdoD.Checked)
                {
                    strCard = "D";
                }

                if (rdoF.Checked)
                {
                    strCard = "F";
                }
                string sql = "Update MediaM " +
                                "Set MediaMDesc='" + txtMediaMDesc.Text + "'," +
                                " MediaNo='" + ddlMedia.SelectedValue + "'," +
                                " CMediaNo='" + ddlMediaC.SelectedValue + "'," +
                                " CMediaMNo='" + ddlMediaMC.SelectedValue + "'," +
                                " RMediaNo='" + ddlMediaR.SelectedValue + "'," +
                                " RMediaMNo='" + ddlMediaMR.SelectedValue + "'," +
                                " ChangeAuto='" + strAuto + "'," +
                                " CardType='" + strCard + "'," +
                                " ComTenant='" + txtComTenant.Text.Trim() + "'," +
                                " ComMall='" + txtComMall.Text.Trim() + "'," +
                                " Deletetime=null," +
                                " EntryAt='" + DateTime.Now + "'," +
                                " EntryBy='" + strUserCode + "'" +
                                " WHERE MediaMNo='" + txtMediaMNo.Text + "'";

                bo.ExecuteUpdate(sql);
                //lblErr2.Text = (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success");
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            }
            else
            {
                //lblErr2.Text = (String)GetGlobalResourceObject("BaseInfo", "BankCard_NotSelect");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + Label16.Text + (String)GetGlobalResourceObject("BaseInfo", "BankCard_NotSelect") + "'", true);
            }
        }
        catch (Exception ex)
        {
            //lblErr2.Text = (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
        }
        showtree();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "treearray()", true);
    }
    protected void BtnDelM_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtMediaMNo.Text != "")
           {
            BaseBO bo = new BaseBO();
            string sql = "Update MediaM Set Deletetime='" + DateTime.Now + "',EntryAt='" + DateTime.Now + "'" +
                            " WHERE MediaMNo='" + txtMediaMNo.Text + "'";

            bo.ExecuteUpdate(sql);
            //lblErr2.Text = (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success");
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
           }        
            else
            {
                //lblErr2.Text = (String)GetGlobalResourceObject("BaseInfo", "BankCard_NotSelect");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + Label16.Text + (String)GetGlobalResourceObject("BaseInfo", "BankCard_NotSelect") + "'", true);
            }
        }
        catch (Exception ex)
        {
            //lblErr2.Text = (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
        }
        showtree();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "treearray()", true);
    }
    protected void btnQuitM_Click(object sender, EventArgs e)
    {
        txtNULL();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  ''", true);
    }
   
    private void txtNULL()
    {
        txtMediaMNo.Text = "";
        txtMediaMDesc.Text = "";
        txtComMall.Text = "";
        txtComTenant.Text = "";
        ddlMediaMC.Text = "";
        ddlMediaMR.Text = "";
        ddlMediaC.Text = "";
        ddlMediaR.Text = "";
        ddlMedia.Text = "";
        lblErr2.Text = "";
    }
    
    private void showtree()
    {
        string jsdept = "";
        BaseBO baseBo = new BaseBO();
        baseBo.OrderBy = "MediaNo";
        string sql = "select MediaNo,'9999' as PMediaNo,MediaDesc from Media  Union all select MediaMNo,MediaNo as PMediaNo,MediaMDesc from MediaM  order by MediaNo";
        DataTable rs = baseBo.QueryDataSet(sql).Tables[0];

        if (rs.Rows.Count > 0)
        {
            jsdept = "100" + "|" + "0" + "|" + (String)GetGlobalResourceObject("BaseInfo", "Rpt_MediaMDesc") + "^";
            for (int i = 0; i <= rs.Rows.Count - 1; i++)
            {
                if (Convert.ToString(rs.Rows[i]["PMediaNo"]) == "9999")
                {
                    jsdept += rs.Rows[i]["MediaNo"] + "P" + "|" + "100" + "|" + rs.Rows[i]["MediaNo"]+"-"+rs.Rows[i]["MediaDesc"] + "|" + "" + "^";
                }
                else
                {
                    jsdept += rs.Rows[i]["MediaNo"] + "L" + "|" + rs.Rows[i]["PMediaNo"] + "P" + "|" + rs.Rows[i]["MediaNo"] + "-" + rs.Rows[i]["MediaDesc"] + "|" + "^";
                }
            }
            depttxt.Value = jsdept;
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
            BindDropList();
            lblErr1.Text = "";
            lblErr2.Text = "";
            BaseBO bo = new BaseBO();
            DataTable Tb = new DataTable();
            if (deptid.Value.IndexOf("P")>=0)
            {
                Panel2.Visible = true;
                Panel1.Visible = false;
                string sql = "Select MediaNo,MediaDesc,Overpaid,Deletetime from Media where MediaNo='" + deptid.Value.Substring(0, deptid.Value.IndexOf("P")) + "'";
                Tb = bo.QueryDataSet(sql).Tables[0];
                if (Tb.Rows.Count > 0)
                {
                    txtMediaNo.Text = Tb.Rows[0]["MediaNo"].ToString();
                    txtMediaDesc.Text = Tb.Rows[0]["MediaDesc"].ToString();
                    if (Convert.ToString(Tb.Rows[0]["Overpaid"]).Equals("1"))
                    {
                        ChkAuto.Checked = true;
                    }
                    else
                    {
                        ChkAuto.Checked = false;
                    }
                    if (!Convert.IsDBNull(Tb.Rows[0]["Deletetime"]))
                    {
                        //lblErr1.Text = (String)GetGlobalResourceObject("BaseInfo", "Associator_rabInvalid");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + Label4.Text + (String)GetGlobalResourceObject("BaseInfo", "Associator_rabInvalid") + "'", true);
                    }
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
                }
                else
                {
                    //lblErr1.Text = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidNoData");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidNoData") + "'", true);
                }
            }
            else if (deptid.Value.IndexOf("L")>=0)
            {
                Panel1.Visible = true;
                Panel2.Visible = false;

                string sql = "Select MediaMNo,MediaMDesc,MediaNo,CMediaNo,CMediaMNo,RMediaNo,RMediaMNo,ChangeAuto,CardType,ComTenant,ComMall,DeleteTime,EntryAt,EntryBy,MediaMEDesc from MediaM where MediaMNo='" + deptid.Value.Substring(0, deptid.Value.IndexOf("L")) + "'";
                Tb = bo.QueryDataSet(sql).Tables[0];
                if (Tb.Rows.Count > 0)
                {
                    if (!Convert.ToString(Tb.Rows[0]["CMediaMNo"]).Equals("0"))
                    {
                        ddlMediaMC.Text = Tb.Rows[0]["CMediaMNo"].ToString();
                    }
                    else
                    {
                        ddlMediaMC.Text = "";
                    }
                    if (!Convert.ToString(Tb.Rows[0]["RMediaMNo"]).Equals("0"))
                    {
                        ddlMediaMR.Text = Tb.Rows[0]["RMediaMNo"].ToString();
                    }
                    else
                    {
                        ddlMediaMR.Text = "";
                    }

                    if (!Convert.ToString(Tb.Rows[0]["CMediaNo"]).Equals("0"))
                    {
                        ddlMediaC.Text = Tb.Rows[0]["CMediaNo"].ToString();
                    }
                    else
                    {
                        ddlMediaC.Text = "";
                    }
                    if (!Convert.ToString(Tb.Rows[0]["RMediaNo"]).Equals("0"))
                    {
                        ddlMediaR.Text = Tb.Rows[0]["RMediaNo"].ToString();
                    }
                    else
                    {
                        ddlMediaR.Text = "";
                    }
                    if (!Convert.ToString(Tb.Rows[0]["MediaNo"]).Equals("0"))
                    {
                        ddlMedia.Text = Tb.Rows[0]["MediaNo"].ToString();
                    }
                    else
                    {
                        ddlMedia.Text = "";
                    }
                    txtComMall.Text = Tb.Rows[0]["ComMall"].ToString();
                    txtComTenant.Text = Tb.Rows[0]["ComTenant"].ToString();
                    txtMediaMNo.Text = Tb.Rows[0]["MediaMNo"].ToString();
                    txtMediaMDesc.Text = Tb.Rows[0]["MediaMDesc"].ToString();
                    if (Convert.ToString(Tb.Rows[0]["ChangeAuto"]).Equals("1"))
                    {
                        ChkAutoM.Checked = true;
                    }
                    else
                    {
                        ChkAutoM.Checked = false;
                    }

                    if (Convert.ToString(Tb.Rows[0]["CardType"]).Equals("D"))
                    {
                        rdoD.Checked = true;
                        rdoF.Checked = false;
                    }
                    if (Convert.ToString(Tb.Rows[0]["CardType"]).Equals("F"))
                    {
                        rdoF.Checked = true;
                        rdoD.Checked = false;
                    }
                    if (!Convert.IsDBNull(Tb.Rows[0]["Deletetime"]))
                    {
                        //lblErr2.Text = (String)GetGlobalResourceObject("BaseInfo", "Associator_rabInvalid");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + Label16.Text + (String)GetGlobalResourceObject("BaseInfo", "Associator_rabInvalid") + "'", true);
                    }
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
                }
                else
                {
                    txtNULL();
                    //lblErr2.Text = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidNoData");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ad", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidNoData") + "'", true);
                }
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "treearray()", true);
        //Response.Write("<script language=javascript> window.opener.execScrip('treearray()','JavaScrip');</script>");
    }

    protected void btnDisplay_Click(object sender, EventArgs e)
    {
        Panel1.Visible = false;
        Panel2.Visible = true;
        txtMediaNo.Text  = "";
        txtMediaDesc.Text = "";
        ChkAuto.Checked = false;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "treearray()", true);
    }
    protected void btnDisplayM_Click(object sender, EventArgs e)
    {
        Panel1.Visible = true;
        Panel2.Visible = false;
        txtNULL();
        if (deptid.Value.IndexOf("P") >= 0)
        {
            ddlMedia.Text = deptid.Value.Substring(0, deptid.Value.IndexOf("P"));
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "treearray()", true);
        BindDropList();
    }
}
