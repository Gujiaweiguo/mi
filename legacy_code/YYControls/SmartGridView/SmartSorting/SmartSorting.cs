using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Web.UI;

namespace YYControls
{
    /// <summary>
    /// 高级排序功能实体类
    /// </summary>
    [ToolboxItem(false)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SmartSorting
    {
        private bool _allowSortTip;
        /// <summary>
        /// 是否启用排序提示
        /// </summary>
        [
        Description("是否启用排序提示"),
        Category("扩展"),
        NotifyParentProperty(true),
        DefaultValue(false)
        ]
        public bool AllowSortTip
        {
            get { return _allowSortTip; }
            set { _allowSortTip = value; }
        }
        
        private bool _allowMultiSorting;
        /// <summary>
        /// 是否启用复合排序
        /// </summary>
        [
        Description("是否启用复合排序"),
        Category("扩展"),
        NotifyParentProperty(true),
        DefaultValue(false)
        ]
        public bool AllowMultiSorting
        {
            get { return _allowMultiSorting; }
            set { _allowMultiSorting = value; }
        }

        private string _sortAscImageUrl;
        /// <summary>
        /// 升序提示图片的URL
        /// </summary>
        [
        Description("升序提示图片的URL"),
        Category("扩展"),
        Editor("System.Web.UI.Design.UrlEditor", typeof(System.Drawing.Design.UITypeEditor)),
        NotifyParentProperty(true)
        ]
        public string SortAscImageUrl
        {
            get { return _sortAscImageUrl; }
            set { _sortAscImageUrl = value; }
        }

        private string _sortDescImageUrl;
        /// <summary>
        /// 降序提示图片的URL
        /// </summary>
        [
        Description("降序提示图片的URL"),
        Category("扩展"),
        Editor("System.Web.UI.Design.UrlEditor", typeof(System.Drawing.Design.UITypeEditor)),
        NotifyParentProperty(true)
        ]
        public string SortDescImageUrl
        {
            get { return _sortDescImageUrl; }
            set { _sortDescImageUrl = value; }
        }

        private string _sortAscText;
        /// <summary>
        /// 升序提示文本
        /// </summary>
        [
        Description("升序提示文本"),
        Category("扩展"),
        NotifyParentProperty(true)
        ]
        public string SortAscText
        {
            get { return _sortAscText; }
            set { _sortAscText = value; }
        }

        private string _sortDescText;
        /// <summary>
        /// 降序提示文本
        /// </summary>
        [
        Description("降序提示文本"),
        Category("扩展"),
        NotifyParentProperty(true)
        ]
        public string SortDescText
        {
            get { return _sortDescText; }
            set { _sortDescText = value; }
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
