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

public partial class Associator_Perform_AssociatorInfo : System.Web.UI.Page
{

    #region
    public string integral;  //积分
    public string IssueCard;  //发行新卡
    public string Info;   //信息
    public string Query;  //查询
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            integral = (String)GetGlobalResourceObject("BaseInfo", "Tab_integral");
            IssueCard = (String)GetGlobalResourceObject("BaseInfo", "Tab_IssueCard");
            Info = (String)GetGlobalResourceObject("BaseInfo", "Tab_Info");
            Query = (String)GetGlobalResourceObject("BaseInfo", "Tab_Query"); 
        }

    }
}
