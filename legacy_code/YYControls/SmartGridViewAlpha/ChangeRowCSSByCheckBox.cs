using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;

namespace YYControls.SmartGridViewAlpha
{
    /// <summary>
    /// 通过行的CheckBox的选中与否来修改行的样式
    /// 实体类
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ChangeRowCSSByCheckBox
    {
        private string _checkBoxID;
        /// <summary>
        /// 根据哪个ChecxBox来判断是否选中了行，指定该CheckBox的ID
        /// </summary>
        [
        Description("根据哪个ChecxBox来判断是否选中了行，指定该CheckBox的ID"),
        Category("扩展"),
        DefaultValue(""),
        NotifyParentProperty(true)
        ]
        public string CheckBoxID
        {
            get { return _checkBoxID; }
            set { _checkBoxID = value; }
        }

        private string _cssClassRowSelected;
        /// <summary>
        /// 选中行的样式的 CSS 类名
        /// </summary>
        [
        Description("选中行的样式的 CSS 类名"),
        Category("扩展"),
        DefaultValue(""),
        NotifyParentProperty(true)
        ]
        public string CssClassRowSelected
        {
            get { return _cssClassRowSelected; }
            set { _cssClassRowSelected = value; }
        }

        /// <summary>
        /// ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "ChangeRowCSSByCheckBox";
        }
    }
}
