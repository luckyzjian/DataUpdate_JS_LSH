using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xcOBDServices;
using System.Data;
using System.Xml;

namespace JbtNetLibrary
{
    public class OBD_lr : OBD_parent
    {
        
        private bool isInitSuucess = false;
        /// <summary>
        /// 用于UDP发送的网络服务类
        /// </summary>
        //private UdpClient udpcSend;
        private xcOBDServices.xcPDAService obdserver;
        Thread thread = null;

        string url = "";
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public OBD_lr(string ip, string port)
        {
            url = ip;
        }

        public OBD_lr()
        {
        }
        public OBD_lr(string ip, string port,bool iswj)
        {
            url = ip;
            //initServer();
        }
        private Thread th_wait = null;
        private bool isWaiting = true;
        private bool isReceivedResult = false;//是否成功接收到接口文件
        private string recievefailmsg = "";


        
        #region 实现OBD接口
        static string ConvertXmlToString(XmlDocument xmlDoc)
        {
            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, null);
            writer.Formatting =System.Xml.Formatting.Indented;
            xmlDoc.Save(writer);
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            stream.Position = 0;
            string xmlString = sr.ReadToEnd();
            sr.Close();
            stream.Close();
            DataUpdate.FileOpreate.SaveLog(xmlString, "[XCWEB发送]", 3);
            return xmlString;
        }
        private bool getUsers(out string msg)
        {
            string code = "";
            msg = "";
            try
            {
                string ack = obdserver.Query("XC01", "");
                DataSet ds = new DataSet();
                StringReader stream = new StringReader(ack);
                XmlTextReader reader = new XmlTextReader(stream);
                ds.ReadXml(reader);
                DataTable dt1 = null;
                dt1 = ds.Tables["Response"];
                code = dt1.Rows[0]["Code"].ToString();
                msg = dt1.Rows[0]["Message"].ToString();
                if (code == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception er)
            {
                msg = er.Message;
                return false;
            }
        }
        string[] syntimearray = { "1", "2", "3", "4", "5", "6" };
        public bool startWjService(out string msg)
        {
            //JbtNetLibrary.LogWrite.WriteInfo("接口[开启服务]");
            obdserver = new xcPDAService(url);
            if (getUsers(out msg))
            {

                //if (myLED_OBD_connected != null) myLED_OBD_connected.Invoke(new Action(() => { if (myLED_OBD_connected != null) myLED_OBD_connected.IsLightLed = true; }));
                isInitSuucess = true;
                return true;
            }
            else
            {
                isInitSuucess = false;
                return false;
            }
        }
        public override bool startService(out string msg)
        {
            obdserver = new xcPDAService(url);
            if(getUsers(out msg))
            {
                
                isInitSuucess = true;
                return true;
            }
            else
            {
                isInitSuucess = false;
                return false;
            }
        }
        public override bool stopService(out string msg)
        {
            msg = "";
            try
            {
                return true;
            }
            catch (Exception er)
            {
                msg = er.Message;
                return false;
            }
        }
        string jylsh;
        string jccs;
        #endregion


        public override bool getOutlookPic(string jylsh,string jccs,string photocode,out string code,out string msg,out outlookPic pic )
        {

            pic = new outlookPic();

            code = "1";
            msg = "成功";
            try
            {
                string xml = "";
                XmlDocument xmldoc, xlmrecivedoc;
                XmlNode xmlnode;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("Query");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("Query");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("JYLSH");//创建一个<Node>节点        
                XmlElement xe2 = xmldoc.CreateElement("JCCS");//创建一个<Node>节点   
                XmlElement xe3 = xmldoc.CreateElement("User");//创建一个<Node>节点  
                XmlElement xe4 = xmldoc.CreateElement("PhotoCode");//创建一个<Node>节点           
                xe1.InnerText = jylsh;
                xe2.InnerText = jccs;
                xe3.InnerText = "";
                xe4.InnerText = photocode;
                root.AppendChild(xe1);
                root.AppendChild(xe2);
                root.AppendChild(xe3);
                root.AppendChild(xe4);
                string ack = obdserver.Query("XC22", ConvertXmlToString(xmldoc));
                DataUpdate.FileOpreate.SaveLog(ack,"[XCWEB响应]",3);
                DataSet ds = new DataSet();
                StringReader stream = new StringReader(ack);
                XmlTextReader reader = new XmlTextReader(stream);
                ds.ReadXml(reader);
                DataTable dt1 = null;
                dt1 = ds.Tables["Response"];
                code = dt1.Rows[0]["Code"].ToString();
                msg = dt1.Rows[0]["Message"].ToString();
                if (code == "1")
                {
                    dt1 = ds.Tables["Row"];
                    pic.jylsh= dt1.Rows[0]["JYLSH"].ToString();
                    pic.jccs = dt1.Rows[0]["JCCS"].ToString();
                    pic.photocode = dt1.Rows[0]["PhotoCode"].ToString();
                    pic.pssj = DateTime.Parse(dt1.Rows[0]["Pssj"].ToString());
                    pic.photo = dt1.Rows[0]["Photo"].ToString();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception er)
            {
                DataUpdate.FileOpreate.SaveLog(er.Message, "[xcWEB读取外检照片异常]", 3);
                //JbtNetLibrary.LogWrite.WriteInfo("读取外检照片异常:" + er.Message);
                code = "-1";
                msg = "读取外检照片异常:" + er.Message;
                return false;
            }
        }
    }
}
