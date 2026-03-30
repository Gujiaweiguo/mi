using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.Biz;
using Base.DB;
using Base;
using Lease.ConShop;
using Base.Page;
using System.Drawing;
using BaseInfo.User;
using BaseInfo.authUser;

public partial class Lease_Shop_ShopHdw : BasePage
{
    public string baseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDrop();
            BindGV();
            btnEdit.Enabled = false;
            txtShopCode.Attributes.Add("onclick", "ShowTree()");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_lblShopHdwModi");
        }
        btnAdd.Attributes.Add("onclick","return CheckData()");
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        ShopHdw shopHdw = new ShopHdw();
        BaseBO basebo = new BaseBO();
        DataSet ds = new DataSet();
        ds = basebo.QueryDataSet("select * from shophdw where hdwcode='"+this.txtHdwCode.Text+"'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "LeaseShopHdw_lblHdwCode") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "。'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtHdwCode.select()", true);
        }
        else
        {
            GetHdwInfo(shopHdw);
            shopHdw.HdwID = BaseApp.GetHdwID();
            shopHdw.ShopID = Convert.ToInt32(allvalue.Value);
            int result = ShopHdwPO.InsertShopHdw(shopHdw);
            if (result == 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                ClearControlValue();

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
            BindGV();
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (txtHdwCode.Text.ToString() != "")
        {
            BaseBO baseBO = new BaseBO();
            DataSet ds = new DataSet();
            ds = baseBO.QueryDataSet("select * from shophdw where hdwcode='"+this.txtHdwCode.Text+"'");
            if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0]["hdwcode"].ToString() == ViewState["editLog"].ToString())
            {
                if (Convert.ToInt32(ViewState["hdwID"]) != null)
                {
                    ShopHdw shopHdw = new ShopHdw();
                    GetHdwInfo(shopHdw);
                    shopHdw.HdwID = Convert.ToInt32(ViewState["hdwID"]);
                    shopHdw.ShopID = Convert.ToInt32(ViewState["shopID"]);
                    int result = ShopHdwPO.UpdateShopHdwByID(shopHdw);
                    if (result == 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "'", true);
                        ClearControlValue();
                        btnEdit.Enabled = false;
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                    }
                }
                btnAdd.Enabled = true;
                BindGV();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "LeaseShopHdw_lblHdwCode") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "。'", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtHdwCode.select()", true);
                BindGV();
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "LeaseShopHdw_lblHdwCode") + "不能为空。'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtHdwCode.select()", true);
            BindGV();
        }
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        ClearControlValue();
        btnEdit.Enabled = false;
        btnAdd.Enabled = true;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
        BindGV();
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        //lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        BindGV();
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        //lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        BindGV();
    }
    protected void gvChargeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        int shopHdwID = Convert.ToInt32(gvChargeType.SelectedRow.Cells[0].Text);
        ViewState["hdwID"] = shopHdwID;
        Resultset rs = ShopHdwPO.GetShopHdwByID(shopHdwID);
        if (rs.Count > 0)
        {
            ShopHdw shopHdw = rs.Dequeue() as ShopHdw;
            dropHdwTypeID.SelectedValue = shopHdw.HdwTypeID.ToString();
            txtHdwCode.Text = shopHdw.HdwCode;
            txtHdwName.Text = shopHdw.HdwName;
            txtHdwQty.Text = shopHdw.HdwQty.ToString();
            txtHdwStd.Text = shopHdw.HdwStd;
            txtHdwUnit.Text = shopHdw.HdwUnit;
            txtNote.Text = shopHdw.Note;
            dropHdwCond.SelectedValue = shopHdw.HdwCond.ToString();
            dropOwner.SelectedValue = shopHdw.Owner.ToString();
            ViewState["shopID"] = shopHdw.ShopID;
            ViewState["editLog"] = txtHdwCode.Text;

            DataSet ds = ConShopPO.GetConShopByID(shopHdw.ShopID);
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtShopCode.Text = ds.Tables[0].Rows[0]["ShopCode"].ToString() + "(" + ds.Tables[0].Rows[0]["ShopName"].ToString() + ")";
            }
            else
            {
                txtShopCode.Text = "";
            }
            btnEdit.Enabled = true;
            btnAdd.Enabled = false;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
        BindGV();
    }
    protected void gvChargeType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string gIntro = "";
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells.Count > 1)
            {
                if (e.Row.Cells[1].Text == "&nbsp;")
                {
                    e.Row.Cells[8].Text = "";
                }
            }
        }
    }

    private void GetHdwInfo(ShopHdw shopHdw)
    {
        shopHdw.HdwTypeID = Convert.ToInt32(dropHdwTypeID.SelectedValue);
        
        shopHdw.HdwCode = txtHdwCode.Text;
        shopHdw.HdwName = txtHdwName.Text;
        shopHdw.HdwQty = Convert.ToInt32(txtHdwQty.Text);
        shopHdw.HdwUnit = txtHdwUnit.Text;
        shopHdw.HdwStd = txtHdwStd.Text;
        shopHdw.HdwCond = Convert.ToInt32(dropHdwCond.SelectedValue);
        shopHdw.Owner = Convert.ToInt32(dropOwner.SelectedValue);
        shopHdw.Note = txtNote.Text;
    }

    private void BindDrop()
    {
        //绑定硬件状况
        dropHdwCond.Items.Clear();
        int[] hdwCond = ShopHdw.GetHdwCond();
        int s = hdwCond.Length;
        for (int i = 0; i < s; i++)
        {
            dropHdwCond.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ShopHdw.GetHdwCondDesc(hdwCond[i])), hdwCond[i].ToString()));
        }

        //资产拥有者

        dropOwner.Items.Clear();
        int[] owner = ShopHdw.GetOwner();
        int l = owner.Length;
        for (int n = 0; n < l; n++)
        {
            dropOwner.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ShopHdw.GetOwnerDesc(owner[n])), owner[n].ToString()));
        }

        //硬件类型
        dropHdwTypeID.Items.Clear();
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "HdwTypeStatus = " + HdwType.HDWTYPESTATUS_EFFECTIVE;
        Resultset rs = baseBO.Query(new HdwType());
        foreach (HdwType hdwType in rs)
        {
            dropHdwTypeID.Items.Add(new ListItem(hdwType.HdwTypeName.ToString(), hdwType.HdwTypeID.ToString()));
        }
    }

    private void BindGV()
    {
        BaseBO baseBo = new BaseBO();

        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            baseBo.WhereClause = " EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
        }
        
        DataTable dt = baseBo.QueryDataSet(new ShopHdw()).Tables[0];

        int count = dt.Rows.Count;

        dt.Columns.Add("HdwCondName");
        dt.Columns.Add("OwnerName");
        dt.Columns.Add("ShopCode");
        for (int j = 0; j < count; j++)
        {
            dt.Rows[j]["HdwCondName"] = (String)GetGlobalResourceObject("Parameter", ShopHdw.GetHdwCondDesc(Convert.ToInt32(dt.Rows[j]["HdwCond"])));
            dt.Rows[j]["OwnerName"] = (String)GetGlobalResourceObject("Parameter", ShopHdw.GetOwnerDesc(Convert.ToInt32(dt.Rows[j]["Owner"])));
            dt.Rows[j]["ShopCode"] = (String)GetGlobalResourceObject("Parameter", ShopHdw.GetOwnerDesc(Convert.ToInt32(dt.Rows[j]["Owner"])));
            DataSet ds = ConShopPO.GetConShopByID(Convert.ToInt32(dt.Rows[j]["ShopID"]));
            if (ds.Tables[0].Rows.Count == 1)
            {
                dt.Rows[j]["ShopCode"] = ds.Tables[0].Rows[0]["ShopCode"].ToString();
            }
        }

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvChargeType.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvChargeType.DataSource = pds;
            gvChargeType.DataBind();
        }
        else
        {
            this.gvChargeType.DataSource = pds;
            this.gvChargeType.DataBind();
            spareRow = gvChargeType.Rows.Count;
            for (int i = 0; i < gvChargeType.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvChargeType.DataSource = pds;
            gvChargeType.DataBind();
        }

    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        DataSet ds = ConShopPO.GetConShopByID(Convert.ToInt32(allvalue.Value));
        if (ds.Tables[0].Rows.Count == 1)
        {
            ViewState["shopID"] = ds.Tables[0].Rows[0]["ShopID"].ToString();
            txtShopCode.Text = ds.Tables[0].Rows[0]["ShopCode"].ToString() + "(" + ds.Tables[0].Rows[0]["ShopName"].ToString() + ")";
        }
        BindGV();
    }

    private void ClearControlValue()
    {
        txtHdwCode.Text = "";
        txtHdwName.Text = "";
        txtHdwQty.Text = "";
        txtHdwStd.Text = "";
        txtHdwUnit.Text = "";
        txtNote.Text = "";
        txtShopCode.Text = "";
        BindDrop();
    }
    protected void gvChargeType_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView theGrid = sender as GridView;
        int newPageIndex = 0;
        if (-2 == e.NewPageIndex)
        {
            TextBox txtNewPageIndex = null;
            GridViewRow pagerRow = theGrid.BottomPagerRow;
            if (null != pagerRow)
            {
                txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;
            }
            if (null != txtNewPageIndex)
            {
                newPageIndex = int.Parse(txtNewPageIndex.Text) - 1;
            }
        }
        else
        { newPageIndex = e.NewPageIndex; }
        newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
        newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;
        theGrid.PageIndex = newPageIndex;
        foreach (GridViewRow grv in this.gvChargeType.Rows)
        {
            grv.BackColor = Color.White;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
        this.BindGV();
    }
}
