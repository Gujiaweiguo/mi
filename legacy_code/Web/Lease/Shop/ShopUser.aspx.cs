using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.DB;
using Base.Biz;
using RentableArea;
using BaseInfo.Dept;
using Base;
using Lease.ConShop;
using Lease.SMSPara;
using BaseInfo.authUser;
using BaseInfo.User;
using System.Drawing;

public partial class Lease_Shop_ShopUser : System.Web.UI.Page
{
    public string title = "";
    private static int OPR_ADD = 1;/*添加*/
    private static int OPR_EDIT = 2;/*更新*/
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        if (!IsPostBack)
        {
            showtreenode();

            ViewState["ShopID"] = deptid.Value;

            page();

            title = (String)GetGlobalResourceObject("BaseInfo", "TpUse_lblAddUserTitle");

            ViewState["Flag"] = OPR_ADD;
            BindDrop();
           

            cmbStation.Items.Add(new ListItem("收款员"));

            BaseBO baseBO = new BaseBO();
            DataSet ds = new DataSet();

            ds = baseBO.QueryDataSet("Select AutoTPUserID From SMSPara");
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["AutoTPUserID"]) == SMSPara.AUTO_YES)
            {
                txtWorkNo.ReadOnly = true;

                ViewState["Lock"] = SMSPara.AUTO_YES;
            }
            else
            {
                ViewState["Lock"] = SMSPara.AUTO_NO;
            }

        }
        else
        {
            txtPassword.Attributes.Add("value", "");
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
            //ClearGridViewSelected();
        }
    }

    private void BindDrop()
    {
        /*是否有效*/
        int[] status = TpUsr.GetTpUsrStatus();
        cmbStatus.Items.Clear();
        for (int i = 0; i < status.Length; i++)
        {
            cmbStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", TpUsr.GetTpUsrStatusDesc(status[i])), status[i].ToString()));
        }

        /*收款权限*/
        cmbGathering.Items.Clear();
        for (int i = 1; i < 10; i++)
        {
            cmbGathering.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
    }

    private void showtreenode()
    {
        #region
        //string jsdept = "";
        //BaseBO baseBO = new BaseBO();
        //BaseBO baseBOBuilding = new BaseBO();
        //BaseBO baseareaBO = new BaseBO();
        //Resultset rs = new Resultset();
        //Resultset rsf = new Resultset();
        //Resultset rsl = new Resultset();
        //Resultset rss = new Resultset();
        //Dept dept = new Dept();

        //baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL;

        //rs = baseBO.Query(dept);

        //if (rs.Count == 1)
        //{
        //    dept = rs.Dequeue() as Dept;
        //    jsdept = dept.DeptID + "|" + "0" + "|" + dept.DeptName + "^";
        //}
        //else
        //{
        //    return;
        //}

        //rs = baseBOBuilding.Query(new Building());

        //if (rs.Count > 0)
        //{
        //    foreach (Building building in rs)
        //    {
        //        jsdept += building.BuildingID + "|" + dept.DeptID + "|" + building.BuildingName + "^";

        //        baseBO.WhereClause = "BuildingID=" + building.BuildingID;
        //        rsf = baseBO.Query(new Floors());
        //        foreach (Floors floors in rsf)
        //        {
        //            jsdept += building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + floors.BuildingID + "|" + floors.FloorName + "^";

        //            baseBO.WhereClause = "FloorID=" + floors.FloorID;
        //            rsl = baseBO.Query(new Location());

        //            foreach (Location location in rsl)
        //            {
        //                jsdept += building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + "|" + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + location.LocationName + "^";

        //                baseBO.WhereClause = "LocationID=" + location.LocationID + " and FloorID=" + floors.FloorID + " and BuildingID=" + building.BuildingID + " And a.ShopTypeID=b.ShopTypeID";
        //                rss = baseBO.Query(new ConShop());
        //                foreach (ConShop conShop in rss)
        //                {
        //                    jsdept += building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + conShop.ShopId + "|" + building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + "|" + conShop.ShopName + "^";
        //                }
        //            }
        //        }
        //    }
        //}
        //depttxt.Value = jsdept;
        #endregion
        string jsdept = "";
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        Dept dept = new Dept();

        baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_CHILD_COMPANY;   //根节点,取得集团
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        rs = baseBO.Query(dept);
        if (rs.Count == 1)
        {
            dept = rs.Dequeue() as Dept;
            jsdept = dept.DeptID + "|" + "0" + "|" + dept.DeptName + "^";
        }
        else
        {
            return;
        }

        baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL;
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            baseBO.WhereClause += " and EXISTS (SELECT storeID FROM authUser WHERE  dept.deptID = authUser.storeID AND userID =" + sessionUser.UserID + ")";
        }
        rs = baseBO.Query(dept);
        if (rs.Count > 0)
        {
            foreach (Dept store in rs)
            {
                jsdept += store.DeptID + "|" + dept.DeptID + "|" + store.DeptName + "^";
                baseBO.WhereClause = "StoreId=" + store.DeptID;
                rs = baseBO.Query(new Building());
                if (rs.Count > 0)
                {
                    foreach (Building building in rs)//大楼
                    {
                        jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + "|" + store.DeptID.ToString() + "|" + building.BuildingName.ToString() + "^";
                        baseBO.WhereClause = "floors.BuildingID=" + building.BuildingID;
                        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
                        {
                            baseBO.WhereClause += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
                        }

                        rs = baseBO.Query(new floorsAuth());
                        foreach (floorsAuth floors in rs)//楼层
                        {
                            jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + store.DeptID.ToString() + building.BuildingID + "|" + floors.FloorName + "^";
                            baseBO.WhereClause = "FloorID=" + floors.FloorID;
                            rs = baseBO.Query(new Location());

                            foreach (Location location in rs)
                            {
                                jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + "|" + store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + location.LocationName + "^";

                                baseBO.WhereClause = "storeID=" + store.DeptID + " and LocationID=" + location.LocationID + " and FloorID=" + floors.FloorID + " and BuildingID=" + building.BuildingID + " And a.ShopTypeID=b.ShopTypeID And ShopStatus =" + ConShop.CONSHOP_TYPE_INGEAR;
                                rs = baseBO.Query(new ConShop());
                                foreach (ConShop conShop in rs)
                                {
                                    jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + conShop.ShopId + "|" + store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + "|" + conShop.ShopName + "^";
                                }
                            }
                        }
                    }
                }
            }
        }
        depttxt.Value = jsdept;
    }
    protected void GrdUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[4].Text = "";
            }
        }
    }
    protected void GrdUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        TpUsr tpUsr = new TpUsr();

        baseBO.WhereClause = "TPUsrId = '" + GrdUser.SelectedRow.Cells[0].Text.Trim() + "'";

        rs = baseBO.Query(tpUsr);

        if (rs.Count == 1)
        {
            txtPassword.Attributes.Add("value", "****");
            tpUsr = rs.Dequeue() as TpUsr;
            ViewState["TPUSERID"] = tpUsr.TPUsrId.Trim();
            txtWorkNo.Text = tpUsr.TPUsrId.Trim().Substring(tpUsr.TPUsrId.Trim().Length-2);
            txtID.Text = tpUsr.IDNo.Trim();
            txtUserName.Text = tpUsr.TPUsrNm.Trim();
            txtMobile.Text = tpUsr.Phone.Trim();
            hidnPassword.Value = tpUsr.szPin.Trim();
            //Page.RegisterStartupScript("init", @"<script   language=javascript>ShopUser.txtPassword.value='" + tpUsr.szPin.Trim() + "'   </script>"); 

            if (tpUsr.Sex.Trim() == "M")
            {
                rdoMan.Checked = true;
                rdoWoman.Checked = false;
            }
            else if (tpUsr.Sex.Trim() == "F")
            {
                rdoMan.Checked = false;
                rdoWoman.Checked = true;
            }

            txtBirth.Text = Convert.ToDateTime(tpUsr.Dob).ToString("yyyy-MM-dd");
            txtBeginWorkDate.Text = Convert.ToDateTime(tpUsr.DateStart).ToString("yyyy-MM-dd");
            cmbStation.SelectedItem.Text = tpUsr.JobTitleNm.Trim();
            cmbGathering.SelectedItem.Text = tpUsr.GpId;

            if (tpUsr.TPUsrStatus.Trim() == "E")
            {
                chkConcerned.Checked = true;
            }
            else if (tpUsr.TPUsrStatus.Trim() == "N")
            {
                chkConcerned.Checked = false;
            }

            if (tpUsr.DeleteTime == null)
            {
                cmbStatus.SelectedValue = TpUsr.TPUSR_STATUS_YES.ToString();
            }
            else
            {
                cmbStatus.SelectedValue = TpUsr.TPUSR_STATUS_NO.ToString();
            }

            ViewState["Flag"] = OPR_EDIT;
            page();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (this.txtUserName.Text.Trim() == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "User_lblUserName") + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
            ClearGridViewSelected();
            return;
            
        } if (this.txtWorkNo.Text.Trim().Length !=2)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "User_lblWorkNo") +  "为2位数字'", true);
            ClearGridViewSelected();
            return;
        }
        BaseBO baseBO = new BaseBO();
        TpUsr tpUsr = new TpUsr();
        string buildingid = "0";
        string custid = "0";

        DataSet buildDS = baseBO.QueryDataSet("select conshop.BuildingID,contract.CustID from conshop inner join contract on (conshop.contractid=contract.contractid) where conshop.ShopID=" + ViewState["ShopID"].ToString());
        if (buildDS.Tables[0].Rows.Count == 1)
        {
            buildingid = buildDS.Tables[0].Rows[0]["BuildingID"].ToString();
            custid = buildDS.Tables[0].Rows[0]["CustID"].ToString();
        }

        if (Convert.ToInt32(ViewState["Lock"]) == SMSPara.AUTO_YES)
        {
            tpUsr.TPUsrId = BaseApp.GetSMSParaNextTPUserID().ToString();
        }
        else
        {
            if (this.txtWorkNo.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "User_lblWorkNo") + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
                return;
            }
            tpUsr.TPUsrId = ViewState["ShopID"].ToString().Substring(ViewState["ShopID"].ToString().Length -3,3).Trim()+txtWorkNo.Text.Trim();
        }
        tpUsr.IDNo = txtID.Text.Trim();
        tpUsr.TPUsrNm = txtUserName.Text.Trim();
        
        tpUsr.Phone = txtMobile.Text.Trim();
        if (rdoMan.Checked)
        {
            tpUsr.Sex = "M";
        }
        else if (rdoWoman.Checked)
        {
            tpUsr.Sex = "F";
        }
        try { tpUsr.Dob = Convert.ToDateTime(txtBirth.Text); }
        catch { tpUsr.Dob = DateTime.Now.Date; }
        try { tpUsr.DateStart = Convert.ToDateTime(txtBeginWorkDate.Text); }
        catch { tpUsr.DateStart = DateTime.Now.Date; }
        tpUsr.JobTitleNm = cmbStation.SelectedItem.Text;
        tpUsr.GpId = cmbGathering.SelectedItem.Text;
        

        if (chkConcerned.Checked)
        {
            tpUsr.TPUsrStatus = "E";
        }
        else
        {
            tpUsr.TPUsrStatus = "N";
        }
        tpUsr.UnitId = ViewState["ShopID"].ToString().Substring(ViewState["ShopID"].ToString().Length -3,3);
        tpUsr.BuildingID = buildingid;
        tpUsr.CustID = custid;
        if (Convert.ToInt32(cmbStatus.SelectedValue) == TpUsr.TPUSR_STATUS_NO)
        {
            tpUsr.DeleteTime = DateTime.Now;
        }

        if (Convert.ToInt32(ViewState["Flag"]) == OPR_ADD)
        {
            tpUsr.szPin = txtPassword.Text.Trim();

            string str_sql = "select TPUsrID from TpUsr where TPUsrID = '" + tpUsr.TPUsrId + "'";
            DataSet ds = baseBO.QueryDataSet(str_sql);
            if (ds.Tables[0].Rows.Count <= 0)
            {
                if (baseBO.Insert(tpUsr) == -1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "    " + (String)GetGlobalResourceObject("BaseInfo", "Associator_lblStaffID") + ":" + tpUsr.TPUsrId + "'", true);
                    ClearText();
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Exist") + "'", true);
            }
        }
        else if (Convert.ToInt32(ViewState["Flag"]) == OPR_EDIT)
        {

            if (Convert.ToInt32(cmbStatus.SelectedValue) == TpUsr.TPUSR_STATUS_NO)
            {
                if (baseBO.ExecuteUpdate("Update TpUsr Set DeleteTime = '" + DateTime.Now + "' Where TPUsrId = '" + ViewState["TPUSERID"].ToString() + "'") == -1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                    ClearText();
                }
            }
            else
            {
                //if (txtPassword.Text == "****")
                //{
                //    tpUsr.szPin = hidnPassword.Value;
                //}
                //else
                //{
                    tpUsr.szPin = txtPassword.Text.Trim();
               // }
                BaseTrans trans = new BaseTrans();
                trans.BeginTrans();
                try
                {
                    tpUsr.DeleteTime = DateTime.Now;
                    trans.WhereClause = "TPUsrId = '" + ViewState["TPUSERID"] + "'";

                    if (trans.Update(tpUsr) == -1)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                    }
                    else
                    {
                        trans.ExecuteUpdate("update TpUsr set DeleteTime = null where TPUsrId = '" + ViewState["TPUSERID"] + "'");
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                        ClearText();
                    }
                }
                catch(Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                trans.Commit();
            }
        }
        BindDrop();
        ViewState["Flag"] = OPR_ADD;
        page();
        //ViewState["ShopID"] = "";
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ViewState["ShopID"] = "";
        ClearText();
        page();
        foreach (GridViewRow grv in GrdUser.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
    
    protected void treeClick_Click(object sender, EventArgs e)
    {
        string deptId = "";
        deptId = deptid.Value;
        selectdeptid.Value = deptid.Value;
        if (deptId.Length > 12)
        {
            ViewState["ShopID"] = deptId.Substring(deptId.Length - 3, 3);
            page();
            this.btnSave.Enabled = true;
        }
        else
        {
            ViewState["ShopID"] = "";
            page();
            this.btnSave.Enabled = false;
        }
        this.ClearText();
        foreach (GridViewRow grv in GrdUser.Rows)
        {
            grv.BackColor = Color.White;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }

    protected void page()
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        if (ViewState["ShopID"].ToString() == "")
        {
            baseBO.WhereClause = "UnitID =-1";
        }
        else
        {
            baseBO.WhereClause = "UnitID =" + ViewState["ShopID"];
        }

        baseBO.OrderBy = "TPUsrId";

        DataTable dt = baseBO.QueryDataSet(new TpUsr()).Tables[0];

        int count = dt.Rows.Count;
        if (count==0 || count % 13 != 0)
        {
            for (int i = (count % 13); i < 13; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
        }
        GrdUser.DataSource = dt;
        GrdUser.DataBind();

        foreach (GridViewRow grv in GrdUser.Rows)
        {
            if (grv.Cells[1].Text != "&nbsp;")
            {
                grv.Cells[3].Text = grv.Cells[3].Text.Trim().Substring(0, 10);
            }
        }
        //ClearGridViewSelected();
    }

    private void ClearGridViewSelected()
    {
        foreach (GridViewRow gvr in GrdUser.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[4].Text = "";
            }
            else
            {
                gvr.Cells[3].Text = gvr.Cells[3].Text.Substring(0,10);
            }
        }
    }

    private void ClearText()
    {
        txtBeginWorkDate.Text = "";
        txtBirth.Text = "";
        txtID.Text = "";
        txtMobile.Text = "";
        txtPassword.Text = "";
        txtUserName.Text = "";
        txtWorkNo.Text = "";
        cmbGathering.SelectedIndex=0;
        cmbStation.SelectedIndex=0;
        //ViewState["ShopID"] = "";
    }
    protected void GrdUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        this.page();
        foreach (GridViewRow grv in GrdUser.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
}
