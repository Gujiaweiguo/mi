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
using Lease.ConShop;
using Sell;
using Base;
using Base.Page;

public partial class Sell_CashInformation : BasePage
{
    public string baseInfo;
    public int edit;
    public string sql;
    public string url;
    protected void Page_Load(object sender, EventArgs e)

    {
        edit = Convert.ToInt16(Request.QueryString["modify"]);
        if (!IsPostBack)
        {

            if (edit == 0)
            {
                txtBizDate.Enabled = true;
                txtUserID.Enabled = true;
                txtUserName.Enabled = false;
                txtBizDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                page(edit);
                SUM();
                baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_NewCashInfo");
                url = "sell/CashInformation.aspx?modify=0";
                btnCancel.Visible = true;

            }
            if (edit == 1)
            {
                txtBizDate.Enabled = false;
                txtUserID.Enabled = false;
                btnQuery.Visible = false;
                txtUserName.Enabled = false;
                string userid = Request.Cookies["Info"].Values["UserID"].ToString();
                string BizDate = Request.Cookies["Info"].Values["BizDate"].ToString();
                txtBizDate.Text = BizDate;
                txtUserID.Text = userid;
                page(edit);
                Bindusername();
                SUM();
                baseInfo = (String)GetGlobalResourceObject("BaseInfo", "CashInformation_Query");
                url = "sell/CashInformationQuery.aspx";
                btnCancel.Visible = false;
             

            }

        }


    }
    protected void page(int edit)
    {
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        if (edit==0)
        {
             sql = "SELECT MediaM.mediaMNo, " +
                      " MediaM.mediaMDesc, " +
                      " ISNULL(amountt,0) AS amount, " +
                      " ISNULL(localAmt,0) AS localAmt " +
                "FROM  MediaM INNER JOIN " +
                     " (" +
                        " SELECT transMedia.MediaMCd," +
                               " SUM(CASE MediaType WHEN 'T' THEN amountt WHEN 'C' THEN 0 - amountt ELSE 0 END) AS amountt," +
                               " SUM(CASE MediaType WHEN 'T' THEN localAmt WHEN 'C' THEN 0 - localAmt ELSE 0 END) AS localAmt" +
                          " FROM TransMedia INNER JOIN " +
                               " TransHeader ON (transMedia.transID = transHeader.transID)" +
                         " WHERE transHeader.bizDate = '" + txtBizDate.Text.Trim().ToString() + "'" +
                "AND transHeader.userID = '" + txtUserID.Text.Trim().ToString() + "'" +
                           " AND transHeader.training = 'N' " +
                      " GROUP BY transMedia.MediaMCd" +
                      " ) AS TMedia" +
                      " ON (TMedia.MediaMCd = MediaM.mediaMNo)" +
                " ORDER BY MediaM.mediaMNo";
        }
        if (edit == 1)
        {
            sql = " select PaymentID as MediaMNo,MediaCd as mediaMDesc,Amountt  as amount,LocalAmt from casherpayment where " +
                         "bizDate = '" + txtBizDate.Text.Trim().ToString() + "' AND userID ='" + txtUserID.Text.Trim().ToString() + "'";

        }

        DataSet ds = baseBO.QueryDataSet(sql);
        dt = ds.Tables[0];
        pds.DataSource = dt.DefaultView;
        GrdCashInfo.DataSource = pds;
        GrdCashInfo.DataBind();
        spareRow = GrdCashInfo.Rows.Count;
        for (int i = 0; i < GrdCashInfo.PageSize - spareRow; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        GrdCashInfo.DataSource = pds;
        GrdCashInfo.DataBind();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
      
        

    }
    protected void Bindusername()
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        TpUsr tpUsr = new TpUsr();
        baseBO.WhereClause = "TPUsrId = " + txtUserID.Text.Trim().ToString();
        rs = baseBO.Query(tpUsr);
        if (rs.Count == 1)
        {
            tpUsr = rs.Dequeue() as TpUsr;
            txtUserName.Text = tpUsr.TPUsrNm;
        }
    }

