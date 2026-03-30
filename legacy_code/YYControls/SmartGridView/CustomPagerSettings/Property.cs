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
        private CustomPagerSettings _customPagerSettings;
        /// <summary>
        /// 自定义分页样式
        /// </summary>
        [
        Description("自定义分页样式"),
        Category("扩展"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual CustomPagerSettings CustomPagerSettings
        {
            get
            {
                if (this._customPagerSettings == null)
                {
                    this._customPagerSettings = new CustomPagerSettings();
                }
                return this._customPagerSettings;
            }
            set { this._customPagerSettings = value; }
        }
    }
}
