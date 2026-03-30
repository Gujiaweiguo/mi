using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Web.UI;

namespace YYControls
{
    /// <summary>
    /// SmartGridView类的属性部分
    /// </summary>
    public partial class SmartGridView
    {
        private ClientButtons _clientButtons;
        /// <summary>
        /// 根据按钮的CommandName设置其客户端属性
        /// </summary>
        [
        Description("根据CommandName设置按钮的客户端属性"),
        Category("扩展"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual ClientButtons ClientButtons
        {
            get 
            {
                if (_clientButtons == null)
                {
                    _clientButtons = new ClientButtons();
                }
                return _clientButtons; 
            }
        }
    }
}
