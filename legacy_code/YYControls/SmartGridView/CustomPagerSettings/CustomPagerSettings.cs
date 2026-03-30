using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Web.UI;

namespace YYControls
{
    /// <summary>
    /// 自定义分页样式实体类
    /// </summary>
    [ToolboxItem(false)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CustomPagerSettings
    {
        private PagingMode _pagingMode;
        /// <summary>
        /// 自定义分页的显示模式
        /// </summary>
        [
        Description("自定义分页的显示模式"),
        Category("扩展"),
        NotifyParentProperty(true),
        DefaultValue(typeof(PagingMode), "Default")
        ]
        public virtual PagingMode PagingMode
        {
            get { return _pagingMode; }
            set { _pagingMode = value; }
        }

        private string _textFormat;
        /// <summary>
        /// 自定义分页的文本显示样式（{0}-每页显示记录数；{1}-总记录数；{2}-当前页数；{3}-总页数）
        /// </summary>
        [
        Description("自定义分页的文本显示样式"),
        Category("扩展"),
        NotifyParentProperty(true),
        ]
        public virtual string TextFormat
        {
            get { return _textFormat; }
            set { _textFormat = value; }
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
