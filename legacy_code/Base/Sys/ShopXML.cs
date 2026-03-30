using System;
using System.Text;
using System.Data;
using Base.Biz;
using Base.XML;
using System.Xml;
using System.IO;

namespace Base.Sys
{
    public class ShopXML
    {
        public void GetXml( string userDoc,string wheresql,int shoptype)
        {

            Directory.CreateDirectory("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc);
            bool xmlFiles = File.Exists("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\Shop" + shoptype + ".xml");
            if (xmlFiles)
            {

                File.Delete("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\Shop" + shoptype + ".xml");

            }
            BaseBO baseBO = new BaseBO();
            ShopXMLInfo shopxmlinfo = new ShopXMLInfo(wheresql, shoptype);
            DataTable objPOdt = baseBO.QueryDataSet(shopxmlinfo).Tables[0];
            StringBuilder xmlData = new StringBuilder();
            xmlData.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            xmlData.Append("<shopInformation>");
            String[] Nodeinfos = { "locaID", "cityID", "mallID", "buildID", "floorID" };//截取节点属性数组
            //将objPOdt中的重复数据过滤掉
            DataTable objInfodt = new DataTable();
            DataView myDataView = new DataView(objPOdt);
            objInfodt = myDataView.ToTable(true, Nodeinfos);
            for (int i = 0; i < objInfodt.Rows.Count; i++)
            {
                xmlData.Append("<ShopInfor locaID=\"" + objInfodt.Rows[i]["locaID"].ToString().Trim() + "\" cityID=\"" + objInfodt.Rows[i]["cityID"].ToString().Trim() +
                    "\" mallID=\"" + objInfodt.Rows[i]["mallID"].ToString().Trim() + "\" buildID=\"" + objInfodt.Rows[i]["buildID"].ToString().Trim() +
                    "\" floorID=\"" + objInfodt.Rows[i]["floorID"].ToString().Trim() + "\">");
                for (int j = 0; j < objPOdt.Rows.Count; j++)
                {
                    if (objPOdt.Rows[j]["locaID"].ToString().Trim() == objInfodt.Rows[i]["locaID"].ToString().Trim() && objPOdt.Rows[j]["cityID"].ToString().Trim() == objInfodt.Rows[i]["cityID"].ToString().Trim() && objPOdt.Rows[j]["mallID"].ToString().Trim() == objInfodt.Rows[i]["mallID"].ToString().Trim() && objPOdt.Rows[j]["buildID"].ToString().Trim() == objInfodt.Rows[i]["buildID"].ToString().Trim() && objPOdt.Rows[j]["floorID"].ToString().Trim() == objInfodt.Rows[i]["floorID"].ToString().Trim())
                    {
                        xmlData.Append("<Shop shopID=\"" + objPOdt.Rows[j]["shopID"].ToString().Trim() + "\" UnitID=\"" + objPOdt.Rows[j]["UnitID"].ToString().Trim() +
                            "\" ShopCode=\"" + objPOdt.Rows[j]["ShopCode"].ToString().Trim() + "\" ShopDesc=\"" + objPOdt.Rows[j]["ShopDesc"].ToString().Trim() +
                            "\" FloorArea=\"" + objPOdt.Rows[j]["FloorArea"].ToString().Trim() + "\" RentArea=\"" + objPOdt.Rows[j]["RentArea"].ToString().Trim() +
                            "\" RentStatus=\"" + objPOdt.Rows[j]["RentStatus"].ToString().Trim() + "\" Brand=\"" + objPOdt.Rows[j]["Brand"].ToString().Trim() +
                            "\" Customer=\"" + objPOdt.Rows[j]["Customer"].ToString().Trim() + "\" map=\"" + objPOdt.Rows[j]["map"].ToString().Trim() +
                            "\" plan=\"" + objPOdt.Rows[j]["plans"].ToString().Trim() + "\" x=\"" + objPOdt.Rows[j]["x"].ToString().Trim() +
                            "\" y=\"" + objPOdt.Rows[j]["y"].ToString().Trim() + "\" depth=\"" + objPOdt.Rows[j]["depth"].ToString().Trim() +
                            "\" rb=\"" + objPOdt.Rows[j]["rb"].ToString().Trim() + "\" gb=\"" + objPOdt.Rows[j]["gb"].ToString().Trim() +
                            "\" bb=\"" + objPOdt.Rows[j]["bb"].ToString().Trim() + "\" NoX=\"" + objPOdt.Rows[j]["NoX"].ToString().Trim() +
                            "\" NoY=\"" + objPOdt.Rows[j]["NoY"].ToString().Trim() + "\" NameX=\"" + objPOdt.Rows[j]["NameX"].ToString().Trim() +
                            "\" NameY=\"" + objPOdt.Rows[j]["NameY"].ToString().Trim() + "\" Remark=\"" + objPOdt.Rows[j]["Remark"].ToString().Trim() + "\" />");
                    }
                }
                xmlData.Append("</ShopInfor>");
            }
            xmlData.Append("</shopInformation>"); 
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlData.ToString());
            try
            {
                string stttt = shoptype == 0 ? "" : shoptype.ToString();
                doc.Save("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\Shop" + stttt  + ".xml");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
    }
}
