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
using Base.Page;
using Base.Sys;
using Base.XML;
using RentableArea;
using Base.DB;
using Base.Biz;
using BaseInfo.User;


public partial class VisualAnalysis_PlanUnitShopTypeColor : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUser sessionUsers = (SessionUser)Session["UserAccessInfo"];
        int userid = sessionUsers.UserID;
        string user = sessionUsers.UserID.ToString();
        Random ro = new Random();
        int iResult = ro.Next();

        if (Request.QueryString["FloorID"] != null)
        {
            string FloorName = "";

            string floorID = Request.QueryString["FloorID"].ToString();

            BaseBO baseBo = new BaseBO();

            Resultset rs2 = new Resultset();
            baseBo.WhereClause = "";
            baseBo.WhereClause = "floorid='" + floorID + "'";
            rs2 = baseBo.Query(new Floors());
            if (rs2.Count == 1)
            {
                Floors floor = rs2.Dequeue() as Floors;
                FloorName = floor.FloorName;
            }

            XmlExecutor xe4 = new XmlExecutor();
            ShopXML sx = new ShopXML();
            ShopXMLInfo4 sxi = new ShopXMLInfo4();
            xe4.BuildXml(sx, sxi, "", user);

            XmlExecutor xe5 = new XmlExecutor();
            ColorLumpXML CLX = new ColorLumpXML();
            ColorLumpXMLInfo CLXI = new ColorLumpXMLInfo(0);
            xe5.BuildXml(CLX, CLXI, "", user);



            Response.Redirect("../Disktop.aspx?FloorID=" + floorID + "&FloorName=" + FloorName + "&nocache=" + iResult);
        }
        else
        {
            XmlExecutor xe4 = new XmlExecutor();
            ShopXML sx = new ShopXML();
            ShopXMLInfo4 sxi = new ShopXMLInfo4();
            xe4.BuildXml(sx, sxi, "", user);

            XmlExecutor xe5 = new XmlExecutor();
            ColorLumpXML CLX = new ColorLumpXML();
            ColorLumpXMLInfo CLXI = new ColorLumpXMLInfo(0);
            xe5.BuildXml(CLX, CLXI, "", user);

            Response.Redirect("../Disktop.aspx?nocache=" + iResult);
        }

    }

}
