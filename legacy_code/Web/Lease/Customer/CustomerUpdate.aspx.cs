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
using Base;
using Lease.CustLicense;
using Base.Page;
using BaseInfo.User;
using Base.DB;
using Lease.PotCust;
using Lease.Customer;
using Base.Page;
public partial class Lease_Customer_CustomerUpdate : BasePage
{
    Resultset rs = null;
    Resultset rs2 = null;
    private int numCount = 0;
    private int numCountCust = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnBack.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnbacking.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnBack.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnback.gif) no-repeat left top';this.style.fontWeight='normal';");
        btnNext.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnnexting.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnNext.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnnext.gif) no-repeat left top';this.style.fontWeight='normal';");
        btnQuery.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnHelping.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnQuery.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnHelp.gif) no-repeat left top';this.style.fontWeight='normal';");
        btnLBack.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnbacking.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnLBack.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnback.gif) no-repeat left top';this.style.fontWeight='normal';");
        btnLNext.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnnexting.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnLNext.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnnext.gif) no-repeat left top';this.style.fontWeight='normal';");

        btnAdd.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnAdd.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
        btnSave.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnSave.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
        btnEdit.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/btnEditing.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnEdit.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/btnEdit.gif) no-repeat left top';this.style.fontWeight='normal';");
        btnCancel.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCanceling.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnCancel.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCancel.gif) no-repeat left top';this.style.fontWeight='normal';");
        btnQuit.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCanceling.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnQuit.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCancel.gif) no-repeat left top';this.style.fontWeight='normal';");
        btnQuit.Attributes.Add("onclick", "return LicenseTextClear()");
        if (!IsPostBack)
        {
            /*商户类型*/
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "CustTypeStatus=" + CustType.CUST_TYPE_STATUS_VALID;
            rs = baseBO.Query(new CustType());
            foreach (CustType custtype in rs)
            {
                cmbCustType.Items.Add(new ListItem(custtype.CustTypeName, custtype.CustTypeID.ToString()));
                cmbCustTypeq.Items.Add(new ListItem(custtype.CustTypeName, custtype.CustTypeID.ToString()));
            }

            /*招商员选择列表*/
            baseBO.WhereClause = "UserStatus=" + UserInfo.USER_STATUS_VALID;
            baseBO.GroupBy = "a.userid,UserName,a.UserCode,WorkNo,OfficeTel,UserStatus";
            rs = baseBO.Query(new UserInfo());
            foreach (UserInfo user in rs)
            {
                cmbCommOper.Items.Add(new ListItem(user.UserName, user.UserID.ToString()));
            }

            /*证照类型*/
            int[] status2 = CustLicenseInfo.GetPotCustLicenseType();
            for (int i = 0; i < status2.Length; i++)
            {
                cmbLicenseType.Items.Add(new ListItem(CustLicenseInfo.GetPotCustLicenseTypeDesc(status2[i]), status2[i].ToString()));
            }
            page("a.CustID= 0");
            BindGridViewPotCustLicense("CustID=0");
        }
    }
    protected void Button5_Click(object sender, EventArgs e)
    {

    }

    public void BindGridViewPotCustLicense(string wherestr)
    {
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        baseBO.WhereClause = wherestr;
        DataTable dt = baseBO.QueryDataSet(new CustLicenseInfo()).Tables[0];
        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GrdCustLicense.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdCustLicense.DataSource = pds;
            GrdCustLicense.DataBind();
        }
        else
        {
            GrdCustLicense.EmptyDataText = "";
            pds.AllowPaging = true;
            pds.PageSize = 11;
            lblLTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
            pds.CurrentPageIndex = int.Parse(lblLCurrent.Text) - 1;
            if (pds.IsFirstPage)
            {
                btnLBack.Enabled = false;
                btnLNext.Enabled = true;
            }

            if (pds.IsLastPage)
            {
                btnLBack.Enabled = true;
                btnLNext.Enabled = false;
            }

            if (pds.IsFirstPage && pds.IsLastPage)
            {
                btnLBack.Enabled = false;
                btnLNext.Enabled = false;
            }

            if (!pds.IsLastPage && !pds.IsFirstPage)
            {
                btnLBack.Enabled = true;
                btnLNext.Enabled = true;
            }
            this.GrdCustLicense.DataSource = pds;
            this.GrdCustLicense.DataBind();
            spareRow = GrdCustLicense.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdCustLicense.DataSource = pds;
            GrdCustLicense.DataBind();
        }
    }
    public string SubStr(string sString, int nLeng)
    {
        if (sString.Length <= nLeng)
        {
            return sString;
        }
        string sNewStr = sString.Substring(0, nLeng);
        sNewStr = sNewStr + "...";
        return sNewStr;
    }
    public void CancelCustLicenseText()
    {
        txtLicenseID.Text = "";
        txtLicenseName.Text = "";
        txtLicenseBeginDate.Text = "";
        txtLicenseEndDate.Text = "";
        cmbLicenseType.SelectedValue = CustLicenseInfo.CUST_DEAL_IN_LICENCE.ToString();
    }
    protected void butSave_Click1(object sender, ImageClickEventArgs e)
    {
        Customer potCustomer = new Customer();
        potCustomer.CustCode = txtCustCode.Text;
        potCustomer.CustName = txtCustName.Text;
        potCustomer.CustShortName = txtCustShortName.Text;
        potCustomer.CustTypeID = Convert.ToInt32(cmbCustType.SelectedValue);
        potCustomer.LegalRep = txtLegalRep.Text;
        potCustomer.LegalRepTitle = txtLegalRepTitle.Text;
        potCustomer.RegCap = Convert.ToDecimal(txtRegCap.Text);
        potCustomer.RegAddr = txtRegAddr.Text;
        potCustomer.RegCode = txtRegCode.Text;
        potCustomer.TaxCode = txtTaxCode.Text;
        potCustomer.BankName = txtBankName.Text;
        potCustomer.BankAcct = txtBankAcct.Text;
        potCustomer.OfficeAddr = txtOfficeAddr.Text;
        potCustomer.PostAddr = txtPostAddr.Text;
        potCustomer.PostCode = txtPostCode.Text;
        potCustomer.WebUrl = txtWeb.Text;

        CustContact potCustContact = new CustContact();
        potCustContact.CustContactID = BaseApp.GetCustContactIDD();
        potCustContact.ContactMan = txtContactorName.Text;
        potCustContact.Title = txtTitle.Text;
        potCustContact.OfficeTel = txtOfficeTel.Text;
        potCustContact.MobileTel = txtMobileTel.Text;
        potCustContact.EMail = txtEMail.Text;
        potCustContact.CustID = potCustomer.CustID;
        //  potCustContact.OfficeAddr = txtOfficeAddr.Text;

        BaseTrans baseTrans = new BaseTrans();



        baseTrans.BeginTrans();
        baseTrans.WhereClause = "CustID=" + ViewState["custID"];
        int result = baseTrans.Update(potCustomer);
        if (result != -1)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('添加成功')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('添加失败')", true);
        }
        baseTrans.WhereClause = "CustID=" + ViewState["custID"];
        baseTrans.Update(potCustContact);

        baseTrans.Commit();

        txtCustCode.Text = "";
        txtCustName.Text = "";
        txtCustShortName.Text = "";
        //potCustomer.CustType = cmbCustType.SelectedIndex;
        txtLegalRep.Text = "";
        txtLegalRepTitle.Text = "";
        txtRegCap.Text = "";
        txtRegAddr.Text = "";
        txtRegCode.Text = "";
        txtTaxCode.Text = "";
        txtBankName.Text = "";
        txtBankAcct.Text = "";
        txtOfficeAddr.Text = "";
        txtPostAddr.Text = "";
        txtPostCode.Text = "";
        txtWeb.Text = "";
        txtContactorName.Text = "";
        txtTitle.Text = "";
        txtOfficeTel.Text = "";
        txtMobileTel.Text = "";
        txtEMail.Text = "";
    }
    protected void ImageButton4_Click(object sender, ImageClickEventArgs e)
    {
        CancelCustLicenseText();
    }

    protected void page(string wherestr)
    {
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        baseBO.WhereClause = wherestr;

        DataTable dt = baseBO.QueryDataSet(new CustomerInfo()).Tables[0];
        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GrdCust.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdCust.DataSource = pds;
            GrdCust.DataBind();
        }
        else
        {
            GrdCust.EmptyDataText = "";
            pds.AllowPaging = true;
            pds.PageSize = 10;
            lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
            pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;
            if (pds.IsFirstPage)
            {
                btnBack.Enabled = false;
                btnNext.Enabled = true;
            }

            if (pds.IsLastPage)
            {
                btnBack.Enabled = true;
                btnNext.Enabled = false;
            }

            if (pds.IsFirstPage && pds.IsLastPage)
            {
                btnBack.Enabled = false;
                btnNext.Enabled = false;
            }

            if (!pds.IsLastPage && !pds.IsFirstPage)
            {
                btnBack.Enabled = true;
                btnNext.Enabled = true;
            }

            this.GrdCust.DataSource = pds;
            this.GrdCust.DataBind();
            spareRow = GrdCust.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdCust.DataSource = pds;
            GrdCust.DataBind();
        }

    }
    protected void ibtnPrev_Click(object sender, ImageClickEventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        QueryData();
    }
    protected void ibtnNext_Click(object sender, ImageClickEventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        QueryData();
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        //查询
        QueryData();
        BindGridViewPotCustLicense("CustID=0");
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);
    }
    private void QueryData()
    {
        if (cmbCustTypeq.SelectedItem != null)
        {
            if (TextBox2.Text != "")
            {
                page("a.CustTypeID='" + cmbCustTypeq.SelectedValue + "'  and CustShortName='" + TextBox2.Text + "'");
            }
            else
            {
                page(" a.CustTypeID= '" + cmbCustTypeq.SelectedValue + "'");
            }
        }
        else
        {
            page("CustShortName='" + TextBox2.Text + "'");
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Customer potCustomer = new Customer();
        potCustomer.CustCode = txtCustCode.Text;
        potCustomer.CustName = txtCustName.Text;
        potCustomer.CustShortName = txtCustShortName.Text;
        potCustomer.CustTypeID = Convert.ToInt32(cmbCustType.SelectedValue);
        potCustomer.LegalRep = txtLegalRep.Text;
        potCustomer.LegalRepTitle = txtLegalRepTitle.Text;
        potCustomer.RegCap = Convert.ToDecimal(txtRegCap.Text);
        potCustomer.RegAddr = txtRegAddr.Text;
        potCustomer.RegCode = txtRegCode.Text;
        potCustomer.TaxCode = txtTaxCode.Text;
        potCustomer.BankName = txtBankName.Text;
        potCustomer.BankAcct = txtBankAcct.Text;
        potCustomer.OfficeAddr = txtOfficeAddr.Text;
        potCustomer.PostAddr = txtPostAddr.Text;
        potCustomer.PostCode = txtPostCode.Text;
        potCustomer.WebUrl = txtWeb.Text;
        potCustomer.CommOper = Convert.ToInt32(cmbCommOper.SelectedValue.ToString());

        CustContact potCustContact = new CustContact();
        potCustContact.CustContactID = BaseApp.GetCustContactIDD();
        potCustContact.ContactMan = txtContactorName.Text;
        potCustContact.Title = txtTitle.Text;
        potCustContact.OfficeTel = txtOfficeTel.Text;
        potCustContact.MobileTel = txtMobileTel.Text;
        potCustContact.EMail = txtEMail.Text;
        potCustContact.CustID = potCustomer.CustID;
        //  potCustContact.OfficeAddr = txtOfficeAddr.Text;

        BaseTrans baseTrans = new BaseTrans();



        baseTrans.BeginTrans();
        baseTrans.WhereClause = "CustID=" + ViewState["custID"];
        int result = baseTrans.Update(potCustomer);
        if (result != -1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "chooseCard", "chooseCard(0);", true);
            page("a.CustID= 0");
            BindGridViewPotCustLicense("CustID=0");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
            return;
        }
        baseTrans.WhereClause = "CustID=" + ViewState["custID"];
        baseTrans.Update(potCustContact);

        baseTrans.Commit();
        clearText();

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearText();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "chooseCard", "chooseCard(1);", true);
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        //添加
        if (ViewState["custID"].ToString() != "")
        {

            CustLicenseInfo potCustLicense = new CustLicenseInfo();
            potCustLicense.CustLicenseID = BaseApp.GetCustLicenseIDD();
            potCustLicense.CustLicenseCode = txtLicenseID.Text;
            potCustLicense.CustLicenseName = txtLicenseName.Text;
            potCustLicense.CustLicenseType = Convert.ToInt32(cmbLicenseType.SelectedValue);
            potCustLicense.CustLicenseStartDate = Convert.ToDateTime(txtLicenseBeginDate.Text);
            potCustLicense.CustLicenseEndDate = Convert.ToDateTime(txtLicenseEndDate.Text);
            potCustLicense.CustID = Convert.ToInt32(ViewState["custID"].ToString());

            BaseBO baseBo = new BaseBO();

            int result = baseBo.Insert(potCustLicense);
            if (result != -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(2)", true);
                return;
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidWrite.Value + "'", true);
        }

        btnAdd.Enabled = false;
        btnEdit.Enabled = true;
        BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
        CancelCustLicenseText();
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(2)", true);
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        //更新
        CustLicenseInfo potCustLicense = new CustLicenseInfo();
        potCustLicense.CustLicenseCode = txtLicenseID.Text;
        potCustLicense.CustLicenseName = txtLicenseName.Text;
        potCustLicense.CustLicenseType = Convert.ToInt32(cmbLicenseType.SelectedValue);
        potCustLicense.CustLicenseStartDate = DateTime.Now;
        potCustLicense.CustLicenseEndDate = DateTime.Now;

        BaseBO baseBo = new BaseBO();

        baseBo.WhereClause = "CustLicenseID=" + ViewState["custLicenseID"].ToString();

        int result = baseBo.Update(potCustLicense);
        if (result != -1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(2)", true);
            return;
        }
        BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
        btnEdit.Enabled = false;
        btnAdd.Enabled = true;
        CancelCustLicenseText();
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(2)", true);
    }
    protected void btnQuit_Click(object sender, EventArgs e)
    {
        //txtLicenseID.Text = "";
        //txtLicenseName.Text = "";
        //cmbLicenseType.SelectedValue = "1";
        //txtLicenseBeginDate.Text = "";
        //txtLicenseEndDate.Text = "";
        //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(2)", true);
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        QueryData();
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        QueryData();
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);
    }
    protected void btnLBack_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblLCurrent.Text) - 1);
        BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1)", true);
    }
    protected void btnLNext_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblLCurrent.Text) + 1);
        BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1)", true);
    }
    protected void GrdCust_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells[1].Text == "&nbsp;")
        {
            e.Row.Cells[7].Text = "";
        }
    }
    protected void GrdCust_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();

        baseBO.WhereClause = "a.CustID=" + GrdCust.SelectedRow.Cells[0].Text;
        ViewState["custID"] = GrdCust.SelectedRow.Cells[0].Text;
        rs = baseBO.Query(new CustomerQuery());
        if (rs != null)
        {
            CustomerQuery potCustomerQuery = rs.Dequeue() as CustomerQuery;
            txtCustCode.Text = potCustomerQuery.CustCode;
            txtCustName.Text = potCustomerQuery.CustName;
            txtCustShortName.Text = potCustomerQuery.CustShortName;
            cmbCustType.SelectedValue = potCustomerQuery.CustTypeID.ToString();
            txtLegalRep.Text = potCustomerQuery.LegalRep;
            txtLegalRepTitle.Text = potCustomerQuery.LegalRepTitle;
            txtRegCap.Text = potCustomerQuery.RegCap.ToString();
            txtRegAddr.Text = potCustomerQuery.RegAddr;
            txtRegCode.Text = potCustomerQuery.RegCode;
            txtTaxCode.Text = potCustomerQuery.TaxCode;
            txtBankName.Text = potCustomerQuery.BankName;
            txtBankAcct.Text = potCustomerQuery.BankAcct;
            txtOfficeAddr.Text = potCustomerQuery.OfficeAddr;
            txtPostAddr.Text = potCustomerQuery.PostAddr;
            txtPostCode.Text = potCustomerQuery.PostCode;
            txtWeb.Text = potCustomerQuery.WebURL;
            txtContactorName.Text = potCustomerQuery.ContactMan;
            txtTitle.Text = potCustomerQuery.Title;
            txtOfficeTel.Text = potCustomerQuery.OfficeTel;
            txtMobileTel.Text = potCustomerQuery.MobileTel;
            txtEMail.Text = potCustomerQuery.EMail;
            cmbCommOper.SelectedValue = potCustomerQuery.CommOper.ToString();
            BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
            QueryData();
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1)", true);
        }
    }
    protected void GrdCustLicense_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string gIntro = "";
        if (e.Row.Cells[1].Text != "&nbsp;")
        {
            gIntro = e.Row.Cells[1].Text.ToString();
            e.Row.Cells[1].Text = SubStr(gIntro, 5);
            gIntro = e.Row.Cells[2].Text.ToString();
            e.Row.Cells[2].Text = SubStr(gIntro, 10);
            gIntro = e.Row.Cells[3].Text.ToString();
            e.Row.Cells[3].Text = SubStr(gIntro, 10);
        }
        else
        {
            e.Row.Cells[4].Text = "";
        }
    }
    protected void GrdCustLicense_SelectedIndexChanged(object sender, EventArgs e)
    {
        //证照
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "CustLicenseID=" + GrdCustLicense.SelectedRow.Cells[0].Text;
        ViewState["custLicenseID"] = GrdCustLicense.SelectedRow.Cells[0].Text;
        rs = baseBO.Query(new CustLicenseInfo());

        CustLicenseInfo potCustLicenseInfo = rs.Dequeue() as CustLicenseInfo;

        txtLicenseID.Text = potCustLicenseInfo.CustLicenseCode;
        txtLicenseName.Text = potCustLicenseInfo.CustLicenseName;
        txtLicenseBeginDate.Text = potCustLicenseInfo.CustLicenseStartDate.ToString();
        txtLicenseEndDate.Text = potCustLicenseInfo.CustLicenseEndDate.ToString();
        cmbLicenseType.SelectedValue = potCustLicenseInfo.CustLicenseType.ToString();
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(2)", true);
        BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
        QueryData();
        btnEdit.Enabled = true;
        btnAdd.Enabled = false;
    }


    protected void clearText()
    {
        txtCustCode.Text = "";
        txtCustName.Text = "";
        txtCustShortName.Text = "";
        //potCustomer.CustType = cmbCustType.SelectedIndex;
        txtLegalRep.Text = "";
        txtLegalRepTitle.Text = "";
        txtRegCap.Text = "";
        txtRegAddr.Text = "";
        txtRegCode.Text = "";
        txtTaxCode.Text = "";
        txtBankName.Text = "";
        txtBankAcct.Text = "";
        txtOfficeAddr.Text = "";
        txtPostAddr.Text = "";
        txtPostCode.Text = "";
        txtWeb.Text = "";
        txtContactorName.Text = "";
        txtTitle.Text = "";
        txtOfficeTel.Text = "";
        txtMobileTel.Text = "";
        txtEMail.Text = "";
    }
}
