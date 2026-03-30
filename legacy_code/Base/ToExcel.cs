using System;
using System.IO;
using System.Reflection;
using System.Data;
using Base.Biz;

namespace Base
{
    public class ToExcel
    {
        public void GrvToXls(string Sql,string cPath)
        {
            BaseBO baseBo = new BaseBO();
            DataSet ds = new DataSet();
            ds = baseBo.QueryDataSet(Sql);
            try
            {
                string CPath = cPath;
                if (File.Exists(CPath))
                    File.Delete(CPath);
                Microsoft.Office.Interop.Excel.XlFileFormat version = Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel7;//Excel 2003版本
                //创建Application对象
                Microsoft.Office.Interop.Excel.Application xApp = new Microsoft.Office.Interop.Excel.Application();
                xApp.Visible = false;
                //WorkBook对象
                Microsoft.Office.Interop.Excel.Workbook xBook = xApp.Workbooks.Add(true);
                //指定要操作的Sheet
                Microsoft.Office.Interop.Excel.Worksheet xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[1];
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (i == 0)
                        {
                            xSheet.Cells[1, j + 1] = ds.Tables[0].Columns[j].ColumnName.ToString();
                        }
                        xSheet.Cells[i + 2, j + 1] = ds.Tables[0].Rows[i][j].ToString();
                    }
                }
                xSheet.SaveAs(CPath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                xSheet = null;
                xBook = null;
                xApp.Workbooks.Close();
                xApp.Quit();
                xApp = null;
                System.GC.Collect();
            }
            catch (Exception ex)
            { }
        }
        public void GrvToXls(DataTable dt,string cPath)
        {
            try
            {
                string CPath = cPath;
                if (File.Exists(CPath))
                    File.Delete(CPath);
                Microsoft.Office.Interop.Excel.XlFileFormat version = Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel7;//Excel 2003版本
                //创建Application对象
                Microsoft.Office.Interop.Excel.Application xApp = new Microsoft.Office.Interop.Excel.Application();
                xApp.Visible = false;
                //WorkBook对象
                Microsoft.Office.Interop.Excel.Workbook xBook = xApp.Workbooks.Add(true);
                //指定要操作的Sheet
                Microsoft.Office.Interop.Excel.Worksheet xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[1];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (i == 0)
                        {
                            xSheet.Cells[1, j + 1] = dt.Columns[j].ColumnName.ToString();
                            Microsoft.Office.Interop.Excel.Range range = xSheet.Cells[1, j + 1] as Microsoft.Office.Interop.Excel.Range;
                            range.Font.Bold = true;
                        }
                        if (dt.Rows[i][dt.Columns[j].ColumnName.ToString()].ToString() != "&nbsp;")
                        {
                            xSheet.Cells[i + 2, j + 1] = dt.Rows[i][dt.Columns[j].ColumnName.ToString()].ToString();
                        }
                    }
                }
                xSheet.SaveAs(CPath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                xSheet = null;
                xBook = null;
                xApp.Workbooks.Close();
                xApp.Quit();
                xApp = null;
                System.GC.Collect();
            }
            catch (Exception ex)
            { }
        }
    }
}
