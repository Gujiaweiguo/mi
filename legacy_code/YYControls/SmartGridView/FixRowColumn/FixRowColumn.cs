using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YYControls
{
    /// <summary>
    /// 固定指定行、指定列的实体类
    /// </summary>
    [ToolboxItem(false)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FixRowColumn
    {
        private string _fixRowType;
        /// <summary>
        /// 需要固定的行的RowType（用逗号“,”分隔）
        /// </summary>
        [
        Description("需要固定的行的RowType（用逗号“,”分隔）"),
        Category("扩展"),
        NotifyParentProperty(true)
        ]
        public virtual string FixRowType
        {
            get { return _fixRowType; }
            set { _fixRowType = value; }
        }

        private string _fixRowState;
        /// <summary>
        /// 需要固定的行的RowState（用逗号“,”分隔）
        /// </summary>
        [
        Description("需要固定的行的RowState（用逗号“,”分隔）"),
        Category("扩展"),
        NotifyParentProperty(true)
        ]
        public virtual string FixRowState
        {
            get { return _fixRowState; }
            set { _fixRowState = value; }
        }

        private string _fixRows;
        /// <summary>
        /// 需要固定的行的索引（用逗号“,”分隔）
        /// </summary>
        [
        Description("需要固定的行的索引（用逗号“,”分隔）"),
        Category("扩展"),
        NotifyParentProperty(true)
        ]
        public virtual string FixRows
        {
            get { return _fixRows; }
            set { _fixRows = value; }
        }

        private string _fixColumns;
        /// <summary>
        /// 需要固定的列的索引（用逗号“,”分隔）
        /// </summary>
        [
        Description("需要固定的列的索引（用逗号“,”分隔）"), 
        Category("扩展"), 
        NotifyParentProperty(true)
        ]
        public virtual string FixColumns
        {
            get { return _fixColumns; }
            set { _fixColumns = value; }
        }

        private string _tableWidth;
        /// <summary>
        /// 表格的宽度
        /// </summary>
        [
        Description("表格的宽度"),
        Category("扩展"),
        NotifyParentProperty(true)
        ]
        public string TableWidth
        {
            get { return _tableWidth; }
            set { _tableWidth = value; }
        }

        private string _tableHeight;
        /// <summary>
        /// 表格的高度
        /// </summary>
        [
        Description("表格的高度"),
        Category("扩展"),
        NotifyParentProperty(true)
        ]
        public string TableHeight
        {
            get { return _tableHeight; }
            set { _tableHeight = value; }
        }

        /// <summary>
        /// ToString();
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "";
        }
    }
}
