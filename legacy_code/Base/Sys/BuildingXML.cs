using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Base.Biz;
using System.Data;
using System.Xml;

namespace Base.Sys
{
    public class BuildingXML
    {
        public void GetXml(string userDoc)
        {
            Directory.CreateDirectory("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc);
            bool xmlFiles = File.Exists("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\Building.xml");
            if (xmlFiles)
            {

                File.Delete("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\Building.xml");

            }
            BaseBO baseBO = new BaseBO();
            string wherePO = @"SELECT loca.deptid LocalID,city.DeptID as cityid,store.StoreId as MallID,Building.BuildingID,
                                Building.BuildingName as buildingDesc ,store.StoreAddr AS adress ,
                                isnull(building.img, '../../VAGraphic/Img/building.png') as Img,'' as remark,'' as url
                                FROM Building inner join Store on Store.StoreId=Building.StoreId inner join Dept on Store.StoreId=Dept.deptid 
                                left join (select deptid,deptname,PDeptID from Dept where DeptType=5 and DeptStatus=1) city on (city.DeptID=Dept.PDeptID)
                                left join (select deptid,deptname,PDeptID from Dept where DeptType=4 and DeptStatus=1) loca on (loca.DeptID=city.PDeptID)
                                where store.storestatus=1";
            DataTable objPOdt = baseBO.QueryDataSet(wherePO).Tables[0];
            StringBuilder xmlData = new StringBuilder();
            xmlData.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            xmlData.Append("<BuildingInfor nm='´óÂ¥'>");
            for (int i = 0; i < objPOdt.Rows.Count; i++)
            {
                xmlData.Append("<mall locaID=\"" + objPOdt.Rows[i]["LocalID"].ToString().Trim() +
                    "\" cityID=\"" + objPOdt.Rows[i]["cityid"].ToString().Trim() +
                    "\" mallID=\"" + objPOdt.Rows[i]["MallID"].ToString().Trim() +
                    "\" buildID=\"" + objPOdt.Rows[i]["buildingid"].ToString().Trim() +
                    "\" desc=\"" + objPOdt.Rows[i]["buildingDesc"].ToString().Trim() +
                    "\" adress=\"" + objPOdt.Rows[i]["adress"].ToString().Trim() +
                    "\" Img=\"" + objPOdt.Rows[i]["Img"].ToString().Trim() +
                    "\" remark=\"" + objPOdt.Rows[i]["remark"].ToString().Trim() +
                    "\" url=\"" + objPOdt.Rows[i]["url"].ToString().Trim() + "\"/>");

            }
            xmlData.Append("</BuildingInfor>");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlData.ToString());
            try
            {
                doc.Save("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\Building.xml");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
    }
   
}
