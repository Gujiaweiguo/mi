using System;
using System.Collections.Generic;
using System.Text;

using System.Web.UI.WebControls;
using System.Web.UI;

namespace YYControls.SmartGridViewFunction
{
    /// <summary>
    /// 扩展功能：响应行的双击事件
    /// </summary>
    public class RowDoubleClickFunction : ExtendFunction
    {
        List<string> _rowDoubleClickButtonUniqueIdList = new List<string>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public RowDoubleClickFunction()
            : base()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sgv">SmartGridView对象</param>
        public RowDoubleClickFunction(SmartGridView sgv)
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
        }

        /// <summary>
        /// RowDataBoundCell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="gvtc"></param>
        void _sgv_RowDataBoundCell(object sender, GridViewTableCell gvtc)
        {
            TableCell tc = gvtc.TableCell;

            foreach (Control c in tc.Controls)
            {
                IButtonControl ibc = c as IButtonControl;

                if (ibc != null && this._sgv.BoundRowDoubleClickCommandName == ibc.CommandName)
                {
                    // 响应双击事件的脚本
                    string js = this._sgv.Page.ClientScript.GetPostBackClientHyperlink(c, "");

                    GridViewRow gvr = tc.Parent as GridViewRow;
                    Helper.Common.SetAttribute(gvr, "ondblclick", js, AttributeValuePosition.Last);

                    _rowDoubleClickButtonUniqueIdList.Add(c.UniqueID);
                }
            }
        }

        /// <summary>
        /// RenderBegin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="writer"></param>
        void _sgv_RenderBegin(object sender, HtmlTextWriter writer)
        {
            foreach (string uniqueId in this._rowDoubleClickButtonUniqueIdList)
            {
                // 注册回发或回调数据以进行验证
                this._sgv.Page.ClientScript.RegisterForEventValidation(uniqueId);
            }
        }
    }
}