    protected void SUM()
    {

        decimal sum1 = 0;
        decimal sum2 = 0;
            for (int i = 0; i < Convert.ToInt32(ViewState["spareRow"]); i++)
            {

                sum1 += Convert.ToDecimal(((Label)GrdCashInfo.Rows[i].FindControl("Labamount")).Text);
                sum2 += Convert.ToDecimal(((TextBox)GrdCashInfo.Rows[i].FindControl("txtlocalAmt")).Text);
                ((TextBox)GrdCashInfo.Rows[i].FindControl("txtbalance")).Text = (Convert.ToDecimal(((Label)GrdCashInfo.Rows[i].FindControl("Labamount")).Text) - Convert.ToDecimal(((TextBox)GrdCashInfo.Rows[i].FindControl("txtlocalAmt")).Text)).ToString();

            }
            ((Label)GrdCashInfo.FooterRow.FindControl("labamountSum")).Text = sum1.ToString();
            ((Label)GrdCashInfo.FooterRow.FindControl("lablocalAmtSum")).Text = sum2.ToString();
            ((Label)GrdCashInfo.FooterRow.FindControl("labbalanceSum")).Text = (sum1 - sum2).ToString();

            GrdCashInfoClear();
    }

#region 清理
    private void GrdCashInfoClear()
    {
        foreach (GridViewRow gvr in GrdCashInfo.Rows)
        {
            if (gvr.Cells[0].Text=="&nbsp;")
            {
                gvr.Cells[3].Text = "";
                gvr.Cells[4].Text = "";
            }
        }
    }
#endregion

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string message = (string)GetGlobalResourceObject("BaseInfo", "CashInformation_message1"); //请进行最终签退后再进行总收银
        string message1 = (string)GetGlobalResourceObject("BaseInfo", "CashInformation_message2"); //收银员已进行总收，请使用修改功能
        string message2 = (string)GetGlobalResourceObject("BaseInfo", "Prompt_Success"); //操作成功
        string message3 = (string)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose"); //操作失败
        #region  添加
        if (edit == 0)
        {


            BaseBO baseBO = new BaseBO();
            BaseTrans baseTrans = new BaseTrans();
            Resultset rs1 = new Resultset();
            TransHeader transHeader = new TransHeader();

            baseBO.WhereClause = "userId = '" + txtUserID.Text.Trim().ToString() + "' AND bizDate='" + txtBizDate.Text.Trim().ToString() + "'  AND TRANSTATUS='Last Sign Off' ";
            rs1 = baseBO.Query(transHeader);
            if (rs1.Count > 0)
            {
                transHeader = rs1.Dequeue() as TransHeader;
                ViewState["transID"] = transHeader.TransId.ToString();
                BaseBO baseBO2 = new BaseBO();
                Resultset rs2 = new Resultset();
                TransMedia transMedia = new TransMedia();
                baseBO2.WhereClause = "TransID = " + Convert.ToDecimal(ViewState["transID"]);
                rs2 = baseBO2.Query(transMedia);
                if (rs2.Count > 0)
                {
                    transMedia = rs2.Dequeue() as TransMedia;
                    if (transMedia.PaymentStatus == "Y")
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('" + message1 + "')", true);
                        GrdCashInfoClear();
                        return;
                    }
                    else
                    {
                        baseTrans.BeginTrans();

                        foreach (GridViewRow gvr in GrdCashInfo.Rows)
                        {
                            if (((Label)gvr.FindControl("labmediaMDesc")).Text != "" && ((Label)gvr.FindControl("labmediaMDesc")).Text != null)
                            {

                                CasherPayment cashPayMent = new CasherPayment();

                                if (edit == 0)
                                {
                                    cashPayMent.PaymentID = BaseApp.GetID("CasherPayment", "PaymentID");
                                    cashPayMent.BizDate = txtBizDate.Text.Trim().ToString();
                                    cashPayMent.UserId = txtUserID.Text.Trim().ToString();
                                    cashPayMent.MediaCd = ((Label)gvr.FindControl("labmediaMDesc")).Text;
                                    cashPayMent.Amountt = Convert.ToDecimal(((Label)gvr.FindControl("Labamount")).Text);
                                    cashPayMent.LocalAmt = Convert.ToDecimal(((TextBox)gvr.FindControl("txtlocalAmt")).Text);
                                    cashPayMent.PayAmt = Convert.ToDecimal(((TextBox)gvr.FindControl("txtlocalAmt")).Text);
                                    int MediaMNo = Convert.ToInt32(gvr.Cells[0].Text);
                                    string sql = "UPDATE TransMedia " +
                                        "SET PaymentStatus = 'Y',PaymentID='" + cashPayMent.PaymentID + "'" +
                                        " WHERE TransMedia.MediaMCd='" + MediaMNo + "'And TransID IN (" +
                                        " SELECT TransID " +
                                        " FROM transHeader" +
                                        " WHERE transHeader.bizDate = '" + cashPayMent.BizDate + "'" +
                                        " AND transHeader.userID ='" + cashPayMent.UserId + "'" +
                                        " AND transHeader.training = 'N')";
                                    if (baseTrans.ExecuteUpdate(sql) < 1)
                                    {
                                        baseTrans.Rollback();
                                        return;
                                    }

                                    if (baseTrans.Insert(cashPayMent) < 1)
                                    {
                                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('" + message1 + "')", true);
                                        GrdCashInfoClear();
                                        baseTrans.Rollback();
                                        return;
                                    }
                                }
                                if (edit == 1)
                                {

                                    cashPayMent.Amountt = Convert.ToDecimal(((Label)gvr.FindControl("Labamount")).Text);
                                    cashPayMent.LocalAmt = Convert.ToDecimal(((TextBox)gvr.FindControl("txtlocalAmt")).Text);
                                    cashPayMent.PayAmt = Convert.ToDecimal(((TextBox)gvr.FindControl("txtlocalAmt")).Text);
                                    baseTrans.WhereClause = "PayMentID='" + Convert.ToInt32(gvr.Cells[0].Text) + "'";
                                    if (baseTrans.Update(cashPayMent) < 1)
                                    {
                                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('" + message1 + "')", true);
                                        baseTrans.Rollback();
                                        return;
                                    }

                                }
                            }

                        }

                        baseTrans.Commit();
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('" + message2 + "')", true);
  


                    }

                }



            }
            else
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('" + message + "')", true);
                GrdCashInfoClear();
                return;
            }
            
        }
        #endregion
        #region 修改
        if (edit == 1)
        
        {

                BaseBO baseBO = new BaseBO();
                BaseTrans baseTrans = new BaseTrans();
                Resultset rs1 = new Resultset();
                ArrayList aryList = new ArrayList();

                            baseTrans.BeginTrans();

                            foreach (GridViewRow gvr in GrdCashInfo.Rows)
                            {
                                if (((Label)gvr.FindControl("labmediaMDesc")).Text != "" && ((Label)gvr.FindControl("labmediaMDesc")).Text != null)
                                {

                                        CasherPayment cashPayMent = new CasherPayment();
                                        cashPayMent.BizDate = txtBizDate.Text.Trim().ToString();
                                        cashPayMent.UserId = txtUserID.Text.Trim().ToString();
                                        cashPayMent.MediaCd = ((Label)gvr.FindControl("labmediaMDesc")).Text;
                                        cashPayMent.Amountt = Convert.ToDecimal(((Label)gvr.FindControl("Labamount")).Text);
                                        cashPayMent.LocalAmt = Convert.ToDecimal(((TextBox)gvr.FindControl("txtlocalAmt")).Text);
                                        cashPayMent.PayAmt = Convert.ToDecimal(((TextBox)gvr.FindControl("txtlocalAmt")).Text);
                                        baseTrans.WhereClause = "PayMentID='" + Convert.ToInt32(gvr.Cells[0].Text) + "'";
                                        if (baseTrans.Update(cashPayMent) < 1)
                                        {

                                            baseTrans.Rollback();
                                            return;
                                        }

                                }

                            }

                            baseTrans.Commit();
                            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('" + message2 + "')", true);
                            GrdCashInfoClear();
                        }
        #endregion

       GrdCashInfoClear();
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        Response.Redirect("CashInformation.aspx?modify=0");

      
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        SUM();
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (txtBizDate.Text.Trim() != "" && txtUserID.Text.Trim() != "")
        {
            page(edit);
            SUM();
            Bindusername();
        }
        else 
        {
            txtUserID.Focus();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidCondition") + "'", true);
      
        
        }
        GrdCashInfoClear();
    }

}
