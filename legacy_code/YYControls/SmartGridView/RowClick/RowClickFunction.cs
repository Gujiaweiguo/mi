using System;
using System.Collections.Generic;
using System.Text;

using System.Web.UI.WebControls;
using System.Web.UI;

namespace YYControls.SmartGridViewFunction
{
    /// <summary>
    /// 扩展功能：响应行的单击事件
    /// </summary>
    public class RowClickFunction : ExtendFunction
    {
        List<string> _rowClickButtonUniqueIdList = new List<string>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public RowClickFunction()
            : base()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sgv">SmartGridView对象</param>
        public RowClickFunction(SmartGridView sgv)
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

                if (ibc != null && this._sgv.BoundRowClickCommandName == ibc.CommandName)
                {
                    // 300毫秒后响应单击事件的脚本（避免和双击事件冲突）
                    string js = this._sgv.Page.ClientScript.GetPostBackClientHyperlink(c, "");
                    js = js.Insert(11, "setTimeout(\"");
                    js += "\", 300)";

                    GridViewRow gvr = tc.Parent as GridViewRow;
                    Helper.Common.SetAttribute(gvr, "onclick", js, AttributeValuePosition.Last);

                    _rowClickButtonUniqueIdList.Add(c.UniqueID);
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
            foreach (string uniqueId in this._rowClickButtonUniqueIdList)
            {
                // 注册回发或回调数据以进行验证
                this._sgv.Page.ClientScript.RegisterForEventValidation(uniqueId);
            }
        }
    }
}
