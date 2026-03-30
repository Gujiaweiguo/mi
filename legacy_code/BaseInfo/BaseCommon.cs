using Base.Biz;
using Base.DB;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

namespace BaseInfo
{
    /// <summary>
    /// 公共方法类 Add by lcp at 2009-3-19
    /// </summary>
    public class BaseCommon
    {
        /// <summary>
        /// 绑定DropDownList
        /// </summary>
        /// <param name="strSql">sql 语句</param>
        /// <param name="strDataValueField">value</param>
        /// <param name="strDataTextField">text</param>
        /// <param name="ddl">DropDownList</param>
        public static void BindDropDownList(string strSql, string strDataValueField, string strDataTextField, DropDownList ddl)
        {
            BaseBO objBaseBo = new BaseBO();
            DataSet ds = objBaseBo.QueryDataSet(strSql);
            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow dr = ds.Tables[0].NewRow();
                dr[strDataValueField] = "0";
                dr[strDataTextField] = "----";
                ds.Tables[0].Rows.Add(dr);
                ds.AcceptChanges();
                
            }
            ddl.DataValueField = strDataValueField;
            ddl.DataTextField = strDataTextField;
            ddl.DataSource = ds.Tables[0];
            ddl.DataBind();
        }
        /// <summary>
        /// 绑定DropDownList
        /// </summary>
        /// <param name="objBaseBo">Bo</param>
        /// <param name="objBasePo">Po</param>
        /// <param name="strDataValueField">value</param>
        /// <param name="strDataTextField">text</param>
        /// <param name="ddl">DropDownList</param>
        public static void BindDropDownList(BaseBO objBaseBo, BasePO objBasePo, string strDataValueField, string strDataTextField, DropDownList ddl)
        {
            DataSet ds = objBaseBo.QueryDataSet(objBasePo);
            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow dr = ds.Tables[0].NewRow();
                dr[strDataValueField] = "0";
                dr[strDataTextField] = "----";
                ds.Tables[0].Rows.Add(dr);
                ds.AcceptChanges();
            }
            ddl.DataValueField = strDataValueField;
            ddl.DataTextField = strDataTextField;
            ddl.DataSource = ds.Tables[0];
            ddl.DataBind();
        }
        /// <summary>
        /// 绑定GridView并完成分页
        /// </summary>
        /// <param name="objBaseBo">BaseBo</param>
        /// <param name="objBasePo">BasePo</param>
        /// <param name="iPageSize">每页显示记录数</param>
        /// <param name="iCurrentPage">当前页</param>
        /// <param name="btnBack">上页按钮</param>
        /// <param name="btnNext">下页按钮</param>
        /// <param name="GridView">GridView</param>
        public static void BindGridView(BaseBO objBaseBo, BasePO objBasePo, int iPageSize, int iCurrentPage, Button btnBack, Button btnNext,GridView Grd)
        {
            PagedDataSource pds = new PagedDataSource();
            DataTable dt = objBaseBo.QueryDataSet(objBasePo).Tables[0];
            pds.DataSource = dt.DefaultView;
            if (pds.Count < 1)
            {
                for (int i = 0; i < iPageSize; i++)
                {
                    dt.Rows.Add(dt.NewRow());
                }
                pds.DataSource = dt.DefaultView;
                Grd.DataSource = pds;
                Grd.DataBind();
            }
            else
            {
                Grd.EmptyDataText = "";
                pds.AllowPaging = true;
                pds.PageSize = iPageSize;
                pds.CurrentPageIndex = iCurrentPage - 1;
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
                Grd.DataSource = pds;
                Grd.DataBind();
                int spareRow = Grd.Rows.Count;
                for (int i = 0; i < pds.PageSize - spareRow; i++)
                {
                    dt.Rows.Add(dt.NewRow());
                }
                pds.DataSource = dt.DefaultView;
                Grd.DataSource = pds;
                Grd.DataBind();
            }
        }
        /// <summary>
        /// 绑定GridView
        /// </summary>
        /// <param name="objBaseBo">Bo</param>
        /// <param name="objBasePo">Po</param>
        /// <param name="Grd"></param>
        public static void BindGridView(BaseBO objBaseBo, BasePO objBasePo,GridView Grd)
        {
            int spareRow = 0;
            //PagedDataSource pds = new PagedDataSource();
            DataTable dt = objBaseBo.QueryDataSet(objBasePo).Tables[0];
            //pds.DataSource = dt.DefaultView;
            Grd.DataSource = dt;
            Grd.DataBind();
            spareRow = Grd.Rows.Count;
            for (int i = 0; i < Grd.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            Grd.DataSource = dt;
            Grd.DataBind();
        }
        /// <summary>
        /// 绑定GridView
        /// </summary>
        /// <param name="strSql">传入的sql</param>
        /// <param name="Grd"></param>
        public static void BindGridView(string strSql, GridView Grd)
        {
            int spareRow = 0;
            BaseBO objBaseBo = new BaseBO();
            DataTable dt = objBaseBo.QueryDataSet(strSql).Tables[0];
            Grd.DataSource = dt;
            Grd.DataBind();
            spareRow = Grd.Rows.Count;
            for (int i = 0; i < Grd.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            Grd.DataSource = dt;
            Grd.DataBind();
        }
        /// <summary>
        /// 根据传入的ID，得到Text的值（ID值需唯一）
        /// </summary>
        /// <param name="strText">要得到的Text值</param>
        /// <param name="strID">传入的ID</param>
        /// <param name="strTableName">表名</param>
        /// <param name="strIDValue">传入的ID条件</param>
        /// <returns></returns>
        public static string GetTextValueByID(string strText, string strID, string strTableName, string strIDValue)
        {
            BaseBO objBaseBo = new BaseBO();
            string strSql = "select " + strText + " from " + strTableName + " where " + strID + "='" + strIDValue + "'";
            DataSet ds = objBaseBo.QueryDataSet(strSql);
            string strReturn = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                strReturn = dr[0].ToString();
            }
            return strReturn;
        }
        /// <summary>
        /// 设置TextBox控件属性
        /// </summary>
        /// <param name="Ctrls"></param>
        public static void SetNumberTextBoxAttribute(TextBox[] Ctrls)
        {
            SetAttribute(Ctrls, "style", "TEXT-ALIGN:RIGHT;");
            SetAttribute(Ctrls, "onblur", "if(this.value==''){this.value='0';}");
            SetAttribute(Ctrls, "onkeydown", "return CheckKeyCode(this,event.keyCode)");
            SetAttribute(Ctrls, "onkeyup", "if(this.value==''){this.value='0';}");
            SetAttribute(Ctrls, "onpaste", "return false");
            SetAttribute(Ctrls, "onpaste", "return false");
            SetAttribute(Ctrls, "ondragenter", "return false");
        }
        /// <summary>
        /// 设置HtmlText属性
        /// </summary>
        /// <param name="Ctrls"></param>
        public static void SetNumberTextBoxAttribute(HtmlInputText[] Ctrls)
        {
            SetAttribute(Ctrls, "style", "TEXT-ALIGN:RIGHT;");
            SetAttribute(Ctrls, "onblur", "if(this.value==''){this.value='0';}");
            SetAttribute(Ctrls, "onkeydown", "return CheckKeyCode(this,event.keyCode)");
            SetAttribute(Ctrls, "onkeyup", "if(this.value==''){this.value='0';}");
            SetAttribute(Ctrls, "onpaste", "return false");
            SetAttribute(Ctrls, "onpaste", "return false");
            SetAttribute(Ctrls, "ondragenter", "return false");
        }
        /// <summary>
        /// 设置WebControl控件属性
        /// </summary>
        /// <param name="Ctrls"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public static void SetAttribute(WebControl[] Ctrls, string Key, string Value)
        {
            for (int i = 0; i < Ctrls.Length; i++)
            {
                if (Ctrls[i].Attributes[Key] == null)
                {
                    Ctrls[i].Attributes.Add(Key, Value);
                }
                else
                {
                    string Attr = "";
                    if (Ctrls[i].Attributes[Key].IndexOf(Value) == -1)
                    {
                        Attr = Ctrls[i].Attributes[Key] + ";" + Value;
                        Ctrls[i].Attributes.Add(Key, Attr);
                    }
                }
            }
        }
        /// <summary>
        /// 设置HtmlControl控件属性
        /// </summary>
        /// <param name="Ctrls"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public static void SetAttribute(HtmlControl[] Ctrls, string Key, string Value)
        {
            for (int i = 0; i < Ctrls.Length; i++)
            {
                if (Ctrls[i].Attributes[Key] == null)
                {
                    Ctrls[i].Attributes.Add(Key, Value);
                }
                else
                {
                    string Attr = "";
                    if (Ctrls[i].Attributes[Key].IndexOf(Value) == -1)
                    {
                        Attr = Ctrls[i].Attributes[Key] + ";" + Value;
                        Ctrls[i].Attributes.Add(Key, Attr);
                    }
                }
            }
        }


    }
}
