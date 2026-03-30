using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Web.UI;

namespace YYControls
{
    /// <summary>
    /// 右键菜单实体类
    /// </summary>
    [ToolboxItem(false)]
    public class ContextMenu
    {
        private string _text;
        /// <summary>
        /// 菜单的文本内容
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private string _boundCommandName;
        /// <summary>
        /// 需要绑定的CommandName
        /// </summary>
        public string BoundCommandName
        {
            get { return this._boundCommandName; }
            set { this._boundCommandName = value; }
        }

        private string _navigateUrl;
        /// <summary>
        /// 链接的URL
        /// </summary>
        public string NavigateUrl
        {
            get { return this._navigateUrl; }
            set { this._navigateUrl = value; }
        }
            
        private string _target;
        /// <summary>
        /// 链接的目标窗口或框架
        /// </summary>
        public string Target
        {
            get { return this._target; }
            set { this._target = value; }
        }
    }
}
