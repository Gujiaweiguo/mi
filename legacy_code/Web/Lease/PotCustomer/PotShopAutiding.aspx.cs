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
using Lease.PotCustLicense;
using Lease.Contract;
using Base.Biz;
using Base.DB;
using RentableArea;
using Base;
using WorkFlow.Uiltil;
using WorkFlow.WrkFlw;
using WorkFlow;
using BaseInfo.User;
using Shop.ShopType;
public partial class Lease_PotCustomer_PotShopAutiding : BasePage
{
    private static int SELECTED = 999;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();
            PotShop potShop = new PotShop();

            rs = baseBO.Query(new ShopType());
            cmbShopType.Text = "";
            foreach (ShopType shopType in rs)
            {
                cmbShopType.Items.Add(new ListItem(shopType.ShopTypeName, shopType.ShopTypeID.ToString()));
            }

            int[] bizModes = Contract.GetBizModes();
            for (int i = 0; i < bizModes.Length; i++)
            {
                cmbBizMode.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetBizModeDesc(bizModes[i])), bizModes[i].ToString()));
            }

            rs = baseBO.Query(new Area());
            cmbArea.Text = "";
            cmbArea.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "PotShop_Selected"), SELECTED.ToString()));

            foreach (Area area in rs)
            {
                cmbArea.Items.Add(new ListItem(area.AreaName, area.AreaID.ToString()));
            }

            try
            {
                ViewState["CustID"] = Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);
                baseBO.WhereClause = "a.CustID=" + ViewState["CustID"];
                rs = baseBO.Query(new PotCustomerInfo());
                if (rs.Count == 1)
                {
                    PotCustomerInfo potCustomerInfo = rs.Dequeue() as PotCustomerInfo;
                    txtCreateUserID.Text = potCustomerInfo.CustCode.ToString();
                    txtCustName.Text = potCustomerInfo.CustName;
                    txtCustShortName.Text = potCustomerInfo.CustShortName;
                    txtContactorName.Text = potCustomerInfo.ContactorName;
                    txtOfficeTel.Text = potCustomerInfo.OfficeTel;
                    txtMobileTel.Text = potCustomerInfo.MobileTel;
                    txtCommOper.Text = potCustomerInfo.UserName;
                    cmbBizMode.SelectedValue = potShop.BizMode.ToString();
                    ViewState["CustomerStatus"] = potCustomerInfo.CustomerStatus;

                }
                 
                baseBO.WhereClause = "CustID=" + Convert.ToInt32(ViewState["CustID"]);
                rs = baseBO.Query(potShop);
                if (rs.Count == 1)
                {
                    potShop = rs.Dequeue() as PotShop;
                    txtPotShopName.Text = potShop.PotShopName;
                    txtMainBrand.Text = potShop.MainBrand;
                    txtShopStartDate.Text = potShop.ShopStartDate.ToString("yyyy-MM-dd");
                    txtShopEndDate.Text = potShop.ShopEndDate.ToString("yyyy-MM-dd");
                    cmbArea.SelectedValue = potShop.AreaID.ToString();
                    txtRentalPrice.Text = potShop.RentalPrice.ToString();
                    txtRentArea.Text = potShop.RentArea.ToString();
                    txtNode.Text = potShop.Note;
                    cmbShopType.SelectedValue = potShop.ShopTypeID.ToString();
                    cmbBizMode.SelectedValue = potShop.BizMode.ToString();
                }
            }
            catch
            {

            }
        }
    }
    private void textClear()
    {
        txtPotShopName.Text = "";
        txtMainBrand.Text = "";
        txtShopStartDate.Text = "";
        txtShopEndDate.Text = "";
        cmbArea.SelectedIndex = 0;
        txtRentalPrice.Text = "";
        txtRentArea.Text = "";
        txtNode.Text = "";
    }
}
