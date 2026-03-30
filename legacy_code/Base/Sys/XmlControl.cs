using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Base.Sys
{
    public class XmlControl
    {
        protected string strXmlFile;
        protected XmlDocument objXmlDoc = new XmlDocument();


        public XmlControl(string XmlFile)
        {
            strXmlFile = XmlFile;
        }

        public void BuildXml()
        {
            XmlDeclaration XmlDec;
            XmlDec = objXmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            objXmlDoc.AppendChild(XmlDec);
        }

        public void Replace(string XmlPathNode, string Content)
        {
            //更新节点內容。
            objXmlDoc.SelectSingleNode(XmlPathNode).InnerText = Content;
        }

        /// <summary>
        /// 删除一个节点。
        /// </summary>
        public void Delete(string Node)
        {
            string mainNode = Node.Substring(0, Node.LastIndexOf("/"));
            objXmlDoc.SelectSingleNode(mainNode).RemoveChild(objXmlDoc.SelectSingleNode(Node));
        }
        /// <summary>
        /// 插入根节点
        /// </summary>
        public void InsertRoot(string MainNode)
        {
            XmlElement objMainNode = objXmlDoc.CreateElement(MainNode);
            objXmlDoc.AppendChild(objMainNode);
        }

        /// <summary>
        /// 插入根节点带参数
        /// </summary>
        public void InsertRoot(string MainNode, string Attrib, string AttribContent)
        {
            XmlElement objMainNode = objXmlDoc.CreateElement(MainNode);
            objMainNode.SetAttribute(Attrib, AttribContent);
            objXmlDoc.AppendChild(objMainNode);
        }


        /// <summary>
        /// 插入一个节点元素，带一个属性。。
        /// </summary>
        public XmlElement InsertNode(string MainNode, string Element, string Attrib, string AttribContent, string Content)
        {
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.SetAttribute(Attrib, AttribContent);
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);

            return objElement;
        }

        /// <summary>
        /// 插入一个节点，带一个属性和文本。
        /// </summary>
        public XmlElement InsertNode(string MainNode, string Element, string Attrib, string AttribContent)
        {
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.SetAttribute(Attrib, AttribContent);
            objNode.AppendChild(objElement);

            return objElement;
        }

        /// <summary>
        /// 插入一个节点，不带属性带文本。
        /// </summary>
        public XmlElement InsertNode(string MainNode, string Element, string Content)
        {
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);

            return objElement;
        }

        /// <summary>
        /// 插入一个节点元素，带属性和文本。
        /// </summary>
        public void InsertElement(XmlElement xmlElement,string Element, string Attrib, string AttribContent, string Content)
        {
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.SetAttribute(Attrib, AttribContent);
            objElement.InnerText = Content;
            xmlElement.AppendChild(objElement);
        }

        /// <summary>
        /// 插入一个节点元素，带文本。
        /// </summary>
        public void InsertElement(XmlElement xmlElement, string Element,string Content)
        {
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.InnerText = Content;
            xmlElement.AppendChild(objElement);
        }

        /// <summary>
        /// 插入一个节点元素，带属性，不带文本
        /// </summary>
        public XmlElement InsertElementNext(XmlElement xmlElement, string Element, string Attrib, string AttribContent)
        {
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.SetAttribute(Attrib, AttribContent);
            xmlElement.AppendChild(objElement);

            return objElement;
        }


        /// <summary>
        /// 插入一个节点元素，带文本,带下面子节点
        /// </summary>
        public void InsertElementAddElement(XmlElement xmlElement, string Element, string Content, string ElementName, string ContentName)
        {
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.InnerText = Content;
            xmlElement.AppendChild(objElement);

            XmlElement objElementname = objXmlDoc.CreateElement(ElementName);
            objElementname.InnerText = ContentName;
            objElement.AppendChild(objElementname);
        }

        /// <summary>
        /// 保存文档
        /// </summary>
        public void Save()
        {
            try
            {
                objXmlDoc.Save(strXmlFile);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            objXmlDoc = null;
        }
    }
}
