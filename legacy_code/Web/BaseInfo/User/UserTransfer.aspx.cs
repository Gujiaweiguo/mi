using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.Biz;
using Base.DB;
using BaseInfo.User;
using Base.Page;
using BaseInfo.Dept;
using System.Drawing;
namespace MI_Net
{
    public partial class ChangUser : BasePage
    {
        public string baseInfo;
        int numCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["DeptID"] = deptid.Value;
            selectdeptid.Value = Convert.ToString(Session["DeptID"]);
            //读取部门信息
            this.Showdept();
            //给控件添加属性
            this.SetControlProp();
            if (!IsPostBack)
            {
                try
                {
                    page();
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
                baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            }

        }
        /// <summary>lcp 2009-3-5
        /// 给控件添加属性
        /// </summary>
        private void SetControlProp()
        {
            #region
            //btnSave.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
            //btnSave.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
            //btnCancel.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCanceling.gif) no-repeat left top';this.style.fontWeight='bold';");
            //btnCancel.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCancel.gif) no-repeat left top';this.style.fontWeight='normal';");
            //btnBack.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnbacking.gif) no-repeat left top';this.style.fontWeight='bold';");
            //btnBack.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnback.gif) no-repeat left top';this.style.fontWeight='normal';");
            //btnNext.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnnexting.gif) no-repeat left top';this.style.fontWeight='bold';");
            //btnNext.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnnext.gif) no-repeat left top';this.style.fontWeight='normal';");
            #endregion
        }
        /// <summary>lcp,2009-3-5
        /// 读取部门信息
        /// </summary>
        private void Showdept()
        {
            string jsdept = "";

            BaseBO baseBO = new BaseBO();
            //baseBO.OrderBy = "deptid";
            //Resultset rs = baseBO.Query(new Dept());
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
            Resultset rs = baseBO.Query(objDept);

            if (rs.Count > 0)
            {
                foreach (Dept dept in rs)
                {
                    jsdept += dept.DeptID + "|" + dept.PDeptID + "|" + dept.DeptName + "^";
                }
                depttxt.Value = jsdept;

            }
        }
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strUserID = "";
            string strDeptID = "";
            if (ViewState["userID"] != null)
                strUserID = ViewState["userID"].ToString();
            if (Session["DeptID"] != null)
                strDeptID = Session["DeptID"].ToString();
            if (strUserID == "" || strDeptID == "")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidMessage.Value + "'", true);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
                return;
            }

            BaseBO objBasebo = new BaseBO();
            string strSql = "update userrole set deptid='" + strDeptID + "' where userid='" + strUserID + "'";
            if (objBasebo.ExecuteUpdate(strSql) != -1)
            {
                page();
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
                return;
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
        }
        /// <summary>
        /// 树形控件点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void treeClick_Click(object sender, EventArgs e)
        {
            string deptId = "";

            deptId = deptid.Value;
            ViewState["DeptID"] = deptId;

            Session["DeptID"] = deptid.Value;
            page();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/BaseInfo/User/UserTransfer.aspx");
        }
        /// <summary>lcp 2009-3-5
        /// DataGrid页面分页绑定
        /// </summary>
        protected void page()
        {
            BaseBO objBaseBo = new BaseBO();
            UserDepartment objUserDepartment = new UserDepartment();
            BaseInfo.BaseCommon.BindGridView(objBaseBo, objUserDepartment, this.GrdUser);
            #region
            //BaseBO baseBO = new BaseBO();
            //Resultset rs = new Resultset();
            //PagedDataSource pds = new PagedDataSource();
            //int spareRow = 0;
            //DataTable dt = baseBO.QueryDataSet(new UserDepartment()).Tables[0];

            //pds.DataSource = dt.DefaultView;

            //if (pds.Count < 1)
            //{
            //    for (int i = 0; i < GrdUser.PageSize; i++)
            //    {
            //        dt.Rows.Add(dt.NewRow());
            //    }
            //    pds.DataSource = dt.DefaultView;
            //    GrdUser.DataSource = pds;
            //    GrdUser.DataBind();
            //}
            //else
            //{
            //    pds.AllowPaging = true;
            //    pds.PageSize = 12;
            //    //lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
            //    pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;
            //    if (pds.IsFirstPage)
            //    {
            //        btnBack.Enabled = false;
            //        btnNext.Enabled = true;
            //    }

            //    if (pds.IsLastPage)
            //    {
            //        btnBack.Enabled = true;
            //        btnNext.Enabled = false;
            //    }

            //    if (pds.IsFirstPage && pds.IsLastPage)
            //    {
            //        btnBack.Enabled = false;
            //        btnNext.Enabled = false;
            //    }
            //    if (!pds.IsLastPage && !pds.IsFirstPage)
            //    {
            //        btnBack.Enabled = true;
            //        btnNext.Enabled = true;
            //    }
            //    this.GrdUser.DataSource = pds;
            //    this.GrdUser.DataBind();
            //    spareRow = GrdUser.Rows.Count;
            //    for (int i = 0; i < pds.PageSize - spareRow; i++)
            //    {
            //        dt.Rows.Add(dt.NewRow());
            //    }
            //    pds.DataSource = dt.DefaultView;
            //    GrdUser.DataSource = pds;
            //    GrdUser.DataBind();
            //}
            #endregion
        }
        
        /// <summary>
        /// DataqGrid设置（当该行没有数据时，该行选择按钮列不显示）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdUser_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1)
            {
                if (e.Row.Cells[1].Text == "&nbsp;")
                {
                    e.Row.Cells[5].Text = "";
                }
                else
                    e.Row.Cells[4].Text = e.Row.Cells[4].Text + "/" + e.Row.Cells[6].Text;
            }
        }
        /// <summary>
        /// DataGrid选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["userID"] = GrdUser.SelectedRow.Cells[0].Text;
            page();
            this.btnSave.Enabled = true;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "totle", "Load()", true);
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
            this.page();
            foreach (GridViewRow grv in this.GrdUser.Rows)
            {
                grv.BackColor = Color.White;
            }
            this.btnSave.Enabled = false;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
        }
    }

}

