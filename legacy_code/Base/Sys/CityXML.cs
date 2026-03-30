using System;
using System.Collections.Generic;
using System.Text;
using Base.Biz;
using Base.DB;
using Base;
using Base.Util;
using Base.Page;
using Base.XML;
using System.Xml;
using System.IO;
using System.Data;


namespace Base.Sys
{
   public  class CityXML
    {
       public void GetXml(string userDoc)
        {

            Directory.CreateDirectory("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc);
            bool xmlFiles = File.Exists("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\City.xml");
            if (xmlFiles)
            {

                File.Delete("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\City.xml");

            }
            BaseBO baseBO = new BaseBO();
            string wherePO = @"select isnull(pdeptid,0) LocalID,DeptID, deptname,deptname DeptDesc from Dept where DeptType=5 and DeptStatus=1";
            DataTable objPOdt = baseBO.QueryDataSet(wherePO).Tables[0];
            StringBuilder xmlData = new StringBuilder();
            xmlData.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            xmlData.Append("<cityInfor>");
            String[] Nodeinfos = { "LocalID" };//截取节点属性数组
            //将objPOdt中的重复数据过滤掉
            DataTable objInfodt = new DataTable();
            DataView myDataView = new DataView(objPOdt);
            objInfodt = myDataView.ToTable(true, Nodeinfos);
            for (int i = 0; i < objInfodt.Rows.Count; i++)
            {
                xmlData.Append("<city locaID=\"" + objInfodt.Rows[i]["LocalID"].ToString().Trim() + "\">");
                for (int j = 0; j < objPOdt.Rows.Count; j++)
                {
                    if (objPOdt.Rows[j]["LocalID"].ToString().Trim() == objInfodt.Rows[i]["LocalID"].ToString().Trim())
                    {
                        xmlData.Append("<city id=\"" + objPOdt.Rows[j]["DeptID"].ToString().Trim() + "\" nm=\"" + objPOdt.Rows[j]["DeptName"].ToString().Trim() +
                            "\" infor=\"" + objPOdt.Rows[j]["DeptDesc"].ToString().Trim() + "\" />");
                    }
                }
                xmlData.Append("</city>");
            }
            xmlData.Append("</cityInfor>");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlData.ToString());
            try
            {
                doc.Save("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\City.xml");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
    }
}
