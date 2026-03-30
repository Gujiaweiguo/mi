using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace BaseInfo
{
    public class SigmaGrid
    {
        /// <summary>
        /// 返回Grid的JS函数
        /// </summary>
        /// <param name="strSql">查询Sql</param>
        /// <param name="strDiv">DivID</param>
        /// <returns></returns>
        public static string GetGrid(string strSql, string strDiv)
        {
            string strTemp = "";
            Base.Biz.BaseBO baseBo = new Base.Biz.BaseBO();
            DataSet dt = baseBo.QueryDataSet(strSql);
            int intCol = dt.Tables[0].Columns.Count;
            int intRow = dt.Tables[0].Rows.Count;
            strTemp = "<script type='text/javascript'>" + "\r\n" + "function GetGrid() {" + "\r\n" +
                     "var grid_demo_id2 = 'myGrid2';" + "\r\n";

            if (intRow >= 0)
            {
                //数据
                strTemp += "var __TEST_DATA__=" + "\r\n" +
                               "[" + "\r\n";
                for (int i = 0; i < intRow; i++ )
                {
                    for (int j = 0; j < intCol;j++ )
                    {
                        if (j == 0)
                        {
                            strTemp += "[";
                        }
                        if (j < intCol - 1)
                        {
                            strTemp += "'" + dt.Tables[0].Rows[i][j].ToString() + "',";
                        }
                        else if (j == intCol - 1)
                        {
                            strTemp += "'" + dt.Tables[0].Rows[i][j].ToString() + "']";
                        }
                    }
                    if (i < intRow-1)
                    {
                        strTemp += "," + "\r\n";
                    }
                    else if (i == intRow - 1)
                    {
                        strTemp += "" + "\r\n";
                    }
                }
                strTemp += "];" + "\r\n";     
                //标题
                strTemp += "var dsOption = {" + "\r\n" +
                                "fields: [" + "\r\n";
                for (int i = 0; i < intCol;i++ )
                {
                    strTemp += "{ name: '" + dt.Tables[0].Columns[i].ColumnName.ToString() + "'";
                    if (i == intCol - 1)
                    {
                        strTemp += "}" + "\r\n";
                    }
                    else
                    {
                        strTemp += "}," + "\r\n";
                    }
                }
                strTemp += "]," + "\r\n" +
                           "recordType: 'array'," + "\r\n" +
                           "data: __TEST_DATA__" + "\r\n" +
                           "}" + "\r\n";

                //strTemp += "function my_renderer(value, record, columnObj, grid, colNo, rowNo) {" + "\r\n"; 
                //               "var no = record[columnObj.fieldIndex];" + "\r\n" +
                //               "var color = '000000';" + "\r\n" +
                //               "return \"span style=\'color:' + color + ';\'>' + no + '</span>';" + "\r\n" +
                //           "}" + "\r\n" +
                //           "function my_Numrenderer(value, record, columnObj, grid, colNo, rowNo) {" + "\r\n" +
                //               "var no = record[columnObj.fieldIndex];" + "\r\n" +
                //               "if (no <= 0) {" + "\r\n" +
                //                    "var color = 'ff0000';" + "\r\n" +
                //               "return '<span style=\'color:' + color + ';\'>' + no + '</span>';" + "\r\n" +
                //               "} else if (no > 0) {" + "\r\n" +
                //               "var color = '000000';" + "\r\n" +
                //               "return '<span style=\'color:' + color + ';\'>' + no + '</span>';" + "\r\n" +
                //               "}" + "\r\n" +
                //           "}" + "\r\n" +
                //           "function my_hdRenderer(header, colObj, grid) {" + "\r\n" +
                //               "var color = '666666';" + "\r\n" +
                //               "return '<span style=\'color:' + color + ';\' font:24px;>' + String(header) + '</span>';" + "\r\n" +
                //           "}" + "\r\n" +
                //           "function my_Imgrenderer(value, record, columnObj, grid, colNo, rowNo) {" + "\r\n" +
                //               "var no = record[columnObj.fieldIndex];" + "\r\n" +
                //               "return '<img style=\'padding-top:4px;\' src=\'' + no.toLowerCase() + '\'>';" + "\r\n" +
                //           "}" + "\r\n";

                strTemp += "var colsOption = [" + "\r\n";
                for (int i = 0; i < intCol;i++ )
                {
                    strTemp += "{ id: '" + dt.Tables[0].Columns[i].ColumnName.ToString() + "', header: '" + dt.Tables[0].Columns[i].ColumnName.ToString() + "'" +
                               ", width: 120, align: 'center', hdRenderer: my_hdRenderer, headAlign: 'center', renderer: my_renderer }";
                    if (i != intCol - 1)
                    {
                        strTemp += "," + "\r\n";
                    }
                    else
                    {
                        strTemp += "\r\n";
                    }
                }
                strTemp += "];" + "\r\n";
                strTemp += "var gridOption2 = {" + "\r\n" + 
                        "id : grid_demo_id2," + "\r\n" +
                        "width: '100%'," + "\r\n" +
                        "height: '420'," + "\r\n" +
                        "container : '" + strDiv + "'," + "\r\n" +
                        "replaceContainer : true, " + "\r\n" +
                        "dataset : dsOption ," + "\r\n" +
                        "columns : colsOption," + "\r\n" +
                        "pageSize : 20," + "\r\n" +
                        "toolbarContent : 'print'," + "\r\n" +
                        "skin: 'mac'" + "\r\n" +
                        "};" + "\r\n";
                strTemp += "var mygrid2 = new Sigma.Grid(gridOption2);" + "\r\n" +
                          "Sigma.Util.onLoad(Sigma.Grid.render(mygrid2));" + "\r\n" +
                           "}" + "\r\n" +
                           "</script>";
            }

            return strTemp;
        }
        /// <summary>
        /// 返回Grid的JS函数
        /// </summary>
        /// <param name="dt">DataSet</param>
        /// <param name="dt">DivID</param>
        /// <returns></returns>
        public string GetGrid(DataSet dt,string strDiv)
        {
            return "";
        }

    }
}
