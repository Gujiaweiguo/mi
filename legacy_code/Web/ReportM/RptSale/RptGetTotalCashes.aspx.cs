using System;
using CrystalDecisions.Shared;
using Base.Page;
using BaseInfo.authUser;
using BaseInfo.User;

/// <summary>
/// 编写人:hesijian
/// 编写时间：2009年6月26日
/// </summary>
public partial class ReportM_RptSale_RptGetTotalCashes : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            InitTime();
        }
    }

    //初始化时间
    private void InitTime()
    {
        txtTotalDate.Text = DateTime.Now.ToShortDateString();
    
    }

    //水晶报表数据绑定
    private void BindData()
    {
        ParameterFields Fields = new ParameterFields();
        ParameterField[] Field = new ParameterField[16];
        ParameterDiscreteValue[] DiscreteValue = new ParameterDiscreteValue[16];
        ParameterRangeValue RangeValue = new ParameterRangeValue();

        Field[0] = new ParameterField();
        Field[0].Name = "REXMallTitle";
        DiscreteValue[0] = new ParameterDiscreteValue();
        DiscreteValue[0].Value = Session["MallTitle"].ToString();
        Field[0].CurrentValues.Add(DiscreteValue[0]);

        Field[1] = new ParameterField();
        Field[1].Name = "REXTitle";
        DiscreteValue[1] = new ParameterDiscreteValue();
        DiscreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_GetTotalCashes");
        Field[1].CurrentValues.Add(DiscreteValue[1]);
    
        //收银员号
        Field[2] = new ParameterField();
        Field[2].Name = "REXTPUserID";
        DiscreteValue[2] = new ParameterDiscreteValue();
        DiscreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_UserID");
        Field[2].CurrentValues.Add(DiscreteValue[2]);

        //收银员名称
        Field[3] = new ParameterField();
        Field[3].Name = "REXTPUserName";
        DiscreteValue[3] = new ParameterDiscreteValue();
        DiscreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_UserName");
        Field[3].CurrentValues.Add(DiscreteValue[3]);


        //收款时间
        Field[4] = new ParameterField();
        Field[4].Name = "REXGetCashTime";
        DiscreteValue[4] = new ParameterDiscreteValue();
        DiscreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_GetCashTime");
        Field[4].CurrentValues.Add(DiscreteValue[4]);


        //交易时间
        Field[5] = new ParameterField();
        Field[5].Name = "REXTradeTime";
        DiscreteValue[5] = new ParameterDiscreteValue();
        DiscreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_TransTime");
        Field[5].CurrentValues.Add(DiscreteValue[5]);


        //POS机号
        Field[6] = new ParameterField();
        Field[6].Name = "REXPOSID";
        DiscreteValue[6] = new ParameterDiscreteValue();
        DiscreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PosID");
        Field[6].CurrentValues.Add(DiscreteValue[6]);


        //付款方式
        Field[7] = new ParameterField();
        Field[7].Name = "REXMediaMDesc";
        DiscreteValue[7] = new ParameterDiscreteValue();
        DiscreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_MediaMDesc");
        Field[7].CurrentValues.Add(DiscreteValue[7]);


        //状态
        Field[8] = new ParameterField();
        Field[8].Name = "REXMediaStatus";
        DiscreteValue[8] = new ParameterDiscreteValue();
        DiscreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_MediaStatus");
        Field[8].CurrentValues.Add(DiscreteValue[8]);


        //应收金额
        Field[9] = new ParameterField();
        Field[9].Name = "REXLocalAmt";
        DiscreteValue[9] = new ParameterDiscreteValue();
        DiscreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_LocalAmt");
        Field[9].CurrentValues.Add(DiscreteValue[9]);


        //已付金额
        Field[10] = new ParameterField();
        Field[10].Name = "REXPayAmt";
        DiscreteValue[10] = new ParameterDiscreteValue();
        DiscreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvPaidAmt");
        Field[10].CurrentValues.Add(DiscreteValue[10]);


        //差额
        Field[11] = new ParameterField();
        Field[11].Name = "REXDiffCash";
        DiscreteValue[11] = new ParameterDiscreteValue();
        DiscreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_DiffCash");
        Field[11].CurrentValues.Add(DiscreteValue[11]);

        //查询日期
        Field[12] = new ParameterField();
        Field[12].Name = "REXQueryDate";
        DiscreteValue[12] = new ParameterDiscreteValue();
        DiscreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Sdate");
        Field[12].CurrentValues.Add(DiscreteValue[12]);

        //日期值
        Field[13] = new ParameterField();
        Field[13].Name = "REXDate";
        DiscreteValue[13] = new ParameterDiscreteValue();
        DiscreteValue[13].Value = txtTotalDate.Text.Trim();
        Field[13].CurrentValues.Add(DiscreteValue[13]);

        //汇总表
        Field[14] = new ParameterField();
        Field[14].Name = "REXRptTotal";
        DiscreteValue[14] = new ParameterDiscreteValue();
        DiscreteValue[14].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_TotalReport");
        Field[14].CurrentValues.Add(DiscreteValue[14]);

        //明细表
        Field[15] = new ParameterField();
        Field[15].Name = "REXRptDetail";
        DiscreteValue[15] = new ParameterDiscreteValue();
        DiscreteValue[15].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_DetailReport");
        Field[15].CurrentValues.Add(DiscreteValue[15]);
       

        foreach (ParameterField pf in Field)
        {
            Fields.Add(pf);
        
        }

        string addstr1 = "";
        string addstr2 = "";

        //条件查询
        if (txtCasherCode.Text.Trim() != "")
        {
            addstr1 = addstr1 + " And UserID = '" + txtCasherCode.Text + "'";
            addstr2 = addstr2 + " And UserID = '" + txtCasherCode.Text + "'";
        }

        if (txtTotalDate.Text.Trim() != "")
        {
            addstr1 = addstr1 + " And TransHeader.bizDate = '" + txtTotalDate.Text + "'";
            addstr2 = addstr2 + " And BizDate = '" + txtTotalDate.Text + "'";
        }

        string strAuth = "";
        string AddStatusN=" And TransMedia.PaymentStatus = 'N'";
        string AddStatusY=" And TransMedia.PaymentStatus = 'Y'";
        string sqlDatail = "";
        string SqlTotal = "";

        //权限设置
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            strAuth += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        }

        //汇总报表查询
        if (txtTotalRpt.Checked)
        {
            if (Rdo1.Checked)
            {
                SqlTotal = @"SELECT userID,TPUsrNm,mediaMDesc,localAmt,PayAmt,BizDate From (
                                SELECT TransHeader.userID,TpUsr.TPUsrNm,
                                       MediaM.mediaMDesc, 
                                       CASE mediaType WHEN 'T' THEN localAmt WHEN 'C' THEN 0 - localAmt ELSE 0 END AS localAmt,
		                               0 as PayAmt,TransHeader.BizDate
                                FROM TransMedia 
                                INNER JOIN TransHeader ON (TransMedia.transID = TransHeader.transID) 
		                        INNER JOIN TpUsr ON (TransHeader.UserId = TpUsr.TpusrId)
                                INNER JOIN MediaM ON (TransMedia.MediaMCd = MediaM.mediaMNo)
                                WHERE  TransHeader.training = 'N' AND TransMedia.PaymentStatus = 'N'" + addstr1 + strAuth + @"
 

UNION ALL 

                            Select userID,TPUsrNm,RTRIM(mediaCd) AS mediaMDesc,localAmt,PayAmt, BizDate
	                        From CasherPayment 
                            INNER JOIN TpUsr ON (CasherPayment.UserId = TpUsr.TpusrId)
                            Where 1=1 " + addstr2 + strAuth + @") As a";
                Session["paraFil"] = Fields;
                Session["sql"] = SqlTotal;
                Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptGetTotalCashes1.rpt";

            }
            if (Rdo2.Checked)
            {
                SqlTotal = @"SELECT TransHeader.userID,TpUsr.TPUsrNm,MediaM.mediaMDesc,
                                    CASE mediaType WHEN 'T' THEN localAmt WHEN 'C' THEN 0 - localAmt ELSE 0 END AS localAmt,0 as PayAmt,TransHeader.bizDate
                             FROM TransMedia
                             INNER JOIN TransHeader ON (TransMedia.transID = TransHeader.transID) 
		                     INNER JOIN TpUsr ON (TransHeader.UserId = TpUsr.TpusrId)
                             INNER JOIN MediaM ON (TransMedia.MediaMCd = MediaM.mediaMNo)
                             WHERE  TransHeader.training = 'N' AND TransMedia.PaymentStatus = 'N'" + addstr1 + strAuth;

                Session["paraFil"] = Fields;
                Session["sql"] = SqlTotal;
                Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptGetTotalCashes1.rpt";
            }
            if (Rdo3.Checked)
            {
                SqlTotal = @"Select userID,TPUsrNm,RTRIM(mediaCd) AS mediaMDesc,localAmt,PayAmt,BizDate
	                         From CasherPayment 
                             INNER JOIN TpUsr ON (CasherPayment.UserId = TpUsr.TpusrId)
                             Where 1=1 " + addstr2 + strAuth;

                Session["paraFil"] = Fields;
                Session["sql"] = SqlTotal;
                Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptGetTotalCashes1.rpt";
            }
        
        }

        //明细报表查询
        if (txtDetailRpt.Checked)
        {
            if (Rdo1.Checked)
            {
                sqlDatail = @"SELECT TransHeader.posID,TransHeader.userID,TPUsrNm,MediaM.mediaMDesc,
                              CASE mediaType WHEN 'T' THEN localAmt WHEN 'C' THEN 0 - localAmt ELSE 0 END AS localAmt,TransHeader.TransDate,TransMedia.PaymentStatus
                              FROM TransMedia 
                              INNER JOIN TransHeader ON (TransMedia.transID = TransHeader.transID)
		                      INNER JOIN TpUsr ON (TransHeader.UserId = TpUsr.TpusrId)
                              INNER JOIN MediaM ON (TransMedia.MediaMCd = MediaM.mediaMNo)
                              WHERE TransHeader.training = 'N'" + addstr1 + strAuth + @"
                              ORDER BY TransHeader.TransDate";

                Session["paraFil"] = Fields;
                Session["sql"] = sqlDatail;
                Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptGetTotalCashes2.rpt";
                
            }
            if (Rdo2.Checked)
            {
                sqlDatail = @"SELECT TransHeader.posID,TransHeader.userID,TPUsrNm,MediaM.mediaMDesc,
                              CASE mediaType WHEN 'T' THEN localAmt WHEN 'C' THEN 0 - localAmt ELSE 0 END AS localAmt,TransHeader.TransDate,TransMedia.PaymentStatus
                              FROM TransMedia 
                              INNER JOIN TransHeader ON (TransMedia.transID = TransHeader.transID)
		                      INNER JOIN TpUsr ON (TransHeader.UserId = TpUsr.TpusrId)
                              INNER JOIN MediaM ON (TransMedia.MediaMCd = MediaM.mediaMNo)
                              WHERE TransHeader.training = 'N' And TransMedia.PaymentStatus = 'N'" + addstr1 + strAuth + @"
                              ORDER BY TransHeader.TransDate";


                Session["paraFil"] = Fields;
                Session["sql"] = sqlDatail;
                Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptGetTotalCashes2.rpt";
            
            }
            if (Rdo3.Checked)
            {
                sqlDatail = @"SELECT TransHeader.posID,TransHeader.userID,TPUsrNm,MediaM.mediaMDesc,
                              CASE mediaType WHEN 'T' THEN localAmt WHEN 'C' THEN 0 - localAmt ELSE 0 END AS localAmt,TransHeader.TransDate,TransMedia.PaymentStatus
                              FROM TransMedia 
                              INNER JOIN TransHeader ON (TransMedia.transID = TransHeader.transID)
		                      INNER JOIN TpUsr ON (TransHeader.UserId = TpUsr.TpusrId)
                              INNER JOIN MediaM ON (TransMedia.MediaMCd = MediaM.mediaMNo)
                              WHERE TransHeader.training = 'N' And TransMedia.PaymentStatus = 'Y'" + addstr1 + strAuth + @"
                              ORDER BY TransHeader.TransDate";


                Session["paraFil"] = Fields;
                Session["sql"] = sqlDatail;
                Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptGetTotalCashes2.rpt";
            }
        
        }

    }


    //清除页面
    private void ClearPage()
    {
        InitTime();
        txtCasherCode.Text = "";
        txtTotalRpt.Checked = true;
        txtDetailRpt.Checked = false;
        Rdo1.Checked = true;
        Rdo2.Checked = false;
        Rdo3.Checked = false;
    }


    //查询操作
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }

    //撤销操作
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ClearPage();
    }
}
