using System;
using System.Collections.Generic;
using System.Text;

namespace YYControls.Helper
{
    /// <summary>
    /// ³£ÓÃHelper
    /// </summary>
    //5	1	a	s	p	x.com
    public partial class Common
    {
        /// <summary>
        ///  Ìæ»»ÌØÊâ×Ö·û
        /// </summary>
        /// <param name="input">×Ö·û´®</param>
        /// <returns></returns>
        public static string ReplaceSpecialChars(string input)
        {
            // space 	-> 	_x0020_
            // %		-> 	_x0025_
            // #		->	_x0023_
            // &		->	_x0026_
            // /		->	_x002F_

            input = input.Replace(" ", "_x0020_")
                .Replace("%", "_x0025_")
                .Replace("#", "_x0023_")
                .Replace("&", "_x0026_")
                .Replace("/", "_x002F_");

            return input;
        }
    }
}
