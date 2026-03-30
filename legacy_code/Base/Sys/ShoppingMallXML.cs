using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Base.Biz;
using System.Data;
using System.Xml;

namespace Base.Sys
{
    public class ShoppingMallXML
    {
        public void GetXml(string userDoc)
        {
            Directory.CreateDirectory("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc);
            bool xmlFiles = File.Exists("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\Mall.xml");
            if (xmlFiles)
            {

                File.Delete("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\Mall.xml");

            }
            BaseBO baseBO = new BaseBO();
            string wherePO = @"SELECT loca.deptid LocalID,city.DeptID as cityid,Store.storeid as MallID,
                Store.StoreName as MallDesc,store.StoreAddr AS adress ,
               isnull(store.img,'../../VAGraphic/Img/mall.png') as Img,'' as remark,'' as url
                FROM Store 
                inner join Dept on (Dept.DeptID=Store.StoreId) 
                left join (select deptid,deptname,PDeptID from Dept where DeptType=5 and DeptStatus=1) city on (city.DeptID=Dept.PDeptID)
                left join (select deptid,deptname,PDeptID from Dept where DeptType=4 and DeptStatus=1) loca on (loca.DeptID=city.PDeptID)
                where store.storestatus=1";
            DataTable objPOdt = baseBO.QueryDataSet(wherePO).Tables[0];
            StringBuilder xmlData = new StringBuilder();
            xmlData.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            xmlData.Append("<MallInfor>");
            for (int i = 0; i < objPOdt.Rows.Count; i++)
            {
                xmlData.Append("<mall locaID=\"" + objPOdt.Rows[i]["LocalID"].ToString().Trim() +
                    "\" cityID=\"" + objPOdt.Rows[i]["cityid"].ToString().Trim() +
                    "\" mallID=\"" + objPOdt.Rows[i]["MallID"].ToString().Trim() +
                    "\" desc=\"" + objPOdt.Rows[i]["MallDesc"].ToString().Trim() +
                    "\" adress=\"" + objPOdt.Rows[i]["adress"].ToString().Trim() +
                    "\" Img=\"" + objPOdt.Rows[i]["Img"].ToString().Trim() +
                    "\" remark=\"" + objPOdt.Rows[i]["remark"].ToString().Trim() +
                    "\" url=\"" + objPOdt.Rows[i]["url"].ToString().Trim() + "\"/>");

            }
            xmlData.Append("</MallInfor>");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlData.ToString());
            try
            {
                doc.Save("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\Mall.xml");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
    }
}
