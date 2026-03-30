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
        private SmartSorting _smartSorting;
        /// <summary>
        /// 高级排序功能
        /// </summary>
        [
        Description("高级排序功能"),
        Category("扩展"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual SmartSorting SmartSorting
        {
            get
            {
                if (this._smartSorting == null)
                {
                    this._smartSorting = new SmartSorting();
                }
                return this._smartSorting;
            }
            set { this._smartSorting = value; }
        }
    }
}
