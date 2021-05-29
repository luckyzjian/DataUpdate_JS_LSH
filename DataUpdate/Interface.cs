using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Xml;

namespace DataUpdate
{
    public class Interface
    {
        private VeptsOutAccess outlineservice = null;
        private string Organ = "";//调用者编号
        private string Jkxlh = "";//接口序列号
        public Dictionary<string, string> get_hpzl_code = new Dictionary<string, string>();

        /// <summary>
        /// 接口初始化
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="jkxlh">接口序列号</param>
        /// <param name="jczID">检测站编号</param>
        public Interface(string url, string jkxlh, string jczID)
        {
            #region 初始化号牌种类名称与编号
            get_hpzl_code.Add("大型汽车", "01");
            get_hpzl_code.Add("小型汽车", "02");
            get_hpzl_code.Add("使馆汽车", "03");
            get_hpzl_code.Add("领馆汽车", "04");
            get_hpzl_code.Add("境外汽车", "05");
            get_hpzl_code.Add("外籍汽车", "06");
            get_hpzl_code.Add("两、三轮摩托车", "07");
            get_hpzl_code.Add("轻便摩托车", "08");
            get_hpzl_code.Add("使馆摩托车", "09");
            get_hpzl_code.Add("领馆摩托车", "10");
            get_hpzl_code.Add("境外摩托车", "11");
            get_hpzl_code.Add("外籍摩托车", "12");
            get_hpzl_code.Add("农用运输车", "13");
            get_hpzl_code.Add("拖拉机", "14");
            get_hpzl_code.Add("挂车", "15");
            get_hpzl_code.Add("教练汽车", "16");
            get_hpzl_code.Add("教练摩托车", "17");
            get_hpzl_code.Add("试验汽车", "18");
            get_hpzl_code.Add("试验摩托车", "19");
            get_hpzl_code.Add("临时人境汽车", "20");
            get_hpzl_code.Add("临时人境摩托车", "21");
            get_hpzl_code.Add("临时行驶车", "22");
            get_hpzl_code.Add("警用汽车", "23");
            get_hpzl_code.Add("警用摩托", "24");
            get_hpzl_code.Add("其他", "99");
            #endregion

            Organ = jczID;
            Jkxlh = jkxlh;
            outlineservice = new VeptsOutAccess(url);
        }

        /// <summary>
        /// 时钟同步
        /// </summary>
        /// <param name="now_time">获取到的服务器时间</param>
        /// <returns></returns>
        public bool GetSystemDatetime(out DateTime now_time)
        {
            now_time = DateTime.Parse("1111-11-11 11:11:11");
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点
                
                XmlElement xe01 = xmldoc.CreateElement("organ");
                xe01.InnerText = Organ;
                XmlElement xe02 = xmldoc.CreateElement("jkxlh");
                xe02.InnerText = Jkxlh;
                XmlElement xe03 = xmldoc.CreateElement("jkid");
                xe03.InnerText = "SJ001";

                xe1.AppendChild(xe01);
                xe1.AppendChild(xe02);
                xe1.AppendChild(xe03);

                xe2.InnerText = "";

                root.AppendChild(xe1);
                root.AppendChild(xe2);

                string receiveXml = HttpUtility.UrlDecode(outlineservice.query(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);
                string error_info = "";

                //解析查询结果并得出时间返回
                DataTable dt = ReadSeachDatatable(receiveXml, out error_info);
                if (dt != null)
                {
                    try
                    {
                        string time_temp = dt.Rows[0]["syndate"].ToString();
                        string system_time_now = "";
                        if (time_temp.Length > 0)
                        {
                            if (time_temp.Length > 19 && time_temp.LastIndexOf(":") == 19)//带毫秒且毫秒前为冒号时将“:”替换为“.”
                                system_time_now = time_temp.Substring(0, 19) + "." + time_temp.Substring(time_temp.Length - 3, 3);
                            if (DateTime.TryParse(system_time_now, out now_time))
                                return true;
                            else
                                return false;
                        }
                        else
                            return false;
                    }
                    catch (Exception er)
                    {
                        FileOpreate.SaveLog("时间数据格式失败，原因：" + er.Message, "时间同步失败", 3);
                        return false;
                    }
                }
                else
                    return false;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行时间同步失败，原因：" + er.Message, "时间同步", 3);
                return false;
            }
        }

        /// <summary>
        /// 获取待检列表
        /// </summary>
        /// <param name="error_info">错误信息</param>
        /// <returns></returns>
        public DataTable GetVehicleList(out string error_info)
        {
            error_info = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe11 = xmldoc.CreateElement("organ");
                xe11.InnerText = Organ;
                XmlElement xe12 = xmldoc.CreateElement("jkxlh");
                xe12.InnerText = Jkxlh;
                XmlElement xe13 = xmldoc.CreateElement("jkid");
                xe13.InnerText = "DJ001";

                xe1.AppendChild(xe11);
                xe1.AppendChild(xe12);
                xe1.AppendChild(xe13);

                root.AppendChild(xe1);
                root.AppendChild(xe2);

                string receiveXml = HttpUtility.UrlDecode(outlineservice.query(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                return ReadSeachDatatable(receiveXml, out error_info);
            }
            catch (Exception er)
            {
                error_info = er.Message;
                FileOpreate.SaveLog("执行获取待检列表失败，原因：" + er.Message, "待检列表", 3);
                return null;
            }
        }

        #region 山东联网
        //车辆查询
        public DataTable GetCarInfoSD(string HPHM, string HPZL, out string error_info)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节

            XmlElement xe11 = xmldoc.CreateElement("organ");
            xe11.InnerText = Organ;
            XmlElement xe12 = xmldoc.CreateElement("jkxlh");
            xe12.InnerText = Jkxlh;
            XmlElement xe13 = xmldoc.CreateElement("jkid");
            xe13.InnerText = "DL003";
            xe1.AppendChild(xe11);
            xe1.AppendChild(xe12);
            xe1.AppendChild(xe13);

            XmlElement xe21 = xmldoc.CreateElement("hphm");
            xe21.InnerText = DataChange.encodeUTF8(HPHM);
            XmlElement xe22 = xmldoc.CreateElement("hpzl");
            xe22.InnerText = HPZL;
            xe2.AppendChild(xe21);
            xe2.AppendChild(xe22);

            root.AppendChild(xe1);
            root.AppendChild(xe2);

            FileOpreate.SaveLog("DL003", "SEND", 3);
            string receiveXml = HttpUtility.UrlDecode(outlineservice.query(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            return ReadSeachDatatable(receiveXml, out error_info);
        }
        public DataTable GetCarInfoByLshSD(string JYLSH, string JYCS, out string error_info)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节

            XmlElement xe11 = xmldoc.CreateElement("organ");
            xe11.InnerText = Organ;
            XmlElement xe12 = xmldoc.CreateElement("jkxlh");
            xe12.InnerText = Jkxlh;
            XmlElement xe13 = xmldoc.CreateElement("jkid");
            xe13.InnerText = "DL002";
            xe1.AppendChild(xe11);
            xe1.AppendChild(xe12);
            xe1.AppendChild(xe13);

            XmlElement xe21 = xmldoc.CreateElement("jylsh");
            xe21.InnerText = JYLSH;
            XmlElement xe22 = xmldoc.CreateElement("jycs");
            xe22.InnerText = JYCS;
            xe2.AppendChild(xe21);
            xe2.AppendChild(xe22);

            root.AppendChild(xe1);
            root.AppendChild(xe2);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.query(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            return ReadSeachDatatable(receiveXml, out error_info);
        }

        //检测过程开始
        public void writeProjectStartSD(ProjectStart model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点      
            XmlElement xe2 = xmldoc.CreateElement("body");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "KS001";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe21 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe2101 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
            xe2101.InnerText = model.jylsh;
            XmlElement xe2102 = xmldoc.CreateElement("jycs");//创建一个<Node>节点 
            xe2102.InnerText = model.jycs;
            XmlElement xe2103 = xmldoc.CreateElement("jczbh");//创建一个<Node>节点 
            xe2103.InnerText = model.jczbh;
            XmlElement xe2104 = xmldoc.CreateElement("jcgwh");//创建一个<Node>节点 
            xe2104.InnerText = model.jcgwh;
            XmlElement xe2105 = xmldoc.CreateElement("hphm");//创建一个<Node>节点 
            xe2105.InnerText = DataChange.encodeUTF8(model.hphm);
            XmlElement xe2106 = xmldoc.CreateElement("hpzl");//创建一个<Node>节点 
            xe2106.InnerText = model.hpzl;
            XmlElement xe2107 = xmldoc.CreateElement("ycy");//创建一个<Node>节点 
            xe2107.InnerText = DataChange.encodeUTF8(model.ycy);
            XmlElement xe2108 = xmldoc.CreateElement("jcczy");//创建一个<Node>节点 
            xe2108.InnerText = DataChange.encodeUTF8(model.jcczy);
            XmlElement xe2109 = xmldoc.CreateElement("jcff");//创建一个<Node>节点 
            xe2109.InnerText = model.jcff;
            XmlElement xe2110 = xmldoc.CreateElement("ljxslc");//创建一个<Node>节点 
            xe2110.InnerText = model.ljxslc;
            XmlElement xe2111 = xmldoc.CreateElement("dqsj");//创建一个<Node>节点 
            xe2111.InnerText = model.dqsj;
            XmlElement xe2112 = xmldoc.CreateElement("jcbbh");//创建一个<Node>节点 
            xe2112.InnerText = model.jcbbh;
            xe21.AppendChild(xe2101);
            xe21.AppendChild(xe2102);
            xe21.AppendChild(xe2103);
            xe21.AppendChild(xe2104);
            xe21.AppendChild(xe2105);
            xe21.AppendChild(xe2106);
            xe21.AppendChild(xe2107);
            xe21.AppendChild(xe2108);
            xe21.AppendChild(xe2109);
            xe21.AppendChild(xe2110);
            xe21.AppendChild(xe2111);
            xe21.AppendChild(xe2112);
            xe2.AppendChild(xe21);

            root.AppendChild(xe1);
            root.AppendChild(xe2);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }

        //检测过程结束
        public void writeProjectStopSD(ProjectStop model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("body");//创建一个<Node>节点  

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "JS001";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe21 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe2101 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
            xe2101.InnerText = model.jylsh;
            XmlElement xe2102 = xmldoc.CreateElement("jycs");//创建一个<Node>节点 
            xe2102.InnerText = model.jycs;
            XmlElement xe2103 = xmldoc.CreateElement("jczbh");//创建一个<Node>节点 
            xe2103.InnerText = model.jczbh;
            XmlElement xe2104 = xmldoc.CreateElement("jcgwh");//创建一个<Node>节点 
            xe2104.InnerText = model.jcgwh;
            XmlElement xe2105 = xmldoc.CreateElement("hphm");//创建一个<Node>节点 
            xe2105.InnerText = DataChange.encodeUTF8(model.hphm);
            XmlElement xe2106 = xmldoc.CreateElement("hpzl");//创建一个<Node>节点 
            xe2106.InnerText = model.hpzl;
            XmlElement xe2107 = xmldoc.CreateElement("pdjg");//创建一个<Node>节点 
            xe2107.InnerText = model.pdjg;
            xe21.AppendChild(xe2101);
            xe21.AppendChild(xe2102);
            xe21.AppendChild(xe2103);
            xe21.AppendChild(xe2104);
            xe21.AppendChild(xe2105);
            xe21.AppendChild(xe2106);
            xe21.AppendChild(xe2107);
            xe2.AppendChild(xe21);

            root.AppendChild(xe1);
            root.AppendChild(xe2);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }

        //车辆照片
        public void writeCapturePictureSD(CapturePicture model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("body");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "ZP002";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe21 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe2101 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
            xe2101.InnerText = model.jylsh;
            XmlElement xe2102 = xmldoc.CreateElement("jycs");//创建一个<Node>节点 
            xe2102.InnerText = model.jycs;
            XmlElement xe2103 = xmldoc.CreateElement("jczbh");//创建一个<Node>节点 
            xe2103.InnerText = model.jczbh;
            XmlElement xe2104 = xmldoc.CreateElement("jcgwh");//创建一个<Node>节点 
            xe2104.InnerText = model.jcgwh;
            XmlElement xe2105 = xmldoc.CreateElement("zpbh");//创建一个<Node>节点 
            xe2105.InnerText = model.zpbh;
            XmlElement xe2106 = xmldoc.CreateElement("photo_date");//创建一个<Node>节点 
            xe2106.InnerText = model.photo_date;
            xe21.AppendChild(xe2101);
            xe21.AppendChild(xe2102);
            xe21.AppendChild(xe2103);
            xe21.AppendChild(xe2104);
            xe21.AppendChild(xe2105);
            xe21.AppendChild(xe2106);

            xe2.AppendChild(xe21);

            root.AppendChild(xe1);
            root.AppendChild(xe2);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }

        //瞬态结果数据
        public void writeVmasResultSD(VmasResult model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "JC001";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
            xe201.InnerText = model.jylsh;
            XmlElement xe202 = xmldoc.CreateElement("jycs");//创建一个<Node>节点 
            xe202.InnerText = model.jycs;
            XmlElement xe203 = xmldoc.CreateElement("sbrzbm");//创建一个<Node>节点 
            xe203.InnerText = model.sbrzbm;
            XmlElement xe204 = xmldoc.CreateElement("sbmc");//创建一个<Node>节点 
            xe204.InnerText = DataChange.encodeUTF8(model.sbmc);
            XmlElement xe205 = xmldoc.CreateElement("sbxh");//创建一个<Node>节点 
            xe205.InnerText = DataChange.encodeUTF8(model.sbxh);
            XmlElement xe206 = xmldoc.CreateElement("sbzzc");//创建一个<Node>节点 
            xe206.InnerText = DataChange.encodeUTF8(model.sbzzc);
            XmlElement xe207 = xmldoc.CreateElement("dpcgj");//创建一个<Node>节点 
            xe207.InnerText = model.dpcgj;
            XmlElement xe208 = xmldoc.CreateElement("pqfxy");//创建一个<Node>节点 
            xe208.InnerText = model.pqfxy;
            XmlElement xe209 = xmldoc.CreateElement("wd");//创建一个<Node>节点 
            xe209.InnerText = model.wd;
            XmlElement xe210 = xmldoc.CreateElement("dqy");//创建一个<Node>节点 
            xe210.InnerText = model.dqy;
            XmlElement xe211 = xmldoc.CreateElement("xdsd");//创建一个<Node>节点 
            xe211.InnerText = model.xdsd;
            XmlElement xe212 = xmldoc.CreateElement("coxz");//创建一个<Node>节点 
            xe212.InnerText = model.coxz;
            XmlElement xe213 = xmldoc.CreateElement("coclz");//创建一个<Node>节点 
            xe213.InnerText = model.coclz;
            XmlElement xe214 = xmldoc.CreateElement("copdjg");//创建一个<Node>节点 
            xe214.InnerText = model.copdjg == "合格" ? "1" : "0";
            XmlElement xe215 = xmldoc.CreateElement("hcxz");//创建一个<Node>节点 
            xe215.InnerText = model.hcxz;
            XmlElement xe216 = xmldoc.CreateElement("hcclz");//创建一个<Node>节点 
            xe216.InnerText = model.hcclz;
            XmlElement xe217 = xmldoc.CreateElement("hcpdjg");//创建一个<Node>节点 
            xe217.InnerText = model.hcpdjg == "合格" ? "1" : "0";
            XmlElement xe218 = xmldoc.CreateElement("noxz");//创建一个<Node>节点 
            xe218.InnerText = model.noxz;
            XmlElement xe219 = xmldoc.CreateElement("noclz");//创建一个<Node>节点 
            xe219.InnerText = model.noclz;
            XmlElement xe220 = xmldoc.CreateElement("nopdjg");//创建一个<Node>节点 
            xe220.InnerText = model.nopdjg == "合格" ? "1" : "0";
            XmlElement xe221 = xmldoc.CreateElement("hcnoxz");//创建一个<Node>节点 
            xe221.InnerText = model.hcnoxz;
            XmlElement xe222 = xmldoc.CreateElement("hcnoclz");//创建一个<Node>节点 
            xe222.InnerText = model.hcnoclz;
            XmlElement xe223 = xmldoc.CreateElement("hcnopdjg");//创建一个<Node>节点 
            xe223.InnerText = model.hcnopdjg == "合格" ? "1" : "0";
            XmlElement xe224 = xmldoc.CreateElement("csljccsj");//创建一个<Node>节点 
            xe224.InnerText = model.csljccsj;
            XmlElement xe225 = xmldoc.CreateElement("xslc");//创建一个<Node>节点 
            xe225.InnerText = model.xslc;
            XmlElement xe226 = xmldoc.CreateElement("PDJG");//创建一个<Node>节点 
            xe226.InnerText = model.PDJG == "合格" ? "1" : "0";
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe2.AppendChild(xe208);
            xe2.AppendChild(xe209);
            xe2.AppendChild(xe210);
            xe2.AppendChild(xe211);
            xe2.AppendChild(xe212);
            xe2.AppendChild(xe213);
            xe2.AppendChild(xe214);
            xe2.AppendChild(xe215);
            xe2.AppendChild(xe216);
            xe2.AppendChild(xe217);
            xe2.AppendChild(xe218);
            xe2.AppendChild(xe219);
            xe2.AppendChild(xe220);
            xe2.AppendChild(xe221);
            xe2.AppendChild(xe222);
            xe2.AppendChild(xe223);
            xe2.AppendChild(xe224);
            xe2.AppendChild(xe225);
            xe2.AppendChild(xe226);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }

        //稳态结果数据
        public void writeASMResultSD(ASMResult model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "JC002";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
            xe201.InnerText = model.jylsh;
            XmlElement xe202 = xmldoc.CreateElement("jycs");//创建一个<Node>节点 
            xe202.InnerText = model.jycs;
            XmlElement xe203 = xmldoc.CreateElement("sbrzbm");//创建一个<Node>节点 
            xe203.InnerText = model.sbrzbm;
            XmlElement xe204 = xmldoc.CreateElement("sbmc");//创建一个<Node>节点 
            xe204.InnerText = DataChange.encodeUTF8(model.sbmc);
            XmlElement xe205 = xmldoc.CreateElement("sbxh");//创建一个<Node>节点 
            xe205.InnerText = DataChange.encodeUTF8(model.sbxh);;
            XmlElement xe206 = xmldoc.CreateElement("sbzzc");//创建一个<Node>节点 
            xe206.InnerText = DataChange.encodeUTF8(model.sbzzc);
            XmlElement xe207 = xmldoc.CreateElement("dpcgj");//创建一个<Node>节点 
            xe207.InnerText = model.dpcgj;
            XmlElement xe208 = xmldoc.CreateElement("pqfxy");//创建一个<Node>节点 
            xe208.InnerText = model.pqfxy;
            XmlElement xe209 = xmldoc.CreateElement("wd");//创建一个<Node>节点 
            xe209.InnerText = model.wd;
            XmlElement xe210 = xmldoc.CreateElement("dqy");//创建一个<Node>节点 
            xe210.InnerText = model.dqy;
            XmlElement xe211 = xmldoc.CreateElement("xdsd");//创建一个<Node>节点 
            xe211.InnerText = model.xdsd;
            XmlElement xe212 = xmldoc.CreateElement("hc5025xz");//创建一个<Node>节点 
            xe212.InnerText = model.hc5025xz;
            XmlElement xe213 = xmldoc.CreateElement("hc5025clz");//创建一个<Node>节点 
            xe213.InnerText = model.hc5025clz;
            XmlElement xe214 = xmldoc.CreateElement("hc5025pdjg");//创建一个<Node>节点 
            xe214.InnerText = model.hc5025pdjg == "合格" ? "1" : "0";
            XmlElement xe215 = xmldoc.CreateElement("co5025xz");//创建一个<Node>节点 
            xe215.InnerText = model.co5025xz;
            XmlElement xe216 = xmldoc.CreateElement("co5025clz");//创建一个<Node>节点 
            xe216.InnerText = model.co5025clz;
            XmlElement xe217 = xmldoc.CreateElement("co5025pdjg");//创建一个<Node>节点 
            xe217.InnerText = model.co5025pdjg == "合格" ? "1" : "0";
            XmlElement xe218 = xmldoc.CreateElement("no5025xz");//创建一个<Node>节点 
            xe218.InnerText = model.no5025xz;
            XmlElement xe219 = xmldoc.CreateElement("no5025clz");//创建一个<Node>节点 
            xe219.InnerText = model.no5025clz;
            XmlElement xe220 = xmldoc.CreateElement("no5025pdjg");//创建一个<Node>节点 
            xe220.InnerText = model.no5025pdjg == "合格" ? "1" : "0";
            XmlElement xe221 = xmldoc.CreateElement("fdjzs5025");//创建一个<Node>节点 
            xe221.InnerText = model.fdjzs5025;
            XmlElement xe222 = xmldoc.CreateElement("fdjyw5025");//创建一个<Node>节点 
            xe222.InnerText = model.fdjyw5025;
            XmlElement xe223 = xmldoc.CreateElement("hc2540xz");//创建一个<Node>节点 
            xe223.InnerText = model.hc2540xz;
            XmlElement xe224 = xmldoc.CreateElement("hc2540clz");//创建一个<Node>节点 
            xe224.InnerText = model.hc2540clz;
            XmlElement xe225 = xmldoc.CreateElement("hc2540pdjg");//创建一个<Node>节点 
            xe225.InnerText = model.hc2540pdjg == "合格" ? "1" : (model.hc2540pdjg != "" ? "0" : "");
            XmlElement xe226 = xmldoc.CreateElement("co2540xz");//创建一个<Node>节点 
            xe226.InnerText = model.co2540xz;
            XmlElement xe227 = xmldoc.CreateElement("co2540clz");//创建一个<Node>节点 
            xe227.InnerText = model.co2540clz;
            XmlElement xe228 = xmldoc.CreateElement("co2540pdjg");//创建一个<Node>节点 
            xe228.InnerText = model.co2540pdjg == "合格" ? "1" : (model.co2540pdjg != "" ? "0" : "");
            XmlElement xe229 = xmldoc.CreateElement("no2540xz");//创建一个<Node>节点 
            xe229.InnerText = model.no2540xz;
            XmlElement xe230 = xmldoc.CreateElement("no2540clz");//创建一个<Node>节点 
            xe230.InnerText = model.no2540clz;
            XmlElement xe231 = xmldoc.CreateElement("no2540pdjg");//创建一个<Node>节点 
            xe231.InnerText = model.no2540pdjg == "合格" ? "1" : (model.no2540pdjg != "" ? "0" : "");
            XmlElement xe232 = xmldoc.CreateElement("fdjzs2540");//创建一个<Node>节点 
            xe232.InnerText = model.fdjzs2540;
            XmlElement xe233 = xmldoc.CreateElement("fdjyw2540");//创建一个<Node>节点 
            xe233.InnerText = model.fdjyw2540;
            XmlElement xe234 = xmldoc.CreateElement("jzzgl5025");//创建一个<Node>节点 
            xe234.InnerText = model.jzzgl5025;
            XmlElement xe235 = xmldoc.CreateElement("jzzgl2540");//创建一个<Node>节点 
            xe235.InnerText = model.jzzgl2540;
            XmlElement xe236 = xmldoc.CreateElement("PDJG");//创建一个<Node>节点 
            xe236.InnerText = model.PDJG == "合格" ? "1" : "0";
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe2.AppendChild(xe208);
            xe2.AppendChild(xe209);
            xe2.AppendChild(xe210);
            xe2.AppendChild(xe211);
            xe2.AppendChild(xe212);
            xe2.AppendChild(xe213);
            xe2.AppendChild(xe214);
            xe2.AppendChild(xe215);
            xe2.AppendChild(xe216);
            xe2.AppendChild(xe217);
            xe2.AppendChild(xe218);
            xe2.AppendChild(xe219);
            xe2.AppendChild(xe220);
            xe2.AppendChild(xe221);
            xe2.AppendChild(xe222);
            xe2.AppendChild(xe223);
            xe2.AppendChild(xe224);
            xe2.AppendChild(xe225);
            xe2.AppendChild(xe226);
            xe2.AppendChild(xe227);
            xe2.AppendChild(xe228);
            xe2.AppendChild(xe229);
            xe2.AppendChild(xe230);
            xe2.AppendChild(xe231);
            xe2.AppendChild(xe232);
            xe2.AppendChild(xe233);
            xe2.AppendChild(xe234);
            xe2.AppendChild(xe235);
            xe2.AppendChild(xe236);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }

        //双怠速结果数据
        public void writeSDSResultSD(SDSResult model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "JC003";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
            xe201.InnerText = model.jylsh;
            XmlElement xe202 = xmldoc.CreateElement("jycs");//创建一个<Node>节点 
            xe202.InnerText = model.jycs;
            XmlElement xe203 = xmldoc.CreateElement("sbrzbm");//创建一个<Node>节点 
            xe203.InnerText = model.sbrzbm;
            XmlElement xe204 = xmldoc.CreateElement("sbmc");//创建一个<Node>节点 
            xe204.InnerText = DataChange.encodeUTF8(model.sbmc);
            XmlElement xe205 = xmldoc.CreateElement("sbxh");//创建一个<Node>节点 
            xe205.InnerText = DataChange.encodeUTF8(model.sbxh);
            XmlElement xe206 = xmldoc.CreateElement("sbzzc");//创建一个<Node>节点 
            xe206.InnerText = DataChange.encodeUTF8(model.sbzzc);
            XmlElement xe207 = xmldoc.CreateElement("pqfxy");//创建一个<Node>节点 
            xe207.InnerText = model.pqfxy;
            XmlElement xe208 = xmldoc.CreateElement("wd");//创建一个<Node>节点 
            xe208.InnerText = model.wd;
            XmlElement xe209 = xmldoc.CreateElement("dqy");//创建一个<Node>节点 
            xe209.InnerText = model.dqy;
            XmlElement xe210 = xmldoc.CreateElement("xdsd");//创建一个<Node>节点 
            xe210.InnerText = model.xdsd;
            XmlElement xe211 = xmldoc.CreateElement("glkqxsz");//创建一个<Node>节点 
            xe211.InnerText = model.glkqxsz;
            XmlElement xe212 = xmldoc.CreateElement("glkqxssx");//创建一个<Node>节点 
            xe212.InnerText = model.glkqxssx;
            XmlElement xe213 = xmldoc.CreateElement("glkqsxxx");//创建一个<Node>节点 
            xe213.InnerText = model.glkqsxxx;
            XmlElement xe214 = xmldoc.CreateElement("glkqxspdjg");//创建一个<Node>节点 
            if (model.glkqxspdjg == "合格")
                xe214.InnerText = "1";
            else if (model.glkqxspdjg == "不合格")
                xe214.InnerText = "0";
            else
                xe214.InnerText = "-";
            XmlElement xe215 = xmldoc.CreateElement("ddscoxz");//创建一个<Node>节点 
            xe215.InnerText = model.ddscoxz;
            XmlElement xe216 = xmldoc.CreateElement("ddscoz");//创建一个<Node>节点 
            xe216.InnerText = model.ddscoz;
            XmlElement xe217 = xmldoc.CreateElement("ddscopdjg");//创建一个<Node>节点 
            xe217.InnerText = model.ddscopdjg;
            XmlElement xe218 = xmldoc.CreateElement("ddshcxz");//创建一个<Node>节点 
            xe218.InnerText = model.ddshcxz;
            XmlElement xe219 = xmldoc.CreateElement("ddshcz");//创建一个<Node>节点 
            xe219.InnerText = model.ddshcz;
            XmlElement xe220 = xmldoc.CreateElement("ddshcpdjg");//创建一个<Node>节点 
            xe220.InnerText = model.ddshcpdjg;
            XmlElement xe221 = xmldoc.CreateElement("fdjdszs");//创建一个<Node>节点 
            xe221.InnerText = model.fdjdszs;
            XmlElement xe222 = xmldoc.CreateElement("ddsjywd");//创建一个<Node>节点 
            xe222.InnerText = model.ddsjywd;
            XmlElement xe223 = xmldoc.CreateElement("gdscoxz");//创建一个<Node>节点 
            xe223.InnerText = model.gdscoxz;
            XmlElement xe224 = xmldoc.CreateElement("gdscoz");//创建一个<Node>节点 
            xe224.InnerText = model.gdscoz;
            XmlElement xe225 = xmldoc.CreateElement("gdscopdjg");//创建一个<Node>节点 
            xe225.InnerText = model.gdscopdjg;
            XmlElement xe226 = xmldoc.CreateElement("gdshcz");//创建一个<Node>节点 
            xe226.InnerText = model.gdshcz;
            XmlElement xe227 = xmldoc.CreateElement("gdshcxz");//创建一个<Node>节点 
            xe227.InnerText = model.gdshcxz;
            XmlElement xe228 = xmldoc.CreateElement("gdshcpdjg");//创建一个<Node>节点 
            xe228.InnerText = model.gdshcpdjg;
            XmlElement xe229 = xmldoc.CreateElement("gdszs");//创建一个<Node>节点 
            xe229.InnerText = model.gdszs;
            XmlElement xe230 = xmldoc.CreateElement("gdsjywd");//创建一个<Node>节点 
            xe230.InnerText = model.gdsjywd;
            XmlElement xe231 = xmldoc.CreateElement("PDJG");//创建一个<Node>节点 
            xe231.InnerText = model.PDJG;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe2.AppendChild(xe208);
            xe2.AppendChild(xe209);
            xe2.AppendChild(xe210);
            xe2.AppendChild(xe211);
            xe2.AppendChild(xe212);
            xe2.AppendChild(xe213);
            xe2.AppendChild(xe214);
            xe2.AppendChild(xe215);
            xe2.AppendChild(xe216);
            xe2.AppendChild(xe217);
            xe2.AppendChild(xe218);
            xe2.AppendChild(xe219);
            xe2.AppendChild(xe220);
            xe2.AppendChild(xe221);
            xe2.AppendChild(xe222);
            xe2.AppendChild(xe223);
            xe2.AppendChild(xe224);
            xe2.AppendChild(xe225);
            xe2.AppendChild(xe226);
            xe2.AppendChild(xe227);
            xe2.AppendChild(xe228);
            xe2.AppendChild(xe229);
            xe2.AppendChild(xe230);
            xe2.AppendChild(xe231);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }

        //加载减速结果数据
        public void writeJZJSResultSD(JZJSResult model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "JC004";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
            xe201.InnerText = model.jylsh;
            XmlElement xe202 = xmldoc.CreateElement("jycs");//创建一个<Node>节点 
            xe202.InnerText = model.jycs;
            XmlElement xe203 = xmldoc.CreateElement("sbrzbm");//创建一个<Node>节点 
            xe203.InnerText = model.sbrzbm;
            XmlElement xe204 = xmldoc.CreateElement("sbmc");//创建一个<Node>节点 
            xe204.InnerText = DataChange.encodeUTF8(model.sbmc);
            XmlElement xe205 = xmldoc.CreateElement("sbxh");//创建一个<Node>节点 
            xe205.InnerText = DataChange.encodeUTF8(model.sbxh);
            XmlElement xe206 = xmldoc.CreateElement("sbzzc");//创建一个<Node>节点 
            xe206.InnerText = DataChange.encodeUTF8(model.sbzzc);
            XmlElement xe207 = xmldoc.CreateElement("dpcgj");//创建一个<Node>节点 
            xe207.InnerText = model.dpcgj;
            XmlElement xe208 = xmldoc.CreateElement("wd");//创建一个<Node>节点 
            xe208.InnerText = model.wd;
            XmlElement xe209 = xmldoc.CreateElement("dqy");//创建一个<Node>节点 
            xe209.InnerText = model.dqy;
            XmlElement xe210 = xmldoc.CreateElement("xdsd");//创建一个<Node>节点 
            xe210.InnerText = model.xdsd;
            XmlElement xe211 = xmldoc.CreateElement("velmaxhp");//创建一个<Node>节点 
            xe211.InnerText = model.velmaxhp;
            XmlElement xe212 = xmldoc.CreateElement("velmaxhpzs");//创建一个<Node>节点 
            xe212.InnerText = model.velmaxhpzs;
            XmlElement xe213 = xmldoc.CreateElement("zdlbgl");//创建一个<Node>节点 
            xe213.InnerText = model.zdlbgl;
            XmlElement xe214 = xmldoc.CreateElement("zdlbglxz");//创建一个<Node>节点 
            xe214.InnerText = model.zdlbglxz;
            XmlElement xe215 = xmldoc.CreateElement("zdlbglzs");//创建一个<Node>节点 
            xe215.InnerText = model.zdlbglzs;
            XmlElement xe216 = xmldoc.CreateElement("zdlbglpdjg");//创建一个<Node>节点 
            xe216.InnerText = model.zdlbglpdjg == "合格" ? "1" : "0";
            XmlElement xe217 = xmldoc.CreateElement("ydxz");//创建一个<Node>节点 
            xe217.InnerText = model.ydxz;
            XmlElement xe218 = xmldoc.CreateElement("ydpdjg");//创建一个<Node>节点 
            xe218.InnerText = model.ydpdjg == "合格" ? "1" : "0";
            XmlElement xe219 = xmldoc.CreateElement("k100");//创建一个<Node>节点 
            xe219.InnerText = model.k100;
            XmlElement xe220 = xmldoc.CreateElement("k90");//创建一个<Node>节点 
            xe220.InnerText = model.k90;
            XmlElement xe221 = xmldoc.CreateElement("k80");//创建一个<Node>节点 
            xe221.InnerText = model.k80;
            XmlElement xe222 = xmldoc.CreateElement("raterevup");//创建一个<Node>节点 
            xe222.InnerText = model.raterevup;
            XmlElement xe223 = xmldoc.CreateElement("raterevdown");//创建一个<Node>节点 
            xe223.InnerText = model.raterevdown;
            XmlElement xe224 = xmldoc.CreateElement("PDJG");//创建一个<Node>节点 
            xe224.InnerText = model.PDJG == "合格" ? "1" : "0";
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe2.AppendChild(xe208);
            xe2.AppendChild(xe209);
            xe2.AppendChild(xe210);
            xe2.AppendChild(xe211);
            xe2.AppendChild(xe212);
            xe2.AppendChild(xe213);
            xe2.AppendChild(xe214);
            xe2.AppendChild(xe215);
            xe2.AppendChild(xe216);
            xe2.AppendChild(xe217);
            xe2.AppendChild(xe218);
            xe2.AppendChild(xe219);
            xe2.AppendChild(xe220);
            xe2.AppendChild(xe221);
            xe2.AppendChild(xe222);
            xe2.AppendChild(xe223);
            xe2.AppendChild(xe224);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }

        //不透光结果数据
        public void writeZYJSResultSD(ZYJSResult model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "JC005";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
            xe201.InnerText = model.jylsh;
            XmlElement xe202 = xmldoc.CreateElement("jycs");//创建一个<Node>节点 
            xe202.InnerText = model.jycs;
            XmlElement xe203 = xmldoc.CreateElement("sbrzbm");//创建一个<Node>节点 
            xe203.InnerText = model.sbrzbm;
            XmlElement xe204 = xmldoc.CreateElement("sbmc");//创建一个<Node>节点 
            xe204.InnerText = DataChange.encodeUTF8(model.sbmc);
            XmlElement xe205 = xmldoc.CreateElement("sbxh");//创建一个<Node>节点 
            xe205.InnerText = DataChange.encodeUTF8(model.sbxh);
            XmlElement xe206 = xmldoc.CreateElement("sbzzc");//创建一个<Node>节点 
            xe206.InnerText = DataChange.encodeUTF8(model.sbzzc);
            XmlElement xe207 = xmldoc.CreateElement("wd");//创建一个<Node>节点 
            xe207.InnerText = model.wd;
            XmlElement xe208 = xmldoc.CreateElement("dqy");//创建一个<Node>节点 
            xe208.InnerText = model.dqy;
            XmlElement xe209 = xmldoc.CreateElement("xdsd");//创建一个<Node>节点 
            xe209.InnerText = model.xdsd;
            XmlElement xe210 = xmldoc.CreateElement("fdjdszs");//创建一个<Node>节点 
            xe210.InnerText = model.fdjdszs;
            XmlElement xe211 = xmldoc.CreateElement("smoke4");//创建一个<Node>节点 
            xe211.InnerText = model.smoke4;
            XmlElement xe212 = xmldoc.CreateElement("smoke3");//创建一个<Node>节点 
            xe212.InnerText = model.smoke3;
            XmlElement xe213 = xmldoc.CreateElement("smoke2");//创建一个<Node>节点 
            xe213.InnerText = model.smoke2;
            XmlElement xe214 = xmldoc.CreateElement("smoke1");//创建一个<Node>节点 
            xe214.InnerText = model.smoke1;
            XmlElement xe215 = xmldoc.CreateElement("pjz");//创建一个<Node>节点 
            xe215.InnerText = model.pjz;
            XmlElement xe216 = xmldoc.CreateElement("pdjg");//创建一个<Node>节点 
            xe216.InnerText = model.pdjg == "合格" ? "1" : "0";
            XmlElement xe217 = xmldoc.CreateElement("xz");//创建一个<Node>节点 
            xe217.InnerText = model.xz;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe2.AppendChild(xe208);
            xe2.AppendChild(xe209);
            xe2.AppendChild(xe210);
            xe2.AppendChild(xe211);
            xe2.AppendChild(xe212);
            xe2.AppendChild(xe213);
            xe2.AppendChild(xe214);
            xe2.AppendChild(xe215);
            xe2.AppendChild(xe216);
            xe2.AppendChild(xe217);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }

        //瞬态过程数据
        public void writeVmasProcessSD(VmasProcess model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "GC001";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
            xe201.InnerText = model.jylsh;
            XmlElement xe202 = xmldoc.CreateElement("jycs");//创建一个<Node>节点 
            xe202.InnerText = model.jycs;
            XmlElement xe203 = xmldoc.CreateElement("cyds");//创建一个<Node>节点 
            xe203.InnerText = model.cyds;
            XmlElement xe204 = xmldoc.CreateElement("hcclz");//创建一个<Node>节点 
            xe204.InnerText = model.hcclz;
            XmlElement xe205 = xmldoc.CreateElement("noclz");//创建一个<Node>节点 
            xe205.InnerText = model.noclz;
            XmlElement xe206 = xmldoc.CreateElement("cs");//创建一个<Node>节点 
            xe206.InnerText = model.cs;
            XmlElement xe207 = xmldoc.CreateElement("bzss");//创建一个<Node>节点 
            xe207.InnerText = model.bzss;
            XmlElement xe208 = xmldoc.CreateElement("zs");//创建一个<Node>节点 
            xe208.InnerText = model.zs;
            XmlElement xe209 = xmldoc.CreateElement("coclz");//创建一个<Node>节点 
            xe209.InnerText = model.coclz;
            XmlElement xe210 = xmldoc.CreateElement("co2clz");//创建一个<Node>节点 
            xe210.InnerText = model.co2clz;
            XmlElement xe211 = xmldoc.CreateElement("o2clz");//创建一个<Node>节点 
            xe211.InnerText = model.o2clz;
            XmlElement xe212 = xmldoc.CreateElement("xsxzxs");//创建一个<Node>节点 
            xe212.InnerText = model.xsxzxs;
            XmlElement xe213 = xmldoc.CreateElement("sdxzxs");//创建一个<Node>节点 
            xe213.InnerText = model.sdxzxs;
            XmlElement xe214 = xmldoc.CreateElement("jsgl");//创建一个<Node>节点 
            xe214.InnerText = model.jsgl;
            XmlElement xe215 = xmldoc.CreateElement("zsgl");//创建一个<Node>节点 
            xe215.InnerText = model.zsgl;
            XmlElement xe216 = xmldoc.CreateElement("jzgl");//创建一个<Node>节点 
            xe216.InnerText = model.jzgl;
            XmlElement xe217 = xmldoc.CreateElement("hjwd");//创建一个<Node>节点 
            xe217.InnerText = model.hjwd;
            XmlElement xe218 = xmldoc.CreateElement("dqyl");//创建一个<Node>节点 
            xe218.InnerText = model.dqyl;
            XmlElement xe219 = xmldoc.CreateElement("xdsd");//创建一个<Node>节点 
            xe219.InnerText = model.xdsd;
            XmlElement xe220 = xmldoc.CreateElement("yw");//创建一个<Node>节点 
            xe220.InnerText = model.yw;
            XmlElement xe221 = xmldoc.CreateElement("lljo2");//创建一个<Node>节点 
            xe221.InnerText = model.lljo2;
            XmlElement xe222 = xmldoc.CreateElement("hjo2");//创建一个<Node>节点 
            xe222.InnerText = model.hjo2;
            XmlElement xe223 = xmldoc.CreateElement("lljsjll");//创建一个<Node>节点 
            xe223.InnerText = model.lljsjll;
            XmlElement xe224 = xmldoc.CreateElement("lljbzll");//创建一个<Node>节点 
            xe224.InnerText = model.lljbzll;
            XmlElement xe225 = xmldoc.CreateElement("qcwqll");//创建一个<Node>节点 
            xe225.InnerText = model.qcwqll;
            XmlElement xe226 = xmldoc.CreateElement("xsb");//创建一个<Node>节点 
            xe226.InnerText = model.xsb;
            XmlElement xe227 = xmldoc.CreateElement("lljwd");//创建一个<Node>节点 
            xe227.InnerText = model.lljwd;
            XmlElement xe228 = xmldoc.CreateElement("lljqy");//创建一个<Node>节点 
            xe228.InnerText = model.lljqy;
            XmlElement xe229 = xmldoc.CreateElement("hcpfzl");//创建一个<Node>节点 
            xe229.InnerText = model.hcpfzl;
            XmlElement xe230 = xmldoc.CreateElement("nopfzl");//创建一个<Node>节点 
            xe230.InnerText = model.nopfzl;
            XmlElement xe231 = xmldoc.CreateElement("copfzl");//创建一个<Node>节点 
            xe231.InnerText = model.copfzl;
            XmlElement xe232 = xmldoc.CreateElement("jczt");//创建一个<Node>节点 
            xe232.InnerText = model.jczt;
            XmlElement xe233 = xmldoc.CreateElement("sjxl");//创建一个<Node>节点 
            xe233.InnerText = model.sjxl;
            XmlElement xe234 = xmldoc.CreateElement("fxyglyl");//创建一个<Node>节点 
            xe234.InnerText = model.fxyglyl;
            XmlElement xe235 = xmldoc.CreateElement("co2pfzl");//创建一个<Node>节点 
            xe235.InnerText = model.co2pfzl;
            XmlElement xe236 = xmldoc.CreateElement("nl");//创建一个<Node>节点 
            xe236.InnerText = model.nl;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe2.AppendChild(xe208);
            xe2.AppendChild(xe209);
            xe2.AppendChild(xe210);
            xe2.AppendChild(xe211);
            xe2.AppendChild(xe212);
            xe2.AppendChild(xe213);
            xe2.AppendChild(xe214);
            xe2.AppendChild(xe215);
            xe2.AppendChild(xe216);
            xe2.AppendChild(xe217);
            xe2.AppendChild(xe218);
            xe2.AppendChild(xe219);
            xe2.AppendChild(xe220);
            xe2.AppendChild(xe221);
            xe2.AppendChild(xe222);
            xe2.AppendChild(xe223);
            xe2.AppendChild(xe224);
            xe2.AppendChild(xe225);
            xe2.AppendChild(xe226);
            xe2.AppendChild(xe227);
            xe2.AppendChild(xe228);
            xe2.AppendChild(xe229);
            xe2.AppendChild(xe230);
            xe2.AppendChild(xe231);
            xe2.AppendChild(xe232);
            xe2.AppendChild(xe233);
            xe2.AppendChild(xe234);
            xe2.AppendChild(xe235);
            xe2.AppendChild(xe236);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }

        //稳态过程数据
        public void writeASMProcessSD(ASMProcess model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "GC002";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
            xe201.InnerText = model.jylsh;
            XmlElement xe202 = xmldoc.CreateElement("jycs");//创建一个<Node>节点 
            xe202.InnerText = model.jycs;
            XmlElement xe203 = xmldoc.CreateElement("cyds");//创建一个<Node>节点 
            xe203.InnerText = model.cyds;
            XmlElement xe204 = xmldoc.CreateElement("hcclz");//创建一个<Node>节点 
            xe204.InnerText = model.hcclz;
            XmlElement xe205 = xmldoc.CreateElement("noclz");//创建一个<Node>节点 
            xe205.InnerText = model.noclz;
            XmlElement xe206 = xmldoc.CreateElement("cs");//创建一个<Node>节点 
            xe206.InnerText = model.cs;
            XmlElement xe207 = xmldoc.CreateElement("zs");//创建一个<Node>节点 
            xe207.InnerText = model.zs;
            XmlElement xe208 = xmldoc.CreateElement("coclz");//创建一个<Node>节点 
            xe208.InnerText = model.coclz;
            XmlElement xe209 = xmldoc.CreateElement("co2clz");//创建一个<Node>节点 
            xe209.InnerText = model.co2clz;
            XmlElement xe210 = xmldoc.CreateElement("o2clz");//创建一个<Node>节点 
            xe210.InnerText = model.o2clz;
            XmlElement xe211 = xmldoc.CreateElement("xsxzxs");//创建一个<Node>节点 
            xe211.InnerText = model.xsxzxs;
            XmlElement xe212 = xmldoc.CreateElement("sdxzxs");//创建一个<Node>节点 
            xe212.InnerText = model.sdxzxs;
            XmlElement xe213 = xmldoc.CreateElement("jsgl");//创建一个<Node>节点 
            xe213.InnerText = model.jsgl;
            XmlElement xe214 = xmldoc.CreateElement("zsgl");//创建一个<Node>节点 
            xe214.InnerText = model.zsgl;
            XmlElement xe215 = xmldoc.CreateElement("hjwd");//创建一个<Node>节点 
            xe215.InnerText = model.hjwd;
            XmlElement xe216 = xmldoc.CreateElement("dqyl");//创建一个<Node>节点 
            xe216.InnerText = model.dqyl;
            XmlElement xe217 = xmldoc.CreateElement("xdsd");//创建一个<Node>节点 
            xe217.InnerText = model.xdsd;
            XmlElement xe218 = xmldoc.CreateElement("yw");//创建一个<Node>节点 
            xe218.InnerText = model.yw;
            XmlElement xe219 = xmldoc.CreateElement("jczt");//创建一个<Node>节点 
            xe219.InnerText = model.jczt;
            XmlElement xe220 = xmldoc.CreateElement("sjxl");//创建一个<Node>节点 
            xe220.InnerText = model.sjxl;
            XmlElement xe221 = xmldoc.CreateElement("nl");//创建一个<Node>节点 
            xe221.InnerText = model.nl;
            XmlElement xe222 = xmldoc.CreateElement("scfz");//创建一个<Node>节点 
            xe222.InnerText = model.scfz;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe2.AppendChild(xe208);
            xe2.AppendChild(xe209);
            xe2.AppendChild(xe210);
            xe2.AppendChild(xe211);
            xe2.AppendChild(xe212);
            xe2.AppendChild(xe213);
            xe2.AppendChild(xe214);
            xe2.AppendChild(xe215);
            xe2.AppendChild(xe216);
            xe2.AppendChild(xe217);
            xe2.AppendChild(xe218);
            xe2.AppendChild(xe219);
            xe2.AppendChild(xe220);
            xe2.AppendChild(xe221);
            xe2.AppendChild(xe222);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }

        //双怠速过程数据
        public void writeSDSProcessSD(SDSProcess model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "GC003";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
            xe201.InnerText = model.jylsh;
            XmlElement xe202 = xmldoc.CreateElement("jycs");//创建一个<Node>节点 
            xe202.InnerText = model.jycs;
            XmlElement xe203 = xmldoc.CreateElement("cyds");//创建一个<Node>节点 
            xe203.InnerText = model.cyds;
            XmlElement xe204 = xmldoc.CreateElement("hcclz");//创建一个<Node>节点 
            xe204.InnerText = model.hcclz;
            XmlElement xe205 = xmldoc.CreateElement("zs");//创建一个<Node>节点 
            xe205.InnerText = model.zs;
            XmlElement xe206 = xmldoc.CreateElement("coclz");//创建一个<Node>节点 
            xe206.InnerText = model.coclz;
            XmlElement xe207 = xmldoc.CreateElement("glkqxs");//创建一个<Node>节点 
            xe207.InnerText = model.glkqxs;
            XmlElement xe208 = xmldoc.CreateElement("yw");//创建一个<Node>节点 
            xe208.InnerText = model.yw;
            XmlElement xe209 = xmldoc.CreateElement("co2clz");//创建一个<Node>节点 
            xe209.InnerText = model.co2clz;
            XmlElement xe210 = xmldoc.CreateElement("o2clz");//创建一个<Node>节点 
            xe210.InnerText = model.o2clz;
            XmlElement xe211 = xmldoc.CreateElement("hjwd");//创建一个<Node>节点 
            xe211.InnerText = model.hjwd;
            XmlElement xe212 = xmldoc.CreateElement("dqyl");//创建一个<Node>节点 
            xe212.InnerText = model.dqyl;
            XmlElement xe213 = xmldoc.CreateElement("xdsd");//创建一个<Node>节点 
            xe213.InnerText = model.xdsd;
            XmlElement xe214 = xmldoc.CreateElement("sjxl");//创建一个<Node>节点 
            xe214.InnerText = model.sjxl;
            XmlElement xe215 = xmldoc.CreateElement("jczt");//创建一个<Node>节点 
            xe215.InnerText = model.jczt;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe2.AppendChild(xe208);
            xe2.AppendChild(xe209);
            xe2.AppendChild(xe210);
            xe2.AppendChild(xe211);
            xe2.AppendChild(xe212);
            xe2.AppendChild(xe213);
            xe2.AppendChild(xe214);
            xe2.AppendChild(xe215);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }

        //加载减速过程果数据
        public void writeJZJSProcessSD(JZJSProcess model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "GC004";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
            xe201.InnerText = model.jylsh;
            XmlElement xe202 = xmldoc.CreateElement("jycs");//创建一个<Node>节点 
            xe202.InnerText = model.jycs;
            XmlElement xe203 = xmldoc.CreateElement("cyds");//创建一个<Node>节点 
            xe203.InnerText = model.cyds;
            XmlElement xe204 = xmldoc.CreateElement("btgclz");//创建一个<Node>节点 
            xe204.InnerText = model.btgclz;
            XmlElement xe205 = xmldoc.CreateElement("cs");//创建一个<Node>节点 
            xe205.InnerText = model.cs;
            XmlElement xe206 = xmldoc.CreateElement("zs");//创建一个<Node>节点 
            xe206.InnerText = model.zs;
            XmlElement xe207 = xmldoc.CreateElement("zgl");//创建一个<Node>节点 
            xe207.InnerText = model.zgl;
            XmlElement xe208 = xmldoc.CreateElement("gxsxs");//创建一个<Node>节点 
            xe208.InnerText = model.gxsxs;
            XmlElement xe209 = xmldoc.CreateElement("zsgl");//创建一个<Node>节点 
            xe209.InnerText = model.zsgl;
            XmlElement xe210 = xmldoc.CreateElement("yw");//创建一个<Node>节点 
            xe210.InnerText = model.yw;
            XmlElement xe211 = xmldoc.CreateElement("glxzxs");//创建一个<Node>节点 
            xe211.InnerText = model.glxzxs;
            XmlElement xe212 = xmldoc.CreateElement("jsgl");//创建一个<Node>节点 
            xe212.InnerText = model.jsgl;
            XmlElement xe213 = xmldoc.CreateElement("jczt");//创建一个<Node>节点 
            xe213.InnerText = model.jczt;
            XmlElement xe214 = xmldoc.CreateElement("btgd");//创建一个<Node>节点 
            xe214.InnerText = model.btgd;
            XmlElement xe215 = xmldoc.CreateElement("dqyl");//创建一个<Node>节点 
            xe215.InnerText = model.dqyl;
            XmlElement xe216 = xmldoc.CreateElement("xdsd");//创建一个<Node>节点 
            xe216.InnerText = model.xdsd;
            XmlElement xe217 = xmldoc.CreateElement("hjwd");//创建一个<Node>节点 
            xe217.InnerText = model.hjwd;
            XmlElement xe218 = xmldoc.CreateElement("sjxl");//创建一个<Node>节点 
            xe218.InnerText = model.sjxl;
            XmlElement xe219 = xmldoc.CreateElement("nl");//创建一个<Node>节点 
            xe219.InnerText = model.nl;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe2.AppendChild(xe208);
            xe2.AppendChild(xe209);
            xe2.AppendChild(xe210);
            xe2.AppendChild(xe211);
            xe2.AppendChild(xe212);
            xe2.AppendChild(xe213);
            xe2.AppendChild(xe214);
            xe2.AppendChild(xe215);
            xe2.AppendChild(xe216);
            xe2.AppendChild(xe217);
            xe2.AppendChild(xe218);
            xe2.AppendChild(xe219);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }

        //不透光过程数据
        public void writeZYJSProcessSD(ZYJSProcess model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "GC005";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
            xe201.InnerText = model.jylsh;
            XmlElement xe202 = xmldoc.CreateElement("jycs");//创建一个<Node>节点 
            xe202.InnerText = model.jycs;
            XmlElement xe203 = xmldoc.CreateElement("cyds");//创建一个<Node>节点 
            xe203.InnerText = model.cyds;
            XmlElement xe204 = xmldoc.CreateElement("ydzds");//创建一个<Node>节点 
            xe204.InnerText = model.ydzds;
            XmlElement xe205 = xmldoc.CreateElement("fdjdszs");//创建一个<Node>节点 
            xe205.InnerText = model.fdjdszs;
            XmlElement xe206 = xmldoc.CreateElement("yw");//创建一个<Node>节点 
            xe206.InnerText = model.yw;
            XmlElement xe207 = xmldoc.CreateElement("jczt");//创建一个<Node>节点 
            xe207.InnerText = model.jczt;
            XmlElement xe208 = xmldoc.CreateElement("sjxl");//创建一个<Node>节点 
            xe208.InnerText = model.sjxl;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe2.AppendChild(xe208);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }

        //设备自检数据
        public void writeSelfCheckJZHX_SD(JZHXselfcheckSD model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "ZJ000";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
            xe201.InnerText = model.sbbh;
            XmlElement xe202 = xmldoc.CreateElement("zjlx");//创建一个<Node>节点 
            xe202.InnerText = model.zjlx;
            XmlElement xe203 = xmldoc.CreateElement("zjsj");//创建一个<Node>节点 
            xe203.InnerText = model.zjsj;
            XmlElement xe204 = xmldoc.CreateElement("zjnr");//创建一个<Node>节点 
            #region 加载滑行自检内容
            XmlElement xe20401 = xmldoc.CreateElement("txjc");//创建一个<Node>节点 
            xe20401.InnerText = model.txjc;
            XmlElement xe20402 = xmldoc.CreateElement("jsqjc");//创建一个<Node>节点 
            xe20402.InnerText = model.jsqjc;
            XmlElement xe20403 = xmldoc.CreateElement("yrqssj");//创建一个<Node>节点 
            xe20403.InnerText = model.yrqssj;
            XmlElement xe20404 = xmldoc.CreateElement("yrjssj");//创建一个<Node>节点 
            xe20404.InnerText = model.yrjssj;
            XmlElement xe20405 = xmldoc.CreateElement("gxdl");//创建一个<Node>节点 
            xe20405.InnerText = model.gxdl;
            XmlElement xe20406 = xmldoc.CreateElement("cs1");//创建一个<Node>节点 
            xe20406.InnerText = model.cs1;
            XmlElement xe20407 = xmldoc.CreateElement("jzgl1");//创建一个<Node>节点 
            xe20407.InnerText = model.jzgl1;
            XmlElement xe20408 = xmldoc.CreateElement("jsgl1");//创建一个<Node>节点 
            xe20408.InnerText = model.jsgl1;
            XmlElement xe20409 = xmldoc.CreateElement("llhxsj1");//创建一个<Node>节点 
            xe20409.InnerText = model.llhxsj1;
            XmlElement xe20410 = xmldoc.CreateElement("sjhxsj1");//创建一个<Node>节点 
            xe20410.InnerText = model.sjhxsj1;
            XmlElement xe20411 = xmldoc.CreateElement("wc1");//创建一个<Node>节点 
            xe20411.InnerText = model.wc1;
            XmlElement xe20412 = xmldoc.CreateElement("kssj1");//创建一个<Node>节点 
            xe20412.InnerText = model.kssj1;
            XmlElement xe20413 = xmldoc.CreateElement("jssj1");//创建一个<Node>节点 
            xe20413.InnerText = model.jssj1;
            XmlElement xe20414 = xmldoc.CreateElement("cs2");//创建一个<Node>节点 
            xe20414.InnerText = model.cs2;
            XmlElement xe20415 = xmldoc.CreateElement("jzgl2");//创建一个<Node>节点 
            xe20415.InnerText = model.jzgl2;
            XmlElement xe20416 = xmldoc.CreateElement("jsgl2");//创建一个<Node>节点 
            xe20416.InnerText = model.jsgl2;
            XmlElement xe20417 = xmldoc.CreateElement("llhxsj2");//创建一个<Node>节点 
            xe20417.InnerText = model.llhxsj2;
            XmlElement xe20418 = xmldoc.CreateElement("sjhxsj2");//创建一个<Node>节点 
            xe20418.InnerText = model.sjhxsj2;
            XmlElement xe20419 = xmldoc.CreateElement("wc2");//创建一个<Node>节点 
            xe20419.InnerText = model.wc2;
            XmlElement xe20420 = xmldoc.CreateElement("kssj2");//创建一个<Node>节点 
            xe20420.InnerText = model.kssj2;
            XmlElement xe20421 = xmldoc.CreateElement("jssj2");//创建一个<Node>节点 
            xe20421.InnerText = model.jssj2;
            xe204.AppendChild(xe20401);
            xe204.AppendChild(xe20402);
            xe204.AppendChild(xe20403);
            xe204.AppendChild(xe20404);
            xe204.AppendChild(xe20405);
            xe204.AppendChild(xe20406);
            xe204.AppendChild(xe20407);
            xe204.AppendChild(xe20408);
            xe204.AppendChild(xe20409);
            xe204.AppendChild(xe20410);
            xe204.AppendChild(xe20411);
            xe204.AppendChild(xe20412);
            xe204.AppendChild(xe20413);
            xe204.AppendChild(xe20414);
            xe204.AppendChild(xe20415);
            xe204.AppendChild(xe20416);
            xe204.AppendChild(xe20417);
            xe204.AppendChild(xe20418);
            xe204.AppendChild(xe20419);
            xe204.AppendChild(xe20420);
            xe204.AppendChild(xe20421);
            #endregion
            XmlElement xe205 = xmldoc.CreateElement("zjjg");//创建一个<Node>节点 
            xe205.InnerText = model.zjjg;
            XmlElement xe206 = xmldoc.CreateElement("jczbh");//创建一个<Node>节点 
            xe206.InnerText = model.jczbh;
            XmlElement xe207 = xmldoc.CreateElement("jcgwh");//创建一个<Node>节点 
            xe207.InnerText = model.jcgwh;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }
        public void writeSelfCheckFQY_SD(FQYselfcheckSD model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "ZJ000";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
            xe201.InnerText = model.sbbh;
            XmlElement xe202 = xmldoc.CreateElement("zjlx");//创建一个<Node>节点 
            xe202.InnerText = model.zjlx;
            XmlElement xe203 = xmldoc.CreateElement("zjsj");//创建一个<Node>节点 
            xe203.InnerText = model.zjsj;
            XmlElement xe204 = xmldoc.CreateElement("zjnr");//创建一个<Node>节点 
            #region 废气仪自检内容
            XmlElement xe20401 = xmldoc.CreateElement("txjc");//创建一个<Node>节点 
            xe20401.InnerText = model.txjc;
            XmlElement xe20402 = xmldoc.CreateElement("yqyr");//创建一个<Node>节点 
            xe20402.InnerText = model.yqyr;
            XmlElement xe20403 = xmldoc.CreateElement("yqjl");//创建一个<Node>节点 
            xe20403.InnerText = model.yqjl;
            XmlElement xe20404 = xmldoc.CreateElement("yqtl");//创建一个<Node>节点 
            xe20404.InnerText = model.yqtl;
            XmlElement xe20405 = xmldoc.CreateElement("yqyq");//创建一个<Node>节点 
            xe20405.InnerText = model.yqyq;
            XmlElement xe20406 = xmldoc.CreateElement("clhc");//创建一个<Node>节点 
            xe20406.InnerText = model.clhc;
            XmlElement xe20407 = xmldoc.CreateElement("kssj");//创建一个<Node>节点 
            xe20407.InnerText = model.kssj;
            XmlElement xe20408 = xmldoc.CreateElement("jssj");//创建一个<Node>节点 
            xe20408.InnerText = model.jssj;
            xe204.AppendChild(xe20401);
            xe204.AppendChild(xe20402);
            xe204.AppendChild(xe20403);
            xe204.AppendChild(xe20404);
            xe204.AppendChild(xe20405);
            xe204.AppendChild(xe20406);
            xe204.AppendChild(xe20407);
            xe204.AppendChild(xe20408);
            #endregion
            XmlElement xe205 = xmldoc.CreateElement("zjjg");//创建一个<Node>节点 
            xe205.InnerText = model.zjjg;
            XmlElement xe206 = xmldoc.CreateElement("jczbh");//创建一个<Node>节点 
            xe206.InnerText = model.jczbh;
            XmlElement xe207 = xmldoc.CreateElement("jcgwh");//创建一个<Node>节点 
            xe207.InnerText = model.jcgwh;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }
        public void writeSelfCheckYDJ_SD(YDJselfcheckSD model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "ZJ000";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
            xe201.InnerText = model.sbbh;
            XmlElement xe202 = xmldoc.CreateElement("zjlx");//创建一个<Node>节点 
            xe202.InnerText = model.zjlx;
            XmlElement xe203 = xmldoc.CreateElement("zjsj");//创建一个<Node>节点 
            xe203.InnerText = model.zjsj;
            XmlElement xe204 = xmldoc.CreateElement("zjnr");//创建一个<Node>节点 
            #region 烟度计自检内容
            XmlElement xe20401 = xmldoc.CreateElement("txjc");//创建一个<Node>节点 
            xe20401.InnerText = model.txjc;
            XmlElement xe20402 = xmldoc.CreateElement("yqyr");//创建一个<Node>节点 
            xe20402.InnerText = model.yqyr;
            XmlElement xe20403 = xmldoc.CreateElement("yqtl");//创建一个<Node>节点 
            xe20403.InnerText = model.yqtl;
            XmlElement xe20404 = xmldoc.CreateElement("lcjc");//创建一个<Node>节点 
            xe20404.InnerText = model.lcjc;
            XmlElement xe20405 = xmldoc.CreateElement("sdz1");//创建一个<Node>节点 
            xe20405.InnerText = model.sdz1;
            XmlElement xe20406 = xmldoc.CreateElement("scz1");//创建一个<Node>节点 
            xe20406.InnerText = model.scz1;
            XmlElement xe20407 = xmldoc.CreateElement("wcz1");//创建一个<Node>节点 
            xe20407.InnerText = model.wcz1;
            XmlElement xe20408 = xmldoc.CreateElement("sdz2");//创建一个<Node>节点 
            xe20408.InnerText = model.sdz2;
            XmlElement xe20409 = xmldoc.CreateElement("scz2");//创建一个<Node>节点 
            xe20409.InnerText = model.scz2;
            XmlElement xe20410 = xmldoc.CreateElement("wcz2");//创建一个<Node>节点 
            xe20410.InnerText = model.wcz2;
            XmlElement xe20411 = xmldoc.CreateElement("kssj");//创建一个<Node>节点 
            xe20411.InnerText = model.kssj;
            XmlElement xe20412 = xmldoc.CreateElement("jssj");//创建一个<Node>节点 
            xe20412.InnerText = model.jssj;
            xe204.AppendChild(xe20401);
            xe204.AppendChild(xe20402);
            xe204.AppendChild(xe20403);
            xe204.AppendChild(xe20404);
            xe204.AppendChild(xe20405);
            xe204.AppendChild(xe20406);
            xe204.AppendChild(xe20407);
            xe204.AppendChild(xe20408);
            xe204.AppendChild(xe20409);
            xe204.AppendChild(xe20410);
            xe204.AppendChild(xe20411);
            xe204.AppendChild(xe20412);
            #endregion
            XmlElement xe205 = xmldoc.CreateElement("zjjg");//创建一个<Node>节点 
            xe205.InnerText = model.zjjg;
            XmlElement xe206 = xmldoc.CreateElement("jczbh");//创建一个<Node>节点 
            xe206.InnerText = model.jczbh;
            XmlElement xe207 = xmldoc.CreateElement("jcgwh");//创建一个<Node>节点 
            xe207.InnerText = model.jcgwh;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }
        public void writeSelfCheckDZHJY_SD(HJYselfcheckSD model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "ZJ000";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
            xe201.InnerText = model.sbbh;
            XmlElement xe202 = xmldoc.CreateElement("zjlx");//创建一个<Node>节点 
            xe202.InnerText = model.zjlx;
            XmlElement xe203 = xmldoc.CreateElement("zjsj");//创建一个<Node>节点 
            xe203.InnerText = model.zjsj;
            XmlElement xe204 = xmldoc.CreateElement("zjnr");//创建一个<Node>节点 
            #region 电子环境信息仪自检内容
            XmlElement xe20401 = xmldoc.CreateElement("txjc");//创建一个<Node>节点 
            xe20401.InnerText = model.txjc;
            XmlElement xe20402 = xmldoc.CreateElement("hjwd");//创建一个<Node>节点 
            xe20402.InnerText = model.hjwd;
            XmlElement xe20403 = xmldoc.CreateElement("hjsd");//创建一个<Node>节点 
            xe20403.InnerText = model.hjsd;
            XmlElement xe20404 = xmldoc.CreateElement("hjqy");//创建一个<Node>节点 
            xe20404.InnerText = model.hjqy;
            XmlElement xe20405 = xmldoc.CreateElement("yqwd");//创建一个<Node>节点 
            xe20405.InnerText = model.yqwd;
            XmlElement xe20406 = xmldoc.CreateElement("yqsd");//创建一个<Node>节点 
            xe20406.InnerText = model.yqsd;
            XmlElement xe20407 = xmldoc.CreateElement("yqqy");//创建一个<Node>节点 
            xe20407.InnerText = model.yqqy;
            XmlElement xe20408 = xmldoc.CreateElement("kssj");//创建一个<Node>节点 
            xe20408.InnerText = model.kssj;
            XmlElement xe20409 = xmldoc.CreateElement("jssj");//创建一个<Node>节点 
            xe20409.InnerText = model.jssj;
            xe204.AppendChild(xe20401);
            xe204.AppendChild(xe20402);
            xe204.AppendChild(xe20403);
            xe204.AppendChild(xe20404);
            xe204.AppendChild(xe20405);
            xe204.AppendChild(xe20406);
            xe204.AppendChild(xe20407);
            xe204.AppendChild(xe20408);
            xe204.AppendChild(xe20409);
            #endregion
            XmlElement xe205 = xmldoc.CreateElement("zjjg");//创建一个<Node>节点 
            xe205.InnerText = model.zjjg;
            XmlElement xe206 = xmldoc.CreateElement("jczbh");//创建一个<Node>节点 
            xe206.InnerText = model.jczbh;
            XmlElement xe207 = xmldoc.CreateElement("jcgwh");//创建一个<Node>节点 
            xe207.InnerText = model.jcgwh;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }
        public void writeSelfCheckZSJ_SD(ZSJselfcheckSD model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "ZJ000";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
            xe201.InnerText = model.sbbh;
            XmlElement xe202 = xmldoc.CreateElement("zjlx");//创建一个<Node>节点 
            xe202.InnerText = model.zjlx;
            XmlElement xe203 = xmldoc.CreateElement("zjsj");//创建一个<Node>节点 
            xe203.InnerText = model.zjsj;
            XmlElement xe204 = xmldoc.CreateElement("zjnr");//创建一个<Node>节点 
            #region 转速计自检内容
            XmlElement xe20401 = xmldoc.CreateElement("txjc");//创建一个<Node>节点 
            xe20401.InnerText = model.txjc;
            XmlElement xe20402 = xmldoc.CreateElement("dszs");//创建一个<Node>节点 
            xe20402.InnerText = model.dszs;
            xe204.AppendChild(xe20401);
            xe204.AppendChild(xe20402);
            #endregion
            XmlElement xe205 = xmldoc.CreateElement("zjjg");//创建一个<Node>节点 
            xe205.InnerText = model.zjjg;
            XmlElement xe206 = xmldoc.CreateElement("jczbh");//创建一个<Node>节点 
            xe206.InnerText = model.jczbh;
            XmlElement xe207 = xmldoc.CreateElement("jcgwh");//创建一个<Node>节点 
            xe207.InnerText = model.jcgwh;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }
        public void writeSelfCheckLLJ_SD(LLJselfcheckSD model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "ZJ000";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
            xe201.InnerText = model.sbbh;
            XmlElement xe202 = xmldoc.CreateElement("zjlx");//创建一个<Node>节点 
            xe202.InnerText = model.zjlx;
            XmlElement xe203 = xmldoc.CreateElement("zjsj");//创建一个<Node>节点 
            xe203.InnerText = model.zjsj;
            XmlElement xe204 = xmldoc.CreateElement("zjnr");//创建一个<Node>节点 
            #region 流量计自检内容
            XmlElement xe20401 = xmldoc.CreateElement("txjc");//创建一个<Node>节点 
            xe20401.InnerText = model.txjc;
            XmlElement xe20402 = xmldoc.CreateElement("yqyr");//创建一个<Node>节点 
            xe20402.InnerText = model.yqyr;
            XmlElement xe20403 = xmldoc.CreateElement("lljc");//创建一个<Node>节点 
            xe20403.InnerText = model.lljc;
            XmlElement xe20404 = xmldoc.CreateElement("yqlc");//创建一个<Node>节点 
            xe20404.InnerText = model.yqlc;
            XmlElement xe20405 = xmldoc.CreateElement("yqll");//创建一个<Node>节点 
            xe20405.InnerText = model.yqll;
            XmlElement xe20406 = xmldoc.CreateElement("yqyq");//创建一个<Node>节点 
            xe20406.InnerText = model.yqyq;
            XmlElement xe20407 = xmldoc.CreateElement("kssj");//创建一个<Node>节点 
            xe20407.InnerText = model.kssj;
            XmlElement xe20408 = xmldoc.CreateElement("jssj");//创建一个<Node>节点 
            xe20408.InnerText = model.jssj;
            xe204.AppendChild(xe20401);
            xe204.AppendChild(xe20402);
            xe204.AppendChild(xe20403);
            xe204.AppendChild(xe20404);
            xe204.AppendChild(xe20405);
            xe204.AppendChild(xe20406);
            xe204.AppendChild(xe20407);
            xe204.AppendChild(xe20408);
            #endregion
            XmlElement xe205 = xmldoc.CreateElement("zjjg");//创建一个<Node>节点 
            xe205.InnerText = model.zjjg;
            XmlElement xe206 = xmldoc.CreateElement("jczbh");//创建一个<Node>节点 
            xe206.InnerText = model.jczbh;
            XmlElement xe207 = xmldoc.CreateElement("jcgwh");//创建一个<Node>节点 
            xe207.InnerText = model.jcgwh;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }
        public void writeSelfCheckJSGL_SD(JSGLselfcheckSD model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "ZJ000";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
            xe201.InnerText = model.sbbh;
            XmlElement xe202 = xmldoc.CreateElement("zjlx");//创建一个<Node>节点 
            xe202.InnerText = model.zjlx;
            XmlElement xe203 = xmldoc.CreateElement("zjsj");//创建一个<Node>节点 
            xe203.InnerText = model.zjsj;
            XmlElement xe204 = xmldoc.CreateElement("zjnr");//创建一个<Node>节点 
            #region 寄生功率自检内容
            XmlElement xe20401 = xmldoc.CreateElement("hxsj1");//创建一个<Node>节点 
            xe20401.InnerText = model.hxsj1;
            XmlElement xe20402 = xmldoc.CreateElement("jsgl1");//创建一个<Node>节点 
            xe20402.InnerText = model.jsgl1;
            XmlElement xe20403 = xmldoc.CreateElement("sdzd1");//创建一个<Node>节点 
            xe20403.InnerText = model.sdzd1;
            XmlElement xe20404 = xmldoc.CreateElement("sdzx1");//创建一个<Node>节点 
            xe20404.InnerText = model.sdzx1;
            XmlElement xe20405 = xmldoc.CreateElement("mysd1");//创建一个<Node>节点 
            xe20405.InnerText = model.mysd1;
            XmlElement xe20406 = xmldoc.CreateElement("kssj1");//创建一个<Node>节点 
            xe20406.InnerText = model.kssj1;
            XmlElement xe20407 = xmldoc.CreateElement("jssj1");//创建一个<Node>节点 
            xe20407.InnerText = model.jssj1;
            XmlElement xe20408 = xmldoc.CreateElement("hxsj2");//创建一个<Node>节点 
            xe20408.InnerText = model.hxsj2;
            XmlElement xe20409 = xmldoc.CreateElement("jsgl2");//创建一个<Node>节点 
            xe20409.InnerText = model.jsgl2;
            XmlElement xe20410 = xmldoc.CreateElement("sdzd2");//创建一个<Node>节点 
            xe20410.InnerText = model.sdzd2;
            XmlElement xe20411 = xmldoc.CreateElement("sdzx2");//创建一个<Node>节点 
            xe20411.InnerText = model.sdzx2;
            XmlElement xe20412 = xmldoc.CreateElement("mysd2");//创建一个<Node>节点 
            xe20412.InnerText = model.mysd2;
            XmlElement xe20413 = xmldoc.CreateElement("kssj2");//创建一个<Node>节点 
            xe20413.InnerText = model.kssj2;
            XmlElement xe20414 = xmldoc.CreateElement("jssj2");//创建一个<Node>节点 
            xe20414.InnerText = model.jssj2;
            XmlElement xe20415 = xmldoc.CreateElement("hxsj3");//创建一个<Node>节点 
            xe20415.InnerText = model.hxsj3;
            XmlElement xe20416 = xmldoc.CreateElement("jsgl3");//创建一个<Node>节点 
            xe20416.InnerText = model.jsgl3;
            XmlElement xe20417 = xmldoc.CreateElement("sdzd3");//创建一个<Node>节点 
            xe20417.InnerText = model.sdzd3;
            XmlElement xe20418 = xmldoc.CreateElement("sdzx3");//创建一个<Node>节点 
            xe20418.InnerText = model.sdzx3;
            XmlElement xe20419 = xmldoc.CreateElement("mysd3");//创建一个<Node>节点 
            xe20419.InnerText = model.mysd3;
            XmlElement xe20420 = xmldoc.CreateElement("kssj3");//创建一个<Node>节点 
            xe20420.InnerText = model.kssj3;
            XmlElement xe20421 = xmldoc.CreateElement("jssj3");//创建一个<Node>节点 
            xe20421.InnerText = model.jssj3;
            XmlElement xe20422 = xmldoc.CreateElement("hxsj4");//创建一个<Node>节点 
            xe20422.InnerText = model.hxsj4;
            XmlElement xe20423 = xmldoc.CreateElement("jsgl4");//创建一个<Node>节点 
            xe20423.InnerText = model.jsgl4;
            XmlElement xe20424 = xmldoc.CreateElement("sdzd4");//创建一个<Node>节点 
            xe20424.InnerText = model.sdzd4;
            XmlElement xe20425 = xmldoc.CreateElement("sdzx4");//创建一个<Node>节点 
            xe20425.InnerText = model.sdzx4;
            XmlElement xe20426 = xmldoc.CreateElement("mysd4");//创建一个<Node>节点 
            xe20426.InnerText = model.mysd4;
            XmlElement xe20427 = xmldoc.CreateElement("kssj4");//创建一个<Node>节点 
            xe20427.InnerText = model.kssj4;
            XmlElement xe20428 = xmldoc.CreateElement("jssj4");//创建一个<Node>节点 
            xe20428.InnerText = model.jssj4;
            XmlElement xe20429 = xmldoc.CreateElement("hxsj5");//创建一个<Node>节点 
            xe20429.InnerText = model.hxsj5;
            XmlElement xe20430 = xmldoc.CreateElement("jsgl5");//创建一个<Node>节点 
            xe20430.InnerText = model.jsgl5;
            XmlElement xe20431 = xmldoc.CreateElement("sdzd5");//创建一个<Node>节点 
            xe20431.InnerText = model.sdzd5;
            XmlElement xe20432 = xmldoc.CreateElement("sdzx5");//创建一个<Node>节点 
            xe20432.InnerText = model.sdzx5;
            XmlElement xe20433 = xmldoc.CreateElement("mysd5");//创建一个<Node>节点 
            xe20433.InnerText = model.mysd5;
            XmlElement xe20434 = xmldoc.CreateElement("kssj5");//创建一个<Node>节点 
            xe20434.InnerText = model.kssj5;
            XmlElement xe20435 = xmldoc.CreateElement("jssj5");//创建一个<Node>节点 
            xe20435.InnerText = model.jssj5;
            xe204.AppendChild(xe20401);
            xe204.AppendChild(xe20402);
            xe204.AppendChild(xe20403);
            xe204.AppendChild(xe20404);
            xe204.AppendChild(xe20405);
            xe204.AppendChild(xe20406);
            xe204.AppendChild(xe20407);
            xe204.AppendChild(xe20408);
            xe204.AppendChild(xe20409);
            xe204.AppendChild(xe20410);
            xe204.AppendChild(xe20411);
            xe204.AppendChild(xe20412);
            xe204.AppendChild(xe20413);
            xe204.AppendChild(xe20414);
            xe204.AppendChild(xe20415);
            xe204.AppendChild(xe20416);
            xe204.AppendChild(xe20417);
            xe204.AppendChild(xe20418);
            xe204.AppendChild(xe20419);
            xe204.AppendChild(xe20420);
            xe204.AppendChild(xe20421);
            xe204.AppendChild(xe20422);
            xe204.AppendChild(xe20423);
            xe204.AppendChild(xe20424);
            xe204.AppendChild(xe20425);
            xe204.AppendChild(xe20426);
            xe204.AppendChild(xe20427);
            xe204.AppendChild(xe20428);
            xe204.AppendChild(xe20429);
            xe204.AppendChild(xe20430);
            xe204.AppendChild(xe20431);
            xe204.AppendChild(xe20432);
            xe204.AppendChild(xe20433);
            xe204.AppendChild(xe20434);
            xe204.AppendChild(xe20435);
            #endregion
            XmlElement xe205 = xmldoc.CreateElement("zjjg");//创建一个<Node>节点 
            xe205.InnerText = model.zjjg;
            XmlElement xe206 = xmldoc.CreateElement("jczbh");//创建一个<Node>节点 
            xe206.InnerText = model.jczbh;
            XmlElement xe207 = xmldoc.CreateElement("jcgwh");//创建一个<Node>节点 
            xe207.InnerText = model.jcgwh;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }
        //设备标定数据
        public void writeSpeedBD_SD(SpeedBD_SD model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "BD000";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
            xe201.InnerText = model.sbbh;
            XmlElement xe202 = xmldoc.CreateElement("bdsj");//创建一个<Node>节点 
            xe202.InnerText = model.bdsj;
            XmlElement xe203 = xmldoc.CreateElement("bdr");//创建一个<Node>节点 
            xe203.InnerText = model.bdr;
            XmlElement xe204 = xmldoc.CreateElement("bdlx");//创建一个<Node>节点 
            xe204.InnerText = model.bdlx;
            XmlElement xe205 = xmldoc.CreateElement("bdnr");//创建一个<Node>节点 
            #region 速度标定内容
            XmlElement xe20501 = xmldoc.CreateElement("sdz1");//创建一个<Node>节点 
            xe20501.InnerText = model.sdz1;
            XmlElement xe20502 = xmldoc.CreateElement("scz1");//创建一个<Node>节点 
            xe20502.InnerText = model.scz1;
            XmlElement xe20503 = xmldoc.CreateElement("jdwcz1");//创建一个<Node>节点 
            xe20503.InnerText = model.jdwcz1;
            XmlElement xe20504 = xmldoc.CreateElement("xdwcz1");//创建一个<Node>节点 
            xe20504.InnerText = model.xdwcz1;
            XmlElement xe20505 = xmldoc.CreateElement("sdz2");//创建一个<Node>节点 
            xe20505.InnerText = model.sdz2;
            XmlElement xe20506 = xmldoc.CreateElement("scz2");//创建一个<Node>节点 
            xe20506.InnerText = model.scz2;
            XmlElement xe20507 = xmldoc.CreateElement("jdwcz2");//创建一个<Node>节点 
            xe20507.InnerText = model.jdwcz2;
            XmlElement xe20508 = xmldoc.CreateElement("xdwcz2");//创建一个<Node>节点 
            xe20508.InnerText = model.xdwcz2;
            XmlElement xe20509 = xmldoc.CreateElement("sdz3");//创建一个<Node>节点 
            xe20509.InnerText = model.sdz3;
            XmlElement xe20510 = xmldoc.CreateElement("scz3");//创建一个<Node>节点 
            xe20510.InnerText = model.scz3;
            XmlElement xe20511 = xmldoc.CreateElement("jdwcz3");//创建一个<Node>节点 
            xe20511.InnerText = model.jdwcz3;
            XmlElement xe20512 = xmldoc.CreateElement("xdwcz3");//创建一个<Node>节点 
            xe20512.InnerText = model.xdwcz3;
            xe205.AppendChild(xe20501);
            xe205.AppendChild(xe20502);
            xe205.AppendChild(xe20503);
            xe205.AppendChild(xe20504);
            xe205.AppendChild(xe20505);
            xe205.AppendChild(xe20506);
            xe205.AppendChild(xe20507);
            xe205.AppendChild(xe20508);
            xe205.AppendChild(xe20509);
            xe205.AppendChild(xe20510);
            xe205.AppendChild(xe20511);
            xe205.AppendChild(xe20512);
            #endregion
            XmlElement xe206 = xmldoc.CreateElement("bdjg");//创建一个<Node>节点 
            xe206.InnerText = model.bdjg;
            XmlElement xe207 = xmldoc.CreateElement("jczbh");//创建一个<Node>节点 
            xe207.InnerText = model.jczbh;
            XmlElement xe208 = xmldoc.CreateElement("jcgwh");//创建一个<Node>节点 
            xe208.InnerText = model.jcgwh;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe2.AppendChild(xe208);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }
        public void writeNlBD_SD(NlBD_SD model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "BD000";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
            xe201.InnerText = model.sbbh;
            XmlElement xe202 = xmldoc.CreateElement("bdsj");//创建一个<Node>节点 
            xe202.InnerText = model.bdsj;
            XmlElement xe203 = xmldoc.CreateElement("bdr");//创建一个<Node>节点 
            xe203.InnerText = model.bdr;
            XmlElement xe204 = xmldoc.CreateElement("bdlx");//创建一个<Node>节点 
            xe204.InnerText = model.bdlx;
            XmlElement xe205 = xmldoc.CreateElement("bdnr");//创建一个<Node>节点 
            #region 扭力标定内容
            XmlElement xe20501 = xmldoc.CreateElement("nlsdz1");//创建一个<Node>节点 
            xe20501.InnerText = model.nlsdz1;
            XmlElement xe20502 = xmldoc.CreateElement("nlscz1");//创建一个<Node>节点 
            xe20502.InnerText = model.nlscz1;
            XmlElement xe20503 = xmldoc.CreateElement("nlwcz1");//创建一个<Node>节点 
            xe20503.InnerText = model.nlwcz1;
            XmlElement xe20504 = xmldoc.CreateElement("nlsdz2");//创建一个<Node>节点 
            xe20504.InnerText = model.nlsdz2;
            XmlElement xe20505 = xmldoc.CreateElement("nlscz2");//创建一个<Node>节点 
            xe20505.InnerText = model.nlscz2;
            XmlElement xe20506 = xmldoc.CreateElement("nlwcz2");//创建一个<Node>节点 
            xe20506.InnerText = model.nlwcz2;
            XmlElement xe20507 = xmldoc.CreateElement("nlsdz3");//创建一个<Node>节点 
            xe20507.InnerText = model.nlsdz3;
            XmlElement xe20508 = xmldoc.CreateElement("nlscz3");//创建一个<Node>节点 
            xe20508.InnerText = model.nlscz3;
            XmlElement xe20509 = xmldoc.CreateElement("nlwcz3");//创建一个<Node>节点 
            xe20509.InnerText = model.nlwcz3;
            XmlElement xe20510 = xmldoc.CreateElement("nlsdz4");//创建一个<Node>节点 
            xe20510.InnerText = model.nlsdz4;
            XmlElement xe20511 = xmldoc.CreateElement("nlscz4");//创建一个<Node>节点 
            xe20511.InnerText = model.nlscz4;
            XmlElement xe20512 = xmldoc.CreateElement("nlwcz4");//创建一个<Node>节点 
            xe20512.InnerText = model.nlwcz4;
            XmlElement xe20513 = xmldoc.CreateElement("nlsdz5");//创建一个<Node>节点 
            xe20513.InnerText = model.nlsdz5;
            XmlElement xe20514 = xmldoc.CreateElement("nlscz5");//创建一个<Node>节点 
            xe20514.InnerText = model.nlscz5;
            XmlElement xe20515 = xmldoc.CreateElement("nlwcz5");//创建一个<Node>节点 
            xe20515.InnerText = model.nlwcz5;
            xe205.AppendChild(xe20501);
            xe205.AppendChild(xe20502);
            xe205.AppendChild(xe20503);
            xe205.AppendChild(xe20504);
            xe205.AppendChild(xe20505);
            xe205.AppendChild(xe20506);
            xe205.AppendChild(xe20507);
            xe205.AppendChild(xe20508);
            xe205.AppendChild(xe20509);
            xe205.AppendChild(xe20510);
            xe205.AppendChild(xe20511);
            xe205.AppendChild(xe20512);
            xe205.AppendChild(xe20513);
            xe205.AppendChild(xe20514);
            xe205.AppendChild(xe20515);
            #endregion
            XmlElement xe206 = xmldoc.CreateElement("bdjg");//创建一个<Node>节点 
            xe206.InnerText = model.bdjg;
            XmlElement xe207 = xmldoc.CreateElement("jczbh");//创建一个<Node>节点 
            xe207.InnerText = model.jczbh;
            XmlElement xe208 = xmldoc.CreateElement("jcgwh");//创建一个<Node>节点 
            xe208.InnerText = model.jcgwh;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe2.AppendChild(xe208);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }
        public void writeJsglBD_SD(JsglBD model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "BD000";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
            xe201.InnerText = model.sbbh;
            XmlElement xe202 = xmldoc.CreateElement("bdsj");//创建一个<Node>节点 
            xe202.InnerText = model.bdsj;
            XmlElement xe203 = xmldoc.CreateElement("bdr");//创建一个<Node>节点 
            xe203.InnerText = model.bdr;
            XmlElement xe204 = xmldoc.CreateElement("bdlx");//创建一个<Node>节点 
            xe204.InnerText = model.bdlx;
            XmlElement xe205 = xmldoc.CreateElement("bdnr");//创建一个<Node>节点 
            #region 寄生功率标定内容
            XmlElement xe20501 = xmldoc.CreateElement("hxsj1");//创建一个<Node>节点 
            xe20501.InnerText = model.hxsj1;
            XmlElement xe20502 = xmldoc.CreateElement("jsgl1");//创建一个<Node>节点 
            xe20502.InnerText = model.jsgl1;
            XmlElement xe20503 = xmldoc.CreateElement("sdzd1");//创建一个<Node>节点 
            xe20503.InnerText = model.sdzd1;
            XmlElement xe20504 = xmldoc.CreateElement("sdzx1");//创建一个<Node>节点 
            xe20504.InnerText = model.sdzx1;
            XmlElement xe20505 = xmldoc.CreateElement("mysd1");//创建一个<Node>节点 
            xe20505.InnerText = model.mysd1;
            XmlElement xe20506 = xmldoc.CreateElement("jbgl");//创建一个<Node>节点 
            xe20506.InnerText = model.jbgl;
            XmlElement xe20507 = xmldoc.CreateElement("kssj1");//创建一个<Node>节点 
            xe20507.InnerText = model.kssj1;
            XmlElement xe20508 = xmldoc.CreateElement("jssj1");//创建一个<Node>节点 
            xe20508.InnerText = model.jssj1;
            XmlElement xe20509 = xmldoc.CreateElement("hxsj2");//创建一个<Node>节点 
            xe20509.InnerText = model.hxsj2;
            XmlElement xe20510 = xmldoc.CreateElement("jsgl2");//创建一个<Node>节点 
            xe20510.InnerText = model.jsgl2;
            XmlElement xe20511 = xmldoc.CreateElement("sdzd2");//创建一个<Node>节点 
            xe20511.InnerText = model.sdzd2;
            XmlElement xe20512 = xmldoc.CreateElement("sdzx2");//创建一个<Node>节点 
            xe20512.InnerText = model.sdzx2;
            XmlElement xe20513 = xmldoc.CreateElement("mysd2");//创建一个<Node>节点 
            xe20513.InnerText = model.mysd2;
            XmlElement xe20514 = xmldoc.CreateElement("kssj2");//创建一个<Node>节点 
            xe20514.InnerText = model.kssj2;
            XmlElement xe20515 = xmldoc.CreateElement("jssj2");//创建一个<Node>节点 
            xe20515.InnerText = model.jssj2;
            XmlElement xe20516 = xmldoc.CreateElement("hxsj3");//创建一个<Node>节点 
            xe20516.InnerText = model.hxsj3;
            XmlElement xe20517 = xmldoc.CreateElement("jsgl3");//创建一个<Node>节点 
            xe20517.InnerText = model.jsgl3;
            XmlElement xe20518 = xmldoc.CreateElement("sdzd3");//创建一个<Node>节点 
            xe20518.InnerText = model.sdzd3;
            XmlElement xe20519 = xmldoc.CreateElement("sdzx3");//创建一个<Node>节点 
            xe20519.InnerText = model.sdzx3;
            XmlElement xe20520 = xmldoc.CreateElement("mysd3");//创建一个<Node>节点 
            xe20520.InnerText = model.mysd3;
            XmlElement xe20521 = xmldoc.CreateElement("kssj3");//创建一个<Node>节点 
            xe20521.InnerText = model.kssj3;
            XmlElement xe20522 = xmldoc.CreateElement("jssj3");//创建一个<Node>节点 
            xe20522.InnerText = model.jssj3;
            XmlElement xe20523 = xmldoc.CreateElement("hxsj4");//创建一个<Node>节点 
            xe20523.InnerText = model.hxsj4;
            XmlElement xe20524 = xmldoc.CreateElement("jsgl4");//创建一个<Node>节点 
            xe20524.InnerText = model.jsgl4;
            XmlElement xe20525 = xmldoc.CreateElement("sdzd4");//创建一个<Node>节点 
            xe20525.InnerText = model.sdzd4;
            XmlElement xe20526 = xmldoc.CreateElement("sdzx4");//创建一个<Node>节点 
            xe20526.InnerText = model.sdzx4;
            XmlElement xe20527 = xmldoc.CreateElement("mysd4");//创建一个<Node>节点 
            xe20527.InnerText = model.mysd4;
            XmlElement xe20528 = xmldoc.CreateElement("kssj4");//创建一个<Node>节点 
            xe20528.InnerText = model.kssj4;
            XmlElement xe20529 = xmldoc.CreateElement("jssj4");//创建一个<Node>节点 
            xe20529.InnerText = model.jssj4;
            XmlElement xe20530 = xmldoc.CreateElement("hxsj5");//创建一个<Node>节点 
            xe20530.InnerText = model.hxsj5;
            XmlElement xe20531 = xmldoc.CreateElement("jsgl5");//创建一个<Node>节点 
            xe20531.InnerText = model.jsgl5;
            XmlElement xe20532 = xmldoc.CreateElement("sdzd5");//创建一个<Node>节点 
            xe20532.InnerText = model.sdzd5;
            XmlElement xe20533 = xmldoc.CreateElement("sdzx5");//创建一个<Node>节点 
            xe20533.InnerText = model.sdzx5;
            XmlElement xe20534 = xmldoc.CreateElement("mysd5");//创建一个<Node>节点 
            xe20534.InnerText = model.mysd5;
            XmlElement xe20535 = xmldoc.CreateElement("kssj5");//创建一个<Node>节点 
            xe20535.InnerText = model.kssj5;
            XmlElement xe20536 = xmldoc.CreateElement("jssj5");//创建一个<Node>节点 
            xe20536.InnerText = model.jssj5;
            xe205.AppendChild(xe20501);
            xe205.AppendChild(xe20502);
            xe205.AppendChild(xe20503);
            xe205.AppendChild(xe20504);
            xe205.AppendChild(xe20505);
            xe205.AppendChild(xe20506);
            xe205.AppendChild(xe20507);
            xe205.AppendChild(xe20508);
            xe205.AppendChild(xe20509);
            xe205.AppendChild(xe20510);
            xe205.AppendChild(xe20511);
            xe205.AppendChild(xe20512);
            xe205.AppendChild(xe20513);
            xe205.AppendChild(xe20514);
            xe205.AppendChild(xe20515);
            xe205.AppendChild(xe20516);
            xe205.AppendChild(xe20517);
            xe205.AppendChild(xe20518);
            xe205.AppendChild(xe20519);
            xe205.AppendChild(xe20520);
            xe205.AppendChild(xe20521);
            xe205.AppendChild(xe20522);
            xe205.AppendChild(xe20523);
            xe205.AppendChild(xe20524);
            xe205.AppendChild(xe20525);
            xe205.AppendChild(xe20526);
            xe205.AppendChild(xe20527);
            xe205.AppendChild(xe20528);
            xe205.AppendChild(xe20529);
            xe205.AppendChild(xe20530);
            xe205.AppendChild(xe20531);
            xe205.AppendChild(xe20532);
            xe205.AppendChild(xe20533);
            xe205.AppendChild(xe20534);
            xe205.AppendChild(xe20535);
            xe205.AppendChild(xe20536);
            #endregion
            XmlElement xe206 = xmldoc.CreateElement("bdjg");//创建一个<Node>节点 
            xe206.InnerText = model.bdjg;
            XmlElement xe207 = xmldoc.CreateElement("jczbh");//创建一个<Node>节点 
            xe207.InnerText = model.jczbh;
            XmlElement xe208 = xmldoc.CreateElement("jcgwh");//创建一个<Node>节点 
            xe208.InnerText = model.jcgwh;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe2.AppendChild(xe208);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }
        public void writeJzhxBD_SD(JzhxBD model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "BD000";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
            xe201.InnerText = model.sbbh;
            XmlElement xe202 = xmldoc.CreateElement("bdsj");//创建一个<Node>节点 
            xe202.InnerText = model.bdsj;
            XmlElement xe203 = xmldoc.CreateElement("bdr");//创建一个<Node>节点 
            xe203.InnerText = model.bdr;
            XmlElement xe204 = xmldoc.CreateElement("bdlx");//创建一个<Node>节点 
            xe204.InnerText = model.bdlx;
            XmlElement xe205 = xmldoc.CreateElement("bdnr");//创建一个<Node>节点 
            #region 加载滑行标定内容
            XmlElement xe20501 = xmldoc.CreateElement("hxqj1");//创建一个<Node>节点 
            xe20501.InnerText = model.hxqj1;
            XmlElement xe20502 = xmldoc.CreateElement("hxqj2");//创建一个<Node>节点 
            xe20502.InnerText = model.hxqj2;
            XmlElement xe20503 = xmldoc.CreateElement("hxqj3");//创建一个<Node>节点 
            xe20503.InnerText = model.hxqj3;
            XmlElement xe20504 = xmldoc.CreateElement("hxqj4");//创建一个<Node>节点 
            xe20504.InnerText = model.hxqj4;
            XmlElement xe20505 = xmldoc.CreateElement("jzgl1");//创建一个<Node>节点 
            xe20505.InnerText = model.jzgl1;
            XmlElement xe20506 = xmldoc.CreateElement("jzgl2");//创建一个<Node>节点 
            xe20506.InnerText = model.jzgl2;
            XmlElement xe20507 = xmldoc.CreateElement("jzgl3");//创建一个<Node>节点 
            xe20507.InnerText = model.jzgl3;
            XmlElement xe20508 = xmldoc.CreateElement("jzgl4");//创建一个<Node>节点 
            xe20508.InnerText = model.jzgl4;
            XmlElement xe20509 = xmldoc.CreateElement("jsgl1");//创建一个<Node>节点 
            xe20509.InnerText = model.jsgl1;
            XmlElement xe20510 = xmldoc.CreateElement("jsgl2");//创建一个<Node>节点 
            xe20510.InnerText = model.jsgl2;
            XmlElement xe20511 = xmldoc.CreateElement("jsgl3");//创建一个<Node>节点 
            xe20511.InnerText = model.jsgl3;
            XmlElement xe20512 = xmldoc.CreateElement("jsgl4");//创建一个<Node>节点 
            xe20512.InnerText = model.jsgl4;
            XmlElement xe20513 = xmldoc.CreateElement("hxsj1");//创建一个<Node>节点 
            xe20513.InnerText = model.hxsj1;
            XmlElement xe20514 = xmldoc.CreateElement("hxsj2");//创建一个<Node>节点 
            xe20514.InnerText = model.hxsj2;
            XmlElement xe20515 = xmldoc.CreateElement("hxsj3");//创建一个<Node>节点 
            xe20515.InnerText = model.hxsj3;
            XmlElement xe20516 = xmldoc.CreateElement("hxsj4");//创建一个<Node>节点 
            xe20516.InnerText = model.hxsj4;
            XmlElement xe20517 = xmldoc.CreateElement("llsj1");//创建一个<Node>节点 
            xe20517.InnerText = model.llsj1;
            XmlElement xe20518 = xmldoc.CreateElement("llsj2");//创建一个<Node>节点 
            xe20518.InnerText = model.llsj2;
            XmlElement xe20519 = xmldoc.CreateElement("llsj3");//创建一个<Node>节点 
            xe20519.InnerText = model.llsj3;
            XmlElement xe20520 = xmldoc.CreateElement("llsj4");//创建一个<Node>节点 
            xe20520.InnerText = model.llsj4;
            XmlElement xe20521 = xmldoc.CreateElement("wc1");//创建一个<Node>节点 
            xe20521.InnerText = model.wc1;
            XmlElement xe20522 = xmldoc.CreateElement("wc2");//创建一个<Node>节点 
            xe20522.InnerText = model.wc2;
            XmlElement xe20523 = xmldoc.CreateElement("wc3");//创建一个<Node>节点 
            xe20523.InnerText = model.wc3;
            XmlElement xe20524 = xmldoc.CreateElement("wc4");//创建一个<Node>节点 
            xe20524.InnerText = model.wc4;
            XmlElement xe20525 = xmldoc.CreateElement("jbgl");//创建一个<Node>节点 
            xe20525.InnerText = model.jbgl;
            XmlElement xe20526 = xmldoc.CreateElement("hxqj1jcjg");//创建一个<Node>节点 
            xe20526.InnerText = model.hxqj1jcjg;
            XmlElement xe20527 = xmldoc.CreateElement("hxqj2jcjg");//创建一个<Node>节点 
            xe20527.InnerText = model.hxqj2jcjg;
            XmlElement xe20528 = xmldoc.CreateElement("hxqj3jcjg");//创建一个<Node>节点 
            xe20528.InnerText = model.hxqj3jcjg;
            XmlElement xe20529 = xmldoc.CreateElement("hxqj4jcjg");//创建一个<Node>节点 
            xe20529.InnerText = model.hxqj4jcjg;
            xe205.AppendChild(xe20501);
            xe205.AppendChild(xe20502);
            xe205.AppendChild(xe20503);
            xe205.AppendChild(xe20504);
            xe205.AppendChild(xe20505);
            xe205.AppendChild(xe20506);
            xe205.AppendChild(xe20507);
            xe205.AppendChild(xe20508);
            xe205.AppendChild(xe20509);
            xe205.AppendChild(xe20510);
            xe205.AppendChild(xe20511);
            xe205.AppendChild(xe20512);
            xe205.AppendChild(xe20513);
            xe205.AppendChild(xe20514);
            xe205.AppendChild(xe20515);
            xe205.AppendChild(xe20516);
            xe205.AppendChild(xe20517);
            xe205.AppendChild(xe20518);
            xe205.AppendChild(xe20519);
            xe205.AppendChild(xe20520);
            xe205.AppendChild(xe20521);
            xe205.AppendChild(xe20522);
            xe205.AppendChild(xe20523);
            xe205.AppendChild(xe20524);
            xe205.AppendChild(xe20525);
            xe205.AppendChild(xe20526);
            xe205.AppendChild(xe20527);
            xe205.AppendChild(xe20528);
            xe205.AppendChild(xe20529);
            #endregion
            XmlElement xe206 = xmldoc.CreateElement("bdjg");//创建一个<Node>节点 
            xe206.InnerText = model.bdjg;
            XmlElement xe207 = xmldoc.CreateElement("jczbh");//创建一个<Node>节点 
            xe207.InnerText = model.jczbh;
            XmlElement xe208 = xmldoc.CreateElement("jcgwh");//创建一个<Node>节点 
            xe208.InnerText = model.jcgwh;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe2.AppendChild(xe208);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }
        public void writeFqyBD_SD(FqBD model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "BD000";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
            xe201.InnerText = model.sbbh;
            XmlElement xe202 = xmldoc.CreateElement("bdsj");//创建一个<Node>节点 
            xe202.InnerText = model.bdsj;
            XmlElement xe203 = xmldoc.CreateElement("bdr");//创建一个<Node>节点 
            xe203.InnerText = model.bdr;
            XmlElement xe204 = xmldoc.CreateElement("bdlx");//创建一个<Node>节点 
            xe204.InnerText = model.bdlx;
            XmlElement xe205 = xmldoc.CreateElement("bdnr");//创建一个<Node>节点 
            #region 废气仪标定内容
            XmlElement xe20501 = xmldoc.CreateElement("lx");//创建一个<Node>节点 
            xe20501.InnerText = model.lx;
            XmlElement xe20502 = xmldoc.CreateElement("bzc3h8");//创建一个<Node>节点 
            xe20502.InnerText = model.bzc3h8;
            XmlElement xe20503 = xmldoc.CreateElement("kssj");//创建一个<Node>节点 
            xe20503.InnerText = model.kssj;
            XmlElement xe20504 = xmldoc.CreateElement("jssj");//创建一个<Node>节点 
            xe20504.InnerText = model.jssj;
            XmlElement xe20505 = xmldoc.CreateElement("sdzhc1");//创建一个<Node>节点 
            xe20505.InnerText = model.sdzhc1;
            XmlElement xe20506 = xmldoc.CreateElement("sczhc1");//创建一个<Node>节点 
            xe20506.InnerText = model.sczhc1;
            XmlElement xe20507 = xmldoc.CreateElement("jdwczhc1");//创建一个<Node>节点 
            xe20507.InnerText = model.jdwczhc1;
            XmlElement xe20508 = xmldoc.CreateElement("xdwczhc1");//创建一个<Node>节点 
            xe20508.InnerText = model.xdwczhc1;
            XmlElement xe20509 = xmldoc.CreateElement("sdzhc2");//创建一个<Node>节点 
            xe20509.InnerText = model.sdzhc2;
            XmlElement xe20510 = xmldoc.CreateElement("sczhc2");//创建一个<Node>节点 
            xe20510.InnerText = model.sczhc2;
            XmlElement xe20511 = xmldoc.CreateElement("jdwczhc2");//创建一个<Node>节点 
            xe20511.InnerText = model.jdwczhc2;
            XmlElement xe20512 = xmldoc.CreateElement("xdwczhc2");//创建一个<Node>节点 
            xe20512.InnerText = model.xdwczhc2;
            XmlElement xe20513 = xmldoc.CreateElement("sdzco1");//创建一个<Node>节点 
            xe20513.InnerText = model.sdzco1;
            XmlElement xe20514 = xmldoc.CreateElement("sczco1");//创建一个<Node>节点 
            xe20514.InnerText = model.sczco1;
            XmlElement xe20515 = xmldoc.CreateElement("jdwczco1");//创建一个<Node>节点 
            xe20515.InnerText = model.jdwczco1;
            XmlElement xe20516 = xmldoc.CreateElement("xdwczco1");//创建一个<Node>节点 
            xe20516.InnerText = model.xdwczco1;
            XmlElement xe20517 = xmldoc.CreateElement("sdzco2");//创建一个<Node>节点 
            xe20517.InnerText = model.sdzco2;
            XmlElement xe20518 = xmldoc.CreateElement("sczco2");//创建一个<Node>节点 
            xe20518.InnerText = model.sczco2;
            XmlElement xe20519 = xmldoc.CreateElement("jdwczco2");//创建一个<Node>节点 
            xe20519.InnerText = model.jdwczco2;
            XmlElement xe20520 = xmldoc.CreateElement("xdwczco2");//创建一个<Node>节点 
            xe20520.InnerText = model.xdwczco2;
            XmlElement xe20521 = xmldoc.CreateElement("sdzco21");//创建一个<Node>节点 
            xe20521.InnerText = model.sdzco21;
            XmlElement xe20522 = xmldoc.CreateElement("sczco21");//创建一个<Node>节点 
            xe20522.InnerText = model.sczco21;
            XmlElement xe20523 = xmldoc.CreateElement("jdwczco21");//创建一个<Node>节点 
            xe20523.InnerText = model.jdwczco21;
            XmlElement xe20524 = xmldoc.CreateElement("xdwczco21");//创建一个<Node>节点 
            xe20524.InnerText = model.xdwczco21;
            XmlElement xe20525 = xmldoc.CreateElement("sdzco22");//创建一个<Node>节点 
            xe20525.InnerText = model.sdzco22;
            XmlElement xe20526 = xmldoc.CreateElement("sczco22");//创建一个<Node>节点 
            xe20526.InnerText = model.sczco22;
            XmlElement xe20527 = xmldoc.CreateElement("jdwczco22");//创建一个<Node>节点 
            xe20527.InnerText = model.jdwczco22;
            XmlElement xe20528 = xmldoc.CreateElement("xdwczco22");//创建一个<Node>节点 
            xe20528.InnerText = model.xdwczco22;
            XmlElement xe20529 = xmldoc.CreateElement("sdzno1");//创建一个<Node>节点 
            xe20529.InnerText = model.sdzno1;
            XmlElement xe20530 = xmldoc.CreateElement("sczno1");//创建一个<Node>节点 
            xe20530.InnerText = model.sczno1;
            XmlElement xe20531 = xmldoc.CreateElement("jdwczno1");//创建一个<Node>节点 
            xe20531.InnerText = model.jdwczno1;
            XmlElement xe20532 = xmldoc.CreateElement("xdwczno1");//创建一个<Node>节点 
            xe20532.InnerText = model.xdwczno1;
            XmlElement xe20533 = xmldoc.CreateElement("sdzno2");//创建一个<Node>节点 
            xe20533.InnerText = model.sdzno2;
            XmlElement xe20534 = xmldoc.CreateElement("sczno2");//创建一个<Node>节点 
            xe20534.InnerText = model.sczno2;
            XmlElement xe20535 = xmldoc.CreateElement("jdwczno2");//创建一个<Node>节点 
            xe20535.InnerText = model.jdwczno2;
            XmlElement xe20536 = xmldoc.CreateElement("xdwczno2");//创建一个<Node>节点 
            xe20536.InnerText = model.xdwczno2;
            XmlElement xe20537 = xmldoc.CreateElement("pef");//创建一个<Node>节点 
            xe20537.InnerText = model.pef;
            XmlElement xe20538 = xmldoc.CreateElement("jcjg");//创建一个<Node>节点 
            xe20538.InnerText = model.jcjg;
            xe205.AppendChild(xe20501);
            xe205.AppendChild(xe20502);
            xe205.AppendChild(xe20503);
            xe205.AppendChild(xe20504);
            xe205.AppendChild(xe20505);
            xe205.AppendChild(xe20506);
            xe205.AppendChild(xe20507);
            xe205.AppendChild(xe20508);
            xe205.AppendChild(xe20509);
            xe205.AppendChild(xe20510);
            xe205.AppendChild(xe20511);
            xe205.AppendChild(xe20512);
            xe205.AppendChild(xe20513);
            xe205.AppendChild(xe20514);
            xe205.AppendChild(xe20515);
            xe205.AppendChild(xe20516);
            xe205.AppendChild(xe20517);
            xe205.AppendChild(xe20518);
            xe205.AppendChild(xe20519);
            xe205.AppendChild(xe20520);
            xe205.AppendChild(xe20521);
            xe205.AppendChild(xe20522);
            xe205.AppendChild(xe20523);
            xe205.AppendChild(xe20524);
            xe205.AppendChild(xe20525);
            xe205.AppendChild(xe20526);
            xe205.AppendChild(xe20527);
            xe205.AppendChild(xe20528);
            xe205.AppendChild(xe20529);
            xe205.AppendChild(xe20530);
            xe205.AppendChild(xe20531);
            xe205.AppendChild(xe20532);
            xe205.AppendChild(xe20533);
            xe205.AppendChild(xe20534);
            xe205.AppendChild(xe20535);
            xe205.AppendChild(xe20536);
            xe205.AppendChild(xe20537);
            xe205.AppendChild(xe20538);
            #endregion
            XmlElement xe206 = xmldoc.CreateElement("bdjg");//创建一个<Node>节点 
            xe206.InnerText = model.bdjg;
            XmlElement xe207 = xmldoc.CreateElement("jczbh");//创建一个<Node>节点 
            xe207.InnerText = model.jczbh;
            XmlElement xe208 = xmldoc.CreateElement("jcgwh");//创建一个<Node>节点 
            xe208.InnerText = model.jcgwh;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe2.AppendChild(xe208);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }
        public void writeYdjBD_SD(YdBD model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "BD000";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
            xe201.InnerText = model.sbbh;
            XmlElement xe202 = xmldoc.CreateElement("bdsj");//创建一个<Node>节点 
            xe202.InnerText = model.bdsj;
            XmlElement xe203 = xmldoc.CreateElement("bdr");//创建一个<Node>节点 
            xe203.InnerText = model.bdr;
            XmlElement xe204 = xmldoc.CreateElement("bdlx");//创建一个<Node>节点 
            xe204.InnerText = model.bdlx;
            XmlElement xe205 = xmldoc.CreateElement("bdnr");//创建一个<Node>节点 
            #region 烟度计标定内容
            XmlElement xe20501 = xmldoc.CreateElement("btgydsdz1");//创建一个<Node>节点 
            xe20501.InnerText = model.btgydsdz1;
            XmlElement xe20502 = xmldoc.CreateElement("btgydscz1");//创建一个<Node>节点 
            xe20502.InnerText = model.btgydscz1;
            XmlElement xe20503 = xmldoc.CreateElement("btgydwcz1");//创建一个<Node>节点 
            xe20503.InnerText = model.btgydwcz1;
            XmlElement xe20504 = xmldoc.CreateElement("btgydsdz2");//创建一个<Node>节点 
            xe20504.InnerText = model.btgydsdz2;
            XmlElement xe20505 = xmldoc.CreateElement("btgydscz2");//创建一个<Node>节点 
            xe20505.InnerText = model.btgydscz2;
            XmlElement xe20506 = xmldoc.CreateElement("btgydwcz2");//创建一个<Node>节点 
            xe20506.InnerText = model.btgydwcz2;
            XmlElement xe20507 = xmldoc.CreateElement("btgydsdz3");//创建一个<Node>节点 
            xe20507.InnerText = model.btgydsdz3;
            XmlElement xe20508 = xmldoc.CreateElement("btgydscz3");//创建一个<Node>节点 
            xe20508.InnerText = model.btgydscz3;
            XmlElement xe20509 = xmldoc.CreateElement("btgydwcz3");//创建一个<Node>节点 
            xe20509.InnerText = model.btgydwcz3;
            xe205.AppendChild(xe20501);
            xe205.AppendChild(xe20502);
            xe205.AppendChild(xe20503);
            xe205.AppendChild(xe20504);
            xe205.AppendChild(xe20505);
            xe205.AppendChild(xe20506);
            xe205.AppendChild(xe20507);
            xe205.AppendChild(xe20508);
            xe205.AppendChild(xe20509);
            #endregion
            XmlElement xe206 = xmldoc.CreateElement("bdjg");//创建一个<Node>节点 
            xe206.InnerText = model.bdjg;
            XmlElement xe207 = xmldoc.CreateElement("jczbh");//创建一个<Node>节点 
            xe207.InnerText = model.jczbh;
            XmlElement xe208 = xmldoc.CreateElement("jcgwh");//创建一个<Node>节点 
            xe208.InnerText = model.jcgwh;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe2.AppendChild(xe208);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }
        public void writeYlcBD_SD(YlcCheck model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "BD000";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
            xe201.InnerText = model.sbbh;
            XmlElement xe202 = xmldoc.CreateElement("bdsj");//创建一个<Node>节点 
            xe202.InnerText = model.bdsj;
            XmlElement xe203 = xmldoc.CreateElement("bdr");//创建一个<Node>节点 
            xe203.InnerText = model.bdr;
            XmlElement xe204 = xmldoc.CreateElement("bdlx");//创建一个<Node>节点 
            xe204.InnerText = model.bdlx;
            XmlElement xe205 = xmldoc.CreateElement("bdnr");//创建一个<Node>节点 
            #region 分析仪氧量程标定内容
            XmlElement xe20501 = xmldoc.CreateElement("kssj");//创建一个<Node>节点 
            xe20501.InnerText = model.kssj;
            XmlElement xe20502 = xmldoc.CreateElement("o2lcbz");//创建一个<Node>节点 
            xe20502.InnerText = model.o2lcbz;
            XmlElement xe20503 = xmldoc.CreateElement("o2lcclz");//创建一个<Node>节点 
            xe20503.InnerText = model.o2lcclz;
            XmlElement xe20504 = xmldoc.CreateElement("o2lcwc");//创建一个<Node>节点 
            xe20504.InnerText = model.o2lcwc;
            XmlElement xe20505 = xmldoc.CreateElement("jcjg");//创建一个<Node>节点 
            xe20505.InnerText = model.jcjg;
            xe205.AppendChild(xe20501);
            xe205.AppendChild(xe20502);
            xe205.AppendChild(xe20503);
            xe205.AppendChild(xe20504);
            xe205.AppendChild(xe20505);
            #endregion
            XmlElement xe206 = xmldoc.CreateElement("bdjg");//创建一个<Node>节点 
            xe206.InnerText = model.bdjg;
            XmlElement xe207 = xmldoc.CreateElement("jczbh");//创建一个<Node>节点 
            xe207.InnerText = model.jczbh;
            XmlElement xe208 = xmldoc.CreateElement("jcgwh");//创建一个<Node>节点 
            xe208.InnerText = model.jcgwh;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe2.AppendChild(xe208);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }
        public void writeLljBD_SD(LljCheck model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "BD000";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
            xe201.InnerText = model.sbbh;
            XmlElement xe202 = xmldoc.CreateElement("bdsj");//创建一个<Node>节点 
            xe202.InnerText = model.bdsj;
            XmlElement xe203 = xmldoc.CreateElement("bdr");//创建一个<Node>节点 
            xe203.InnerText = model.bdr;
            XmlElement xe204 = xmldoc.CreateElement("bdlx");//创建一个<Node>节点 
            xe204.InnerText = model.bdlx;
            XmlElement xe205 = xmldoc.CreateElement("bdnr");//创建一个<Node>节点 
            #region 流量计标定内容
            XmlElement xe20501 = xmldoc.CreateElement("kssj");//创建一个<Node>节点 
            xe20501.InnerText = model.kssj;
            XmlElement xe20502 = xmldoc.CreateElement("glcbz");//创建一个<Node>节点 
            xe20502.InnerText = model.glcbz;
            XmlElement xe20503 = xmldoc.CreateElement("glcclz");//创建一个<Node>节点 
            xe20503.InnerText = model.glcclz;
            XmlElement xe20504 = xmldoc.CreateElement("glcwc");//创建一个<Node>节点 
            xe20504.InnerText = model.glcwc;
            XmlElement xe20505 = xmldoc.CreateElement("dlcbz");//创建一个<Node>节点 
            xe20505.InnerText = model.dlcbz;
            XmlElement xe20506 = xmldoc.CreateElement("dlcclz");//创建一个<Node>节点 
            xe20506.InnerText = model.dlcclz;
            XmlElement xe20507 = xmldoc.CreateElement("dlcwc");//创建一个<Node>节点 
            xe20507.InnerText = model.dlcwc;
            XmlElement xe20508 = xmldoc.CreateElement("jcjg");//创建一个<Node>节点 
            xe20508.InnerText = model.jcjg;
            xe205.AppendChild(xe20501);
            xe205.AppendChild(xe20502);
            xe205.AppendChild(xe20503);
            xe205.AppendChild(xe20504);
            xe205.AppendChild(xe20505);
            xe205.AppendChild(xe20506);
            xe205.AppendChild(xe20507);
            xe205.AppendChild(xe20508);
            #endregion
            XmlElement xe206 = xmldoc.CreateElement("bdjg");//创建一个<Node>节点 
            xe206.InnerText = model.bdjg;
            XmlElement xe207 = xmldoc.CreateElement("jczbh");//创建一个<Node>节点 
            xe207.InnerText = model.jczbh;
            XmlElement xe208 = xmldoc.CreateElement("jcgwh");//创建一个<Node>节点 
            xe208.InnerText = model.jcgwh;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe2.AppendChild(xe208);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }
        public void writeGlBD_SD(GlBD model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "BD000";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
            xe201.InnerText = model.sbbh;
            XmlElement xe202 = xmldoc.CreateElement("bdsj");//创建一个<Node>节点 
            xe202.InnerText = model.bdsj;
            XmlElement xe203 = xmldoc.CreateElement("bdr");//创建一个<Node>节点 
            xe203.InnerText = model.bdr;
            XmlElement xe204 = xmldoc.CreateElement("bdlx");//创建一个<Node>节点 
            xe204.InnerText = model.bdlx;
            XmlElement xe205 = xmldoc.CreateElement("bdnr");//创建一个<Node>节点 
            #region 惯量标定内容
            XmlElement xe20501 = xmldoc.CreateElement("mpbsz");//创建一个<Node>节点 
            xe20501.InnerText = model.mpbsz;
            XmlElement xe20502 = xmldoc.CreateElement("sjclz");//创建一个<Node>节点 
            xe20502.InnerText = model.sjclz;
            XmlElement xe20503 = xmldoc.CreateElement("glwc");//创建一个<Node>节点 
            xe20503.InnerText = model.glwc;
            XmlElement xe20504 = xmldoc.CreateElement("jcjg");//创建一个<Node>节点 
            xe20504.InnerText = model.jcjg;
            xe205.AppendChild(xe20501);
            xe205.AppendChild(xe20502);
            xe205.AppendChild(xe20503);
            xe205.AppendChild(xe20504);
            #endregion
            XmlElement xe206 = xmldoc.CreateElement("bdjg");//创建一个<Node>节点 
            xe206.InnerText = model.bdjg;
            XmlElement xe207 = xmldoc.CreateElement("jczbh");//创建一个<Node>节点 
            xe207.InnerText = model.jczbh;
            XmlElement xe208 = xmldoc.CreateElement("jcgwh");//创建一个<Node>节点 
            xe208.InnerText = model.jcgwh;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe2.AppendChild(xe208);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }
        public void writeXysjBD_SD(XysjBD model, out string code, out string message)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
            XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
            XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

            XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
            xe101.InnerText = Organ;
            XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
            xe102.InnerText = Jkxlh;
            XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
            xe103.InnerText = "BD000";
            xe1.AppendChild(xe101);
            xe1.AppendChild(xe102);
            xe1.AppendChild(xe103);

            XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
            xe201.InnerText = model.sbbh;
            XmlElement xe202 = xmldoc.CreateElement("bdsj");//创建一个<Node>节点 
            xe202.InnerText = model.bdsj;
            XmlElement xe203 = xmldoc.CreateElement("bdr");//创建一个<Node>节点 
            xe203.InnerText = model.bdr;
            XmlElement xe204 = xmldoc.CreateElement("bdlx");//创建一个<Node>节点 
            xe204.InnerText = model.bdlx;
            XmlElement xe205 = xmldoc.CreateElement("bdnr");//创建一个<Node>节点 
            #region 响应时间标定内容
            XmlElement xe20501 = xmldoc.CreateElement("xysj");//创建一个<Node>节点 
            xe20501.InnerText = model.xysj;
            XmlElement xe20502 = xmldoc.CreateElement("wdsj");//创建一个<Node>节点 
            xe20502.InnerText = model.wdsj;
            XmlElement xe20503 = xmldoc.CreateElement("jzl1");//创建一个<Node>节点 
            xe20503.InnerText = model.jzl1;
            XmlElement xe20504 = xmldoc.CreateElement("jzl2");//创建一个<Node>节点 
            xe20504.InnerText = model.jzl2;
            XmlElement xe20505 = xmldoc.CreateElement("qhsdd");//创建一个<Node>节点 
            xe20505.InnerText = model.qhsdd;
            XmlElement xe20506 = xmldoc.CreateElement("jcjg");//创建一个<Node>节点 
            xe20506.InnerText = model.jcjg;
            xe205.AppendChild(xe20501);
            xe205.AppendChild(xe20502);
            xe205.AppendChild(xe20503);
            xe205.AppendChild(xe20504);
            xe205.AppendChild(xe20505);
            xe205.AppendChild(xe20506);
            #endregion
            XmlElement xe206 = xmldoc.CreateElement("bdjg");//创建一个<Node>节点 
            xe206.InnerText = model.bdjg;
            XmlElement xe207 = xmldoc.CreateElement("jczbh");//创建一个<Node>节点 
            xe207.InnerText = model.jczbh;
            XmlElement xe208 = xmldoc.CreateElement("jcgwh");//创建一个<Node>节点 
            xe208.InnerText = model.jcgwh;
            xe2.AppendChild(xe201);
            xe2.AppendChild(xe202);
            xe2.AppendChild(xe203);
            xe2.AppendChild(xe204);
            xe2.AppendChild(xe205);
            xe2.AppendChild(xe206);
            xe2.AppendChild(xe207);
            xe2.AppendChild(xe208);
            xe20.AppendChild(xe2);

            root.AppendChild(xe1);
            root.AppendChild(xe20);
            
            string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
            FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

            ReadACKString(receiveXml, out code, out message);
        }
        #endregion

        #region 辽宁联网
        /// <summary>
        /// DL003查询车辆信息
        /// </summary>
        /// <param name="JYLSH"></param>
        /// <param name="TESTTIMES"></param>
        /// <param name="error_info"></param>
        /// <returns></returns>
        public DataTable GetCarInfoLN(string JYLSH, string TESTTIMES, out string error_info)
        {
            error_info = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节

                XmlElement xe11 = xmldoc.CreateElement("organ");
                xe11.InnerText = Organ;
                XmlElement xe12 = xmldoc.CreateElement("jkxlh");
                xe12.InnerText = Jkxlh;
                XmlElement xe13 = xmldoc.CreateElement("jkid");
                xe13.InnerText = "DL003";
                xe1.AppendChild(xe11);
                xe1.AppendChild(xe12);
                xe1.AppendChild(xe13);

                XmlElement xe21 = xmldoc.CreateElement("jylsh");
                xe21.InnerText = JYLSH;
                XmlElement xe22 = xmldoc.CreateElement("testtimes");
                xe22.InnerText = TESTTIMES;
                xe2.AppendChild(xe21);
                xe2.AppendChild(xe22);

                root.AppendChild(xe1);
                root.AppendChild(xe2);

                FileOpreate.SaveLog("DL003", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.query(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                return ReadSeachDatatable(receiveXml, out error_info);
            }
            catch (Exception er)
            {
                error_info = er.Message;
                FileOpreate.SaveLog("流水号" + JYLSH + "|检验次数" + TESTTIMES + "获取车辆信息失败，原因：" + er.Message, "车辆信息", 3);
                return null;
            }
        }

        /// <summary>
        /// DL002查询车辆信息
        /// </summary>
        /// <param name="JYLSH"></param>
        /// <param name="TESTTIMES"></param>
        /// <param name="error_info"></param>
        /// <returns></returns>
        public DataTable GetCarInfoByLshLN(string JYLSH, string TESTTIMES, out string error_info)
        {
            error_info = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节

                XmlElement xe11 = xmldoc.CreateElement("organ");
                xe11.InnerText = Organ;
                XmlElement xe12 = xmldoc.CreateElement("jkxlh");
                xe12.InnerText = Jkxlh;
                XmlElement xe13 = xmldoc.CreateElement("jkid");
                xe13.InnerText = "DL002";
                xe1.AppendChild(xe11);
                xe1.AppendChild(xe12);
                xe1.AppendChild(xe13);

                XmlElement xe21 = xmldoc.CreateElement("jylsh");
                xe21.InnerText = JYLSH;
                XmlElement xe22 = xmldoc.CreateElement("testtimes");
                xe22.InnerText = TESTTIMES;
                xe2.AppendChild(xe21);
                xe2.AppendChild(xe22);

                root.AppendChild(xe1);
                root.AppendChild(xe2);

                FileOpreate.SaveLog("DL002", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.query(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                return ReadSeachDatatable(receiveXml, out error_info);
            }
            catch (Exception er)
            {
                error_info = er.Message;
                FileOpreate.SaveLog("执行流水号" + JYLSH + "|检验次数" + TESTTIMES + "获取车辆信息失败，原因：" + er.Message, "车辆信息", 3);
                return null;
            }
        }

        /// <summary>
        /// KS001项目开始
        /// </summary>
        /// <param name="model"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public void writeProjectStartLN(ProjectStart model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "KS001";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
                xe201.InnerText = model.jylsh;
                XmlElement xe202 = xmldoc.CreateElement("testtimes");//创建一个<Node>节点 
                xe202.InnerText = model.jycs;
                XmlElement xe203 = xmldoc.CreateElement("tsno");//创建一个<Node>节点 
                xe203.InnerText = model.jczbh;
                XmlElement xe204 = xmldoc.CreateElement("testlineno");//创建一个<Node>节点 
                xe204.InnerText = model.jcgwh;
                XmlElement xe205 = xmldoc.CreateElement("license");//创建一个<Node>节点 
                xe205.InnerText = DataChange.encodeUTF8(model.hphm);
                XmlElement xe206 = xmldoc.CreateElement("licensecode");//创建一个<Node>节点 
                xe206.InnerText = model.hpzl;
                XmlElement xe207 = xmldoc.CreateElement("jcjsy");//创建一个<Node>节点 
                xe207.InnerText = DataChange.encodeUTF8(model.ycy);
                XmlElement xe208 = xmldoc.CreateElement("jcczy");//创建一个<Node>节点 
                xe208.InnerText = DataChange.encodeUTF8(model.jcczy);
                XmlElement xe209 = xmldoc.CreateElement("testtype");//创建一个<Node>节点 
                xe209.InnerText = model.jcff;
                XmlElement xe210 = xmldoc.CreateElement("odometer");//创建一个<Node>节点 
                xe210.InnerText = model.ljxslc;
                XmlElement xe211 = xmldoc.CreateElement("dqsj");//创建一个<Node>节点 
                xe211.InnerText = DateTime.Parse(model.dqsj).ToString("yyyy-MM-dd HH:mm:ss");
                XmlElement xe212 = xmldoc.CreateElement("jcbbh");//创建一个<Node>节点 
                xe212.InnerText = model.jcbbh;
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe2.AppendChild(xe210);
                xe2.AppendChild(xe211);
                xe2.AppendChild(xe212);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("KS001", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行发送项目开始失败，原因：" + er.Message, "项目开始", 3);
            }
        }

        /// <summary>
        /// JS001项目结束
        /// </summary>
        /// <param name="model"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public void writeProjectStopLN(ProjectStop model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "JS001";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
                xe201.InnerText = model.jylsh;
                XmlElement xe202 = xmldoc.CreateElement("testtimes");//创建一个<Node>节点 
                xe202.InnerText = model.jycs;
                XmlElement xe203 = xmldoc.CreateElement("tsno");//创建一个<Node>节点 
                xe203.InnerText = model.jczbh;
                XmlElement xe204 = xmldoc.CreateElement("testlineno");//创建一个<Node>节点 
                xe204.InnerText = model.jcgwh;
                XmlElement xe205 = xmldoc.CreateElement("license");//创建一个<Node>节点 
                xe205.InnerText = DataChange.encodeUTF8(model.hphm);
                XmlElement xe206 = xmldoc.CreateElement("licensecode");//创建一个<Node>节点 
                xe206.InnerText = model.hpzl;
                XmlElement xe207 = xmldoc.CreateElement("result");//创建一个<Node>节点 
                xe207.InnerText = model.pdjg == "合格" ? "1" : "0";
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("JS001", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行发送项目结束失败，原因：" + er.Message, "项目结束", 3);
            }
        }

        /// <summary>
        /// ZP002检测照片
        /// </summary>
        /// <param name="model"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public void writeCapturePictureLN(CapturePicture model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "ZP002";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
                xe201.InnerText = model.jylsh;
                XmlElement xe202 = xmldoc.CreateElement("testtimes");//创建一个<Node>节点 
                xe202.InnerText = model.jycs;
                XmlElement xe203 = xmldoc.CreateElement("tsno");//创建一个<Node>节点 
                xe203.InnerText = model.jczbh;
                XmlElement xe204 = xmldoc.CreateElement("testlineno");//创建一个<Node>节点 
                xe204.InnerText = model.jcgwh;
                XmlElement xe205 = xmldoc.CreateElement("zpbh");//创建一个<Node>节点 
                xe205.InnerText = model.zpbh;
                XmlElement xe206 = xmldoc.CreateElement("photo_date");//创建一个<Node>节点 
                xe206.InnerText = model.photo_date;
                xe20.AppendChild(xe201);
                xe20.AppendChild(xe202);
                xe20.AppendChild(xe203);
                xe20.AppendChild(xe204);
                xe20.AppendChild(xe205);
                xe20.AppendChild(xe206);
                xe2.AppendChild(xe20);

                root.AppendChild(xe1);
                root.AppendChild(xe2);

                FileOpreate.SaveLog("ZP002", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);
                if (receiveXml == "Failed")
                    return;
                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行发送检测照片失败，原因：" + er.Message, "检测照片", 3);
            }
        }

        //瞬态结果数据
        public void writeVmasResultLN(VmasResult model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "JC001";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
                xe201.InnerText = model.jylsh;
                XmlElement xe202 = xmldoc.CreateElement("testtimes");//创建一个<Node>节点 
                xe202.InnerText = model.jycs;
                XmlElement xe203 = xmldoc.CreateElement("wd");//创建一个<Node>节点 
                xe203.InnerText = model.wd;
                XmlElement xe204 = xmldoc.CreateElement("dqy");//创建一个<Node>节点 
                xe204.InnerText = model.dqy;
                XmlElement xe205 = xmldoc.CreateElement("xdsd");//创建一个<Node>节点 
                xe205.InnerText = model.xdsd;
                XmlElement xe206 = xmldoc.CreateElement("colimit");//创建一个<Node>节点 
                xe206.InnerText = model.coxz;
                XmlElement xe207 = xmldoc.CreateElement("co");//创建一个<Node>节点 
                xe207.InnerText = model.coclz;
                XmlElement xe208 = xmldoc.CreateElement("cojudge");//创建一个<Node>节点 
                xe208.InnerText = model.copdjg == "合格" ? "1" : "0";
                XmlElement xe209 = xmldoc.CreateElement("hclimit");//创建一个<Node>节点 
                xe209.InnerText = model.hcxz;
                XmlElement xe210 = xmldoc.CreateElement("hc");//创建一个<Node>节点 
                xe210.InnerText = model.hcclz;
                XmlElement xe211 = xmldoc.CreateElement("hcjudge");//创建一个<Node>节点 
                xe211.InnerText = model.hcpdjg == "合格" ? "1" : "0";
                XmlElement xe212 = xmldoc.CreateElement("noxlimit");//创建一个<Node>节点 
                xe212.InnerText = model.noxz;
                XmlElement xe213 = xmldoc.CreateElement("nox");//创建一个<Node>节点 
                xe213.InnerText = model.noclz;
                XmlElement xe214 = xmldoc.CreateElement("noxjudge");//创建一个<Node>节点 
                xe214.InnerText = model.nopdjg == "合格" ? "1" : "0";
                XmlElement xe215 = xmldoc.CreateElement("hcnoxlimit");//创建一个<Node>节点 
                xe215.InnerText = model.hcnoxz;
                XmlElement xe216 = xmldoc.CreateElement("hcnox");//创建一个<Node>节点 
                xe216.InnerText = model.hcnoclz;
                XmlElement xe217 = xmldoc.CreateElement("hcnoxjudge");//创建一个<Node>节点 
                xe217.InnerText = model.hcnopdjg == "合格" ? "1" : "0";
                XmlElement xe218 = xmldoc.CreateElement("csljccsj");//创建一个<Node>节点 
                xe218.InnerText = model.csljccsj;
                XmlElement xe219 = xmldoc.CreateElement("xslc");//创建一个<Node>节点 
                xe219.InnerText = model.xslc;
                XmlElement xe220 = xmldoc.CreateElement("result");//创建一个<Node>节点 
                xe220.InnerText = model.PDJG == "合格" ? "1" : "0";
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe2.AppendChild(xe210);
                xe2.AppendChild(xe211);
                xe2.AppendChild(xe212);
                xe2.AppendChild(xe213);
                xe2.AppendChild(xe214);
                xe2.AppendChild(xe215);
                xe2.AppendChild(xe216);
                xe2.AppendChild(xe217);
                xe2.AppendChild(xe218);
                xe2.AppendChild(xe219);
                xe2.AppendChild(xe220);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("JC001", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行写入Vmas结果数据失败，原因：" + er.Message, "Vmas结果数据", 3);
            }
        }

        //稳态结果数据
        public void writeASMResultLN(ASMResult model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "JC002";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
                xe201.InnerText = model.jylsh;
                XmlElement xe202 = xmldoc.CreateElement("testtimes");//创建一个<Node>节点 
                xe202.InnerText = model.jycs;
                XmlElement xe203 = xmldoc.CreateElement("wd");//创建一个<Node>节点 
                xe203.InnerText = model.wd;
                XmlElement xe204 = xmldoc.CreateElement("dqy");//创建一个<Node>节点 
                xe204.InnerText = model.dqy;
                XmlElement xe205 = xmldoc.CreateElement("xdsd");//创建一个<Node>节点 
                xe205.InnerText = model.xdsd;
                XmlElement xe206 = xmldoc.CreateElement("hc5025limit");//创建一个<Node>节点 
                xe206.InnerText = model.hc5025xz;
                XmlElement xe207 = xmldoc.CreateElement("hc5025");//创建一个<Node>节点 
                xe207.InnerText = model.hc5025clz;
                XmlElement xe208 = xmldoc.CreateElement("hc5025judge");//创建一个<Node>节点 
                xe208.InnerText = model.hc5025pdjg == "合格" ? "1" : "0";
                XmlElement xe209 = xmldoc.CreateElement("co5025limit");//创建一个<Node>节点 
                xe209.InnerText = model.co5025xz;
                XmlElement xe210 = xmldoc.CreateElement("co5025");//创建一个<Node>节点 
                xe210.InnerText = model.co5025clz;
                XmlElement xe211 = xmldoc.CreateElement("co5025judge");//创建一个<Node>节点 
                xe211.InnerText = model.co5025pdjg == "合格" ? "1" : "0";
                XmlElement xe212 = xmldoc.CreateElement("no5025limit");//创建一个<Node>节点 
                xe212.InnerText = model.no5025xz;
                XmlElement xe213 = xmldoc.CreateElement("no5025");//创建一个<Node>节点 
                xe213.InnerText = model.no5025clz;
                XmlElement xe214 = xmldoc.CreateElement("no5025judge");//创建一个<Node>节点 
                xe214.InnerText = model.no5025pdjg == "合格" ? "1" : "0";
                XmlElement xe215 = xmldoc.CreateElement("fdjzs5025");//创建一个<Node>节点 
                xe215.InnerText = model.fdjzs5025;
                XmlElement xe216 = xmldoc.CreateElement("fdjyw5025");//创建一个<Node>节点 
                xe216.InnerText = model.fdjyw5025;

                XmlElement xe217 = xmldoc.CreateElement("hc2540limit");//创建一个<Node>节点 
                XmlElement xe218 = xmldoc.CreateElement("hc2540");//创建一个<Node>节点 
                XmlElement xe219 = xmldoc.CreateElement("hc2540judge");//创建一个<Node>节点 
                XmlElement xe220 = xmldoc.CreateElement("co2540limit");//创建一个<Node>节点 
                XmlElement xe221 = xmldoc.CreateElement("co2540");//创建一个<Node>节点 
                XmlElement xe222 = xmldoc.CreateElement("co2540judge");//创建一个<Node>节点 
                XmlElement xe223 = xmldoc.CreateElement("no2540limit");//创建一个<Node>节点 
                XmlElement xe224 = xmldoc.CreateElement("no2540");//创建一个<Node>节点 
                XmlElement xe225 = xmldoc.CreateElement("no2540judge");//创建一个<Node>节点 
                XmlElement xe226 = xmldoc.CreateElement("fdjzs2540");//创建一个<Node>节点 
                XmlElement xe227 = xmldoc.CreateElement("fdjyw2540");//创建一个<Node>节点 
                XmlElement xe228 = xmldoc.CreateElement("jzzgl5025");//创建一个<Node>节点 
                XmlElement xe229 = xmldoc.CreateElement("jzzgl2540");//创建一个<Node>节点 
                if (model._stop_at_5025 == false)
                {
                    xe217.InnerText = model.hc2540xz;
                    xe218.InnerText = model.hc2540clz;
                    xe219.InnerText = model.hc2540pdjg == "合格" ? "1" : "0";
                    xe220.InnerText = model.co2540xz;
                    xe221.InnerText = model.co2540clz;
                    xe222.InnerText = model.co2540pdjg == "合格" ? "1" : "0";
                    xe223.InnerText = model.no2540xz;
                    xe224.InnerText = model.no2540clz;
                    xe225.InnerText = model.no2540pdjg == "合格" ? "1" : "0";
                    xe226.InnerText = model.fdjzs2540;
                    xe227.InnerText = model.fdjyw2540;
                    xe228.InnerText = model.jzzgl5025;
                    xe229.InnerText = model.jzzgl2540;
                }

                XmlElement xe230 = xmldoc.CreateElement("result");//创建一个<Node>节点 
                xe230.InnerText = model.PDJG == "合格" ? "1" : "0";
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe2.AppendChild(xe210);
                xe2.AppendChild(xe211);
                xe2.AppendChild(xe212);
                xe2.AppendChild(xe213);
                xe2.AppendChild(xe214);
                xe2.AppendChild(xe215);
                xe2.AppendChild(xe216);
                xe2.AppendChild(xe217);
                xe2.AppendChild(xe218);
                xe2.AppendChild(xe219);
                xe2.AppendChild(xe220);
                xe2.AppendChild(xe221);
                xe2.AppendChild(xe222);
                xe2.AppendChild(xe223);
                xe2.AppendChild(xe224);
                xe2.AppendChild(xe225);
                xe2.AppendChild(xe226);
                xe2.AppendChild(xe227);
                xe2.AppendChild(xe228);
                xe2.AppendChild(xe229);
                xe2.AppendChild(xe230);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("JC002", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行写入ASM结果数据失败，原因：" + er.Message, "ASM结果数据", 3);
            }
        }

        //双怠速结果数据
        public void writeSDSResultLN(SDSResult model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "JC003";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
                XmlElement xe202 = xmldoc.CreateElement("testtimes");//创建一个<Node>节点 
                XmlElement xe203 = xmldoc.CreateElement("wd");//创建一个<Node>节点 
                XmlElement xe204 = xmldoc.CreateElement("dqy");//创建一个<Node>节点 
                XmlElement xe205 = xmldoc.CreateElement("xdsd");//创建一个<Node>节点 
                XmlElement xe206 = xmldoc.CreateElement("lambdadown");//创建一个<Node>节点 
                XmlElement xe207 = xmldoc.CreateElement("lambdaup");//创建一个<Node>节点 
                XmlElement xe208 = xmldoc.CreateElement("lambda");//创建一个<Node>节点 
                XmlElement xe209 = xmldoc.CreateElement("lambdajudge");//创建一个<Node>节点
                XmlElement xe210 = xmldoc.CreateElement("lscolimit");//创建一个<Node>节点 
                XmlElement xe211 = xmldoc.CreateElement("lscoresult");//创建一个<Node>节点 
                XmlElement xe212 = xmldoc.CreateElement("lscojudge");//创建一个<Node>节点 
                XmlElement xe213 = xmldoc.CreateElement("lshclimit");//创建一个<Node>节点 
                XmlElement xe214 = xmldoc.CreateElement("lshcresult");//创建一个<Node>节点 
                XmlElement xe215 = xmldoc.CreateElement("lshcjudge");//创建一个<Node>节点 
                XmlElement xe216 = xmldoc.CreateElement("dszs");//创建一个<Node>节点 
                XmlElement xe217 = xmldoc.CreateElement("ddsjywd");//创建一个<Node>节点 
                XmlElement xe218 = xmldoc.CreateElement("hscolimit");//创建一个<Node>节点 
                XmlElement xe219 = xmldoc.CreateElement("hscoresult");//创建一个<Node>节点 
                XmlElement xe220 = xmldoc.CreateElement("hscojudge");//创建一个<Node>节点 
                XmlElement xe221 = xmldoc.CreateElement("hshcresult");//创建一个<Node>节点 
                XmlElement xe222 = xmldoc.CreateElement("hshclimit");//创建一个<Node>节点 
                XmlElement xe223 = xmldoc.CreateElement("hshcjudge");//创建一个<Node>节点 
                XmlElement xe224 = xmldoc.CreateElement("gdszs");//创建一个<Node>节点 
                XmlElement xe225 = xmldoc.CreateElement("gdsjywd");//创建一个<Node>节点 
                XmlElement xe226 = xmldoc.CreateElement("result");//创建一个<Node>节点 

                if (model.gdscoxz != "" && model.gdshcxz != "")//高怠速限值为空时为部分摩托车，其他正常上传
                {
                    xe201.InnerText = model.jylsh;
                    xe202.InnerText = model.jycs;
                    xe203.InnerText = model.wd;
                    xe204.InnerText = model.dqy;
                    xe205.InnerText = model.xdsd;
                    xe206.InnerText = model.glkqsxxx;
                    xe207.InnerText = model.glkqxssx;
                    xe208.InnerText = model.glkqxsz;
                    if (model.glkqxspdjg == "合格")
                        xe209.InnerText = "1";
                    else if (model.glkqxspdjg == "不合格")
                        xe209.InnerText = "0";
                    else
                        xe209.InnerText = "-";
                    xe210.InnerText = model.ddscoxz;
                    xe211.InnerText = model.ddscoz;
                    xe212.InnerText = model.ddscopdjg == "合格" ? "1" : "0";
                    xe213.InnerText = model.ddshcxz;
                    xe214.InnerText = model.ddshcz;
                    xe215.InnerText = model.ddshcpdjg == "合格" ? "1" : "0";
                    xe216.InnerText = model.fdjdszs;
                    xe217.InnerText = model.ddsjywd;
                    xe218.InnerText = model.gdscoxz;
                    xe219.InnerText = model.gdscoz;
                    if (model.gdscopdjg == "合格")
                        xe220.InnerText = "1";
                    else if (model.gdscopdjg == "不合格")
                        xe220.InnerText = "0";
                    else
                        xe220.InnerText = "-";
                    xe221.InnerText = model.gdshcz;
                    xe222.InnerText = model.gdshcxz;
                    xe223.InnerText = model.gdshcpdjg == "合格" ? "1" : "0";
                    xe224.InnerText = model.gdszs;
                    xe225.InnerText = model.gdsjywd;
                    xe226.InnerText = model.PDJG == "合格" ? "1" : "0";
                }
                else
                {
                    xe201.InnerText = model.jylsh;
                    xe202.InnerText = model.jycs;
                    xe203.InnerText = model.wd;
                    xe204.InnerText = model.dqy;
                    xe205.InnerText = model.xdsd;
                    xe206.InnerText = "/";
                    xe207.InnerText = "/";
                    xe208.InnerText = "/";
                    xe209.InnerText = "/";
                    xe210.InnerText = model.ddscoxz;
                    xe211.InnerText = model.ddscoz;
                    xe212.InnerText = model.ddscopdjg == "合格" ? "1" : "0";
                    xe213.InnerText = model.ddshcxz;
                    xe214.InnerText = model.ddshcz;
                    xe215.InnerText = model.ddshcpdjg == "合格" ? "1" : "0";
                    xe216.InnerText = model.fdjdszs;
                    xe217.InnerText = model.ddsjywd;
                    xe218.InnerText = "/";
                    xe219.InnerText = "/";
                    xe220.InnerText = "/";
                    xe221.InnerText = "/";
                    xe222.InnerText = "/";
                    xe223.InnerText = "/";
                    xe224.InnerText = "/";
                    xe225.InnerText = "/";
                    xe226.InnerText = model.PDJG == "合格" ? "1" : "0";
                }
                
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe2.AppendChild(xe210);
                xe2.AppendChild(xe211);
                xe2.AppendChild(xe212);
                xe2.AppendChild(xe213);
                xe2.AppendChild(xe214);
                xe2.AppendChild(xe215);
                xe2.AppendChild(xe216);
                xe2.AppendChild(xe217);
                xe2.AppendChild(xe218);
                xe2.AppendChild(xe219);
                xe2.AppendChild(xe220);
                xe2.AppendChild(xe221);
                xe2.AppendChild(xe222);
                xe2.AppendChild(xe223);
                xe2.AppendChild(xe224);
                xe2.AppendChild(xe225);
                xe2.AppendChild(xe226);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("JC003", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行写入SDS结果数据失败，原因：" + er.Message, "SDS结果数据", 3);
            }
        }

        //加载减速结果数据
        public void writeJZJSResultLN(JZJSResult model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "JC004";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
                xe201.InnerText = model.jylsh;
                XmlElement xe202 = xmldoc.CreateElement("testtimes");//创建一个<Node>节点 
                xe202.InnerText = model.jycs;
                XmlElement xe203 = xmldoc.CreateElement("wd");//创建一个<Node>节点 
                xe203.InnerText = model.wd;
                XmlElement xe204 = xmldoc.CreateElement("dqy");//创建一个<Node>节点 
                xe204.InnerText = model.dqy;
                XmlElement xe205 = xmldoc.CreateElement("xdsd");//创建一个<Node>节点 
                xe205.InnerText = model.xdsd;
                XmlElement xe206 = xmldoc.CreateElement("velmaxhp");//创建一个<Node>节点 
                xe206.InnerText = model.velmaxhp;
                XmlElement xe207 = xmldoc.CreateElement("rev100");//创建一个<Node>节点 
                xe207.InnerText = model.velmaxhpzs;
                XmlElement xe208 = xmldoc.CreateElement("maxpower");//创建一个<Node>节点 
                xe208.InnerText = model.zdlbgl;
                XmlElement xe209 = xmldoc.CreateElement("maxpowerlimit");//创建一个<Node>节点 
                xe209.InnerText = model.zdlbglxz;
                XmlElement xe210 = xmldoc.CreateElement("zdlbglzs");//创建一个<Node>节点 
                xe210.InnerText = model.zdlbglzs;
                XmlElement xe211 = xmldoc.CreateElement("zdlbgljudge");//创建一个<Node>节点 
                xe211.InnerText = model.zdlbglpdjg == "合格" ? "1" : "0";
                XmlElement xe212 = xmldoc.CreateElement("smokeklimit");//创建一个<Node>节点 
                xe212.InnerText = model.ydxz;
                XmlElement xe213 = xmldoc.CreateElement("ydjudge");//创建一个<Node>节点 
                xe213.InnerText = model.ydpdjg == "合格" ? "1" : "0";
                XmlElement xe214 = xmldoc.CreateElement("k100");//创建一个<Node>节点 
                xe214.InnerText = model.k100;
                XmlElement xe215 = xmldoc.CreateElement("k90");//创建一个<Node>节点 
                xe215.InnerText = model.k90;
                XmlElement xe216 = xmldoc.CreateElement("k80");//创建一个<Node>节点 
                xe216.InnerText = model.k80;
                XmlElement xe217 = xmldoc.CreateElement("raterevup");//创建一个<Node>节点 
                xe217.InnerText = model.raterevup;
                XmlElement xe218 = xmldoc.CreateElement("raterevdown");//创建一个<Node>节点 
                xe218.InnerText = model.raterevdown;
                XmlElement xe219 = xmldoc.CreateElement("result");//创建一个<Node>节点 
                xe219.InnerText = model.PDJG == "合格" ? "1" : "0";
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe2.AppendChild(xe210);
                xe2.AppendChild(xe211);
                xe2.AppendChild(xe212);
                xe2.AppendChild(xe213);
                xe2.AppendChild(xe214);
                xe2.AppendChild(xe215);
                xe2.AppendChild(xe216);
                xe2.AppendChild(xe217);
                xe2.AppendChild(xe218);
                xe2.AppendChild(xe219);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("JC004", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行写入JZJS结果数据失败，原因：" + er.Message, "JZJS结果数据", 3);
            }
        }

        //不透光结果数据
        public void writeZYJSResultLN(ZYJSResult model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "JC005";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
                xe201.InnerText = model.jylsh;
                XmlElement xe202 = xmldoc.CreateElement("testtimes");//创建一个<Node>节点 
                xe202.InnerText = model.jycs;
                XmlElement xe203 = xmldoc.CreateElement("wd");//创建一个<Node>节点 
                xe203.InnerText = model.wd;
                XmlElement xe204 = xmldoc.CreateElement("dqy");//创建一个<Node>节点 
                xe204.InnerText = model.dqy;
                XmlElement xe205 = xmldoc.CreateElement("xdsd");//创建一个<Node>节点 
                xe205.InnerText = model.xdsd;
                XmlElement xe206 = xmldoc.CreateElement("idlerev");//创建一个<Node>节点 
                xe206.InnerText = model.fdjdszs;
                XmlElement xe207 = xmldoc.CreateElement("smokek4");//创建一个<Node>节点 
                xe207.InnerText = model.smoke4;
                XmlElement xe208 = xmldoc.CreateElement("smokek3");//创建一个<Node>节点 
                xe208.InnerText = model.smoke3;
                XmlElement xe209 = xmldoc.CreateElement("smokek2");//创建一个<Node>节点 
                xe209.InnerText = model.smoke2;
                XmlElement xe210 = xmldoc.CreateElement("smokek1");//创建一个<Node>节点 
                xe210.InnerText = model.smoke1;
                XmlElement xe211 = xmldoc.CreateElement("smokeavg");//创建一个<Node>节点 
                xe211.InnerText = model.pjz;
                XmlElement xe212 = xmldoc.CreateElement("result");//创建一个<Node>节点 
                xe212.InnerText = model.pdjg == "合格" ? "1" : "0";
                XmlElement xe213 = xmldoc.CreateElement("smokeklimit");//创建一个<Node>节点 
                xe213.InnerText = model.xz;
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe2.AppendChild(xe210);
                xe2.AppendChild(xe211);
                xe2.AppendChild(xe212);
                xe2.AppendChild(xe213);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("JC005", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行写入ZYJS结果数据失败，原因：" + er.Message, "ZYJS结果数据", 3);
            }
        }

        //瞬态过程数据
        public void writeVmasProcessLN(VmasProcess model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "GC001";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
                xe201.InnerText = model.jylsh;
                XmlElement xe202 = xmldoc.CreateElement("testtimes");//创建一个<Node>节点 
                xe202.InnerText = model.jycs;
                XmlElement xe203 = xmldoc.CreateElement("cyds");//创建一个<Node>节点 
                xe203.InnerText = model.cyds;
                XmlElement xe204 = xmldoc.CreateElement("cysx");//创建一个<Node>节点 
                xe204.InnerText = model.cysx;
                XmlElement xe205 = xmldoc.CreateElement("hcclz");//创建一个<Node>节点 
                xe205.InnerText = model.hcclz;
                XmlElement xe206 = xmldoc.CreateElement("noclz");//创建一个<Node>节点 
                xe206.InnerText = model.noclz;
                XmlElement xe207 = xmldoc.CreateElement("cs");//创建一个<Node>节点 
                xe207.InnerText = model.cs;
                XmlElement xe208 = xmldoc.CreateElement("bzss");//创建一个<Node>节点 
                xe208.InnerText = model.bzss;
                XmlElement xe209 = xmldoc.CreateElement("zs");//创建一个<Node>节点 
                xe209.InnerText = model.zs;
                XmlElement xe210 = xmldoc.CreateElement("coclz");//创建一个<Node>节点 
                xe210.InnerText = model.coclz;
                XmlElement xe211 = xmldoc.CreateElement("co2clz");//创建一个<Node>节点 
                xe211.InnerText = model.co2clz;
                XmlElement xe212 = xmldoc.CreateElement("o2clz");//创建一个<Node>节点 
                xe212.InnerText = model.o2clz;
                XmlElement xe213 = xmldoc.CreateElement("xsxzxs");//创建一个<Node>节点 
                xe213.InnerText = model.xsxzxs;
                XmlElement xe214 = xmldoc.CreateElement("sdxzxs");//创建一个<Node>节点 
                xe214.InnerText = model.sdxzxs;
                XmlElement xe215 = xmldoc.CreateElement("jsgl");//创建一个<Node>节点 
                xe215.InnerText = model.jsgl;
                XmlElement xe216 = xmldoc.CreateElement("zsgl");//创建一个<Node>节点 
                xe216.InnerText = model.zsgl;
                XmlElement xe217 = xmldoc.CreateElement("jzgl");//创建一个<Node>节点 
                xe217.InnerText = model.jzgl;
                XmlElement xe218 = xmldoc.CreateElement("hjwd");//创建一个<Node>节点 
                xe218.InnerText = model.hjwd;
                XmlElement xe219 = xmldoc.CreateElement("dqyl");//创建一个<Node>节点 
                xe219.InnerText = model.dqyl;
                XmlElement xe220 = xmldoc.CreateElement("xdsd");//创建一个<Node>节点 
                xe220.InnerText = model.xdsd;
                XmlElement xe221 = xmldoc.CreateElement("yw");//创建一个<Node>节点 
                xe221.InnerText = model.yw;
                XmlElement xe222 = xmldoc.CreateElement("lljo2");//创建一个<Node>节点 
                xe222.InnerText = model.lljo2;
                XmlElement xe223 = xmldoc.CreateElement("hjo2");//创建一个<Node>节点 
                xe223.InnerText = model.hjo2;
                XmlElement xe224 = xmldoc.CreateElement("lljsjll");//创建一个<Node>节点 
                xe224.InnerText = model.lljsjll;
                XmlElement xe225 = xmldoc.CreateElement("lljbzll");//创建一个<Node>节点 
                xe225.InnerText = model.lljbzll;
                XmlElement xe226 = xmldoc.CreateElement("qcwqll");//创建一个<Node>节点 
                xe226.InnerText = model.qcwqll;
                XmlElement xe227 = xmldoc.CreateElement("xsb");//创建一个<Node>节点 
                xe227.InnerText = model.xsb;
                XmlElement xe228 = xmldoc.CreateElement("lljwd");//创建一个<Node>节点 
                xe228.InnerText = model.lljwd;
                XmlElement xe229 = xmldoc.CreateElement("lljqy");//创建一个<Node>节点 
                xe229.InnerText = model.lljqy;
                XmlElement xe230 = xmldoc.CreateElement("hcpfzl");//创建一个<Node>节点 
                xe230.InnerText = model.hcpfzl;
                XmlElement xe231 = xmldoc.CreateElement("nopfzl");//创建一个<Node>节点 
                xe231.InnerText = model.nopfzl;
                XmlElement xe232 = xmldoc.CreateElement("copfzl");//创建一个<Node>节点 
                xe232.InnerText = model.copfzl;
                XmlElement xe233 = xmldoc.CreateElement("sjxl");//创建一个<Node>节点 
                xe233.InnerText = model.sjxl;
                XmlElement xe234 = xmldoc.CreateElement("fxyglyl");//创建一个<Node>节点 
                xe234.InnerText = model.fxyglyl;
                XmlElement xe235 = xmldoc.CreateElement("nl");//创建一个<Node>节点 
                xe235.InnerText = model.nl;
                XmlElement xe236 = xmldoc.CreateElement("jczt");//创建一个<Node>节点 
                xe236.InnerText = model.jczt;
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe2.AppendChild(xe210);
                xe2.AppendChild(xe211);
                xe2.AppendChild(xe212);
                xe2.AppendChild(xe213);
                xe2.AppendChild(xe214);
                xe2.AppendChild(xe215);
                xe2.AppendChild(xe216);
                xe2.AppendChild(xe217);
                xe2.AppendChild(xe218);
                xe2.AppendChild(xe219);
                xe2.AppendChild(xe220);
                xe2.AppendChild(xe221);
                xe2.AppendChild(xe222);
                xe2.AppendChild(xe223);
                xe2.AppendChild(xe224);
                xe2.AppendChild(xe225);
                xe2.AppendChild(xe226);
                xe2.AppendChild(xe227);
                xe2.AppendChild(xe228);
                xe2.AppendChild(xe229);
                xe2.AppendChild(xe230);
                xe2.AppendChild(xe231);
                xe2.AppendChild(xe232);
                xe2.AppendChild(xe233);
                xe2.AppendChild(xe234);
                xe2.AppendChild(xe235);
                xe2.AppendChild(xe236);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("GC001", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                if (receiveXml.Length > 200)
                {
                    code = "-2";
                    if (receiveXml.Contains("将截断字符串或二进制数据"))
                        message = "过程数据长度过长无法写入需重检";
                    else if (receiveXml.Contains("站点返回:系统异常"))
                        message = "站点返回系统异常";
                    else
                        message = "过程数据写入返回异常" + receiveXml.Substring(0, 200);
                }
                else
                    ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行写入Vmas过程数据失败，原因：" + er.Message, "Vmas过程数据", 3);
            }
        }

        //稳态过程数据
        public void writeASMProcessLN(ASMProcess model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "GC002";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
                xe201.InnerText = model.jylsh;
                XmlElement xe202 = xmldoc.CreateElement("testtimes");//创建一个<Node>节点 
                xe202.InnerText = model.jycs;
                XmlElement xe203 = xmldoc.CreateElement("cyds");//创建一个<Node>节点 
                xe203.InnerText = model.cyds;
                XmlElement xe204 = xmldoc.CreateElement("cysx");//创建一个<Node>节点 
                xe204.InnerText = model.cysx;
                XmlElement xe205 = xmldoc.CreateElement("hcclz");//创建一个<Node>节点 
                xe205.InnerText = model.hcclz;
                XmlElement xe206 = xmldoc.CreateElement("noclz");//创建一个<Node>节点 
                xe206.InnerText = model.noclz;
                XmlElement xe207 = xmldoc.CreateElement("cs");//创建一个<Node>节点 
                xe207.InnerText = model.cs;
                XmlElement xe208 = xmldoc.CreateElement("zs");//创建一个<Node>节点 
                xe208.InnerText = model.zs;
                XmlElement xe209 = xmldoc.CreateElement("coclz");//创建一个<Node>节点 
                xe209.InnerText = model.coclz;
                XmlElement xe210 = xmldoc.CreateElement("co2clz");//创建一个<Node>节点 
                xe210.InnerText = model.co2clz;
                XmlElement xe211 = xmldoc.CreateElement("o2clz");//创建一个<Node>节点 
                xe211.InnerText = model.o2clz;
                XmlElement xe212 = xmldoc.CreateElement("xsxzxs");//创建一个<Node>节点 
                xe212.InnerText = model.xsxzxs;
                XmlElement xe213 = xmldoc.CreateElement("sdxzxs");//创建一个<Node>节点 
                xe213.InnerText = model.sdxzxs;
                XmlElement xe214 = xmldoc.CreateElement("jsgl");//创建一个<Node>节点 
                xe214.InnerText = model.jsgl;
                XmlElement xe215 = xmldoc.CreateElement("zsgl");//创建一个<Node>节点 
                xe215.InnerText = model.zsgl;
                XmlElement xe216 = xmldoc.CreateElement("jzgl");//创建一个<Node>节点 
                xe216.InnerText = model.jzgl;
                XmlElement xe217 = xmldoc.CreateElement("hjwd");//创建一个<Node>节点 
                xe217.InnerText = model.hjwd;
                XmlElement xe218 = xmldoc.CreateElement("dqyl");//创建一个<Node>节点 
                xe218.InnerText = model.dqyl;
                XmlElement xe219 = xmldoc.CreateElement("xdsd");//创建一个<Node>节点 
                xe219.InnerText = model.xdsd;
                XmlElement xe220 = xmldoc.CreateElement("yw");//创建一个<Node>节点 
                xe220.InnerText = model.yw;
                XmlElement xe221 = xmldoc.CreateElement("sjxl");//创建一个<Node>节点 
                xe221.InnerText = model.sjxl;
                XmlElement xe222 = xmldoc.CreateElement("nl");//创建一个<Node>节点 
                xe222.InnerText = model.nl;
                XmlElement xe223 = xmldoc.CreateElement("scfz");//创建一个<Node>节点 
                xe223.InnerText = model.scfz;
                XmlElement xe224 = xmldoc.CreateElement("jczt");//创建一个<Node>节点 
                xe224.InnerText = model.jczt;
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe2.AppendChild(xe210);
                xe2.AppendChild(xe211);
                xe2.AppendChild(xe212);
                xe2.AppendChild(xe213);
                xe2.AppendChild(xe214);
                xe2.AppendChild(xe215);
                xe2.AppendChild(xe216);
                xe2.AppendChild(xe217);
                xe2.AppendChild(xe218);
                xe2.AppendChild(xe219);
                xe2.AppendChild(xe220);
                xe2.AppendChild(xe221);
                xe2.AppendChild(xe222);
                xe2.AppendChild(xe223);
                xe2.AppendChild(xe224);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("GC002", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                if (receiveXml.Length > 200)
                {
                    code = "-2";
                    if (receiveXml.Contains("将截断字符串或二进制数据"))
                        message = "过程数据长度过长无法写入需重检";
                    else if (receiveXml.Contains("站点返回:系统异常"))
                        message = "站点返回系统异常";
                    else
                        message = "过程数据写入返回异常" + receiveXml.Substring(0, 200);
                }
                else
                    ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行写入ASM过程数据失败，原因：" + er.Message, "ASM过程数据", 3);
            }
        }

        //双怠速过程数据
        public void writeSDSProcessLN(SDSProcess model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "GC003";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
                xe201.InnerText = model.jylsh;
                XmlElement xe202 = xmldoc.CreateElement("testtimes");//创建一个<Node>节点 
                xe202.InnerText = model.jycs;
                XmlElement xe203 = xmldoc.CreateElement("cyds");//创建一个<Node>节点 
                xe203.InnerText = model.cyds;
                XmlElement xe204 = xmldoc.CreateElement("cysx");//创建一个<Node>节点 
                xe204.InnerText = model.cysx;
                XmlElement xe205 = xmldoc.CreateElement("hcclz");//创建一个<Node>节点 
                xe205.InnerText = model.hcclz;
                XmlElement xe206 = xmldoc.CreateElement("zs");//创建一个<Node>节点 
                xe206.InnerText = model.zs;
                XmlElement xe207 = xmldoc.CreateElement("coclz");//创建一个<Node>节点 
                xe207.InnerText = model.coclz;
                XmlElement xe208 = xmldoc.CreateElement("glkqxs");//创建一个<Node>节点 
                xe208.InnerText = model.glkqxs;
                XmlElement xe209 = xmldoc.CreateElement("yw");//创建一个<Node>节点 
                xe209.InnerText = model.yw;
                XmlElement xe210 = xmldoc.CreateElement("co2clz");//创建一个<Node>节点 
                xe210.InnerText = model.co2clz;
                XmlElement xe211 = xmldoc.CreateElement("o2clz");//创建一个<Node>节点 
                xe211.InnerText = model.o2clz;
                XmlElement xe212 = xmldoc.CreateElement("hjwd");//创建一个<Node>节点 
                xe212.InnerText = model.hjwd;
                XmlElement xe213 = xmldoc.CreateElement("dqyl");//创建一个<Node>节点 
                xe213.InnerText = model.dqyl;
                XmlElement xe214 = xmldoc.CreateElement("xdsd");//创建一个<Node>节点 
                xe214.InnerText = model.xdsd;
                XmlElement xe215 = xmldoc.CreateElement("sjxl");//创建一个<Node>节点 
                xe215.InnerText = model.sjxl;
                XmlElement xe216 = xmldoc.CreateElement("jczt");//创建一个<Node>节点 
                xe216.InnerText = model.jczt;
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe2.AppendChild(xe210);
                xe2.AppendChild(xe211);
                xe2.AppendChild(xe212);
                xe2.AppendChild(xe213);
                xe2.AppendChild(xe214);
                xe2.AppendChild(xe215);
                xe2.AppendChild(xe216);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("GC003", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                if (receiveXml.Length > 200)
                {
                    code = "-2";
                    if (receiveXml.Contains("将截断字符串或二进制数据"))
                        message = "过程数据长度过长无法写入需重检";
                    else if (receiveXml.Contains("站点返回:系统异常"))
                        message = "站点返回系统异常";
                    else
                        message = "过程数据写入返回异常" + receiveXml.Substring(0, 200);
                }
                else
                    ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行写入SDS过程数据失败，原因：" + er.Message, "SDS过程数据", 3);
            }
        }

        //加载减速过程果数据
        public void writeJZJSProcessLN(JZJSProcess model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "GC004";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
                xe201.InnerText = model.jylsh;
                XmlElement xe202 = xmldoc.CreateElement("testtimes");//创建一个<Node>节点 
                xe202.InnerText = model.jycs;
                XmlElement xe203 = xmldoc.CreateElement("cyds");//创建一个<Node>节点 
                xe203.InnerText = model.cyds;
                XmlElement xe204 = xmldoc.CreateElement("cysx");//创建一个<Node>节点 
                xe204.InnerText = model.cysx;
                XmlElement xe205 = xmldoc.CreateElement("btgclz");//创建一个<Node>节点 
                xe205.InnerText = model.btgclz;
                XmlElement xe206 = xmldoc.CreateElement("cs");//创建一个<Node>节点 
                xe206.InnerText = model.cs;
                XmlElement xe207 = xmldoc.CreateElement("zs");//创建一个<Node>节点 
                xe207.InnerText = model.zs;
                XmlElement xe208 = xmldoc.CreateElement("jzgl");//创建一个<Node>节点 
                xe208.InnerText = model.zgl;
                XmlElement xe209 = xmldoc.CreateElement("gxsxs");//创建一个<Node>节点 
                xe209.InnerText = model.gxsxs;
                XmlElement xe210 = xmldoc.CreateElement("zsgl");//创建一个<Node>节点 
                xe210.InnerText = model.zsgl;
                XmlElement xe211 = xmldoc.CreateElement("yw");//创建一个<Node>节点 
                xe211.InnerText = model.yw;
                XmlElement xe212 = xmldoc.CreateElement("glxzxs");//创建一个<Node>节点 
                xe212.InnerText = model.glxzxs;
                XmlElement xe213 = xmldoc.CreateElement("jsgl");//创建一个<Node>节点 
                xe213.InnerText = model.jsgl;
                XmlElement xe214 = xmldoc.CreateElement("btgd");//创建一个<Node>节点 
                xe214.InnerText = model.btgd;
                XmlElement xe215 = xmldoc.CreateElement("dqyl");//创建一个<Node>节点 
                xe215.InnerText = model.dqyl;
                XmlElement xe216 = xmldoc.CreateElement("xdsd");//创建一个<Node>节点 
                xe216.InnerText = model.xdsd;
                XmlElement xe217 = xmldoc.CreateElement("hjwd");//创建一个<Node>节点 
                xe217.InnerText = model.hjwd;
                XmlElement xe218 = xmldoc.CreateElement("sjxl");//创建一个<Node>节点 
                xe218.InnerText = model.sjxl;
                XmlElement xe219 = xmldoc.CreateElement("nl");//创建一个<Node>节点 
                xe219.InnerText = model.nl;
                XmlElement xe220 = xmldoc.CreateElement("jczt");//创建一个<Node>节点 
                xe220.InnerText = model.jczt;
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe2.AppendChild(xe210);
                xe2.AppendChild(xe211);
                xe2.AppendChild(xe212);
                xe2.AppendChild(xe213);
                xe2.AppendChild(xe214);
                xe2.AppendChild(xe215);
                xe2.AppendChild(xe216);
                xe2.AppendChild(xe217);
                xe2.AppendChild(xe218);
                xe2.AppendChild(xe219);
                xe2.AppendChild(xe220);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("GC004", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                if (receiveXml.Length > 200)
                {
                    code = "-2";
                    if (receiveXml.Contains("将截断字符串或二进制数据"))
                        message = "过程数据长度过长无法写入需重检";
                    else if (receiveXml.Contains("站点返回:系统异常"))
                        message = "站点返回系统异常";
                    else
                        message = "过程数据写入返回异常" + receiveXml.Substring(0, 200);
                }
                else
                    ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行写入JZJS过程数据失败，原因：" + er.Message, "JZJS过程数据", 3);
            }
        }

        //不透光过程数据
        public void writeZYJSProcessLN(ZYJSProcess model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "GC005";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
                xe201.InnerText = model.jylsh;
                XmlElement xe202 = xmldoc.CreateElement("testtimes");//创建一个<Node>节点 
                xe202.InnerText = model.jycs;
                XmlElement xe203 = xmldoc.CreateElement("cyds");//创建一个<Node>节点 
                xe203.InnerText = model.cyds;
                XmlElement xe204 = xmldoc.CreateElement("cysx");//创建一个<Node>节点 
                xe204.InnerText = model.cysx;
                XmlElement xe205 = xmldoc.CreateElement("ydzds");//创建一个<Node>节点 
                xe205.InnerText = model.ydzds;
                XmlElement xe206 = xmldoc.CreateElement("fdjdszs");//创建一个<Node>节点 
                xe206.InnerText = model.fdjdszs;
                XmlElement xe207 = xmldoc.CreateElement("yw");//创建一个<Node>节点 
                xe207.InnerText = model.yw;
                XmlElement xe208 = xmldoc.CreateElement("sjxl");//创建一个<Node>节点 
                xe208.InnerText = model.sjxl;
                XmlElement xe209 = xmldoc.CreateElement("jczt");//创建一个<Node>节点 
                xe209.InnerText = model.jczt;
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("GC005", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                if (receiveXml.Length > 200)
                {
                    code = "-2";
                    if (receiveXml.Contains("将截断字符串或二进制数据"))
                        message = "过程数据长度过长无法写入需重检";
                    else if (receiveXml.Contains("站点返回:系统异常"))
                        message = "站点返回系统异常";
                    else
                        message = "过程数据写入返回异常" + receiveXml.Substring(0, 200);
                }
                else
                    ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行写入ZYJS过程数据失败，原因：" + er.Message, "ZYJS过程数据", 3);
            }
        }

        #region 设备自检数据
        public void writeSelfCheckJZHX_LN(JZHXselfcheckLN model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "ZJ000";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("tsno");//创建一个<Node>节点 
                xe201.InnerText = model.tsno;
                XmlElement xe202 = xmldoc.CreateElement("testlineno");//创建一个<Node>节点 
                xe202.InnerText = model.testlineno;
                XmlElement xe203 = xmldoc.CreateElement("zjlx");//创建一个<Node>节点 
                xe203.InnerText = model.zjlx;
                XmlElement xe204 = xmldoc.CreateElement("jcrq");//创建一个<Node>节点 
                xe204.InnerText = model.jcrq;
                XmlElement xe205 = xmldoc.CreateElement("jckssj");//创建一个<Node>节点 
                xe205.InnerText = model.jckssj;
                XmlElement xe206 = xmldoc.CreateElement("sjhxsj1");//创建一个<Node>节点 
                xe206.InnerText = model.sjhxsj1;
                XmlElement xe207 = xmldoc.CreateElement("sjhxsj2");//创建一个<Node>节点 
                xe207.InnerText = model.sjhxsj2;
                XmlElement xe208 = xmldoc.CreateElement("ns1");//创建一个<Node>节点 
                xe208.InnerText = model.ns1;
                XmlElement xe209 = xmldoc.CreateElement("ns2");//创建一个<Node>节点 
                xe209.InnerText = model.ns2;
                XmlElement xe210 = xmldoc.CreateElement("myhxsj1");//创建一个<Node>节点 
                xe210.InnerText = model.myhxsj1;
                XmlElement xe211 = xmldoc.CreateElement("myhxsj2");//创建一个<Node>节点 
                xe211.InnerText = model.myhxsj2;
                XmlElement xe212 = xmldoc.CreateElement("zsgl1");//创建一个<Node>节点 
                xe212.InnerText = model.zsgl1;
                XmlElement xe213 = xmldoc.CreateElement("zsgl2");//创建一个<Node>节点 
                xe213.InnerText = model.zsgl2;
                XmlElement xe214 = xmldoc.CreateElement("jbgl");//创建一个<Node>节点 
                xe214.InnerText = model.jbgl;
                XmlElement xe215 = xmldoc.CreateElement("jcjg1");//创建一个<Node>节点 
                xe215.InnerText = model.jcjg1;
                XmlElement xe216 = xmldoc.CreateElement("jcjg2");//创建一个<Node>节点 
                xe216.InnerText = model.jcjg2;
                XmlElement xe217 = xmldoc.CreateElement("jcjg");//创建一个<Node>节点 
                xe217.InnerText = model.jcjg;

                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe2.AppendChild(xe210);
                xe2.AppendChild(xe211);
                xe2.AppendChild(xe212);
                xe2.AppendChild(xe213);
                xe2.AppendChild(xe214);
                xe2.AppendChild(xe215);
                xe2.AppendChild(xe216);
                xe2.AppendChild(xe217);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("ZJ000/1/jzhx", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行发送加载滑行自检失败，原因：" + er.Message, "加载滑行自检", 3);
            }
        }
        public void writeSelfCheckFJGLSS_LN(FJGLSSselfcheckLN model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "ZJ000";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("tsno");//创建一个<Node>节点 
                xe201.InnerText = model.tsno;
                XmlElement xe202 = xmldoc.CreateElement("testlineno");//创建一个<Node>节点 
                xe202.InnerText = model.testlineno;
                XmlElement xe203 = xmldoc.CreateElement("zjlx");//创建一个<Node>节点 
                xe203.InnerText = model.zjlx;
                XmlElement xe204 = xmldoc.CreateElement("jcrq");//创建一个<Node>节点 
                xe204.InnerText = model.jcrq;
                XmlElement xe205 = xmldoc.CreateElement("jckssj");//创建一个<Node>节点 
                xe205.InnerText = model.jckssj;
                XmlElement xe206 = xmldoc.CreateElement("jcjssj");//创建一个<Node>节点 
                xe206.InnerText = model.jcjssj;
                XmlElement xe207 = xmldoc.CreateElement("sjhxsj1");//创建一个<Node>节点 
                xe207.InnerText = model.sjhxsj1;
                XmlElement xe208 = xmldoc.CreateElement("sjhxsj2");//创建一个<Node>节点 
                xe208.InnerText = model.sjhxsj2;
                XmlElement xe209 = xmldoc.CreateElement("ns1");//创建一个<Node>节点 
                xe209.InnerText = model.ns1;
                XmlElement xe210 = xmldoc.CreateElement("ns2");//创建一个<Node>节点 
                xe210.InnerText = model.ns2;
                XmlElement xe211 = xmldoc.CreateElement("jbgl");//创建一个<Node>节点 
                xe211.InnerText = model.jbgl;
                XmlElement xe212 = xmldoc.CreateElement("jcjg");//创建一个<Node>节点 
                xe212.InnerText = model.jcjg;
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe2.AppendChild(xe210);
                xe2.AppendChild(xe211);
                xe2.AppendChild(xe212);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("ZJ000/2/fjglss", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行发送附加功率损失自检失败，原因：" + er.Message, "附加功率损失自检", 3);
            }
        }
        public void writeSelfCheckFXY_LN(FXYselfcheckLN model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "ZJ000";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("tsno");//创建一个<Node>节点 
                xe201.InnerText = model.tsno;
                XmlElement xe202 = xmldoc.CreateElement("testlineno");//创建一个<Node>节点 
                xe202.InnerText = model.testlineno;
                XmlElement xe203 = xmldoc.CreateElement("zjlx");//创建一个<Node>节点 
                xe203.InnerText = model.zjlx;
                XmlElement xe204 = xmldoc.CreateElement("jcrq");//创建一个<Node>节点 
                xe204.InnerText = model.jcrq;
                XmlElement xe205 = xmldoc.CreateElement("jclx");//创建一个<Node>节点 
                xe205.InnerText = model.jclx;
                XmlElement xe206 = xmldoc.CreateElement("jckssj");//创建一个<Node>节点 
                xe206.InnerText = model.jckssj;
                XmlElement xe207 = xmldoc.CreateElement("c3h8nd");//创建一个<Node>节点 
                xe207.InnerText = model.c3h8nd;
                XmlElement xe208 = xmldoc.CreateElement("cond");//创建一个<Node>节点 
                xe208.InnerText = model.cond;
                XmlElement xe209 = xmldoc.CreateElement("co2nd");//创建一个<Node>节点 
                xe209.InnerText = model.co2nd;
                XmlElement xe210 = xmldoc.CreateElement("nond");//创建一个<Node>节点 
                xe210.InnerText = model.nond;
                XmlElement xe211 = xmldoc.CreateElement("o2nd");//创建一个<Node>节点 
                xe211.InnerText = model.o2nd;
                XmlElement xe212 = xmldoc.CreateElement("hcjcz");//创建一个<Node>节点 
                xe212.InnerText = model.hcjcz;
                XmlElement xe213 = xmldoc.CreateElement("cojcz");//创建一个<Node>节点 
                xe213.InnerText = model.cojcz;
                XmlElement xe214 = xmldoc.CreateElement("co2jcz");//创建一个<Node>节点 
                xe214.InnerText = model.co2jcz;
                XmlElement xe215 = xmldoc.CreateElement("nojcz");//创建一个<Node>节点 
                xe215.InnerText = model.nojcz;
                XmlElement xe216 = xmldoc.CreateElement("o2jcz");//创建一个<Node>节点 
                xe216.InnerText = model.o2jcz;
                XmlElement xe217 = xmldoc.CreateElement("pef");//创建一个<Node>节点 
                xe217.InnerText = model.pef;
                XmlElement xe218 = xmldoc.CreateElement("jcjg");//创建一个<Node>节点 
                xe218.InnerText = model.jcjg;
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe2.AppendChild(xe210);
                xe2.AppendChild(xe211);
                xe2.AppendChild(xe212);
                xe2.AppendChild(xe213);
                xe2.AppendChild(xe214);
                xe2.AppendChild(xe215);
                xe2.AppendChild(xe216);
                xe2.AppendChild(xe217);
                xe2.AppendChild(xe218);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("ZJ000/3/fxy", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行发送分析仪自检失败，原因：" + er.Message, "分析仪自检", 3);
            }
        }
        public void writeSelfCheckXL_LN(XLselfcheckLN model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "ZJ000";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("tsno");//创建一个<Node>节点 
                xe201.InnerText = model.tsno;
                XmlElement xe202 = xmldoc.CreateElement("testlineno");//创建一个<Node>节点 
                xe202.InnerText = model.testlineno;
                XmlElement xe203 = xmldoc.CreateElement("zjlx");//创建一个<Node>节点 
                xe203.InnerText = model.zjlx;
                XmlElement xe204 = xmldoc.CreateElement("jcrq");//创建一个<Node>节点 
                xe204.InnerText = model.jcrq;
                XmlElement xe205 = xmldoc.CreateElement("jckssj");//创建一个<Node>节点 
                xe205.InnerText = model.jckssj;
                XmlElement xe206 = xmldoc.CreateElement("jcjg");//创建一个<Node>节点 
                xe206.InnerText = model.jcjg;

                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("ZJ000/4/xl", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行发送泄露自检失败，原因：" + er.Message, "泄露自检", 3);
            }
        }
        public void writeSelfCheckFXYYLC_LN(FXYYLCselfcheckLN model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "ZJ000";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("tsno");//创建一个<Node>节点 
                xe201.InnerText = model.tsno;
                XmlElement xe202 = xmldoc.CreateElement("testlineno");//创建一个<Node>节点 
                xe202.InnerText = model.testlineno;
                XmlElement xe203 = xmldoc.CreateElement("zjlx");//创建一个<Node>节点 
                xe203.InnerText = model.zjlx;
                XmlElement xe204 = xmldoc.CreateElement("jcrq");//创建一个<Node>节点 
                xe204.InnerText = model.jcrq;
                XmlElement xe205 = xmldoc.CreateElement("jckssj");//创建一个<Node>节点 
                xe205.InnerText = model.jckssj;
                XmlElement xe206 = xmldoc.CreateElement("o2lcbz");//创建一个<Node>节点 
                xe206.InnerText = model.o2lcbz;
                XmlElement xe207 = xmldoc.CreateElement("o2lcclz");//创建一个<Node>节点 
                xe207.InnerText = model.o2lcclz;
                XmlElement xe208 = xmldoc.CreateElement("o2lcwc");//创建一个<Node>节点 
                xe208.InnerText = model.o2lcwc;
                XmlElement xe209 = xmldoc.CreateElement("jcjg");//创建一个<Node>节点 
                xe209.InnerText = model.jcjg;

                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("ZJ000/5/fxyylc", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行发送分析仪氧量程自检失败，原因：" + er.Message, "分析仪氧量程自检", 3);
            }
        }
        public void writeSelfCheckDBQ_LN(DBQselfcheckLN model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "ZJ000";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("tsno");//创建一个<Node>节点 
                xe201.InnerText = model.tsno;
                XmlElement xe202 = xmldoc.CreateElement("testlineno");//创建一个<Node>节点 
                xe202.InnerText = model.testlineno;
                XmlElement xe203 = xmldoc.CreateElement("zjlx");//创建一个<Node>节点 
                xe203.InnerText = model.zjlx;
                XmlElement xe204 = xmldoc.CreateElement("jcrq");//创建一个<Node>节点 
                xe204.InnerText = model.jcrq;
                XmlElement xe205 = xmldoc.CreateElement("jclx");//创建一个<Node>节点 
                xe205.InnerText = model.jclx;
                XmlElement xe206 = xmldoc.CreateElement("jckssj");//创建一个<Node>节点 
                xe206.InnerText = model.jckssj;
                XmlElement xe207 = xmldoc.CreateElement("c3h8nd");//创建一个<Node>节点 
                xe207.InnerText = model.c3h8nd;
                XmlElement xe208 = xmldoc.CreateElement("cond");//创建一个<Node>节点 
                xe208.InnerText = model.cond;
                XmlElement xe209 = xmldoc.CreateElement("co2nd");//创建一个<Node>节点 
                xe209.InnerText = model.co2nd;
                XmlElement xe210 = xmldoc.CreateElement("nond");//创建一个<Node>节点 
                xe210.InnerText = model.nond;
                XmlElement xe211 = xmldoc.CreateElement("o2nd");//创建一个<Node>节点 
                xe211.InnerText = model.o2nd;
                XmlElement xe212 = xmldoc.CreateElement("hcjcz");//创建一个<Node>节点 
                xe212.InnerText = model.hcjcz;
                XmlElement xe213 = xmldoc.CreateElement("cojcz");//创建一个<Node>节点 
                xe213.InnerText = model.cojcz;
                XmlElement xe214 = xmldoc.CreateElement("co2jcz");//创建一个<Node>节点 
                xe214.InnerText = model.co2jcz;
                XmlElement xe215 = xmldoc.CreateElement("nojcz");//创建一个<Node>节点 
                xe215.InnerText = model.nojcz;
                XmlElement xe216 = xmldoc.CreateElement("o2jcz");//创建一个<Node>节点 
                xe216.InnerText = model.o2jcz;
                XmlElement xe217 = xmldoc.CreateElement("pef");//创建一个<Node>节点 
                xe217.InnerText = model.pef;
                XmlElement xe218 = xmldoc.CreateElement("jcjg");//创建一个<Node>节点 
                xe218.InnerText = model.jcjg;

                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe2.AppendChild(xe210);
                xe2.AppendChild(xe211);
                xe2.AppendChild(xe212);
                xe2.AppendChild(xe213);
                xe2.AppendChild(xe214);
                xe2.AppendChild(xe215);
                xe2.AppendChild(xe216);
                xe2.AppendChild(xe217);
                xe2.AppendChild(xe218);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("ZJ000/6/dbq", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行发送低标气自检失败，原因：" + er.Message, "低标气自检", 3);
            }
        }
        public void writeSelfCheckLLJ_LN(LLJselfcheckLN model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "ZJ000";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("tsno");//创建一个<Node>节点 
                xe201.InnerText = model.tsno;
                XmlElement xe202 = xmldoc.CreateElement("testlineno");//创建一个<Node>节点 
                xe202.InnerText = model.testlineno;
                XmlElement xe203 = xmldoc.CreateElement("zjlx");//创建一个<Node>节点 
                xe203.InnerText = model.zjlx;
                XmlElement xe204 = xmldoc.CreateElement("jcrq");//创建一个<Node>节点 
                xe204.InnerText = model.jcrq;
                XmlElement xe205 = xmldoc.CreateElement("jckssj");//创建一个<Node>节点 
                xe205.InnerText = model.jckssj;
                XmlElement xe206 = xmldoc.CreateElement("o2glcbz");//创建一个<Node>节点 
                xe206.InnerText = model.o2glcbz;
                XmlElement xe207 = xmldoc.CreateElement("o2glcclz");//创建一个<Node>节点 
                xe207.InnerText = model.o2glcclz;
                XmlElement xe208 = xmldoc.CreateElement("o2glcwc");//创建一个<Node>节点 
                xe208.InnerText = model.o2glcwc;
                XmlElement xe209 = xmldoc.CreateElement("o2dlcbz");//创建一个<Node>节点 
                xe209.InnerText = model.o2dlcbz;
                XmlElement xe210 = xmldoc.CreateElement("o2dlcclz");//创建一个<Node>节点 
                xe210.InnerText = model.o2dlcclz;
                XmlElement xe211 = xmldoc.CreateElement("o2dlcwc");//创建一个<Node>节点 
                xe211.InnerText = model.o2dlcwc;
                XmlElement xe212 = xmldoc.CreateElement("jcjg");//创建一个<Node>节点 
                xe212.InnerText = model.jcjg;

                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe2.AppendChild(xe210);
                xe2.AppendChild(xe211);
                xe2.AppendChild(xe212);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("ZJ000/7/llj", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行发送流量计自检失败，原因：" + er.Message, "流量计自检", 3);
            }
        }
        #endregion
        #region 设备标定数据
        public void writeSpeedBD_LN(SpeedBD_LN model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "BD000";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
                xe201.InnerText = model.sbbh;
                XmlElement xe202 = xmldoc.CreateElement("bdsj");//创建一个<Node>节点 
                xe202.InnerText = model.bdsj;
                XmlElement xe203 = xmldoc.CreateElement("bdr");//创建一个<Node>节点 
                xe203.InnerText = model.bdr;
                XmlElement xe204 = xmldoc.CreateElement("czr");//创建一个<Node>节点 
                xe204.InnerText = model.czr;
                XmlElement xe205 = xmldoc.CreateElement("bdlx");//创建一个<Node>节点 
                xe205.InnerText = model.bdlx;
                XmlElement xe206 = xmldoc.CreateElement("bdnr");//创建一个<Node>节点 
                #region 速度标定内容
                XmlElement xe20600 = xmldoc.CreateElement("csxs");//创建一个<Node>节点 
                xe20600.InnerText = model.csxs;
                XmlElement xe20601 = xmldoc.CreateElement("sdz1");//创建一个<Node>节点 
                xe20601.InnerText = model.sdz1;
                XmlElement xe20602 = xmldoc.CreateElement("scz1");//创建一个<Node>节点 
                xe20602.InnerText = model.scz1;
                XmlElement xe20603 = xmldoc.CreateElement("jdwcz1");//创建一个<Node>节点 
                xe20603.InnerText = model.jdwcz1;
                XmlElement xe20604 = xmldoc.CreateElement("xdwcz1");//创建一个<Node>节点 
                xe20604.InnerText = model.xdwcz1;
                XmlElement xe20605 = xmldoc.CreateElement("sdz2");//创建一个<Node>节点 
                xe20605.InnerText = model.sdz2;
                XmlElement xe20606 = xmldoc.CreateElement("scz2");//创建一个<Node>节点 
                xe20606.InnerText = model.scz2;
                XmlElement xe20607 = xmldoc.CreateElement("jdwcz2");//创建一个<Node>节点 
                xe20607.InnerText = model.jdwcz2;
                XmlElement xe20608 = xmldoc.CreateElement("xdwcz2");//创建一个<Node>节点 
                xe20608.InnerText = model.xdwcz2;
                XmlElement xe20609 = xmldoc.CreateElement("sdz3");//创建一个<Node>节点 
                xe20609.InnerText = model.sdz3;
                XmlElement xe20610 = xmldoc.CreateElement("scz3");//创建一个<Node>节点 
                xe20610.InnerText = model.scz3;
                XmlElement xe20611 = xmldoc.CreateElement("jdwcz3");//创建一个<Node>节点 
                xe20611.InnerText = model.jdwcz3;
                XmlElement xe20612 = xmldoc.CreateElement("xdwcz3");//创建一个<Node>节点 
                xe20612.InnerText = model.xdwcz3;
                xe206.AppendChild(xe20600);
                xe206.AppendChild(xe20601);
                xe206.AppendChild(xe20602);
                xe206.AppendChild(xe20603);
                xe206.AppendChild(xe20604);
                xe206.AppendChild(xe20605);
                xe206.AppendChild(xe20606);
                xe206.AppendChild(xe20607);
                xe206.AppendChild(xe20608);
                xe206.AppendChild(xe20609);
                xe206.AppendChild(xe20610);
                xe206.AppendChild(xe20611);
                xe206.AppendChild(xe20612);
                #endregion
                XmlElement xe207 = xmldoc.CreateElement("bdjg");//创建一个<Node>节点 
                xe207.InnerText = model.bdjg;
                XmlElement xe208 = xmldoc.CreateElement("tsno");//创建一个<Node>节点 
                xe208.InnerText = model.jczbh;
                XmlElement xe209 = xmldoc.CreateElement("testlineno");//创建一个<Node>节点 
                xe209.InnerText = model.jcgwh;
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("BD000/1/speed", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行发送速度标定失败，原因：" + er.Message, "速度标定", 3);
            }
        }
        public void writeNlBD_LN(NlBD_LN model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "BD000";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
                xe201.InnerText = model.sbbh;
                XmlElement xe202 = xmldoc.CreateElement("bdsj");//创建一个<Node>节点 
                xe202.InnerText = model.bdsj;
                XmlElement xe203 = xmldoc.CreateElement("bdr");//创建一个<Node>节点 
                xe203.InnerText = model.bdr;
                XmlElement xe204 = xmldoc.CreateElement("czr");//创建一个<Node>节点 
                xe204.InnerText = model.czr;
                XmlElement xe205 = xmldoc.CreateElement("bdlx");//创建一个<Node>节点 
                xe205.InnerText = model.bdlx;
                XmlElement xe206 = xmldoc.CreateElement("bdnr");//创建一个<Node>节点 
                #region 扭力标定内容
                XmlElement xe20600 = xmldoc.CreateElement("nlxs");//创建一个<Node>节点 
                xe20600.InnerText = model.nlxs;
                XmlElement xe20601 = xmldoc.CreateElement("nlsdz1");//创建一个<Node>节点 
                xe20601.InnerText = model.nlsdz1;
                XmlElement xe20602 = xmldoc.CreateElement("nlscz1");//创建一个<Node>节点 
                xe20602.InnerText = model.nlscz1;
                XmlElement xe20603 = xmldoc.CreateElement("nlwcz1");//创建一个<Node>节点 
                xe20603.InnerText = model.nlwcz1;
                XmlElement xe20604 = xmldoc.CreateElement("nlsdz2");//创建一个<Node>节点 
                xe20604.InnerText = model.nlsdz2;
                XmlElement xe20605 = xmldoc.CreateElement("nlscz2");//创建一个<Node>节点 
                xe20605.InnerText = model.nlscz2;
                XmlElement xe20606 = xmldoc.CreateElement("nlwcz2");//创建一个<Node>节点 
                xe20606.InnerText = model.nlwcz2;
                XmlElement xe20607 = xmldoc.CreateElement("nlsdz3");//创建一个<Node>节点 
                xe20607.InnerText = model.nlsdz3;
                XmlElement xe20608 = xmldoc.CreateElement("nlscz3");//创建一个<Node>节点 
                xe20608.InnerText = model.nlscz3;
                XmlElement xe20609 = xmldoc.CreateElement("nlwcz3");//创建一个<Node>节点 
                xe20609.InnerText = model.nlwcz3;
                XmlElement xe20610 = xmldoc.CreateElement("nlsdz4");//创建一个<Node>节点 
                xe20610.InnerText = model.nlsdz4;
                XmlElement xe20611 = xmldoc.CreateElement("nlscz4");//创建一个<Node>节点 
                xe20611.InnerText = model.nlscz4;
                XmlElement xe20612 = xmldoc.CreateElement("nlwcz4");//创建一个<Node>节点 
                xe20612.InnerText = model.nlwcz4;
                XmlElement xe20613 = xmldoc.CreateElement("nlsdz5");//创建一个<Node>节点 
                xe20613.InnerText = model.nlsdz5;
                XmlElement xe20614 = xmldoc.CreateElement("nlscz5");//创建一个<Node>节点 
                xe20614.InnerText = model.nlscz5;
                XmlElement xe20615 = xmldoc.CreateElement("nlwcz5");//创建一个<Node>节点 
                xe20615.InnerText = model.nlwcz5;
                xe206.AppendChild(xe20600);
                xe206.AppendChild(xe20601);
                xe206.AppendChild(xe20602);
                xe206.AppendChild(xe20603);
                xe206.AppendChild(xe20604);
                xe206.AppendChild(xe20605);
                xe206.AppendChild(xe20606);
                xe206.AppendChild(xe20607);
                xe206.AppendChild(xe20608);
                xe206.AppendChild(xe20609);
                xe206.AppendChild(xe20610);
                xe206.AppendChild(xe20611);
                xe206.AppendChild(xe20612);
                xe206.AppendChild(xe20613);
                xe206.AppendChild(xe20614);
                xe206.AppendChild(xe20615);
                #endregion
                XmlElement xe207 = xmldoc.CreateElement("bdjg");//创建一个<Node>节点 
                xe207.InnerText = model.bdjg;
                XmlElement xe208 = xmldoc.CreateElement("tsno");//创建一个<Node>节点 
                xe208.InnerText = model.jczbh;
                XmlElement xe209 = xmldoc.CreateElement("testlineno");//创建一个<Node>节点 
                xe209.InnerText = model.jcgwh;
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("BD000/2/nl", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行发送扭力标定失败，原因：" + er.Message, "扭力标定", 3);
            }
        }
        public void writeJsglBD_LN(JsglBD model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "BD000";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
                xe201.InnerText = model.sbbh;
                XmlElement xe202 = xmldoc.CreateElement("bdsj");//创建一个<Node>节点 
                xe202.InnerText = model.bdsj;
                XmlElement xe203 = xmldoc.CreateElement("bdr");//创建一个<Node>节点 
                xe203.InnerText = model.bdr;
                XmlElement xe204 = xmldoc.CreateElement("czr");//创建一个<Node>节点 
                xe204.InnerText = model.czr;
                XmlElement xe205 = xmldoc.CreateElement("bdlx");//创建一个<Node>节点 
                xe205.InnerText = model.bdlx;
                XmlElement xe206 = xmldoc.CreateElement("bdnr");//创建一个<Node>节点  
                #region 寄生功率标定内容
                XmlElement xe20601 = xmldoc.CreateElement("hxsj1");//创建一个<Node>节点 
                xe20601.InnerText = model.hxsj1;
                XmlElement xe20602 = xmldoc.CreateElement("jsgl1");//创建一个<Node>节点 
                xe20602.InnerText = model.jsgl1;
                XmlElement xe20603 = xmldoc.CreateElement("sdzd1");//创建一个<Node>节点 
                xe20603.InnerText = model.sdzd1;
                XmlElement xe20604 = xmldoc.CreateElement("sdzx1");//创建一个<Node>节点 
                xe20604.InnerText = model.sdzx1;
                XmlElement xe20605 = xmldoc.CreateElement("mysd1");//创建一个<Node>节点 
                xe20605.InnerText = model.mysd1;
                XmlElement xe20606 = xmldoc.CreateElement("jbgl");//创建一个<Node>节点 
                xe20606.InnerText = model.jbgl;
                XmlElement xe20607 = xmldoc.CreateElement("kssj1");//创建一个<Node>节点 
                xe20607.InnerText = model.kssj1;
                XmlElement xe20608 = xmldoc.CreateElement("jssj1");//创建一个<Node>节点 
                xe20608.InnerText = model.jssj1;
                XmlElement xe20609 = xmldoc.CreateElement("hxsj2");//创建一个<Node>节点 
                xe20609.InnerText = model.hxsj2;
                XmlElement xe20610 = xmldoc.CreateElement("jsgl2");//创建一个<Node>节点 
                xe20610.InnerText = model.jsgl2;
                XmlElement xe20611 = xmldoc.CreateElement("sdzd2");//创建一个<Node>节点 
                xe20611.InnerText = model.sdzd2;
                XmlElement xe20612 = xmldoc.CreateElement("sdzx2");//创建一个<Node>节点 
                xe20612.InnerText = model.sdzx2;
                XmlElement xe20613 = xmldoc.CreateElement("mysd2");//创建一个<Node>节点 
                xe20613.InnerText = model.mysd2;
                XmlElement xe20614 = xmldoc.CreateElement("kssj2");//创建一个<Node>节点 
                xe20614.InnerText = model.kssj2;
                XmlElement xe20615 = xmldoc.CreateElement("jssj2");//创建一个<Node>节点 
                xe20615.InnerText = model.jssj2;
                XmlElement xe20616 = xmldoc.CreateElement("hxsj3");//创建一个<Node>节点 
                xe20616.InnerText = model.hxsj3;
                XmlElement xe20617 = xmldoc.CreateElement("jsgl3");//创建一个<Node>节点 
                xe20617.InnerText = model.jsgl3;
                XmlElement xe20618 = xmldoc.CreateElement("sdzd3");//创建一个<Node>节点 
                xe20618.InnerText = model.sdzd3;
                XmlElement xe20619 = xmldoc.CreateElement("sdzx3");//创建一个<Node>节点 
                xe20619.InnerText = model.sdzx3;
                XmlElement xe20620 = xmldoc.CreateElement("mysd3");//创建一个<Node>节点 
                xe20620.InnerText = model.mysd3;
                XmlElement xe20621 = xmldoc.CreateElement("kssj3");//创建一个<Node>节点 
                xe20621.InnerText = model.kssj3;
                XmlElement xe20622 = xmldoc.CreateElement("jssj3");//创建一个<Node>节点 
                xe20622.InnerText = model.jssj3;
                XmlElement xe20623 = xmldoc.CreateElement("hxsj4");//创建一个<Node>节点 
                xe20623.InnerText = model.hxsj4;
                XmlElement xe20624 = xmldoc.CreateElement("jsgl4");//创建一个<Node>节点 
                xe20624.InnerText = model.jsgl4;
                XmlElement xe20625 = xmldoc.CreateElement("sdzd4");//创建一个<Node>节点 
                xe20625.InnerText = model.sdzd4;
                XmlElement xe20626 = xmldoc.CreateElement("sdzx4");//创建一个<Node>节点 
                xe20626.InnerText = model.sdzx4;
                XmlElement xe20627 = xmldoc.CreateElement("mysd4");//创建一个<Node>节点 
                xe20627.InnerText = model.mysd4;
                XmlElement xe20628 = xmldoc.CreateElement("kssj4");//创建一个<Node>节点 
                xe20628.InnerText = model.kssj4;
                XmlElement xe20629 = xmldoc.CreateElement("jssj4");//创建一个<Node>节点 
                xe20629.InnerText = model.jssj4;
                XmlElement xe20630 = xmldoc.CreateElement("hxsj5");//创建一个<Node>节点 
                xe20630.InnerText = model.hxsj5;
                XmlElement xe20631 = xmldoc.CreateElement("jsgl5");//创建一个<Node>节点 
                xe20631.InnerText = model.jsgl5;
                XmlElement xe20632 = xmldoc.CreateElement("sdzd5");//创建一个<Node>节点 
                xe20632.InnerText = model.sdzd5;
                XmlElement xe20633 = xmldoc.CreateElement("sdzx5");//创建一个<Node>节点 
                xe20633.InnerText = model.sdzx5;
                XmlElement xe20634 = xmldoc.CreateElement("mysd5");//创建一个<Node>节点 
                xe20634.InnerText = model.mysd5;
                XmlElement xe20635 = xmldoc.CreateElement("kssj5");//创建一个<Node>节点 
                xe20635.InnerText = model.kssj5;
                XmlElement xe20636 = xmldoc.CreateElement("jssj5");//创建一个<Node>节点 
                xe20636.InnerText = model.jssj5;
                xe206.AppendChild(xe20601);
                xe206.AppendChild(xe20602);
                xe206.AppendChild(xe20603);
                xe206.AppendChild(xe20604);
                xe206.AppendChild(xe20605);
                xe206.AppendChild(xe20606);
                xe206.AppendChild(xe20607);
                xe206.AppendChild(xe20608);
                xe206.AppendChild(xe20609);
                xe206.AppendChild(xe20610);
                xe206.AppendChild(xe20611);
                xe206.AppendChild(xe20612);
                xe206.AppendChild(xe20613);
                xe206.AppendChild(xe20614);
                xe206.AppendChild(xe20615);
                xe206.AppendChild(xe20616);
                xe206.AppendChild(xe20617);
                xe206.AppendChild(xe20618);
                xe206.AppendChild(xe20619);
                xe206.AppendChild(xe20620);
                xe206.AppendChild(xe20621);
                xe206.AppendChild(xe20622);
                xe206.AppendChild(xe20623);
                xe206.AppendChild(xe20624);
                xe206.AppendChild(xe20625);
                xe206.AppendChild(xe20626);
                xe206.AppendChild(xe20627);
                xe206.AppendChild(xe20628);
                xe206.AppendChild(xe20629);
                xe206.AppendChild(xe20630);
                xe206.AppendChild(xe20631);
                xe206.AppendChild(xe20632);
                xe206.AppendChild(xe20633);
                xe206.AppendChild(xe20634);
                xe206.AppendChild(xe20635);
                xe206.AppendChild(xe20636);
                #endregion
                XmlElement xe207 = xmldoc.CreateElement("bdjg");//创建一个<Node>节点 
                xe207.InnerText = model.bdjg;
                XmlElement xe208 = xmldoc.CreateElement("tsno");//创建一个<Node>节点 
                xe208.InnerText = model.jczbh;
                XmlElement xe209 = xmldoc.CreateElement("testlineno");//创建一个<Node>节点 
                xe209.InnerText = model.jcgwh;
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("BD000/3/jsgl", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行发送寄生功率标定失败，原因：" + er.Message, "寄生功率标定", 3);
            }
        }
        public void writeJzhxBD_LN(JzhxBD model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "BD000";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
                xe201.InnerText = model.sbbh;
                XmlElement xe202 = xmldoc.CreateElement("bdsj");//创建一个<Node>节点 
                xe202.InnerText = model.bdsj;
                XmlElement xe203 = xmldoc.CreateElement("bdr");//创建一个<Node>节点 
                xe203.InnerText = model.bdr;
                XmlElement xe204 = xmldoc.CreateElement("czr");//创建一个<Node>节点 
                xe204.InnerText = model.czr;
                XmlElement xe205 = xmldoc.CreateElement("bdlx");//创建一个<Node>节点 
                xe205.InnerText = model.bdlx;
                XmlElement xe206 = xmldoc.CreateElement("bdnr");//创建一个<Node>节点  
                #region 加载滑行标定内容
                XmlElement xe20601 = xmldoc.CreateElement("hxqj1");//创建一个<Node>节点 
                xe20601.InnerText = model.hxqj1;
                XmlElement xe20602 = xmldoc.CreateElement("hxqj2");//创建一个<Node>节点 
                xe20602.InnerText = model.hxqj2;
                XmlElement xe20603 = xmldoc.CreateElement("hxqj3");//创建一个<Node>节点 
                xe20603.InnerText = model.hxqj3;
                XmlElement xe20604 = xmldoc.CreateElement("hxqj4");//创建一个<Node>节点 
                xe20604.InnerText = model.hxqj4;
                XmlElement xe20605 = xmldoc.CreateElement("jzgl1");//创建一个<Node>节点 
                xe20605.InnerText = model.jzgl1;
                XmlElement xe20606 = xmldoc.CreateElement("jzgl2");//创建一个<Node>节点 
                xe20606.InnerText = model.jzgl2;
                XmlElement xe20607 = xmldoc.CreateElement("jzgl3");//创建一个<Node>节点 
                xe20607.InnerText = model.jzgl3;
                XmlElement xe20608 = xmldoc.CreateElement("jzgl4");//创建一个<Node>节点 
                xe20608.InnerText = model.jzgl4;
                XmlElement xe20609 = xmldoc.CreateElement("jsgl1");//创建一个<Node>节点 
                xe20609.InnerText = model.jsgl1;
                XmlElement xe20610 = xmldoc.CreateElement("jsgl2");//创建一个<Node>节点 
                xe20610.InnerText = model.jsgl2;
                XmlElement xe20611 = xmldoc.CreateElement("jsgl3");//创建一个<Node>节点 
                xe20611.InnerText = model.jsgl3;
                XmlElement xe20612 = xmldoc.CreateElement("jsgl4");//创建一个<Node>节点 
                xe20612.InnerText = model.jsgl4;
                XmlElement xe20613 = xmldoc.CreateElement("hxsj1");//创建一个<Node>节点 
                xe20613.InnerText = model.hxsj1;
                XmlElement xe20614 = xmldoc.CreateElement("hxsj2");//创建一个<Node>节点 
                xe20614.InnerText = model.hxsj2;
                XmlElement xe20615 = xmldoc.CreateElement("hxsj3");//创建一个<Node>节点 
                xe20615.InnerText = model.hxsj3;
                XmlElement xe20616 = xmldoc.CreateElement("hxsj4");//创建一个<Node>节点 
                xe20616.InnerText = model.hxsj4;
                XmlElement xe20617 = xmldoc.CreateElement("llsj1");//创建一个<Node>节点 
                xe20617.InnerText = model.llsj1;
                XmlElement xe20618 = xmldoc.CreateElement("llsj2");//创建一个<Node>节点 
                xe20618.InnerText = model.llsj2;
                XmlElement xe20619 = xmldoc.CreateElement("llsj3");//创建一个<Node>节点 
                xe20619.InnerText = model.llsj3;
                XmlElement xe20620 = xmldoc.CreateElement("llsj4");//创建一个<Node>节点 
                xe20620.InnerText = model.llsj4;
                XmlElement xe20621 = xmldoc.CreateElement("wc1");//创建一个<Node>节点 
                xe20621.InnerText = model.wc1;
                XmlElement xe20622 = xmldoc.CreateElement("wc2");//创建一个<Node>节点 
                xe20622.InnerText = model.wc2;
                XmlElement xe20623 = xmldoc.CreateElement("wc3");//创建一个<Node>节点 
                xe20623.InnerText = model.wc3;
                XmlElement xe20624 = xmldoc.CreateElement("wc4");//创建一个<Node>节点 
                xe20624.InnerText = model.wc4;
                XmlElement xe20625 = xmldoc.CreateElement("jbgl");//创建一个<Node>节点 
                xe20625.InnerText = model.jbgl;
                XmlElement xe20626 = xmldoc.CreateElement("hxqj1jcjg");//创建一个<Node>节点 
                xe20626.InnerText = model.hxqj1jcjg;
                XmlElement xe20627 = xmldoc.CreateElement("hxqj2jcjg");//创建一个<Node>节点 
                xe20627.InnerText = model.hxqj2jcjg;
                XmlElement xe20628 = xmldoc.CreateElement("hxqj3jcjg");//创建一个<Node>节点 
                xe20628.InnerText = model.hxqj3jcjg;
                XmlElement xe20629 = xmldoc.CreateElement("hxqj4jcjg");//创建一个<Node>节点 
                xe20629.InnerText = model.hxqj4jcjg;
                xe206.AppendChild(xe20601);
                xe206.AppendChild(xe20602);
                xe206.AppendChild(xe20603);
                xe206.AppendChild(xe20604);
                xe206.AppendChild(xe20605);
                xe206.AppendChild(xe20606);
                xe206.AppendChild(xe20607);
                xe206.AppendChild(xe20608);
                xe206.AppendChild(xe20609);
                xe206.AppendChild(xe20610);
                xe206.AppendChild(xe20611);
                xe206.AppendChild(xe20612);
                xe206.AppendChild(xe20613);
                xe206.AppendChild(xe20614);
                xe206.AppendChild(xe20615);
                xe206.AppendChild(xe20616);
                xe206.AppendChild(xe20617);
                xe206.AppendChild(xe20618);
                xe206.AppendChild(xe20619);
                xe206.AppendChild(xe20620);
                xe206.AppendChild(xe20621);
                xe206.AppendChild(xe20622);
                xe206.AppendChild(xe20623);
                xe206.AppendChild(xe20624);
                xe206.AppendChild(xe20625);
                xe206.AppendChild(xe20626);
                xe206.AppendChild(xe20627);
                xe206.AppendChild(xe20628);
                xe206.AppendChild(xe20629);
                #endregion
                XmlElement xe207 = xmldoc.CreateElement("bdjg");//创建一个<Node>节点 
                xe207.InnerText = model.bdjg;
                XmlElement xe208 = xmldoc.CreateElement("tsno");//创建一个<Node>节点 
                xe208.InnerText = model.jczbh;
                XmlElement xe209 = xmldoc.CreateElement("testlineno");//创建一个<Node>节点 
                xe209.InnerText = model.jcgwh;
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("BD000/4/jzhx", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行发送加载滑行标定失败，原因：" + er.Message, "加载滑行标定", 3);
            }
        }
        public void writeFqyBD_LN(FqBD model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "BD000";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
                xe201.InnerText = model.sbbh;
                XmlElement xe202 = xmldoc.CreateElement("bdsj");//创建一个<Node>节点 
                xe202.InnerText = model.bdsj;
                XmlElement xe203 = xmldoc.CreateElement("bdr");//创建一个<Node>节点 
                xe203.InnerText = model.bdr;
                XmlElement xe204 = xmldoc.CreateElement("czr");//创建一个<Node>节点 
                xe204.InnerText = model.czr;
                XmlElement xe205 = xmldoc.CreateElement("bdlx");//创建一个<Node>节点 
                xe205.InnerText = model.bdlx;
                XmlElement xe206 = xmldoc.CreateElement("bdnr");//创建一个<Node>节点 
                #region 废气仪标定内容
                XmlElement xe20601 = xmldoc.CreateElement("lx");//创建一个<Node>节点 
                xe20601.InnerText = model.lx;
                XmlElement xe20602 = xmldoc.CreateElement("bzc3h8");//创建一个<Node>节点 
                xe20602.InnerText = model.bzc3h8;
                XmlElement xe20603 = xmldoc.CreateElement("kssj");//创建一个<Node>节点 
                xe20603.InnerText = model.kssj;
                XmlElement xe20604 = xmldoc.CreateElement("jssj");//创建一个<Node>节点 
                xe20604.InnerText = model.jssj;
                XmlElement xe20605 = xmldoc.CreateElement("sdzhc1");//创建一个<Node>节点 
                xe20605.InnerText = model.sdzhc1;
                XmlElement xe20606 = xmldoc.CreateElement("sczhc1");//创建一个<Node>节点 
                xe20606.InnerText = model.sczhc1;
                XmlElement xe20607 = xmldoc.CreateElement("jdwczhc1");//创建一个<Node>节点 
                xe20607.InnerText = model.jdwczhc1;
                XmlElement xe20608 = xmldoc.CreateElement("xdwczhc1");//创建一个<Node>节点 
                xe20608.InnerText = model.xdwczhc1;
                XmlElement xe20609 = xmldoc.CreateElement("sdzhc2");//创建一个<Node>节点 
                xe20609.InnerText = model.sdzhc2;
                XmlElement xe20610 = xmldoc.CreateElement("sczhc2");//创建一个<Node>节点 
                xe20610.InnerText = model.sczhc2;
                XmlElement xe20611 = xmldoc.CreateElement("jdwczhc2");//创建一个<Node>节点 
                xe20611.InnerText = model.jdwczhc2;
                XmlElement xe20612 = xmldoc.CreateElement("xdwczhc2");//创建一个<Node>节点 
                xe20612.InnerText = model.xdwczhc2;
                XmlElement xe20613 = xmldoc.CreateElement("sdzco1");//创建一个<Node>节点 
                xe20613.InnerText = model.sdzco1;
                XmlElement xe20614 = xmldoc.CreateElement("sczco1");//创建一个<Node>节点 
                xe20614.InnerText = model.sczco1;
                XmlElement xe20615 = xmldoc.CreateElement("jdwczco1");//创建一个<Node>节点 
                xe20615.InnerText = model.jdwczco1;
                XmlElement xe20616 = xmldoc.CreateElement("xdwczco1");//创建一个<Node>节点 
                xe20616.InnerText = model.xdwczco1;
                XmlElement xe20617 = xmldoc.CreateElement("sdzco2");//创建一个<Node>节点 
                xe20617.InnerText = model.sdzco2;
                XmlElement xe20618 = xmldoc.CreateElement("sczco2");//创建一个<Node>节点 
                xe20618.InnerText = model.sczco2;
                XmlElement xe20619 = xmldoc.CreateElement("jdwczco2");//创建一个<Node>节点 
                xe20619.InnerText = model.jdwczco2;
                XmlElement xe20620 = xmldoc.CreateElement("xdwczco2");//创建一个<Node>节点 
                xe20620.InnerText = model.xdwczco2;
                XmlElement xe20621 = xmldoc.CreateElement("sdzco21");//创建一个<Node>节点 
                xe20621.InnerText = model.sdzco21;
                XmlElement xe20622 = xmldoc.CreateElement("sczco21");//创建一个<Node>节点 
                xe20622.InnerText = model.sczco21;
                XmlElement xe20623 = xmldoc.CreateElement("jdwczco21");//创建一个<Node>节点 
                xe20623.InnerText = model.jdwczco21;
                XmlElement xe20624 = xmldoc.CreateElement("xdwczco21");//创建一个<Node>节点 
                xe20624.InnerText = model.xdwczco21;
                XmlElement xe20625 = xmldoc.CreateElement("sdzco22");//创建一个<Node>节点 
                xe20625.InnerText = model.sdzco22;
                XmlElement xe20626 = xmldoc.CreateElement("sczco22");//创建一个<Node>节点 
                xe20626.InnerText = model.sczco22;
                XmlElement xe20627 = xmldoc.CreateElement("jdwczco22");//创建一个<Node>节点 
                xe20627.InnerText = model.jdwczco22;
                XmlElement xe20628 = xmldoc.CreateElement("xdwczco22");//创建一个<Node>节点 
                xe20628.InnerText = model.xdwczco22;
                XmlElement xe20629 = xmldoc.CreateElement("sdzno1");//创建一个<Node>节点 
                xe20629.InnerText = model.sdzno1;
                XmlElement xe20630 = xmldoc.CreateElement("sczno1");//创建一个<Node>节点 
                xe20630.InnerText = model.sczno1;
                XmlElement xe20631 = xmldoc.CreateElement("jdwczno1");//创建一个<Node>节点 
                xe20631.InnerText = model.jdwczno1;
                XmlElement xe20632 = xmldoc.CreateElement("xdwczno1");//创建一个<Node>节点 
                xe20632.InnerText = model.xdwczno1;
                XmlElement xe20633 = xmldoc.CreateElement("sdzno2");//创建一个<Node>节点 
                xe20633.InnerText = model.sdzno2;
                XmlElement xe20634 = xmldoc.CreateElement("sczno2");//创建一个<Node>节点 
                xe20634.InnerText = model.sczno2;
                XmlElement xe20635 = xmldoc.CreateElement("jdwczno2");//创建一个<Node>节点 
                xe20635.InnerText = model.jdwczno2;
                XmlElement xe20636 = xmldoc.CreateElement("xdwczno2");//创建一个<Node>节点 
                xe20636.InnerText = model.xdwczno2;
                XmlElement xe20637 = xmldoc.CreateElement("pef");//创建一个<Node>节点 
                xe20637.InnerText = model.pef;
                XmlElement xe20638 = xmldoc.CreateElement("jcjg");//创建一个<Node>节点 
                xe20638.InnerText = model.jcjg;
                xe206.AppendChild(xe20601);
                xe206.AppendChild(xe20602);
                xe206.AppendChild(xe20603);
                xe206.AppendChild(xe20604);
                xe206.AppendChild(xe20605);
                xe206.AppendChild(xe20606);
                xe206.AppendChild(xe20607);
                xe206.AppendChild(xe20608);
                xe206.AppendChild(xe20609);
                xe206.AppendChild(xe20610);
                xe206.AppendChild(xe20611);
                xe206.AppendChild(xe20612);
                xe206.AppendChild(xe20613);
                xe206.AppendChild(xe20614);
                xe206.AppendChild(xe20615);
                xe206.AppendChild(xe20616);
                xe206.AppendChild(xe20617);
                xe206.AppendChild(xe20618);
                xe206.AppendChild(xe20619);
                xe206.AppendChild(xe20620);
                xe206.AppendChild(xe20621);
                xe206.AppendChild(xe20622);
                xe206.AppendChild(xe20623);
                xe206.AppendChild(xe20624);
                xe206.AppendChild(xe20625);
                xe206.AppendChild(xe20626);
                xe206.AppendChild(xe20627);
                xe206.AppendChild(xe20628);
                xe206.AppendChild(xe20629);
                xe206.AppendChild(xe20630);
                xe206.AppendChild(xe20631);
                xe206.AppendChild(xe20632);
                xe206.AppendChild(xe20633);
                xe206.AppendChild(xe20634);
                xe206.AppendChild(xe20635);
                xe206.AppendChild(xe20636);
                xe206.AppendChild(xe20637);
                xe206.AppendChild(xe20638);
                #endregion
                XmlElement xe207 = xmldoc.CreateElement("bdjg");//创建一个<Node>节点 
                xe207.InnerText = model.bdjg;
                XmlElement xe208 = xmldoc.CreateElement("tsno");//创建一个<Node>节点 
                xe208.InnerText = model.jczbh;
                XmlElement xe209 = xmldoc.CreateElement("testlineno");//创建一个<Node>节点 
                xe209.InnerText = model.jcgwh;
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("BD000/5/fqy", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行发送废气仪标定失败，原因：" + er.Message, "废气仪标定", 3);
            }
        }
        public void writeYdjBD_LN(YdBD model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {

                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "BD000";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("sbbh");//创建一个<Node>节点 
                xe201.InnerText = model.sbbh;
                XmlElement xe202 = xmldoc.CreateElement("bdsj");//创建一个<Node>节点 
                xe202.InnerText = model.bdsj;
                XmlElement xe203 = xmldoc.CreateElement("bdr");//创建一个<Node>节点 
                xe203.InnerText = model.bdr;
                XmlElement xe204 = xmldoc.CreateElement("czr");//创建一个<Node>节点 
                xe204.InnerText = model.czr;
                XmlElement xe205 = xmldoc.CreateElement("bdlx");//创建一个<Node>节点 
                xe205.InnerText = model.bdlx;
                XmlElement xe206 = xmldoc.CreateElement("bdnr");//创建一个<Node>节点 
                #region 烟度计标定内容
                XmlElement xe20601 = xmldoc.CreateElement("btgydsdz1");//创建一个<Node>节点 
                xe20601.InnerText = model.btgydsdz1;
                XmlElement xe20602 = xmldoc.CreateElement("btgydscz1");//创建一个<Node>节点 
                xe20602.InnerText = model.btgydscz1;
                XmlElement xe20603 = xmldoc.CreateElement("btgydwcz1");//创建一个<Node>节点 
                xe20603.InnerText = model.btgydwcz1;
                XmlElement xe20604 = xmldoc.CreateElement("btgydsdz2");//创建一个<Node>节点 
                xe20604.InnerText = model.btgydsdz2;
                XmlElement xe20605 = xmldoc.CreateElement("btgydscz2");//创建一个<Node>节点 
                xe20605.InnerText = model.btgydscz2;
                XmlElement xe20606 = xmldoc.CreateElement("btgydwcz2");//创建一个<Node>节点 
                xe20606.InnerText = model.btgydwcz2;
                XmlElement xe20607 = xmldoc.CreateElement("btgydsdz3");//创建一个<Node>节点 
                xe20607.InnerText = model.btgydsdz3;
                XmlElement xe20608 = xmldoc.CreateElement("btgydscz3");//创建一个<Node>节点 
                xe20608.InnerText = model.btgydscz3;
                XmlElement xe20609 = xmldoc.CreateElement("btgydwcz3");//创建一个<Node>节点 
                xe20609.InnerText = model.btgydwcz3;
                xe206.AppendChild(xe20601);
                xe206.AppendChild(xe20602);
                xe206.AppendChild(xe20603);
                xe206.AppendChild(xe20604);
                xe206.AppendChild(xe20605);
                xe206.AppendChild(xe20606);
                xe206.AppendChild(xe20607);
                xe206.AppendChild(xe20608);
                xe206.AppendChild(xe20609);
                #endregion
                XmlElement xe207 = xmldoc.CreateElement("bdjg");//创建一个<Node>节点 
                xe207.InnerText = model.bdjg;
                XmlElement xe208 = xmldoc.CreateElement("tsno");//创建一个<Node>节点 
                xe208.InnerText = model.jczbh;
                XmlElement xe209 = xmldoc.CreateElement("testlineno");//创建一个<Node>节点 
                xe209.InnerText = model.jcgwh;
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("BD000/6/ydj", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行发送烟度计标定失败，原因：" + er.Message, "烟度计标定", 3);
            }
        }
        #endregion
        #endregion

        #region 内蒙古联网
        //除开开始命令、加载减速过程数据外，其余同辽宁大连联网/// <summary>
        /// KS001项目开始
        /// </summary>
        /// <param name="model"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public void writeProjectStartNM(ProjectStart model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "KS001";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
                xe201.InnerText = model.jylsh;
                XmlElement xe202 = xmldoc.CreateElement("testtimes");//创建一个<Node>节点 
                xe202.InnerText = model.jycs;
                XmlElement xe203 = xmldoc.CreateElement("tsno");//创建一个<Node>节点 
                xe203.InnerText = model.jczbh;
                XmlElement xe204 = xmldoc.CreateElement("testlineno");//创建一个<Node>节点 
                xe204.InnerText = model.jcgwh;
                XmlElement xe205 = xmldoc.CreateElement("license");//创建一个<Node>节点 
                xe205.InnerText = DataChange.encodeUTF8(model.hphm);
                XmlElement xe206 = xmldoc.CreateElement("licensecode");//创建一个<Node>节点 
                xe206.InnerText = model.hpzl;
                XmlElement xe207 = xmldoc.CreateElement("jcjsy");//创建一个<Node>节点 
                xe207.InnerText = DataChange.encodeUTF8(model.ycy);
                XmlElement xe208 = xmldoc.CreateElement("jcczy");//创建一个<Node>节点 
                xe208.InnerText = DataChange.encodeUTF8(model.jcczy);
                XmlElement xe209 = xmldoc.CreateElement("zlkzy");//创建一个<Node>节点 
                xe209.InnerText = DataChange.encodeUTF8(FileOpreate.NM_Zlkzr);
                XmlElement xe210 = xmldoc.CreateElement("sqr");//创建一个<Node>节点 
                xe210.InnerText = DataChange.encodeUTF8(FileOpreate.NM_Sqr);
                XmlElement xe211 = xmldoc.CreateElement("testtype");//创建一个<Node>节点 
                xe211.InnerText = model.jcff;
                XmlElement xe212 = xmldoc.CreateElement("odometer");//创建一个<Node>节点 
                xe212.InnerText = model.ljxslc;
                XmlElement xe213 = xmldoc.CreateElement("dqsj");//创建一个<Node>节点 
                xe213.InnerText = DateTime.Parse(model.dqsj).ToString("yyyy-MM-dd HH:mm:ss");
                XmlElement xe214 = xmldoc.CreateElement("jcbbh");//创建一个<Node>节点 
                xe214.InnerText = model.jcbbh;
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe2.AppendChild(xe210);
                xe2.AppendChild(xe211);
                xe2.AppendChild(xe212);
                xe2.AppendChild(xe213);
                xe2.AppendChild(xe214);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("KS001", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行发送项目开始失败，原因：" + er.Message, "项目开始", 3);
            }
        }

        //加载减速过程果数据
        public void writeJZJSProcessNM(JZJSProcess model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "GC004";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("jylsh");//创建一个<Node>节点 
                xe201.InnerText = model.jylsh;
                XmlElement xe202 = xmldoc.CreateElement("testtimes");//创建一个<Node>节点 
                xe202.InnerText = model.jycs;
                XmlElement xe203 = xmldoc.CreateElement("cyds");//创建一个<Node>节点 
                xe203.InnerText = model.cyds;
                XmlElement xe204 = xmldoc.CreateElement("cysx");//创建一个<Node>节点 
                xe204.InnerText = model.cysx;
                //XmlElement xe205 = xmldoc.CreateElement("btgclz");//创建一个<Node>节点 
                //xe205.InnerText = model.btgclz;
                XmlElement xe206 = xmldoc.CreateElement("cs");//创建一个<Node>节点 
                xe206.InnerText = model.cs;
                XmlElement xe207 = xmldoc.CreateElement("zs");//创建一个<Node>节点 
                xe207.InnerText = model.zs;
                XmlElement xe208 = xmldoc.CreateElement("jzgl");//创建一个<Node>节点 
                xe208.InnerText = model.zgl;
                XmlElement xe209 = xmldoc.CreateElement("gxsxs");//创建一个<Node>节点 
                xe209.InnerText = model.gxsxs;
                XmlElement xe210 = xmldoc.CreateElement("zsgl");//创建一个<Node>节点 
                xe210.InnerText = model.zsgl;
                XmlElement xe211 = xmldoc.CreateElement("yw");//创建一个<Node>节点 
                xe211.InnerText = model.yw;
                XmlElement xe212 = xmldoc.CreateElement("glxzxs");//创建一个<Node>节点 
                xe212.InnerText = model.glxzxs;
                XmlElement xe213 = xmldoc.CreateElement("jsgl");//创建一个<Node>节点 
                xe213.InnerText = model.jsgl;
                XmlElement xe214 = xmldoc.CreateElement("btgd");//创建一个<Node>节点 
                xe214.InnerText = model.btgd;
                XmlElement xe215 = xmldoc.CreateElement("dqyl");//创建一个<Node>节点 
                xe215.InnerText = model.dqyl;
                XmlElement xe216 = xmldoc.CreateElement("xdsd");//创建一个<Node>节点 
                xe216.InnerText = model.xdsd;
                XmlElement xe217 = xmldoc.CreateElement("hjwd");//创建一个<Node>节点 
                xe217.InnerText = model.hjwd;
                XmlElement xe218 = xmldoc.CreateElement("sjxl");//创建一个<Node>节点 
                xe218.InnerText = model.sjxl;
                XmlElement xe219 = xmldoc.CreateElement("nl");//创建一个<Node>节点 
                xe219.InnerText = model.nl;
                XmlElement xe220 = xmldoc.CreateElement("jczt");//创建一个<Node>节点 
                xe220.InnerText = model.jczt;
                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                //xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe2.AppendChild(xe210);
                xe2.AppendChild(xe211);
                xe2.AppendChild(xe212);
                xe2.AppendChild(xe213);
                xe2.AppendChild(xe214);
                xe2.AppendChild(xe215);
                xe2.AppendChild(xe216);
                xe2.AppendChild(xe217);
                xe2.AppendChild(xe218);
                xe2.AppendChild(xe219);
                xe2.AppendChild(xe220);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("GC004", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行写入JZJS过程数据失败，原因：" + er.Message, "JZJS过程数据", 3);
            }
        }
        #endregion

        #region 贵州六盘水联网
        public void writeSelfCheckDZHJ_GZ(DZHJselfcheckGZ model, out string code, out string message)
        {
            code = "";
            message = "";
            try
            {
                XmlDocument xmldoc;
                XmlElement xmlelem;
                xmldoc = new XmlDocument();
                xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("root");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("head");//创建一个<Node>节点
                XmlElement xe20 = xmldoc.CreateElement("body");//创建一个<Node>节点
                XmlElement xe2 = xmldoc.CreateElement("vehispara");//创建一个<Node>节点

                XmlElement xe101 = xmldoc.CreateElement("organ");//创建一个<Node>节点 
                xe101.InnerText = Organ;
                XmlElement xe102 = xmldoc.CreateElement("jkxlh");//创建一个<Node>节点 
                xe102.InnerText = Jkxlh;
                XmlElement xe103 = xmldoc.CreateElement("jkid");//创建一个<Node>节点 
                xe103.InnerText = "ZJ000";
                xe1.AppendChild(xe101);
                xe1.AppendChild(xe102);
                xe1.AppendChild(xe103);

                XmlElement xe201 = xmldoc.CreateElement("tsno");//创建一个<Node>节点 
                xe201.InnerText = model.tsno;
                XmlElement xe202 = xmldoc.CreateElement("testlineno");//创建一个<Node>节点 
                xe202.InnerText = model.testlineno;
                XmlElement xe203 = xmldoc.CreateElement("zjlx");//创建一个<Node>节点 
                xe203.InnerText = model.zjlx;
                XmlElement xe204 = xmldoc.CreateElement("jcrq");//创建一个<Node>节点 
                xe204.InnerText = model.jcrq;
                XmlElement xe205 = xmldoc.CreateElement("jckssj");//创建一个<Node>节点 
                xe205.InnerText = model.jckssj;
                XmlElement xe206 = xmldoc.CreateElement("jcjssj");//创建一个<Node>节点 
                xe206.InnerText = model.jcjssj;
                XmlElement xe207 = xmldoc.CreateElement("txjc");//创建一个<Node>节点 
                xe207.InnerText = model.txjc;
                XmlElement xe208 = xmldoc.CreateElement("hjwd");//创建一个<Node>节点 
                xe208.InnerText = model.hjwd;
                XmlElement xe209 = xmldoc.CreateElement("yqwd");//创建一个<Node>节点 
                xe209.InnerText = model.yqwd;
                XmlElement xe210 = xmldoc.CreateElement("hjsd");//创建一个<Node>节点 
                xe210.InnerText = model.hjsd;
                XmlElement xe211 = xmldoc.CreateElement("yqsd");//创建一个<Node>节点 
                xe211.InnerText = model.yqsd;
                XmlElement xe212 = xmldoc.CreateElement("hjqy");//创建一个<Node>节点 
                xe210.InnerText = model.hjqy;
                XmlElement xe213 = xmldoc.CreateElement("yqqy");//创建一个<Node>节点 
                xe211.InnerText = model.yqqy;
                XmlElement xe214 = xmldoc.CreateElement("jcjg");//创建一个<Node>节点 
                xe212.InnerText = model.jcjg;

                xe2.AppendChild(xe201);
                xe2.AppendChild(xe202);
                xe2.AppendChild(xe203);
                xe2.AppendChild(xe204);
                xe2.AppendChild(xe205);
                xe2.AppendChild(xe206);
                xe2.AppendChild(xe207);
                xe2.AppendChild(xe208);
                xe2.AppendChild(xe209);
                xe2.AppendChild(xe210);
                xe2.AppendChild(xe211);
                xe2.AppendChild(xe212);
                xe2.AppendChild(xe213);
                xe2.AppendChild(xe214);
                xe20.AppendChild(xe2);

                root.AppendChild(xe1);
                root.AppendChild(xe20);

                FileOpreate.SaveLog("ZJ000/9/dzhj", "SEND", 3);
                string receiveXml = HttpUtility.UrlDecode(outlineservice.write(DataChange.ConvertXmlToString(xmldoc)));
                FileOpreate.SaveLog(receiveXml, "RECEIVED", 3);

                ReadACKString(receiveXml, out code, out message);
            }
            catch (Exception er)
            {
                message = er.Message;
                FileOpreate.SaveLog("执行发送电子环境自检失败，原因：" + er.Message, "电子环境自检", 3);
            }
        }
        #endregion

        #region 消息处理
        /// <summary>
        /// 解析查询返回消息
        /// </summary>
        /// <param name="xmlstring">返回的全部消息</param>
        /// <param name="result">查询结果</param>
        /// <param name="info">查询结果消息</param>
        /// <returns>查询成功时返回DataTable，反之返回null</returns>
        public DataTable ReadSeachDatatable(string xmlstring, out string info)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = DataChange.CXmlToDataSet(xmlstring);
                if (ds.Tables["head"].Rows[0]["code"].ToString() != "1")
                {
                    info = ds.Tables[0].Rows[0]["message"].ToString();
                    return null;
                }
                else
                {
                    StringBuilder str = new StringBuilder();
                    str.AppendLine(xmlstring);
                    int iBegin = str.ToString().IndexOf(@"<body>");
                    int iEnd = str.ToString().IndexOf(@"</body>");
                    if (iBegin == -1 || iEnd == -1 || iBegin >= iEnd)
                    {
                        info = "查询成功，查询结果为空！";
                        return null;
                    }
                    string temp = str.ToString().Substring(iBegin, iEnd - iBegin + 7);
                    info = "success";
                    return DataChange.CXmlToDatatTable(temp);
                }
            }
            catch (Exception er)
            {
                info = "消息解析失败";
                FileOpreate.SaveLog("解析失败原因：" + er.Message, "查询消息解析失败", 3);
                return null;
            }
        }

        /// <summary>
        /// 解析写入返回消息
        /// </summary>
        /// <param name="xmlstring">返回的全部消息</param>
        /// <param name="result">写入是否成功</param>
        /// <param name="info">写入操作返回</param>
        public void ReadACKString(string xmlstring, out string result, out string info)
        {
            try
            {
                DataSet ds = new DataSet();
                StringReader stream = new StringReader(xmlstring);
                XmlTextReader reader = new XmlTextReader(stream);
                ds.ReadXml(reader);
                DataTable dt1 = ds.Tables["head"];
                result = dt1.Rows[0]["code"].ToString();
                info = dt1.Rows[0]["message"].ToString();
            }
            catch (Exception er)
            {
                result = "";
                info = "消息解析失败";
                FileOpreate.SaveLog("解析失败原因：" + er.Message, "写入消息解析失败", 3);
            }
        }
        #endregion
    }

    public class DataChange
    {
        // 对中文进行UTF8编码
        public static String encodeUTF8(string xmlDoc)
        {
            Encoding utf8 = Encoding.UTF8;
            string str = "";
            try
            {
                str = HttpUtility.UrlEncode(xmlDoc, utf8);
            }
            catch
            { }
            return str;
        }

        // 将XmlDocument转化为string  
        public static string ConvertXmlToString(XmlDocument xmlDoc)
        {
            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, null);
            writer.Formatting = Formatting.Indented;
            xmlDoc.Save(writer);
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            stream.Position = 0;
            string xmlString = sr.ReadToEnd();
            sr.Close();
            stream.Close();
            string newstring = xmlString.Replace("xml version=\"1.0\"", "xml version=\"1.0\" encoding=\"GBK\"");
            return newstring;
        }

        /// <summary>
        /// 将Xml内容字符串转换成DataSet对象
        /// </summary>
        /// <param >Xml内容字符串</param>
        /// <returns>DataSet对象</returns>
        public static DataSet CXmlToDataSet(string xmlStr)
        {
            if (!string.IsNullOrEmpty(xmlStr))
            {
                StringReader StrStream = null;
                XmlTextReader Xmlrdr = null;
                try
                {
                    DataSet ds = new DataSet();
                    //读取字符串中的信息
                    StrStream = new StringReader(xmlStr);
                    //获取StrStream中的数据
                    Xmlrdr = new XmlTextReader(StrStream);
                    //ds获取Xmlrdr中的数据                
                    ds.ReadXml(Xmlrdr);
                    return ds;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    //释放资源
                    if (Xmlrdr != null)
                    {
                        Xmlrdr.Close();
                        StrStream.Close();
                        StrStream.Dispose();
                    }
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 将Xml字符串转换成DataTable对象
        /// </summary>
        /// <param >Xml字符串</param>
        /// <param >Table表索引</param>
        /// <returns>DataTable对象</returns>
        public static DataTable CXmlToDatatTable(string xmlStr, int tableIndex)
        {
            return CXmlToDataSet(xmlStr).Tables[tableIndex];
        }

        /// <summary>
        /// 将Xml字符串转换成DataTable对象
        /// </summary>
        /// <param >Xml字符串</param>
        /// <returns>DataTable对象</returns>
        public static DataTable CXmlToDatatTable(string xmlStr)
        {
            return CXmlToDataSet(xmlStr).Tables[0];
        }

        /// <summary>
        /// 若字符串尾部为英文逗号，则删除
        /// </summary>
        /// <param name="input_str">字符串</param>
        /// <returns></returns>
        public static string delete_comma(string input_str)
        {
            if (input_str.EndsWith(","))
                return input_str.Substring(0, input_str.Length - 1);
            else
                return input_str;
        }
    }

    #region 联网用到的数据类
    //检测过程开始——KS001
    public class ProjectStart
    {
        public string jylsh;
        public string jycs;
        public string jczbh;
        public string jcgwh;
        public string hphm;
        public string hpzl;
        public string ycy;
        public string jcczy;
        public string jcff;
        public string ljxslc;
        public string dqsj;
        public string jcbbh;
        public ProjectStart(string jylsh,string jycs,string jczbh,string jcgwh,string hphm,string hpzl,string ycy,string jcczy,string jcff,string ljxslc,string dqsj,string jcbbh)
        {
            this.jylsh = jylsh;
            this.jycs = jycs;
            this.jczbh = jczbh;
            this.jcgwh = jcgwh;
            this.hphm = hphm;
            this.hpzl = hpzl;
            this.ycy = ycy;
            this.jcczy = jcczy;
            switch (jcff)
            {
                case "SDS":
                    this.jcff = "1";
                    break;
                case "ASM":
                    this.jcff = "2";
                    break;
                case "VMAS":
                    this.jcff = "3";
                    break;
                case "JZJS":
                    this.jcff = "4";
                    break;
                case "ZYJS":
                    this.jcff = "5";
                    break;
                case "SDSM":
                    this.jcff = "1";
                    break;
            }
            this.ljxslc = ljxslc;
            this.dqsj = dqsj;
            this.jcbbh = jcbbh;
        }
    }

    //检测照片——ZP002
    public class CapturePicture
    {
        public string jylsh;
        public string jycs;
        public string jczbh;
        public string jcgwh;
        public string zpbh;
        public string photo_date;
        public CapturePicture(string jylsh, string jycs, string jczbh, string jcgwh, string zpbh, string photo_date)
        {
            this.jylsh = jylsh;
            this.jycs = jycs;
            this.jczbh = jczbh;
            this.jcgwh = jcgwh;
            this.zpbh = zpbh;
            this.photo_date = DateTime.Parse(photo_date).ToString("yyyy-MM-dd HH:mm:ss");
        }
    }

    //检测过程结束——JS001
    public class ProjectStop
    {
        public string jylsh;
        public string jycs;
        public string jczbh;
        public string jcgwh;
        public string hphm;
        public string hpzl;
        public string pdjg;
        public ProjectStop(string jylsh, string jycs, string jczbh, string jcgwh, string hphm, string hpzl, string pdjg)
        {
            this.jylsh = jylsh;
            this.jycs = jycs;
            this.jczbh = jczbh;
            this.jcgwh = jcgwh;
            this.hphm = hphm;
            this.hpzl = hpzl;
            this.pdjg = pdjg;
        }
    }

    //瞬态结果数据——JC001
    public class VmasResult
    {
        public string jylsh;
        public string jycs;
        public string sbrzbm;
        public string sbmc;
        public string sbxh;
        public string sbzzc;
        public string dpcgj;
        public string pqfxy;
        public string wd;
        public string dqy;
        public string xdsd;
        public string coxz;
        public string coclz;
        public string copdjg;
        public string hcxz;
        public string hcclz;
        public string hcpdjg;
        public string noxz;
        public string noclz;
        public string nopdjg;
        public string hcnoxz;
        public string hcnoclz;
        public string hcnopdjg;
        public string csljccsj;
        public string xslc;
        public string PDJG;
        public VmasResult(DataRow result_data)
        {
            this.jylsh = result_data["JYLSH"].ToString();
            this.jycs = result_data["JYCS"].ToString();
            this.sbrzbm = result_data["SBRZBM"].ToString();
            this.sbmc = result_data["SBMC"].ToString();
            this.sbxh = result_data["SBXH"].ToString();
            this.sbzzc = result_data["SBZZC"].ToString();
            this.dpcgj = result_data["CGJXH"].ToString();
            this.pqfxy = result_data["FXYXH"].ToString();
            this.wd = result_data["WD"].ToString();
            this.dqy = result_data["DQY"].ToString();
            this.xdsd = result_data["SD"].ToString();
            this.coxz = result_data["COXZ"].ToString();
            this.coclz = result_data["COZL"].ToString();
            this.copdjg = result_data["COPD"].ToString();
            this.hcxz = result_data["HCXZ"].ToString();
            this.hcclz = result_data["HCZL"].ToString();
            this.hcpdjg = result_data["HCPD"].ToString();
            this.noxz = result_data["NOXXZ"].ToString();
            this.noclz = result_data["NOXZL"].ToString();
            this.nopdjg = result_data["NOXPD"].ToString();
            this.hcnoxz = result_data["NOXXZ"].ToString();
            //this.hcnoclz = result_data["NOXZL"].ToString();
            this.hcnoclz = (float.Parse(result_data["NOXZL"].ToString()) + float.Parse(result_data["HCZL"].ToString())).ToString();
            this.hcnopdjg = result_data["NOXPD"].ToString();
            this.csljccsj = result_data["CSLJCCSJ"].ToString();
            this.xslc = result_data["XSLC"].ToString();
            this.PDJG = result_data["ZHPD"].ToString();
        }
    }

    //稳态结果数据——JC002
    public class ASMResult
    {
        public string jylsh;
        public string jycs;
        public string sbrzbm;
        public string sbmc;
        public string sbxh;
        public string sbzzc;
        public string dpcgj;
        public string pqfxy;
        public string wd;
        public string dqy;
        public string xdsd;
        public string hc5025xz;
        public string hc5025clz;
        public string hc5025pdjg;
        public string co5025xz;
        public string co5025clz;
        public string co5025pdjg;
        public string no5025xz;
        public string no5025clz;
        public string no5025pdjg;
        public string fdjzs5025;
        public string fdjyw5025;
        public string hc2540xz;
        public string hc2540clz;
        public string hc2540pdjg;
        public string co2540xz;
        public string co2540clz;
        public string co2540pdjg;
        public string no2540xz;
        public string no2540clz;
        public string no2540pdjg;
        public string fdjzs2540;
        public string fdjyw2540;
        public string jzzgl5025;
        public string jzzgl2540;
        public string PDJG;
        public bool _stop_at_5025;
        public ASMResult(DataRow result_data, bool _stop_at_5025)
        {
            this.jylsh = result_data["JYLSH"].ToString();
            this.jycs = result_data["JYCS"].ToString();
            this.sbrzbm = result_data["SBRZBM"].ToString();
            this.sbmc = result_data["SBMC"].ToString();
            this.sbxh = result_data["SBXH"].ToString();
            this.sbzzc = result_data["SBZZC"].ToString();
            this.dpcgj = result_data["CGJXH"].ToString();
            this.pqfxy = result_data["FXYXH"].ToString();
            this.wd = result_data["WD"].ToString();
            this.dqy = result_data["DQY"].ToString();
            this.xdsd = result_data["SD"].ToString();
            this.hc5025xz = result_data["HC25XZ"].ToString();
            this.hc5025clz = result_data["HC25CLZ"].ToString();
            this.hc5025pdjg = result_data["HC25PD"].ToString();
            this.co5025xz = result_data["CO25XZ"].ToString();
            this.co5025clz = result_data["CO25CLZ"].ToString();
            this.co5025pdjg = result_data["CO25PD"].ToString();
            this.no5025xz = result_data["NOX25XZ"].ToString();
            this.no5025clz = result_data["NOX25CLZ"].ToString();
            this.no5025pdjg = result_data["NOX25PD"].ToString();
            this.fdjzs5025 = result_data["FDJZS5025"].ToString();
            this.fdjyw5025 = result_data["FDJYW5025"].ToString();
            this.jzzgl5025 = result_data["JZZGL5025"].ToString();
            if (_stop_at_5025)
            {
                this.hc2540xz = "";
                this.hc2540clz = "";
                this.hc2540pdjg = "";
                this.co2540xz = "";
                this.co2540clz = "";
                this.co2540pdjg = "";
                this.no2540xz = "";
                this.no2540clz = "";
                this.no2540pdjg = "";
                this.fdjzs2540 = "";
                this.fdjyw2540 = "";
                this.jzzgl2540 = "";
            }
            else
            {
                this.hc2540xz = result_data["HC40XZ"].ToString();
                this.hc2540clz = result_data["HC40CLZ"].ToString();
                this.hc2540pdjg = result_data["HC40PD"].ToString();
                this.co2540xz = result_data["CO40XZ"].ToString();
                this.co2540clz = result_data["CO40CLZ"].ToString();
                this.co2540pdjg = result_data["CO40PD"].ToString();
                this.no2540xz = result_data["NOX40XZ"].ToString();
                this.no2540clz = result_data["NOX40CLZ"].ToString();
                this.no2540pdjg = result_data["NOX40PD"].ToString();
                this.fdjzs2540 = result_data["FDJZS2540"].ToString();
                this.fdjyw2540 = result_data["FDJYW2540"].ToString();
                this.jzzgl2540 = result_data["JZZGL2540"].ToString();
            }
            this.PDJG = result_data["ZHPD"].ToString();
            this._stop_at_5025 = _stop_at_5025;
        }
    }

    //双怠速结果数据——JC003
    public class SDSResult
    {
        public string jylsh;
        public string jycs;
        public string sbrzbm;
        public string sbmc;
        public string sbxh;
        public string sbzzc;
        public string pqfxy;
        public string wd;
        public string dqy;
        public string xdsd;
        public string glkqxsz;
        public string glkqxssx;
        public string glkqsxxx;
        public string glkqxspdjg;
        public string ddscoxz;
        public string ddscoz;
        public string ddscopdjg;
        public string ddshcxz;
        public string ddshcz;
        public string ddshcpdjg;
        public string fdjdszs;
        public string ddsjywd;
        public string gdscoxz;
        public string gdscoz;
        public string gdscopdjg;
        public string gdshcz;
        public string gdshcxz;
        public string gdshcpdjg;
        public string gdszs;
        public string gdsjywd;
        public string PDJG;
        public SDSResult(DataRow result_data)
        {
            this.jylsh = result_data["JYLSH"].ToString();
            this.jycs = result_data["JYCS"].ToString();
            this.sbrzbm = result_data["SBRZBM"].ToString();
            this.sbmc = result_data["SBMC"].ToString();
            this.sbxh = result_data["SBXH"].ToString();
            this.sbzzc = result_data["SBZZC"].ToString();
            this.pqfxy = result_data["FXYXH"].ToString();
            this.wd = result_data["WD"].ToString();
            this.dqy = result_data["DQY"].ToString();
            this.xdsd = result_data["SD"].ToString();
            this.glkqxsz = result_data["LAMDAHIGHCLZ"].ToString();
            this.glkqxssx = result_data["GLKQXSSX"].ToString();
            this.glkqsxxx = result_data["GLKQXSXX"].ToString();
            this.glkqxspdjg = result_data["LAMDAHIGHPD"].ToString();
            this.ddscoxz = result_data["COLOWXZ"].ToString();
            this.ddscoz = result_data["COLOWCLZ"].ToString();
            this.ddscopdjg = result_data["COLOWPD"].ToString();
            this.ddshcxz = result_data["HCLOWXZ"].ToString();
            this.ddshcz = result_data["HCLOWCLZ"].ToString();
            this.ddshcpdjg = result_data["HCLOWPD"].ToString();
            this.fdjdszs = result_data["ZSLOW"].ToString();
            this.ddsjywd = result_data["JYWDLOW"].ToString();
            this.gdscoxz = result_data["COHIGHXZ"].ToString();
            this.gdscoz = result_data["COHIGHCLZ"].ToString();
            this.gdscopdjg = result_data["COHIGHPD"].ToString();
            this.gdshcz = result_data["HCHIGHCLZ"].ToString();
            this.gdshcxz = result_data["HCHIGHXZ"].ToString();
            this.gdshcpdjg = result_data["HCHIGHPD"].ToString();
            this.gdszs = result_data["ZSHIGH"].ToString();
            this.gdsjywd = result_data["JYWDHIGH"].ToString();
            this.PDJG = result_data["ZHPD"].ToString();
        }
    }
    
    //加载减速结果数据——JC004
    public class JZJSResult
    {
        public string jylsh;
        public string jycs;
        public string sbrzbm;
        public string sbmc;
        public string sbxh;
        public string sbzzc;
        public string dpcgj;
        public string wd;
        public string dqy;
        public string xdsd;
        public string velmaxhp;
        public string velmaxhpzs;
        public string zdlbgl;
        public string zdlbglxz;
        public string zdlbglzs;
        public string zdlbglpdjg;
        public string ydxz;
        public string ydpdjg;
        public string k100;
        public string k90;
        public string k80;
        public string raterevup;
        public string raterevdown;
        public string PDJG;
        public JZJSResult(DataRow result_data)
        {
            this.jylsh = result_data["JYLSH"].ToString();
            this.jycs = result_data["JYCS"].ToString();
            this.sbrzbm = result_data["SBRZBM"].ToString();
            this.sbmc = result_data["SBMC"].ToString();
            this.sbxh = result_data["SBXH"].ToString();
            this.sbzzc = result_data["SBZZC"].ToString();
            this.dpcgj = result_data["CGJXH"].ToString();
            this.wd = result_data["WD"].ToString();
            this.dqy = result_data["DQY"].ToString();
            this.xdsd = result_data["SD"].ToString();
            this.velmaxhp = result_data["VELMAXHP"].ToString();
            this.velmaxhpzs = result_data["VELMAXHPZS"].ToString();
            this.zdlbgl = result_data["MAXLBGL"].ToString();
            this.zdlbglxz = result_data["GLXZ"].ToString();
            this.zdlbglzs = result_data["MAXLBZS"].ToString();
            this.zdlbglpdjg = result_data["GLPD"].ToString();
            this.ydxz = result_data["YDXZ"].ToString();
            if(result_data["NKPD"].ToString()=="不合格"|| result_data["EKPD"].ToString() == "不合格"|| result_data["HKPD"].ToString() == "不合格")
                this.ydpdjg = "不合格";
            else
                this.ydpdjg = "合格";
            this.k100 = result_data["HK"].ToString();
            this.k90 = result_data["NK"].ToString();
            this.k80 = result_data["EK"].ToString();
            this.raterevup = result_data["RATEREVUP"].ToString();
            this.raterevdown = result_data["RATEREVDOWN"].ToString();
            this.PDJG = result_data["ZHPD"].ToString();
        }
    }

    //不透光结果数据——JC005（滤纸数据与不透光一样，标识为JC006）
    public class ZYJSResult
    {
        public string jylsh;
        public string jycs;
        public string sbrzbm;
        public string sbmc;
        public string sbxh;
        public string sbzzc;
        public string wd;
        public string dqy;
        public string xdsd;
        public string fdjdszs;
        public string smoke4;
        public string smoke3;
        public string smoke2;
        public string smoke1;
        public string pjz;
        public string pdjg;
        public string xz;
        public ZYJSResult(DataRow result_data)
        {
            this.jylsh = result_data["JYLSH"].ToString();
            this.jycs = result_data["JYCS"].ToString();
            this.sbrzbm = result_data["SBRZBM"].ToString();
            this.sbmc = result_data["SBMC"].ToString();
            this.sbxh = result_data["SBXH"].ToString();
            this.sbzzc = result_data["SBZZC"].ToString();
            this.wd = result_data["WD"].ToString();
            this.dqy = result_data["DQY"].ToString();
            this.xdsd = result_data["SD"].ToString();
            this.fdjdszs = result_data["DSZS"].ToString();
            this.smoke1 = result_data["THIRDDATA"].ToString();
            this.smoke2 = result_data["SECONDDATA"].ToString();
            this.smoke3 = result_data["FIRSTDATA"].ToString();
            this.smoke4 = result_data["FOURTHDATA"].ToString();
            this.pjz = result_data["AVERAGEDATA"].ToString();
            this.pdjg = result_data["ZHPD"].ToString();
            this.xz = result_data["YDXZ"].ToString();
        }
    }

    //瞬态过程数据——GC001
    public class VmasProcess
    {
        public string jylsh;
        public string jycs;
        public string cyds;
        public string cysx;
        public string hcclz;
        public string noclz;
        public string cs;
        public string bzss;
        public string zs;
        public string coclz;
        public string co2clz;
        public string o2clz;
        public string xsxzxs;
        public string sdxzxs;
        public string jsgl;
        public string zsgl;
        public string jzgl;
        public string hjwd;
        public string dqyl;
        public string xdsd;
        public string yw;
        public string lljo2;
        public string hjo2;
        public string lljsjll;
        public string lljbzll;
        public string qcwqll;
        public string xsb;
        public string lljwd;
        public string lljqy;
        public string hcpfzl;
        public string nopfzl;
        public string copfzl;
        public string jczt;
        public string sjxl;
        public string fxyglyl;
        public string co2pfzl;
        public string nl;
        public VmasProcess(DataRow process_data)
        {
            jylsh = process_data["JYLSH"].ToString();
            jycs = process_data["JYCS"].ToString();
            cyds = "195";
            cysx = DataChange.delete_comma(process_data["MMSX"].ToString());
            hcclz = DataChange.delete_comma(process_data["MMHC"].ToString());
            noclz = DataChange.delete_comma(process_data["MMNO"].ToString());
            cs = DataChange.delete_comma(process_data["MMCS"].ToString());
            bzss = DataChange.delete_comma(process_data["MMBZCS"].ToString());
            zs = DataChange.delete_comma(process_data["MMZS"].ToString());
            coclz = DataChange.delete_comma(process_data["MMCO"].ToString());
            co2clz = DataChange.delete_comma(process_data["MMCO2"].ToString());
            o2clz = DataChange.delete_comma(process_data["MMO2"].ToString());
            xsxzxs = DataChange.delete_comma(process_data["MMXSXZ"].ToString());
            sdxzxs = DataChange.delete_comma(process_data["MMSDXZ"].ToString());
            jsgl = DataChange.delete_comma(process_data["MMJSGL"].ToString());
            zsgl = DataChange.delete_comma(process_data["MMZSGL"].ToString());
            jzgl = DataChange.delete_comma(process_data["MMJZGL"].ToString());
            hjwd = DataChange.delete_comma(process_data["MMWD"].ToString());
            dqyl = DataChange.delete_comma(process_data["MMDQY"].ToString());
            xdsd = DataChange.delete_comma(process_data["MMSD"].ToString());
            yw = DataChange.delete_comma(process_data["MMYW"].ToString());
            lljo2 = DataChange.delete_comma(process_data["MMXSO2"].ToString());
            hjo2 = DataChange.delete_comma(process_data["MMHJO2"].ToString());
            lljsjll = DataChange.delete_comma(process_data["MMSJLL"].ToString());
            lljbzll = DataChange.delete_comma(process_data["MMBZLL"].ToString());
            qcwqll = DataChange.delete_comma(process_data["MMWQLL"].ToString());
            xsb = DataChange.delete_comma(process_data["MMXSB"].ToString());
            lljwd = DataChange.delete_comma(process_data["MMLLJWD"].ToString());
            lljqy = DataChange.delete_comma(process_data["MMLLJYL"].ToString());
            hcpfzl = DataChange.delete_comma(process_data["MMHCZL"].ToString());
            nopfzl = DataChange.delete_comma(process_data["MMNOZL"].ToString());
            copfzl = DataChange.delete_comma(process_data["MMCOZL"].ToString());
            jczt = DataChange.delete_comma(process_data["MMLB"].ToString()).Replace("1", "2");
            string temp_mmtime = DataChange.delete_comma(process_data["MMTIME"].ToString());
            string[] length = temp_mmtime.Split(',');
            for (int i = 0; i < length.Length; i++)
            {
                sjxl = sjxl + DateTime.Parse(length[i]).ToString("yyyy-MM-dd HH:mm:ss") + ",";
            }
            sjxl = sjxl.Substring(0, sjxl.Length - 1);
            fxyglyl = DataChange.delete_comma(process_data["MMGLYL"].ToString());
            co2pfzl = DataChange.delete_comma(process_data["MMCO2ZL"].ToString());
            nl = DataChange.delete_comma(process_data["MMNJ"].ToString());
        }
    }

    //稳态过程数据——GC002
    public class ASMProcess
    {
        public string jylsh;
        public string jycs;
        public string cyds;
        public string cysx;
        public string hcclz;
        public string noclz;
        public string cs;
        public string zs;
        public string coclz;
        public string co2clz;
        public string o2clz;
        public string xsxzxs;
        public string sdxzxs;
        public string jsgl;
        public string zsgl;
        public string jzgl;
        public string hjwd;
        public string dqyl;
        public string xdsd;
        public string yw;
        public string jczt;
        public string sjxl;
        public string nl;
        public string scfz;
        public ASMProcess(DataRow process_data)
        {
            jylsh = process_data["JYLSH"].ToString();
            jycs = process_data["JYCS"].ToString();
            cyds = process_data["CYDS"].ToString();
            cysx = DataChange.delete_comma(process_data["MMSX"].ToString());
            hcclz = DataChange.delete_comma(process_data["MMHC"].ToString());
            noclz = DataChange.delete_comma(process_data["MMNO"].ToString());
            cs = DataChange.delete_comma(process_data["MMCS"].ToString());
            zs = DataChange.delete_comma(process_data["MMZS"].ToString());
            coclz = DataChange.delete_comma(process_data["MMCO"].ToString());
            co2clz = DataChange.delete_comma(process_data["MMCO2"].ToString());
            o2clz = DataChange.delete_comma(process_data["MMO2"].ToString());
            xsxzxs = DataChange.delete_comma(process_data["MMXSXZ"].ToString());
            sdxzxs = DataChange.delete_comma(process_data["MMSDXZ"].ToString());
            jsgl = DataChange.delete_comma(process_data["MMJSGL"].ToString());
            zsgl = DataChange.delete_comma(process_data["MMZSGL"].ToString());
            jzgl = DataChange.delete_comma(process_data["MMGL"].ToString());
            nl = DataChange.delete_comma(process_data["MMNL"].ToString());
            hjwd = DataChange.delete_comma(process_data["MMWD"].ToString());
            dqyl = DataChange.delete_comma(process_data["MMDQY"].ToString());
            xdsd = DataChange.delete_comma(process_data["MMSD"].ToString());
            yw = DataChange.delete_comma(process_data["MMYW"].ToString());
            jczt = DataChange.delete_comma(process_data["MMZT"].ToString());
            sjxl = DataChange.delete_comma(process_data["MMTIME"].ToString());
            //scfz = DataChange.delete_comma(process_data["SCFZ"].ToString());
            string[] length = sjxl.Split(',');
            sjxl = "";
            for (int i = 0; i < length.Length; i++)
            {
                sjxl = sjxl + DateTime.Parse(length[i]).ToString("yyyy-MM-dd HH:mm:ss") + ",";
                scfz += "907,";
            }
            sjxl = sjxl.Substring(0, sjxl.Length - 1);
            scfz = scfz.Substring(0, scfz.Length - 1);
        }
    }

    //双怠速过程数据——GC003
    public class SDSProcess
    {
        public string jylsh;
        public string jycs;
        public string cyds;
        public string cysx;
        public string hcclz;
        public string zs;
        public string coclz;
        public string glkqxs;
        public string yw;
        public string co2clz;
        public string o2clz;
        public string hjwd;
        public string dqyl;
        public string xdsd;
        public string sjxl;
        public string jczt;
        public SDSProcess(DataRow process_data)
        {
            if (int.Parse(cyds = process_data["CYDS"].ToString()) > 200)
            {
                jylsh = process_data["JYLSH"].ToString();
                jycs = process_data["JYCS"].ToString();
                cyds = process_data["CYDS"].ToString();
                cysx = "";
                hcclz = "";
                zs = "";
                coclz = "";
                glkqxs = "";
                yw = "";
                co2clz = "";
                o2clz = "";
                hjwd = "";
                dqyl = "";
                xdsd = "";
                sjxl = "";
                jczt = "";
                string lb = process_data["MMLB"].ToString();
                //string[] cysx_temp = process_data["MMSX"].ToString().Split(',');
                string[] hcclz_temp = process_data["MMHC"].ToString().Split(',');
                string[] zs_temp = process_data["MMZS"].ToString().Split(',');
                string[] coclz_temp = process_data["MMCO"].ToString().Split(',');
                string[] glkqxs_temp = process_data["MMLAMDA"].ToString().Split(',');
                string[] yw_temp = process_data["MMYW"].ToString().Split(',');
                string[] co2clz_temp = process_data["MMCO2"].ToString().Split(',');
                string[] o2clz_temp = process_data["MMO2"].ToString().Split(',');
                string[] hjwd_temp = process_data["MMWD"].ToString().Split(',');
                string[] dqyl_temp = process_data["MMDQY"].ToString().Split(',');
                string[] xdsd_temp = process_data["MMSD"].ToString().Split(',');
                string[] sjxl_temp = process_data["MMTIME"].ToString().Split(',');
                string[] jczt_temp = lb.Split(',');

                int first_one = Array.IndexOf(jczt_temp,"1");
                int last_zero = first_one - 1;
                int first_two = Array.IndexOf(jczt_temp, "2");
                int last_one = first_two - 1;
                int first_three = Array.IndexOf(jczt_temp, "3");
                int last_two = first_three - 1;
                int first_four = Array.IndexOf(jczt_temp, "4");
                int last_three = first_four - 1;
                int total_cyds = 0;
                
                //类别0数量大于30个时取30个，其他全取
                if (last_zero > 30)
                {
                    for (int i = last_zero - 29; i <= last_zero; i++)
                    {
                        //cysx = cysx + cysx_temp[i] + ",";
                        hcclz = hcclz + hcclz_temp[i] + ",";
                        zs = zs + zs_temp[i] + ",";
                        coclz = coclz + coclz_temp[i] + ",";
                        glkqxs = glkqxs + glkqxs_temp[i] + ",";
                        yw = yw + yw_temp[i] + ",";
                        co2clz = co2clz + co2clz_temp[i] + ",";
                        o2clz = o2clz + o2clz_temp[i] + ",";
                        hjwd = hjwd + hjwd_temp[i] + ",";
                        dqyl = dqyl + dqyl_temp[i] + ",";
                        xdsd = xdsd + xdsd_temp[i] + ",";
                        sjxl = sjxl + DateTime.Parse(sjxl_temp[i]).ToString("yyyy-MM-dd HH:mm:ss") + ",";
                        jczt = jczt + jczt_temp[i] + ",";
                        total_cyds++;
                    }
                }
                else
                {
                    for (int i = 0; i <= last_zero; i++)
                    {
                        //cysx = cysx + cysx_temp[i] + ",";
                        hcclz = hcclz + hcclz_temp[i] + ",";
                        zs = zs + zs_temp[i] + ",";
                        coclz = coclz + coclz_temp[i] + ",";
                        glkqxs = glkqxs + glkqxs_temp[i] + ",";
                        yw = yw + yw_temp[i] + ",";
                        co2clz = co2clz + co2clz_temp[i] + ",";
                        o2clz = o2clz + o2clz_temp[i] + ",";
                        hjwd = hjwd + hjwd_temp[i] + ",";
                        dqyl = dqyl + dqyl_temp[i] + ",";
                        xdsd = xdsd + xdsd_temp[i] + ",";
                        sjxl = sjxl + DateTime.Parse(sjxl_temp[i]).ToString("yyyy-MM-dd HH:mm:ss") + ",";
                        jczt = jczt + jczt_temp[i] + ",";
                        total_cyds++;
                    }
                }
                //类别1数量大于15个时取后15个，其他全取
                if (last_one - first_one > 15)
                {
                    for (int i = last_one - 14; i <= last_one; i++)
                    {
                        //cysx = cysx + cysx_temp[i] + ",";
                        hcclz = hcclz + hcclz_temp[i] + ",";
                        zs = zs + zs_temp[i] + ",";
                        coclz = coclz + coclz_temp[i] + ",";
                        glkqxs = glkqxs + glkqxs_temp[i] + ",";
                        yw = yw + yw_temp[i] + ",";
                        co2clz = co2clz + co2clz_temp[i] + ",";
                        o2clz = o2clz + o2clz_temp[i] + ",";
                        hjwd = hjwd + hjwd_temp[i] + ",";
                        dqyl = dqyl + dqyl_temp[i] + ",";
                        xdsd = xdsd + xdsd_temp[i] + ",";
                        sjxl = sjxl + DateTime.Parse(sjxl_temp[i]).ToString("yyyy-MM-dd HH:mm:ss") + ",";
                        jczt = jczt + jczt_temp[i] + ",";
                        total_cyds++;
                    }
                }
                else
                {
                    for (int i = first_one; i <= last_one; i++)
                    {
                        //cysx = cysx + cysx_temp[i] + ",";
                        hcclz = hcclz + hcclz_temp[i] + ",";
                        zs = zs + zs_temp[i] + ",";
                        coclz = coclz + coclz_temp[i] + ",";
                        glkqxs = glkqxs + glkqxs_temp[i] + ",";
                        yw = yw + yw_temp[i] + ",";
                        co2clz = co2clz + co2clz_temp[i] + ",";
                        o2clz = o2clz + o2clz_temp[i] + ",";
                        hjwd = hjwd + hjwd_temp[i] + ",";
                        dqyl = dqyl + dqyl_temp[i] + ",";
                        xdsd = xdsd + xdsd_temp[i] + ",";
                        sjxl = sjxl + DateTime.Parse(sjxl_temp[i]).ToString("yyyy-MM-dd HH:mm:ss") + ",";
                        jczt = jczt + jczt_temp[i] + ",";
                        total_cyds++;
                    }
                }
                //类别2全取
                for (int i = first_two; i <= last_two; i++)
                {
                    if (jczt_temp[i] == "2")
                    {
                        //cysx = cysx + cysx_temp[i] + ",";
                        hcclz = hcclz + hcclz_temp[i] + ",";
                        zs = zs + zs_temp[i] + ",";
                        coclz = coclz + coclz_temp[i] + ",";
                        glkqxs = glkqxs + glkqxs_temp[i] + ",";
                        yw = yw + yw_temp[i] + ",";
                        co2clz = co2clz + co2clz_temp[i] + ",";
                        o2clz = o2clz + o2clz_temp[i] + ",";
                        hjwd = hjwd + hjwd_temp[i] + ",";
                        dqyl = dqyl + dqyl_temp[i] + ",";
                        xdsd = xdsd + xdsd_temp[i] + ",";
                        sjxl = sjxl + DateTime.Parse(sjxl_temp[i]).ToString("yyyy-MM-dd HH:mm:ss") + ",";
                        jczt = jczt + jczt_temp[i] + ",";
                        total_cyds++;
                    }
                }
                //类别3数量大于15个时取后15个，其他全取
                if (last_three - first_three > 15)
                {
                    for (int i = last_three - 14; i <= last_three; i++)
                    {
                        //cysx = cysx + cysx_temp[i] + ",";
                        hcclz = hcclz + hcclz_temp[i] + ",";
                        zs = zs + zs_temp[i] + ",";
                        coclz = coclz + coclz_temp[i] + ",";
                        glkqxs = glkqxs + glkqxs_temp[i] + ",";
                        yw = yw + yw_temp[i] + ",";
                        co2clz = co2clz + co2clz_temp[i] + ",";
                        o2clz = o2clz + o2clz_temp[i] + ",";
                        hjwd = hjwd + hjwd_temp[i] + ",";
                        dqyl = dqyl + dqyl_temp[i] + ",";
                        xdsd = xdsd + xdsd_temp[i] + ",";
                        sjxl = sjxl + DateTime.Parse(sjxl_temp[i]).ToString("yyyy-MM-dd HH:mm:ss") + ",";
                        jczt = jczt + jczt_temp[i] + ",";
                        total_cyds++;
                    }
                }
                else
                {
                    for (int i = first_three; i <= last_three; i++)
                    {
                        //cysx = cysx + cysx_temp[i] + ",";
                        hcclz = hcclz + hcclz_temp[i] + ",";
                        zs = zs + zs_temp[i] + ",";
                        coclz = coclz + coclz_temp[i] + ",";
                        glkqxs = glkqxs + glkqxs_temp[i] + ",";
                        yw = yw + yw_temp[i] + ",";
                        co2clz = co2clz + co2clz_temp[i] + ",";
                        o2clz = o2clz + o2clz_temp[i] + ",";
                        hjwd = hjwd + hjwd_temp[i] + ",";
                        dqyl = dqyl + dqyl_temp[i] + ",";
                        xdsd = xdsd + xdsd_temp[i] + ",";
                        sjxl = sjxl + DateTime.Parse(sjxl_temp[i]).ToString("yyyy-MM-dd HH:mm:ss") + ",";
                        jczt = jczt + jczt_temp[i] + ",";
                        total_cyds++;
                    }
                }
                //类别4全取
                for (int i = first_four; i < jczt_temp.Length; i++)
                {
                    if (jczt_temp[i] == "4")
                    {
                        //cysx = cysx + cysx_temp[i] + ",";
                        hcclz = hcclz + hcclz_temp[i] + ",";
                        zs = zs + zs_temp[i] + ",";
                        coclz = coclz + coclz_temp[i] + ",";
                        glkqxs = glkqxs + glkqxs_temp[i] + ",";
                        yw = yw + yw_temp[i] + ",";
                        co2clz = co2clz + co2clz_temp[i] + ",";
                        o2clz = o2clz + o2clz_temp[i] + ",";
                        hjwd = hjwd + hjwd_temp[i] + ",";
                        dqyl = dqyl + dqyl_temp[i] + ",";
                        xdsd = xdsd + xdsd_temp[i] + ",";
                        sjxl = sjxl + DateTime.Parse(sjxl_temp[i]).ToString("yyyy-MM-dd HH:mm:ss") + ",";
                        jczt = jczt + jczt_temp[i] + ",";
                        total_cyds++;
                    }
                }

                cyds = total_cyds.ToString();
                for (int i = 0; i < total_cyds; i++)
                {
                    cysx += (i + 1).ToString() + ",";
                }
                cysx = DataChange.delete_comma(cysx);
                hcclz = DataChange.delete_comma(hcclz);
                zs = DataChange.delete_comma(zs);
                coclz = DataChange.delete_comma(coclz);
                glkqxs = DataChange.delete_comma(glkqxs);
                yw = DataChange.delete_comma(yw);
                co2clz = DataChange.delete_comma(co2clz);
                o2clz = DataChange.delete_comma(o2clz);
                hjwd = DataChange.delete_comma(hjwd);
                dqyl = DataChange.delete_comma(dqyl);
                xdsd = DataChange.delete_comma(xdsd);
                sjxl = DataChange.delete_comma(sjxl);
                jczt = DataChange.delete_comma(jczt);
            }
            else
            {
                jylsh = process_data["JYLSH"].ToString();
                jycs = process_data["JYCS"].ToString();
                cyds = process_data["CYDS"].ToString();
                cysx = DataChange.delete_comma(process_data["MMSX"].ToString());
                hcclz = DataChange.delete_comma(process_data["MMHC"].ToString());
                zs = DataChange.delete_comma(process_data["MMZS"].ToString());
                coclz = DataChange.delete_comma(process_data["MMCO"].ToString());
                glkqxs = DataChange.delete_comma(process_data["MMLAMDA"].ToString());
                yw = DataChange.delete_comma(process_data["MMYW"].ToString());
                co2clz = DataChange.delete_comma(process_data["MMCO2"].ToString());
                o2clz = DataChange.delete_comma(process_data["MMO2"].ToString());
                hjwd = DataChange.delete_comma(process_data["MMWD"].ToString());
                dqyl = DataChange.delete_comma(process_data["MMDQY"].ToString());
                xdsd = DataChange.delete_comma(process_data["MMSD"].ToString());
                //sjxl = DataChange.delete_comma(process_data["MMTIME"].ToString());
                string temp_mmtime = DataChange.delete_comma(process_data["MMTIME"].ToString());
                string[] length = temp_mmtime.Split(',');
                for (int i = 0; i < length.Length; i++)
                {
                    sjxl = sjxl + DateTime.Parse(length[i]).ToString("yyyy-MM-dd HH:mm:ss") + ",";
                }
                sjxl = sjxl.Substring(0, sjxl.Length - 1);
                jczt = DataChange.delete_comma(process_data["MMLB"].ToString());
            }
        }
    }

    //加载减速过程数据——GC004
    public class JZJSProcess
    {
        public string jylsh;
        public string jycs;
        public string cyds;
        public string cysx;
        public string btgclz;
        public string cs;
        public string zs;
        public string zgl;
        public string gxsxs;
        public string zsgl;
        public string yw;
        public string glxzxs;
        public string jsgl;
        public string jczt;
        public string btgd;
        public string dqyl;
        public string xdsd;
        public string hjwd;
        public string sjxl;
        public string nl;
        public JZJSProcess(DataRow process_data)
        {
            jylsh = process_data["JYLSH"].ToString();
            jycs = process_data["JYCS"].ToString();
            cyds = process_data["CYDS"].ToString();
            cysx = DataChange.delete_comma(process_data["MMSX"].ToString());
            btgclz = DataChange.delete_comma(process_data["MMBTGD"].ToString());
            cs = DataChange.delete_comma(process_data["MMCS"].ToString());
            zs = DataChange.delete_comma(process_data["MMZS"].ToString());
            zgl = DataChange.delete_comma(process_data["MMZGL"].ToString());
            gxsxs = DataChange.delete_comma(process_data["MMK"].ToString());
            zsgl = DataChange.delete_comma(process_data["MMZSGL"].ToString());
            yw = DataChange.delete_comma(process_data["MMYW"].ToString());
            //for (int i = 0; i < zsgl.Split(',').Count(); i++)
            //{
            //    yw += "80,";
            //}
            //yw = yw.Substring(0, yw.Length - 1);
            glxzxs = DataChange.delete_comma(process_data["MMGLXZXS"].ToString());
            jsgl = DataChange.delete_comma(process_data["MMJSGL"].ToString());
            jczt = DataChange.delete_comma(process_data["MMLB"].ToString());
            btgd = DataChange.delete_comma(process_data["MMBTGD"].ToString());
            dqyl = DataChange.delete_comma(process_data["MMDQYL"].ToString());
            xdsd = DataChange.delete_comma(process_data["MMXDSD"].ToString());
            hjwd = DataChange.delete_comma(process_data["MMHJWD"].ToString());
            //sjxl = DataChange.delete_comma(process_data["MMTIME"].ToString());
            string temp_mmtime = DataChange.delete_comma(process_data["MMTIME"].ToString());
            string[] length = temp_mmtime.Split(',');
            for (int i = 0; i < length.Length; i++)
            {
                sjxl = sjxl + DateTime.Parse(length[i]).ToString("yyyy-MM-dd HH:mm:ss") + ",";
            }
            sjxl = sjxl.Substring(0, sjxl.Length - 1);
            nl = DataChange.delete_comma(process_data["MMNL"].ToString());
        }
    }

    //不透光过程数据——GC005
    public class ZYJSProcess
    {
        public string jylsh;
        public string jycs;
        public string cyds;
        public string cysx;
        public string ydzds;
        public string fdjdszs;
        public string yw;
        public string jczt;
        public string sjxl;
        public ZYJSProcess(DataRow process_data)
        {
            jylsh = process_data["JYLSH"].ToString();
            jycs = process_data["JYCS"].ToString();
            cyds = process_data["CYDS"].ToString();
            cysx = DataChange.delete_comma(process_data["MMSX"].ToString());
            ydzds = DataChange.delete_comma(process_data["MMK"].ToString());
            fdjdszs = DataChange.delete_comma(process_data["MMZS"].ToString());
            yw = DataChange.delete_comma(process_data["MMYW"].ToString());
            //for (int i = 0; i < fdjdszs.Split(',').Count(); i++)
            //{
            //    yw += "80,";
            //}
            //yw = yw.Substring(0, yw.Length - 1);
            jczt = DataChange.delete_comma(process_data["MMLB"].ToString());
            //sjxl = DataChange.delete_comma(process_data["MMTIME"].ToString());
            string temp_mmtime = DataChange.delete_comma(process_data["MMTIME"].ToString());
            string[] length = temp_mmtime.Split(',');
            for (int i = 0; i < length.Length; i++)
            {
                sjxl = sjxl + DateTime.Parse(length[i]).ToString("yyyy-MM-dd HH:mm:ss") + ",";
            }
            sjxl = sjxl.Substring(0, sjxl.Length - 1);
        }
    }

    #region 设备自检数据——ZJ000
    //山东
    public class JZHXselfcheckSD
    {
        public string sbbh;
        public string zjlx;
        public string zjsj;
        public string zjjg;
        public string jczbh;
        public string jcgwh;
        public string txjc;
        public string jsqjc;
        public string yrqssj;
        public string yrjssj;
        public string gxdl;
        public string cs1;
        public string jzgl1;
        public string jsgl1;
        public string llhxsj1;
        public string sjhxsj1;
        public string wc1;
        public string kssj1;
        public string jssj1;
        public string cs2;
        public string jzgl2;
        public string jsgl2;
        public string llhxsj2;
        public string sjhxsj2;
        public string wc2;
        public string kssj2;
        public string jssj2;
        public JZHXselfcheckSD(DataRow SelfCheckData)
        {
            sbbh = SelfCheckData["SBBH"].ToString();
            zjlx = SelfCheckData["ZJLX"].ToString();
            zjsj = SelfCheckData["ZJSJ"].ToString();
            zjjg = SelfCheckData["ZJJG"].ToString();
            jczbh = SelfCheckData["JCZBH"].ToString();
            jcgwh = SelfCheckData["JCGWH"].ToString();
            txjc = SelfCheckData["DATA1"].ToString();
            jsqjc = SelfCheckData["DATA2"].ToString();
            yrqssj = SelfCheckData["DATA3"].ToString();
            yrjssj = SelfCheckData["DATA4"].ToString();
            gxdl = SelfCheckData["DATA5"].ToString();
            cs1 = SelfCheckData["DATA6"].ToString();
            jzgl1 = SelfCheckData["DATA7"].ToString();
            jsgl1 = SelfCheckData["DATA8"].ToString();
            llhxsj1 = SelfCheckData["DATA9"].ToString();
            sjhxsj1 = SelfCheckData["DATA10"].ToString();
            wc1 = SelfCheckData["DATA11"].ToString();
            kssj1 = SelfCheckData["DATA12"].ToString();
            jssj1 = SelfCheckData["DATA13"].ToString();
            cs2 = SelfCheckData["DATA14"].ToString();
            jzgl2 = SelfCheckData["DATA15"].ToString();
            jsgl2 = SelfCheckData["DATA16"].ToString();
            llhxsj2 = SelfCheckData["DATA17"].ToString();
            sjhxsj2 = SelfCheckData["DATA18"].ToString();
            wc2 = SelfCheckData["DATA19"].ToString();
            kssj2 = SelfCheckData["DATA20"].ToString();
            jssj2 = SelfCheckData["DATA21"].ToString();
        }
    }
    public class FQYselfcheckSD
    {
        public string sbbh;
        public string zjlx;
        public string zjsj;
        public string zjjg;
        public string jczbh;
        public string jcgwh;
        public string txjc;
        public string yqyr;
        public string yqjl;
        public string yqtl;
        public string yqyq;
        public string clhc;
        public string kssj;
        public string jssj;

        public FQYselfcheckSD(DataRow SelfCheckData)
        {
            sbbh = SelfCheckData["SBBH"].ToString();
            zjlx = SelfCheckData["ZJLX"].ToString();
            zjsj = SelfCheckData["ZJSJ"].ToString();
            zjjg = SelfCheckData["ZJJG"].ToString();
            jczbh = SelfCheckData["JCZBH"].ToString();
            jcgwh = SelfCheckData["JCGWH"].ToString();
            txjc = SelfCheckData["DATA1"].ToString();
            yqyr = SelfCheckData["DATA2"].ToString();
            yqjl = SelfCheckData["DATA3"].ToString();
            yqtl = SelfCheckData["DATA4"].ToString();
            yqyq = SelfCheckData["DATA5"].ToString();
            clhc = SelfCheckData["DATA6"].ToString();
            kssj = SelfCheckData["DATA7"].ToString();
            jssj = SelfCheckData["DATA8"].ToString();
        }
    }
    public class YDJselfcheckSD
    {
        public string sbbh;
        public string zjlx;
        public string zjsj;
        public string zjjg;
        public string jczbh;
        public string jcgwh;
        public string txjc;
        public string yqyr;
        public string yqtl;
        public string lcjc;
        public string sdz1;
        public string scz1;
        public string wcz1;
        public string sdz2;
        public string scz2;
        public string wcz2;
        public string kssj;
        public string jssj;
        public YDJselfcheckSD(DataRow SelfCheckData)
        {
            sbbh = SelfCheckData["SBBH"].ToString();
            zjlx = SelfCheckData["ZJLX"].ToString();
            zjsj = SelfCheckData["ZJSJ"].ToString();
            zjjg = SelfCheckData["ZJJG"].ToString();
            jczbh = SelfCheckData["JCZBH"].ToString();
            jcgwh = SelfCheckData["JCGWH"].ToString();
            txjc = SelfCheckData["DATA1"].ToString();
            yqyr = SelfCheckData["DATA2"].ToString();
            yqtl = SelfCheckData["DATA3"].ToString();
            lcjc = SelfCheckData["DATA4"].ToString();
            sdz1 = SelfCheckData["DATA5"].ToString();
            scz1 = SelfCheckData["DATA6"].ToString();
            wcz1 = SelfCheckData["DATA7"].ToString();
            sdz2 = SelfCheckData["DATA8"].ToString();
            scz2 = SelfCheckData["DATA9"].ToString();
            wcz2 = SelfCheckData["DATA10"].ToString();
            kssj = SelfCheckData["DATA11"].ToString();
            jssj = SelfCheckData["DATA12"].ToString();
        }
    }
    public class HJYselfcheckSD
    {
        public string sbbh;
        public string zjlx;
        public string zjsj;
        public string zjjg;
        public string jczbh;
        public string jcgwh;
        public string txjc;
        public string hjwd;
        public string hjsd;
        public string hjqy;
        public string yqwd;
        public string yqsd;
        public string yqqy;
        public string kssj;
        public string jssj;
        public HJYselfcheckSD(DataRow SelfCheckData)
        {
            sbbh = SelfCheckData["SBBH"].ToString();
            zjlx = SelfCheckData["ZJLX"].ToString();
            zjsj = SelfCheckData["ZJSJ"].ToString();
            zjjg = SelfCheckData["ZJJG"].ToString();
            jczbh = SelfCheckData["JCZBH"].ToString();
            jcgwh = SelfCheckData["JCGWH"].ToString();
            txjc = SelfCheckData["DATA1"].ToString();
            hjwd = SelfCheckData["DATA2"].ToString();
            hjsd = SelfCheckData["DATA3"].ToString();
            hjqy = SelfCheckData["DATA4"].ToString();
            yqwd = SelfCheckData["DATA5"].ToString();
            yqsd = SelfCheckData["DATA6"].ToString();
            yqqy = SelfCheckData["DATA7"].ToString();
            kssj = SelfCheckData["DATA12"].ToString();
            jssj = SelfCheckData["DATA13"].ToString();
        }
    }
    public class ZSJselfcheckSD
    {
        public string sbbh;
        public string zjlx;
        public string zjsj;
        public string zjjg;
        public string jczbh;
        public string jcgwh;
        public string txjc;
        public string dszs;
        public ZSJselfcheckSD(DataRow SelfCheckData)
        {
            sbbh = SelfCheckData["SBBH"].ToString();
            zjlx = SelfCheckData["ZJLX"].ToString();
            zjsj = SelfCheckData["ZJSJ"].ToString();
            zjjg = SelfCheckData["ZJJG"].ToString();
            jczbh = SelfCheckData["JCZBH"].ToString();
            jcgwh = SelfCheckData["JCGWH"].ToString();
            txjc = SelfCheckData["DATA1"].ToString();
            dszs = SelfCheckData["DATA2"].ToString();
        }
    }
    public class LLJselfcheckSD
    {
        public string sbbh;
        public string zjlx;
        public string zjsj;
        public string zjjg;
        public string jczbh;
        public string jcgwh;
        public string txjc;
        public string yqyr;
        public string lljc;
        public string yqlc;
        public string yqll;
        public string yqyq;
        public string kssj;
        public string jssj;
        public LLJselfcheckSD(DataRow SelfCheckData)
        {
            sbbh = SelfCheckData["SBBH"].ToString();
            zjlx = SelfCheckData["ZJLX"].ToString();
            zjsj = SelfCheckData["ZJSJ"].ToString();
            zjjg = SelfCheckData["ZJJG"].ToString();
            jczbh = SelfCheckData["JCZBH"].ToString();
            jcgwh = SelfCheckData["JCGWH"].ToString();
            txjc = SelfCheckData["DATA1"].ToString();
            yqyr = SelfCheckData["DATA2"].ToString();
            lljc = SelfCheckData["DATA3"].ToString();
            yqlc = SelfCheckData["DATA4"].ToString();
            yqll = SelfCheckData["DATA5"].ToString();
            yqyq = SelfCheckData["DATA6"].ToString();
            kssj = SelfCheckData["DATA7"].ToString();
            jssj = SelfCheckData["DATA8"].ToString();
        }
    }
    public class JSGLselfcheckSD
    {
        public string sbbh;
        public string zjlx;
        public string zjsj;
        public string zjjg;
        public string jczbh;
        public string jcgwh;
        public string hxsj1;
        public string jsgl1;
        public string sdzd1;
        public string sdzx1;
        public string mysd1;
        public string kssj1;
        public string jssj1;
        public string hxsj2;
        public string jsgl2;
        public string sdzd2;
        public string sdzx2;
        public string mysd2;
        public string kssj2;
        public string jssj2;
        public string hxsj3;
        public string jsgl3;
        public string sdzd3;
        public string sdzx3;
        public string mysd3;
        public string kssj3;
        public string jssj3;
        public string hxsj4;
        public string jsgl4;
        public string sdzd4;
        public string sdzx4;
        public string mysd4;
        public string kssj4;
        public string jssj4;
        public string hxsj5;
        public string jsgl5;
        public string sdzd5;
        public string sdzx5;
        public string mysd5;
        public string kssj5;
        public string jssj5;
        public JSGLselfcheckSD(DataRow SelfCheckData)
        {
            sbbh = SelfCheckData["SBBH"].ToString();
            zjlx = SelfCheckData["ZJLX"].ToString();
            zjsj = SelfCheckData["ZJSJ"].ToString();
            zjjg = SelfCheckData["ZJJG"].ToString();
            jczbh = SelfCheckData["JCZBH"].ToString();
            jcgwh = SelfCheckData["JCGWH"].ToString();
            hxsj1 = SelfCheckData["DATA1"].ToString();
            jsgl1 = SelfCheckData["DATA2"].ToString();
            sdzd1 = SelfCheckData["DATA3"].ToString();
            sdzx1 = SelfCheckData["DATA4"].ToString();
            mysd1 = SelfCheckData["DATA5"].ToString();
            kssj1 = SelfCheckData["DATA6"].ToString();
            jssj1 = SelfCheckData["DATA7"].ToString();
            hxsj2 = SelfCheckData["DATA8"].ToString();
            jsgl2 = SelfCheckData["DATA9"].ToString();
            sdzd2 = SelfCheckData["DATA10"].ToString();
            sdzx2 = SelfCheckData["DATA11"].ToString();
            mysd2 = SelfCheckData["DATA12"].ToString();
            kssj2 = SelfCheckData["DATA13"].ToString();
            jssj2 = SelfCheckData["DATA14"].ToString();
            hxsj3 = SelfCheckData["DATA15"].ToString();
            jsgl3 = SelfCheckData["DATA16"].ToString();
            sdzd3 = SelfCheckData["DATA17"].ToString();
            sdzx3 = SelfCheckData["DATA18"].ToString();
            mysd3 = SelfCheckData["DATA19"].ToString();
            kssj3 = SelfCheckData["DATA20"].ToString();
            jssj3 = SelfCheckData["DATA21"].ToString();
            hxsj4 = SelfCheckData["DATA22"].ToString();
            jsgl4 = SelfCheckData["DATA23"].ToString();
            sdzd4 = SelfCheckData["DATA24"].ToString();
            sdzx4 = SelfCheckData["DATA25"].ToString();
            mysd4 = SelfCheckData["DATA26"].ToString();
            kssj4 = SelfCheckData["DATA27"].ToString();
            jssj4 = SelfCheckData["DATA28"].ToString();
            hxsj5 = SelfCheckData["DATA29"].ToString();
            jsgl5 = SelfCheckData["DATA30"].ToString();
            sdzd5 = SelfCheckData["DATA31"].ToString();
            sdzx5 = SelfCheckData["DATA32"].ToString();
            mysd5 = SelfCheckData["DATA33"].ToString();
            kssj5 = SelfCheckData["DATA34"].ToString();
            jssj5 = SelfCheckData["DATA35"].ToString();
        }
    }
    //辽宁
    public class JZHXselfcheckLN
    {
        public string tsno;
        public string testlineno;
        public string zjlx;
        public string jcrq;
        public string jckssj;
        public string sjhxsj1;
        public string sjhxsj2;
        public string ns1;
        public string ns2;
        public string myhxsj1;
        public string myhxsj2;
        public string zsgl1;
        public string zsgl2;
        public string jbgl;
        public string jcjg1;
        public string jcjg2;
        public string jcjg;
        public JZHXselfcheckLN(DataRow SelfCheckData)
        {
            tsno = SelfCheckData["JCZBH"].ToString();
            testlineno = SelfCheckData["JCGWH"].ToString();
            zjlx = SelfCheckData["ZJLX"].ToString();
            jcrq = DateTime.Parse(SelfCheckData["ZJSJ"].ToString()).ToString("yyyy-MM-dd");
            jckssj = SelfCheckData["DATA1"].ToString().Insert(4, "-").Insert(7, "-").Insert(10, " ").Insert(13, ":").Insert(16, ":");
            sjhxsj1 = SelfCheckData["DATA2"].ToString();
            sjhxsj2 = SelfCheckData["DATA3"].ToString();
            ns1 = SelfCheckData["DATA4"].ToString();
            ns2 = SelfCheckData["DATA5"].ToString();
            myhxsj1 = SelfCheckData["DATA6"].ToString();
            myhxsj2 = SelfCheckData["DATA7"].ToString();
            zsgl1 = SelfCheckData["DATA8"].ToString();
            zsgl2 = SelfCheckData["DATA9"].ToString();
            jbgl = SelfCheckData["DATA10"].ToString();
            jcjg1 = SelfCheckData["DATA11"].ToString();
            jcjg2 = SelfCheckData["DATA12"].ToString();
            jcjg = SelfCheckData["DATA13"].ToString();
        }
    }
    public class FJGLSSselfcheckLN
    {
        public string tsno;
        public string testlineno;
        public string zjlx;
        public string jcrq;
        public string jckssj;
        public string jcjssj;
        public string sjhxsj1;
        public string sjhxsj2;
        public string ns1;
        public string ns2;
        public string jbgl;
        public string jcjg;
        public FJGLSSselfcheckLN(DataRow SelfCheckData)
        {
            tsno = SelfCheckData["JCZBH"].ToString();
            testlineno = SelfCheckData["JCGWH"].ToString();
            zjlx = SelfCheckData["ZJLX"].ToString();
            jcrq = DateTime.Parse(SelfCheckData["ZJSJ"].ToString()).ToString("yyyy-MM-dd");
            jckssj = SelfCheckData["DATA1"].ToString().Insert(4, "-").Insert(7, "-").Insert(10, " ").Insert(13, ":").Insert(16, ":");
            jcjssj = SelfCheckData["DATA2"].ToString().Insert(4, "-").Insert(7, "-").Insert(10, " ").Insert(13, ":").Insert(16, ":");
            sjhxsj1 = SelfCheckData["DATA3"].ToString();
            sjhxsj2 = SelfCheckData["DATA4"].ToString();
            ns1 = SelfCheckData["DATA5"].ToString();
            ns2 = SelfCheckData["DATA6"].ToString();
            jbgl = SelfCheckData["DATA7"].ToString();
            jcjg = SelfCheckData["DATA8"].ToString();
        }
    }
    public class FXYselfcheckLN
    {
        public string tsno;
        public string testlineno;
        public string zjlx;
        public string jcrq;
        public string jclx;
        public string jckssj;
        public string c3h8nd;
        public string cond;
        public string co2nd;
        public string nond;
        public string o2nd;
        public string hcjcz;
        public string cojcz;
        public string co2jcz;
        public string nojcz;
        public string o2jcz;
        public string pef;
        public string jcjg;
        public FXYselfcheckLN(DataRow SelfCheckData)
        {
            tsno = SelfCheckData["JCZBH"].ToString();
            testlineno = SelfCheckData["JCGWH"].ToString();
            zjlx = SelfCheckData["ZJLX"].ToString();
            jcrq = DateTime.Parse(SelfCheckData["ZJSJ"].ToString()).ToString("yyyy-MM-dd");
            jclx = SelfCheckData["DATA1"].ToString();
            jckssj = SelfCheckData["DATA2"].ToString().Insert(4, "-").Insert(7, "-").Insert(10, " ").Insert(13, ":").Insert(16, ":");
            c3h8nd = SelfCheckData["DATA3"].ToString();
            cond = SelfCheckData["DATA4"].ToString();
            co2nd = SelfCheckData["DATA5"].ToString();
            nond = SelfCheckData["DATA6"].ToString();
            o2nd = SelfCheckData["DATA7"].ToString();
            hcjcz = SelfCheckData["DATA8"].ToString();
            cojcz = SelfCheckData["DATA9"].ToString();
            co2jcz = SelfCheckData["DATA10"].ToString();
            nojcz = SelfCheckData["DATA11"].ToString();
            o2jcz = SelfCheckData["DATA12"].ToString();
            pef = SelfCheckData["DATA13"].ToString();
            jcjg = SelfCheckData["DATA14"].ToString();

        }
    }
    public class XLselfcheckLN
    {
        public string tsno;
        public string testlineno;
        public string zjlx;
        public string jcrq;
        public string jckssj;
        public string jcjg;
        public XLselfcheckLN(DataRow SelfCheckData)
        {
            tsno = SelfCheckData["JCZBH"].ToString();
            testlineno = SelfCheckData["JCGWH"].ToString();
            zjlx = SelfCheckData["ZJLX"].ToString();
            jcrq = DateTime.Parse(SelfCheckData["ZJSJ"].ToString()).ToString("yyyy-MM-dd");
            jckssj = SelfCheckData["DATA1"].ToString().Insert(4, "-").Insert(7, "-").Insert(10, " ").Insert(13, ":").Insert(16, ":");
            jcjg = SelfCheckData["DATA2"].ToString();
        }
    }
    public class FXYYLCselfcheckLN
    {
        public string tsno;
        public string testlineno;
        public string zjlx;
        public string jcrq;
        public string jckssj;
        public string o2lcbz;
        public string o2lcclz;
        public string o2lcwc;
        public string jcjg;
        public FXYYLCselfcheckLN(DataRow SelfCheckData)
        {
            tsno = SelfCheckData["JCZBH"].ToString();
            testlineno = SelfCheckData["JCGWH"].ToString();
            zjlx = SelfCheckData["ZJLX"].ToString();
            jcrq = DateTime.Parse(SelfCheckData["ZJSJ"].ToString()).ToString("yyyy-MM-dd");
            jckssj = SelfCheckData["DATA1"].ToString().ToString().Insert(4, "-").Insert(7, "-").Insert(10, " ").Insert(13, ":").Insert(16, ":");
            o2lcbz = SelfCheckData["DATA2"].ToString();
            o2lcclz = SelfCheckData["DATA3"].ToString();
            o2lcwc = SelfCheckData["DATA4"].ToString();
            jcjg = SelfCheckData["DATA5"].ToString();
        }
    }
    public class DBQselfcheckLN
    {
        public string tsno;
        public string testlineno;
        public string zjlx;
        public string jcrq;
        public string jclx;
        public string jckssj;
        public string c3h8nd;
        public string cond;
        public string co2nd;
        public string nond;
        public string o2nd;
        public string hcjcz;
        public string cojcz;
        public string co2jcz;
        public string nojcz;
        public string o2jcz;
        public string pef;
        public string jcjg;
        public DBQselfcheckLN(DataRow SelfCheckData)
        {
            tsno = SelfCheckData["JCZBH"].ToString();
            testlineno = SelfCheckData["JCGWH"].ToString();
            zjlx = SelfCheckData["ZJLX"].ToString();
            jcrq = DateTime.Parse(SelfCheckData["ZJSJ"].ToString()).ToString("yyyy-MM-dd");
            jclx = SelfCheckData["DATA1"].ToString();
            jckssj = SelfCheckData["DATA2"].ToString().Insert(4, "-").Insert(7, "-").Insert(10, " ").Insert(13, ":").Insert(16, ":");
            c3h8nd = SelfCheckData["DATA3"].ToString();
            cond = SelfCheckData["DATA4"].ToString();
            co2nd = SelfCheckData["DATA5"].ToString();
            nond = SelfCheckData["DATA6"].ToString();
            o2nd = SelfCheckData["DATA7"].ToString();
            hcjcz = SelfCheckData["DATA8"].ToString();
            cojcz = SelfCheckData["DATA9"].ToString();
            co2jcz = SelfCheckData["DATA10"].ToString();
            nojcz = SelfCheckData["DATA11"].ToString();
            o2jcz = SelfCheckData["DATA12"].ToString();
            pef = SelfCheckData["DATA13"].ToString();
            jcjg = SelfCheckData["DATA14"].ToString();
        }
    }
    public class LLJselfcheckLN
    {
        public string tsno;
        public string testlineno;
        public string zjlx;
        public string jcrq;
        public string jckssj;
        public string o2glcbz;
        public string o2glcclz;
        public string o2glcwc;
        public string o2dlcbz;
        public string o2dlcclz;
        public string o2dlcwc;
        public string jcjg;
        public LLJselfcheckLN(DataRow SelfCheckData)
        {
            tsno = SelfCheckData["JCZBH"].ToString();
            testlineno = SelfCheckData["JCGWH"].ToString();
            zjlx = SelfCheckData["ZJLX"].ToString();
            jcrq = DateTime.Parse(SelfCheckData["ZJSJ"].ToString()).ToString("yyyy-MM-dd");
            jckssj = SelfCheckData["DATA1"].ToString().Insert(4, "-").Insert(7, "-").Insert(10, " ").Insert(13, ":").Insert(16, ":");
            o2glcbz = SelfCheckData["DATA2"].ToString();
            o2glcclz = SelfCheckData["DATA3"].ToString();
            o2glcwc = SelfCheckData["DATA4"].ToString();
            o2dlcbz = SelfCheckData["DATA5"].ToString();
            o2dlcclz = SelfCheckData["DATA6"].ToString();
            o2dlcwc = SelfCheckData["DATA7"].ToString();
            jcjg = SelfCheckData["DATA8"].ToString();
        }
    }
    //贵州六盘水
    public class DZHJselfcheckGZ
    {
        public string tsno;
        public string testlineno;
        public string zjlx;
        public string jcrq;
        public string jckssj;
        public string jcjssj;
        public string txjc;
        public string hjwd;
        public string yqwd;
        public string hjsd;
        public string yqsd;
        public string hjqy;
        public string yqqy;
        public string jcjg;
        public DZHJselfcheckGZ(DataRow SelfCheckData)
        {
            tsno = SelfCheckData["JCZBH"].ToString();
            testlineno = SelfCheckData["JCGWH"].ToString();
            zjlx = SelfCheckData["ZJLX"].ToString();
            jcrq = DateTime.Parse(SelfCheckData["ZJSJ"].ToString()).ToString("yyyy-MM-dd");
            jckssj = SelfCheckData["DATA8"].ToString();
            jcjssj = SelfCheckData["DATA9"].ToString();
            txjc = SelfCheckData["DATA1"].ToString();
            hjwd = SelfCheckData["DATA2"].ToString();
            hjsd = SelfCheckData["DATA3"].ToString();
            hjqy = SelfCheckData["DATA4"].ToString();
            yqwd = SelfCheckData["DATA5"].ToString();
            yqsd = SelfCheckData["DATA6"].ToString();
            yqqy = SelfCheckData["DATA7"].ToString();
            jcjg = SelfCheckData["ZJJG"].ToString();
        }
    }
    #endregion
    #region 设备标定数据——BD000
    public class SpeedBD_SD
    {
        public string sbbh;
        public string bdsj;
        public string bdr;
        public string czr;
        public string bdlx;
        public string bdjg;
        public string jczbh;
        public string jcgwh;
        public string csxs;
        public string sdz1;
        public string scz1;
        public string jdwcz1;
        public string xdwcz1;
        public string sdz2;
        public string scz2;
        public string jdwcz2;
        public string xdwcz2;
        public string sdz3;
        public string scz3;
        public string jdwcz3;
        public string xdwcz3;
        public SpeedBD_SD(DataRow BDData)
        {
            sbbh = BDData["SBBH"].ToString();
            bdsj = DateTime.Parse(BDData["BDSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            bdr = BDData["BDR"].ToString();
            //czr = BDData["CZR"].ToString();
            bdlx = BDData["BDLX"].ToString();
            bdjg = BDData["BDJG"].ToString();
            jczbh = BDData["JCZBH"].ToString();
            jcgwh = BDData["JCGWH"].ToString();
            sdz1 = BDData["DATA1"].ToString();
            scz1 = BDData["DATA2"].ToString();
            jdwcz1 = BDData["DATA3"].ToString();
            xdwcz1 = BDData["DATA4"].ToString();
            sdz2 = BDData["DATA5"].ToString();
            scz2 = BDData["DATA6"].ToString();
            jdwcz2 = BDData["DATA7"].ToString();
            xdwcz2 = BDData["DATA8"].ToString();
            sdz3 = BDData["DATA9"].ToString();
            scz3 = BDData["DATA10"].ToString();
            jdwcz3 = BDData["DATA11"].ToString();
            xdwcz3 = BDData["DATA12"].ToString();
        }
    }

    public class SpeedBD_LN
    {
        public string sbbh;
        public string bdsj;
        public string bdr;
        public string czr;
        public string bdlx;
        public string bdjg;
        public string jczbh;
        public string jcgwh;
        public string csxs;
        public string sdz1;
        public string scz1;
        public string jdwcz1;
        public string xdwcz1;
        public string sdz2;
        public string scz2;
        public string jdwcz2;
        public string xdwcz2;
        public string sdz3;
        public string scz3;
        public string jdwcz3;
        public string xdwcz3;
        public SpeedBD_LN(DataRow BDData)
        {
            sbbh = BDData["SBBH"].ToString();
            bdsj = DateTime.Parse(BDData["BDSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            bdr = BDData["BDR"].ToString();
            //czr = BDData["CZR"].ToString();
            bdlx = BDData["BDLX"].ToString();
            bdjg = BDData["BDJG"].ToString();
            jczbh = BDData["JCZBH"].ToString();
            jcgwh = BDData["JCGWH"].ToString();
            csxs = BDData["DATA1"].ToString();
            sdz1 = BDData["DATA2"].ToString();
            scz1 = BDData["DATA3"].ToString();
            jdwcz1 = BDData["DATA4"].ToString();
            xdwcz1 = BDData["DATA5"].ToString();
            sdz2 = BDData["DATA6"].ToString();
            scz2 = BDData["DATA7"].ToString();
            jdwcz2 = BDData["DATA8"].ToString();
            xdwcz2 = BDData["DATA9"].ToString();
            sdz3 = BDData["DATA10"].ToString();
            scz3 = BDData["DATA11"].ToString();
            jdwcz3 = BDData["DATA12"].ToString();
            xdwcz3 = BDData["DATA13"].ToString();
        }
    }
    public class NlBD_SD
    {
        public string sbbh;
        public string bdsj;
        public string bdr;
        public string czr;
        public string bdlx;
        public string bdjg;
        public string jczbh;
        public string jcgwh;
        public string nlsdz1;
        public string nlscz1;
        public string nlwcz1;
        public string nlsdz2;
        public string nlscz2;
        public string nlwcz2;
        public string nlsdz3;
        public string nlscz3;
        public string nlwcz3;
        public string nlsdz4;
        public string nlscz4;
        public string nlwcz4;
        public string nlsdz5;
        public string nlscz5;
        public string nlwcz5;
        public NlBD_SD(DataRow BDData)
        {
            sbbh = BDData["SBBH"].ToString();
            bdsj = DateTime.Parse(BDData["BDSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            bdr = BDData["BDR"].ToString();
            //czr = BDData["CZR"].ToString();
            bdlx = BDData["BDLX"].ToString();
            bdjg = BDData["BDJG"].ToString();
            jczbh = BDData["JCZBH"].ToString();
            jcgwh = BDData["JCGWH"].ToString();
            nlsdz1 = BDData["DATA1"].ToString();
            nlscz1 = BDData["DATA2"].ToString();
            nlwcz1 = BDData["DATA3"].ToString();
            nlsdz2 = BDData["DATA4"].ToString();
            nlscz2 = BDData["DATA5"].ToString();
            nlwcz2 = BDData["DATA6"].ToString();
            nlsdz3 = BDData["DATA7"].ToString();
            nlscz3 = BDData["DATA8"].ToString();
            nlwcz3 = BDData["DATA9"].ToString();
            nlsdz4 = BDData["DATA10"].ToString();
            nlscz4 = BDData["DATA11"].ToString();
            nlwcz4 = BDData["DATA12"].ToString();
            nlsdz5 = BDData["DATA13"].ToString();
            nlscz5 = BDData["DATA14"].ToString();
            nlwcz5 = BDData["DATA15"].ToString();
        }
    }
    public class NlBD_LN
    {
        public string sbbh;
        public string bdsj;
        public string bdr;
        public string czr;
        public string bdlx;
        public string bdjg;
        public string jczbh;
        public string jcgwh;
        public string nlxs;
        public string nlsdz1;
        public string nlscz1;
        public string nlwcz1;
        public string nlsdz2;
        public string nlscz2;
        public string nlwcz2;
        public string nlsdz3;
        public string nlscz3;
        public string nlwcz3;
        public string nlsdz4;
        public string nlscz4;
        public string nlwcz4;
        public string nlsdz5;
        public string nlscz5;
        public string nlwcz5;
        public NlBD_LN(DataRow BDData)
        {
            sbbh = BDData["SBBH"].ToString();
            bdsj = DateTime.Parse(BDData["BDSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            bdr = BDData["BDR"].ToString();
            //czr = BDData["CZR"].ToString();
            bdlx = BDData["BDLX"].ToString();
            bdjg = BDData["BDJG"].ToString();
            jczbh = BDData["JCZBH"].ToString();
            jcgwh = BDData["JCGWH"].ToString();
            nlxs = BDData["DATA1"].ToString();
            nlsdz1 = BDData["DATA2"].ToString();
            nlscz1 = BDData["DATA3"].ToString();
            nlwcz1 = BDData["DATA4"].ToString();
            nlsdz2 = BDData["DATA5"].ToString();
            nlscz2 = BDData["DATA6"].ToString();
            nlwcz2 = BDData["DATA7"].ToString();
            nlsdz3 = BDData["DATA8"].ToString();
            nlscz3 = BDData["DATA9"].ToString();
            nlwcz3 = BDData["DATA10"].ToString();
            nlsdz4 = BDData["DATA11"].ToString();
            nlscz4 = BDData["DATA12"].ToString();
            nlwcz4 = BDData["DATA13"].ToString();
            nlsdz5 = BDData["DATA14"].ToString();
            nlscz5 = BDData["DATA15"].ToString();
            nlwcz5 = BDData["DATA16"].ToString();
        }
    }
    public class JsglBD
    {
        public string sbbh;
        public string bdsj;
        public string bdr;
        public string czr;
        public string bdlx;
        public string bdjg;
        public string jczbh;
        public string jcgwh;
        public string hxsj1;
        public string jsgl1;
        public string sdzd1;
        public string sdzx1;
        public string mysd1;
        public string jbgl;
        public string kssj1;
        public string jssj1;
        public string hxsj2;
        public string jsgl2;
        public string sdzd2;
        public string sdzx2;
        public string mysd2;
        public string kssj2;
        public string jssj2;
        public string hxsj3;
        public string jsgl3;
        public string sdzd3;
        public string sdzx3;
        public string mysd3;
        public string kssj3;
        public string jssj3;
        public string hxsj4;
        public string jsgl4;
        public string sdzd4;
        public string sdzx4;
        public string mysd4;
        public string kssj4;
        public string jssj4;
        public string hxsj5;
        public string jsgl5;
        public string sdzd5;
        public string sdzx5;
        public string mysd5;
        public string kssj5;
        public string jssj5;
        public JsglBD(DataRow BDData)
        {
            sbbh = BDData["SBBH"].ToString();
            bdsj = DateTime.Parse(BDData["BDSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            bdr = BDData["BDR"].ToString();
            //czr = BDData["CZR"].ToString();
            bdlx = BDData["BDLX"].ToString();
            bdjg = BDData["BDJG"].ToString();
            jczbh = BDData["JCZBH"].ToString();
            jcgwh = BDData["JCGWH"].ToString();
            hxsj1 = BDData["DATA1"].ToString();
            jsgl1 = BDData["DATA2"].ToString();
            sdzd1 = BDData["DATA3"].ToString();
            sdzx1 = BDData["DATA4"].ToString();
            mysd1 = BDData["DATA5"].ToString();
            jbgl = BDData["DATA6"].ToString();
            kssj1 = BDData["DATA7"].ToString();
            jssj1 = BDData["DATA8"].ToString();
            hxsj2 = BDData["DATA9"].ToString();
            jsgl2 = BDData["DATA10"].ToString();
            sdzd2 = BDData["DATA11"].ToString();
            sdzx2 = BDData["DATA12"].ToString();
            mysd2 = BDData["DATA13"].ToString();
            kssj2 = BDData["DATA14"].ToString();
            jssj2 = BDData["DATA15"].ToString();
            hxsj3 = BDData["DATA16"].ToString();
            jsgl3 = BDData["DATA17"].ToString();
            sdzd3 = BDData["DATA18"].ToString();
            sdzx3 = BDData["DATA19"].ToString();
            mysd3 = BDData["DATA20"].ToString();
            kssj3 = BDData["DATA21"].ToString();
            jssj3 = BDData["DATA22"].ToString();
            hxsj4 = BDData["DATA23"].ToString();
            jsgl4 = BDData["DATA24"].ToString();
            sdzd4 = BDData["DATA25"].ToString();
            sdzx4 = BDData["DATA26"].ToString();
            mysd4 = BDData["DATA27"].ToString();
            kssj4 = BDData["DATA28"].ToString();
            jssj4 = BDData["DATA29"].ToString();
            hxsj5 = BDData["DATA30"].ToString();
            jsgl5 = BDData["DATA31"].ToString();
            sdzd5 = BDData["DATA32"].ToString();
            sdzx5 = BDData["DATA33"].ToString();
            mysd5 = BDData["DATA34"].ToString();
            kssj5 = BDData["DATA35"].ToString();
            jssj5 = BDData["DATA36"].ToString();
        }
    }
    public class JzhxBD
    {
        public string sbbh;
        public string bdsj;
        public string bdr;
        public string czr;
        public string bdlx;
        public string bdjg;
        public string jczbh;
        public string jcgwh;
        public string hxqj1;
        public string hxqj2;
        public string hxqj3;
        public string hxqj4;
        public string jzgl1;
        public string jzgl2;
        public string jzgl3;
        public string jzgl4;
        public string jsgl1;
        public string jsgl2;
        public string jsgl3;
        public string jsgl4;
        public string hxsj1;
        public string hxsj2;
        public string hxsj3;
        public string hxsj4;
        public string llsj1;
        public string llsj2;
        public string llsj3;
        public string llsj4;
        public string wc1;
        public string wc2;
        public string wc3;
        public string wc4;
        public string jbgl;
        public string hxqj1jcjg;
        public string hxqj2jcjg;
        public string hxqj3jcjg;
        public string hxqj4jcjg;
        public JzhxBD(DataRow BDData)
        {
            sbbh = BDData["SBBH"].ToString();
            bdsj = DateTime.Parse(BDData["BDSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            bdr = BDData["BDR"].ToString();
            //czr = BDData["CZR"].ToString();
            bdlx = BDData["BDLX"].ToString();
            bdjg = BDData["BDJG"].ToString();
            jczbh = BDData["JCZBH"].ToString();
            jcgwh = BDData["JCGWH"].ToString();
            hxqj1 = BDData["DATA1"].ToString();
            hxqj2 = BDData["DATA2"].ToString();
            hxqj3 = BDData["DATA3"].ToString();
            hxqj4 = BDData["DATA4"].ToString();
            jzgl1 = BDData["DATA5"].ToString();
            jzgl2 = BDData["DATA6"].ToString();
            jzgl3 = BDData["DATA7"].ToString();
            jzgl4 = BDData["DATA8"].ToString();
            jsgl1 = BDData["DATA9"].ToString();
            jsgl2 = BDData["DATA10"].ToString();
            jsgl3 = BDData["DATA11"].ToString();
            jsgl4 = BDData["DATA12"].ToString();
            hxsj1 = BDData["DATA13"].ToString();
            hxsj2 = BDData["DATA14"].ToString();
            hxsj3 = BDData["DATA15"].ToString();
            hxsj4 = BDData["DATA16"].ToString();
            llsj1 = BDData["DATA17"].ToString();
            llsj2 = BDData["DATA18"].ToString();
            llsj3 = BDData["DATA19"].ToString();
            llsj4 = BDData["DATA20"].ToString();
            wc1 = BDData["DATA21"].ToString();
            wc2 = BDData["DATA22"].ToString();
            wc3 = BDData["DATA23"].ToString();
            wc4 = BDData["DATA24"].ToString();
            jbgl = BDData["DATA25"].ToString();
            hxqj1jcjg = BDData["DATA26"].ToString();
            hxqj2jcjg = BDData["DATA27"].ToString();
            hxqj3jcjg = BDData["DATA28"].ToString();
            hxqj4jcjg = BDData["DATA29"].ToString();
        }
    }
    public class FqBD
    {
        public string sbbh;
        public string bdsj;
        public string bdr;
        public string czr;
        public string bdlx;
        public string bdjg;
        public string jczbh;
        public string jcgwh;
        public string lx;
        public string bzc3h8;
        public string kssj;
        public string jssj;
        public string sdzhc1;
        public string sczhc1;
        public string jdwczhc1;
        public string xdwczhc1;
        public string sdzhc2;
        public string sczhc2;
        public string jdwczhc2;
        public string xdwczhc2;
        public string sdzco1;
        public string sczco1;
        public string jdwczco1;
        public string xdwczco1;
        public string sdzco2;
        public string sczco2;
        public string jdwczco2;
        public string xdwczco2;
        public string sdzco21;
        public string sczco21;
        public string jdwczco21;
        public string xdwczco21;
        public string sdzco22;
        public string sczco22;
        public string jdwczco22;
        public string xdwczco22;
        public string sdzno1;
        public string sczno1;
        public string jdwczno1;
        public string xdwczno1;
        public string sdzno2;
        public string sczno2;
        public string jdwczno2;
        public string xdwczno2;
        public string pef;
        public string jcjg;

        public FqBD(DataRow BDData)
        {
            sbbh = BDData["SBBH"].ToString();
            bdsj = DateTime.Parse(BDData["BDSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            bdr = BDData["BDR"].ToString();
            //czr = BDData["CZR"].ToString();
            bdlx = BDData["BDLX"].ToString();
            bdjg = BDData["BDJG"].ToString();
            jczbh = BDData["JCZBH"].ToString();
            jcgwh = BDData["JCGWH"].ToString();
            lx = BDData["DATA1"].ToString();
            bzc3h8 = BDData["DATA2"].ToString();
            kssj = BDData["DATA3"].ToString();
            jssj = BDData["DATA4"].ToString();
            sdzhc1 = BDData["DATA5"].ToString();
            sczhc1 = BDData["DATA6"].ToString();
            jdwczhc1 = BDData["DATA7"].ToString();
            xdwczhc1 = BDData["DATA8"].ToString();
            sdzhc2 = BDData["DATA9"].ToString();
            sczhc2 = BDData["DATA10"].ToString();
            jdwczhc2 = BDData["DATA11"].ToString();
            xdwczhc2 = BDData["DATA12"].ToString();
            sdzco1 = BDData["DATA13"].ToString();
            sczco1 = BDData["DATA14"].ToString();
            jdwczco1 = BDData["DATA15"].ToString();
            xdwczco1 = BDData["DATA16"].ToString();
            sdzco2 = BDData["DATA17"].ToString();
            sczco2 = BDData["DATA18"].ToString();
            jdwczco2 = BDData["DATA19"].ToString();
            xdwczco2 = BDData["DATA20"].ToString();
            sdzco21 = BDData["DATA21"].ToString();
            sczco21 = BDData["DATA22"].ToString();
            jdwczco21 = BDData["DATA23"].ToString();
            xdwczco21 = BDData["DATA24"].ToString();
            sdzco22 = BDData["DATA25"].ToString();
            sczco22 = BDData["DATA26"].ToString();
            jdwczco22 = BDData["DATA27"].ToString();
            xdwczco22 = BDData["DATA28"].ToString();
            sdzno1 = BDData["DATA29"].ToString();
            sczno1 = BDData["DATA30"].ToString();
            jdwczno1 = BDData["DATA31"].ToString();
            xdwczno1 = BDData["DATA32"].ToString();
            sdzno2 = BDData["DATA33"].ToString();
            sczno2 = BDData["DATA34"].ToString();
            jdwczno2 = BDData["DATA35"].ToString();
            xdwczno2 = BDData["DATA36"].ToString();
            pef = BDData["DATA37"].ToString();
            jcjg = BDData["DATA38"].ToString();
        }
    }
    public class YdBD
    {
        public string sbbh;
        public string bdsj;
        public string bdr;
        public string czr;
        public string bdlx;
        public string bdjg;
        public string jczbh;
        public string jcgwh;
        public string btgydsdz1;
        public string btgydscz1;
        public string btgydwcz1;
        public string btgydsdz2;
        public string btgydscz2;
        public string btgydwcz2;
        public string btgydsdz3;
        public string btgydscz3;
        public string btgydwcz3;
        public YdBD(DataRow BDData)
        {
            sbbh = BDData["SBBH"].ToString();
            bdsj = DateTime.Parse(BDData["BDSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            bdr = BDData["BDR"].ToString();
            //czr = BDData["CZR"].ToString();
            bdlx = BDData["BDLX"].ToString();
            bdjg = BDData["BDJG"].ToString();
            jczbh = BDData["JCZBH"].ToString();
            jcgwh = BDData["JCGWH"].ToString();
            btgydsdz1 = BDData["DATA1"].ToString();
            btgydscz1 = BDData["DATA2"].ToString();
            btgydwcz1 = BDData["DATA3"].ToString();
            btgydsdz2 = BDData["DATA4"].ToString();
            btgydscz2 = BDData["DATA5"].ToString();
            btgydwcz2 = BDData["DATA6"].ToString();
            btgydsdz3 = BDData["DATA7"].ToString();
            btgydscz3 = BDData["DATA8"].ToString();
            btgydwcz3 = BDData["DATA9"].ToString();
        }
    }
    public class YlcCheck
    {
        public string sbbh;
        public string bdsj;
        public string bdr;
        public string bdlx;
        public string bdjg;
        public string jczbh;
        public string jcgwh;
        public string kssj;
        public string o2lcbz;
        public string o2lcclz;
        public string o2lcwc;
        public string jcjg;
        public YlcCheck(DataRow BDData)
        {
            sbbh = BDData["SBBH"].ToString();
            bdsj = DateTime.Parse(BDData["BDSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            bdr = BDData["BDR"].ToString();
            bdlx = BDData["BDLX"].ToString();
            bdjg = BDData["BDJG"].ToString();
            jczbh = BDData["JCZBH"].ToString();
            jcgwh = BDData["JCGWH"].ToString();
            kssj = BDData["DATA1"].ToString();
            o2lcbz = BDData["DATA2"].ToString();
            o2lcclz = BDData["DATA3"].ToString();
            o2lcwc = BDData["DATA4"].ToString();
            jcjg = BDData["DATA5"].ToString();
        }
    }
    public class LljCheck
    {
        public string sbbh;
        public string bdsj;
        public string bdr;
        public string bdlx;
        public string bdjg;
        public string jczbh;
        public string jcgwh;
        public string kssj;
        public string glcbz;
        public string glcclz;
        public string glcwc;
        public string dlcbz;
        public string dlcclz;
        public string dlcwc;
        public string jcjg;
        public LljCheck(DataRow BDData)
        {
            sbbh = BDData["SBBH"].ToString();
            bdsj = DateTime.Parse(BDData["BDSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            bdr = BDData["BDR"].ToString();
            bdlx = BDData["BDLX"].ToString();
            bdjg = BDData["BDJG"].ToString();
            jczbh = BDData["JCZBH"].ToString();
            jcgwh = BDData["JCGWH"].ToString();
            kssj = BDData["DATA1"].ToString();
            glcbz = BDData["DATA2"].ToString();
            glcclz = BDData["DATA3"].ToString();
            glcwc = BDData["DATA4"].ToString();
            dlcbz = BDData["DATA5"].ToString();
            dlcclz = BDData["DATA6"].ToString();
            dlcwc = BDData["DATA7"].ToString();
            jcjg = BDData["DATA8"].ToString();
        }
    }
    public class GlBD
    {
        public string sbbh;
        public string bdsj;
        public string bdr;
        public string bdlx;
        public string bdjg;
        public string jczbh;
        public string jcgwh;
        public string mpbsz;
        public string sjclz;
        public string glwc;
        public string jcjg;
        public GlBD(DataRow BDData)
        {
            sbbh = BDData["SBBH"].ToString();
            bdsj = DateTime.Parse(BDData["BDSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            bdr = BDData["BDR"].ToString();
            bdlx = BDData["BDLX"].ToString();
            bdjg = BDData["BDJG"].ToString();
            jczbh = BDData["JCZBH"].ToString();
            jcgwh = BDData["JCGWH"].ToString();
            mpbsz = BDData["DATA1"].ToString();
            sjclz = BDData["DATA2"].ToString();
            glwc = BDData["DATA3"].ToString();
            jcjg = BDData["DATA4"].ToString();
        }
    }
    public class XysjBD
    {
        public string sbbh;
        public string bdsj;
        public string bdr;
        public string bdlx;
        public string bdjg;
        public string jczbh;
        public string jcgwh;

        public string xysj;
        public string wdsj;
        public string jzl1;
        public string jzl2;
        public string qhsdd;
        public string jcjg;
        public XysjBD(DataRow BDData)
        {
            sbbh = BDData["SBBH"].ToString();
            bdsj = DateTime.Parse(BDData["BDSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            bdr = BDData["BDR"].ToString();
            bdlx = BDData["BDLX"].ToString();
            bdjg = BDData["BDJG"].ToString();
            jczbh = BDData["JCZBH"].ToString();
            jcgwh = BDData["JCGWH"].ToString();
            xysj = BDData["DATA1"].ToString();
            wdsj = BDData["DATA2"].ToString();
            jzl1 = BDData["DATA3"].ToString();
            jzl2 = BDData["DATA4"].ToString();
            qhsdd = BDData["DATA5"].ToString();
            jcjg = BDData["DATA6"].ToString();
        }
    }
    #endregion
    //遥感检测数据——YG001
    #endregion
}