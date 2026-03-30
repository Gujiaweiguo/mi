using System;
using System.Collections.Generic;
using System.Text;

using System.Web.UI.WebControls;
using System.Web.UI;

namespace YYControls.Helper
{
    /// <summary>
    /// 常用Helper
    /// </summary>
    public partial class Common
    {
        /// <summary>
        /// 获得某个控件的子控件的ID的前缀
        /// </summary>
        /// <param name="control">控件（本方法用于获得该控件的子控件的ID的前缀）</param>
        /// <returns></returns>
        public static string GetChildControlPrefix(Control control)
        {
            if (control.NamingContainer.Parent == null)
            {
                return control.ID;
            }
            else
            {
                return String.Format("{0}_{1}", control.NamingContainer.ClientID, control.ID);
            }
        }
    }
}
