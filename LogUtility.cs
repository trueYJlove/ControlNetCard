using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Engine
{
    public class LogUtility
    {
        private static string logPath = ConfigXml.GetXmlNode("LogPath", "") + "log\\";

        /// <summary>
        /// 多线程同步对象
        /// </summary>
        private static Object SYNC_OBJ = new Object();

        /// <summary>
        /// Log message to application log.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void Log(string message)
        {
            lock (SYNC_OBJ)
            {
                try
                {
                    // make sure we have an event log
                    if (!logPath.EndsWith(@"\"))
                        logPath += @"\";

                    DirectoryInfo di = new DirectoryInfo(logPath);
                    if (!di.Exists)
                        di.Create();

                    TextWriter output = File.AppendText(@logPath + DateTime.Today.ToString("yyyy-MM-dd") + ".txt");
                    if (message != null)
                        output.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + message);
                    output.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// 每个特服号一个发送日至
        /// </summary>
        /// <param name="message"></param>
        /// <param name="serviceCode"></param>
        public static void Log(string message, string serviceCode)
        {
            lock (SYNC_OBJ)
            {
                try
                {
                    // make sure we have an event log
                    if (!logPath.EndsWith(@"\"))
                        logPath += @"\";

                    DirectoryInfo di = new DirectoryInfo(logPath);
                    if (!di.Exists)
                        di.Create();

                    TextWriter output = File.AppendText(@logPath + DateTime.Today.ToString("yyyy-MM-dd") + "_" + serviceCode + ".txt");
                    if (message != null)
                        output.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + message);
                    output.Close();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// 记录错误日至
        /// </summary>
        /// <param name="message">错误信息</param>
        public static void LogErr(string message)
        {
            lock (SYNC_OBJ)
            {
                try
                {
                    // make sure we have an event log
                    if (!logPath.EndsWith(@"\"))
                        logPath += @"\";

                    DirectoryInfo di = new DirectoryInfo(logPath);
                    if (!di.Exists)
                        di.Create();

                    TextWriter output = File.AppendText(@logPath + DateTime.Today.ToString("yyyy-MM-dd") + "_err.txt");
                    if (message != null)
                        output.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + message);

                    output.Close();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
