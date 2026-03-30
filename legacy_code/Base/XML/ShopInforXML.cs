using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using Base.DB;
using Base.Biz;

namespace Base.XML
{
    public class ShopInforXML
    {
        public void GetXml(string strFilePath)
        {
            XmlDocument doc = new XmlDocument();
            BaseBO basebo = new BaseBO();
            string strwhere = "select shopid,UnitID,ShopCode,ShopDesc,FloorArea,RentArea,RentStatus,Brand,Customer,map,plans,x,y,depth,rb,gb,bb,NoX,NoY,NameX,NameY,Remark from shopxml";
            DataTable dt = basebo.QueryDataSet(strwhere).Tables[0];
            string strShop = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strShop = strShop + "<Shop shopID='" + dt.Rows[i][0].ToString() + "' UnitID='" + dt.Rows[i][1].ToString() + "' ShopCode='" + dt.Rows[i][2].ToString() + "' ShopDesc='" + dt.Rows[i][3].ToString() + "' FloorArea='" + dt.Rows[i][4].ToString() + "' RentArea='" + dt.Rows[i][5].ToString() + "' RentStatus='" + dt.Rows[i][6].ToString() + "' Brand='" + dt.Rows[i][7].ToString() + "' Customer='" + dt.Rows[i][8].ToString() + "' map='" + dt.Rows[i][9].ToString() + "' plan='" + dt.Rows[i][10].ToString() + "' x='" + dt.Rows[i][11].ToString() + "' y='" + dt.Rows[i][12].ToString() + "' depth='" + dt.Rows[i][13].ToString() + "' rb='" + dt.Rows[i][14].ToString() + "' gb='" + dt.Rows[i][15].ToString() + "' bb='" + dt.Rows[i][16].ToString() + "' NoX='" + dt.Rows[i][17].ToString() + "' NoY='" + dt.Rows[i][18].ToString() + "' NameX='" + dt.Rows[i][19].ToString() + "' NameY='" + dt.Rows[i][20].ToString() + "' Remark='" + dt.Rows[i][21].ToString() + "' />";


                }
            }
            doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<shopInformation>" +
                "<ShopInfor locaID='001' cityID='1' mallID='M001' buildID='B001' floorID='F001'>" +
                strShop+
                "</ShopInfor>"+
                "</shopInformation>");

            try
            {
                doc.Save(strFilePath);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
