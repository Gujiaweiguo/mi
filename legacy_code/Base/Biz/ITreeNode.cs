
using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;

namespace Base.Biz
{
    public interface ITreeNode
    {
        /**
         * 获取节点的值
         */
        String GetValue();

        /**
         * 获取节点的显示文本
         */
        String GetText();

        /**
         * 获得节点的提示信息
         */
        String GetTip();

        /**
         * 添加子节点
         */
        void AddChild(ITreeNode childNode);

        /**
         * 获取子节点的列表
         */
        List<ITreeNode> GetChildren();

        /**
         * 设置父节点
         */
        void SetParent(ITreeNode parent);

        /**
         * 获取父节点
         */
        ITreeNode GetParent();

        /**
         * 获取子节点时使用的where条件，格式如："PDeptID="+DeptID
         */
        String GetChildrenWhere();

        /**
         * 获取根结点时需要的where条件，格式如："DeptLevle=1"
         */
        String GetRootWhere();
    }
}
