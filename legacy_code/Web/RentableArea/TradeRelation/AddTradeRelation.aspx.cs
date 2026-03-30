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
using RentableArea;
using Base.DB;
public partial class RentableArea_TradeRelation_AddTradeRelation : System.Web.UI.Page
{
    TradeRelation tradeRelation = new TradeRelation();
        BaseBO bo = new BaseBO();
        
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //填充状态
           

            int[] status = TradeRelation.GetTradeRelationStatus();
            for (int i = 0; i < status.Length; i++)
            {
                cmbTradeStatus1.Items.Add(new ListItem(TradeRelation.GetTradeRelationStatusDesc(status[i]), status[i].ToString()));
                cmbTradeStatus2.Items.Add(new ListItem(TradeRelation.GetTradeRelationStatusDesc(status[i]), status[i].ToString()));
                cmbTradeStatus3.Items.Add(new ListItem(TradeRelation.GetTradeRelationStatusDesc(status[i]), status[i].ToString()));
            }

            int[] tradeLevelStatus = TradeRelation.GetTradeLevelStatus();
            for (int i = 0; i < tradeLevelStatus.Length; i++)
            {
                cmbTradeLevel1.Items.Add(new ListItem(TradeRelation.GetTradeLevelStatusDesc(tradeLevelStatus[i]), tradeLevelStatus[i].ToString()));
                cmbTradeLevel2.Items.Add(new ListItem(TradeRelation.GetTradeLevelStatusDesc(tradeLevelStatus[i]), tradeLevelStatus[i].ToString()));
                cmbTradeLevel3.Items.Add(new ListItem(TradeRelation.GetTradeLevelStatusDesc(tradeLevelStatus[i]), tradeLevelStatus[i].ToString()));
            }
                    cmbPTradeID_Two();
                    cmbPTradeID_Three();
        }


    }
    protected void btnOk_Click1(object sender, EventArgs e)
    {
        

        tradeRelation.TradeID = RentableAreaApp.GetID("TradeRelation", "TradeID");
        tradeRelation.TradeCode = txtTradeCode1.Text;
        tradeRelation.TradeName = txtTradeName1.Text;
        tradeRelation.PTradeID = 0;
        tradeRelation.TradeLevel = Convert.ToInt32(cmbTradeLevel1.SelectedValue);
        tradeRelation.TradeStatus = Convert.ToInt32(cmbTradeStatus1.SelectedValue);
        try
        {
            //插入数据
            if (bo.Insert(tradeRelation) != -1)
            {
                Response.Write("<script language=javascript>alert('添加成功!');</script>");
                txtTradeCode1.Text = null;
                txtTradeName1.Text = null;
                cmbPTradeID2.Items.Clear();
                cmbPTradeID_Two();
            }
            else
            {
                Response.Write("<script language=javascript>alert('添加失败!');</script>");
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    public void cmbPTradeID_Two()
    {

        bo.WhereClause = "TradeLevel =" + TradeRelation.TRADELEVEL_STATUS_ONE + " and TradeStatus=" + TradeRelation.TRADERELATION_STATUS_VALID;
           Resultset rs = bo.Query(tradeRelation);

            //显示要更新的数据
           foreach (TradeRelation tradeRelations in rs)
            {
                this.cmbPTradeID2.Items.Add(new ListItem(tradeRelations.TradeName, tradeRelations.TradeID.ToString()));
            }
    }
    public void cmbPTradeID_Three()
    {

        bo.WhereClause = "TradeLevel =" + TradeRelation.TRADELEVEL_STATUS_TWO + " and TradeStatus=" + TradeRelation.TRADERELATION_STATUS_VALID;
        Resultset rs = bo.Query(tradeRelation);

        //显示要更新的数据
        foreach (TradeRelation tradeRelations in rs)
        {
            this.cmbPTradeID3.Items.Add(new ListItem(tradeRelations.TradeName, tradeRelations.TradeID.ToString()));
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        tradeRelation.TradeID = RentableAreaApp.GetID("TradeRelation", "TradeID");
        tradeRelation.TradeCode = txtTradeCode2.Text;
        tradeRelation.TradeName = txtTradeName2.Text;
        tradeRelation.PTradeID = Convert.ToInt32(cmbPTradeID2.SelectedValue);
        tradeRelation.TradeLevel = Convert.ToInt32(cmbTradeLevel2.SelectedValue);
        tradeRelation.TradeStatus = Convert.ToInt32(cmbTradeStatus2.SelectedValue);
        try
        {
            //插入数据
            if (bo.Insert(tradeRelation) != -1)
            {
                Response.Write("<script language=javascript>alert('添加成功!');</script>");
                cmbPTradeID3.Items.Clear();
                cmbPTradeID_Three();
                txtTradeCode1.Text = null;
                txtTradeName1.Text = null;
            }
            else
            {
                Response.Write("<script language=javascript>alert('添加失败!');</script>");
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
    protected void Button11_Click(object sender, EventArgs e)
    {
        try
        {
            TradeDef tradeDef = new TradeDef();
            BaseTrans trans = new BaseTrans();
            Resultset rs = new Resultset();
            trans.BeginTrans();

            tradeRelation.TradeID = RentableAreaApp.GetID("TradeRelation", "TradeID");
            tradeRelation.TradeCode = txtTradeCode3.Text;
            tradeRelation.TradeName = txtTradeName3.Text;
            tradeRelation.PTradeID = Convert.ToInt32(cmbPTradeID3.SelectedValue);
            tradeRelation.TradeLevel = Convert.ToInt32(cmbTradeLevel3.SelectedValue);
            tradeRelation.TradeStatus = Convert.ToInt32(cmbTradeStatus3.SelectedValue);
            trans.Insert(tradeRelation);

            tradeDef.Trade3ID = tradeRelation.TradeID;
            tradeDef.Trade3Code = tradeRelation.TradeCode;
            tradeDef.Trade3Name = tradeRelation.TradeName;

            bo.WhereClause = "TradeID = " + tradeRelation.PTradeID;
            rs = bo.Query(tradeRelation);
            tradeRelation = rs.Dequeue() as TradeRelation;
            tradeDef.Trade2ID = tradeRelation.TradeID;
            tradeDef.Trade2Code = tradeRelation.TradeCode;
            tradeDef.Trade2Name = tradeRelation.TradeName;

            bo.WhereClause = "TradeID = " + tradeRelation.PTradeID;
            rs = bo.Query(tradeRelation);
            tradeRelation = rs.Dequeue() as TradeRelation;
            tradeDef.Trade1ID = tradeRelation.TradeID;
            tradeDef.Trade1Code = tradeRelation.TradeCode;
            tradeDef.Trade1Name = tradeRelation.TradeName;

            tradeDef.TradeDefStatus = tradeRelation.TradeStatus;
            tradeDef.Note = txtNote.Text;


            trans.Insert(tradeDef);
            trans.Commit();
            txtTradeCode1.Text = null;
            txtTradeName1.Text = null;
            Response.Write("<script language=javascript>alert('添加成功!');</script>");
            Response.Redirect("QueryTradeRelation.aspx");
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
    protected void Button12_Click(object sender, EventArgs e)
    {
        Response.Redirect("QueryTradeRelation.aspx");
    }
}
