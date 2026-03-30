using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
using Base.Biz;
using System.Xml;
using System.Data;
using System.IO;
using Base.Page;


namespace Base.Sys
{
    public class XmlExecutor:BasePage
    {
        public XmlControl xmlControl;
        public void BuildXml(XmlPO xmlPO,BasePO basePO,string sqlWhere,string userDoc)
        {
            
            String[] columns = new String[0];
            try
            {

                BaseBO baseBO = new BaseBO();

                int i = 1;

                baseBO.WhereClause = sqlWhere;

                int type = 0;


                Directory.CreateDirectory("E:\\work\\mi_net\\Code\\Web\\VisualAnalysis\\VAMenu\\" + userDoc);
                //查找对应表中数据


                DataSet dataSet = baseBO.QueryDataSet(basePO);




                
                if (dataSet.Tables[0].Rows.Count > -1)
                {
                    bool xmlFiles = File.Exists("E:\\work\\mi_net\\Code\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\" + xmlPO.GetXmlName() + ".xml");
                    if (xmlFiles)
                    {

                        File.Delete("E:\\work\\mi_net\\Code\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\" + xmlPO.GetXmlName() + ".xml");

                     }

                    xmlControl = new XmlControl("E:\\work\\mi_net\\Code\\Web\\VisualAnalysis\\VAMenu\\" + userDoc + "\\" + xmlPO.GetXmlName() + ".xml");
         

                    if (xmlPO.GetXmlName().ToString() != "Shop")
                    {
                        //创建Xml头文件
                        xmlControl.BuildXml();

                        //创建Xml根目录
                        xmlControl.InsertRoot(xmlPO.GetXmlName());
                    }
                    if (xmlPO.GetXmlName().ToString() == "Shop")
                    {
                        //创建Xml头文件
                        xmlControl.BuildXml();

                        //创建Xml根目录
                        xmlControl.InsertRoot(xmlPO.GetXmlNodeNames());
                    
                    }

                    //查找对应XmlPO中的元素字段
                    String[] columnNodes = xmlPO.GetXmlElementNames().Split(XmlPO.FIELD_SPLITTER);

                    //查找对应BasePO中的字段
                    if (basePO.GetQuerySql() == "")
                    {
                        columns = basePO.GetColumnNames().Split(BasePO.FIELD_SPLITTER);

                    }
                    else
                    {
                        string s = basePO.GetQuerySql().ToString();
                        int ss = s.IndexOf("from");
                        string sss = s.Substring(7, ss - 8);
                        columns = sss.Split(BasePO.FIELD_SPLITTER);

                    }

                    //对应BasePO中数据循环构建Xml内容
                    if (xmlPO.GetXmlNodeNextNames().ToString() != "")
                    {
                        DataSet ds = baseBO.QueryDataSet(xmlPO.GetDataSetSql());
                      
                        for (int jj = 0; jj < ds.Tables[0].Rows.Count; jj++)
                        {
                            XmlElement xmlElement = xmlControl.InsertNode(xmlPO.GetXmlNodeNames(), "ShopInfor", xmlPO.GetXmlNodeNextNames(), ds.Tables[0].Rows[jj]["FloorCode"].ToString());

                            for (int j = 0; j < dataSet.Tables[0].Rows.Count; j++)
                            {


                                if (dataSet.Tables[0].Rows[j]["FloorId"].ToString() == ds.Tables[0].Rows[jj]["FloorCode"].ToString() && dataSet.Tables[0].Rows[j]["x"].ToString() != "" && dataSet.Tables[0].Rows[j]["x"].ToString() != null)
                                {


                                    XmlElement xmlElementNext = xmlControl.InsertElementNext(xmlElement, xmlPO.GetXmlName(), "ID", Convert.ToString(i++) /*dataSet.Tables[0].Rows[j]["ShopID"].ToString()*/);

                                                //创建Xml节点
                                                foreach (String columnNode in columnNodes)
                                                {
                                                    foreach (String column in columns)
                                                    {
                                                        type = 0;
                                                        if (columnNode.Equals(column))
                                                        {
                                                          //  if (dataSet.Tables[0].Rows[j]["x"].ToString() != "" && dataSet.Tables[0].Rows[j]["x"].ToString() != null)
                                                           // {
                                                                xmlControl.InsertElement(xmlElementNext, columnNode, dataSet.Tables[0].Rows[j][columnNode].ToString());
                                                                type = 1;
                                                                break;
                                                           // }
                                                        }
                                                    }

                                                    if (type != 1)
                                                    {
                                                        xmlControl.InsertElement(xmlElementNext, columnNode, " ");
                                                    }
                                                }

                                }

                                
                            }
                        }
                    }
                    else
                    {

                        for (int j = 0; j < dataSet.Tables[0].Rows.Count; j++)
                        {
                            //创建Xml节点
                            if (xmlPO.GetXmlName() != "ColorLumpInformation")
                            {

                                XmlElement xmlElement = xmlControl.InsertNode(xmlPO.GetXmlName(), xmlPO.GetXmlNodeNames(), "ID", Convert.ToString(i++));

                                foreach (String columnNode in columnNodes)
                                {
                                    foreach (String column in columns)
                                    {
                                        type = 0;
                                        if (columnNode.Equals(column))
                                        {
                                            if (columnNode.Equals("MenuDesc"))
                                            {
                                                xmlControl.InsertElement(xmlElement, columnNode, (String)GetGlobalResourceObject("BaseInfo", dataSet.Tables[0].Rows[j][columnNode].ToString()));
                                            }
                                            else
                                            {
                                                xmlControl.InsertElement(xmlElement, columnNode, dataSet.Tables[0].Rows[j][columnNode].ToString());
                                            }
                                            type = 1;
                                            break;
                                        }
                                    }

                                    if (type != 1)
                                    {
                                        xmlControl.InsertElement(xmlElement, columnNode, " ");
                                    }
                                }
                            }
                            else
                            {

                                XmlElement xmlElement = xmlControl.InsertNode(xmlPO.GetXmlName(), xmlPO.GetXmlNodeNames(), "desc", dataSet.Tables[0].Rows[j]["Tradename"].ToString());
                                foreach (String columnNode in columnNodes)
                                {
                                    foreach (String column in columns)
                                    {
                                        type = 0;
                                        if (columnNode.Equals(column))
                                        {
                                            if (columnNode.Equals("MenuDesc"))
                                            {
                                                xmlControl.InsertElement(xmlElement, columnNode, (String)GetGlobalResourceObject("BaseInfo", dataSet.Tables[0].Rows[j][columnNode].ToString()));
                                            }
                                            else
                                            {
                                                xmlControl.InsertElement(xmlElement, columnNode, dataSet.Tables[0].Rows[j][columnNode].ToString());
                                            }
                                            type = 1;
                                            break;
                                        }
                                    }

                                    if (type != 1)
                                    {
                                        xmlControl.InsertElement(xmlElement, columnNode, " ");
                                    }
                                }
                                
                            
                            }
                        }
                        
                    }
                    xmlControl.Save();
                }
            }




            finally
            {

            }
        }
    }
}
