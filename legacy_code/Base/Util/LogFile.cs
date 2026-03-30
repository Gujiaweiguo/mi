using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Base.Util
{
    /**
     * 该类实现了日志信息输出的功能，包括输出到文件和控制台
     */
    class LogFile
    {
        private bool logToConsole = true;
        private bool logOnDate = true;
        private String date = null;
        private String logFile = "Log_";
        private StreamWriter writer = null;

        public LogFile()
        {
            Init();
        }

        public LogFile(String logFile, bool logToConsole, bool logOnDate)
        {
            this.logFile = logFile;
            this.logToConsole = logToConsole;
            this.logOnDate = logOnDate;
            Init();
        }

        public void Log(String msg)
        {
            Init();
            writer.WriteLine("["+System.DateTime.Now+"] "+msg);
            writer.Flush();
            if (this.logToConsole)
            {
                Console.WriteLine("[" + System.DateTime.Now + "] " + msg);
            }
        }

        public void Log(String msg, Exception ex)
        {
            Log(msg);
            writer.WriteLine(ex.Message);
            writer.WriteLine(ex.StackTrace);
            writer.Flush();
            if (this.logToConsole)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        public bool LogToConsole
        {
            get { return this.logToConsole; }
        }

        public bool LogOnDate
        {
            get { return this.logOnDate; }
        }

        public String Date
        {
            get { return this.date; }
        }
        /**
         * 初始化日志文件
         */
        private void Init()
        {
            DateTime dt = System.DateTime.Today;
            String curDate = dt.Year + "-" + (dt.Month < 10 ? "0" : "") + dt.Month + "-" + (dt.Day < 10 ? "0" : "") + dt.Day;

            if (writer == null || (logOnDate && !curDate.Equals(this.date)))
            {
                String fileName = null;
                if (logOnDate)
                {
                    this.date = curDate;
                    fileName = logFile + curDate + ".log";
                }
                else
                {
                    fileName = logFile + ".log";
                }
                if (writer != null)
                {
                    writer.Close();
                    writer = null;
                }
                writer = new StreamWriter(fileName, true);
            }

        }
        
    }
}
