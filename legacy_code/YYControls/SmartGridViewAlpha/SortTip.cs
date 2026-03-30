using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;

namespace YYControls.SmartGridViewAlpha
{
    /// <summary>
    /// 排序提示类
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SortTip
    {
        private string _sortDescImage;
        /// <summary>
        /// 降序提示图片
        /// </summary>
        [
        Description("降序提示图片"),
        Category("扩展"),
        Editor("System.Web.UI.Design.UrlEditor", typeof(System.Drawing.Design.UITypeEditor)),
        DefaultValue(""),
        NotifyParentProperty(true)
        ]
        public string SortDescImage
        {
            get { return _sortDescImage; }
            set { _sortDescImage = value; }
        }

        private string _sortAscImage;
        /// <summary>
        /// 升序提示图片
        /// </summary>
        [
        Description("升序提示图片"),
        Category("扩展"),
        Editor("System.Web.UI.Design.UrlEditor", typeof(System.Drawing.Design.UITypeEditor)),
        DefaultValue(""),
        NotifyParentProperty(true)
        ]
        public string SortAscImage
        {
            get { return _sortAscImage; }
            set { _sortAscImage = value; }
        }

        private string _sortDescText;
        /// <summary>
        /// 降序提示文本
        /// </summary>
        [
        Description("降序提示文本"),
        Category("扩展"),
        DefaultValue(""),
        NotifyParentProperty(true)
        ]
        public string SortDescText
        {
            get { return _sortDescText; }
            set { _sortDescText = value; }
        }

        private string _sortAscText;
        /// <summary>
        /// 升序提示文本
        /// </summary>
        [
        Description("升序提示文本"),
        Category("扩展"),
        DefaultValue(""),
        NotifyParentProperty(true)
        ]
        public string SortAscText
        {
            get { return _sortAscText; }
            set { _sortAscText = value; }
        }

        /// <summary>
        /// ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "SortTip";
        }
    }
}
