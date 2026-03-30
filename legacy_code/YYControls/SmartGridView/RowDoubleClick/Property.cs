using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Web.UI;

namespace YYControls
{
    /// <summary>
    /// SmartGridView类的属性部分
    /// </summary>
    public partial class SmartGridView
    {
        private string _boundRowDoubleClickCommandName;
        /// <summary>
        /// 行的双击事件需要绑定的CommandName
        /// </summary>
        [
        Browsable(true),
        Description("行的双击事件需要绑定的CommandName"),
        Category("扩展")
        ]
        public virtual string BoundRowDoubleClickCommandName
        {
            get { return _boundRowDoubleClickCommandName; }
            set { _boundRowDoubleClickCommandName = value; }
        }
    }
}
