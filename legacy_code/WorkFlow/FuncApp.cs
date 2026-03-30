using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;

using WorkFlow.Func;
using WorkFlow.WrkFlw;

namespace WorkFlow
{
    /**
     * 该类实现了应用调用接口的方法
     */
    public class FuncApp
    {
        //调用该接口时的操作 － 确认
        public static int OPER_TYPE_CONFIRM = 1;
        //调用该接口时的操作 － 驳回
        public static int OPER_TYPE_REJECT = 2;

        /**
         * 根据工作流内码、节点内码获取该节点的业务处理对象
         */
        public static IConfirmVoucher GetConfirmVoucherInterface(int wrkFlwID, int nodeID)
        {
            //获取处理对象类名，格式：assembly:class
            WrkFlwNode wrkFlwNode = WrkFlwApp.GetWrkFlwNode(wrkFlwID, nodeID);
            if (wrkFlwNode == null)
            {
                throw new WrkFlwException("No workflow node defination.wrkflwid:" + wrkFlwID + "-nodeid:" + nodeID);
            }
            String processClass = wrkFlwNode.ProcessClass;
            if (processClass == null || processClass.Trim().Length == 0)
            {
                throw new WrkFlwException("No process class name defination for workflow node.wrkflwid:" + wrkFlwID + "-nodeid:" + nodeID);
            }
            String[] assemblyClass = processClass.Split(new char[] { ':' });
            if (assemblyClass.Length != 2)
            {
                throw new WrkFlwException("Process class name format is wrong.classname:" + processClass);
            }
            String assemblyName = assemblyClass[0];
            String className = assemblyClass[1];
            //生成对象
            ObjectHandle objHanle = Activator.CreateInstance(assemblyName, className);
            IConfirmVoucher obj = (IConfirmVoucher)objHanle.Unwrap();
            return obj;
        }

        /**
         * 根据工作流内码获取该工作流的业务处理对象
         */
        public static ITransitWrkFlw GetTransitWrkFlwInterface(int wrkFlwID)
        {
            //获取处理对象类名，格式：assembly:class
            WrkFlwBO bo = new WrkFlwBO();
            WrkFlw.WrkFlw wrkFlw = bo.GetWrkFlw(wrkFlwID);
            if (wrkFlw == null)
            {
                throw new WrkFlwException("No workflow defination.wrkflwid:" + wrkFlwID);
            }
            String processClass = wrkFlw.ProcessClass;
            if (processClass == null || processClass.Trim().Length == 0)
            {
                throw new WrkFlwException("No process class name defination for workflow.wrkflwid:" + wrkFlwID);
            }
            String[] assemblyClass = processClass.Split(new char[] { ':' });
            if (assemblyClass.Length != 2)
            {
                throw new WrkFlwException("Process class name format is wrong for workflow.classname:" + processClass);
            }
            String assemblyName = assemblyClass[0];
            String className = assemblyClass[1];
            //生成对象
            ObjectHandle objHanle = Activator.CreateInstance(assemblyName, className);
            ITransitWrkFlw obj = (ITransitWrkFlw)objHanle.Unwrap();
            return obj;
        }
    }
}
