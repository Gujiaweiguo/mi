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
using BaseInfo.Dept;

public partial class RentableArea_Building_MiInfoVindicateQuery : BasePage
{
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
            }
        }
    }
}
