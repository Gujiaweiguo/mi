using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Web.UI;

namespace YYControls
{
    /// <summary>
    /// 行的指定复选框选中时改变行的样式实体类
    /// </summary>
    [ToolboxItem(false)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CheckedRowCssClass
    {
        private string _checkBoxID;
        /// <summary>
        /// 模板列中 数据行的复选框ID
        /// </summary>
        [
        Description("模板列中 数据行的复选框ID"),
        Category("扩展"),
        NotifyParentProperty(true)
        ]
        public string CheckBoxID
        {
            get { return _checkBoxID; }
            set { _checkBoxID = value; }
        }

        private string _cssClass;
        /// <summary>
        /// 选中的行的 CSS 类名
        /// </summary>
        [
        Description("选中的行的 CSS 类名"),
        Category("扩展"),
        NotifyParentProperty(true)
        ]
        public string CssClass
        {
            get { return _cssClass; }
            set { _cssClass = value; }
        }

        /// <summary>
        /// ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "";
        }

    }
}
