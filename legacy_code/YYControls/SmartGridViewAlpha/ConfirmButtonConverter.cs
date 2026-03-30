using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Globalization;

namespace YYControls.SmartGridViewAlpha
{
    /// <summary>
    /// 类型转换器
    /// </summary>
    public class ConfirmButtonConverter : ExpandableObjectConverter
    {
        /// <summary>
        /// 返回值能否将ConfirmButton类型转换为String类型
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// 将ConfirmButton类型转换为String类型
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture,
            object value, Type destinationType)
        {
            if (value != null)
            {
                if (!(value is YYControls.SmartGridViewAlpha.ConfirmButton))
                {
                    throw new ArgumentException(
                        "无效的ConfirmButton", "value");
                }
            }

            if (destinationType.Equals(typeof(string)))
            {
                if (value == null)
                {
                    return String.Empty;
                }
                return "ConfirmButton";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
