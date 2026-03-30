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
   public class LocalXML
    {
       public void GetXml( string userDoc)
       {
           Directory.CreateDirectory("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc);
           bool xmlFiles = File.Exists("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\Loca.xml");
           if (xmlFiles)
           {

               File.Delete("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\Loca.xml");

           }
           BaseBO baseBO = new BaseBO();
           string wherePO = "select deptid LocalID, deptname LocalName,deptname LocalDesc from Dept where DeptType=4 and DeptStatus=1";
           DataTable objPOdt = baseBO.QueryDataSet(wherePO).Tables[0];
           StringBuilder xmlData = new StringBuilder();
           xmlData.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
           xmlData.Append("<locaInfor>");
           for (int i = 0; i < objPOdt.Rows.Count; i++)
           {
               xmlData.Append("<loca id=\"" + objPOdt.Rows[i]["LocalID"].ToString().Trim() + "\" nm=\"" + objPOdt.Rows[i]["LocalName"].ToString().Trim() + "\" infor=\"" + objPOdt.Rows[i]["LocalDesc"].ToString().Trim() + "\"/>");

           }
           xmlData.Append("</locaInfor>");
           XmlDocument doc = new XmlDocument();
           doc.LoadXml(xmlData.ToString());
           try
           {
               doc.Save("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\Loca.xml");
           }
           catch (System.Exception ex)
           {
               throw ex;
           }

       }

    }
}
