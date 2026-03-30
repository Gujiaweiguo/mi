using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections;

namespace Base.Sys
{
    /**
     * 实现对象池的管理，该类为抽象类，需要由实际的实现类提供对具体的对象（如数据库连接、线程等）进行管理。
     * 1、支持对象的检入、检出；
     * 2、对象的创建、释放、有效性检查由子类完成
     * 3、支持对象最小、最大数量的配置
     * 4、支持对象最小可用数量的配置（在对象总数量不超过最大数量的前提下）
     */
    abstract class ObjectPool
    {
        /**
         * 对象清理线程
         */
        private CleanupThread cleaner = null;

        /**
         * 对象池参数
         */
        // 对象失效时长(milliseconds)
        private int expireTime = 30 * 1000;
        // 检出对象允许的最长耗时(milliseconds)
        private int checkOutTimeout = 1 *1000;
        // 对象的最大数量
        private int maxObjNum = 100;
        // 对象的最小数量
        private int minObjNum = 1;

        /**
         * 对象容器
         */
        //已被检出对象池
        private Hashtable lockedPool = null;
        //未被检出对象池
        private Hashtable unlockedPool = null;

        /**
         * 使用缺省值构造对象池
         */
        protected ObjectPool()
        {
            Init();
        }

        /**
         * 使用传输的参数值，构造对象池
         * 
         */
        protected ObjectPool(int expireTime, int checkOutTimeout, int objMaxNum, int objMinNum)
        {
            this.expireTime = expireTime;
            this.checkOutTimeout = checkOutTimeout;
            this.maxObjNum = objMaxNum;
            this.minObjNum = objMinNum;
            Init();
        }

        /**
         * 检入对象，将对象归还到对象池中
         * obj 待检入的对象
         */
        protected void CheckIn(Object obj) {
            lock (this)
            {
                if (obj != null)
                {
                    lockedPool.Remove(obj);
                    unlockedPool.Add(obj, System.DateTime.Now.Millisecond);
                    Monitor.PulseAll(this);
                }
            }
        }


        /**
         * 检出对象，从对象池中获取对象，如果对象已用完，则创建新的对象
         * 如果创建的对象个数达到指定的最大个数，则等待其它线程释放对象
         * 直到获取；如果有定义等待超时，如果在指定时间内还没有获取到对
         * 象，则抛出获取对象超时异常
         * 
         * @return Object
         */
        protected Object CheckOut()
        {
            lock (this)
            {
                int now = System.DateTime.Now.Millisecond;
                Object obj = null;
                while(obj==null){
                    // 从对象池中获取对象
                    ICollection objList = unlockedPool.Keys;
                    foreach (Object o in objList)
                    {
                        obj = o;
                        if (Validate(obj))
                        {
                            unlockedPool.Remove(obj);
                            lockedPool.Add(obj, now);
                            return (obj);
                        }
                        else
                        {
                            unlockedPool.Remove(obj);
                            Expire(obj);
                            obj = null;
                        }
                    }

                    // 如果对象池中没有可用对象，并且当前对象数小于最大对象数，则创建，如果最大对象数小于等于0，则直接创建对象
                    if (maxObjNum > 0)
                    {
                        if (unlockedPool.Count + lockedPool.Count < maxObjNum)
                        {
                            obj = Create();
                        }
                    }
                    else
                    {
                        obj = Create();
                    }

                    //如果超时，仍未获得对象，则抛出超时异常（如果检出超时小于等于0，则不抛出异常，继续等待）
                    if (obj == null && checkOutTimeout > 0 &&
                        System.DateTime.Now.Millisecond - now > checkOutTimeout)
                    {
                        throw new TimeoutException("Get object timeout in pool");
                    }
                    //如果为检到对象，则等待一个超时周期（如果未设超时，则等待5秒）
                    if (obj == null)
                    {
                        int timeout = checkOutTimeout > 0 ? checkOutTimeout : 5000;
                        Monitor.Wait(this, timeout);
                    }
                }//while
                lockedPool.Add(obj, now);
                return (obj);
            }//lock
        }

        /**
         * 已检出的对象数量
         */
        protected int LockedCount
        {
            get {
                lock (this) { return this.lockedPool.Count; }
            }

        }

        /**
         * 未检出的对象数量
         */
        protected int UnlockedCount
        {
            get {
                lock (this) return this.unlockedPool.Count;
            }
        }

        /**
         * 检出对象允许的最长耗时(milliseconds)
         */
        protected int ExpireTime
        {
            get { return this.expireTime; }
        }

        /**
         * 检出对象允许的最长耗时(milliseconds)
         */
        protected int CheckOutTimeout
        {
            get { return this.checkOutTimeout; }
        }

        /**
         * 对象的最大数量
         */
        protected int MaxObjNum
        {
            get { return this.maxObjNum; }
        }
        /** 
         * 对象的最小数量
         */
        protected int MinObjNum
        {
            get { return this.minObjNum; }
        }

        /**
         * 创建对象
         * @return Object
         * @throws Exception
         */
        protected abstract Object Create();

        /**
         * 释放对象
         * @param o
         */
        protected abstract void Expire(Object obj);

        /**
         * 检查对象是否有效
         * @param obj 待检查的对象
         * @return boolean 对象是否有效，true, validate, otherwise, invalidate
         */
        protected abstract bool Validate(Object obj);

        /**
         * 清除不需要的对象
         */
        internal void Cleanup() {
            lock (this)
            {
                int now = System.DateTime.Now.Millisecond;

                ICollection keys = unlockedPool.Keys;
                foreach (Object obj in keys)
                {
                    if ((this.unlockedPool.Count + this.lockedPool.Count) <= minObjNum)
                    {
                        break;
                    }

                    if (expireTime > 0 &&
                        (now - (int)unlockedPool[obj] > expireTime))
                    {

                        unlockedPool.Remove(obj);
                        Expire(obj);
                    }
                }
                Monitor.PulseAll(this);
            }//lock
        }
  
        /**
         * 初始化对象池
         */
        private void Init()
        {            
            // 初始化容器对象
            this.lockedPool = new Hashtable();
            this.unlockedPool = new Hashtable();

            // 启动清除线程
            this.cleaner = new CleanupThread(this, this.expireTime);
            Thread thread = new Thread(new ThreadStart(this.cleaner.ThreadProc));
            thread.Start();

        }


    }

    /**
     * 清理线程，用来清理空闲时间超过指定时间的对象
     */
    class CleanupThread
    {
        private ObjectPool pool = null;
        private int sleepTime = 0;

        public CleanupThread(ObjectPool pool, int sleepTime)
        {
            this.pool = pool;
            this.sleepTime = sleepTime;
        }

        public void ThreadProc()
        {
            while (true)
            {
                Thread.Sleep(this.sleepTime);
                pool.Cleanup();
            }
        }
    }

}
