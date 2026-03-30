using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Base.Biz;
using Base.DB;
using RentableArea;
using BaseInfo.Dept;
using Base.Page;
using BaseInfo.authUser;
using BaseInfo.User;
public partial class RentableArea_Building_QueryAreaVindicate : BasePage
{
    public string baseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        showtreenode();
        
        if (!IsPostBack)
        {
            int[] status = Area.GetAreaStatus();
            for (int i = 0; i < status.Length; i++)
            {
                this.cmbStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Area.GetAreaStatusDesc(status[i])), status[i].ToString()));
            }
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_lblAreaQuery");
            strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        textlock();
    }
    protected void treeClick_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        string areaid = "";
        
        Area area = new Area();

        areaid = deptid.Value;
        if (areaid.Length > 3)
        {
            Session["AreaID"] = deptid.Value.Substring(3);

            baseBO.WhereClause = "AreaID=" + areaid.Substring(3);
            Resultset rs = baseBO.Query(area);
            if (rs.Count == 1)
            {
                area = rs.Dequeue() as Area;
                txtAreaCode.Text = area.AreaCode;
                txtAreaName.Text = area.AreaName;
                txtNote.Text = area.Note;
                titleArea.Text = area.AreaName;
                cmbStatus.SelectedValue = area.AreaStatus.ToString();
            }
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }

    private void textlock()
    {
        txtAreaCode.Text = "";

        txtAreaName.Text = "";

        txtNote.Text = "";
        cmbStatus.SelectedValue = Area.AREA_STATUS_VALID.ToString();
    }
    private void showtreenode()
    {
        string jsdept = "";
        BaseBO baseBo = new BaseBO();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string strSql = @"SELECT 
		CreateUserId,CreateTime,DeptID,DeptCode,
		DeptName,DeptLevel,PDeptID,DeptType,City,
		RegAddr,OfficeAddr,PostAddr,PostCode,Tel,
		OfficeTel,Fax,DeptStatus,IndepBalance,isnull(OrderID,0) as OrderID
FROM 
		Dept where DeptType in (1,3,4,5) 
union
SELECT 
		CreateUserId,CreateTime,DeptID,DeptCode,
		DeptName,DeptLevel,PDeptID,DeptType,City,
		RegAddr,OfficeAddr,PostAddr,PostCode,Tel,
		OfficeTel,Fax,DeptStatus,IndepBalance,isnull(OrderID,0) as OrderID
FROM 
		Dept where DeptType=6";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            strSql += " and EXISTS (SELECT storeID FROM authUser WHERE  dept.deptID = authUser.storeID AND userID =" + sessionUser.UserID + ")";
        }
        strSql += " ORDER BY Pdeptid";
        Dept objDept = new Dept();
        objDept.SetQuerySql(strSql);
        Resultset rs = baseBo.Query(objDept);
        if (rs.Count > 0)
        {
            foreach (Dept dept in rs)
            {
                if (dept.DeptType != 6)
                {
                    if (baseBo.QueryDataSet("select deptid  from dept where pdeptid='" + dept.DeptID + "'").Tables[0].Rows.Count > 0)
                    {
                        jsdept += dept.DeptID + "|" + dept.PDeptID + "|" + dept.DeptName + "|" + "" + "^";
                    }
                }
                else
                {
                    jsdept += dept.DeptID + "|" + dept.PDeptID + "|" + dept.DeptName + "|" + "" + "^";
                    baseBo.WhereClause = "StoreID='" + dept.DeptID.ToString() + "'";
                    Resultset rsArea = baseBo.Query(new Area());
                    if (rsArea.Count > 0)
                    {
                        foreach (Area area in rsArea)
                        {
                            if (area.AreaStatus == Area.AREA_STATUS_INVALID)
                            {
                                jsdept += dept.DeptID.ToString() + area.AreaID.ToString() + "|" + dept.DeptID + "|" + area.AreaName + "|" + "../../App_Themes/nlstree/img/node3.gif" + "^";
                            }
                            else
                            {
                                jsdept += dept.DeptID.ToString() + area.AreaID.ToString() + "|" + dept.DeptID + "|" + area.AreaName + "|" + "" + "^";
                            }
                        }
                    }
                }
            }
            depttxt.Value = jsdept;
        }
    }
}
