using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Base.Biz;
using BaseInfo.Role;
using Base.Page;
using Base.DB;
using WorkFlow.WrkFlw;
using System.Drawing;
public partial class BaseInfo_Role_Default : BasePage
{
    public string baseInfo;
    private int numCount = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Response.Buffer = false;
        Page.Response.Cache.SetNoStore();
        #region
        //btnBack.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnbacking.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnBack.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnback.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnNext.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnnexting.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnNext.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnnext.gif) no-repeat left top';this.style.fontWeight='normal';");
        #endregion
        showtree();
        if (!IsPostBack)
        {
            BaseBO baseBO = new BaseBO();
            int[] status = BaseInfo.Role.Role.GetLeader();

            foreach (int sta in status)
            {
                this.cmbLeader.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",BaseInfo.Role.Role.GetLeaderDesc(sta)), sta.ToString()));
            }

            int[] rolestatus = BaseInfo.Role.Role.GetRoleStatus();
            foreach (int sta in rolestatus)
            {
                this.cmbRoleStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",BaseInfo.Role.Role.GetRoleStatusDesc(sta)), sta.ToString()));
            }
            page();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
    }

    private void showchktree(int roleid)
    {
        BaseBO baseBo = new BaseBO();
        FuncChkTree functree = new FuncChkTree();
        string jsdept = "";
        //业务组结果集
        Resultset rsBizGrp = baseBo.Query(new QueryBizGrp());

        //节点结构:节点ID|父节点|节点名称|选中标识
        if (rsBizGrp.Count > 0)
        {
            jsdept = "100" + "|" + "0" + "|" + titletext.Value + "|" + 0 + "^";
            
            foreach ( QueryBizGrp bizGrpTree in rsBizGrp)
            {
                jsdept += bizGrpTree.BizGrpID + "|" + "100" + "|" + bizGrpTree.BizGrpName + "|" + 0 + "^";
            }
            functree.SetQuerySql("select a.BizGrpID,b.FuncID,b.FuncName,(case when c.FuncID is null then '0' else '1' end)as fckStatus  from BizGrp a inner join  Func b on a.BizGrpID=b.BizGrpID left join " +
                "(select FuncID,RoleID from  RoleAuth where RoleID=" + roleid + ") c on b.FuncID=C.funcID Where FuncType=" + FuncTree.FUNC_TYPE_FUNCTION + " group by a.BizGrpID,BizGrpName,b.FuncID,b.FuncName,c.FuncID");
            Resultset rsfunc = baseBo.Query(functree);
            if (rsfunc.Count > 0)
            {
                foreach (FuncChkTree funcTree in rsfunc)
                {
                    jsdept += funcTree.BizGrpID.ToString() + funcTree.FuncID.ToString() + "|" + funcTree.BizGrpID + "|" + funcTree.FuncName + "|" + funcTree.fckStatus + "^";
                }
            }
            depttxt.Value = jsdept;
        }
    }
    private void showtree()
    {
        Resultset rs = new Resultset();
        Resultset rsfunc = new Resultset();
        BaseBO baseBo = new BaseBO();
        string jsdept = "";
        rs = baseBo.Query(new QueryBizGrp());
        jsdept = "100" + "|" + "0" + "|" + titletext.Value + "^";
        if (rs.Count > 0)
        {
            foreach (QueryBizGrp bizGrpTree in rs)
            {
                jsdept += bizGrpTree.BizGrpID + "|" + "100" + "|" + bizGrpTree.BizGrpName + "^";
                baseBo.WhereClause = "a.BizGrpID=b.BizGrpID And FuncType=" + FuncTree.FUNC_TYPE_FUNCTION + " And a.bizgrpid=" + bizGrpTree.BizGrpID;
                rsfunc = baseBo.Query(new FuncTree());
                if (rsfunc.Count > 0)
                {
                    foreach (FuncTree funcTree in rsfunc)
                    {
                        jsdept += bizGrpTree.BizGrpID.ToString() + funcTree.FuncID.ToString() + "|" + bizGrpTree.BizGrpID + "|" + funcTree.FuncName + "^";
                    }
                }
            }
        }
        depttxt.Value = jsdept;
    }
    protected void page()
    {
        BaseBO objBaseBo = new BaseBO();
        Role objRole = new Role();
        BaseInfo.BaseCommon.BindGridView(objBaseBo, objRole, this.GrdRole);
        #region
        //BaseBO baseBO = new BaseBO();
        //Resultset rs = new Resultset();
        //PagedDataSource pds = new PagedDataSource();
        //int spareRow = 0;

        //DataTable dt = baseBO.QueryDataSet(new Role()).Tables[0];

        //pds.DataSource = dt.DefaultView;

        //if (pds.Count < 1)
        //{
        //    for (int i = 0; i < GrdRole.PageSize; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    GrdRole.DataSource = pds;
        //    GrdRole.DataBind();
        //}
        //else
        //{
        //    pds.AllowPaging = true;
        //    pds.PageSize = 11;
        //    lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
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

        //    this.GrdRole.DataSource = pds;
        //    this.GrdRole.DataBind();
        //    spareRow = GrdRole.Rows.Count;
        //    for (int i = 0; i < pds.PageSize - spareRow; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    GrdRole.DataSource = pds;
        //    GrdRole.DataBind();
        //}
        #endregion
    }
    //protected void btnBack_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
    //    page();
    //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    //}
    //protected void btnNext_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
    //    page();
    //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    //}
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
    protected void GrdRole_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string gIntro="";
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text != "&nbsp;")
            {
                gIntro = e.Row.Cells[2].Text.ToString();
                e.Row.Cells[2].Text = SubStr(gIntro, 5);
            }
            else
            {
                e.Row.Cells[4].Text = "";
            }

            if (e.Row.Cells[3].Text.Length == 1)
            {
                if (e.Row.Cells[3].Text.Equals(Role.IS_ROLESTATUS_YES.ToString()))
                {
                    e.Row.Cells[3].Text = (String)GetGlobalResourceObject("Parameter", "IS_ROLESTATUS_YES");
                }
                else if (e.Row.Cells[3].Text.Equals(Role.IS_ROLESTATUS_NO.ToString()))
                {
                    e.Row.Cells[3].Text = (String)GetGlobalResourceObject("Parameter", "IS_ROLESTATUS_NO");
                }
            }
        }
    }
    protected void GrdRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Role role = new Role();
        Resultset rs = new Resultset();
        int roleid = Convert.ToInt32(GrdRole.SelectedRow.Cells[0].Text.ToString());
        Session["RoleID"] = roleid;
        baseBO.WhereClause = "roleid=" + roleid;
        rs = baseBO.Query(role);
        if (rs.Count == 1)
        {
            role = rs.Dequeue() as Role;
            txtRoleCode.Text = role.RoleCode;
            txtRoleName.Text = role.RoleName;
            cmbRoleStatus.SelectedValue = role.RoleStatus.ToString();
            cmbLeader.SelectedValue = role.IsLeader.ToString();
            showchktree(roleid);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        }
        page();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "title", "ShowTitle()", true);
    }
    protected void GrdRole_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        foreach (GridViewRow grv in GrdRole.Rows)
        {
            grv.BackColor = Color.White;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
}
