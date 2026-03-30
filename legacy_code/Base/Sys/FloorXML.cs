using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using Base.Biz;
using System.Data;

namespace Base.Sys
{ 
    public class FloorXML 
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
            string wherePO = @"SELECT loca.DeptID LocalID,city.DeptID as cityid,Store.StoreId as MallID,floors.buildingid,floors.floorid,
                floorname as floorDesc ,Store.StoreAddr AS adress ,ISNULL(floors.img,'../../VAGraphic/Img/f1.png') img,'' as remark,'' as url
                FROM floors inner join Store on Store.StoreId=floors.StoreID  inner join Dept on Store.StoreId=Dept.deptid 
                left join (select deptid,deptname,PDeptID from Dept where DeptType=5 and DeptStatus=1) city on (city.DeptID=Dept.PDeptID)
                left join (select deptid,deptname,PDeptID from Dept where DeptType=4 and DeptStatus=1) loca on (loca.DeptID=city.PDeptID)
                where store.storestatus=1 " + wheresql;
            DataTable objPOdt = baseBO.QueryDataSet(wherePO).Tables[0];
            StringBuilder xmlData = new StringBuilder();
            xmlData.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            xmlData.Append("<FloorInfor nm='Â¥²ã'>");
            for (int i = 0; i < objPOdt.Rows.Count; i++)
            {
                xmlData.Append("<floor locaID=\"" + objPOdt.Rows[i]["LocalID"].ToString().Trim() +
                    "\" cityID=\"" + objPOdt.Rows[i]["cityid"].ToString().Trim() +
                    "\" mallID=\"" + objPOdt.Rows[i]["MallID"].ToString().Trim() +
                    "\" buildID=\"" + objPOdt.Rows[i]["buildingid"].ToString().Trim() +
                    "\" floorID=\"" + objPOdt.Rows[i]["floorid"].ToString().Trim() +
                    "\" desc=\"" + objPOdt.Rows[i]["floorDesc"].ToString().Trim() +
                    "\" adress=\"" + objPOdt.Rows[i]["adress"].ToString().Trim() +
                    "\" Img=\"" + objPOdt.Rows[i]["Img"].ToString().Trim() +
                    "\" remark=\"" + objPOdt.Rows[i]["remark"].ToString().Trim() +
                    "\" url=\"" + objPOdt.Rows[i]["url"].ToString().Trim() + "\"/>");

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

