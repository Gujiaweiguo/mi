using System;
using System.Collections.Generic;
using System.Text;

namespace YYControls.SmartTreeViewFunction
{
    /// <summary>
    /// 扩展功能类，抽象类
    /// </summary>
    public abstract class ExtendFunction
    {
        /// <summary>
        /// SmartTreeView对象变量
        /// </summary>
        protected SmartTreeView _stv;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ExtendFunction()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stv">SmartTreeView对象</param>
        public ExtendFunction(SmartTreeView stv)
        {
            this._stv = stv;
        }

        /// <summary>
        /// SmartTreeView对象
        /// </summary>
        public SmartTreeView SmartTreeView
        {
            get { return this._stv; }
            set { this._stv = value; }
        }

        /// <summary>
        /// 实现扩展功能
        /// </summary>
        public void Complete()
        {
            if (this._stv == null)
            {
                throw new ArgumentNullException("SmartTreeView", "扩展功能时未设置SmartTreeView对象");
            }
            else
            {
                Execute();
            }
        }

        /// <summary>
        /// 扩展功能的具体实现
        /// </summary>
        protected abstract void Execute();
    }
}
