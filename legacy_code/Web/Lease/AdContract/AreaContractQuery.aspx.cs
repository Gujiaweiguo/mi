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
using Base.DB;
using Base.Biz;
using Lease.AdContract;
using Lease.Customer;
using Base;
using BaseInfo.Dept;
using Base.Page;
public partial class Lease_AdContract_AreaContractQuery : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        //{
        //    showtreenode("AdContractStatus" + );
        //}
    }
    protected void rbtnNoLeaseOut_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void rbtnLeaseOut_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void treeClick_Click(object sender, EventArgs e)
    {

    }
    private void showtreenode(string areaContractstatus)
    {
        string jsdept = "";
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        Dept dept = new Dept();

        baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL;

        rs = baseBO.Query(dept);

        if (rs.Count == 1)
        {
            dept = rs.Dequeue() as Dept;
            jsdept = dept.DeptID + "|" + "0" + "|" + dept.DeptName + "^";
        }
        else
        {
            return;
        }
        baseBO.WhereClause = areaContractstatus;
        rs = baseBO.Query(new AreaContract());

        if (rs.Count > 0)
        {
            foreach (AreaContract areaContract in rs)
            {
                jsdept += areaContract.AdContractID + "|" + dept.DeptID + "|" + areaContract.AdDesc + "^";
            }
        }
        depttxt.Value = jsdept;
    }
}