using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Web.UI;

namespace YYControls
{
    /// <summary>
    /// 联动复选框实体类
    /// </summary>
    [ToolboxItem(false)]
    public class CascadeCheckbox
    {
        private string _parentCheckboxID;
        /// <summary>
        /// 模板列中 父复选框ID
        /// </summary>
        public string ParentCheckboxID
        {
            get { return _parentCheckboxID; }
            set { _parentCheckboxID = value; }
        }

        private string _childCheckboxID;
        /// <summary>
        /// 模板列中 子复选框ID
        /// </summary>
        public string ChildCheckboxID
        {
            get { return _childCheckboxID; }
            set { _childCheckboxID = value; }
        }
    }
}
