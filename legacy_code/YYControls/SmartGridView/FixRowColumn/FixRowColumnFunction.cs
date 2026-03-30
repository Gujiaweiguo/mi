using System;
using System.Collections.Generic;
using System.Text;

using System.Web.UI.WebControls;
using System.Web.UI;

namespace YYControls.SmartGridViewFunction
{
    /// <summary>
    /// 扩展功能：固定指定行、指定列
    /// </summary>
    public class FixRowColumnFunction : ExtendFunction
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FixRowColumnFunction()
            : base()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sgv">SmartGridView对象</param>
        public FixRowColumnFunction(SmartGridView sgv)
            : base(sgv)
        {

        }

        /// <summary>
        /// 扩展功能的实现
        /// </summary>
        protected override void Execute()
        {
            this._sgv.RowDataBoundCell += new SmartGridView.RowDataBoundCellHandler(_sgv_RowDataBoundCell);
            this._sgv.RenderBegin += new SmartGridView.RenderBeginHandler(_sgv_RenderBegin);
            this._sgv.RenderEnd += new SmartGridView.RenderEndHandler(_sgv_RenderEnd);
        }

        /// <summary>
        /// SmartGridView的RowDataBoundCell事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="gvtc"></param>
        void _sgv_RowDataBoundCell(object sender, GridViewTableCell gvtc)
        {
            TableCell tc = gvtc.TableCell;
            GridViewRow gvr = (GridViewRow)tc.Parent;

            int i = 0; // 0-既不固定行也不固定列；1-固定行或固定列；2-既固定行也固定列
            // 固定行
            if 
            (
                (
                    !String.IsNullOrEmpty(this._sgv.FixRowColumn.FixRows) 
                    && 
                    Array.Exists(this._sgv.FixRowColumn.FixRows.Split(','), delegate(string s) { return s == gvr.RowIndex.ToString(); })
                )
                || 
                (
                    !String.IsNullOrEmpty(this._sgv.FixRowColumn.FixRowType) 
                    && 
                    Array.Exists(this._sgv.FixRowColumn.FixRowType.Split(','), delegate(string s) { return s == gvr.RowType.ToString(); })
                )
                || 
                (
                    !String.IsNullOrEmpty(this._sgv.FixRowColumn.FixRowState) 
                    && 
                    Array.Exists(this._sgv.FixRowColumn.FixRowState.Split(','), delegate(string s) { return s == gvr.RowState.ToString(); })
                )
            )
            {
                i++;
                Helper.Common.SetAttribute(tc, "class", "yy_sgv_fixRow", AttributeValuePosition.Last, ' ');
            }
            // 固定列
            if (Array.Exists(this._sgv.FixRowColumn.FixColumns.Split(','), delegate(string s) { return s == gvtc.ColumnIndex.ToString(); }))
            {
                i++;
                Helper.Common.SetAttribute(tc, "class", "yy_sgv_fixCol", AttributeValuePosition.Last, ' ');
            }

            // 低等级的z-index
            if (i == 1)
            {
                Helper.Common.SetAttribute(tc, "class", "yy_sgv_fixLow", AttributeValuePosition.Last, ' ');
            }
            // 高等级的z-index
            else if (i == 2)
            {
                Helper.Common.SetAttribute(tc, "class", "yy_sgv_fixHigh", AttributeValuePosition.Last, ' ');
            }
        }

        /// <summary>
        /// RenderBegin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="writer"></param>
        void _sgv_RenderBegin(object sender, HtmlTextWriter writer)
        {
            writer.AddStyleAttribute(HtmlTextWriterStyle.Overflow, "auto");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "relative");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, String.IsNullOrEmpty(this._sgv.FixRowColumn.TableWidth) ? "100%" : this._sgv.FixRowColumn.TableWidth);
            writer.AddStyleAttribute(HtmlTextWriterStyle.Height, String.IsNullOrEmpty(this._sgv.FixRowColumn.TableHeight) ? "100%" : this._sgv.FixRowColumn.TableHeight);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
        }

        /// <summary>
        /// RenderEnd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="writer"></param>
        void _sgv_RenderEnd(object sender, HtmlTextWriter writer)
        {
            writer.RenderEndTag();
        }
    }
}
