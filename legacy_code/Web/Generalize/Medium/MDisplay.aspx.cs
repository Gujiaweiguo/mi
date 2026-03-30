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
using Base.Page;
using Base;
using Generalize.Medium;
using BaseInfo.User;
using RentableArea;

public partial class Generalize_Medium_MDisplay : BasePage
{
    public string errorMes = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Request.Cookies["AnPMaster"].Values["AnPID"] == "")
            {
                ViewState["AnPID"] = -1;
            }
            else
            {

                ViewState["AnPID"] = Request.Cookies["AnPMaster"].Values["AnPID"];
            }

            /*绑定大楼*/
            BindBuilding();

            /*绑定经营区名称*/
            BindArea();

            BindGV();

            errorMes = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");
            txtEstCosts.Attributes.Add("onkeydown", "textleave()");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "PublicMessage", "Load()", true);
        }
    }
    protected void DDownListBuilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDownListBuilding.SelectedValue.ToString() != "-1")
        {
            DDownListFloors.Enabled = true;
            int builder = Convert.ToInt32(DDownListBuilding.SelectedValue);
            BindFollrs(builder);
        }
        else
        {
            DDownListFloors.Enabled = false;
            DDownListLocation.Enabled = false;
            BindFollrs(-1);
            BindLocation(-1);
        }
        BindGV();
    }
    protected void DDownListFloors_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDownListFloors.SelectedValue.ToString() != "-1")
        {
            DDownListLocation.Enabled = true;
            int floorID = Convert.ToInt32(DDownListFloors.SelectedValue);
            BindLocation(floorID);
        }
        else
        {
            DDownListLocation.Enabled = false;
            BindLocation(-1);
        }
        BindGV();
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        BindGV();
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        BindGV();
    }
    protected void gvMDisplay_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        MDisplay mDisplay = new MDisplay();

        ViewState["MDisplayID"] = gvMDisplay.SelectedRow.Cells[0].Text;

        baseBO.WhereClause = "MDisplayID = " + gvMDisplay.SelectedRow.Cells[0].Text;

        rs = baseBO.Query(mDisplay);

        if (rs.Count == 1)
        {
            mDisplay = rs.Dequeue() as MDisplay;
            txtDisplayNm.Text = mDisplay.DisplayNm;
            DDownListBuilding.SelectedValue = mDisplay.BuildingID.ToString();

            BindFollrs(mDisplay.BuildingID);

            DDownListFloors.SelectedValue = mDisplay.FloorID.ToString();

            BindLocation(mDisplay.FloorID);

            DDownListLocation.SelectedValue = mDisplay.LocationID.ToString();
            DDownListAreaName.SelectedValue = mDisplay.AreaID.ToString();
            txtLocationDesc.Text = mDisplay.LocationDesc;
            txtintention.Text = mDisplay.Intention;
            txtEstCosts.Text = mDisplay.EstCosts.ToString();
            txtStartDate.Text = mDisplay.StartDate.ToString("yyyy-MM-dd");
            txtEndDate.Text = mDisplay.EndDate.ToString("yyyy-MM-dd");
            txtMcompany.Text = mDisplay.Mcompany;
            txtDisplayDesc.Text = mDisplay.DisplayDesc;

            btnEdit.Enabled = true;
        }
        BindGV();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        MDisplay mDisplay = new MDisplay();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        mDisplay.MDisplayID = BaseApp.GetMDisplayID();
        mDisplay.AnPID = Convert.ToInt32(ViewState["AnPID"]);
        mDisplay.DisplayNm = txtDisplayNm.Text.Trim();
        mDisplay.BuildingID = Convert.ToInt32(DDownListBuilding.SelectedValue);
        mDisplay.FloorID = Convert.ToInt32(DDownListFloors.SelectedValue);
        mDisplay.LocationID = Convert.ToInt32(DDownListLocation.SelectedValue);
        mDisplay.AreaID = Convert.ToInt32(DDownListAreaName.SelectedValue);
        mDisplay.LocationDesc = txtLocationDesc.Text.Trim();
        mDisplay.Intention = txtintention.Text.Trim();
        mDisplay.EstCosts = Convert.ToDecimal(txtEstCosts.Text);
        mDisplay.StartDate = Convert.ToDateTime(txtStartDate.Text);
        mDisplay.EndDate = Convert.ToDateTime(txtEndDate.Text);
        mDisplay.Mcompany = txtMcompany.Text.Trim();
        mDisplay.DisplayDesc = txtDisplayDesc.Text.Trim();

        mDisplay.CreateUserID = sessionUser.UserID;
        mDisplay.OprDeptID = sessionUser.DeptID;
        mDisplay.OprRoleID = sessionUser.RoleID;

        if (baseBO.Insert(mDisplay) == 1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ClearText();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
        }
        BindGV();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        MDisplay mDisplay = new MDisplay();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        baseBO.WhereClause = "MDisplayID = " + ViewState["MDisplayID"];
        
        mDisplay.DisplayNm = txtDisplayNm.Text.Trim();
        mDisplay.BuildingID = Convert.ToInt32(DDownListBuilding.SelectedValue);
        mDisplay.FloorID = Convert.ToInt32(DDownListFloors.SelectedValue);
        mDisplay.LocationID = Convert.ToInt32(DDownListLocation.SelectedValue);
        mDisplay.AreaID = Convert.ToInt32(DDownListAreaName.SelectedValue);
        mDisplay.LocationDesc = txtLocationDesc.Text.Trim();
        mDisplay.Intention = txtintention.Text.Trim();
        mDisplay.EstCosts = Convert.ToDecimal(txtEstCosts.Text);
        mDisplay.StartDate = Convert.ToDateTime(txtStartDate.Text);
        mDisplay.EndDate = Convert.ToDateTime(txtEndDate.Text);
        mDisplay.Mcompany = txtMcompany.Text.Trim();
        mDisplay.DisplayDesc = txtDisplayDesc.Text.Trim();

        mDisplay.ModifyUserID = sessionUser.UserID;

        if (baseBO.Update(mDisplay) == 1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ClearText();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
        }
        BindGV();
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        ClearText();
        btnEdit.Enabled = false;
        BindGV();
    }

    #region 初始化DropDownList

    //绑定大楼
    protected void BindBuilding()
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();

        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        rs = baseBO.Query(new Building());
        DDownListBuilding.Items.Add(new ListItem(selected, "-1"));
        foreach (Building building in rs)
        {
            DDownListBuilding.Items.Add(new ListItem(building.BuildingName, building.BuildingID.ToString()));
        }
    }

    //绑定楼层名称
    protected void BindFollrs(int bulid)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();

        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        DDownListFloors.Items.Clear();

        baseBO.WhereClause = "BuildingID = " + bulid;
        rs = baseBO.Query(new Floors());
        DDownListFloors.Items.Add(new ListItem(selected, "-1"));
        foreach (Floors floors in rs)
        {
            DDownListFloors.Items.Add(new ListItem(floors.FloorName, floors.FloorID.ToString()));
        }
    }

    //绑定方位名称
    protected void BindLocation(int floor)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();

        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        DDownListLocation.Items.Clear();

        baseBO.WhereClause = "FloorID = " + floor;
        rs = baseBO.Query(new Location());
        DDownListLocation.Items.Add(new ListItem(selected, "-1"));
        foreach (Location loca in rs)
        {
            DDownListLocation.Items.Add(new ListItem(loca.LocationName, loca.LocationID.ToString()));
        }
    }

    /*绑定经营区名称*/
    protected void BindArea()
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();

        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");

        rs = baseBO.Query(new Area());
        DDownListAreaName.Items.Add(new ListItem(selected, "-1"));

        foreach (Area area in rs)
        {
            DDownListAreaName.Items.Add(new ListItem(area.AreaName, area.AreaID.ToString()));
        }
    }
    #endregion

    private void BindGV()
    {
        BaseBO baseBo = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        baseBo.WhereClause = "AnPID = " + ViewState["AnPID"];

        DataTable dt = baseBo.QueryDataSet(new MDisplay()).Tables[0];

        int count = dt.Rows.Count;

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvMDisplay.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvMDisplay.DataSource = pds;
            gvMDisplay.DataBind();
        }
        else
        {
            gvMDisplay.EmptyDataText = "";
            pds.AllowPaging = true;
            pds.PageSize = 11;
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

            this.gvMDisplay.DataSource = pds;
            this.gvMDisplay.DataBind();
            spareRow = gvMDisplay.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvMDisplay.DataSource = pds;
            gvMDisplay.DataBind();
        }
        ClearGridViewSelected();
    }

    private void ClearGridViewSelected()
    {
        foreach (GridViewRow gvr in gvMDisplay.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[4].Text = "";
            }
        }
    }

    private void ClearText()
    {
        txtDisplayNm.Text = "";
        DDownListBuilding.SelectedIndex  = 0;
        DDownListFloors.SelectedIndex = 0;
        DDownListLocation.SelectedIndex = 0;
        DDownListAreaName.SelectedIndex = 0;
        txtLocationDesc.Text = "";
        txtintention.Text = "";
        txtEstCosts.Text = "";
        txtStartDate.Text = "";
        txtEndDate.Text = "";
        txtMcompany.Text = "";
        txtDisplayDesc.Text = "";
    }
}
