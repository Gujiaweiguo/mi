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

public partial class Lease_TradeRelation_TradeRelationSelect : BasePage
{
    public string selectTradeLevel;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ShowTree();
            selectTradeLevel = (String)GetGlobalResourceObject("BaseInfo", "Prompt_LeaseTradeID");
        }
    }

    private void ShowTree()
    {
        BaseBO baseBO = new BaseBO();
        string jsdept = "";

        baseBO.WhereClause = "TradeStatus = " + TradeRelation.TRADERELATION_STATUS_VALID;
        baseBO.OrderBy = "TradeLevel,PTradeID";

        Resultset rs = baseBO.Query(new TradeRelation());
        
        if (rs.Count > 0)
        {
            jsdept = "100" + "|" + "0" + "|" + (String)GetGlobalResourceObject("BaseInfo", "LeaseholdContract_labTradeID") + "^";
            foreach (TradeRelation tradeRelation in rs)
            {
                if (tradeRelation.TradeLevel == 1)
                {
                    jsdept += tradeRelation.TradeID + "|" + "100" + "|" + tradeRelation.TradeName + "^";
                }
                else
                {
                    jsdept += tradeRelation.TradeID + "|" + tradeRelation.PTradeID + "|" + tradeRelation.TradeName + "^";
                }
            }
            depttxt.Value = jsdept;
        }
    }
}
