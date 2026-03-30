using System;
using System.Collections.Generic;
using System.Text;

using System.Web.UI.WebControls;
using System.Web.UI;

namespace YYControls
{
    /// <summary>
    /// SmartGridView类的委托部分
    /// </summary>
    public partial class SmartGridView
    {
        /// <summary>
        /// RowDataBoundDataRow事件委托
        /// </summary>
        /// <remarks>
        /// RowDataBound事件中的DataControlRowType.DataRow部分
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void RowDataBoundDataRowHandler(object sender, GridViewRowEventArgs e);

        /// <summary>
        /// RowDataBoundCell事件委托
        /// </summary>
        /// <remarks>
        /// RowDataBound事件中的所有单元格
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="gvtc"></param>
        public delegate void RowDataBoundCellHandler(object sender, GridViewTableCell gvtc);

        /// <summary>
        /// RenderBegin事件委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="writer"></param>
        public delegate void RenderBeginHandler(object sender, HtmlTextWriter writer);

        /// <summary>
        /// RenderEnd事件委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="writer"></param>
        public delegate void RenderEndHandler(object sender, HtmlTextWriter writer);
    }
}
