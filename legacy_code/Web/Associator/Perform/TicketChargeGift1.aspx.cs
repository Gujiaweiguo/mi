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

public partial class Associator_Perform_TicketChargeGift : System.Web.UI.Page
{
    public string ticketChargeGift;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ticketChargeGift = (String)GetGlobalResourceObject("BaseInfo", "Associator_TicketChargeGift");
        }
    }
}
