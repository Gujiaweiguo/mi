using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Base.Biz;
using BaseInfo.Dept;
using Base.DB;
using Base.Page;
public partial class DeptUpdate : BasePage
{
    private int addorUpdateLevel = 0;
    public string baseInfo;
    private void Page_Load(object sender, System.EventArgs e)
    {
        Session["DeptID"] = deptid.Value;
        selectdeptid.Value = Convert.ToString(Session["DeptID"]);

        string jsdept = "";

        BaseBO baseBo = new BaseBO();
        string strSql = @"SELECT 
		CreateUserId,CreateTime,DeptID,DeptCode,
		DeptName,DeptLevel,PDeptID,DeptType,City,
		RegAddr,OfficeAddr,PostAddr,PostCode,Tel,
		OfficeTel,Fax,DeptStatus,IndepBalance,OrderID
FROM 
		Dept
 Group  By PDeptID,CreateUserId,CreateTime,DeptID,DeptCode,
		DeptName,DeptLevel,PDeptID,DeptType,City,
		RegAddr,OfficeAddr,PostAddr,PostCode,Tel,
		OfficeTel,Fax,DeptStatus,IndepBalance,OrderID
 ORDER BY Pdeptid,isnull(orderid,0) ";
        Dept objDept = new Dept();
        objDept.SetQuerySql(strSql);
        Resultset rs = baseBo.Query(objDept);
        if (rs.Count > 0)
        {

            foreach (Dept dept in rs)
            {
                if (dept.DeptStatus == Dept.DEPTSTATUS_INVALID)
                {
                    jsdept += dept.DeptID + "|" + dept.PDeptID + "|" + dept.DeptName + "|" + "../../App_Themes/nlstree/img/node3.gif" + "^";
                }
                else
                {
                    jsdept += dept.DeptID + "|" + dept.PDeptID + "|" + dept.DeptName + "|" + "" + "^";
                }
            }
            depttxt.Value = jsdept;

        }
        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "a1", "styletabbar_atv()", true);
        if (!IsPostBack)
        {
            //int[] status = Dept.GetDeptType();
            //for (int i = 0; i < status.Length; i++)
            //{
            //    ddlstDeptType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Dept.GetDeptTypeDesc(status[i])), status[i].ToString()));
            //}
            //绑定部门，只绑定下级部门 edit by lcp 
            BaseBO objBaseBo = new BaseBO();
            BaseInfo.BaseCommon.BindDropDownList("select DeptType,DeptTypeName from DeptType", "DeptType", "DeptTypeName", this.ddlstDeptType);
            //

            int[] statusIndepBalance = Dept.GetIndepBalanceStatus();
            for (int i = 0; i < statusIndepBalance.Length; i++)
            {
                cmbIndepBalance.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Dept.GetIndepBalanceStatusDesc(statusIndepBalance[i])), statusIndepBalance[i].ToString()));
            }

            int[] deptStatus = Dept.GetDeptStatus();
            for (int i = 0; i < deptStatus.Length; i++)
            {
                cmbDeptStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Dept.GetDeptStatusDesc(deptStatus[i])), deptStatus[i].ToString()));
            }

            //rbtnAllShop.Attributes.Add("onclick", "allShopChange()");
            //rbtnShop.Attributes.Add("onclick", "ShopChange()");

            //rbtnAllTreaty.Attributes.Add("onclick", "AllTreaty()");
            //rbtnTreaty.Attributes.Add("onclick", "Treaty()");

            //rbtnAllVocation.Attributes.Add("onclick", "AllVocation()");
            //rbtnVocation.Attributes.Add("onclick", "Vocation()");

            //rbtnNoRrestrict.Attributes.Add("onclick", "NoRrestrict()");
            //rbtnRrestrict.Attributes.Add("onclick", "Rrestrict()");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }


    }

    protected void deptid_ValueChanged(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        DeptBO deptBO = new DeptBO();
        DeptInfo objDeptInfo = new DeptInfo();
        string deptId = "";
        TreeNode Node = new TreeNode();



        deptId = deptid.Value;
        //Response.Redirect("DeptAdd.aspx?id=" + deptId.Substring(deptId.IndexOf(",") + 1, deptId.Length - deptId.IndexOf(",") - 1) + "&deptLevel=" + intDeptLevel+"&DeptName="+deptIDName);

        ViewState["DeptID"] = deptId;

        Session["DeptID"] = deptid.Value;

        deptBO.WhereClause = "DeptID =" + deptId;
        Resultset rs = deptBO.Query(objDeptInfo);
        if (rs.Count == 1)
        {
            objDeptInfo = rs.Dequeue() as DeptInfo;
            txtDeptCode.Text = objDeptInfo.DeptCode;
            txtDeptName.Text = objDeptInfo.DeptName;
            treetext.Text = objDeptInfo.DeptName;
            ddlstDeptType.SelectedValue = objDeptInfo.DeptType.ToString();
            cmbDeptStatus.SelectedValue = objDeptInfo.DeptStatus.ToString();
            txtDeptLevel.Text = Convert.ToString(objDeptInfo.DeptLevel);
            this.txtOrderID.Text = objDeptInfo.OrderID.ToString().Trim();
            //if (objDeptInfo.ConcessionAuth.ToString() == "" || objDeptInfo.ConcessionAuth.ToString() == null)
            //{
            //    rbtnAllShop.Checked = true;
            //    rbtnShop.Checked = false;
            //    txtConcessionAuth.Text = "";
            //}
            //else
            //{
            //    rbtnAllShop.Checked = false;
            //    rbtnShop.Checked = true;
            //    txtConcessionAuth.Text = objDeptInfo.ConcessionAuth;
            //}
            //if (objDeptInfo.ContractAuth.ToString() == "" || objDeptInfo.ContractAuth.ToString() == null)
            //{
            //    rbtnAllTreaty.Checked = true;
            //    rbtnTreaty.Checked = false;
            //    txtContractAuth.Text = "";
            //}
            //else
            //{
            //    rbtnAllTreaty.Checked = false;
            //    rbtnTreaty.Checked = true;
            //    txtContractAuth.Text = objDeptInfo.ContractAuth;
            //}
            //if (objDeptInfo.TradeAuth.ToString() == "" || objDeptInfo.TradeAuth.ToString() == null)
            //{
            //    rbtnAllVocation.Checked = true;
            //    rbtnVocation.Checked = false;
            //    txtTradeAuth.Text = "";
            //}
            //else
            //{
            //    rbtnAllVocation.Checked = false;
            //    rbtnVocation.Checked = true;
            //    txtTradeAuth.Text = objDeptInfo.TradeAuth;
            //}
            //if (objDeptInfo.OtherAuth.ToString() == "" || objDeptInfo.OtherAuth.ToString() == null)
            //{
            //    rbtnNoRrestrict.Checked = true;
            //    rbtnRrestrict.Checked = false;
            //    txtOtherAuth.Text = "";
            //}
            //else
            //{
            //    rbtnNoRrestrict.Checked = false;
            //    rbtnRrestrict.Checked = true;
            //    txtOtherAuth.Text = objDeptInfo.OtherAuth;
            //}
            cmbIndepBalance.SelectedValue = objDeptInfo.IndepBalance.ToString();
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }

    protected void rbtnAllShop_CheckedChanged(object sender, EventArgs e)
    {

    }

    private void textlock()
    {
        txtDeptCode.ReadOnly = true;

        txtDeptName.ReadOnly = true;

        //txtConcessionAuth.ReadOnly = true;
        //txtConcessionAuth.Text = "";

        //txtContractAuth.ReadOnly = true;
        //txtContractAuth.Text = "";

        //txtTradeAuth.ReadOnly = true;
        //txtTradeAuth.Text = "";

        //txtOtherAuth.ReadOnly = true;
        //txtOtherAuth.Text = "";

        txtDeptCode.Text = "";
        txtDeptName.Text = "";

        ddlstDeptType.Enabled = false;
        cmbIndepBalance.Enabled = false;
        cmbDeptStatus.Enabled = false;

        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = ''", true);
    }

    private void textopen()
    {
        txtDeptCode.ReadOnly = false;

        txtDeptName.ReadOnly = false;

        //txtConcessionAuth.ReadOnly = false;
        //txtConcessionAuth.Text = "";

        //txtContractAuth.ReadOnly = false;
        //txtContractAuth.Text = "";

        //txtTradeAuth.ReadOnly = false;
        //txtTradeAuth.Text = "";

        //txtOtherAuth.ReadOnly = false;
        //txtOtherAuth.Text = "";

        txtDeptCode.Text = "";
        txtDeptName.Text = "";

        ddlstDeptType.Enabled = true;
        cmbIndepBalance.Enabled = true;
        cmbDeptStatus.Enabled = true;
    }

}
