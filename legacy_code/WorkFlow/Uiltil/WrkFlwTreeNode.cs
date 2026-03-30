using System;
using System.Collections.Generic;
using System.Text;

namespace WorkFlow.Uiltil
{
    public class WrkFlwTreeNode
    {
        private List<WrkFlwTreeNode> childrenNodes = new List<WrkFlwTreeNode>();
        private WrkFlwTreeNode parentNode = null;

        private String nodeText = null;
        private String nodeValue = null;
        private String imageURL = null;
        private int count = 0;

        public WrkFlwTreeNode(String nodeText, String nodeValue, String imageURL)
        {
            this.nodeText = nodeText;
            this.nodeValue = nodeValue;
            this.imageURL = imageURL;
        }

        public int IncreaseCount(int count)
        {
            this.count += count;
            if (this.ParentNode != null)
            {
                this.ParentNode.IncreaseCount(count);
            }
            return this.count;
        }

        public int DecreaseCount(int count)
        {
            this.count -= count;
            if (this.ParentNode != null)
            {
                this.ParentNode.DecreaseCount(count);
            }
            return this.count;
        }

        public String NodeText
        {
            get { return this.nodeText + (this.Count > 0 ? "(" + this.Count + ")" : ""); }
        }

        public String NodeValue
        {
            get { return this.nodeValue; }
        }
        public String ImageURL
        {
            get { return this.imageURL; }
        }

        public WrkFlwTreeNode ParentNode
        {
            get { return this.parentNode; }
            set { this.parentNode = value; }
        }

        public int Count
        {
            get { return this.count; }
            set { this.count = value; }
        }

        public List<WrkFlwTreeNode> ChildNodes
        {
            get { return this.childrenNodes; }
        }
    }
}
