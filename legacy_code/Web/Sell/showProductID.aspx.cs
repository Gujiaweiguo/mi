using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Base.Page;
using RentableArea;
using Base.Biz;
using Base.DB;

public partial class Sell_showProductID : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ShowTree();
    }

    private void ShowTree()
    {
        //string jsdept = "";
        //jsdept += "100|0|商品分类|^101|100|商品分类1|^102|100|商品分类2|^";
        //depttxt.Value = jsdept;
        BaseBO baseBO = new BaseBO();
        string jsdept = "";
        //baseBO.WhereClause = "TradeStatus = " + TradeRelation.TRADERELATION_STATUS_VALID;
        baseBO.OrderBy = "ClassLevel,PClassID";
        Resultset rs = baseBO.Query(new SkuClass());
        jsdept = "100" + "|" + "0" + "|" + (String)GetGlobalResourceObject("BaseInfo", "RentableArea_SkuClass") + "^";
        if (rs.Count > 0)
        {
            foreach (SkuClass objSkuClass in rs)
            {
                if (objSkuClass.ClassLevel == 1)
                {
                    jsdept += objSkuClass.ClassID + "|" + "100" + "|" + objSkuClass.ClassName + "^";
                }
                else
                {
                    jsdept += objSkuClass.PClassID + objSkuClass.ClassID.ToString() + "|" + objSkuClass.PClassID + "|" + objSkuClass.ClassName + "^";
                }
            }
        }
        depttxt.Value = jsdept;
    }

    //protected void treeClick_Click(object sender, EventArgs e)
    //{
    //    ViewState["node"] = deptid.Value;
    //    BaseBO baseBo = new BaseBO();
    //    DataSet ds = new DataSet();
    //    if (ViewState["node"].ToString().Length == 6)
    //    {
    //        ds = baseBo.QueryDataSet("select classname from skuclass where classid=" + ViewState["node"].ToString().Substring(3, 3));
    //        if(ds.Tables[0].Rows.Count>)
    //        {
                
    //        }
    //    }
    //    else
    //    {
    //    }
    //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    //}
}
