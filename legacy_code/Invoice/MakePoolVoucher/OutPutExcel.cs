using System;
using System.Collections.Generic;
using System.Text;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Excel;
namespace Invoice.MakePoolVoucher
{
    public class OutPutExcel : System.Web.UI.Page
    {
        /// <summary> 
        /// 导出Excel 
        /// </summary> 
        /// <param name="dic">导出数据的泛型集合类</param>
        public void OutOut()
        {
            string filename = "";
            Excel.ApplicationClass oExcel;
            oExcel = new Excel.ApplicationClass();
            oExcel.UserControl = false;
            Excel.WorkbookClass wb = (Excel.WorkbookClass)oExcel.Workbooks.Add(System.Reflection.Missing.Value);
            for (int i = 1; i <= 5; i++)
            {
                oExcel.Cells[i, 1] = i.ToString();
                oExcel.Cells[i, 2] = "'第2列";
                oExcel.Cells[i, 3] = "'第3列";
                oExcel.Cells[i, 4] = "'第4列";
            }
            wb.Saved = true;
            filename = Request.PhysicalApplicationPath + "test.xls";
            oExcel.ActiveWorkbook.SaveCopyAs(filename);
            oExcel.Quit();
            System.GC.Collect();
            Response.Redirect(Request.ApplicationPath + "/test.xls");
        }
    }
}
