using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Web.UI;

namespace YYControls.SmartGridViewAlpha
{
    /// <summary>
    /// ContextMenu 的摘要说明。
    /// </summary>
    [ToolboxItem(false)]
    public class ContextMenu
    {
        private string _icon;
        /// <summary>
        /// 文字左边的图标的链接
        /// </summary>
        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        private string _text;
        /// <summary>
        /// 菜单的文字
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
       
        private string _commandButtonId;
        /// <summary>
        /// 所调用的命令按钮的ID
        /// </summary>
        public string CommandButtonId
        {
            get { return _commandButtonId; }
            set { _commandButtonId = value; }
        }

        private string _navigateUrl;
        /// <summary>
        /// 链接的url
        /// </summary>
        public string NavigateUrl
        {
            get { return _navigateUrl; }
            set { _navigateUrl = value; }
        }

        private string _key;
        /// <summary>
        /// 自定义属性key
        /// </summary>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private string _value;
        /// <summary>
        /// 自定义属性value
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private ItemTypeCollection _itemType;
        /// <summary>
        /// 右键菜单的项的类别
        /// </summary>
        public ItemTypeCollection ItemType
        {
            get { return _itemType; }
            set { _itemType = value; }
        }

        private TargetCollection _target;
        /// <summary>
        /// 链接的target
        /// </summary>
        public TargetCollection Target
        {
            get { return _target; }
            set { _target = value; }
        }


        /// <summary>
        /// 右键菜单的项的类别
        /// </summary>
        public enum ItemTypeCollection
        {
            /// <summary>
            /// 链接
            /// </summary>
            Link,
            /// <summary>
            /// 按钮
            /// </summary>
            Command,
            /// <summary>
            /// 自定义
            /// </summary>
            Custom,
            /// <summary>
            /// 分隔线
            /// </summary>
            Separator
        }

        /// <summary>
        /// 链接的target
        /// </summary>
        public enum TargetCollection
        {
            /// <summary>
            /// 新开窗口
            /// </summary>
            Blank,
            /// <summary>
            /// 当前窗口
            /// </summary>
            Self,
            /// <summary>
            /// 跳出框架
            /// </summary>
            Top
        }
    }
}
