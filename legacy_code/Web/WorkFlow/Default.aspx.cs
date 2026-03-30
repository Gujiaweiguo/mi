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
using WorkFlow.WrkFlw;
using Base.Biz;
using BaseInfo.Role;
using Base;
using Base.DB;
using BaseInfo.Func;
public partial class WorkFlow_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            BaseBO baseBO = new BaseBO();
            DataSet objDataSet = new DataSet();
            baseBO.WhereClause = " BizGrpStatus = " + BizGrp.BIZ_GRP_STATUS_VALID;
            objDataSet = baseBO.QueryDataSet(new BizGrp());

            cmbBizGrpID.DataSource = objDataSet;
            cmbBizGrpID.DataValueField = "BizGrpID";
            cmbBizGrpID.DataTextField = "BizGrpName";
            cmbBizGrpID.DataBind();

            baseBO.WhereClause = "RoleStatus = 1";
            objDataSet = baseBO.QueryDataSet(new Role());
            cmbNodeRoleID.DataSource = objDataSet.Tables[0].DefaultView;
            cmbNodeRoleID.DataValueField = "RoleID";
            cmbNodeRoleID.DataTextField = "RoleName";
            cmbNodeRoleID.DataBind();

            baseBO.WhereClause = "";
            objDataSet = baseBO.QueryDataSet(new BaseFunc());
            cmbNodeFuncID.DataSource = objDataSet.Tables[0].DefaultView;
            cmbNodeFuncID.DataValueField = "FuncID";
            cmbNodeFuncID.DataTextField = "FuncName";
            cmbNodeFuncID.DataBind();
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
        }

    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtNodeProcessClass.Text == "")
        {
            if (Convert.ToInt32(cmbNodeValidAfterConfirm.SelectedItem.Value) ==  WrkFlwNode.VALID_AFTER_CONFIRM_YES || Convert.ToInt32(cmbNodePrintAfterConfirm.SelectedItem.Value) == WrkFlwNode.PRINT_AFTER_CONFIRM_YES)
            {
                Response.Write("<script language=javascript>alert('必须填写处理接口类名称!!');</script>");
                return;
            }
        }
        

        BaseTrans trans = new BaseTrans();
        WrkFlw objWrkFlw = new WrkFlw();
        int result = 0;
        if (Session["WrkFlwID"] == null)
        {
            objWrkFlw.WrkFlwID = BaseApp.GetWrkFlwID();
            Session["WrkFlwID"] = objWrkFlw.WrkFlwID;
            objWrkFlw.BizGrpID = Convert.ToInt32(cmbBizGrpID.SelectedValue);
            objWrkFlw.VoucherTypeID = Convert.ToInt32(txtVoucherTypeID.Text);
            objWrkFlw.WrkFlwName = txtWrkFlwName.Text.Trim();
            objWrkFlw.InitVoucher = Convert.ToInt32(cmbInitVoucher.SelectedValue);
            objWrkFlw.Efficiency = Convert.ToInt32(txtEfficiency.Text.Trim());
            objWrkFlw.TraceDays = Convert.ToInt32(txtTraceDays.Text.Trim());
            objWrkFlw.WrkFlwStatus = Convert.ToInt32(cmbWrkFlwStatus.SelectedValue);
            objWrkFlw.ProcessClass = txtWrkFlwProcessClass.Text.Trim();

            WrkFlwNode objWrkFlwNode = new WrkFlwNode();
            objWrkFlwNode.WrkFlwID = objWrkFlw.WrkFlwID;
            objWrkFlwNode.FuncID = Convert.ToInt32(cmbNodeFuncID.SelectedValue);
            objWrkFlwNode.RoleID = Convert.ToInt32(cmbNodeRoleID.SelectedValue);
            objWrkFlwNode.NodeName = txtNodeNodeName.Text.Trim();
            objWrkFlwNode.WrkStep = Convert.ToInt32(txtNodeWrkStep.Text.Trim());
            objWrkFlwNode.SmtToMgr = Convert.ToInt32(cmbNodeSmtToMgr.SelectedValue);
            objWrkFlwNode.ValidAfterConfirm = Convert.ToInt32(cmbNodeValidAfterConfirm.SelectedValue);
            objWrkFlwNode.PrintAfterConfirm = Convert.ToInt32(cmbNodePrintAfterConfirm.SelectedValue);
            objWrkFlwNode.LongestDelay = Convert.ToInt32(txtNodeLongestDelay.Text.Trim());
            objWrkFlwNode.TimeoutHandler = Convert.ToInt32(cmbNodeTimeoutHandler.SelectedValue);
            objWrkFlwNode.ProcessClass = txtNodeProcessClass.Text.Trim();
            objWrkFlwNode.ImageURL = "";
            trans.BeginTrans();
            trans.Insert(objWrkFlw);
            trans.Insert(objWrkFlwNode);
            trans.Commit();
            Response.Write("<script language=javascript>alert('操作成功!!');</script>");
            WrkFlwNodeQuery();
        }
        else
        {
            WrkFlwNode objWrkFlwNode = new WrkFlwNode();
            objWrkFlwNode.WrkFlwID =Convert.ToInt32(Session["WrkFlwID"]);
            objWrkFlwNode.FuncID = Convert.ToInt32(cmbNodeFuncID.SelectedValue);
            objWrkFlwNode.RoleID = Convert.ToInt32(cmbNodeRoleID.SelectedValue);
            objWrkFlwNode.NodeName = txtNodeNodeName.Text.Trim();
            objWrkFlwNode.WrkStep = Convert.ToInt32(txtNodeWrkStep.Text.Trim());
            objWrkFlwNode.SmtToMgr = Convert.ToInt32(cmbNodeSmtToMgr.SelectedValue);
            objWrkFlwNode.ValidAfterConfirm = Convert.ToInt32(cmbNodeValidAfterConfirm.SelectedValue);
            objWrkFlwNode.PrintAfterConfirm = Convert.ToInt32(cmbNodePrintAfterConfirm.SelectedValue);
            objWrkFlwNode.LongestDelay = Convert.ToInt32(txtNodeLongestDelay.Text.Trim());
            objWrkFlwNode.TimeoutHandler = Convert.ToInt32(cmbNodeTimeoutHandler.SelectedValue);
            objWrkFlwNode.ProcessClass = txtNodeProcessClass.Text.Trim();
            objWrkFlwNode.ImageURL = "";
            result = new BaseBO().Insert(objWrkFlwNode);

            if (result != -1)
            {
                Response.Write("<script language=javascript>alert('操作成功!!');</script>");
            }
            else
            {
                Response.Write("<script language=javascript>alert('操作失败!!');</script>");
            }

            WrkFlwNodeQuery();
        }

    }
    private void WrkFlwNodeQuery()
    {
        BaseBO baseBO = new BaseBO();
        DataSet objDataSet = new DataSet();
        baseBO.WhereClause = " WrkFlwID = " + Session["WrkFlwID"];
        objDataSet = baseBO.QueryDataSet(new WrkFlwNode());
        GridView1.DataSource = objDataSet;
        GridView1.DataBind();
    }
}
