using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Web.UI;

namespace YYControls.SmartGridViewAlpha
{
    /// <summary>
    /// CheckboxAll 的摘要说明。
    /// </summary>
    [ToolboxItem(false)]
    public class CheckboxAll
    {
        private string _checkboxAllID;
        /// <summary>
        /// 模板列全选复选框ID
        /// </summary>
        public string CheckboxAllID
        {
            get { return _checkboxAllID; }
            set { _checkboxAllID = value; }
        }

        private string _checkboxItemID;
        /// <summary>
        /// 模板列项复选框ID
        /// </summary>
        public string CheckboxItemID
        {
            get { return _checkboxItemID; }
            set { _checkboxItemID = value; }
        }
    }
}
