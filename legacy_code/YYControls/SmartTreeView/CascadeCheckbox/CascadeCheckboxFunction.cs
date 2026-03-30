using System;
using System.Collections.Generic;
using System.Text;

using System.Web.UI.WebControls;
using System.Web.UI;

namespace YYControls.SmartTreeViewFunction
{
    /// <summary>
    /// 扩展功能：联动复选框
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
        /// <param name="stv">SmartTreeView对象</param>
        public CascadeCheckboxFunction(SmartTreeView stv)
            : base(stv)
        {

        }

        /// <summary>
        /// 扩展功能的实现
        /// </summary>
        protected override void Execute()
        {
            this._stv.PreRender += new EventHandler(_stv_PreRender);
        }

        /// <summary>
        /// SmartTreeView的PreRender事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _stv_PreRender(object sender, EventArgs e)
        {
            // 注册向数组中添加成员的脚本
            if (!this._stv.Page.ClientScript.IsClientScriptBlockRegistered(String.Format("yy_stv_cascadeCheckbox_{0}", this._stv.ID)))
            {
                this._stv.Page.ClientScript.RegisterClientScriptBlock
                (
                    this.GetType(),
                    String.Format("yy_stv_cascadeCheckbox_{0}", this._stv.ID),
                    String.Format("yy_stv_ccTreeView_pre.push('{0}');", Helper.Common.GetChildControlPrefix(this._stv)),
                    true
                );
            }
        }
    }
}
