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

using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base;
using Base.Page;
using Lease;
using Lease.Customer;
using Lease.Contract;
using Lease.PotBargain;
using Base.Util;
using RentableArea;
using BaseInfo.authUser;
using BaseInfo.User;

public partial class ReportM_RptSale_RptGoodsRejected : BasePage
{
    public string baseInfo;
    public string sRptName;
    public string Fresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            txtBizEDate.Text = DateTime.Now.ToShortDateString();
            txtBizSDate.Text = DateTime.Now.ToShortDateString();
            InitDDL();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_RptGoodsRejected");
            Fresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
    }

    private void InitDDL()
    {
        //绑定店
        BaseBO baseBo = new BaseBO();

        //baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        Resultset rs = baseBo.Query(new Building());
        ddlBuildingName.Items.Add(new ListItem("", ""));
        foreach (Building bd in rs)
            ddlBuildingName.Items.Add(new ListItem(bd.BuildingName, bd.BuildingID.ToString()));

        //绑定楼层
        //baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        Resultset rs1 = baseBo.Query(new Floors());
        ddlFloorName.Items.Add(new ListItem("", ""));
        foreach (Floors bd in rs1)
            ddlFloorName.Items.Add(new ListItem(bd.FloorName, bd.FloorID.ToString()));

        //绑定区域
        //baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        Resultset rs3 = baseBo.Query(new Area());
        ddlAreaName.Items.Add(new ListItem("", ""));
        foreach (Area bd in rs3)
            ddlAreaName.Items.Add(new ListItem(bd.AreaName, bd.AreaID.ToString()));

    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptSale/RptGoodsRejected.aspx");
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[29];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[29];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_RptGoodsRejected");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXCustCode";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustCode");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXCustName";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustName");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXShopCode";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopCode");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXShopName";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXPaidAmt";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PaidAmt");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXSkuId";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_SkuId");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXSkuDesc";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_SkuDesc");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXCommChg";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_CommChg");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXNetAmt";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_NetAmt");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXTotalAmt";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXCommRate";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_CommRate");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXPosID";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PosID");
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "REXUserID";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_UserID");
        paraField[13].CurrentValues.Add(discreteValue[13]);

        paraField[14] = new ParameterField();
        paraField[14].Name = "REXTransTime";
        discreteValue[14] = new ParameterDiscreteValue();
        discreteValue[14].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_TransTime");
        paraField[14].CurrentValues.Add(discreteValue[14]);

        paraField[15] = new ParameterField();
        paraField[15].Name = "REXReceiptID";
        discreteValue[15] = new ParameterDiscreteValue();
        discreteValue[15].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_ReceiptID");
        paraField[15].CurrentValues.Add(discreteValue[15]);

        paraField[16] = new ParameterField();
        paraField[16].Name = "REXCardID";
        discreteValue[16] = new ParameterDiscreteValue();
        discreteValue[16].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_CardID");
        paraField[16].CurrentValues.Add(discreteValue[16]);

        paraField[17] = new ParameterField();
        paraField[17].Name = "REXEFTID";
        discreteValue[17] = new ParameterDiscreteValue();
        discreteValue[17].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_EFTID");
        paraField[17].CurrentValues.Add(discreteValue[17]);

        paraField[18] = new ParameterField();
        paraField[18].Name = "REXMediaMNo";
        discreteValue[18] = new ParameterDiscreteValue();
        discreteValue[18].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_MediaMNo");
        paraField[18].CurrentValues.Add(discreteValue[18]);

        paraField[19] = new ParameterField();
        paraField[19].Name = "REXMediaMDesc";
        discreteValue[19] = new ParameterDiscreteValue();
        discreteValue[19].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_MediaMDesc");
        paraField[19].CurrentValues.Add(discreteValue[19]);

        paraField[20] = new ParameterField();
        paraField[20].Name = "REXSdate";
        discreteValue[20] = new ParameterDiscreteValue();
        discreteValue[20].Value = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Sdate");
        paraField[20].CurrentValues.Add(discreteValue[20]);

        paraField[21] = new ParameterField();
        paraField[21].Name = "REXQBDate";
        discreteValue[21] = new ParameterDiscreteValue();
        discreteValue[21].Value = txtBizSDate.Text;
        paraField[21].CurrentValues.Add(discreteValue[21]);

        paraField[22] = new ParameterField();
        paraField[22].Name = "REXQEDate";
        discreteValue[22] = new ParameterDiscreteValue();
        discreteValue[22].Value = txtBizEDate.Text;
        paraField[22].CurrentValues.Add(discreteValue[22]);

        paraField[23] = new ParameterField();
        paraField[23].Name = "REXSubtotal";
        discreteValue[23] = new ParameterDiscreteValue();
        discreteValue[23].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSalesMediaSum_Subtotal");
        paraField[23].CurrentValues.Add(discreteValue[23]);

        paraField[24] = new ParameterField();
        paraField[24].Name = "REXMallTitle";
        discreteValue[24] = new ParameterDiscreteValue();
        discreteValue[24].Value = Session["MallTitle"].ToString();
        paraField[24].CurrentValues.Add(discreteValue[24]);

        paraField[25] = new ParameterField();
        paraField[25].Name = "REXTransId";
        discreteValue[25] = new ParameterDiscreteValue();
        discreteValue[25].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_ReceiptID");
        paraField[25].CurrentValues.Add(discreteValue[25]);

        paraField[26] = new ParameterField();
        paraField[26].Name = "REXMembCardID";
        discreteValue[26] = new ParameterDiscreteValue();
        discreteValue[26].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblAssociatorCard");
        paraField[26].CurrentValues.Add(discreteValue[26]);

        paraField[27] = new ParameterField();
        paraField[27].Name = "REXBonusAmt";
        discreteValue[27] = new ParameterDiscreteValue();
        discreteValue[27].Value = (String)GetGlobalResourceObject("ReportInfo", "RptBonusAmt");
        paraField[27].CurrentValues.Add(discreteValue[27]);

        paraField[28] = new ParameterField();
        paraField[28].Name = "RptREXTotalAmt";
        discreteValue[28] = new ParameterDiscreteValue();
        discreteValue[28].Value = (String)GetGlobalResourceObject("ReportInfo", "RptREXTotalAmt");
        paraField[28].CurrentValues.Add(discreteValue[28]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string str_sql = "";

        //str_sql =  "Select ConShop.ShopCode,ConShop.ShopId,"+
        //        "ConShop.ShopName,PosID,UserID,BizDate,TransTime,TransSkuMedia.ReceiptID,CardID,EFTID,MediaMNo,MediaMDesc," +
        //        "TransSkuMedia.SkuId,TransSkuMedia.SkuDesc,PaidAmt,CommRate,CommChg,TransSkuMedia.NetAmt,TotalAmt,BonusAmt,TransSkuMedia.TransId,MembCardID   " +
        //        "From TransSkuMedia Left Join TransFooter On convert(varchar(20),TransFooter.TransID)=TransSkuMedia.TransID "+
        //        "Left Join Purhist On convert(varchar(20),TransFooter.TransID) = Purhist.TransID Left Join ConShop On TransSkuMedia.ShopId=ConShop.ShopId Where TotalAmt<= 0 ";

        str_sql = "SELECT conShop.ShopCode,transSku.ShopId,conShop.ShopName,transSku.PosID,transSku.UserID,transSku.BizDate,transSku.TransTime,transSku.ReceiptID,transSku.SkuId,transSku.SkuDesc,transSku.PaidAmt,Purhist.BonusAmt,transSku.TransId,purhist.MembCardID" +
                  " FROM transSku" +
                      "  LEFT JOIN purhist ON (transSku.transID = purhist.transID )" +
                      "  LEFT JOIN conShop ON (transSku.shopID = conShop.shopID)" +
                 " WHERE" +
                       " transSku.transType = 2" +
                       " AND DataSource = 1";

        if (ddlBuildingName.Text != "")
        {
            str_sql = str_sql + " AND TransSku.BuildingID = '" + ddlBuildingName.SelectedValue + "'";
        }
        if (ddlFloorName.Text != "")
        {
            str_sql = str_sql + " AND TransSku.FloorId = '" + ddlFloorName.SelectedValue + "'";
        }
        if (ddlAreaName.Text != "")
        {
            str_sql = str_sql + " AND TransSku.AreaId = '" + ddlAreaName.Text + "'";
        }

        if (txtBizSDate.Text != "")
        {
            str_sql = str_sql + " AND TransSku.BizDate >= '" + txtBizSDate.Text + "'";
        }
        if (txtBizEDate.Text != "")
        {
            str_sql = str_sql + " AND TransSku.BizDate <= '" + txtBizEDate.Text + "'";
        }

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            //str_sql += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
            //            ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
            //            ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
            //            ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
            string strAuth = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
            for (int i = 0; i < 5; i++)
            {
                //将权限查询Sql中Conshop替换成查询表的名称，该表有ShopID字段
                strAuth = strAuth.Replace("ConShop", "transshopday");
            }
            str_sql = str_sql + strAuth;
        }


        str_sql = str_sql + " Order By ConShop.ShopCode";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptGoodsRejected.rpt";

    }


      
}
