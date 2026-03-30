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

using WorkFlow.WrkFlw;
using WorkFlow;
using WorkFlow.Uiltil;

using Base.Biz;
using Base;
using Lease.PotCustLicense;
using Base.Page;
using BaseInfo.User;
using Base.DB;
using BaseInfo.Dept;

public partial class RentableArea_Building_MiInfoVindicate : BasePage
{
    public static int MI_INFO_VINDICATE_ADD = 1;
    public static int MI_INFO_VINDICATE_UPDATE = 2;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();
            baseBO.WhereClause = "DeptID = 100";
            rs = baseBO.Query(new MiInfoVindicate());
            if (rs.Count == 1)
            {
                MiInfoVindicate miInfo = rs.Dequeue() as MiInfoVindicate;

                txtMallName.Text = miInfo.DeptName;
                txtOfficeAddr.Text = miInfo.OfficeAddr;
                txtOfficeAddr2.Text = miInfo.OfficeAddr2;
                txtOfficeAddr3.Text = miInfo.OfficeAddr3;
                txtPostAddr.Text = miInfo.PostAddr;
                txtPostAddr2.Text = miInfo.PostAddr2;
                txtPostAddr3.Text = miInfo.PostAddr3;
                txtPostCode.Text = miInfo.PostCode;

                txtOfficeTel.Text = miInfo.OfficeTel;
                txtTel.Text = miInfo.Tel;
                txtPropertytel.Text = miInfo.Propertytel;

                txtLegalRep.Text = miInfo.LegalRep;
                txtLegalRepTitle.Text = miInfo.LegalRepTitle;
                txtRegCap.Text = miInfo.RegCap.ToString();
                txtRegAddr.Text = miInfo.RegAddr;

                txtRegCode.Text = miInfo.RegCode;
                txtTaxCode.Text = miInfo.TaxCode;
                txtBankName.Text = miInfo.BankName;
                txtBankAcct.Text = miInfo.BankAcct;
                ViewState["Flag"] = MI_INFO_VINDICATE_UPDATE;
            }
            else if (rs.Count == 0)
            {
                ViewState["Flag"] = MI_INFO_VINDICATE_ADD;
            }
        }
    }
    //protected void butCancel_Click(object sender, EventArgs e)
    //{
    //    txtOfficeAddr.Text = "";
    //    txtPostAddr.Text = "";
    //    txtPostCode.Text = "";
    //    txtWeb.Text = "";
    //    txtEMail.Text = "";

    //    txtOfficeTel.Text = "";
    //    txtTel.Text = "";
    //    txtPropertytel.Text = "";

    //    txtLegalRep.Text = "";
    //    txtLegalRepTitle.Text = "";
    //    txtRegCap.Text = "";
    //    txtRegAddr.Text = "";

    //    txtRegCode.Text = "";
    //    txtTaxCode.Text = "";
    //    txtBankName.Text = "";
    //    txtBankAcct.Text = "";
    //}
    protected void btnSave_Click(object sender, EventArgs e)
    {
        MiInfoVindicate miInfo = new MiInfoVindicate();

        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];

        miInfo.DeptId = 100;
        miInfo.CreateUserID = objSessionUser.CreateUserID;
        miInfo.CeateTime = DateTime.Now;
        miInfo.ModifyTime = DateTime.Now;
        miInfo.ModifyUserID = objSessionUser.ModifyUserID;
        miInfo.OprDeptID = objSessionUser.OprDeptID;
        miInfo.OprRoleID = objSessionUser.OprRoleID;

        miInfo.DeptName = txtMallName.Text.Trim();
        miInfo.OfficeAddr = txtOfficeAddr.Text;
        miInfo.OfficeAddr2 = txtOfficeAddr2.Text;
        miInfo.OfficeAddr3 = txtOfficeAddr3.Text;
        miInfo.PostAddr = txtPostAddr.Text;
        miInfo.PostAddr2 = txtPostAddr2.Text;
        miInfo.PostAddr3 = txtPostAddr3.Text;
        miInfo.PostCode = txtPostCode.Text;

        miInfo.OfficeTel = txtOfficeTel.Text;
        miInfo.Tel = txtTel.Text;
        miInfo.Propertytel = txtPropertytel.Text;

        miInfo.LegalRep = txtLegalRep.Text;
        miInfo.LegalRepTitle = txtLegalRepTitle.Text;
        miInfo.RegCap = Convert.ToDecimal(txtRegCap.Text);
        miInfo.RegAddr = txtRegAddr.Text;

        miInfo.RegCode = txtRegCode.Text;
        miInfo.TaxCode = txtTaxCode.Text;
        miInfo.BankName = txtBankName.Text;
        miInfo.BankAcct = txtBankAcct.Text;

        BaseBO baseBO = new BaseBO();
        if (Convert.ToInt32(ViewState["Flag"]) == MI_INFO_VINDICATE_ADD)
        {
            if (baseBO.Insert(miInfo) == -1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                return;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            }
        }
        else if (Convert.ToInt32(ViewState["Flag"]) == MI_INFO_VINDICATE_UPDATE)
        {
            baseBO.WhereClause = "DeptID = 100";

            if (baseBO.Update(miInfo) == -1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                return;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            }
        
        }

    }
}
