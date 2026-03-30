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
using CrystalDecisions.CrystalReports.Engine;

using Base.Biz;
using Base.DB;
using Base;
using Base.Page;
using Lease;
using Lease.Customer;
using Lease.Contract;
using Lease.CustLicense;
using Lease.PotBargain;
using RentableArea;
using Base.Util;
using Lease.PotCust;
using BaseInfo.User;
using BaseInfo.authUser;

public partial class ReportM_RptMember_RptMemberAlterForBonus : BasePage
{
    public string baseInfo;  //基本信息
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {


            baseInfo = "会员积分异常情况查询";
            txtStartDate.Text = DateTime.Now.ToShortDateString();
            txtEndDate.Text=DateTime.Now.ToShortDateString();
            txtBonusAmt.Attributes.Add("onkeydown", "textleave()");
            txtBonusLead.Attributes.Add("onkeydown", "textleave()");
            txtTradeFrequency.Attributes.Add("onkeydown", "textleave()");
        }

    }



    //private String GetStrNull(String s)
    //{
    //    return s.Trim() == "" ? "-32766" : s;
    //}



    protected void btnOK_Click(object sender, EventArgs e)
    {
        string mex = "信息不能为空";
        string ex = "信息不能为0";

        if (txtBonusAmt.Text !=""   && txtBonusLead.Text !="" && txtTradeFrequency.Text !="")
            {
                if (Convert.ToInt32(txtBonusAmt.Text)  > 0 && Convert.ToDouble(txtBonusLead.Text) > 0 && Convert.ToInt32(txtTradeFrequency.Text) > 0)
                {
                    Session["subReportSql"] = "";
                    Session["subRpt"] = "";
                    BindData();
                    this.Response.Redirect("../ReportShow.aspx"); ;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('" + ex + "')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('" + mex + "')", true);
            }
        
    }

   
    //private String GetdateNull(String s)
    //{
    //    return s.Trim() == "" ? "2007-12-25" : s;
    //}

    private void BindData()
    {
        //(String)GetGlobalResourceObject("ReportInfo", "Rpt_CustName")
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[14];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[14];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXMembID";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblAssociatorNum");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXMembCardID";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblCardID");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXSalutation";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = "称呼"; 
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXMemberName";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = "会员姓名"; 
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXMobilPhone";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblMobileTel");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXEmail";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value =  (String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorEmail");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXShopCode";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value =  (String)GetGlobalResourceObject("BaseInfo", "Lease_lblShopCode");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXShopName";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value =(String)GetGlobalResourceObject("BaseInfo", "PotShop_lblPotShopName");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXReceiptID";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value =  (String)GetGlobalResourceObject("BaseInfo", "Rpt_TransId");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXTransDT";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value =  (String)GetGlobalResourceObject("BaseInfo", "Rpt_TransTime");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXNetAmt";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_PayInAmt");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXBonusAmt";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = "本次积分"; 
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXTitle";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = "会员积分异常情况查询";
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "REXMallTitle";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = Session["MallTitle"].ToString();
        paraField[13].CurrentValues.Add(discreteValue[13]);




        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }




        string strwhere1 = "";
        string strwhere2 = "";
        string strwhere3 = "";
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            strwhere3 = "";
                //" AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                //        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                //        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                //        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        }
        if (txtmembCardID.Text != "")
        {
            strwhere1 = " AND purhist.membCardID like  '%" + txtmembCardID.Text.Trim() + "%' ";
        }
        if (txtmemberName.Text != "")
        {
            strwhere2 = " AND member.memberName like  '%" + txtmemberName.Text.Trim() + "%' ";
        }

                string str_sql = "SELECT " +
                            "member.MembID,purhist.MembCardID,member.Salutation,member.MemberName" +
                            ",member.MobilPhone,member.Email,conShop.ShopCode,conShop.ShopName" +
                            ",ReceiptID,purhist.TransDT,NetAmt,BonusAmt" +
                            "  FROM purhist" +
                            "       INNER JOIN member ON (purhist.membID = member.membID)" +
                            "       INNER JOIN conShop ON (conshop.shopID = purhist.shopID)" +
                            "    WHERE 1=1 ";
                            //and (ABS(BonusAmt) > '" + txtBonusAmt.Text + "' " +
                            //"and (bonusAmt / CASE WHEN netamt > 0 THEN netamt END) > '" + txtBonusLead.Text + "'" +
                            ////"and" +
                            ////"(purhist.MembID in (select membID" +
                            ////"                FROM purhist" +
                            ////"               WHERE    purhist.transDT BETWEEN '" + txtStartDate.Text + "' AND '" + txtEndDate.Text + "'" +
                            ////"            GROUP BY membID,shopID HAVING count(*) > '" + txtTradeFrequency.Text + "' and datediff(d,min(transDt),max(transDT)) < 30" +
                            ////"))" +
                            //"and (purhist.MembID IN (SELECT membID" +
                            //"                FROM purhist" +
                            //"               WHERE   purhist.transDT BETWEEN '" + txtStartDate.Text + "' AND '" + txtEndDate.Text + "'" +
                            //"            GROUP BY membID,convert(char(10),transDT,120) HAVING count(*) >'" + txtTradeFrequency.Text + "')" +
                            //"   )" +
                            //"   )" +
                            //"AND purhist.transDT BETWEEN '" + txtStartDate.Text + "' AND '" + txtEndDate.Text + "'" +strwhere1 + strwhere2 + strwhere3 +
                            //"  ORDER BY member.membID,purhist.transDT";

                if (cbBonus.Checked == true)
                {
                    str_sql += " AND (ABS(BonusAmt) > '" + txtBonusAmt.Text + "')";
                }
                if (cbDouble.Checked == true)
                {
                    str_sql += " AND (bonusAmt / CASE WHEN netamt > 0 THEN netamt END ) > '" + txtBonusLead.Text + "'";
                }
                if (cbF.Checked == true)
                {
                    str_sql += @" AND (purhist.MembID in (select membID
                                             FROM purhist
                                             WHERE    purhist.transDT BETWEEN'" + txtStartDate.Text + "' AND '" + txtEndDate.Text + "'" +
                                            " GROUP BY membID,shopID HAVING count(*) > '" + txtTradeFrequency.Text + "'" +
                                 " ))";
                }
                str_sql += " AND purhist.transDT BETWEEN '" + txtStartDate.Text + "' AND '" + txtEndDate.Text + "'" + strwhere1 + strwhere2 + strwhere3 +
                            " ORDER BY member.membID,purhist.transDT";


                Session["paraFil"] = paraFields;
                Session["sql"] = str_sql;
                Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Member\\RptMemberAlterForBonus.rpt";

            
        

    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        this.txtmembCardID.Text = "";
        this.txtmemberName.Text = "";
        this.txtBonusAmt.Text = "10000";
        this.txtBonusLead.Text = "5";
        this.txtTradeFrequency.Text = "15";
        txtStartDate.Text = DateTime.Now.ToShortDateString();
        txtEndDate.Text = DateTime.Now.ToShortDateString();
    }

}

