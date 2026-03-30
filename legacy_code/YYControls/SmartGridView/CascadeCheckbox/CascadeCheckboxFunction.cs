using System;
using System.Collections.Generic;
using System.Text;

using System.Web.UI.WebControls;
using System.Web.UI;

namespace YYControls.SmartGridViewFunction
{
    /// <summary>
    /// 扩展功能：联动复选框（复选框的全选和取消全选）
    /// </summary>
    public class CascadeCheckboxFunction : ExtendFunction
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CascadeCheckboxFunction()
            : base()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sgv">SmartGridView对象</param>
        public CascadeCheckboxFunction(SmartGridView sgv)
            : base(sgv)
        {

        }

        /// <summary>
        /// 扩展功能的实现
        /// </summary>
        protected override void Execute()
        {
            this._sgv.PreRender += new EventHandler(_sgv_PreRender);
        }

        /// <summary>
        /// SmartGridView的PreRender事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _sgv_PreRender(object sender, EventArgs e)
        {
            // 构造向数组中添加成员的脚本
            string scriptString = "";
            foreach (CascadeCheckbox cc in this._sgv.CascadeCheckboxes)
            {
                scriptString += String.Format("yy_sgv_ccGridView_pre.push('{0}');", Helper.Common.GetChildControlPrefix(this._sgv));
                scriptString += String.Format("yy_sgv_ccAll_post.push('{0}');", cc.ParentCheckboxID);
                scriptString += String.Format("yy_sgv_ccItem_post.push('{0}');", cc.ChildCheckboxID);
            }

            // 注册向数组中添加成员的脚本
            if (!this._sgv.Page.ClientScript.IsClientScriptBlockRegistered(String.Format("yy_sgv_cascadeCheckbox_{0}", this._sgv.ID)))
            {
                this._sgv.Page.ClientScript.RegisterClientScriptBlock
                (
                    this._sgv.GetType(),
                    String.Format("yy_sgv_cascadeCheckbox_{0}", this._sgv.ID),
                    scriptString,
                    true
                );
            }
        }
    }
}
