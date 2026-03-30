using System;
using System.Collections.Generic;
using System.Text;

using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;

using YYControls.SmartTreeViewFunction;

#if DEBUG
[assembly: System.Web.UI.WebResource("YYControls.SmartTreeView.Resources.ScriptLibraryDebug.js", "text/javascript")]
#else
[assembly: System.Web.UI.WebResource("YYControls.SmartTreeView.Resources.ScriptLibrary.js", "text/javascript")]
#endif

namespace YYControls
{
    /// <summary>
    /// SmartTreeView类，继承自TreeView
    /// </summary>
    [ToolboxData(@"<{0}:SmartTreeView runat='server'></{0}:SmartTreeView>")]
    [System.Drawing.ToolboxBitmap(typeof(YYControls.Resources.Icon), "SmartTreeView.bmp")]
    public partial class SmartTreeView : TreeView
    {
        // 需要扩展的功能对象容器
        private List<ExtendFunction> _efs = new List<ExtendFunction>();

        /// <summary>
        /// OnInit
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.PreRender += new EventHandler(SmartTreeView_PreRender);

            // 将需要扩展的功能对象添加到功能扩展列表里
            if (this._allowCascadeCheckbox)
                this._efs.Add(new CascadeCheckboxFunction());

            // 遍历需要实现的功能扩展，并实现它
            foreach (ExtendFunction ef in this._efs)
            {
                ef.SmartTreeView = this;
                ef.Complete();
            }

            base.OnInit(e);
        }

        /// <summary>
        /// SmartTreeView的PreRender事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SmartTreeView_PreRender(object sender, EventArgs e)
        {
            // 注册所需脚本
            if (!this.Page.ClientScript.IsClientScriptIncludeRegistered(this.GetType(), "yy_stv_scriptLibrary"))
            {
                this.Page.ClientScript.RegisterClientScriptInclude
                (
                    this.GetType(),
                    "yy_stv_scriptLibrary",
                    this.Page.ClientScript.GetWebResourceUrl
                    (
                        #if DEBUG
                        this.GetType(), "YYControls.SmartTreeView.Resources.ScriptLibraryDebug.js"
                        #else
                        this.GetType(), "YYControls.SmartTreeView.Resources.ScriptLibrary.js"
                        #endif
                    )
                );
            }
        }
    }
}
