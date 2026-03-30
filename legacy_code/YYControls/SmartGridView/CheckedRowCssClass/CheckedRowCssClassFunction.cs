using System;
using System.Collections.Generic;
using System.Text;

using System.Web.UI.WebControls;
using System.Web.UI;

namespace YYControls.SmartGridViewFunction
{
    /// <summary>
    /// 扩展功能：行的指定复选框选中时改变行的样式
    /// </summary>
    public class CheckedRowCssClassFunction : ExtendFunction
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckedRowCssClassFunction()
            : base()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sgv">SmartGridView对象</param>
        public CheckedRowCssClassFunction(SmartGridView sgv)
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
            scriptString += String.Format("yy_sgv_crGridView_pre.push('{0}');", Helper.Common.GetChildControlPrefix(this._sgv));
            scriptString += String.Format("yy_sgv_crCheckbox_post.push('{0}');", this._sgv.CheckedRowCssClass.CheckBoxID);
            scriptString += String.Format("yy_sgv_crClassName = '{0}';", this._sgv.CheckedRowCssClass.CssClass);

            // 注册向数组中添加成员的脚本
            if (!this._sgv.Page.ClientScript.IsClientScriptBlockRegistered(String.Format("yy_sgv_checkedRowCssClass_{0}", this._sgv.ClientID)))
            {
                this._sgv.Page.ClientScript.RegisterClientScriptBlock
                (
                    this._sgv.GetType(),
                    String.Format("yy_sgv_checkedRowCssClass_{0}", this._sgv.ClientID),
                    scriptString,
                    true
                );
            }
        }
    }
}
