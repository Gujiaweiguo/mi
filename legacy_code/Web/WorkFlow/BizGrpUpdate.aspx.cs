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
using WorkFlow.WrkFlw;
using Base.Page;
public partial class WorkFlow_BizGrpUpdate : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int[] status = BizGrp.GetBizGrpStatus();
            for (int i = 0; i < status.Length; i++)
            {
                cmbBizGrpStatus.Items.Add(new ListItem(BizGrp.GetBizGrpStatusDesc(status[i]), status[i].ToString()));
            }
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "BizGrpID = " + Request.QueryString["ID"].ToString();
            Resultset rs = baseBO.Query(new BizGrp());
            if (rs.Count == 1)
            {
                BizGrp bizGrp = rs.Dequeue() as BizGrp;
                txtBizGrpName.Text = bizGrp.BizGrpName.ToString();
                cmbBizGrpStatus.SelectedValue = bizGrp.BizGrpStatus.ToString();
            }
        }
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO(); 
        BizGrp objBizGrp = new BizGrp();
        int result = 0;
        baseBO.WhereClause = "BizGrpID = " + Request.QueryString["ID"].ToString();
        objBizGrp.BizGrpName = txtBizGrpName.Text;
        objBizGrp.BizGrpStatus = Convert.ToInt32(cmbBizGrpStatus.SelectedValue);
        result = baseBO.Update(objBizGrp);
        if (result < 1)
        {
            Response.Write("<script language=javascript>alert('修改未成功');</script>");
        }
        else
        {
            //Response.Write("<script language=javascript>alert('修改成功');</script>");
            Response.Redirect("BizGrp.aspx");
        }
        
    }
}
