using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using DongFang;

namespace Engine
{
    public class MO//上行内容
    {
        public string Id;//唯一id
        public string Mobile;//上行手机号
        public string Content;//短信内容
    }
    class SmsSender
    {
        public static cn.entinfo.sdk2.WebService md = new cn.entinfo.sdk2.WebService();
        private static string cardName = ConfigXml.GetXmlNode("cardName", "");
        private static string sn = ConfigXml.GetXmlNode("sn", "");
        private static string password = ConfigXml.GetXmlNode("password", "");
        private static string moblie = ConfigXml.GetXmlNode("moblie", "");
        private static string serverId = ConfigXml.GetXmlNode("serverId", "");//服务器编号
        private static string today = DateTime.Today.ToString("yyyy-MM-dd");
        private static IList<MO> ReadDb()//从数据库得到上行短信信息
        {
            IList<MO> moList = new List<MO>();
            string str = "select SMS_MOID,SMS_MOBILE,SMS_CONTENT from SMS_MO where SMS_MODATE = to_date('" + today + "','yyyy-mm-dd')";
            using (Oracle.DataAccess.Client.OracleDataReader dr = DataBase.getSysSqlSdr(str))
            {
                while (dr.Read())
                {
                    MO result = new MO();
                    result.Id = dr[0].ToString();
                    result.Mobile = dr[1].ToString();
                    result.Content = dr[2].ToString();
                    moList.Add(result);
                }
                return moList;
            }
        }

