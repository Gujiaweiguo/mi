using System;
using System.Data;
using System.Web.UI;

using Base.Biz;
using Base.DB;
using Base.Page;
using Sell;
using Lease.ConShop;
using BaseInfo.User;
using System.Web.UI.WebControls;


public partial class Sell_SkuInfo :  BasePage 
{
    #region 定义
    public string baseInfo;
    public static int strUserCode;
    public static string strUserName;
    #endregion
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        if (!this.IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_SkuInfo");
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            strUserCode = objSessionUser.UserID ;
            strUserName = objSessionUser.UserName  ;
            //txtShopCode.Attributes.Add("onclick", "ShowTree(LinkButton1)");
            txtQuery.Attributes.Add("onclick", "ShowShopTree(LinkButton2)");
            //BindDropList();
            InitTxt();

            this.SkuStatus.Items.Clear();
            this.SkuStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "SkuInfo_SkuYes"), "V"));
            this.SkuStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "SkuInfo_SkuNo"), "T"));

            txtBrand.Attributes.Add("onclick", "selectShopBrand()");
        }
        btnSave.Attributes.Add("onclick","return check()");
        btnEdit.Attributes.Add("onclick","return check()");
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        BaseTrans baseTrans = new BaseTrans();
        BaseBO baseBO = new BaseBO();

        string skuID = "";
        baseTrans.BeginTrans();
        //DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        SkuMaster skuMaster = new SkuMaster();


            if (!ExistColumn("SkuId", "SkuMaster", txtSkuId.Text))  //判断是否存在skuid
            {
                if (txtSkuId.Text.Trim().Length == 3)
                {
                    skuID = txtSkuId.Text.Trim();
                }
                    //if(ddlDeptId.Text=="")
                    //{
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '请选择商品分类'", true);
                    //    return;
                    //}
                    //if (txtBrand.Text == "")
                    //{
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '请选择品牌'", true);
                    //    return;
                    //}

                if(txtSkuId.Text.Trim().Length != 3)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '编码后缀为3位数字'", true);
                    return;
                }
            }
            else
            {
                txtSkuId.Text = "";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Exist") + "'", true);
                return;
            }
            //商品编码
            skuMaster.TenantId = ViewState["shopID"].ToString().Trim();
            skuMaster.SkuId = ViewState["shopID"].ToString().Trim()+skuID;//txtSkuId.Text.Trim();
            skuMaster.SkuDesc = txtSkuDesc.Text.Trim();
            skuMaster.UnitPrice = Convert.ToDecimal(txtPrice.Text.Trim());

            ds1=baseBO.QueryDataSet("select classid from skuclass where classname='"+ddlDeptId.Text.Trim()+"'");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                skuMaster.DeptId = ds1.Tables[0].Rows[0][0].ToString().Trim();
            }
            else
            {
                skuMaster.DeptId = "0";
            }
            skuMaster.CatgId = "0";
            skuMaster.ClassId = "0";
            skuMaster.Pcode = txtPcode.Text.Trim();
            ds1 = baseBO.QueryDataSet("select brandid from conshopbrand where brandname='" + txtBrand.Text.Trim() + "'");
            if (ds1.Tables[0].Rows.Count == 1)
            { skuMaster.Brand = ds1.Tables[0].Rows[0][0].ToString(); }
            else
            { skuMaster.Brand = "0"; }
            
            skuMaster.Spec = txtSpec.Text.Trim();
            skuMaster.color = txtColor.Text.Trim();
            skuMaster.Unit = txtUnit.Text.Trim();
            skuMaster.Produce = txtProduct.Text.Trim();
            skuMaster.Level = txtLevel.Text.Trim();
            skuMaster.Status = this.SkuStatus.SelectedValue.ToString() ;
            if (this.SkuStatus.SelectedItem.Text == "无效")
            {
                skuMaster.Deletetime = DateTime.Now;
            }

            skuMaster.DataSource = "M";
            skuMaster.IsClassCode = "N";
            skuMaster.EntryAt = DateTime.Now;
            skuMaster.EntryBy = strUserCode.ToString();
            skuMaster.ModifyAt = DateTime.Now;
            skuMaster.ModifyBy = strUserCode.ToString();
            skuMaster.Component = txtComponent.Text.Trim();
            skuMaster.dPriceMin = Convert.ToDecimal(txtPriceMin.Text.Trim());
            skuMaster.dPriceMax = Convert.ToDecimal(txtPriceMax.Text.Trim());
            if (chkSkuLocked.Checked)
            {
                skuMaster.chLocked = SkuMaster.CHLOCKED_LOCK;
            }
            else
            {
                skuMaster.chLocked = SkuMaster.CHLOCKED_NOT_LOCK;
            }
            if (txtStock.Text != "")
            {
                skuMaster.dStock = Convert.ToDecimal(txtStock.Text.Trim());
            }
            else
            {
                skuMaster.dStock = 0;
            }
            if (RadoY.Checked)
            {
                skuMaster.isDiscountCode = "Y";
            }
            else
            {
                skuMaster.isDiscountCode = "N";
            }
            skuMaster.DiscountPcentRate = Convert.ToDecimal(txtPcent.Text.Trim());
            skuMaster.BonusGpPer = Convert.ToDecimal(txtBonusPre.Text.Trim());

            try
            {
                baseTrans.Insert(skuMaster);
                baseTrans.Commit();
                txtNULL();
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "     " + (String)GetGlobalResourceObject("BaseInfo", "Rpt_SkuId") + ":" + ViewState["shopID"].ToString().Trim()+skuID + "'", true);
            }
            catch (Exception ex)
            {
                baseTrans.Rollback();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }

        setRadio();
    }

    private void InitTxt()
    {
        txtPriceMin.Text = "0.01";
        txtPriceMax.Text = "999999";
        txtPcent.Text = "0";
        txtBonusPre.Text = "0";
        txtStock.Text = "0";
        txtPrice.Text = "0";
        RadoN.Checked = true;

    }

    private void txtNULL()
    {
        txtQuery.Text = "";
        this.txtShopCode.Text = "";
        txtSkuId.Text = "";
        txtSkuDesc.Text = "";
        ddlDeptId.Text = "";
        txtSpec.Text = "";
        txtPcode.Text = "";
        txtUnit.Text = "";
        txtColor.Text = "";
        txtProduct.Text = "";
        txtBrand.Text = "";
        txtLevel.Text = "";
        txtComponent.Text = "";
        txtPriceMin.Text = "0.01";
        txtPriceMax.Text = "999999";
        txtPcent.Text = "0";
        txtBonusPre.Text = "0";
        txtStock.Text = "0";
        txtPrice.Text = "0";
        RadoN.Checked = true;
        chkSkuLocked.Checked = false;
        //BindDropList();
        dropSkuID.Items.Clear();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        BaseTrans baseTrans = new BaseTrans();
        baseTrans.BeginTrans();
        SkuMaster skuMaster = new SkuMaster();

        if (txtShopCode.Text != "" && txtSkuId.Text.ToString().Length==3)
        {
            //商品编码
            skuMaster.TenantId = ViewState["shopID"].ToString();
            skuMaster.SkuId = txtSkuId.Text.Trim();
            skuMaster.SkuDesc = txtSkuDesc.Text.Trim();
            skuMaster.UnitPrice = Convert.ToDecimal(txtPrice.Text.Trim());
            ds = baseBo.QueryDataSet("select classid from skuclass where classname='" + ddlDeptId.Text.Trim() + "'");
            if (ds.Tables[0].Rows.Count > 0)  //商品分类
            {
                skuMaster.DeptId = ds.Tables[0].Rows[0][0].ToString().Trim();
            }
            else
            {
                skuMaster.DeptId = "0";
            }
            skuMaster.CatgId = "0";
            skuMaster.ClassId = "0";
            skuMaster.Pcode = txtPcode.Text.Trim();
            ds = baseBo.QueryDataSet("Select brandID from conshopbrand where brandname ='" + txtBrand.Text.Trim() + "'");  //商品品牌
            if (ds.Tables[0].Rows.Count == 1)
            {
                skuMaster.Brand = ds.Tables[0].Rows[0][0].ToString().Trim();
            }
            else
            {
                skuMaster.Brand = "0";
            }
            
            skuMaster.Spec = txtSpec.Text.Trim();
            skuMaster.color = txtColor.Text.Trim();
            skuMaster.Unit = txtUnit.Text.Trim();
            skuMaster.Produce = txtProduct.Text.Trim();
            skuMaster.Level = txtLevel.Text.Trim();
            skuMaster.Status = this.SkuStatus.SelectedValue.ToString();
            if (this.SkuStatus.SelectedItem.Text == "无效")
            {
                skuMaster.Deletetime = DateTime.Now;
            }
            
            skuMaster.DataSource = "M";
            skuMaster.IsClassCode = "N";
            skuMaster.EntryAt = DateTime.Now;
            skuMaster.EntryBy = strUserCode.ToString();
            skuMaster.ModifyAt = DateTime.Now;
            skuMaster.ModifyBy = strUserCode.ToString();
            skuMaster.Component = txtComponent.Text.Trim();
            skuMaster.dPriceMin = Convert.ToDecimal(txtPriceMin.Text.Trim());
            skuMaster.dPriceMax = Convert.ToDecimal(txtPriceMax.Text.Trim());

            if (chkSkuLocked.Checked)
            {
                skuMaster.chLocked = SkuMaster.CHLOCKED_LOCK;
            }
            else
            {
                skuMaster.chLocked = SkuMaster.CHLOCKED_NOT_LOCK;
            }

            if (txtStock.Text != "")
            {
                skuMaster.dStock = Convert.ToDecimal(txtStock.Text.Trim());
            }
            else
            {
                skuMaster.dStock = 0;
            }
            if (RadoY.Checked)
            {
                skuMaster.isDiscountCode = "Y";
            }
            else
            {
                skuMaster.isDiscountCode = "N";
            }
            skuMaster.DiscountPcentRate = Convert.ToDecimal(txtPcent.Text.Trim());
            skuMaster.BonusGpPer = Convert.ToDecimal(txtBonusPre.Text.Trim());
            try
            {
                baseTrans.WhereClause = "SkuId='"+ViewState["shopID"].ToString().Trim()+txtSkuId.Text.ToString().Trim()+ "'";
                baseTrans.Update(skuMaster);
                baseTrans.Commit();
                txtNULL();
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            }
            catch (Exception ex)
            {
                baseTrans.Rollback();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "BankCard_NotSelect") + "'", true);
            txtShopCode.Text = "";
        }
        txtSkuId.Enabled = true;
        setRadio();
    }
    protected void BtnDel_Click(object sender, EventArgs e)
    {
        SkuMaster skuMaster = new SkuMaster();
        BaseBO baseBO = new BaseBO();

        if (txtSkuId.Text != "")
        {
            try
            {
                string sql = "Update SkuMaster Set EntryBy =" + strUserCode.ToString() + ",Status='T',chLocked=1,Deletetime='" + Convert.ToDateTime("1900-1-1") + "' Where SkuId='" + ViewState["shopID"].ToString().Trim() + txtSkuId.Text.Trim() + "'";
                if (baseBO.ExecuteUpdate(sql) == -1)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                    return;
                }
                txtNULL();
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "BankCard_NotSelect") + "'", true);
        }
        txtSkuId.Enabled = true;
        setRadio();
    }
    protected void btnQuit_Click(object sender, EventArgs e)
    {
        txtNULL();
        txtSkuId.Enabled = true;
        btnSave.Enabled = true;
        setRadio();
    }

    private void Query(string skuID)
    {
        BaseBO baseBO = new BaseBO();
        DataTable tempTb = new DataTable();

        string sql = "SELECT TenantId,conshop.ShopCode,conshop.ShopName,SkuId,SkuDesc,UnitPrice,UnitPrice2,DeptId,skuclass.classname as DeptName,brandname," +
                    "CatgId,SkuMaster.ClassId,Pcode,Brand,Spec,color,Unit,Produce,Level,Status,DataSource,IsClassCode,EntryAt,EntryBy,ModifyAt,ModifyBy,Component,dPriceMin,dPriceMax,dStock,Deletetime,chLocked,isDiscountCode,DiscountPcentRate,BonusGpPer "+
                    "FROM SkuMaster inner join Conshop on (conshop.shopid=skumaster.TenantId) left join skuclass on (skuclass.classid=skumaster.deptid) left join conshopbrand on (SkuMaster.brand=conshopbrand.brandid)" +
                    " where SkuID='" + skuID + "'";

        Resultset rs = new Resultset();
        DataSet ds=new DataSet();
        tempTb = baseBO.QueryDataSet(sql).Tables[0];

        if (!tempTb.Rows.Count.Equals(0))
        {
            this.InitTxt();

            ViewState["shopID"] = tempTb.Rows[0]["TenantId"].ToString().Trim();
            txtShopCode.Text = tempTb.Rows[0]["ShopCode"].ToString() + " " + tempTb.Rows[0]["ShopName"].ToString();
            int len = tempTb.Rows[0]["SkuId"].ToString().Trim().Length;
            txtSkuId.Text = tempTb.Rows[0]["SkuId"].ToString().Trim().Substring(len-3);
            txtSkuDesc.Text = tempTb.Rows[0]["SkuDesc"].ToString().Trim();
            ddlDeptId.Text = tempTb.Rows[0]["DeptName"].ToString().Trim();  //商品分类
            txtSpec.Text = tempTb.Rows[0]["Spec"].ToString().Trim();
            txtPcode.Text = tempTb.Rows[0]["Pcode"].ToString().Trim();
            txtUnit.Text = tempTb.Rows[0]["Unit"].ToString().Trim();
            txtColor.Text = tempTb.Rows[0]["color"].ToString().Trim();
            txtProduct.Text = tempTb.Rows[0]["Produce"].ToString().Trim();
            txtLevel.Text = tempTb.Rows[0]["Level"].ToString().Trim();
            txtComponent.Text = tempTb.Rows[0]["Component"].ToString().Trim();
            txtStock.Text = tempTb.Rows[0]["dStock"].ToString().Trim();
            txtPriceMin.Text = tempTb.Rows[0]["dPriceMin"].ToString().Trim();
            txtPriceMax.Text = tempTb.Rows[0]["dPriceMax"].ToString().Trim();
            txtPcent.Text = tempTb.Rows[0]["DiscountPcentRate"].ToString().Trim();
            txtBonusPre.Text = tempTb.Rows[0]["BonusGpPer"].ToString().Trim();
            txtPrice.Text = tempTb.Rows[0]["UnitPrice"].ToString().Trim();
            txtBrand.Text = tempTb.Rows[0]["brandname"].ToString().Trim();
            SkuStatus.SelectedValue = tempTb.Rows[0]["Status"].ToString().Trim();
            if (tempTb.Rows[0]["chLocked"].ToString().Trim() == SkuMaster.CHLOCKED_LOCK)
            {
                chkSkuLocked.Checked = true;
            }
            else if (tempTb.Rows[0]["chLocked"].ToString().Trim() == SkuMaster.CHLOCKED_NOT_LOCK)
            {
                chkSkuLocked.Checked = false;
            }

            if (tempTb.Rows[0]["isDiscountCode"].ToString() == "N")
            {
                RadoN.Checked = true;
                RadoY.Checked = false;
            }
            else
            {
                RadoY.Checked = true;
                RadoN.Checked = false;
            }
            if (tempTb.Rows[0]["Status"].Equals("T"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Rpt_SkuId") + (String)GetGlobalResourceObject("BaseInfo", "Associator_rabInvalid") + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ' '", true);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidNoData") + "'", true);
            txtNULL();
        }
    }

    private bool ExistColumn(string Column,string tableName,string ColumnValue)
    {
        BaseBO baseBO = new BaseBO();
        DataTable tempTb = new DataTable();
        string sql = "Select "+ Column + " FROM " +tableName+" WHERE "+Column+" ='" +ViewState["shopID"].ToString().Trim() + ColumnValue+ "'";
        tempTb = baseBO.QueryDataSet(sql).Tables[0];
        
        if (tempTb.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
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
    }
    protected void txtSpec_TextChanged(object sender, EventArgs e)
    {

    }

    private void setRadio()
    {
        RadoN.Checked = true;
        RadoY.Checked = false;
    }
    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        this.txtNULL();
        DataSet ds = ConShopPO.GetConShopByID(Convert.ToInt32(allvalue.Value));
        if (ds.Tables[0].Rows.Count == 1)
        {
            ViewState["shopID"] = ds.Tables[0].Rows[0]["ShopID"].ToString();
            txtQuery.Text = ds.Tables[0].Rows[0]["ShopCode"].ToString() + "(" + ds.Tables[0].Rows[0]["ShopName"].ToString() + ")";
            txtShopCode.Text = ds.Tables[0].Rows[0]["ShopName"].ToString();
            //GetShopBrand(ds.Tables[0].Rows[0]["BrandID"].ToString());

            BaseBO baseBO = new BaseBO();
            int a = Convert.ToInt32(ViewState["shopID"]);
           // baseBO.WhereClause = "TenantId = " + Convert.ToInt32(ViewState["shopID"]) + " and status='V'";
            baseBO.WhereClause = "TenantId = " + Convert.ToInt32(ViewState["shopID"]) ;
            Resultset rs = baseBO.Query(new SkuMaster());
            if (rs.Count > 0)
            {
                BindDropSkuID(rs);
                SkuMaster skuMaster = rs.Dequeue() as SkuMaster;
                Query(skuMaster.SkuId);
            }
        }
    }

    private void BindDropSkuID(Resultset rs)
    {
        dropSkuID.Items.Clear();
        foreach (SkuMaster skuMaster in rs)
        {
            dropSkuID.Items.Add(skuMaster.SkuId);
        }
    }

    protected void dropSkuID_SelectedIndexChanged(object sender, EventArgs e)
    {
        Query(dropSkuID.SelectedItem.Text);
    }
    protected void LinkButton3_Click(object sender, EventArgs e)
    {
        string s = brand.Value.ToString();
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = baseBo.QueryDataSet("select brandname from conshopbrand where brandid=" + Convert.ToInt32(s));
        if (ds.Tables[0].Rows.Count > 0)
        {
            //string[] ss = Regex.Split(s, ",");

            //string brandID = ss[0].ToString();
            //string brandName = ss[1].ToString();

            //if (brandID == "")
            //{
            //    return;
            //}
            ViewState["brandID"] = s;
            txtBrand.Text = ds.Tables[0].Rows[0][0].ToString();
        }
    }

    private void GetShopBrand(string shopBrandID)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "BrandID = " + shopBrandID;
        Resultset rs = baseBO.Query(new ConShopBrand());
        if (rs.Count == 1)
        {
            //ConShopBrand shopBrand = rs.Dequeue() as ConShopBrand;
            ////txtBrand.Text = shopBrand.BrandName;
            //ViewState["brandID"] = shopBrand.BrandId;
        }
        else
            txtBrand.Text = "";
    }
 
    protected void LinkButton4_Click(object sender, EventArgs e)
    {
        string s = allvalue.Value.ToString();
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        if (s.Length == 6)
        {
            ds = baseBo.QueryDataSet("select classname from skuclass where classid=" + Convert.ToInt32(s.Substring(3, 3)));
            if(ds.Tables[0].Rows.Count>0)
            {
                ddlDeptId.Text = ds.Tables[0].Rows[0][0].ToString();
            }
        }
        else
        {

        }
    }
 
}
