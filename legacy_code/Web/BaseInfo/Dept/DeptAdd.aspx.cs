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
using BaseInfo.User;
using BaseInfo.Dept;
using Base.Page;
public partial class DeptAdd :BasePage
{
    private int deptID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            deptID = Convert.ToInt32(Request.QueryString["id"].ToString());
            this.txtPDeptID.Text = Request.QueryString["DeptName"].ToString();
            this.txtDeptLevel.Text = Request.QueryString["deptLevel"];
            int[] status = Dept.GetDeptType();
            for (int i = 0; i < status.Length; i++)
            {
                ddlstDeptType.Items.Add(new ListItem(Dept.GetDeptTypeDesc(status[i]), status[i].ToString()));
            }

            int[] statusIndepBalance = Dept.GetIndepBalanceStatus();
            for (int i = 0; i < statusIndepBalance.Length; i++)
            {
                cmbIndepBalance.Items.Add(new ListItem(Dept.GetIndepBalanceStatusDesc(statusIndepBalance[i]), statusIndepBalance[i].ToString()));
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        
        Dept objDept = new Dept();
        DeptAuth objDeptAuth = new DeptAuth();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int result = 0;
        /**
         * --初始化添加数据--
         */
        objDept.CreateUserId = objSessionUser.UserID;
        objDept.CreateTime = DateTime.Now;

        objDept.DeptID = BaseApp.GetDeptID();
        objDept.DeptCode = txtDeptCode.Text.Trim();
        objDept.DeptName = txtDeptName.Text.Trim();
        objDept.PDeptID = Convert.ToInt32(deptID);
        objDept.DeptLevel = Convert.ToInt32(txtDeptLevel.Text); 
        objDept.DeptType = Convert.ToInt32(ddlstDeptType.SelectedValue);
        objDept.City = ddlstCity.SelectedItem.Text;
        objDept.RegAddr = txtRegAddr.Text.Trim();
        objDept.OfficeAddr = txtOfficeAddr.Text.Trim();
        objDept.PostAddr = txtPostAddr.Text.Trim();
        objDept.PostCode = TtxtPostCode.Text.Trim();
        objDept.Tel = txtTel.Text.Trim();
        objDept.OfficeTel =txtOfficeTel.Text.Trim();
        objDept.Fax = txtFax.Text.Trim();
        objDept.IndepBalance = Convert.ToInt32(cmbIndepBalance.SelectedValue);           //测试用值
        objDept.DeptStatus = Convert.ToInt32(ddlstDeptType.Text);

        objDeptAuth.DeptAuthID = BaseApp.GetDeptAuthID();
        objDeptAuth.DeptID = objDept.DeptID;
        objDeptAuth.ConcessionAuth = txtConcessionAuth.Text.Trim().ToString();
        objDeptAuth.ContractAuth = txtContractAuth.Text.Trim().ToString();
        objDeptAuth.TradeAuth = txtTradeAuth.Text.Trim().ToString();
        objDeptAuth.FeeAuth = txtFeeAuth.Text.Trim().ToString();
        objDeptAuth.OtherAuth = txtOtherAuth.Text.Trim().ToString();
        objDeptAuth.DeptAuthName = "Oasis";
        result = new BaseBO().Insert(objDept);

        if (result != -1 )
        {
            Response.Write("<script language=javascript>alert('操作成功!!');</script>");
        }
        else
        {
            Response.Write("<script language=javascript>alert('操作失败!!');</script>");
        }
        
        result = new BaseBO().Insert(objDeptAuth);
    }
}
