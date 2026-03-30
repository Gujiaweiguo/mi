using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PosSystem_PosSvrInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["getdatasql"] = "select store.storename,possvrinfo.possvrid,possvrinfo.PosSvrName,PosSvrInfo.IP,possvrinfo.UpdateTime from possvrinfo " +
                               "inner join store on store.storeid=possvrinfo.storeid";
    }
}
