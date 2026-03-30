using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Base.Util
{
    /**
     * 该类实现了应用程序日志的记录，日志分为三类：系统日志、业务日志、跟踪日志
     */
    public class Logger
    {
        /**
         * 是否将日志输出到控制台
         */
        private static bool logToConsole = true;
        /**
         * 是否按日期记录日志文件
         */
        private static bool logOnDate = true;
        /**
         * 是否记录跟踪日志
         */
        private static bool traceOn = true;

        private static String LOG_DIR = "E:/work/mi_net/Code/Log/";
        private static String LOG_FILE_NAME = LOG_DIR + "Log_";
        private static String BIZ_FILE_NAME = LOG_DIR + "Biz_";
        private static String TRACE_FILE_NAME = LOG_DIR + "Trace_";

        private static LogFile logFile = null;
        private static LogFile bizFile = null;
        private static LogFile traceFile = null;

        private static Logger instance = new Logger();

        private DirectoryInfo info = Directory.CreateDirectory(LOG_DIR);

        /**
         * 记录系统日志
         * msg：待记录的字符串
         */
        public static void Log(String msg)
        {
            if (logFile == null)
            {
                logFile = new LogFile(LOG_FILE_NAME, logToConsole, logOnDate);
            }
            logFile.Log(msg);
        }

        /**
         * 记录系统日志
         * msg：待记录的字符串
         * ex：异常
         */
        public static void Log(String msg, Exception ex)
        {
            if (logFile == null)
            {
                logFile = new LogFile(LOG_FILE_NAME, logToConsole, logOnDate);
            }
            logFile.Log(msg, ex);
        }

        /**
         * 记录业务日志
         * msg：待记录的字符串
         */
        public static void LogBiz(String msg)
        {
            if (bizFile == null)
            {
                bizFile = new LogFile(BIZ_FILE_NAME, logToConsole, logOnDate);
            }
            bizFile.Log(msg);
        }

        /**
         * 记录业务日志
         * msg：待记录的字符串
         * ex：异常
         */
        public static void LogBiz(String msg, Exception ex)
        {
            if (bizFile == null)
            {
                bizFile = new LogFile(BIZ_FILE_NAME, logToConsole, logOnDate);
            }
            bizFile.Log(msg, ex);
        }

        /**
         * 记录跟踪日志
         * msg：待记录的字符串
         */
        public static void Trace(String msg)
        {
            if (traceFile == null)
            {
                traceFile = new LogFile(TRACE_FILE_NAME, logToConsole, logOnDate);
            }
            traceFile.Log(msg);
        }

        /**
         * 记录跟踪日志
         * msg：待记录的字符串
         * ex：异常
         */
        public static void Trace(String msg, Exception ex)
        {
            if (traceFile == null)
            {
                traceFile = new LogFile(TRACE_FILE_NAME, logToConsole, logOnDate);
            }
            traceFile.Log(msg, ex);
        }

        public static bool LogToConsole
        {
            get { return logToConsole; }
            set { logToConsole = value; }
        }

        public static bool LogOnDate
        {
            get { return logOnDate; }
            set { logOnDate = value; }
        }

        public static bool TraceOn
        {
            get { return traceOn; }
            set { traceOn = value; }
        }
    }
}
