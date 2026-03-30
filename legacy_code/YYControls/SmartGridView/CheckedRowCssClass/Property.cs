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
        private CheckedRowCssClass _checkedRowCssClass;
        /// <summary>
        /// 行的指定复选框选中行时行的 CSS 类名
        /// </summary>
        [
        Description("行的指定复选框选中行时行的 CSS 类名"),
        Category("扩展"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual CheckedRowCssClass CheckedRowCssClass
        {
            get
            {
                if (this._checkedRowCssClass == null)
                {
                    this._checkedRowCssClass = new CheckedRowCssClass();
                }
                return this._checkedRowCssClass;
            }
            set { this._checkedRowCssClass = value; }
        }
    }
}
