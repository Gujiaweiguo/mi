using System;
using CrystalDecisions.Shared;
using BaseInfo.User;
using Base.Page;
using Base.DB;
using Base.Biz;
using RentableArea;
using BaseInfo.authUser;


/// <summary>
/// 编写人:何思键
/// 编写时间:2009年4月20日
/// </summary>
public partial class Rpt_TradeSalesAnalysis:BasePage
{
    public string baseInfo = "";
    private string BID = "";
    private string FID = "";
    private string buildingName = "";
    private string floorName = "";
    private int buildingID = 0;
    private int floorID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        string backhome = "";
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_BackHome");
        BID = Request.QueryString["BuildingID"] + "";
        FID = Request.QueryString["FloorID"] + "";

        int BuildingID = 0;
        int FloorID = 0;

        if (BID.Length == 0)
        {
            BuildingID = 0;
            if (FID.Length > 0 && !FID.Contains("d"))
            {
                FloorID = int.Parse(FID);
            }
            else
            {
                FloorID = 0;
            }
            if (BuildingID == 0)
            {
                BaseBO baseBo = new BaseBO();
                baseBo.WhereClause = " Floors.FloorID=" + FloorID;
                Resultset rs = baseBo.Query(new Floors());
                foreach (Floors floor in rs)
                {
                    floorName = floor.FloorName;
                }
            }
            backhome = (String)GetGlobalResourceObject("BaseInfo", "Menu_BackHome");
            txtHidden.Value = "Disktop.aspx?FloorID=" + FloorID + "&FloorName=" + floorName;

        }
        else
        {
            FloorID = 0;
            if (BID.Length > 0 && !BID.Contains("d"))
            {
                BuildingID = int.Parse(BID);
            }
            else
            {
                BuildingID = 0;
            }
            backhome = (String)GetGlobalResourceObject("BaseInfo", "Menu_BackHome");
            txtHidden.Value = "Disktop.aspx";

        }

