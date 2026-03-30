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
using BaseInfo.Dept;
using System.Xml;
using System.Text;
using Base.Page;
using WorkFlow.WorkFlowMail;

public partial class WorkFlow_Default : BasePage
{
    Resultset rs = new Resultset();
    BaseBO baseBO = new BaseBO();
    int numCount = 0;
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnSave.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnCancel.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnBlankOuting.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnCancel.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnBlankOut.gif) no-repeat left top';this.style.fontWeight='normal';");
        btnSave.Attributes.Add("onclick", "endNodeGetActionXML()");
        txtEfficiency.Attributes.Add("onblur", "textleave()");
        txtTraceDays.Attributes.Add("onblur", "textleave()");
        txtNodeWrkStep.Attributes.Add("onblur", "textleave()");
        txtNodeLongestDelay.Attributes.Add("onblur", "textleave()");
        if (!IsPostBack)
        {
            List<WrkFlwNode> wrkFlwNodes = (List<WrkFlwNode>)Session["WrkFlwNodes"];
            rs = baseBO.Query(new QueryBizGrp());
            foreach (QueryBizGrp bizGrps in rs)
            {
                cmbBizGrpID.Items.Add(new ListItem(bizGrps.BizGrpName, bizGrps.BizGrpID.ToString()));
            }


            /*功能选择列表*/
            baseBO.WhereClause = "FuncStatus =" + BaseFunc.FUNC_STATUS_VALID + " And FuncType=" + BaseFunc.FUNC_TYPE;
            rs = baseBO.Query(new BaseFunc());
            foreach (BaseFunc baseFuncs in rs)
            {
                cmbNodeFuncID.Items.Add(new ListItem(baseFuncs.FuncName, baseFuncs.FuncID.ToString()));
            }


            //角色
            baseBO.WhereClause = "RoleStatus =" + Role.IS_ROLESTATUS_YES;
            rs = baseBO.Query(new Role());
            cmbNodeRoleID.Text = "";
            //cmbNodeRoleID.ClearSelection = false;
            foreach (Role roles in rs)
            {
                cmbNodeRoleID.Items.Add(new ListItem(roles.RoleName, roles.RoleID.ToString()));

            }


            //邮件模板
            baseBO.WhereClause = "MailStatus =" + WrkFlwMail.WorkFlowMail_YES;
            rs = baseBO.Query(new WrkFlwMail());
            cmbMail.Text = "";

            //cmbNodeRoleID.ClearSelection = false;
            foreach (WrkFlwMail wrkFlwMail in rs)
            {
                cmbMail.Items.Add(new ListItem(wrkFlwMail.MailSubject, wrkFlwMail.WrkFlwMailID.ToString()));

            }

            //工作流状态


            int[] status = WrkFlw.GetWrkFlwStatus();
            for (int i = 0; i < status.Length; i++)
            {
                cmbWrkFlwStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("parameter", WrkFlw.GetWrkFlwStatusDesc(status[i])), status[i].ToString()));
            }

            //首节点是否制作单据


            int[] vouche = WrkFlw.GetInitVoucher();
            for (int i = 0; i < vouche.Length; i++)
            {
                cmbInitVoucher.Items.Add(new ListItem((String)GetGlobalResourceObject("parameter", WrkFlw.GetIfYESorNODesc(vouche[i])), vouche[i].ToString()));
            }

            //是否可送上级审批


            int[] smtToMgr = WrkFlwNode.GetSmtToMgr();
            for (int i = 0; i < smtToMgr.Length; i++)
            {
                cmbNodeSmtToMgr.Items.Add(new ListItem((String)GetGlobalResourceObject("parameter", WrkFlwNode.GetSmtToMgrDesc(smtToMgr[i])), smtToMgr[i].ToString()));
            }

            //确认后数据是否生效


            int[] validAfterConfirm = WrkFlwNode.GetValidAfterConfirm();
            for (int i = 0; i < validAfterConfirm.Length; i++)
            {
                cmbNodeValidAfterConfirm.Items.Add(new ListItem((String)GetGlobalResourceObject("parameter", WrkFlwNode.GetValidAfterConfirmDesc(validAfterConfirm[i])), validAfterConfirm[i].ToString()));
            }

            //确认后数据是否可打印
            int[] printAfterConfirm = WrkFlwNode.GetPrintAfterConfirm();
            for (int i = 0; i < printAfterConfirm.Length; i++)
            {
                cmbNodePrintAfterConfirm.Items.Add(new ListItem((String)GetGlobalResourceObject("parameter", WrkFlwNode.GetPrintAfterConfirmDesc(printAfterConfirm[i])), printAfterConfirm[i].ToString()));
            }

            //超时后自动处理


            int[] timeoutHandler = WrkFlwNode.GetTimeoutHandler();
            for (int i = 0; i < timeoutHandler.Length; i++)
            {
                cmbNodeTimeoutHandler.Items.Add(new ListItem((String)GetGlobalResourceObject("parameter", WrkFlwNode.GetTimeoutHandlerDesc(timeoutHandler[i])), timeoutHandler[i].ToString()));
            }

            //工作流是否转接


            int[] ifTransit = WrkFlw.GetIfTransit();
            for (int i = 0; i < ifTransit.Length; i++)
            {
                cmbTransit.Items.Add(new ListItem((String)GetGlobalResourceObject("parameter", WrkFlw.GetIfTransitDesc(ifTransit[i])), ifTransit[i].ToString()));
            }

            /*工作流类型*/
            int[] voucherTypeID = WrkFlw.GetVoucherTypeID();
            for (int i = 0; i < voucherTypeID.Length; i++)
            {
                cmbVoucherTypeID.Items.Add(new ListItem((String)GetGlobalResourceObject("parameter",WrkFlw.GetVoucherTypeIDDesc(voucherTypeID[i])), voucherTypeID[i].ToString()));
                cmbVoucherTypeID.SelectedIndex = -1;
            }

            ////showFunc();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "WrkFlw_lblWorkFlow");
        }

    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/WorkFlow/WrkFlw.aspx");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        byte[] bytes = null;
        btnSave.OnClientClick = "nodeend();";
        WrkFlwNode objWrkFlwNode = new WrkFlwNode();
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(TextBox1.Text);
        XmlElement element = doc.DocumentElement;
        XmlElement flowConfig = (XmlElement)doc.GetElementsByTagName("FlowConfig").Item(0);
        XmlElement steps = (XmlElement)doc.GetElementsByTagName("Steps").Item(0);
        //添加工作流


        BaseTrans trans = new BaseTrans();
        try
        {
            trans.BeginTrans();
            WrkFlw wrkFlw = new WrkFlw();
            wrkFlw.WrkFlwID = BaseApp.GetWrkFlwID();
            wrkFlw.BizGrpID = Convert.ToInt32(flowConfig.GetElementsByTagName("WrkFlw").Item(0).Attributes["bizGrpID"].Value);
            wrkFlw.VoucherTypeID = Convert.ToInt32(flowConfig.GetElementsByTagName("WrkFlw").Item(0).Attributes["voucherTypeID"].Value);
            wrkFlw.WrkFlwName = flowConfig.GetElementsByTagName("WrkFlw").Item(0).Attributes["wrkFlwName"].Value;
            wrkFlw.InitVoucher = Convert.ToInt32(flowConfig.GetElementsByTagName("WrkFlw").Item(0).Attributes["initVoucher"].Value);
            wrkFlw.Efficiency = Convert.ToInt32(flowConfig.GetElementsByTagName("WrkFlw").Item(0).Attributes["efficiency"].Value);
            wrkFlw.TraceDays = Convert.ToInt32(flowConfig.GetElementsByTagName("WrkFlw").Item(0).Attributes["traceDays"].Value);
            wrkFlw.WrkFlwStatus = Convert.ToInt32(flowConfig.GetElementsByTagName("WrkFlw").Item(0).Attributes["wrkFlwStatus"].Value);
            wrkFlw.ProcessClass = flowConfig.GetElementsByTagName("WrkFlw").Item(0).Attributes["wrkFlwProcessClass"].Value;
            bytes = Encoding.GetEncoding("utf-8").GetBytes(TextBox2.Text);
            wrkFlw.WorkFlowDrawing = Convert.ToBase64String(bytes);
            bytes = Encoding.GetEncoding("utf-8").GetBytes(TextBox1.Text);
            wrkFlw.WorkFlowNode = Convert.ToBase64String(bytes);

            if (trans.Insert(wrkFlw) < 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "message()", true);
                //Response.Write("<script language=javascript>parent.document.all.txtWroMessage.value ="+ hidInsert.Value +"</script>");
                trans.Rollback();
                return;
            }
            /*添加节点*/
            for (int i = 0; i < steps.ChildNodes.Count; i++)
            {
                XmlElement step = (XmlElement)steps.ChildNodes.Item(i);

                objWrkFlwNode.WrkFlwID = wrkFlw.WrkFlwID;
                objWrkFlwNode.NodeID = BaseApp.GetWrkFlwNodeID();
                objWrkFlwNode.FuncID = Convert.ToInt32(step.GetElementsByTagName("BaseProperties").Item(0).Attributes["nodeFuncID"].Value);
                objWrkFlwNode.RoleID = Convert.ToInt32(step.GetElementsByTagName("BaseProperties").Item(0).Attributes["nodeRoleID"].Value);
                objWrkFlwNode.NodeName = step.GetElementsByTagName("BaseProperties").Item(0).Attributes["nodeNodeName"].Value;
                objWrkFlwNode.WrkStep = Convert.ToInt32(step.GetElementsByTagName("BaseProperties").Item(0).Attributes["nodeWrkStep"].Value);
                objWrkFlwNode.SmtToMgr = Convert.ToInt32(step.GetElementsByTagName("BaseProperties").Item(0).Attributes["nodeSmtToMgr"].Value);
                objWrkFlwNode.ValidAfterConfirm = Convert.ToInt32(step.GetElementsByTagName("BaseProperties").Item(0).Attributes["nodeValidAfterConfirm"].Value);
                objWrkFlwNode.PrintAfterConfirm = Convert.ToInt32(step.GetElementsByTagName("BaseProperties").Item(0).Attributes["nodePrintAfterConfirm"].Value);
                objWrkFlwNode.LongestDelay = Convert.ToInt32(step.GetElementsByTagName("BaseProperties").Item(0).Attributes["nodeLongestDelay"].Value);
                objWrkFlwNode.TimeoutHandler = Convert.ToInt32(step.GetElementsByTagName("BaseProperties").Item(0).Attributes["nodeTimeoutHandler"].Value);
                objWrkFlwNode.WrkFlwMailID = Convert.ToInt32(step.GetElementsByTagName("BaseProperties").Item(0).Attributes["mail"].Value);
                objWrkFlwNode.ProcessClass = step.GetElementsByTagName("BaseProperties").Item(0).Attributes["nodeProcessClass"].Value;
                objWrkFlwNode.ImageURL = "";

                if (trans.Insert(objWrkFlwNode) < 1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "message()", true);
                    trans.Rollback();
                    return;
                }

            }
            trans.Commit();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "hidAdd()", true);
            txtEfficiency.Text = "";
            txtNodeLongestDelay.Text = "";
            txtNodeProcessClass.Text = "";
            txtNodeWrkStep.Text = "";
            txtTraceDays.Text = "";
            txtWrkFlwName.Text = "";
            txtWrkFlwProcessClass.Text = "";
            txtNodeNodeName.Text = "";
        }
        catch (Exception ex)
        {
            trans.Rollback();
        }

    }
    //protected void cmbBizGrpID_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //    showFunc();
    //}

    //protected void showFunc()
    //{
    //    //业务处理
    //    cmbNodeFuncID.Items.Clear();
    //    baseBO.WhereClause = "FuncStatus = " + BaseFunc.FUNC_STATUS_VALID + " and BizGrpID=" + Convert.ToInt32(cmbBizGrpID.SelectedValue);
    //    rs = baseBO.Query(new BaseFunc());
    //    foreach (BaseFunc baseFuncs in rs)
    //    {
    //        //cmbNodeFuncID.Items.Add(new ListItem(baseFuncs.FuncName, baseFuncs.FuncID.ToString()));
    //        cmbNodeFuncID.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo",baseFuncs.BaseInfo), baseFuncs.FuncID.ToString()));
    //        cmbNodeFuncID.SelectedIndex = -1;
    //    }
    //}
}
