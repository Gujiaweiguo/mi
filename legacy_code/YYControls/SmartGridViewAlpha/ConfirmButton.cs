using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Web.UI;

namespace YYControls.SmartGridViewAlpha
{
    /// <summary>
    /// ConfirmButton 的摘要说明。
    /// </summary>
    [ToolboxItem(false)]
    [TypeConverter(typeof(ConfirmButtonConverter))]
    public class ConfirmButton
    {
        private string _commandName;
        /// <summary>
        /// 按钮的CommandName
        /// </summary>
        public string CommandName
        {
            get { return this._commandName; }
            set { this._commandName = value; }
        }

        private string _confirmMessage;
        /// <summary>
        /// 确认框弹出的信息
        /// </summary>
        public string ConfirmMessage
        {
            get { return this._confirmMessage; }
            set { this._confirmMessage = value; }
        }
    }
}
