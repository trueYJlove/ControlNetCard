using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using System.Data.SqlClient;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now.ToString() + " 网卡控制程序启动..." + "\r");
           //加一行注释 ，测试git
            // 获取上行MO线程
            Thread thread_getrecive = new Thread(new ThreadStart(thread_getMo));
            thread_getrecive.IsBackground = false;
            thread_getrecive.Start();

            Console.WindowWidth = 60;
            Console.WindowHeight = 20;
            //int a = Console.WindowHeight;
            //int b=Console.WindowWidth;
            ////定时打印信息到界面，表示程序运行正常
            //Thread Print = new Thread(new ThreadStart(thread_Print));
            //Print.IsBackground = false;
            //Print.Start();
        }

        /// <summary>
        ///  获取上行MO线程
        /// </summary>
        public static void thread_getMo()
        {
            while (true)
            {
                SmsSender.getRecieve();
                Thread.Sleep(5000);
            }
        }

        /// <summary>
        ///  定时打印信息到界面
        /// </summary>
        public static void thread_Print()
        {
            while (true)
            {
                Console.WriteLine(DateTime.Now.ToString() + " 程序正常运行中，每隔10s打印一次此信息" + "\r");
                Thread.Sleep(10000);
            }
        }


    }
}
