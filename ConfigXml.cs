using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
namespace Engine
{
    public class ConfigXml
    {
        /// <summary>
        /// 构造
        /// </summary>
        public ConfigXml()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 参数文件
        /// </summary>
        /// <returns></returns>
        public static string GetConfigXmlPath()
        {
            //return Application.StartupPath + @"\service.config";
            return "service.config";
        }
        /// <summary>
        /// 读NODE
        /// </summary>
        /// <param name="TagName"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string GetXmlNode(string TagName, string result)
        {
            string sReturn = result;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(GetConfigXmlPath());
                XmlNodeList xmlNodeList = xmlDoc.GetElementsByTagName(TagName);
                sReturn = xmlNodeList[0].InnerText.Trim();
            }
            catch (Exception e)
            {
                LogUtility.LogErr("getxmlnode:" + TagName + "," + e.Message);
            }
            return sReturn;
        }
        /// <summary>
        /// 写NODE
        /// </summary>
        /// <param name="TagName"></param>
        /// <param name="sValue"></param>
        public static void WriteXmlNode(string TagName, string sValue)
        {
            try
            {
                StreamReader sr = File.OpenText(GetConfigXmlPath());
                string sXmlContent = sr.ReadToEnd();
                sr.Close();
                Regex regex = new Regex("<" + TagName + ">.+" + "</" + TagName + ">");
                sXmlContent = regex.Replace(sXmlContent, "<" + TagName + ">" + sValue + "</" + TagName + ">");
                StreamWriter sw = new StreamWriter(GetConfigXmlPath());
                sw.Write(sXmlContent);
                sw.Close();
            }
            catch (Exception e)
            {
                LogUtility.LogErr("写xml文件时出错：" + e.Message);
            }
        }

    }
}
