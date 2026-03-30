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
using Base.DB;
using Base;
using Lease.PayIn;
using BaseInfo.User;
using Base.Page;
using Lease.ConShop;

public partial class Lease_PayIn_PayInput : BasePage
{
    public string baseInfo;  //基本信息
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDropPayInType();
            BindDropPayInDataSource();

            //输入控制
            txtPayInAmt.Attributes.Add("onkeydown", "textleave()");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblPayInPut");
            txtShopCode.Attributes.Add("onclick", "ShowTree()");
        }

    }
    private void clearTxtbox()
    {
        dropContractID.Items.Clear();
        dropShopID.Items.Clear();

        //txtCustCode.Text = "";
        txtCustName.Text = "";
        txtPayInAmt.Text = "";

        txtPayInEndDate.Text = "";
        txtPayInPeriod.Text = "";
        txtPayInStartDate.Text = "";
        //txtShopCode.Text = "";

    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if ((txtCustCode.Text != "") || (txtShopCode.Text != ""))
        {
            clearTxtbox();
            BaseBO baseBo = new BaseBO();
            string sql = "select A.CustName,B.ContractID,B.ContractCode,C.ShopID,C.ShopCode from Customer A,Contract B,ConShop C" +
                            " where A.CustID = B.CustID and B.ContractID = C.ContractID";
            if (txtCustCode.Text.Trim() != "")
            {
                sql = sql + " and A.CustCode = '" + txtCustCode.Text + "'";
            }
            //if (txtShopCode.Text.Trim() != "")
            //{
            //    sql = sql + " and C.ShopCode = '" + txtShopCode.Text + "'";
            //}
            if (allvalue.Value != "")
            {
                sql = sql + " and C.ShopID = '" + allvalue.Value + "'";
            }
            DataSet ds = new DataSet();
            ds=baseBo.QueryDataSet(sql);
            if (ds.Tables[0].Rows.Count >= 1)
            {
                txtCustName.Text = ds.Tables[0].Rows[0]["CustName"].ToString();
                txtCustName.Enabled = false;

                BindDropCustAndShop(ds);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "msg_NotFindData") + "'", true);
            }
            ds.Clear();   //清除内存中数据，保证下次查询准备
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
        }
    }

    private void BindDropCustAndShop(DataSet myDS)
    {
        int count = myDS.Tables[0].Rows.Count;
        for (int i = 0; i < count; i++)
        {
            //绑定合同号


            dropContractID.Items.Add(new ListItem(myDS.Tables[0].Rows[i]["ContractCode"].ToString(), myDS.Tables[0].Rows[i]["ContractID"].ToString()));
            //绑定商铺号


            dropShopID.Items.Add(new ListItem(myDS.Tables[0].Rows[i]["ShopCode"].ToString(), myDS.Tables[0].Rows[i]["ShopID"].ToString()));
        }
    }

    /// <summary>
    /// 绑定代收款类型


    /// </summary>
    private void BindDropPayInType()
    {
        dropPayInType.Items.Clear();
        //int[] payInType = Invoice.InvoiceH.PayIn.GetPayInType();
        int[] payInType = Lease.PayIn.PayIn.GetPayInType();
        int s = payInType.Length;
        for (int i = 0; i < s; i++)
        {
            dropPayInType.Items.Add(new ListItem((String)GetGlobalResourceObject("parameter",PayIn.GetPayInTypeDesc(payInType[i])),payInType[i].ToString()));
        }
    }

    /// <summary>
    /// 绑定数据来源
    /// </summary>
    private void BindDropPayInDataSource()
    {
        dropPayInDataSource.Items.Clear();
        int[] payInDataSource = PayIn.GetPayInDataSource();
        int s = payInDataSource.Length;
        for (int i = 0; i < s; i++)
        {
            dropPayInDataSource.Items.Add(new ListItem((String)GetGlobalResourceObject("parameter",PayIn.GetPayInDataSourceDesc(payInDataSource[i])), payInDataSource[i].ToString()));
        }
        dropPayInDataSource.SelectedIndex = 1;
    }
    private bool txtboxIsNull()
    {
        bool blStatus=true;
        if(txtPayInPeriod.Text.Trim()  =="")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
            blStatus= false;
        }
        if (txtPayInStartDate.Text.Trim() == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
            blStatus = false;
        }
        if (txtPayInEndDate.Text.Trim() == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
            blStatus = false;
        }
        if (txtPayInAmt.Text.Trim()  == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
            blStatus = false;
        }
        return blStatus;
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (txtboxIsNull() == true)
        {
            BaseBO baseBo = new BaseBO();
            Invoice.InvoiceH.PayIn payIn = new Invoice.InvoiceH.PayIn();
            payIn.PayInID = BaseApp.GetPayInID();
            payIn.ShopID = Convert.ToInt32(dropShopID.SelectedValue);
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            payIn.CreateUserID = objSessionUser.UserID;
            payIn.ModifyUserID = objSessionUser.UserID;
            payIn.OprRoleID = objSessionUser.RoleID;
            payIn.OprDeptID = objSessionUser.DeptID;
            payIn.PayInCode = Convert.ToString(payIn.PayInID);        // PayInCode=PayInID
            payIn.PayInPeriod = Convert.ToDateTime(txtPayInPeriod.Text).AddDays(-Convert.ToDateTime(txtPayInPeriod.Text).Day + 1);
            payIn.PayInType = Convert.ToInt32(dropPayInType.SelectedValue);
            payIn.PayInStartDate = Convert.ToDateTime(txtPayInStartDate.Text);
            payIn.PayInEndDate = Convert.ToDateTime(txtPayInEndDate.Text);
            payIn.PayInDate = Convert.ToDateTime(txtPayInStartDate.Text);
            payIn.PaidAmt = Convert.ToDecimal(txtPayInAmt.Text);
            payIn.PayInStatus = PayIn.PAYINSTATRS_NOINV;
            payIn.PayInDataSource = Convert.ToInt32(dropPayInDataSource.SelectedValue);
            payIn.CreateTime = DateTime.Now;
            payIn.ModifyTime = DateTime.Now;

            ////获取用户机构
            //baseBo.WhereClause = "UserID = " + payIn.CreateUserID + " and RoleID = " + payIn.OprRoleID;
            //Resultset rs = baseBo.Query(new UserRole());
            //UserRole userRole = rs.Dequeue() as UserRole;
            //payIn.OprDeptID = userRole.DeptID;

            //baseBo.WhereClause = "";
            if (baseBo.Insert(payIn) != -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
                // ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
            }
            SetControls();
        }
    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        SetControls();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
    }

    private void SetControls()
    {
        txtCustCode.Text = "";
        txtCustName.Text = "";
        txtPayInAmt.Text = "";

        txtPayInEndDate.Text = "";
        txtPayInPeriod.Text = "";
        txtPayInStartDate.Text = "";
        txtShopCode.Text = "";
        allvalue.Value = "";

        dropContractID.Items.Clear();
        dropShopID.Items.Clear();
        BindDropPayInType();
        BindDropPayInDataSource();
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        DataSet ds = ConShopPO.GetConShopByID(Convert.ToInt32(allvalue.Value));
        if (ds.Tables[0].Rows.Count == 1)
        {
            this.txtShopCode.Text = ds.Tables[0].Rows[0]["ShopName"].ToString();
        }
    }
}
