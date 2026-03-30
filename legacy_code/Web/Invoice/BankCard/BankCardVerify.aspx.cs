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
using Base.Sys;
using Invoice.BankCard;
using Base.Page;
public partial class Invoice_BankCard_BankCardVerify : BasePage
{
    public string baseInfo;
    public int intlog = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            showBank(9, DateTime.Now.ToString(), DateTime.Now.ToString());
            showPOS(9, DateTime.Now.ToString(), DateTime.Now.ToString());
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "BankCard_BankCardVerify");
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        Resultset rs = new Resultset();
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "ReconcId =0 AND BizDate  BETWEEN '" + txtCustCode.Text + "' AND '" + txtEndDate.Text + "' AND SUBSTRING(EFTID,1,3) = '" + RadioBank.SelectedValue + "' ORDER BY BizDate";
        rs = baseBO.Query(new TransCard());

        if(rs.Count > 0)
        {
            foreach (TransCard transCard in rs)
            {
                decimal amt = transCard.Amt;
                if (transCard.MediaType == TransCard.MEDIATYPE_NEGATIVE)
                {
                    amt = 0 - amt;
                }
                UpdateTransCard(transCard.TransCrdID, transCard.EFTID.ToString().Trim(),Convert.ToString(transCard.CardID),transCard.BizDate.ToString("yyyy-MM-dd"), amt,transCard.TransID);
            }
        }
        showBank(BankTransDet.BANKTRANSDET_RECONCTYPE_NOT_ANTITHESES, txtCustCode.Text, txtEndDate.Text);
        showPOS(TransCard.TRANSCARD_TRANSTYPE_NO, txtCustCode.Text, txtEndDate.Text);
        lblLog.Text = intlog.ToString();
    }

    protected void showBank(int reconcType,string startDate,string endDate)
    {
        /*绑定结算单明细*/
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        baseBO.WhereClause = "ReconcType=" + reconcType + " and ReconcID=0 and SUBSTRING(BankEFTID,1,3) = '" + RadioBank.SelectedValue + "' and Convert(varchar(10),BankTransTime,120) BETWEEN '" + startDate + "' and '" + endDate + "'";
        DataTable dt = baseBO.QueryDataSet(new BankTransDet()).Tables[0];

        pds.DataSource = dt.DefaultView;

        GrdBank.PageSize = 9;
        pds.PageSize = 9;

        GrdBank.AllowPaging = true;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GrdBank.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdBank.DataSource = pds;
            GrdBank.DataBind();
        }
        else
        {

            this.GrdBank.DataSource = pds;
            this.GrdBank.DataBind();
            spareRow = GrdBank.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdBank.DataSource = pds;
            GrdBank.DataBind();
        }
        ClearGridViewSelectedBank();
    }

    protected void showPOS(int reconcType, string startDate, string endDate)
    {
        /*绑定结算单明细*/
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        baseBO.WhereClause = "ReconcType=" + reconcType + " and ReconcID=0 and SUBSTRING(EFTID,1,3) = '" + RadioBank.SelectedValue + "' and BizDate BETWEEN '" + startDate + "' and '" + endDate + "'";
        DataTable dt = baseBO.QueryDataSet(new TransCard()).Tables[0];

        int count = dt.Rows.Count;
        for (int i = 0; i < count; i++)
        {
            if (dt.Rows[i]["MediaType"].ToString() == TransCard.MEDIATYPE_NEGATIVE)
            {
                dt.Rows[i]["Amt"] = 0 - Convert.ToDecimal(dt.Rows[i]["Amt"]);
            }
        }

        pds.DataSource = dt.DefaultView;

        GrdPOS.AllowPaging = true;
        GrdPOS.PageSize = 9;
        pds.PageSize = 9;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GrdPOS.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdPOS.DataSource = pds;
            GrdPOS.DataBind();
        }
        else
        {

            this.GrdPOS.DataSource = pds;
            this.GrdPOS.DataBind();
            spareRow = GrdPOS.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdPOS.DataSource = pds;
            GrdPOS.DataBind();
        }
        ClearGridViewSelectedPOS();
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

    protected void UpdateTransCard(int transCrdID, string bankEFTID, string bankCardID,String bankTransTime,decimal bankAmt,string transID)
    {
        BankTransDet bankTransDet = new BankTransDet();
        BankTransDet bankTransDets = new BankTransDet();
        TransCard transCard = new TransCard();
        BaseTrans baseTrans = new BaseTrans();
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();


        //baseBO.WhereClause = "bankEFTID='" + bankEFTID + "' and  bankTransTime='" + bankTransTime + "' and  bankCardID='" + bankCardID + "' and  bankAmt=" + bankAmt;
        //baseBO.WhereClause = "bankEFTID='" + bankEFTID + "' and  Convert(varchar(10),bankTransTime,120)='" + bankTransTime + "' and  bankCardID='" + bankCardID + "' and  bankAmt=" + bankAmt + " and ReconcType = " + BankTransDet.BANKTRANSDET_RECONCTYPE_NOT_ANTITHESES;
        string str_sql = "SELECT MIN(BankTransID) as BankTransID,BankEFTID,BankTransTime,BankCardID,BankAmt,BankChgAmt,BankNetAmt,ReconcType,DataSource " +
                        " FROM BankTransDet " +
                        " WHERE bankEFTID='" + bankEFTID + "' AND  Convert(varchar(10),bankTransTime,120)='" + bankTransTime + "' AND  bankCardID='" + bankCardID +
                        "' AND  bankAmt=" + bankAmt + " AND ReconcType = " + BankTransDet.BANKTRANSDET_RECONCTYPE_NOT_ANTITHESES +
                        " AND SUBSTRING(BankEFTID,1,3) = '" + RadioBank.SelectedValue +
                        "' GROUP BY BankEFTID,BankTransTime,BankCardID,BankAmt,BankChgAmt,BankNetAmt,ReconcType,DataSource";
        //rs = baseBO.Query(bankTransDet);
        DataSet bankDS = baseBO.QueryDataSet(str_sql);
        if (bankDS.Tables[0].Rows.Count > 0)
        {
            string sql = "SELECT TransHeader.TenantId FROM TransHeader,TransCard WHERE TransHeader.TransID = TransCard.TransID AND TransCard.TransID = '" + transID + "'";
            baseBO.WhereClause = "";
            DataSet ds = baseBO.QueryDataSet(sql);

            baseTrans.BeginTrans();

            //bankTransDets = rs.Dequeue() as BankTransDet;
            baseTrans.WhereClause = "bankEFTID='" + bankEFTID + "' and  Convert(varchar(10),bankTransTime,120)='" + bankTransTime + "' and  bankCardID='" + bankCardID + "' and  bankAmt=" + bankAmt + " and BankTransID = " + bankDS.Tables[0].Rows[0]["BankTransID"];
            bankTransDet.ReconcType = BankTransDet.BANKTRANSDET_RECONCTYPE_SUCCEED_ANTITHESES;
            bankTransDet.ReconcID = transCrdID;
            bankTransDet.ShopID = Convert.ToInt32(ds.Tables[0].Rows[0]["TenantId"]);
            if (baseTrans.Update(bankTransDet) < 1)
            {
                baseTrans.Rollback();
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_Error") + "'", true);
                return;
            }

            decimal cardAmt = bankAmt;
            if (cardAmt < 0)
            {
                cardAmt = 0 - cardAmt;
            }
            baseTrans.WhereClause = "EFTID='" + bankEFTID + "' and  BizDate='" + bankTransTime + "' and  CardID='" + bankCardID + "' and  Amt=" + cardAmt + " and TransCrdID = " + transCrdID;
            transCard.ReconcID = Convert.ToDecimal(bankDS.Tables[0].Rows[0]["BankTransID"]);
            transCard.ReconcType = TransCard.TRANSCARD_TRANSTYPE_YES;
            if (baseTrans.Update(transCard) < 1)
            {
                baseTrans.Rollback();
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_Error") + "'", true);
                return;
            }
            intlog++;
            baseTrans.Commit();
        }
    }
    protected void GrdBank_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
    }
    protected void GrdPOS_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void GrdBank_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdBank.PageIndex = e.NewPageIndex;
        showBank(BankTransDet.BANKTRANSDET_RECONCTYPE_NOT_ANTITHESES, txtCustCode.Text, txtEndDate.Text);
        GrdBank.DataBind();
        ClearGridViewSelectedBank();
    }

    private void ClearGridViewSelectedBank()
    {
        string gIntro = "";
        foreach (GridViewRow gvr in GrdBank.Rows)
        {
            if (gvr.Cells[0].Text != "&nbsp;")
            {
                gIntro = gvr.Cells[2].Text.ToString();
                gvr.Cells[2].Text = SubStr(gIntro, 10);
            }
        }
    }

    protected void GrdPOS_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdPOS.PageIndex = e.NewPageIndex;
        showPOS(BankTransDet.BANKTRANSDET_RECONCTYPE_NOT_ANTITHESES, txtCustCode.Text, txtEndDate.Text);
        GrdPOS.DataBind();
        ClearGridViewSelectedPOS();
        
    }

    private void ClearGridViewSelectedPOS()
    {
        string gIntro = "";
        foreach (GridViewRow gvr in GrdPOS.Rows)
        {
            if (gvr.Cells[0].Text != "&nbsp;")
            {
                gIntro = gvr.Cells[2].Text.ToString();
                gvr.Cells[2].Text = SubStr(gIntro, 10);
            }
        }
    }
}
