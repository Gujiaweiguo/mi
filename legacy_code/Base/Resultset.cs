using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Base.DB
{
    /**
     * 该类实现了对数据库结果集以po对象方式的存储、管理等操作
     */
    public class Resultset : IEnumerable
    {
        private Queue<BasePO> queue = new Queue<BasePO>();

        public void Add(BasePO po)
        {
            this.queue.Enqueue(po);
        }

        public BasePO Dequeue()
        {
            return this.queue.Dequeue();
        }

        public int Count
        {
            get { return this.queue.Count; }
        }

        public IEnumerator GetEnumerator()
        {
            return this.queue.GetEnumerator();
        }
    }
}
