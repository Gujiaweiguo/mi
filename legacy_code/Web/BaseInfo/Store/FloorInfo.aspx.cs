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
using BaseInfo.Dept;
using BaseInfo.Store;
using RentableArea;
using BaseInfo.User;
using BaseInfo.authUser;

public partial class BaseInfo_Store_FloorInfo : BasePage
{
    public string isbro;
    public string baseinfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //显示树形列表
            this.ShowTree();
            //为按钮添加属性
            this.SetControlPro();
            if (Request["look"] != null)
            {
                isbro = Request["look"].ToString();
                if (isbro.ToLower() == "yes")
                {
                    this.btnCancel.Visible = false;
                    this.btnSave.Visible = false;
                    this.LockControl();
                    baseinfo = (String)GetGlobalResourceObject("BaseInfo", "Store_BusinessItemBasicInfobrowse");
                }
                else
                    baseinfo = (String)GetGlobalResourceObject("BaseInfo", "Store_BusinessItemBasicInfoMaintenance");
            }
            this.btnSave.Enabled = false;
        }
    }
    private void LockControl()
    {
        this.txtDate.Enabled = false;
        this.txtArea.Enabled = false;
        this.txtThing.Enabled = false;
        this.txtType.Enabled = false;
    }
    /// <summary>
    /// 显示树形列表
    /// </summary>
    private void ShowTree()
    {
        string jsdept = "";
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "deptlevel=1";
        Resultset rsD = objBaseBo.Query(new Dept());
        //jsdept = "100" + "|" + "10" + "|" + "阳光新业" + "^";
        Dept objD = rsD.Dequeue() as Dept;
        jsdept = objD.DeptID + "|" + objD.PDeptID + "|" + objD.DeptName + "^";

        objBaseBo.WhereClause = "depttype = 6";
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            objBaseBo.WhereClause += " and EXISTS (SELECT storeID FROM authUser WHERE  dept.deptID = authUser.storeID AND userID =" + sessionUser.UserID + ")";
        }
        objBaseBo.OrderBy = "OrderID";
        Resultset rs = objBaseBo.Query(new Dept());
        objBaseBo.OrderBy = "";
        if (rs.Count > 0)
        {
            foreach (Dept objDept in rs)
            {
                //jsdept += objDept.DeptID + "|" + "100" + "|" + objDept.DeptName + "^";
                jsdept += objDept.DeptID + "|" + objD.DeptID + "|" + objDept.DeptName + "^";
                objBaseBo.WhereClause = "StoreId="+objDept.DeptID;
                Resultset rsBuilding = objBaseBo.Query(new Building());
                if (rsBuilding.Count > 0)
                {
                    foreach (Building objBuilding in rsBuilding)
                    {
                        jsdept +=objBuilding.BuildingID + "|" + objDept.DeptID + "|" + objBuilding.BuildingName+ "^";
                        objBaseBo.WhereClause = "floors.StoreId=" + objDept.DeptID + " and floors.buildingId=" + objBuilding.BuildingID;
                        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
                        {
                            objBaseBo.WhereClause += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
                        }
                        Resultset rsFloor = objBaseBo.Query(new floorsAuth());
                        {
                            if (rsFloor.Count > 0)
                            {
                                foreach (floorsAuth objFloors in rsFloor)
                                {
                                    jsdept += objDept.DeptID + "," + objBuilding.BuildingID+","+objFloors.FloorID + "|" + objBuilding.BuildingID + "|" + objFloors.FloorName + "^";
                                }
                            }
                        }
                    }
                }
            }
        }
        depttxt.Value = jsdept;
    }
    /// <summary>
    /// 为按钮添加属性
    /// </summary>
    private void SetControlPro()
    {
        this.btnSave.Attributes.Add("onclick", "return CheckIsNull()");
    }
    protected void treeClick_Click(object sender, EventArgs e)
    {
        this.BindData(deptid.Value);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
        this.btnSave.Enabled = true;
    }
    /// <summary>
    /// 绑定数据
    /// </summary>
    private void BindData(string strDeptID)
    {
        if (strDeptID != "")
        {
            string[] array = strDeptID.Split(',');//ViewState["FloorID"]里面存储部门编号、大楼编号、楼层编号，以逗号分隔
            if (array.Length == 3)
            {
                ViewState["DeptID"] = strDeptID;
                BaseBO objBaseBo = new BaseBO();
                objBaseBo.WhereClause = "floorid=" + array[2].ToString();
                Resultset rs = objBaseBo.Query(new FloorInfo());
                if (rs.Count == 1)
                {
                    FloorInfo objFloorInfo = rs.Dequeue() as FloorInfo;
                    this.txtDate.Text = objFloorInfo.CompleteDate.ToShortDateString();
                    this.txtType.Text = objFloorInfo.ConfigurationType;
                    this.txtThing.Text = objFloorInfo.FloorThing;
                    this.txtArea.Text = objFloorInfo.Area.ToString();
                    ViewState["FloorID"] = objFloorInfo.FloorId;
                }
                else
                {
                    ViewState["FloorID"] = array[2].ToString();
                    this.SetTextBoxClear();
                }
            }
        }
    }
    /// <summary>
    /// 清空输入框
    /// </summary>
    private void SetTextBoxClear()
    {
        this.txtDate.Text = "";
        this.txtArea.Text = "";
        this.txtThing.Text = "";
        this.txtType.Text = "";
    }
    
    /// <summary>
    /// 新增
    /// </summary>
    /// <returns></returns>
    private void SaveAdd()
    {
        BaseBO objBaseBo = new BaseBO();
        FloorInfo objFloorInfo = new FloorInfo();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        //objFloorInfo.FloorId = Base.BaseApp.GetCustumerID("FloorInfo", "FloorId");
        objFloorInfo.FloorId =Int32.Parse(ViewState["FloorID"].ToString());
        if (this.txtDate.Text.Trim()!="")
            objFloorInfo.CompleteDate =DateTime.Parse(this.txtDate.Text.Trim());
        objFloorInfo.FloorThing = this.txtThing.Text.Trim();
        objFloorInfo.ConfigurationType = this.txtType.Text.Trim();
        if (this.txtArea.Text.Trim() != "")
            objFloorInfo.Area = decimal.Parse(this.txtArea.Text.Trim());
        else
            objFloorInfo.Area = 0;
        objFloorInfo.CreateUserId = objSessionUser.CreateUserID;
        objFloorInfo.CreateTime = DateTime.Now;
        objFloorInfo.OprDeptID = objSessionUser.OprDeptID;
        objFloorInfo.OprRoleID = objSessionUser.OprRoleID;
        if (objBaseBo.Insert(objFloorInfo) != -1)
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
            return;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
    }
    /// <summary>
    /// 更新
    /// </summary>
    private void SaveUpdate()
    {
        BaseBO objBaseBo = new BaseBO();
        FloorInfo objFloorInfo = new FloorInfo();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        if (this.txtDate.Text.Trim() != "")
            objFloorInfo.CompleteDate = DateTime.Parse(this.txtDate.Text.Trim());
        objFloorInfo.FloorThing = this.txtThing.Text.Trim();
        objFloorInfo.ConfigurationType = this.txtType.Text.Trim();
        if (this.txtArea.Text.Trim() != "")
            objFloorInfo.Area = decimal.Parse(this.txtArea.Text.Trim());
        else
            objFloorInfo.Area = 0;
        objFloorInfo.ModifyUserId = objSessionUser.ModifyUserID;
        objFloorInfo.ModifyTime = DateTime.Now;
        objFloorInfo.OprDeptID = objSessionUser.OprDeptID;
        objFloorInfo.OprRoleID = objSessionUser.OprRoleID;
        objBaseBo.WhereClause = "floorid=" +Int32.Parse(ViewState["FloorID"].ToString());
        if (objBaseBo.Update(objFloorInfo) != -1)
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
            return;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
    }
    /// <summary>
    /// 判断FloorInfo表中是否存在数据
    /// </summary>
    /// <param name="strFloorID"></param>
    /// <returns></returns>
    private bool GetFloorValue(string strFloorID)
    {
        BaseBO objBaseBo = new BaseBO();
        FloorInfo objFloorInfo = new FloorInfo();
        objBaseBo.WhereClause = "floorid="+strFloorID;
        Resultset rs = objBaseBo.Query(objFloorInfo);
        if (rs.Count > 0)
            return true;
        else
            return false;
    }
    /// <summary>
    /// 保存事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ViewState["DeptID"] != null && ViewState["DeptID"].ToString() != "")
        {
            if (!this.GetFloorValue(ViewState["FloorID"].ToString()))
                this.SaveAdd();//新增
            else
                this.SaveUpdate();//更新
        }
        ViewState["FloorID"] = "";
        ViewState["DeptID"] = "";
        this.SetTextBoxClear();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/BaseInfo/Store/FloorInfo.aspx?look=no");
    }
}
