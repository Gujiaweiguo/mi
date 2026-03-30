using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WorkFlow.WrkFlw;
using Base.Biz;
using BaseInfo.Role;
using Base;
using Base.DB;
using BaseInfo.Func;
using Base.Page;
public partial class WorkFlow_WrkFlwDelete : Page
{
    Resultset rs = new Resultset();
    BaseBO baseBO = new BaseBO();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            rs = baseBO.Query(new BizGrp());
            foreach (BizGrp bizGrps in rs)
            {
                cmbBizGrpID.Items.Add(new ListItem(bizGrps.BizGrpName, bizGrps.BizGrpID.ToString()));
            }

            baseBO.WhereClause = "RoleStatus = 1";
            rs = baseBO.Query(new Role());
            foreach (Role roles in rs)
            {
                cmbNodeRoleID.Items.Add(new ListItem(roles.RoleName, roles.RoleID.ToString()));
            }

            baseBO.WhereClause = "FuncStatus = 1";
            rs = baseBO.Query(new BaseFunc());
            foreach (BaseFunc baseFuncs in rs)
            {
                cmbNodeFuncID.Items.Add(new ListItem(baseFuncs.FuncName, baseFuncs.FuncID.ToString()));
            }
            //工作流状态

            int[] status = WrkFlw.GetWrkFlwStatus();
            for (int i = 0; i < status.Length; i++)
            {
                cmbWrkFlwStatus.Items.Add(new ListItem(WrkFlw.GetWrkFlwStatusDesc(status[i]), status[i].ToString()));
            }

            //首节点是否制作单据

            int[] vouche = WrkFlw.GetInitVoucher();
            for (int i = 0; i < vouche.Length; i++)
            {
                cmbInitVoucher.Items.Add(new ListItem(WrkFlw.GetInitVoucherDesc(vouche[i]), vouche[i].ToString()));
            }

            //是否可送上级审批

            int[] smtToMgr = WrkFlwNode.GetSmtToMgr();
            for (int i = 0; i < smtToMgr.Length; i++)
            {
                cmbNodeSmtToMgr.Items.Add(new ListItem(WrkFlwNode.GetSmtToMgrDesc(smtToMgr[i]), smtToMgr[i].ToString()));
            }

            //确认后数据是否生效

            int[] validAfterConfirm = WrkFlwNode.GetValidAfterConfirm();
            for (int i = 0; i < validAfterConfirm.Length; i++)
            {
                cmbNodeValidAfterConfirm.Items.Add(new ListItem(WrkFlwNode.GetValidAfterConfirmDesc(validAfterConfirm[i]), validAfterConfirm[i].ToString()));
            }

            //确认后数据是否可打印
            int[] printAfterConfirm = WrkFlwNode.GetPrintAfterConfirm();
            for (int i = 0; i < printAfterConfirm.Length; i++)
            {
                cmbNodePrintAfterConfirm.Items.Add(new ListItem(WrkFlwNode.GetPrintAfterConfirmDesc(printAfterConfirm[i]), printAfterConfirm[i].ToString()));
            }

            //超时后自动处理

            int[] timeoutHandler = WrkFlwNode.GetTimeoutHandler();
            for (int i = 0; i < timeoutHandler.Length; i++)
            {
                cmbNodeTimeoutHandler.Items.Add(new ListItem(WrkFlwNode.GetTimeoutHandlerDesc(timeoutHandler[i]), timeoutHandler[i].ToString()));
            }

            //工作流是否转接

            int[] ifTransit = WrkFlw.GetIfTransit();
            for (int i = 0; i < ifTransit.Length; i++)
            {
                cmbTransit.Items.Add(new ListItem(WrkFlw.GetIfTransitDesc(ifTransit[i]), ifTransit[i].ToString()));
            }

            int[] voucherTypeID = WrkFlw.GetVoucherTypeID();
            for (int i = 0; i < voucherTypeID.Length; i++)
            {
                cmbVoucherTypeID.Items.Add(new ListItem(WrkFlw.GetVoucherTypeIDDesc(voucherTypeID[i]), voucherTypeID[i].ToString()));
            }


            baseBO.WhereClause = "WrkFlwID = " + Request.QueryString["WrkFlwID"].ToString();
            rs = baseBO.Query(new WrkFlw());
            if (rs.Count == 1)
            {
                WrkFlw wrkFlw = rs.Dequeue() as WrkFlw;
                cmbBizGrpID.SelectedValue = wrkFlw.BizGrpID.ToString();
                this.cmbVoucherTypeID.SelectedValue = wrkFlw.VoucherTypeID.ToString();
                txtWrkFlwName.Text = wrkFlw.WrkFlwName;
                cmbInitVoucher.SelectedValue = wrkFlw.InitVoucher.ToString();
                txtEfficiency.Text = wrkFlw.Efficiency.ToString();
                txtTraceDays.Text = wrkFlw.TraceDays.ToString();
                cmbWrkFlwStatus.SelectedValue = wrkFlw.WrkFlwStatus.ToString();
                cmbTransit.SelectedValue = wrkFlw.IfTransit.ToString();
                txtWrkFlwProcessClass.Text = wrkFlw.ProcessClass;
            }

            baseBO.WhereClause = "WrkFlwID = " + Request.QueryString["WrkFlwID"].ToString();
            GridView1.DataSource = baseBO.Query(new WrkFlwNode());
            GridView1.DataBind();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(cmbWrkFlwStatus.SelectedValue) == WrkFlw.WRK_FLW__STATUS_VALID)
        {
            Response.Write("<script language=javascript>alert('此工作流为有效状态，不能删除!!');</script>");
            return;
        }

        int result = 0;
        BaseTrans trans = new BaseTrans();
        trans.BeginTrans();
        trans.WhereClause = "WrkFlwID=" + Request.QueryString["WrkFlwID"].ToString();
        result = trans.Delete(new WrkFlw());
        if (result < 1)
        {
            Response.Write("<script language=javascript>alert('操作失败!!');</script>");
            return;
        }
        trans.WhereClause = "WrkFlwID=" + Request.QueryString["WrkFlwID"].ToString();
        result = trans.Delete(new WrkFlwNode());
        if (result < 1)
        {
            Response.Write("<script language=javascript>alert('操作失败!!');</script>");
            return;
        }
        trans.Commit();
        Response.Redirect("~/BaseInfo/User/MainPage.aspx");
    }
}
