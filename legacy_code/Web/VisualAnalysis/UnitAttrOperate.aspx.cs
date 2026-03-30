using System;
using System.Data;
using System.Web.UI;
using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Dept;
using RentableArea;
using Base.XML;
using System.IO;
using System.Windows.Forms;

public partial class VisualAnalysis_UnitAttrOperate : BasePage
{
    public string baseinfo = "";
    public static string strFileFullName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        baseinfo = (String)GetGlobalResourceObject("BaseInfo", "ShopXml_UnitAttrVindicate");
        if (!this.IsPostBack)
        {
            this.btnUp.Attributes.Add("onclick", this.Page.GetPostBackEventReference(this.btn));//获得上传文件
            this.ShowTree();
        }
    }
    /// <summary>
    /// 显示树形列表
    /// </summary>
    private void ShowTree()
    {
        string jsdept = "";
        BaseBO baseBO = new BaseBO();
        BaseBO baseBOBuilding = new BaseBO();
        BaseBO baseareaBO = new BaseBO();
        Resultset rs = new Resultset();
        Resultset rsd = new Resultset();
        Resultset rsf = new Resultset();
        Resultset rsl = new Resultset();
        Resultset rsu = new Resultset();
        Dept dept = new Dept();
        Dept deptGrp = new Dept();
        baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_CHILD_COMPANY;   //根节点,取得集团
        rs = baseBO.Query(dept);
        if (rs.Count == 1)
        {
            deptGrp = rs.Dequeue() as Dept;
            jsdept = deptGrp.DeptID + "|" + "0" + "|" + deptGrp.DeptName + "^";
        }
        else
        {
            return;
        }
        baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL;
        baseBO.OrderBy = "orderid";
        rsd = baseBO.Query(dept);
        baseBO.OrderBy = "";
        if (rsd.Count > 0)
        {
            foreach (Dept store in rsd)
            {
                jsdept += store.DeptID + "|" + deptGrp.DeptID + "|" + store.DeptName + "^";
                baseBOBuilding.WhereClause = "StoreId=" + store.DeptID;
                rs = baseBOBuilding.Query(new Building());

                if (rs.Count > 0)
                {
                    foreach (Building building in rs)
                    {
                        jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + "|" + store.DeptID.ToString() + "|" + building.BuildingName.ToString() + "^";
                        baseBO.WhereClause = "BuildingID=" + building.BuildingID;
                        rsf = baseBO.Query(new Floors());
                        foreach (Floors floors in rsf)
                        {
                            jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + store.DeptID.ToString() + floors.BuildingID.ToString() + "|" + floors.FloorName.ToString() + "^";
                            baseBO.WhereClause = "FloorID=" + floors.FloorID;
                            rsl = baseBO.Query(new Location());
                            foreach (Location location in rsl)
                            {
                                jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + "|" + store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + location.LocationName + "^";
                                baseBO.WhereClause = "LocationID=" + location.LocationID + " and FloorID=" + floors.FloorID + " and BuildingID=" + building.BuildingID;
                                rsu = baseBO.Query(new Units());
                                foreach (Units units in rsu)
                                {
                                    jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + units.UnitID.ToString() + "|" + store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + "|" + units.UnitCode.ToString() + "^";
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
    /// 树形列表的点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void treeClick_Click(object sender, EventArgs e)
    {
        this.txtX.Text = "";
        this.txtY.Text = "";
        ViewState["UnitID"] = deptid.Value;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        this.btnSave.Enabled = true;
        BaseBO objBaseBo = new BaseBO();
        if (ViewState["UnitID"].ToString().Length == 15)
        {
            #region
            //DataSet ds = objBaseBo.QueryDataSet("select x,y,map from ShopXML where unitid='" + ViewState["UnitID"].ToString().Substring(ViewState["UnitID"].ToString().Length - 3) + "'");
            //if (ds != null && ds.Tables[0].Rows.Count == 1)
            //{
            //    this.txtX.Text = ds.Tables[0].Rows[0]["x"].ToString();
            //    this.txtY.Text = ds.Tables[0].Rows[0]["y"].ToString();
            //    if (ds.Tables[0].Rows[0]["map"].ToString() != "")
            //    {
            //        this.Image1.Visible = true;
            //        string strUrl = ds.Tables[0].Rows[0]["map"].ToString();
            //        this.Image1.ImageUrl = strUrl;
            //    }
            //    else
            //        this.Image1.Visible = false;
            //}
            //else
            #endregion
            this.Image1.Visible = false;
            this.btnSave.Enabled = true;
        }
        else
            this.btnSave.Enabled = true;
        
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ViewState["UnitID"] != null && ViewState["UnitID"].ToString() != "")
        {
            #region 
            //string strFileName = "";
            //if (strFileFullName != "")
            //{
            //    //strFileName = "img\\" + strFileFullName.Substring(strFileFullName.LastIndexOf("\\") + 1);
            //    strFileName = "VAGraphic/Img/Units/" + this.lblmap.Text.Trim();
            //}
            //string strShopID = ViewState["UnitID"].ToString();
            //BaseBO objBaseBo = new BaseBO();
            //string strSql = "select unit.UnitID,ShopID,BuildingID,AreaID,FloorID,LocationID,UnitCode,AreaLevelID,FloorArea,UseArea,Note,UnitStatus,Trade2ID,UnitTypeID,StoreID,rb,gb,bb from unit left join conshopunit on unit.unitid=conshopunit.unitid left join traderelation on unit.Trade2ID = traderelation.tradeid where unit.unitid='" + ViewState["UnitID"].ToString().Substring(ViewState["UnitID"].ToString().Length - 3) + "'";
            //ShopXMLInfo objShopXml = new ShopXMLInfo();
            //DataSet ds = objBaseBo.QueryDataSet(strSql);
            //if (ds != null && ds.Tables[0].Rows.Count == 1)
            //{
            //    //objShopXml.UnitID = ds.Tables[0].Rows[0]["UnitID"].ToString();
            //    objShopXml.ShopID  = ds.Tables[0].Rows[0]["ShopID"].ToString();
            //    objShopXml.StoreID  = ds.Tables[0].Rows[0]["StoreID"].ToString();
            //    objShopXml.BuildingID = ds.Tables[0].Rows[0]["BuildingID"].ToString();
            //    objShopXml.FloorID = ds.Tables[0].Rows[0]["FloorID"].ToString();
            //    objShopXml.LocationID = ds.Tables[0].Rows[0]["LocationID"].ToString();
            //    objShopXml.AreaID = ds.Tables[0].Rows[0]["AreaID"].ToString();
            //    objShopXml.FloorArea = ds.Tables[0].Rows[0]["FloorArea"].ToString();
            //    objShopXml.RentStatus = "0";
            //    objShopXml.map = "Img/icon1.png";
            //    objShopXml.depth = "1";
            //    objShopXml.rb = ds.Tables[0].Rows[0]["rb"].ToString();
            //    objShopXml.gb = ds.Tables[0].Rows[0]["gb"].ToString();
            //    objShopXml.bb = ds.Tables[0].Rows[0]["bb"].ToString();
            //    objShopXml.x = this.txtX.Text.Trim();
            //    objShopXml.y = this.txtY.Text.Trim();
            //    objShopXml.map = strFileName;//文件名称
            //}
            //DataSet dsShopXml = objBaseBo.QueryDataSet("select unitid,buildingid,floorid from ShopXML where unitid ='" + ViewState["UnitID"].ToString().Substring(ViewState["UnitID"].ToString().Length-3) + "'");
            //if (dsShopXml != null && dsShopXml.Tables[0].Rows.Count == 1)//更新
            //{
            //    objBaseBo.WhereClause = "unitid=" + ViewState["UnitID"].ToString().Substring(ViewState["UnitID"].ToString().Length - 3);
            //    if (objBaseBo.Update(objShopXml) != -1)
            //    {
            //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
            //    }
            //    else
            //    {
            //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
            //        return;
            //    }
            //}
            //else//新增
            //{
            //    objShopXml.UnitID = ViewState["UnitID"].ToString().Substring(ViewState["UnitID"].ToString().Length - 3);
            //    if (objBaseBo.Insert(objShopXml) != -1)
            //    {
            //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
            //    }
            //    else
            //    {
            //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
            //        return;
            //    }
            //}
            #endregion
            BaseBO objBaseBo = new BaseBO();
            string strmap = "";
            if(this.lblmap.Text.Trim()!="")
                strmap =  "../../VAGraphic/Img/Units/"+this.lblmap.Text.Trim();
            objBaseBo.ExecuteUpdate("update unit set map='" + strmap + "',x='" + this.txtX.Text.Trim() + "',y='" + this.txtY.Text.Trim() + "' where unitid='" + ViewState["UnitID"].ToString().Substring(ViewState["UnitID"].ToString().Length - 3) + "'");
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
        }
        ViewState["UnitID"] = "";
        this.ClearText();
        this.btnSave.Enabled = false;
    }
    /// <summary>
    /// 判断上传的图片类型
    /// </summary>
    private bool CheckFileType()
    {
        string strType = strFileFullName.Substring(strFileFullName.LastIndexOf(".") + 1);//得到文件的后缀名 
        if (strType.ToLower() != "png")
            return false;
        else
            return true;
    }
    /// <summary>
    /// 清空输入框
    /// </summary>
    private void ClearText()
    {
        this.txtX.Text = "";
        this.txtY.Text = "";
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.ClearText();
    }
    /// <summary>
    /// 获得上传文件名
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Click(object sender, EventArgs e)
    {
        if (this.FileUpload1.HasFile)
        {
            strFileFullName = this.FileUpload1.PostedFile.FileName;
            if (!this.CheckFileType())//只能上传png图片
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "Mess", "alert('只能上传png图片');", true);
                return;
            }
            //string strFileName = strFileFullName.Substring(strFileFullName.LastIndexOf("\\") + 1);
            string strPath = "VAGraphic\\Img\\Units\\" + ViewState["UnitID"].ToString().Substring(ViewState["UnitID"].ToString().Length - 3) + ".png"; ;//路径
            if (File.Exists(Server.MapPath(strPath)))//如果文件已经存在
            {
                if(MessageBox.Show("图片已经存在，是否覆盖?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)==DialogResult.OK)
                {
                    try
                    {
                        File.Delete(Server.MapPath(strPath));
                    }
                    catch { }
                }
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "a", "CheckConfirm();", true);
            }
            //else
                this.btnSaveFile_Click(sender,e);
        }
    }
    protected void btnUp_Click(object sender, EventArgs e)
    {
    }
    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveFile_Click(object sender, EventArgs e)
    {
        try
        {
            string strFileName = strFileFullName.Substring(strFileFullName.LastIndexOf("\\") + 1);
            //string strPath = "VAGraphic\\Img\\" + strFileName;//路径
            string strPath = "VAGraphic\\Img\\Units\\" + ViewState["UnitID"].ToString().Substring(ViewState["UnitID"].ToString().Length - 3)+".png";//路径
            this.FileUpload1.SaveAs(Server.MapPath(strPath));
            this.ShowTree();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "alert", "alert('上传成功。');", true);
            this.lblmap.Text = ViewState["UnitID"].ToString().Substring(ViewState["UnitID"].ToString().Length - 3) + ".png";
        }
        catch
        { }
    }
}
