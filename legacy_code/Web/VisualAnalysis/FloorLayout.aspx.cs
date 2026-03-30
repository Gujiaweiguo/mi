using System;
using System.Web.UI;
using RentableArea;
using BaseInfo.Dept;
using Base.Biz;
using Base.DB;
using BaseInfo.User;
using BaseInfo.authUser;

public partial class VisualAnalysis_FloorLayout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
            this.ShowTree();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string strValue = deptid.Value;
        if (strValue.Length == 9)
        {
            string ss = "~/VisualAnalysis\\VAGraphic\\ImgControl\\ImgLayOut.aspx?FloorID=" + strValue.Substring(strValue.Length - 3);
            Response.Redirect(ss);
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    private void ShowTree()
    {
        BaseBO objBase = new BaseBO();
        Resultset rs = new Resultset();
        Dept objDept = new Dept();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        objBase.WhereClause = "DeptType=" + Dept.DEPT_TYPE_CHILD_COMPANY;   //根节点,取得集团
        string jsdept = "";
        rs = objBase.Query(objDept);
        if (rs.Count == 1)
        {
            objDept = rs.Dequeue() as Dept;
            jsdept = objDept.DeptID + "|" + "0" + "|" + objDept.DeptName + "^";
        }
        else
        {
            return ;
        }
        objBase.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL;//商业项目
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            objBase.WhereClause += " and EXISTS (SELECT storeID FROM authUser WHERE  dept.deptID = authUser.storeID AND userID =" + sessionUser.UserID + ")";
        }
        rs = objBase.Query(objDept);
        if (rs.Count > 0)
        {
            foreach (Dept store in rs)
            {
                jsdept += store.DeptID + "|" + objDept.DeptID + "|" + store.DeptName + "^";
                objBase.WhereClause = "StoreId=" + store.DeptID;
                rs = objBase.Query(new Building());
                if (rs.Count > 0)
                {
                    foreach (Building building in rs)
                    {
                        jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + "|" + store.DeptID.ToString() + "|" + building.BuildingName.ToString() + "^";
                        objBase.WhereClause = "floors.BuildingID=" + building.BuildingID;

                        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
                        {
                            objBase.WhereClause += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
                        }
                        rs = objBase.Query(new floorsAuth());
                        foreach (floorsAuth floors in rs)
                        {
                            jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + store.DeptID.ToString() + building.BuildingID + "|" + floors.FloorName + "^";
                        }
                    }
                }
            }
        }
        depttxt.Value = jsdept;
    }
}
