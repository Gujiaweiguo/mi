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
        private FixRowColumn _fixRowColumn;
        /// <summary>
        /// 固定指定行、指定列
        /// </summary>
        [
        Description("固定指定行、指定列"),
        Category("扩展"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual FixRowColumn FixRowColumn
        {
            get
            {
                if (this._fixRowColumn == null)
                {
                    this._fixRowColumn = new FixRowColumn();
                }
                return this._fixRowColumn;
            }
            set { this._fixRowColumn = value; }
        }
    }
}
