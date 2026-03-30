using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;

namespace YYControls.SmartGridViewAlpha
{
    /// <summary>
    /// 固定表头、指定行或指定列的实体类
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FixRowCol
    {
        private bool _isFixHeader;
        /// <summary>
        /// 固定表头否？
        /// </summary>
        [Description("固定表头否？"), Category("扩展"), DefaultValue(false), NotifyParentProperty(true)]
        public virtual bool IsFixHeader
        {
            get { return _isFixHeader; }
            set { _isFixHeader = value; }
        }

        private bool _isFixPager;
        /// <summary>
        /// 固定分页行否？
        /// </summary>
        [Description("固定分页行否？"), Category("扩展"), DefaultValue(false), NotifyParentProperty(true)]
        public virtual bool IsFixPager
        {
            get { return _isFixPager; }
            set { _isFixPager = value; }
        }

        private string _fixRowIndices;
        /// <summary>
        /// 需要固定的行的索引（用逗号“,”分隔）
        /// </summary>
        [Description("需要固定的行的索引（用逗号“,”分隔）"), Category("扩展"), NotifyParentProperty(true)]
        public virtual string FixRowIndices
        {
            get { return _fixRowIndices; }
            set { _fixRowIndices = value; }
        }

        private string _fixColumnIndices;
        /// <summary>
        /// 需要固定的列的索引（用逗号“,”分隔）
        /// </summary>
        [Description("需要固定的列的索引（用逗号“,”分隔）"), Category("扩展"), NotifyParentProperty(true)]
        public virtual string FixColumnIndices
        {
            get { return _fixColumnIndices; }
            set { _fixColumnIndices = value; }
        }

        private System.Web.UI.WebControls.Unit _tableWidth;
        /// <summary>
        /// 表格的宽度
        /// </summary>
        [Description("表格的宽度"), Category("扩展"), NotifyParentProperty(true)]
        public System.Web.UI.WebControls.Unit TableWidth
        {
            get { return _tableWidth; }
            set { _tableWidth = value; }
        }

        private System.Web.UI.WebControls.Unit _tableHeight;
        /// <summary>
        /// 表格的高度
        /// </summary>
        [Description("表格的高度"), Category("扩展"), NotifyParentProperty(true)]
        public System.Web.UI.WebControls.Unit TableHeight
        {
            get { return _tableHeight; }
            set { _tableHeight = value; }
        }

        private bool _enableScrollState;
        /// <summary>
        /// 是否保持滚动条的状态
        /// </summary>
        [Description("是否保持滚动条的状态"), Category("扩展"), DefaultValue(false), NotifyParentProperty(true)]
        public bool EnableScrollState
        {
            get { return _enableScrollState; }
            set { _enableScrollState = value; }
        }

        /// <summary>
        /// ToString();
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "FixRowCol";
        }
    }
}
