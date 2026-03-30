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
using Lease.PotCustLicense;
using Base.Page;
using BaseInfo.User;
using Base.DB;
using RentableArea;
using WorkFlow.Uiltil;
using WorkFlow.WrkFlw;
using WorkFlow;
using Lease.Customer;
using Lease.Contract;
using Lease.ConShop;

public partial class CustPalaver : System.Web.UI.Page
{
    private int numCount = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        butConsent.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        butConsent.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
        butOverrule.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnBlankOuting.gif) no-repeat left top';this.style.fontWeight='bold';");
        butOverrule.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnBlankOut.gif) no-repeat left top';this.style.fontWeight='normal';");
        btnBack.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnbacking.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnBack.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnback.gif) no-repeat left top';this.style.fontWeight='normal';");
        btnNext.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnnexting.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnNext.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnnext.gif) no-repeat left top';this.style.fontWeight='normal';");
        butOverrule.Attributes.Add("onclick", "return OverruleValidator(form1)");
        if (!IsPostBack)
        {

            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();
            //int[] status = PotShop.GetShopTypeStatus();
            //for (int i = 0; i < status.Length; i++)
            //{
            //    cmbShopType.Items.Add(new ListItem(PotShop.GetShopTypeStatusDesc(status[i]), status[i].ToString()));
            //}
            rs = baseBO.Query(new Area());
            cmbArea.Text = "";
            foreach (Area area in rs)
            {
                cmbArea.Items.Add(new ListItem(area.AreaName, area.AreaID.ToString()));
            }

            int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
            int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);
            int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);
            WrkFlwEntity objWrkFlwEntity = WrkFlwApp.GetWrkFlwEntity(wrkFlwID, nodeID, sequence);

            ViewState["CustID"] = Request.QueryString["VoucherID"];
            baseBO.WhereClause = "a.CustID=" + ViewState["CustID"];

            rs = baseBO.Query(new CustPalaverQuery());
            if (rs.Count == 1)
            {
                CustPalaverQuery custPalaver = rs.Dequeue() as CustPalaverQuery;
                txtCustCode.Text = custPalaver.CustCode;
                txtCustName.Text = custPalaver.CustName;
                txtCustShortName.Text = custPalaver.CustShortName;
                txtMainBrand.Text = custPalaver.MainBrand;
                txtNote.Text = objWrkFlwEntity.PreVoucherMemo;
                txtPotShopName.Text = custPalaver.PotShopName;
                txtRentalPrice.Text = custPalaver.RentalPrice.ToString();
                txtRentArea.Text = custPalaver.RentArea.ToString();
                txtShopEndDate.Text = custPalaver.ShopEndDate.ToShortDateString();
                txtShopStartDate.Text = custPalaver.ShopStartDate.ToShortDateString();
                cmbArea.SelectedValue = custPalaver.AreaID.ToString();
                cmbShopType.SelectedValue = custPalaver.ShopTypeID.ToString();
                txtCommOper.Text = custPalaver.UserName;
            }
            page();
            bool canSmtToMgr = WrkFlwApp.CanSmtToMgr(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), Convert.ToInt32(Request.QueryString["Sequence"]));
            if (canSmtToMgr)
            {
                //butLeadPalaver.Visible = true;
                //butLeadPalaverT.Visible = true;
            }
        }
    }
    protected void butOverrule_Click(object sender, EventArgs e)
    {
        Overrule();
    }
    protected void butConsent_Click(object sender, EventArgs e)
    {
        Consent();
    }
    protected void butLeadPalaver_Click(object sender, EventArgs e)
    {
        ////提交上级审批
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];

        int deptID = objSessionUser.DeptID;
        int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
        int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);
        int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);

        String voucherHints = txtCustShortName.Text;
        String voucherMemo = txtVoucherMemo.Text.Trim();
        int voucherID = Convert.ToInt32(ViewState["CustID"]);
        int operatorID = objSessionUser.UserID;
        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, operatorID);
        WrkFlwApp.SmtToMgr(wrkFlwID, nodeID, sequence, vInfo);

        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('提交成功');", true);
    }
    protected void TextChanged(object sender, EventArgs e)
    {

    }
    protected void page()
    {
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        if (Convert.ToString(ViewState["CustID"]) == "")
        {
            baseBO.WhereClause = "CustID=" + 0;
        }
        else
        {
            baseBO.WhereClause = "CustID=" + Convert.ToInt32(ViewState["CustID"].ToString());
        }


        DataTable dt = baseBO.QueryDataSet(new CustPalaverInfo()).Tables[0];
        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GrdCustPalaverInfo.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdCustPalaverInfo.DataSource = pds;
            GrdCustPalaverInfo.DataBind();
        }
        else
        {
            pds.AllowPaging = true;
            pds.PageSize = 6;
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
            this.GrdCustPalaverInfo.DataSource = pds;
            this.GrdCustPalaverInfo.DataBind();
            spareRow = GrdCustPalaverInfo.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdCustPalaverInfo.DataSource = pds;
            GrdCustPalaverInfo.DataBind();
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

    private void Overrule()
    {
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
        int voucherID = Convert.ToInt32(ViewState["CustID"]);
        int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);
        int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);


        WrkFlwEntity objWrkFlwEntity = WrkFlwApp.GetWrkFlwEntity(wrkFlwID, nodeID, sequence);

        if ((WrkFlwEntity.NODE_STATUS_REJECT_PENDING == objWrkFlwEntity.NodeStatus) || (WrkFlwEntity.NODE_STATUS_NORMAL_PENDING == objWrkFlwEntity.NodeStatus))
        {

            string showmessage = "window.open('Disprove.aspx?" + "WrkFlwID=" + wrkFlwID + "&VoucherID=" + voucherID + "&Sequence=" + sequence + "&NodeID=" + nodeID + "&VoucherMemo=" + txtVoucherMemo.Text.Trim() + "','"+ hidInitiativeOverrule.Value +"',height=200,width=200,status=1,toolbar=0,menubar=0);";
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", showmessage, true);
            page();
        }
        else if (WrkFlwEntity.NODE_STATUS_MGR_PENDING == objWrkFlwEntity.NodeStatus)
        {
            String voucherHints = txtCustShortName.Text.Trim();
            String voucherMemo = txtVoucherMemo.Text.Trim();
            voucherID = Convert.ToInt32(ViewState["CustID"]);
            int operatorID = objSessionUser.UserID;
            int deptID = objSessionUser.DeptID;
            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, operatorID);
            WrkFlwApp.MgrRejectVoucher(wrkFlwID, nodeID, sequence, vInfo);
            //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('添加成功')", true);
        }
        
    }
    private void Consent()
    {
        /*工作流结转和生成正式商户资料*/
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
        int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);
        int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);
        int voucherID =0; //Convert.ToInt32(ViewState["CustID"]);
        int shopType = 0;
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        PotCustomer potcustomer = new PotCustomer();
        Customer customer = new Customer();
        SignUpContract signUpContract = new SignUpContract();
        PotShop potShop = new PotShop();
        TransmitConShop transmitConShop = new TransmitConShop();
        String voucherHints = txtCustShortName.Text;
        String voucherMemo = txtVoucherMemo.Text.Trim();
        int operatorID = objSessionUser.UserID;
        int deptID = objSessionUser.DeptID;
        int i = 0;

        BaseTrans basetrans = new BaseTrans();
        basetrans.BeginTrans();
        if (ViewState["CustID"].ToString() == "")
        {
            return;
        }
        string sqlstr = "insert into  CustLicense select * from  PotCustLicense where custid=" + ViewState["CustID"];
        i = basetrans.ExecuteUpdate(sqlstr);
        if (i < 1)
        {
            basetrans.Rollback();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + hidCustLicense.Value + "'", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1);", true);
            return;
        }
        i = basetrans.ExecuteUpdate("insert into  CustContact select * from  PotCustContact where custid=" + ViewState["CustID"]);
        if (i < 1)
        {
            basetrans.Rollback();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + hidCustContact.Value + "'", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1);", true);
            return;
        }
        baseBO.WhereClause = "custid=" + ViewState["CustID"];
        rs = baseBO.Query(potcustomer);
        if (rs.Count == 1)
        {
            potcustomer = rs.Dequeue() as PotCustomer;
            customer.CommOper = potcustomer.CommOper;
        }
        //basetrans.Insert(customer);

        i = basetrans.ExecuteUpdate("insert into  Customer select * from  PotCustomer where custid=" + ViewState["CustID"]);
        if (i < 1)
        {
            basetrans.Rollback();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + hidCustContact.Value + "'", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1);", true);
            return;
        }

        baseBO.WhereClause = "custid=" + ViewState["CustID"];
        rs = baseBO.Query(potShop);
        if (rs.Count == 1)
        {
            potShop = rs.Dequeue() as PotShop;
            signUpContract.ContractID = BaseApp.GetContractID();
            voucherID = signUpContract.ContractID;
            signUpContract.CustID = Convert.ToInt32(ViewState["CustID"]);
            signUpContract.ContractStatus = Contract.CONTRACTSTATUS_TYPE_FIRST;
            signUpContract.SigningMode = Contract.CONTRACTSTATUS_TYPE_N;
            signUpContract.ConStartDate = Convert.ToDateTime(txtShopStartDate.Text);
            signUpContract.ConEndDate = Convert.ToDateTime(txtShopEndDate.Text);
            signUpContract.ChargeStartDate = Convert.ToDateTime(txtShopStartDate.Text);
            signUpContract.NorentDays = 0;
            signUpContract.BizMode = potShop.BizMode;
            signUpContract.CommOper = customer.CommOper;
            shopType = potShop.BizMode;
            if (shopType == Contract.BIZ_MODE_LEASE)
            {
                signUpContract.ContractTypeID = Contract.BIZ_MODE_LEASE;
            }
            else if (shopType == Contract.BIZ_MODE_UNIT)
            {
                signUpContract.ContractTypeID = Contract.BIZ_MODE_UNIT;
            }
            transmitConShop.ShopID = potShop.PotShopID;
            transmitConShop.ContractID = signUpContract.ContractID;
            transmitConShop.ShopName = potShop.PotShopName;
            transmitConShop.ShopTypeID = potShop.ShopTypeID;
            transmitConShop.ShopStartDate = potShop.ShopStartDate;
            transmitConShop.RentArea = potShop.RentArea;
            transmitConShop.AreaID = potShop.AreaID;
            transmitConShop.ShopEndDate = potShop.ShopEndDate;
        }
        if (basetrans.Insert(signUpContract) < 1)
        {
            basetrans.Rollback();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + hidInsert.Value + "'", true);
            return;
        }
        if (basetrans.Insert(transmitConShop) < 1)
        {
            basetrans.Rollback();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + hidInsert.Value + "'", true);
            return;
        }
        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, operatorID);
        if (shopType == Contract.BIZ_MODE_LEASE)
        {
            WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, sequence, vInfo, basetrans);
            WrkFlwApp.CommitVoucher(104, 108, WrkFlwEntity.NODE_STATUS_NORMAL_PENDING, vInfo, basetrans);
        }
        else if (shopType == Contract.BIZ_MODE_UNIT)
        {
            WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, sequence, vInfo, basetrans);
            WrkFlwApp.CommitVoucher(106, 112, WrkFlwEntity.NODE_STATUS_NORMAL_PENDING, vInfo, basetrans);
        }
        basetrans.Commit();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + hidAdd.Value + "'", true);


        //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "chooseCard(0);", true);
        //Response.Redirect("~/WorkFlow/WrkFlwTree.aspx");
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        page();
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1);", true);
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        page();
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1);", true);
    }

    protected void GrdCustPalaverInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string gIntro="";
        if (e.Row.Cells[1].Text != "&nbsp;")
        {
            gIntro = e.Row.Cells[3].Text.ToString();
            e.Row.Cells[3].Text = SubStr(gIntro, 7);
        }
        else
        {
            e.Row.Cells[4].Text = "";
        }
    }
    protected void GrdCustPalaverInfo_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        CustPalaverInfo tempCustPalaver = new CustPalaverInfo();
        Resultset rs = new Resultset();
        baseBO.WhereClause = "PalaverID=" + GrdCustPalaverInfo.SelectedRow.Cells[0].Text;

        rs = baseBO.Query(tempCustPalaver);
        if (rs.Count == 1)
        {

            CustPalaverInfo custPalaver = rs.Dequeue() as CustPalaverInfo;
            txtPalaverContent.Text = custPalaver.PalaverContent;
            page();
        }
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "chooseCard(1);", true);
    }
}