        /// <summary>
        /// 获取上行
        /// </summary>
        public static void getRecieve()
        {
            try
            {
                var pwd = PubStatic.MD5(sn + password);
                IList<MO> moList = ReadDb();
                if (moList.Count > 0)
                {
                    foreach (MO List in moList)
                    {
                        string id = List.Id;
                        string smobile = List.Mobile;
                        string scontent = List.Content;

                        Console.WriteLine(DateTime.Now.ToString() + " 获得一条上行：" + smobile + " 内容为：" + scontent + "\r");

                        if (smobile != moblie)
                        {
                            continue;//如果发送指令短信的手机号跟配置文件的不一致，则忽略。只认配置文件的手机号发来的指令
                        }
                        LogUtility.Log("读取到操作指令：" + List.Content, "mo");

                        bool enable = true;//是否活动，true为启用，false为禁用，默认为true
                        string temp = "";
                        if (scontent.Contains("close." + serverId))
                        {
                            temp = "禁用网卡";
                            enable = false;
                        }
                        else if (scontent.Contains("open." + serverId))
                        {
                            temp = "启用网卡";
                            enable = true;
                        }
                        else
                        {
                            continue;//除了这两个指令，其余内容忽略
                        }
                        Console.WriteLine(DateTime.Now.ToString() + " 收到" + temp + "的命令！" + "\r");

                        Console.WriteLine(DateTime.Now.ToString() + " 准备执行" + temp + "\r");
                        bool bret = false;
                        string content = "";
                        bret = ChangeNetworkConnectionStatus(enable, cardName);//操作网卡
                        if (bret)
                        {
                            content = " " + temp + " 成功";
                        }
                        else
                        {
                            content = " " + temp + " 失败";
                        }
                        Console.WriteLine(DateTime.Now.ToString() + content + "\r");//

                        string status = "";
                        //发送短信
                        string result = string.Empty;
                        result = md.mt(sn, pwd, moblie, content + "【贵州电网】", "", "", "");
                        if (!result.StartsWith("-"))
                        {
                            status = "成功";
                        }
                        else
                        {
                            LogUtility.LogErr("发送反馈短信错误：" + result);
                            status = "失败";
                        }
                        Console.WriteLine("===========反馈短信发送==========" + status + result + "\r");
                        result = null;

                        string strsql = "delete from SMS_MO where  SMS_MOID =" + id + "";
                        bool ret = DataBase.exeSysSqlStr(strsql);
                        if (ret)
                        {
                            Console.WriteLine(DateTime.Now.ToString() + ":  " + strsql + "删除指指令短信成功！");
                        }
                        else
                        {
                            Console.WriteLine(DateTime.Now.ToString() + ":  " + strsql + "删除指令短信失败！");
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(DateTime.Now.ToString() + " 异常：" + e.Message + "\r");
                LogUtility.LogErr("mo上行：" + e.Message);
            }
        }

        ///// <summary>
        ///// 获取上行
        ///// </summary>
        //public static void getRecieve()
        //{
        //    try
        //    {
        //        var pwd = PubStatic.MD5(sn + password);
        //        string moBody = md.mo(sn, pwd);
        //        if (!moBody.StartsWith("-") && moBody != "1")
        //        {
        //            foreach (string mb in moBody.Split('\n'))
        //            {
        //                LogUtility.Log(moBody, "mo");
        //                string[] mblist = mb.Split(',');
        //                string moid = mblist[0];//moid
        //                string subcode = mblist[1];//特服号
        //                string smobile = mblist[2];//手机
        //                string scontent = mblist[3];//内容
        //                scontent = HttpUtility.UrlDecode(scontent, Encoding.GetEncoding("gb2312"));
        //                scontent = scontent.Trim();
        //                scontent = scontent.Replace("\0", "");
        //                scontent = scontent.Replace("\r", "");
        //                scontent = scontent.Replace("\n", "");
        //                string srecdate = mblist[4];//时间
        //                Console.WriteLine(DateTime.Now.ToString() + " 获得一条上行：" + smobile + " 内容为：" + scontent + "\r");

        //                if (smobile != moblie)
        //                {
        //                    continue;//如果发送指令短信的手机号跟配置文件的不一致，则忽略。只认配置文件的手机号发来的指令
        //                }
        //                bool enable = true;//是否活动，true为启用，false为禁用，默认为true
        //                string temp = "";
        //                if (scontent.Contains("禁用网卡"))
        //                {
        //                    temp = "禁用网卡";
        //                    enable = false;
        //                }
        //                else if (scontent.Contains("启用网卡"))
        //                {
        //                    temp = "启用网卡";
        //                    enable = true;
        //                }
        //                else
        //                {
        //                    continue;//除了这两个指令，其余内容忽略
        //                }
        //                Console.WriteLine(DateTime.Now.ToString() + " 收到" + temp + "的命令！" + "\r");

        //                Console.WriteLine(DateTime.Now.ToString() + " 准备执行" + temp + "\r");
        //                bool bret = false;
        //                string content = "";
        //                bret = ChangeNetworkConnectionStatus(enable, cardName);//操作网卡
        //                if (bret)
        //                {
        //                    content = " " + temp + " 成功";
        //                }
        //                else
        //                {
        //                    content = " " + temp + " 失败";
        //                }
        //                Console.WriteLine(DateTime.Now.ToString() + content + "\r");//

        //                string status = "";
        //                //发送短信
        //                string result = string.Empty;
        //                result = md.mt(sn, pwd, moblie, content + "【贵州电网】", "", "", "");
        //                if (!result.StartsWith("-"))
        //                {
        //                    status = "成功";
        //                }
        //                else
        //                {
        //                    LogUtility.LogErr("发送反馈短信错误：" + result);
        //                    status = "失败";
        //                }
        //                Console.WriteLine("===========反馈短信发送==========" + status + result + "\r");
        //                result = null;
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(DateTime.Now.ToString() + " 异常：" + e.Message + "\r");
        //        LogUtility.LogErr("mo上行：" + e.Message);
        //    }
        //}

        /*
         * 网卡的禁用/启用方法
         * 要在项目中使用此方法，必须添加对System.Diagnostics的引用。
         * enable： 是bool值，表示网卡的启用或禁用，false 禁用
         * networkConnectionName：网卡显示的名称，一般是：本地连接，本地连接2 这样的。
         */
        public static bool ChangeNetworkConnectionStatus(bool enable, string networkConnectionName)
        {
            using (Process process = new Process())
            {
                string netshCmd = "interface set interface name=\"{0}\" admin={1}";
                process.EnableRaisingEvents = false;
                process.StartInfo.Arguments = String.Format(netshCmd, networkConnectionName, enable ? "ENABLED" : "DISABLED");
                process.StartInfo.FileName = "netsh.exe";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.ErrorDialog = false;
                process.StartInfo.RedirectStandardError = false;
                process.StartInfo.RedirectStandardInput = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                string rtn = process.StandardOutput.ReadToEnd();
                if (rtn.Trim().Length == 0)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine(DateTime.Now.ToString() + " 操作网卡失败：" + rtn + "\r");
                    return false;
                }
            }
        }
    }
}
