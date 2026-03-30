using System;
using System.Collections.Generic;
using System.Text;

namespace YYControls.SmartGridViewFunction
{
    /// <summary>
    /// 扩展功能类，抽象类
    /// </summary>
    public abstract class ExtendFunction
    {
        /// <summary>
        /// SmartGridView对象变量
        /// </summary>
        protected SmartGridView _sgv;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ExtendFunction()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sgv">SmartGridView对象</param>
        public ExtendFunction(SmartGridView sgv)
        {
            this._sgv = sgv;
        }

        /// <summary>
        /// SmartGridView对象
        /// </summary>
        public SmartGridView SmartGridView
        {
            get { return this._sgv; }
            set { this._sgv = value; }
        }

        /// <summary>
        /// 实现扩展功能
        /// </summary>
        public void Complete()
        {
            if (this._sgv == null)
            {
                throw new ArgumentNullException("SmartGridView", "扩展功能时未设置SmartGridView对象");
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
