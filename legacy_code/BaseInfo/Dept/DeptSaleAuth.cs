using System;
using System.Collections.Generic;
using System.Text;
using Base.Biz;
using System.Data;
namespace BaseInfo.Dept
{
    public class DeptSaleAuth
    {
        public static string GetStore(int deptid)
        {
            string strTemp = "";
            string strSql = "Select StoreID from DeptSaleAuth where DeptID=" + deptid.ToString();

            BaseBO baseBo = new BaseBO();
            DataSet ds = baseBo.QueryDataSet(strSql);
            int rows = ds.Tables[0].Rows.Count;
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < rows; i++)
                {
                    if (strTemp.Trim() != "")
                    {
                        strTemp += "," + ds.Tables[0].Rows[i]["StoreID"].ToString();
                    }
                    else
                    {
                        strTemp = ds.Tables[0].Rows[i]["StoreID"].ToString();
                    }
                }
            }
            return strTemp;
        }
        public static string GetFloor(int deptid)
        {
            string strTemp = "";
            string strSql = "Select FloorID from DeptSaleAuth where DeptID=" + deptid.ToString();

            BaseBO baseBo = new BaseBO();
            DataSet ds = baseBo.QueryDataSet(strSql);
            int rows = ds.Tables[0].Rows.Count;
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < rows; i++)
                {
                    if (strTemp.Trim() != "")
                    {
                        strTemp += "," + ds.Tables[0].Rows[i]["FloorID"].ToString();
                    }
                    else
                    {
                        strTemp = ds.Tables[0].Rows[i]["FloorID"].ToString();
                    }
                }
            }
            return strTemp;



        }
    }
}
