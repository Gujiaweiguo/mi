using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using Base.Biz;
using System.Data;

namespace Base.Sys
{
    public class FloorMenuXML 
    {
        public void GetXml(string userDoc, string wheresql)
        {

            Directory.CreateDirectory("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc);
            bool xmlFiles = File.Exists("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\Floor.xml");
            if (xmlFiles)
            {

                File.Delete("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\Floor.xml");

            }
            BaseBO baseBO = new BaseBO();
            string wherePO = @"SELECT LOCALAREA.LocalID,DEPT.DeptID as cityid,a.deptid as MallID,building.buildingid,floors.floorid,
                                floorname as floorDesc ,DEPT.DEPTNAME AS adress ,
                                case when floorcode='B2' THEN 'Img/floorM1B1001.png' when floorcode='B1' THEN 'Img/floorM1B1001.png'
                                 WHEN floorcode='L1' THEN 'Img/floorM1B1001.png'WHEN floorcode='L2' THEN 'Img/floorM1B1002.png' 
                                WHEN floorcode='L3' THEN 'Img/floorM1B1003.png'WHEN floorcode='L4' THEN 'Img/floorM1B1004.png'
                                 WHEN floorcode='L5' THEN 'Img/floorM1B1005.png' ELSE 'Img/floorM1B1001.png' END AS Img,
                                '' as remark,'' as url
                                FROM LOCALAREA 
                                INNER JOIN LOCALCITY ON LOCALAREA.LOCALID=LOCALCITY.LOCALID
                                INNER JOIN DEPT ON (LOCALCITY.DEPTID=DEPT.DEPTID)
                                inner join dept a on (dept.deptid=a.pdeptid)
                                inner join Building on a.DEPTID=Building.StoreID
                                inner join floors on building.buildingid=floors.buildingid
                                left join authuser on floors.floorid=authuser.floorid " + wheresql;
            DataTable objPOdt = baseBO.QueryDataSet(wherePO).Tables[0];
            StringBuilder xmlData = new StringBuilder();
            xmlData.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            xmlData.Append("<FloorInfor nm='Â¥²ã'>");
            for (int i = 0; i < objPOdt.Rows.Count; i++)
            {
                xmlData.Append("<floor locaID='" + objPOdt.Rows[i]["LocalID"].ToString().Trim() +
                    "' cityID='" + objPOdt.Rows[i]["cityid"].ToString().Trim() +
                    "' mallID='" + objPOdt.Rows[i]["MallID"].ToString().Trim() +
                    "' buildID='" + objPOdt.Rows[i]["buildingid"].ToString().Trim() +
                    "' floorID='" + objPOdt.Rows[i]["floorid"].ToString().Trim() +
                    "' desc='" + objPOdt.Rows[i]["floorDesc"].ToString().Trim() +
                    "' adress='" + objPOdt.Rows[i]["adress"].ToString().Trim() +
                    "' Img='" + objPOdt.Rows[i]["Img"].ToString().Trim() +
                    "' remark='" + objPOdt.Rows[i]["remark"].ToString().Trim() +
                    "' url='" + objPOdt.Rows[i]["url"].ToString().Trim() + "'/>");

            }
            xmlData.Append("</FloorInfor>");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlData.ToString());
            try
            {
                doc.Save("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\Floor.xml");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
    }
}
