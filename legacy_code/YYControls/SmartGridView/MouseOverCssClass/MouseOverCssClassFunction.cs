using System;
using System.Collections.Generic;
using System.Text;

using System.Web.UI.WebControls;

namespace YYControls.SmartGridViewFunction
{
    /// <summary>
    /// 扩展功能：鼠标经过行时改变行的样式
    /// </summary>
    public class MouseOverCssClassFunction : ExtendFunction
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MouseOverCssClassFunction()
            : base()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sgv">SmartGridView对象</param>
        public MouseOverCssClassFunction(SmartGridView sgv)
            : base(sgv)
        {
    
        }

        /// <summary>
        /// 扩展功能的实现
        /// </summary>
        protected override void Execute()
        {
            this._sgv.RowDataBoundDataRow += new SmartGridView.RowDataBoundDataRowHandler(_sgv_RowDataBoundDataRow);
        }

        /// <summary>
        /// SmartGridView的RowDataBoundDataRow事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _sgv_RowDataBoundDataRow(object sender, GridViewRowEventArgs e)
        {
            // 添加数据行的onmouseover事件和onmouseout事件，以实现鼠标经过行时改变行的样式
            Helper.Common.SetAttribute(e.Row, "onmouseover", String.Format("yy_sgv_changeMouseOverCssClass(this, '{0}')", this._sgv.MouseOverCssClass), AttributeValuePosition.Last);
            Helper.Common.SetAttribute(e.Row, "onmouseout", "yy_sgv_changeMouseOverCssClass(this)", AttributeValuePosition.Last);
        }
    }
}
