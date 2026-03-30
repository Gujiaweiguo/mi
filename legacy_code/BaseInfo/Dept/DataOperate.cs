using System;
using System.Collections.Generic;
using System.Text;
using Base.Biz;
namespace BaseInfo.Dept
{
    public class DataOperate
    {
        public static bool isNumber(string str)
        {
            System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^-?(\d*)$");
            System.Text.RegularExpressions.Match mc = rg.Match(str.ToString());
            return (mc.Success);
        } 
    }
}
