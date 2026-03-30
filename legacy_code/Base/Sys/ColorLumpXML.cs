using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using Base.Biz;
using System.Data;
using Base.XML;

namespace Base.Sys
{
   public  class ColorLumpXML 
    {
       public void GetXml(string userDoc, int Colortype)
       {

           Directory.CreateDirectory("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc);
           bool xmlFiles = File.Exists("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\ColorLump" + Colortype + ".xml");
           if (xmlFiles)
           {

               File.Delete("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\ColorLump" + Colortype + ".xml");

           }
           BaseBO baseBO = new BaseBO();
           ColorLumpXMLInfo colorlump = new ColorLumpXMLInfo(Colortype);
           DataTable objPOdt = baseBO.QueryDataSet(colorlump).Tables[0];
           StringBuilder xmlData = new StringBuilder();
           xmlData.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
           xmlData.Append("<ToolBar>");
           for (int i = 0; i < objPOdt.Rows.Count; i++)
           {
               xmlData.Append("<toolbar toolbarID=\"C10" + (i+1).ToString() +
                   "\" desc=\"" + objPOdt.Rows[i]["classname"].ToString().Trim() +
                   "\" rb=\"" + objPOdt.Rows[i]["rb"].ToString().Trim() +
                   "\" gb=\"" + objPOdt.Rows[i]["gb"].ToString().Trim() +
                   "\" bb=\"" + objPOdt.Rows[i]["bb"].ToString().Trim() + "\"/>");

           }
           xmlData.Append("</ToolBar>");
           XmlDocument doc = new XmlDocument();
           doc.LoadXml(xmlData.ToString());
           try
           {
               doc.Save("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\ColorLump" + Colortype + ".xml");
           }
           catch (System.Exception ex)
           {
               throw ex;
           }

       }
    }
}
