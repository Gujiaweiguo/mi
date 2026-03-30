using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;

namespace YYControls
{
    /// <summary>
    /// 客户端按钮实体类（根据按钮的CommandName设置其客户端属性）
    /// </summary>
    [ToolboxItem(false)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ClientButton
    {
        private string _boundCommandName;
        /// <summary>
        /// 需要绑定的CommandName
        /// </summary>
        public string BoundCommandName
        {
            get { return this._boundCommandName; }
            set { this._boundCommandName = value; }
        }

        private string _attributeKey;
        /// <summary>
        /// 属性的名称
        /// </summary>
        public string AttributeKey
        {
            get { return this._attributeKey; }
            set { this._attributeKey = value; }
        }

        private string _attributeValue;
        /// <summary>
        /// 属性的值（{0} - CommandArgument；{1} - Text）
        /// </summary>
        public string AttributeValue
        {
            get { return this._attributeValue; }
            set { this._attributeValue = value; }
        }

        private AttributeValuePosition _position;
        /// <summary>
        /// 属性的值的位置
        /// </summary>
        public AttributeValuePosition Position
        {
            get { return _position; }
            set { _position = value; }
        }
    }
}
