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

using Base.Page;
using WorkFlow.WrkFlw;
using WorkFlow.WrkFlwRpt;
using BaseInfo.User;
public partial class WorkFlowEntity_WrkFlwTrack : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            GrdVewConfirm.DataSource = WrkFlwRptBO.GetWrkFlwEntityInfoNormal_Completed(sessionUser.RoleID, WrkFlwEntity.NODE_STATUS_WRKFLW_NORMAL_COMPLETED);
            GrdVewConfirm.DataBind();

            GridView1.DataSource = WrkFlwRptBO.GetWrkFlwEntityInfoNormal_Completed(sessionUser.RoleID, WrkFlwEntity.NODE_STATUS_REJECT_PENDING);
            GridView1.DataBind();

            GrdVewOvertime.DataSource = WrkFlwRptBO.GetWrkFlwEntityInfoOvertime(sessionUser.RoleID);
            GrdVewOvertime.DataBind();

            GrdVewDisposaling.DataSource = WrkFlwRptBO.GetWrkFlwEntityInfoDisposaling(sessionUser.RoleID);
            GrdVewDisposaling.DataBind();
        }
    }
}