        if (!IsPostBack)
        {
            InitDDL();
        }
    }

    //绑定时间
    private void InitDDL()
    {
        txtsDate.Text = DateTime.Now.ToShortDateString();
        txteDate.Text = DateTime.Now.ToShortDateString();
    }

    //绑定数据
    private void BindData()
    {

        ParameterFields Fields = new ParameterFields();
        ParameterField[] Field = new ParameterField[8];
        ParameterDiscreteValue[] DiscreteValue = new ParameterDiscreteValue[8];
        ParameterRangeValue RangeValue = new ParameterRangeValue();

        Field[0] =new ParameterField();
        Field[0].Name = "REXTitle";
        DiscreteValue[0] = new ParameterDiscreteValue();
        DiscreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo","Rpt_TradeSalesAnalysis");
        Field[0].CurrentValues.Add(DiscreteValue[0]);

        Field[1] =new ParameterField();
        Field[1].Name = "REXMainTitle";
        DiscreteValue[1] = new ParameterDiscreteValue();
        DiscreteValue[1].Value = Session["MallTitle"].ToString();
        Field[1].CurrentValues.Add(DiscreteValue[1]);

        if(BID.Length > 0 && !BID.Contains("d"))
        {
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = " Building.BuildingID = " + int.Parse(BID);
            Resultset rs = baseBo.Query(new Building());
            foreach (Building building in rs)
            {
                buildingName = building.BuildingName;
            }
        }
        Field[2] = new ParameterField();
        Field[2].Name = "REXBuildingName";
        DiscreteValue[2] = new ParameterDiscreteValue();
        DiscreteValue[2].Value = buildingName;
        Field[2].CurrentValues.Add(DiscreteValue[2]);

        if (FID.Length > 0 && !FID.Contains("d"))
        {
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = " Floors.FloorID = " + int.Parse(FID);
            Resultset rs = baseBo.Query(new Floors());
            foreach (Floors floor in rs)
            {
                floorName = floor.FloorName;
            }
        }

        Field[3] = new ParameterField();
        Field[3].Name = "REXFloorName";
        DiscreteValue[3] = new ParameterDiscreteValue();
        DiscreteValue[3].Value = floorName;
        Field[3].CurrentValues.Add(DiscreteValue[3]);

        Field[4] =new ParameterField();
        Field[4].Name = "REXCheckDate";
        DiscreteValue[4] = new ParameterDiscreteValue();
        DiscreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo","Rpt_SearcheDate");
        Field[4].CurrentValues.Add(DiscreteValue[4]);

        Field[5] = new ParameterField();
        Field[5].Name = "REXSDate";
        DiscreteValue[5] = new ParameterDiscreteValue();
        DiscreteValue[5].Value = DateTime.Parse(txtsDate.Text.Trim()).ToShortDateString();
        Field[5].CurrentValues.Add(DiscreteValue[5]);

        Field[6] = new ParameterField();
        Field[6].Name = "REXEDate";
        DiscreteValue[6] = new ParameterDiscreteValue();
        DiscreteValue[6].Value = DateTime.Parse(txteDate.Text.Trim()).ToShortDateString();
        Field[6].CurrentValues.Add(DiscreteValue[6]);

        Field[7] = new ParameterField();
        Field[7].Name = "REXTotal";
        DiscreteValue[7] = new ParameterDiscreteValue();
        DiscreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "Account_lblTotalMoney");
        Field[7].CurrentValues.Add(DiscreteValue[7]);


        foreach(ParameterField pf in Field)
        {
            Fields.Add(pf);
        }

        string str_sql = "";

        string strAnd = "";

        //权限设置
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string strAuth = "";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            strAuth = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE  + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";

            for (int i = 0; i < 5; i++)
            {
                strAuth = strAuth.Replace("ConShop", "transshopmth");
            }
        }

      
        if (RB1.Checked)
        {
            strAnd = "";
        }
        if (RB2.Checked)
        {
            strAnd = " AND TransSku.datasource=1";
        }
        if (RB3.Checked)
        {
            strAnd = " AND TransSku.datasource=2";
        }
        if (RB4.Checked)
        {
            strAnd = " AND TransSku.datasource=3";
        }

        

        //判断执行显示的报表
        if (FID.Length > 0)
        {
            ViewRptByFloorID(FID, str_sql, strAnd, Fields, strAuth);
        }
        if (BID.Length > 0)
        {
            ViewRptByBuildingID(BID, str_sql, strAnd, Fields, strAuth);
        }

    }

    //当传递大楼ID显示的报表
    private void ViewRptByBuildingID(string BID, string str_sql, string strAnd, ParameterFields Fields, string authWhere)
    {
        if (BID.Length > 0 && !BID.Contains("d"))
        {

            buildingID = int.Parse(BID.ToString());
        }
        else
        {
            buildingID = 101;
        }
        strAnd = strAnd + " And Building.BuildingID =" + buildingID;
        str_sql = "select (SELECT tradeName FROM TradeRelation AS tr1 WHERE tr1.TradeID = tr2.tradeID )  Trade2Name,TransSku.FloorName, RIGHT('0' + rtrim(convert(char(2),datepart(hh,transtime)))+'h',3) transTime, sum(PaidAmt) PAmt from TransSku " +
                  " INNER JOIN" + " TradeRelation AS tr2 ON (tr2.tradeID = TransSku.trade2ID) " +
                  " INNER JOIN Building ON (TransSku.Buildingid = Building.Buildingid) " +
                  " where TransSku.bizDate between '" + txtsDate.Text.Trim() + "' AND '" + txteDate.Text.Trim() + "' " +
                  authWhere + strAnd +
                  " group by tr2.tradeID,TransSku.FloorName,RIGHT('0' + rtrim(convert(char(2),datepart(hh,transtime)))+'h',3)";
                  //+ " UNION ALL SELECT tradeName, floors.floorName,transTime.transTime, 0 FROM TradeRelation " + 
                  //"INNER JOIN TransSku ON (TradeRelation.tradeID = TransSku.trade2ID) " +
                  //"INNER JOIN floors ON (TradeRelation.TradeLevel = 1 AND 1=1) " +
                  //"INNER JOIN" + " transTime ON (1=1)" + authWhere;
        Session["paraFil"] = Fields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\VisualAnalysis\\Report\\TradeSalesAnalysis.rpt";
    }

    //当传递楼层ID显示的报表
    private void ViewRptByFloorID(string FID, string str_sql, string strAnd, ParameterFields Fields, string authWhere)
    {
        if (FID.Length > 0 && !FID.Contains("d"))
        {
            floorID = int.Parse(FID.ToString());
        }
        else
        {
            floorID = 0;
        }
        strAnd = strAnd + " And Floors.FloorID =" + floorID;
        str_sql = "select (SELECT tradeName FROM TradeRelation AS tr1 WHERE tr1.TradeID = tr2.tradeID ) " + " Trade2Name,TransSku.FloorName," + " RIGHT('0' + rtrim(convert(char(2),datepart(hh,transtime)))+'h',3) transTime," + " sum(PaidAmt) PAmt" + " from TransSku INNER JOIN" + " TradeRelation AS tr2 ON (tr2.tradeID = TransSku.trade2ID) INNER JOIN Floors ON (TransSku.Floorid = Floors.Floorid) " + " where TransSku.bizDate between '" + txtsDate.Text.Trim() + "' AND '" + txteDate.Text.Trim() + "' " + authWhere + strAnd +
" group by tr2.tradeID,TransSku.FloorName,RIGHT('0' + rtrim(convert(char(2),datepart(hh,transtime)))+'h',3)";
//" UNION ALL" +
//" SELECT tradeName," + " floors.floorName," + " transTime," + " 0" + " FROM TradeRelation INNER JOIN" +
//" floors ON (TradeRelation.TradeLevel = 1 AND 1=1) INNER JOIN" + " transTime ON (1=1)" + authWhere;
        Session["paraFil"] = Fields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\VisualAnalysis\\Report\\TradeSalesAnalysis.rpt";
    }

    //查询操作
    protected void btnOK_Click(object sender, EventArgs e)
    {

        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        if (txtsDate.Text.Trim() != null && txteDate.Text.Trim() != null)
        {
            BindData();
            this.Response.Redirect("../../ReportM/ReportShow.aspx");
        }
        else
        {
            return;
        }
    }

    //撤消操作
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        txtsDate.Text = DateTime.Now.ToShortDateString();
        txteDate.Text = DateTime.Now.ToShortDateString();
        RB1.Checked = true;
        RB2.Checked = false;
        RB3.Checked = false;
        RB4.Checked = false;
    }
}
