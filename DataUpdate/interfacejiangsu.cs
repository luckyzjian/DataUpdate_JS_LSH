using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Web;
namespace DataUpdate
{

    class interfacejiangsu
    {
        /// <summary>
        /// utf8码转换 解码
        /// </summary>
        /// <param name="str1"></param>
        /// <returns></returns>
        public string UrlDecode(string str1)
        {
            string cllx = HttpUtility.UrlDecode(str1, Encoding.UTF8);
            return cllx;
        }

        /// <summary>
        /// 获取车辆列表及列表内车辆基本信息 如果是平台录入 这里需调用该接口获取 checkid
        /// </summary>
        /// <param name="token">登陆成功后返回的唯一标识字符串</param>
        /// <param name="unitid">机构编号</param>
        /// <returns></returns>
        public DataTable GetCheckList(String token, String unitid)
        {
            DataTable dt = new DataTable();
            //江苏车辆编码是从平台获取的
            dt.Columns.Add("CLID");
            dt.Columns.Add("jylsh");
            dt.Columns.Add("jycs");
            dt.Columns.Add("reallsh");
            dt.Columns.Add("hphm");
            dt.Columns.Add("hpys");
            dt.Columns.Add("hpzl");
            dt.Columns.Add("cllx");
            dt.Columns.Add("cpxh");
            dt.Columns.Add("cpmc");
            dt.Columns.Add("clsbm");
            dt.Columns.Add("clscqy");
            dt.Columns.Add("fdjxh");
            dt.Columns.Add("fdjscqy");
            dt.Columns.Add("fdjh");
            dt.Columns.Add("fdjpl");
            dt.Columns.Add("fdjedzs");
            dt.Columns.Add("fdjedgl");
            dt.Columns.Add("syxz");
            dt.Columns.Add("ccdjrq");
            dt.Columns.Add("ccrq");
            dt.Columns.Add("czmc");
            dt.Columns.Add("lxdh");
            dt.Columns.Add("lxdz");
            dt.Columns.Add("pfbz");
            dt.Columns.Add("bsqxs");
            dt.Columns.Add("jqfs");
            dt.Columns.Add("ryzl");
            dt.Columns.Add("gyfs");
            dt.Columns.Add("qdfs");
            dt.Columns.Add("zdzzl");
            dt.Columns.Add("jzzl");
            dt.Columns.Add("zbzl");
            dt.Columns.Add("qgs");
            dt.Columns.Add("qdltqy");
            dt.Columns.Add("chzhq");
            dt.Columns.Add("rygg");
            dt.Columns.Add("sjcys");
            dt.Columns.Add("ssxq");
            dt.Columns.Add("dws");
            dt.Columns.Add("jclx");
            dt.Columns.Add("jcff");
            dt.Columns.Add("obd");
            dt.Columns.Add("jczbh");
            dt.Columns.Add("ljxslc");
            dt.Columns.Add("dlsj");
            dt.Columns.Add("scqy");
            dt.Columns.Add("pp");
            dt.Columns.Add("xh");
            DataUpdate.FileOpreate.SaveLog("获取待检列表发送：\r\nunitid="+unitid, "[江苏联网]", 3);
            string strCon = ModPublicJiangsu.coa.GetCheckList(token, unitid);
            DataUpdate.FileOpreate.SaveLog("获取待检列表返回：\r\n"+strCon, "[江苏联网]", 3);
            XmlDocument xmlDoc = new XmlDocument();
            string cllx = UrlDecode(strCon);
            xmlDoc.LoadXml(UrlDecode(strCon));
            XmlNodeList xnList = xmlDoc.SelectNodes("//vehicleitem");
            foreach (XmlNode xn in xnList)
            {
                //无法使用xn["ActivityId"].InnerText  
                string VIN = (xn.SelectSingleNode("vin")).InnerText;//LJDHAA125A0008942
                string LSH = xn.SelectSingleNode("checkid").InnerText;//320900991905022304269504

                strCon = ModPublicJiangsu.coa.GetVehicle(token, unitid, VIN);

                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(UrlDecode(strCon));
                XmlNode node1 = xmlDoc.SelectSingleNode("xml");
                XmlNode node2 = node1.SelectSingleNode("status");
                String state = node1.SelectSingleNode("status").InnerText;
                if (state == "true")
                {
                    XmlNode node3 = node1.SelectSingleNode("VEHICLE_INFO");
                    XmlNode node4 = node3.SelectSingleNode("VEHICLE_INFO_CONTENT");
                    string timestring;
                    try
                    {
                        DataRow dr = dt.NewRow();
                        dr["CLID"] =  node4.SelectSingleNode("VEHICLE_ID").InnerText;
                        dr["jylsh"] = node4.SelectSingleNode("VEHICLE_ID").InnerText;// LSH;// node4.SelectSingleNode("VEHICLE_ID").InnerText;
                        dr["jycs"] = "1";// child1.insp.TestTimes;
                        dr["reallsh"] = LSH;//真实流水号
                        string Hphm = node4.SelectSingleNode("PLATE").InnerText;
                        dr["hphm"] = node4.SelectSingleNode("PLATE").InnerText;
                        dr["hpys"] = GetCpys(node4.SelectSingleNode("PLATE_COLOR").InnerText);
                        dr["hpzl"] = GetHpzl(node4.SelectSingleNode("PLATE_TYPE").InnerText);
                        dr["cllx"] = node4.SelectSingleNode("VEHICLE_TYPE").InnerText;

                        dr["cpxh"] = node4.SelectSingleNode("CLXH").InnerText;
                        dr["cpmc"] = node4.SelectSingleNode("CLSB").InnerText;

                        dr["scqy"] = node4.SelectSingleNode("FACTORY_NAME").InnerText;//生产厂家
                        dr["pp"] = node4.SelectSingleNode("CLSB").InnerText;//品牌
                        dr["xh"] = node4.SelectSingleNode("CLXH").InnerText;//型号

                        dr["clsbm"] = node4.SelectSingleNode("VIN").InnerText;
                        dr["clscqy"] = node4.SelectSingleNode("FACTORY_NAME").InnerText.ToString().Trim();
                        dr["fdjxh"] = node4.SelectSingleNode("FDJXH").InnerText;
                        dr["fdjscqy"] = "";
                        dr["fdjh"] = node4.SelectSingleNode("ENGINE_NO").InnerText;

                        dr["fdjpl"] = node4.SelectSingleNode("EXHAUST_QUANTITY").InnerText.ToString().Trim();

                        if (node4.SelectSingleNode("ORDAIN_REV").InnerText == "")
                            dr["fdjedzs"] = "0";
                        else
                            dr["fdjedzs"] = node4.SelectSingleNode("ORDAIN_REV").InnerText;
                        dr["fdjedgl"] = node4.SelectSingleNode("RATING_POWER").InnerText;

                        string syxzcode = string.Empty;
                        string syxzstr = string.Empty;
                        syxzcode = node4.SelectSingleNode("USAGE_NATURE").InnerText;
                        syxzstr = GetSyxz(syxzcode);


                        dr["syxz"] = syxzstr;
                        try
                        {
                            timestring = node4.SelectSingleNode("MANUFACTURE_DATE").InnerText;
                            //timestring = timestring.Substring(0, 4) + "/" + timestring.Substring(4, 2) + "/" + timestring.Substring(6, 2) + " " + timestring.Substring(8, 2) + ":" + timestring.Substring(10, 2) + ":" + timestring.Substring(12, 2);
                            dr["ccdjrq"] = DateTime.Parse(timestring).ToString("yyyy-MM-dd");
                        }
                        catch
                        {
                            dr["ccdjrq"] = DateTime.Now;
                        }
                        dr["ccrq"] = dr["CCDJRQ"];
                        dr["czmc"] = node4.SelectSingleNode("OWNER").InnerText;
                        dr["lxdh"] = node4.SelectSingleNode("PHONE").InnerText;
                        dr["lxdz"] = node4.SelectSingleNode("OWNERADDRESS").InnerText;
                        string pfid = node4.SelectSingleNode("STANDARD_ID").InnerText;
                        dr["pfbz"] = GetPfbz(pfid);
                        dr["bsqxs"] = GetBsxxs(node4.SelectSingleNode("DRIVE_FORM").InnerText);
                        dr["jqfs"] = ""; //node4.SelectSingleNode("ADDMISSION").InnerText;

                        string ryzlcode = string.Empty;
                        string ryzlstr = string.Empty;
                        ryzlcode = node4.SelectSingleNode("FUEL_TYPE").InnerText;
                        ryzlstr = GetRllx(ryzlcode);

                        dr["ryzl"] = ryzlstr;

                        string gyfs = node4.SelectSingleNode("SUPPLY_MODE").InnerText.ToString().Trim();
                        dr["gyfs"] = GetGyfs(gyfs);

                        dr["qdfs"] = GetQdxs(node4.SelectSingleNode("DRIVE_MODE").InnerText);
                        //if (int.TryParse(child1.insp_vehicle.GVM, out a))
                        dr["zdzzl"] = node4.SelectSingleNode("MAXWEIGHT").InnerText;
                        //else
                        //    dr["zdzzl"] = "1500";
                        //if (int.TryParse(child1.insp_vehicle.RM, out a))
                        //    dr["jzzl"] = child1.insp_vehicle.RM;
                        //else
                        dr["jzzl"] = node4.SelectSingleNode("STDWEIGHT").InnerText;
                        dr["zbzl"] = (int.Parse(dr["JZZL"].ToString()) - 100).ToString();// child1.insp_vehicle.VIN;
                        dr["qgs"] = node4.SelectSingleNode("CYLINDER").InnerText;
                        dr["qdltqy"] = "";
                        dr["chzhq"] = "";
                        dr["rygg"] = "";
                        dr["sjcys"] = node4.SelectSingleNode("SEAT_CAPACITY").InnerText;
                        dr["ssxq"] = "";
                        dr["dws"] = "";
                        dr["jclx"] = "";

                        dr["jcff"] = GetJcff(node4.SelectSingleNode("CHECK_METHOD").InnerText);

                        dr["obd"] = node4.SelectSingleNode("HAS_ODB").InnerText;
                        dr["jczbh"] = ModPublicJiangsu.unitid;
                        dr["ljxslc"] = node4.SelectSingleNode("ODO_METER").InnerText;

                        dr["dlsj"] = DateTime.Now;

                        dt.Rows.Add(dr);
                    }
                    catch (Exception er)
                    {
                        FileOpreate.SaveLog("解析车辆信息失败，license=" + strCon + ",exception:" + er.Message, "[downInspRegInfo_Parser]：", 3);
                    }

                    FileOpreate.SaveLog("车辆信息，license=" + strCon, "[downInspRegInfo_Parser]：", 3);
                }
                else
                {
                    FileOpreate.SaveLog("获取信息失败", "[downInspRegInfo_Parser]：", 3);
                }
            }
            return dt;
        }

        /// <summary>
        /// 车辆开始检测
        /// </summary>
        /// <param name="obj_car"></param>
        /// <returns></returns>
        public bool BeginCheck(object obj_car,string qwg_base64,string hwg_base64,string hcl_base64)
        {
            //string lineid = "0000000000";
            string plate = "";
            string vin = "";
            string lineid = ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_car)["LINEID"].ToString(), "00000000000");
            plate = ((DataRow)obj_car)["CLHP"].ToString();
            vin = ((DataRow)obj_car)["CLSBM"].ToString();
            DataUpdate.FileOpreate.SaveLog(string.Format("开始检测发送：\r\nunitid={0}\r\nlineid={1}\r\nplate={2}\r\nvin={3}\r\nqwg={4}\r\nhwg={5}\r\nhcl={6}",
                ModPublicJiangsu.unitid,lineid,plate,vin,"此处省略...", "此处省略...", "此处省略..."), "[江苏联网]", 3);
            string strCon = ModPublicJiangsu.coa.BeginCheck(ModPublicJiangsu.unitid, lineid, plate, vin, qwg_base64,hwg_base64,hcl_base64);

            DataUpdate.FileOpreate.SaveLog("开始检测返回：\r\n" + strCon, "[江苏联网]", 3);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(UrlDecode(strCon));
            XmlNode node1 = xmlDoc.SelectSingleNode("xml");
            String state = node1.SelectSingleNode("status").InnerText;
            return state == "true" ? true : false;
        }

        /// <summary>
        /// 外检
        /// </summary>
        /// <param name="obj_car"></param>
        /// <param name="obj_data"></param>
        /// <param name="obj_process"></param>
        /// <param name="XmlStr"></param>
        /// <returns></returns>
        public bool CarOutLook(object obj_car, object obj_data, object obj_process,string reallsh, out string XmlStr)
        {
            try
            {
                string UserName = ((DataRow)obj_car)["JSY"].ToString();
                string CLPH = ((DataRow)obj_car)["CLHP"].ToString();
                string VIN = ((DataRow)obj_car)["CLSBM"].ToString();
                string Clxh = ((DataRow)obj_car)["XH"].ToString();
                string LSH = reallsh;// ((DataRow)obj_car)["JYLSH"].ToString();
                string JCSJ1 = ((DataRow)obj_car)["JCSJ"].ToString();
                string JCSJ = Convert.ToDateTime(JCSJ1).ToString("yyyy-MM-dd");
                string OBD = ((DataRow)obj_car)["OBD"].ToString();
                string JCFF = ((DataRow)obj_car)["JCFF"].ToString();
                string XSLC= ((DataRow)obj_car)["XSLC"].ToString();
                if (XSLC == "") XSLC = "00000";
                string obdStr = "";
                string jcffStr = "";

                if (OBD == "是" || OBD == "有" || OBD == "Y")
                    obdStr = "1";
                else
                    obdStr = "0";
                if (JCFF == "SDS" || JCFF == "ZYJS")
                    jcffStr = "0";
                else
                    jcffStr = "1";
                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                //check_id是由登录某辆车成功后返回的check_id
                cxtj.Append("<check_id>" + LSH + "</check_id>");
                cxtj.Append("<city_code>" + ModPublicJiangsu.CityCode + "</city_code>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<user_id>" + ModPublicJiangsu.LwUserName + "</user_id>");
                cxtj.Append("<uname>" + UserName + "</uname>");
                cxtj.Append("<vin>" + VIN + "</vin>");
                cxtj.Append("<clxh>" + Clxh + "</clxh>");
                cxtj.Append("<plate>" + CLPH + "</plate>");
                cxtj.Append("<check_date>" + JCSJ + "</check_date>");
                cxtj.Append("<odometer>"+XSLC+ "</odometer>");
                cxtj.Append("<jxzk>1</jxzk>");//机械状况是否良好 0：否 1：是
                cxtj.Append("<wrkzzz>1</wrkzzz>");//污染控制装置是否齐全、正常
                cxtj.Append("<qzxtfxt>1</qzxtfxt>");//曲轴箱通风系统是否正常
                cxtj.Append("<ryzfkzxt>1</ryzfkzxt>");//燃油蒸发控制系统是否正常
                cxtj.Append("<ybgz>1</ybgz>");//仪表工作是否正常
                cxtj.Append("<yxaqjxgz>0</yxaqjxgz>");//有无影响安全或引起测试偏差的机械故障
                cxtj.Append("<sjymhy>0</sjymhy>");//是否存在烧机油或严重冒黑烟状况
                cxtj.Append("<pqxtxl>0</pqxtxl>");//进排气系统是否存在泄露
                cxtj.Append("<ytsl>0</ytsl>");//发动机、变速箱等有无液体渗漏情况
                cxtj.Append("<hasobd>" + obdStr + "</hasobd>");//是否带 OBD
                cxtj.Append("<ltpre>1</ltpre>");//轮胎气压是否正常
                cxtj.Append("<ltgz>1</ltgz>");//轮胎是否干燥、清洁
                cxtj.Append("<closefssb>1</closefssb>");//是否关闭空调、暖风等附属设备
                cxtj.Append("<closeyxgzkz>1</closeyxgzkz>");//是否关闭 ESP、ARS 等可能影响测试的功能
                cxtj.Append("<fuelgz>0</fuelgz>");//油箱和油品是否正常
                cxtj.Append("<fdjcydkb>0</fdjcydkb>");//发动机燃油系统采用电控泵
                cxtj.Append("<isasm>" + jcffStr + "</isasm>");//是否适合工况法检测
                cxtj.Append("<wgpics>1.jpg</wgpics>");// 外观图片名称，多张图片名称中间用逗号间隔
                cxtj.Append("<passed>1</passed>"); //检测结果
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, "[interfacejiangsuOutlook", 3);
                XmlStr = strCon; return true;
            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog("生成江苏外检XML文件时出错" + ex.Message, "creat interfacejiangsu_WjXml error", 4);
                FileOpreate.SaveLog("失败", "[生成WjXml_interfacejiangsu]：", 3);
                XmlStr = null; return false;
            }
        }


        /// <summary>
        /// 上传检测结果
        /// </summary>
        /// <param name="token">登陆成功后返回的唯一标识字符串</param>
        /// <param name="unitid">机构编号</param>
        /// <param name="checkresult">字符串型，以xml字符串传入检测结</param>
        /// <param name="checkmethod">检测方法, 1、双怠速法；2、稳态工况法，3、简易瞬态工况；4、加载减速；5、不透光烟度法；6、汽油车OBD数据；7、柴油车OBD数据；8、外观检测；9、OBD过程数据。</param>
        /// <returns></returns>
        public bool UploadInspectionResult(object obj_car, object obj_data, object obj_process, object obj_obd, string checkmethod,string reallsh,out string bgbh,out string qcrode)
        {
            bgbh = "";
            qcrode = "";
            try
            {
                string Result = "";
                int Jcff_JiangSu = 0;

                if (checkmethod != "outlook")
                {
                    if (ModPublicJiangsu.getPicFromXcPDA)
                    {
                        string JYLSH = ((DataRow)obj_car)["JYLSH"].ToString();
                        string JCCS = ((DataRow)obj_car)["JCCS"].ToString();
                        string[] piclx = { "1", "2", "3" };
                        string[] piccontent = { "", "", "" };
                        for (int i = 0; i < piclx.Length; i++)
                        {
                            string code, msg;
                            JbtNetLibrary.OBD_lr.outlookPic pic;
                            if (ModPublicJiangsu.wjserver.getOutlookPic(JYLSH, JCCS, piclx[i], out code, out msg, out pic))
                                piccontent[i] = pic.photo;
                        }
                        if (!BeginCheck(obj_car, piccontent[0], piccontent[1], piccontent[2]))
                            return false;
                    }
                    else
                    {
                        if (!BeginCheck(obj_car, "", "", ""))
                            return false;
                    }
                }

                switch (checkmethod)
                {
                    case "outlook":
                        Jcff_JiangSu = 8;
                        CarOutLook(obj_car, obj_data, obj_process, reallsh, out Result);
                        break;
                    case "ZYJS":
                        Jcff_JiangSu = 5;
                        if (obj_obd != null)
                        {
                            CycOBD(obj_car, obj_obd,reallsh);
                            CycOBDProcess(obj_car, obj_process, obj_obd, reallsh);
                        }

                        Btg(obj_car, obj_data, obj_process, reallsh, out Result);
                        break;
                    case "JZJS":
                        Jcff_JiangSu = 4;
                        if (obj_obd != null)
                        {
                            CycOBD(obj_car, obj_obd, reallsh);
                            CycOBDProcess(obj_car, obj_process, obj_obd, reallsh);
                        }
                        Jzjs(obj_car, obj_data, obj_process, reallsh, out Result);
                        break;
                    case "SDS":
                        Jcff_JiangSu = 1;
                        if (obj_obd != null)
                        {
                            QycOBD(obj_car, obj_process, obj_obd, reallsh);
                            QycOBDProcess(obj_car, obj_process, obj_obd, reallsh);
                        }
                        SDS(obj_car, obj_data, obj_process, reallsh, out Result);
                        break;
                    case "ASM":
                        Jcff_JiangSu = 2;
                        if (obj_obd != null)
                        {
                            QycOBD(obj_car, obj_process, obj_obd, reallsh);
                            QycOBDProcess(obj_car, obj_process, obj_obd, reallsh);
                        }
                        ASM(obj_car, obj_data, obj_process, reallsh, out Result);
                        break;
                }
               
                string UpResult = ModPublicJiangsu.coa.UploadInspectionResult("", ModPublicJiangsu.unitid, Result, Jcff_JiangSu);
                FileOpreate.SaveLog(UpResult, "[江苏上传" + checkmethod + "]：", 3);
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.LoadXml(UrlDecode(UpResult));
                xmlDoc.LoadXml(UpResult);
                XmlNode node1 = xmlDoc.SelectSingleNode("xml");
                String state = node1.SelectSingleNode("status").InnerText;
                if (state == "true")
                {
                    if (checkmethod != "outlook")
                    {
                        bgbh = node1.SelectSingleNode("bgbh").InnerText;
                        //qcrode = node1.SelectSingleNode("qcrode").InnerText;
                        qcrode = node1.SelectSingleNode("ewm").InnerText;
                    }
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog("江苏上传时出错" + ex.Message, "UploadInspectionResult " + checkmethod + " error", 4);
                FileOpreate.SaveLog("失败", "[江苏上传时出错在UploadInspectionResult]：", 3);
                return false;
            }
        }

        /// <summary>
        /// 双怠速XML
        /// </summary>
        /// <param name="obj_car">已检车辆信息</param>
        /// <param name="obj_data">SDS</param>
        /// <param name="obj_process">SDS_DATASECONDS</param>
        /// <returns></returns>
        public bool SDS(object obj_car, object obj_data, object obj_process, string reallsh, out string XmlStr)
        {
            try
            {
                //表 SDS
                string SHY = ((DataRow)obj_data)["SHY"].ToString();
                string CLID = ((DataRow)obj_data)["CLID"].ToString();
                CLID = CLID.Substring(0, CLID.IndexOf("&"));
                string CLPH = ((DataRow)obj_data)["CLPH"].ToString();
                string LINEID = ((DataRow)obj_car)["LINEID"].ToString();
                string VIN = ((DataRow)obj_car)["CLSBM"].ToString();
                string LSH = reallsh;// ((DataRow)obj_car)["LSH"].ToString();
                string JCSJ1 = ((DataRow)obj_car)["JCSJ"].ToString();
                string JCSJ = Convert.ToDateTime(JCSJ1).ToString("yyyy-MM-dd");

                string StartDate = Convert.ToDateTime(((DataRow)obj_data)["JCKSSJ"].ToString()).ToString("yyyy-MM");
                string EndDate = Convert.ToDateTime(((DataRow)obj_data)["JCJSSJ"].ToString()).ToString("yyyy-MM");

                string StartTime = Convert.ToDateTime(((DataRow)obj_data)["JCKSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                string EndTime = Convert.ToDateTime(((DataRow)obj_data)["JCJSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                string WD = ((DataRow)obj_data)["WD"].ToString();
                string SD = ((DataRow)obj_data)["SD"].ToString();
                string DQY = ((DataRow)obj_data)["DQY"].ToString();
                string YW = ((DataRow)obj_data)["YW"].ToString();
                string GLKQXSSX = ((DataRow)obj_data)["GLKQXSSX"].ToString();
                string GLKQXSXX = ((DataRow)obj_data)["GLKQXSXX"].ToString();
                string LAMDAHIGHCLZ = ((DataRow)obj_data)["LAMDAHIGHCLZ"].ToString();
                string LAMDAHIGHPD = ((DataRow)obj_data)["LAMDAHIGHPD"].ToString() == "不合格" ? "0" : "1";
                string COLOWCLZ = ((DataRow)obj_data)["COLOWCLZ"].ToString();
                string COLOWXZ = ((DataRow)obj_data)["COLOWXZ"].ToString();
                string COLOWPD = ((DataRow)obj_data)["COLOWPD"].ToString() == "不合格" ? "0" : "1";

                string HCLOWCLZ = ((DataRow)obj_data)["HCLOWCLZ"].ToString();
                string HCLOWXZ = ((DataRow)obj_data)["HCLOWXZ"].ToString();
                string HCLOWPD = ((DataRow)obj_data)["HCLOWPD"].ToString() == "不合格" ? "0" : "1";

                string COHIGHCLZ = ((DataRow)obj_data)["COHIGHCLZ"].ToString();
                string COHIGHXZ = ((DataRow)obj_data)["COHIGHXZ"].ToString();
                string COHIGHPD = ((DataRow)obj_data)["COHIGHPD"].ToString() == "不合格" ? "0" : "1";

                string HCHIGHCLZ = ((DataRow)obj_data)["HCHIGHCLZ"].ToString();
                string HCHIGHXZ = ((DataRow)obj_data)["HCHIGHXZ"].ToString();
                string HCHIGHPD = ((DataRow)obj_data)["HCHIGHPD"].ToString() == "不合格" ? "0" : "1";

                string ZHPD = ((DataRow)obj_data)["ZHPD"].ToString() == "不合格" ? "0" : "1";

                //表SDS_DATASECONDS
                string[] MMTIME = ((DataRow)obj_process)["MMTIME"].ToString().Split(',');
                string[] MMLB = ((DataRow)obj_process)["MMLB"].ToString().Split(',');
                string[] MMHC = ((DataRow)obj_process)["MMHC"].ToString().Split(',');
                string[] MMCO = ((DataRow)obj_process)["MMCO"].ToString().Split(',');
                string[] MMO2 = ((DataRow)obj_process)["MMO2"].ToString().Split(',');
                string[] MMCO2 = ((DataRow)obj_process)["MMCO2"].ToString().Split(',');
                string[] MMLAMDA = ((DataRow)obj_process)["MMLAMDA"].ToString().Split(',');
                string[] MMZS = ((DataRow)obj_process)["MMZS"].ToString().Split(',');
                string[] MMYW = ((DataRow)obj_process)["MMYW"].ToString().Split(',');
                string[] MMWD = ((DataRow)obj_process)["MMWD"].ToString().Split(',');
                string[] MMSD = ((DataRow)obj_process)["MMSD"].ToString().Split(',');
                string[] MMDQY = ((DataRow)obj_process)["MMDQY"].ToString().Split(',');

                int count = MMLB.Count();

                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                //check_id是由登录某辆车成功后返回的check_id 检测编号(由接口下发)
                cxtj.Append("<check_id>" + LSH + "</check_id>");
                //检测类型：1年检；2新注册车辆；3外地车转入；4实验比对；5 政府部门监督抽查的复检
                cxtj.Append("<check_type>1</check_type>");
                //所在检测站的城市代码
                cxtj.Append("<city_code>" + ModPublicJiangsu.CityCode + "</city_code>");
                //检测机构编号（由平台提供）
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                //检测线编号（由平台提供）
                cxtj.Append("<line_id>"+ModPublicJiangsu.DicLineid.GetValue(LINEID,"00000000000" )+ "</line_id>");
                //检测用户登录名（由平台提供）
                cxtj.Append("<user_id>" + ModPublicJiangsu.LwUserName + "</user_id>");
                //检验员姓名
                cxtj.Append("<uname>" + SHY + "</uname>");
                //车辆编码
                //CLID = "3600501900023";
                cxtj.Append("<vehicle_id>" + CLID + "</vehicle_id>");
                //车架号
                cxtj.Append("<vin>" + VIN + "</vin>");
                //车牌
                cxtj.Append("<plate>" + CLPH + "</plate>");
                cxtj.Append("<fxyjddate>" + ModPublicJiangsu.DicLineYxq.GetValue(LINEID, DateTime.Now).ToString("yyyy-MM-dd") + "</fxyjddate>");
                //检测日期，（yyyy-mm-dd)
                cxtj.Append("<check_date>" + JCSJ + "</check_date>");
                //检测周期开始日期(yyyy-mm)
                cxtj.Append("<period_start_date>" + StartDate + "</period_start_date>");
                //检测周期结束日期(yyyy-mm)
                cxtj.Append("<period_end_date>" + EndDate + "</period_end_date>");
                //检测开始时间。精确到秒（yyyy-mm-dd hh24:mi:ss）
                cxtj.Append("<start_time>" + StartTime + "</start_time>");
                //检测结束时间。精确到秒（yyyy-mm-dd hh24:mi:ss）
                cxtj.Append("<end_time>" + EndTime + "</end_time>");
                //环境温度（°C）
                cxtj.Append("<temperature>" + WD + "</temperature>");
                //大气压（kPa）
                cxtj.Append("<pressure>" + DQY + "</pressure>");
                //相对湿度（%）
                cxtj.Append("<humidity>" + SD + "</humidity>");
                //过量空气系数限值下限
                cxtj.Append("<lambda_limit_l>" + GLKQXSSX + "</lambda_limit_l>");
                //过量空气系数限值上限
                cxtj.Append("<lambda_limit_h>" + GLKQXSXX + "</lambda_limit_h>");
                //过量空气系数Lambda值
                cxtj.Append("<lambda>" + LAMDAHIGHCLZ + "</lambda>");
                //过量空气系数Lambda是否合格：0：不合格 1：合格
                cxtj.Append("<lambda_passed>" + LAMDAHIGHPD + "</lambda_passed>");
                //低怠速 CO测量限值（%）
                cxtj.Append("<low_co_limit>" + COLOWXZ + "</low_co_limit>");
                //低怠速 CO测量结果（%）
                cxtj.Append("<low_co>" + COLOWCLZ + "</low_co>");
                //低怠速 CO 是否合格：0：不合格 1：合格
                cxtj.Append("<low_co_passed>" + COLOWPD + "</low_co_passed>");
                //低怠速 HC 测量限值（10-6）
                cxtj.Append("<low_hc_limit>" + HCLOWXZ + "</low_hc_limit>");
                //低怠速 HC 测量结果（10-6）
                cxtj.Append("<low_hc>" + HCLOWCLZ + "</low_hc>");
                //低怠速 HC 是否合格：0：不合格 1：合格
                cxtj.Append("<low_hc_passed>" + HCLOWPD + "</low_hc_passed>");
                //高怠速 CO测量限值（%）
                cxtj.Append("<high_co_limit>" + COHIGHXZ + "</high_co_limit>");
                //高怠速 CO测量结果（%）
                cxtj.Append("<high_co>" + COHIGHCLZ + "</high_co>");
                //高怠速 CO 是否合格：0：不合格 1：合格
                cxtj.Append("<high_co_passed>" + COHIGHPD + "</high_co_passed>");
                //高怠速 hc测量限值（10-6）
                cxtj.Append("<high_hc_limit>" + HCHIGHXZ + "</high_hc_limit>");
                //高怠速 hc测量结果（10-6）
                cxtj.Append("<high_hc>" + HCHIGHCLZ + "</high_hc>");
                //高怠速 HC 是否合格：0：不合格 1：合格
                cxtj.Append("<high_hc_passed>" + HCHIGHPD + "</high_hc_passed>");
                //检测结果：0：不合格 1：合格 2：中止 3：无效
                cxtj.Append("<passed>" + ZHPD + "</passed>");
                cxtj.Append("<sqqzrname>" + FileOpreate.NM_Sqr + "</sqqzrname>");
                cxtj.Append("<jyrname>" + ((DataRow)obj_car)["CZY"].ToString() + "</jyrname>");
                cxtj.Append("<pzrname>" + FileOpreate.NM_Zlkzr + "</pzrname>");
                cxtj.Append("</result_data>");

                //过程数据 可以有多个process_data
                for (int i = 0; i < count; i++)
                {
                    cxtj.Append("<process_data>");
                    //全程时序,格式为YYYYMMDD24hmmss
                    cxtj.Append("<time>" + Convert.ToDateTime(MMTIME[i]).ToString("yyyyMMddHHmmss") + "</time>");
                    //工况类型：0-70%额定转速、1-高怠速准备、2-高怠速检测,、3-怠速准备、4-怠速检测
                    cxtj.Append("<idle_type>" + MMLB[i] + "</idle_type>");
                    //采样时序（1开始的序号，1秒一个数据）
                    cxtj.Append("<time_no>" + (i + 1) + "</time_no>");
                    //HC测量值
                    cxtj.Append("<hc>" + MMHC[i] + "</hc>");
                    //CO测量值
                    cxtj.Append("<co>" + MMCO[i] + "</co>");
                    //油温实时值
                    cxtj.Append("<ywssz>" + MMYW[i] + "</ywssz>");
                    //环境温度
                    cxtj.Append("<hjwd>" + MMWD[i] + "</hjwd>");
                    //相对湿度
                    cxtj.Append("<xdsd>" + MMSD[i] + "</xdsd>");
                    //大气压力
                    cxtj.Append("<dqy>" + MMDQY[i] + "</dqy>");
                    //O2测量值
                    cxtj.Append("<o2>" + MMO2[i] + "</o2>");
                    //CO2测量值
                    cxtj.Append("<co2>" + MMCO2[i] + "</co2>");
                    //过量空气系数
                    cxtj.Append("<p_lambda>" + MMLAMDA[i] + "</p_lambda>");
                    //转速
                    cxtj.Append("<rpm>" + MMZS[i] + "</rpm>");
                    cxtj.Append("</process_data>");
                }
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();
                FileOpreate.SaveLog(strCon, "[interfacejiangsuSds]：", 3);
                XmlStr = strCon; return true;
            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog("生成江苏双怠速XML文件时出错" + ex.Message, "creat interfacejiangsu_SdsXml error", 4);
                FileOpreate.SaveLog("失败", "[生成SdsXml_interfacejiangsu]：", 3);
                XmlStr = null; return false;
            }

        }

        /// <summary>
        /// 稳态XML
        /// </summary>
        /// <param name="obj_car">已检车辆信息</param>
        /// <param name="obj_data">ASM</param>
        /// <param name="obj_process">ASM_DATASECONDS</param>
        /// <returns></returns>
        public bool ASM(object obj_car, object obj_data, object obj_process,string reallsh, out string XmlStr)
        {
            try
            {
                //表 ASM
                string SHY = ((DataRow)obj_data)["SHY"].ToString();
                string CLID = ((DataRow)obj_data)["CLID"].ToString();
                CLID = CLID.Substring(0, CLID.IndexOf("&"));
                string CLPH = ((DataRow)obj_data)["CLPH"].ToString();
                string LINEID = ((DataRow)obj_car)["LINEID"].ToString();
                string VIN = ((DataRow)obj_car)["CLSBM"].ToString();
                string LSH = reallsh;// ((DataRow)obj_car)["LSH"].ToString();

                string JZZGL5025 = ((DataRow)obj_data)["JZZGL5025"].ToString();
                string JZZGL2540 = ((DataRow)obj_data)["JZZGL2540"].ToString();

                string FDJZS5025 = ((DataRow)obj_data)["FDJZS5025"].ToString();
                string FDJZS2540 = ((DataRow)obj_data)["FDJZS2540"].ToString();

                string JCSJ1 = ((DataRow)obj_car)["JCSJ"].ToString();
                string JCSJ = Convert.ToDateTime(JCSJ1).ToString("yyyy-MM-dd");
                string StartDate = Convert.ToDateTime(((DataRow)obj_data)["JCKSSJ"].ToString()).ToString("yyyy-MM");
                string EndDate = Convert.ToDateTime(((DataRow)obj_data)["JCJSSJ"].ToString()).ToString("yyyy-MM");

                string StartTime = Convert.ToDateTime(((DataRow)obj_data)["JCKSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                string EndTime = Convert.ToDateTime(((DataRow)obj_data)["JCJSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");

                string WD = ((DataRow)obj_data)["WD"].ToString();
                string SD = ((DataRow)obj_data)["SD"].ToString();
                string DQY = ((DataRow)obj_data)["DQY"].ToString();
                string YW = ((DataRow)obj_data)["YW"].ToString();

                string LAMBDA5025 = ((DataRow)obj_data)["LAMBDA5025"].ToString();
                string LAMBDA2540 = ((DataRow)obj_data)["LAMBDA2540"].ToString();

                string CO25CLZ = ((DataRow)obj_data)["CO25CLZ"].ToString();
                string CO25XZ = ((DataRow)obj_data)["CO25XZ"].ToString();
                string CO25PD = ((DataRow)obj_data)["CO25PD"].ToString() == "不合格" ? "0" : "1";

                string NOX25CLZ = ((DataRow)obj_data)["NOX25CLZ"].ToString();
                string NOX25XZ = ((DataRow)obj_data)["NOX25XZ"].ToString();
                string NOX25PD = ((DataRow)obj_data)["NOX25PD"].ToString() == "不合格" ? "0" : "1";

                string HC25CLZ = ((DataRow)obj_data)["HC25CLZ"].ToString();
                string HC25XZ = ((DataRow)obj_data)["HC25XZ"].ToString();
                string HC25PD = ((DataRow)obj_data)["HC25PD"].ToString() == "不合格" ? "0" : "1";

                string CO40CLZ = ((DataRow)obj_data)["CO40CLZ"].ToString();
                string CO40XZ = ((DataRow)obj_data)["CO40XZ"].ToString();
                string CO40PD = ((DataRow)obj_data)["CO40PD"].ToString() == "不合格" ? "0" : "1";

                string NOX40CLZ = ((DataRow)obj_data)["NOX40CLZ"].ToString();
                string NOX40XZ = ((DataRow)obj_data)["NOX40XZ"].ToString();
                string NOX40PD = ((DataRow)obj_data)["NOX40PD"].ToString() == "不合格" ? "0" : "1";

                string HC40CLZ = ((DataRow)obj_data)["HC40CLZ"].ToString();
                string HC40XZ = ((DataRow)obj_data)["HC40XZ"].ToString();
                string HC40PD = ((DataRow)obj_data)["HC40PD"].ToString() == "不合格" ? "0" : "1";
                string ZHPD = ((DataRow)obj_data)["ZHPD"].ToString() == "不合格" ? "0" : "1";

                //表ASM_DATASECONDS
                string[] MMTIME = ((DataRow)obj_process)["MMTIME"].ToString().Split(',');
                string[] MMLB = ((DataRow)obj_process)["MMLB"].ToString().Split(',');
                string[] MMCO = ((DataRow)obj_process)["MMCO"].ToString().Split(',');
                string[] MMHC = ((DataRow)obj_process)["MMHC"].ToString().Split(',');
                string[] MMNO = ((DataRow)obj_process)["MMNO"].ToString().Split(',');
                string[] MMCO2 = ((DataRow)obj_process)["MMCO2"].ToString().Split(',');
                string[] MMO2 = ((DataRow)obj_process)["MMO2"].ToString().Split(',');
                string[] MMWD = ((DataRow)obj_process)["MMWD"].ToString().Split(',');
                string[] MMSD = ((DataRow)obj_process)["MMSD"].ToString().Split(',');
                string[] MMDQY = ((DataRow)obj_process)["MMDQY"].ToString().Split(',');
                string[] MMCS = ((DataRow)obj_process)["MMCS"].ToString().Split(',');
                string[] MMGL = ((DataRow)obj_process)["MMGL"].ToString().Split(',');
                string[] MMZS = ((DataRow)obj_process)["MMZS"].ToString().Split(',');
                string[] MMXSXZ = ((DataRow)obj_process)["MMXSXZ"].ToString().Split(',');
                string[] MMSDXZ = ((DataRow)obj_process)["MMSDXZ"].ToString().Split(',');
                string[] MMGLYL = ((DataRow)obj_process)["MMGLYL"].ToString().Split(',');
                string[] MMYW = ((DataRow)obj_process)["MMYW"].ToString().Split(',');
                string[] MMJSGL = ((DataRow)obj_process)["MMJSGL"].ToString().Split(',');
                string[] MMZSGL = ((DataRow)obj_process)["MMZSGL"].ToString().Split(',');

                int count = MMLB.Count();

                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                //检测编号(由接口下发)
                cxtj.Append("<check_id>" + LSH + "</check_id>");
                //检测类型：1年检；2新注册车辆；3外地车转入；4实验比对；5 政府部门监督抽查的复检
                cxtj.Append("<check_type>1</check_type>");
                //检测所在地编码
                cxtj.Append("<city_code>" + ModPublicJiangsu.CityCode + "</city_code>");
                //检测机构编号（由平台提供）
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                //检测线编号（由平台提供）
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(LINEID, "00000000000") + "</line_id>");
                //检测用户登录名（由平台提供）
                cxtj.Append("<user_id>" + ModPublicJiangsu.LwUserName + "</user_id>");
                //检验员姓名
                cxtj.Append("<uname>" + SHY + "</uname>");
                //车辆编码
                cxtj.Append("<vehicle_id>" + CLID + "</vehicle_id>");
                //车架号
                cxtj.Append("<vin>" + VIN + "</vin>");
                //车牌
                cxtj.Append("<plate>" + CLPH + "</plate>");
                cxtj.Append("<fxyjddate>" + ModPublicJiangsu.DicLineYxq.GetValue(LINEID, DateTime.Now).ToString("yyyy-MM-dd") + "</fxyjddate>");
                //检测日期，（yyyy-mm-dd)
                cxtj.Append("<check_date>" + JCSJ + "</check_date>");
                //检测周期开始日期(yyyy-mm)
                cxtj.Append("<period_start_date>" + StartDate + "</period_start_date>");
                //检测周期结束日期(yyyy-mm)
                cxtj.Append("<period_end_date>" + EndDate + "</period_end_date>");
                //检测开始时间。精确到秒（yyyy-mm-dd hh24:mi:ss）
                cxtj.Append("<start_time>" + StartTime + "</start_time>");
                //检测结束时间。精确到秒（yyyy-mm-dd hh24:mi:ss）
                cxtj.Append("<end_time>" + EndTime + "</end_time>");
                //环境温度（°C）
                cxtj.Append("<temperature>" + WD + "</temperature>");
                //大气压（kPa）
                cxtj.Append("<pressure>" + DQY + "</pressure>");
                //相对湿度（%）
                cxtj.Append("<humidity>" + SD + "</humidity>");


                //5025 co测量结果（%）
                cxtj.Append("<co_5025>" + CO25CLZ + "</co_5025>");
                //5025工况 co 检测限值（%）
                cxtj.Append("<co_5025_limit>" + CO25XZ + "</co_5025_limit>");
                //5025工况 co 检测结果： 0：不合格 1：合格
                cxtj.Append("<co_5025_passed>" + CO25PD + "</co_5025_passed>");
                //5025 hc测量结果(10-6)
                cxtj.Append("<hc_5025>" + HC25CLZ + "</hc_5025>");
                //5025工况 hc 检测限值(10-6)
                cxtj.Append("<hc_5025_limit>" + HC25XZ + "</hc_5025_limit>");
                //5025工况 hc 检测结果： 0：不合格 1：合格
                cxtj.Append("<hc_5025_passed>" + HC25PD + "</hc_5025_passed>");
                //5025 no测量结果(10-6)
                cxtj.Append("<no_5025>" + NOX25CLZ + "</no_5025>");
                //5025工况 no 检测限值(10-6)
                cxtj.Append("<no_5025_limit>" + NOX25XZ + "</no_5025_limit>");
                //5025工况 no 检测结果： 0：不合格 1：合格
                cxtj.Append("<no_5025_passed>" + NOX25PD + "</no_5025_passed>");

                //5025加载总功率（kw）
                cxtj.Append("<power_5025>" + JZZGL5025 + "</power_5025>");
                //5025转速结果（r/min）
                cxtj.Append("<rev_5025>" + FDJZS5025 + "</rev_5025>");

                //5025 lambda值
                cxtj.Append("<lambda_5025>" + LAMBDA5025 + "</lambda_5025>");
                //2540 co测量结果（%）
                cxtj.Append("<co_2540>" + CO40CLZ + "</co_2540>");
                //2540工况 co检测限值(10-6)
                cxtj.Append("<co_2540_limit>" + CO40XZ + "</co_2540_limit>");
                //2540工况 co检测结果： 0：不合格 1：合格
                cxtj.Append("<co_2540_passed>" + CO40PD + "</co_2540_passed>");
                //2540 hc测量结果(10-6)
                cxtj.Append("<hc_2540>" + HC40CLZ + "</hc_2540>");
                //2540工况 hc 检测限值(10-6)
                cxtj.Append("<hc_2540_limit>" + HC40XZ + "</hc_2540_limit>");
                //2540工况 hc 检测结果： 0：不合格 1：合格
                cxtj.Append("<hc_2540_passed>" + HC40PD + "</hc_2540_passed>");
                //2540 no测量结果(10-6)
                cxtj.Append("<no_2540>" + NOX40CLZ + "</no_2540>");
                //2540工况 no 检测限值(10-6)
                cxtj.Append("<no_2540_limit>" + NOX40XZ + "</no_2540_limit>");
                //2540工况 no 检测结果： 0：不合格 1：合格
                cxtj.Append("<no_2540_passed>" + NOX40PD + "</no_2540_passed>");

                //2540加载总功率（kw）
                cxtj.Append("<power_2540>" + JZZGL2540 + "</power_2540>");
                //2540转速结果（r/min）
                cxtj.Append("<rev_2540>" + FDJZS2540 + "</rev_2540>");

                //2540 lambda值
                cxtj.Append("<lambda_2540>" + LAMBDA2540 + "</lambda_2540>");
                //检测结果：0：不合格 1：合格 2：中止 3：无效
                cxtj.Append("<passed>" + ZHPD + "</passed>");

                cxtj.Append("<sqqzrname>" + FileOpreate.NM_Sqr + "</sqqzrname>");
                cxtj.Append("<jyrname>" + ((DataRow)obj_car)["CZY"].ToString() + "</jyrname>");
                cxtj.Append("<pzrname>" + FileOpreate.NM_Zlkzr + "</pzrname>");

                cxtj.Append("</result_data>");

                for (int i = 0; i < count; i++)
                {
                    //过程数据 可以有多个process_data
                    cxtj.Append("<process_data>");
                    //全程时序,格式为YYYYMMDD24hmmss
                    cxtj.Append("<time>" + Convert.ToDateTime(MMTIME[i]).ToString("yyyyMMddHHmmss") + "</time>");
                    //工况类型：0-检验准备、1-5025工况、2-2540工况、3-加速过程
                    if (Convert.ToInt32(MMLB[i]) == 4)
                        MMLB[i] = "3";
                    cxtj.Append("<asm_type>" + (Convert.ToInt32(MMLB[i])) + "</asm_type>");//-1
                    //采样时序（1开始的序号，1秒一个数据）
                    cxtj.Append("<time_no>" + (i + 1) + "</time_no>");
                    //车速
                    cxtj.Append("<vehicle_speed>" + MMCS[i] + "</vehicle_speed>");
                    //转速
                    cxtj.Append("<rpm>" + MMZS[i] + "</rpm>");
                    cxtj.Append("<gl>" + MMGL[i] + "</gl>");
                    cxtj.Append("<jsgl>" + MMJSGL[i] + "</jsgl>");
                    //加载扭矩
                    cxtj.Append("<torque>" + MMGLYL[i] + "</torque>");
                    //实测加载功率
                    cxtj.Append("<power>" + MMGL[i] + "</power>");
                    //湿度修正系数
                    cxtj.Append("<sdxzxs>" + double.Parse(MMSDXZ[i]) + "</sdxzxs>");
                    //稀释修正系数
                    cxtj.Append("<xsxzxs>" + double.Parse(MMXSXZ[i]) + "</xsxzxs>");
                    //co测量值,未经稀释修正(%)
                    cxtj.Append("<co>" + double.Parse(MMCO[i]) + "</co>");

                    //co2测量值(%)
                    //MMCO2[i] = "0000.10";
                    cxtj.Append("<co2>" + MMCO2[i] + "</co2>");
                    //hc测量值,未经稀释修正(10-6)
                    cxtj.Append("<hc>" + MMHC[i] + "</hc>");
                    //no测量值，未经稀释修正(10-6)
                    cxtj.Append("<no>" + MMNO[i] + "</no>");
                    //o2测量值(%)
                    cxtj.Append("<o2>" + MMO2[i] + "</o2>");

                    //co修正后值,经稀释修正(%)
                    cxtj.Append("<coa>" + Math.Round(double.Parse(MMCO[i]) * double.Parse(MMXSXZ[i]), 2) + "</coa>");
                    //hc修正后值,经稀释修正(10-6)
                    cxtj.Append("<hca>" + Math.Round(int.Parse(MMHC[i]) * double.Parse(MMXSXZ[i]), 0) + "</hca>");
                    //no修正后值，经稀释修正(10-6)
                    cxtj.Append("<noa>" + Math.Round(double.Parse(MMNO[i]) * double.Parse(MMXSXZ[i]) * double.Parse(MMSDXZ[i]), 0) + "</noa>");

                    //过量空气系数
                    cxtj.Append("<lambda>" + LAMBDA2540 + "</lambda>");
                    cxtj.Append("</process_data>");
                }
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, "[interfacejiangsuASM]：", 3);
                XmlStr = strCon; return true;

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog("生成江苏稳态XML文件时出错" + ex.Message, "creat interfacejiangsu_AsmXml error", 4);
                FileOpreate.SaveLog("失败", "[生成AsmXml_interfacejiangsu]：", 3);
                XmlStr = null; return false;
            }

        }

        /// <summary>
        /// 不透光XML
        /// </summary>
        /// <param name="obj_car">已检车辆信息</param>
        /// <param name="obj_data">BTG</param>
        /// <param name="obj_process">BTG_DATASECONDS</param>
        /// <returns></returns>
        public bool Btg(object obj_car, object obj_data, object obj_process,string reallsh, out string XmlStr)
        {
            try
            {
                //表 ASM
                string SHY = ((DataRow)obj_data)["SHY"].ToString();
                string CLID = ((DataRow)obj_data)["CLID"].ToString();
                CLID = CLID.Substring(0, CLID.IndexOf("&"));
                string CLPH = ((DataRow)obj_data)["CLPH"].ToString();
                string LINEID = ((DataRow)obj_car)["LINEID"].ToString();
                string VIN = ((DataRow)obj_car)["CLSBM"].ToString();
                string LSH = reallsh;// ((DataRow)obj_car)["LSH"].ToString();
                string JCSJ1 = ((DataRow)obj_car)["JCSJ"].ToString();
                string JCSJ = Convert.ToDateTime(JCSJ1).ToString("yyyy-MM-dd");

                string StartDate = Convert.ToDateTime(((DataRow)obj_data)["JCKSSJ"].ToString()).ToString("yyyy-MM");
                string EndDate = Convert.ToDateTime(((DataRow)obj_data)["JCJSSJ"].ToString()).ToString("yyyy-MM");

                string StartTime = Convert.ToDateTime(((DataRow)obj_data)["JCKSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                string EndTime = Convert.ToDateTime(((DataRow)obj_data)["JCJSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");

                string WD = ((DataRow)obj_data)["WD"].ToString();
                string SD = ((DataRow)obj_data)["SD"].ToString();
                string DQY = ((DataRow)obj_data)["DQY"].ToString();
                string YW = ((DataRow)obj_data)["YW"].ToString();

                string FIRSTDATA = ((DataRow)obj_data)["FIRSTDATA"].ToString();
                string SECONDDATA = ((DataRow)obj_data)["SECONDDATA"].ToString();
                string THIRDDATA = ((DataRow)obj_data)["THIRDDATA"].ToString();
                string AVERAGEDATA = ((DataRow)obj_data)["AVERAGEDATA"].ToString();
                string DSZS = ((DataRow)obj_data)["DSZS"].ToString();
                string SCZS = ((DataRow)obj_data)["SCZS"].ToString();
                string YDXZ = ((DataRow)obj_data)["YDXZ"].ToString();

                string YDPD = ((DataRow)obj_data)["YDPD"].ToString() == "不合格" ? "0" : "1";
                string ZHPD = ((DataRow)obj_data)["ZHPD"].ToString() == "不合格" ? "0" : "1";

                //表BTG_DATASECONDS
                string[] MMTIME = ((DataRow)obj_process)["MMTIME"].ToString().Split(',');
                string[] MMLB = ((DataRow)obj_process)["MMLB"].ToString().Split(',');
                string[] MMZS = ((DataRow)obj_process)["MMZS"].ToString().Split(',');
                string[] MMYW = ((DataRow)obj_process)["MMYW"].ToString().Split(',');
                string[] MMWD = ((DataRow)obj_process)["MMWD"].ToString().Split(',');
                string[] MMSD = ((DataRow)obj_process)["MMSD"].ToString().Split(',');
                string[] MMDQY = ((DataRow)obj_process)["MMDQY"].ToString().Split(',');
                string[] MMK = ((DataRow)obj_process)["MMK"].ToString().Split(',');
                string[] MMNS = ((DataRow)obj_process)["MMNS"].ToString().Split(',');
                int count = MMLB.Count();

                //取每次的最大转速值
                string MaxSmoke1Rpm = "0", MaxSmoke2Rpm = "0", MaxSmoke3Rpm = "0";
                int countm = 0;
                try
                {
                    foreach (string MB in MMLB)
                    {
                        if (MB == "1")
                        {
                            if (Convert.ToInt32(MaxSmoke1Rpm) < Convert.ToInt32(MMLB[countm]))
                            {
                                MaxSmoke1Rpm = MMLB[countm];
                            }
                        }
                        if (MB == "2")
                        {
                            if (Convert.ToInt32(MaxSmoke2Rpm) < Convert.ToInt32(MMLB[countm]))
                            {
                                MaxSmoke2Rpm = MMLB[countm];
                            }
                        }
                        if (MB == "3")
                        {
                            if (Convert.ToInt32(MaxSmoke3Rpm) < Convert.ToInt32(MMLB[countm]))
                            {
                                MaxSmoke3Rpm = MMLB[countm];
                            }
                        }
                        countm++;
                    }
                }
                catch (Exception ex)
                {
                    FileOpreate.SaveLog("江苏取自由加速转速最大值时出错" + ex.Message, "creat interfacejiangsu_BtgMaxramp error", 4);
                    FileOpreate.SaveLog("失败", "[生成BtgMaxramp_interfacejiangsu]：", 3);
                }

                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<check_id>" + LSH + "</check_id>");
                cxtj.Append("<check_type>1</check_type>");
                cxtj.Append("<city_code>" + ModPublicJiangsu.CityCode + "</city_code>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(LINEID, "00000000000") + "</line_id>");
                cxtj.Append("<user_id>" + ModPublicJiangsu.LwUserName + "</user_id>");
                cxtj.Append("<uname>" + SHY + "</uname>");
                cxtj.Append("<vehicle_id>" + CLID + "</vehicle_id>");
                cxtj.Append("<vin>" + VIN + "</vin>");
                cxtj.Append("<plate>" + CLPH + "</plate>");
                cxtj.Append("<fxyjddate>" + ModPublicJiangsu.DicLineYxq.GetValue(LINEID, DateTime.Now).ToString("yyyy-MM-dd") + "</fxyjddate>");
                cxtj.Append("<check_date>" + JCSJ + "</check_date>");
                cxtj.Append("<period_start_date>" + StartDate + "</period_start_date>");
                cxtj.Append("<period_end_date>" + EndDate + "</period_end_date>");
                cxtj.Append("<start_time>" + StartTime + "</start_time>");
                cxtj.Append("<end_time>" + EndTime + "</end_time>");
                cxtj.Append("<temperature>" + WD + "</temperature>");
                cxtj.Append("<pressure>" + DQY + "</pressure>");
                cxtj.Append("<humidity>" + SD + "</humidity>");

                //怠速转速（r/min）
                cxtj.Append("<idle_rev>" + DSZS + "</idle_rev>");
                cxtj.Append("<rev>" + SCZS + "</rev>");
                //最后四次（第一次)检测烟度值（m-1）
                cxtj.Append("<smoke1>" + FIRSTDATA + "</smoke1>");
                //最后四次（第二次)检测烟度值（m-1）
                cxtj.Append("<smoke2>" + SECONDDATA + "</smoke2>");
                //最后四次（第四次)检测烟度值（m-1）
                cxtj.Append("<smoke3>" + THIRDDATA + "</smoke3>");

                //cxtj.Append("<smoke4>" + THIRDDATA + "</smoke4>");

                //最后四次（第一次)检测最大转速
                cxtj.Append("<smoke1rpm>" + MaxSmoke1Rpm + "</smoke1rpm>");
                //最后四次（第二次)检测最大转速
                cxtj.Append("<smoke2rpm>" + MaxSmoke2Rpm + "</smoke2rpm>");
                //最后四次（第三次)检测最大转速
                cxtj.Append("<smoke3rpm>" + MaxSmoke3Rpm + "</smoke3rpm>");

                //cxtj.Append("<smoke4rpm>" + MaxSmoke3Rpm + "</smoke4rpm>");
                //烟度限值最后四次检测平均值（m-1）
                cxtj.Append("<smoke_avg >" + AVERAGEDATA + "</smoke_avg>");
                //烟度限值
                cxtj.Append("<smoke_limit>" + YDXZ + "</smoke_limit>");
                //检测结果：0：不合格 1：合格 2：中止 3：无效
                cxtj.Append("<passed>" + YDPD + "</passed>");

                cxtj.Append("<sqqzrname>" + FileOpreate.NM_Sqr + "</sqqzrname>");
                cxtj.Append("<jyrname>" + ((DataRow)obj_car)["CZY"].ToString() + "</jyrname>");
                cxtj.Append("<pzrname>" + FileOpreate.NM_Zlkzr + "</pzrname>");

                cxtj.Append("</result_data>");

                string lb = string.Empty;
                for (int i = 0; i < count; i++)
                {
                    //过程数据 可以有多个process_data
                    cxtj.Append("<process_data>");
                    cxtj.Append("<time>" + Convert.ToDateTime(MMTIME[i]).ToString("yyyyMMddHHmmss") + "</time>");
                    cxtj.Append("<time_no>" + (i + 1) + "</time_no>");
                    cxtj.Append("<rpm>" + MMZS[i] + "</rpm>");
                    cxtj.Append("<yw>" + MMYW[i] + "</yw>");
                    cxtj.Append("<kz>" + MMK[i] + "</kz>");
                    cxtj.Append("<nz>" + MMNS[i] + "</nz>");
                    cxtj.Append("<hjwd>" + MMWD[i] + "</hjwd>");
                    cxtj.Append("<xdsd>" + MMSD[i] + "</xdsd>");
                    cxtj.Append("<dqy>" + MMDQY[i] + "</dqy>");

                    if (MMLB[i] == "10")
                    {
                        lb = "0";
                    }
                    if (MMLB[i] == "11")
                    {
                        lb = "1";
                    }
                    if (MMLB[i] == "12")
                    {
                        lb = "2";
                    }
                    if (MMLB[i] == "1")
                    {
                        lb = "3";
                    }
                    if (MMLB[i] == "2")
                    {
                        lb = "4";
                    }
                    if (MMLB[i] == "3")
                    {
                        lb = "5";
                    }
                    if (MMLB[i] == "4")
                    {
                        lb = "6";
                    }

                    cxtj.Append("<state>" + lb + "</state>");

                    cxtj.Append("</process_data>");
                }
                cxtj.Append("</result>");
                string strCon = cxtj.ToString();
                FileOpreate.SaveLog(strCon, "[interfacejiangsuBtg]：", 3);
                XmlStr = strCon; return true;
            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog("生成江苏不透光工况XML文件时出错" + ex.Message, "creat interfacejiangsu_BtgXml error", 4);
                FileOpreate.SaveLog("失败", "[生成BtgXml_interfacejiangsu]：", 3);
                XmlStr = null; return false;
            }
        }

        /// <summary>
        /// 加载减速XML
        /// </summary>
        /// <param name="obj_car">已检车辆信息</param>
        /// <param name="obj_data">JZJS</param>
        /// <param name="obj_process">JZJS_DATASECONDS</param>
        /// <returns></returns>
        public bool Jzjs(object obj_car, object obj_data, object obj_process,string reallsh, out string XmlStr)
        {
            try
            {
                //string SHY = ((DataRow)obj_data)["SHY"].ToString();
                string CLID = ((DataRow)obj_data)["CLID"].ToString();
                CLID = CLID.Substring(0, CLID.IndexOf("&"));
                string CLPH = ((DataRow)obj_data)["CLPH"].ToString();
                string LINEID = ((DataRow)obj_car)["LINEID"].ToString();
                string VIN = ((DataRow)obj_car)["CLSBM"].ToString();
                string LSH = reallsh;// ((DataRow)obj_car)["LSH"].ToString();
                string JCSJ1 = ((DataRow)obj_car)["JCSJ"].ToString();
                string JCSJ = Convert.ToDateTime(JCSJ1).ToString("yyyy-MM-dd");

                string StartDate = Convert.ToDateTime(((DataRow)obj_data)["JCKSSJ"].ToString()).ToString("yyyy-MM");
                string EndDate = Convert.ToDateTime(((DataRow)obj_data)["JCJSSJ"].ToString()).ToString("yyyy-MM");

                string StartTime = Convert.ToDateTime(((DataRow)obj_data)["JCKSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                string EndTime = Convert.ToDateTime(((DataRow)obj_data)["JCJSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");

                string WD = ((DataRow)obj_data)["WD"].ToString();
                string SD = ((DataRow)obj_data)["SD"].ToString();
                string DQY = ((DataRow)obj_data)["DQY"].ToString();
                //string YW = ((DataRow)obj_data)["YW"].ToString();

                string SmokeK100Limit = ((DataRow)obj_data)["YDXZ"].ToString();
                string SmokeK80Limit = ((DataRow)obj_data)["YDXZ"].ToString();
                string NOx80Limit = ((DataRow)obj_data)["ENOXZ"].ToString();
                string K100 = ((DataRow)obj_data)["HK"].ToString();
                string K80 = ((DataRow)obj_data)["EK"].ToString();
                string NOx80 = ((DataRow)obj_data)["ENO"].ToString();
                string MaxhpLimit = ((DataRow)obj_data)["GLXZ"].ToString();
                string MAXLBGL = ((DataRow)obj_data)["MAXLBGL"].ToString();
                string pcf = ((DataRow)obj_data)["GLXZXS"].ToString();
                string RateRevUp = ((DataRow)obj_data)["RATEREVUP"].ToString();
                string RateRevDown = ((DataRow)obj_data)["RATEREVDOWN"].ToString();
                string MaxRPM = ((DataRow)obj_data)["REALVELMAXHP"].ToString();
                //string rev100 = ((DataRow)obj_data)["MAXLBZS"].ToString();
                string edzs100 = ((DataRow)obj_data)["VELMAXHPZS"].ToString();

                //string MaxRPM = ((DataRow)obj_data)["REALVELMAXHP"].ToString();

                string SmokeK100Judge = ((DataRow)obj_data)["HKPD"].ToString();
                string SmokeK80Judge = ((DataRow)obj_data)["EKPD"].ToString();
                string k80nojudge = ((DataRow)obj_data)["ENOPD"].ToString();
                string ZHPD = ((DataRow)obj_data)["ZHPD"].ToString();//总评
                /*
                string sqqzrname = FileOpreate.NM_Sqr;
                string jyrname = ((DataRow)obj_data)["CZY"].ToString();
                string pzrname = FileOpreate.NM_Zlkzr;
                */
                if (ZHPD == "合格")
                {
                    ZHPD = "1";
                }
                else { ZHPD = "0"; }

                SmokeK100Judge = SmokeK100Judge == "不合格" ? "0" : "1";
                SmokeK80Judge = SmokeK80Judge == "不合格" ? "0" : "1";
                k80nojudge = k80nojudge == "不合格" ? "0" : "1";

                string[] MMTIME = ((DataRow)obj_process)["MMTIME"].ToString().Split(',');
                string[] MMLB = ((DataRow)obj_process)["MMLB"].ToString().Split(',');
                string[] MMCS = ((DataRow)obj_process)["MMCS"].ToString().Split(',');
                string[] MMZS = ((DataRow)obj_process)["MMZS"].ToString().Split(',');
                string[] MMZGL = ((DataRow)obj_process)["MMZGL"].ToString().Split(',');
                string[] MMZSGL = ((DataRow)obj_process)["MMZSGL"].ToString().Split(',');
                string[] MMNL = ((DataRow)obj_process)["MMNL"].ToString().Split(',');
                string[] MMK = ((DataRow)obj_process)["MMK"].ToString().Split(',');
                string[] MMCO2 = ((DataRow)obj_process)["MMCO2"].ToString().Split(',');
                string[] MMNO = ((DataRow)obj_process)["MMNO"].ToString().Split(',');

                string[] MMJSGL = ((DataRow)obj_process)["MMJSGL"].ToString().Split(',');
                string[] MMGL = ((DataRow)obj_process)["MMGL"].ToString().Split(',');
                string[] MMGLXZXS = ((DataRow)obj_process)["MMGLXZXS"].ToString().Split(',');

                int count = MMLB.Count();

                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<check_id>" + LSH + "</check_id>");
                cxtj.Append("<check_type>1</check_type>");
                cxtj.Append("<city_code>" + ModPublicJiangsu.CityCode + "</city_code>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(LINEID, "00000000000") + "</line_id>");
                cxtj.Append("<user_id>" + ModPublicJiangsu.LwUserName + "</user_id>");
                cxtj.Append("<uname></uname>");
                cxtj.Append("<vehicle_id>" + CLID + "</vehicle_id>");
                cxtj.Append("<vin>" + VIN + "</vin>");
                cxtj.Append("<plate>" + CLPH + "</plate>");
                cxtj.Append("<fxyjddate>" + ModPublicJiangsu.DicLineYxq.GetValue(LINEID, DateTime.Now).ToString("yyyy-MM-dd") + "</fxyjddate>");
                cxtj.Append("<check_date>" + JCSJ + "</check_date>");
                cxtj.Append("<period_start_date>" + StartDate + "</period_start_date>");
                cxtj.Append("<period_end_date>" + EndDate + "</period_end_date>");
                cxtj.Append("<start_time>" + StartTime + "</start_time>");
                cxtj.Append("<end_time>" + EndTime + "</end_time>");
                cxtj.Append("<temperature>" + WD + "</temperature>");
                cxtj.Append("<pressure>" + DQY + "</pressure>");
                cxtj.Append("<humidity>" + SD + "</humidity>");
                cxtj.Append("<smokeklimit>" + SmokeK100Limit + "</smokeklimit>");
                cxtj.Append("<k80limit>" + SmokeK80Limit + "</k80limit>");
                cxtj.Append("<k80nolimit>" + NOx80Limit + "</k80nolimit>");
                cxtj.Append("<k100>" + K100 + "</k100>");
                cxtj.Append("<k80>" + K80 + "</k80>");
                cxtj.Append("<k80no>" + NOx80 + "</k80no>");
                cxtj.Append("<maxpowerlimit>" + MaxhpLimit + "</maxpowerlimit>");
                cxtj.Append("<maxpower>" + MAXLBGL + "</maxpower>");
                cxtj.Append("<xzxs>" + pcf + "</xzxs>");
                cxtj.Append("<raterevup>" + RateRevUp + "</raterevup>");
                cxtj.Append("<raterevdown>" + RateRevDown + "</raterevdown>");
                cxtj.Append("<rev100>" + MaxRPM + "</rev100>");
                cxtj.Append("<edzs100>" + edzs100 + "</edzs100>");
                cxtj.Append("<k80judge>" + SmokeK80Judge + "</k80judge>");
                cxtj.Append("<k100judge>" + SmokeK100Judge + "</k100judge>");
                cxtj.Append("<k80nojudge>" + k80nojudge + "</k80nojudge>");
                cxtj.Append("<passed>" + ZHPD + "</passed>");
                cxtj.Append("<sqqzrname>" + FileOpreate.NM_Sqr + "</sqqzrname>");
                cxtj.Append("<jyrname>" + ((DataRow)obj_car)["CZY"].ToString() + "</jyrname>");
                cxtj.Append("<pzrname>" + FileOpreate.NM_Zlkzr + "</pzrname>");
                cxtj.Append("</result_data>");


                // < gl ></ gl >
                //< jsgl ></ jsgl >
                //< xzgl ></ xzgl >
                //< glxzxs ></ glxzxs >
                //< lbgl ></ lbgl >
                //< kh ></ kh >

                string gklb = string.Empty;
                //过程数据 可以有多个process_data
                for (int i = 0; i < count; i++)
                {
                    cxtj.Append("<process_data>");
                    cxtj.Append("<time>" + Convert.ToDateTime(MMTIME[i]).ToString("yyyyMMddHHmmss") + "</time>");

                    if (MMLB[i] == "1")
                    {
                        gklb = "0";
                    }
                    else if (MMLB[i] == "2")
                    {
                        gklb = "1";
                    }
                    else if (MMLB[i] == "3")
                    {
                        gklb = "1";
                    }
                    else if (MMLB[i] == "4")
                    {
                        gklb = "2";
                    }
                    else if (MMLB[i] == "8")
                    {
                        gklb = "4";
                    }
                    else
                    {
                        gklb = "9";//自定义 平台无此工况类别的时候
                    }

                    cxtj.Append("<lugdown_type>" + gklb + "</lugdown_type>");
                    //cxtj.Append("<lugdown_type>" + MMLB[i] + "</lugdown_type>");

                    cxtj.Append("<time_no>" + (i + 1) + "</time_no>");
                    cxtj.Append("<vehicle_speed>" + MMCS[i] + "</vehicle_speed>");
                    cxtj.Append("<rpm>" + MMZS[i] + "</rpm>");
                    cxtj.Append("<gl>" + MMZGL[i] + "</gl>");
                    cxtj.Append("<jsgl>" + MMJSGL[i] + "</jsgl>");
                    cxtj.Append("<xzgl>" + MMGL[i] + "</xzgl>");
                    cxtj.Append("<zsgl>" + MMZSGL[i] + "</zsgl>");
                    cxtj.Append("<jzl>" + MMNL[i] + "</jzl>");
                    cxtj.Append("<glxzxs>" + MMGLXZXS[i] + "</glxzxs>");
                    cxtj.Append("<lbgl>" + MMGL[i] + "</lbgl>");
                    cxtj.Append("<kh>" + MMK[i] + "</kh>");
                    cxtj.Append("<dynamometer_load>" + MMGL[i] + "</dynamometer_load>");
                    cxtj.Append("<cgjnnj>" + MMNL[i] + "</cgjnnj>");
                    cxtj.Append("<light_absorption>" + MMK[i] + "</light_absorption>");
                    cxtj.Append("<co2_nd>" + MMCO2[i] + "</co2_nd>");
                    cxtj.Append("<nox>" + MMNO[i] + "</nox>");
                    cxtj.Append("</process_data>");
                }
                cxtj.Append("</result>");
                string strCon = cxtj.ToString();
                FileOpreate.SaveLog(strCon, "[interfacejiangsuJzjs]：", 3);
                XmlStr = strCon; return true;
            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog("生成江苏加载减速XML文件时出错" + ex.Message, "creat interfacejiangsu_JzjsXml error", 4);
                FileOpreate.SaveLog("失败", "[生成JzjsXml_interfacejiangsu]：", 3);
                XmlStr = null; return false;
            }
        }

        /// <summary>
        /// 汽油车的OBD
        /// </summary>
        /// <param name="obj_car">已检车辆信息</param>
        /// <param name="obj_data">JZJS</param>
        /// <param name="obj_process">JZJS_DATASECONDS</param>
        /// <returns></returns>
        public void QycOBD(object obj_car, object obj_process, object obj_obd,string reallsh)
        {
            string XmlStr = "";
            string SplitStr = "_|_";
            try
            {
                //表 obd
                string LSH = reallsh;// ((DataRow)obj_car)["LSH"].ToString();
                string CLPH = ((DataRow)obj_car)["CLHP"].ToString();
                string CLXH = ((DataRow)obj_car)["XH"].ToString();
                string VIN = ((DataRow)obj_car)["CLSBM"].ToString();
                string inspregid = ((DataRow)obj_car)["LSH"].ToString();
                string JCSJ1 = ((DataRow)obj_car)["JCSJ"].ToString();
                string JCSJ = Convert.ToDateTime(JCSJ1).ToString("yyyy-MM-dd");
                string MILXSLC = ((DataRow)obj_obd)["XSLC"].ToString();
                string XSLC = ((DataRow)obj_car)["XSLC"].ToString();
                string[] GZDM = ((DataRow)obj_obd)["GZDM"].ToString().Split(',');
                string OBDJYY = ((DataRow)obj_obd)["BY1"].ToString();
                string XSJYYQ = ((DataRow)obj_obd)["XSJYYQ"].ToString();

                string kzdymcStr = ((DataRow)obj_obd)["KZDYMC"].ToString().Replace(SplitStr, ",");
                string calidStr = ((DataRow)obj_obd)["CALID"].ToString().Replace(SplitStr, ",");
                string cvnStr = ((DataRow)obj_obd)["CVN"].ToString().Replace(SplitStr, ",");

                string[] KZDYMC = kzdymcStr.Split(',');
                string[] CALID = calidStr.Split(',');
                string[] CVN = cvnStr.Split(',');

                int count = GZDM.Count();
                int kzdycount = KZDYMC.Count();

                //表SDS_DATASECONDS   
                string[] MMTIME = ((DataRow)obj_process)["MMTIME"].ToString().Split(',');
                string[] MMOBDJQMJDKD = ((DataRow)obj_process)["MMOBDJQMJDKD"].ToString().Split(',');
                string[] MMOBDJSFHZ = ((DataRow)obj_process)["MMOBDJSFHZ"].ToString().Split(',');
                string[] MMOBDQYCGQXH = ((DataRow)obj_process)["MMOBDQYCGQXH"].ToString().Split(',');
                string[] MMOBDLAMBDA = ((DataRow)obj_process)["MMOBDLAMBDA"].ToString().Split(',');
                string[] MMOBDSPEED = ((DataRow)obj_process)["MMOBDSPEED"].ToString().Split(',');
                string[] MMOBDROTATESPEED = ((DataRow)obj_process)["MMOBDROTATESPEED"].ToString().Split(',');
                string[] MMOBDJQL = ((DataRow)obj_process)["MMOBDJQL"].ToString().Split(',');
                string[] MMOBDJQYL = ((DataRow)obj_process)["MMOBDJQYL"].ToString().Split(',');
                int processcount = MMTIME.Count();


                StringBuilder cxtj = new StringBuilder();

                cxtj.Append("<?xml version='1.0' encoding='utf-8'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                //检测报告编号（新增）
                cxtj.Append("<check_id>" + LSH + "</check_id>");
                //检测所在地编码
                cxtj.Append("<city_code>" + ModPublicJiangsu.CityCode + "</city_code>");
                //检测机构编号
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                //检测用户登录名
                cxtj.Append("<user_id>" + ModPublicJiangsu.LwUserName + "</user_id>");
                //检测用户名（对应检测人员的中文名）
                cxtj.Append("<uname>" + OBDJYY + "</uname>");
                //用户密码（由监管平台管理员提供）
                cxtj.Append("<pwd>" + ModPublicJiangsu.LwUserPass + "</pwd>");
                //车架号
                cxtj.Append("<vin>" + VIN + "</vin>");
                //车辆型号
                cxtj.Append("<clxh>" + CLXH + "</clxh>");
                //车牌
                cxtj.Append("<plate>" + CLPH + "</plate>");
                //燃油种类
                cxtj.Append("<rlzl>A</rlzl>");
                //检查日期
                cxtj.Append("<check_date>" + JCSJ + "</check_date>");
                //车辆里程数
                cxtj.Append("<odometer>" + XSLC + "</odometer>");
                //OBD故障指示器 0：合格  1：不合格
                cxtj.Append("<obdzsq>0</obdzsq>");
                //要求标准（如EOBD,OBDII等）
                cxtj.Append("<obdstand>" + XSJYYQ + "</obdstand>");
                //通讯结果: 0：成功 1：通讯不成功
                cxtj.Append("<communication>0</communication>");
                //通讯不成功描述: 0：接口损坏 1：找不到接口 2：连接后不能通讯
                cxtj.Append("<communication_desc></communication_desc>");
                cxtj.Append("<ycpfzdtx>0</ycpfzdtx>");
                //就绪状态 ：0：未就绪 1：就绪
                cxtj.Append("<isready>0</isready>");
                //就绪失败项目 ：0：催化器 1：氧传感器 2:氧传感器加热器 3: EGR(可组合，多个之间用英文逗号分隔)
                cxtj.Append("<isreadyfailitem></isreadyfailitem>");
                //检测结果： 0：不合格 1：合格 2：中止 3：无效
                cxtj.Append("<passed>1</passed>");
                cxtj.Append("</result_data>");

                //for (int i = 0; i < count; i++)
                //{
                    /*故障代码，可以有多个 trou _data */
                    cxtj.Append("<trou_data>");
                    //故障代码
                    cxtj.Append("<troubleid>" + (count > 1 ? 0 : 1) + "</troubleid>");
                    //mil灯点亮后的行驶里程
                    cxtj.Append("<milodo>" + MILXSLC + "</milodo>");
                    cxtj.Append("<gzmnum>" + count.ToString() + "</gzmnum>");
                    //故障诊断描述
                    cxtj.Append("<trouble_desc></trouble_desc>");
                    cxtj.Append("</trou_data>");
                //}


                for (int i = 0; i < 3; i++)
                {

                    /*控制单元，可以有多个 cal _data */
                    //obd控制单元calid
                    cxtj.Append("<cal_data>");
                    switch (i)
                    {
                        case 0:
                            //obd控制单元名称
                            cxtj.Append("<obd_con_name>" + "发动机控制单元" + "</obd_con_name>");
                            break;
                        case 1:
                            //obd控制单元名称
                            cxtj.Append("<obd_con_name>" + "后处理控制单元" + "</obd_con_name>");
                            break;
                        case 2:
                            //obd控制单元名称
                            cxtj.Append("<obd_con_name>" + "其他控制单元" + "</obd_con_name>");
                            break;
                    }

                    //obd控制单元calid
                    cxtj.Append("<calid>" + CALID[i] + "</calid>");//" + CALID[i] + "
                    //bd控制单元cvn
                    cxtj.Append("<cvn>" + CVN[i] + "</cvn>");//" + CVN[i] + "

                    cxtj.Append("</cal_data>");

                }
                /*iupr 数据，可以有多个条 iupr_data */

                //for (int i = 0; i < count; i++)
                //{
                cxtj.Append("<iupr_data>");
                //监测项目名称
                cxtj.Append("<name>1</name>");
                //监测完成次数
                cxtj.Append("<times>0</times>");
                //符合条件的监测次数
                cxtj.Append("<oktimes>0</oktimes>");
                //iupr率
                cxtj.Append("<iupr>0</iupr>");
                cxtj.Append("</iupr_data>");
                //}

                //for (int i = 0; i < count; i++)
                //{
                //    cxtj.Append("<process_data>");
                //    cxtj.Append("<time_no>" + MMTIME[i] + "</time_no>");//全称时序，格式为YYYYMMDD24hmmss data
                //    cxtj.Append("<jqmjdkd>" + MMOBDJQMJDKD[i] + "</jqmjdkd>");//节气门绝对开度(%)  number(5)
                //    cxtj.Append("<jsfhz>" + MMOBDJSFHZ[i] + "</jsfhz>");//计算负荷值(%) number(5)
                //    cxtj.Append("<qycgqxh>" + MMOBDQYCGQXH[i] + "</qycgqxh>");//前氧传感器信号  number(5)
                //    cxtj.Append("<glkqxs>" + MMOBDLAMBDA[i] + "</glkqxs>");//过量空气系数  number(5)
                //    cxtj.Append("<vehicle_speed>" + MMOBDSPEED[i] + "</vehicle_speed>");//车速 number(5)
                //    cxtj.Append("<rpm>" + MMOBDROTATESPEED[i] + "</rpm>");//发动机转速 number(5)
                //    cxtj.Append("<airinput>" + MMOBDJQL[i] + "</airinput>");//进气量 number(5)
                //    cxtj.Append("<airinput_pressure>" + MMOBDJQYL[i] + "</airinput_pressure>");//进气压力 number(5)
                //    cxtj.Append("</process_data>");
                //}

                cxtj.Append("</result>");
                String strCon = cxtj.ToString();
                FileOpreate.SaveLog(strCon, "[interfacejiangsuQycOBD]：", 6);
                XmlStr = strCon;
            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog("生成江苏汽油车OBDXML文件时出错" + ex.Message, "creat interfacejiangsu_QycOBDXml error", 6);
                FileOpreate.SaveLog("失败", "[生成QycOBDXml_interfacejiangsu]：", 6);
                XmlStr = null;
            }

            if (XmlStr != null)
            {
                string UpResult = ModPublicJiangsu.coa.UploadInspectionResult("", ModPublicJiangsu.unitid, XmlStr, 6);
                FileOpreate.SaveLog(UpResult, "[江苏上传" + "6" + "]：", 3);
            }
        }

        /// <summary>
        /// 汽油车OBD过程数据
        /// </summary>
        /// <param name="obj_car"></param>
        /// <param name="obj_obd"></param>
        public void QycOBDProcess(object obj_car, object obj_process, object obj_obd,string reallsh)
        {
            string XmlStr = "";
            try
            {
                //表 obd
                string LSH = reallsh;// ((DataRow)obj_car)["LSH"].ToString();
                string CLPH = ((DataRow)obj_car)["CLHP"].ToString();
                string CLXH = ((DataRow)obj_car)["XH"].ToString();
                string VIN = ((DataRow)obj_car)["CLSBM"].ToString();
                string JCSJ1 = ((DataRow)obj_car)["JCSJ"].ToString();
                string JCSJ = Convert.ToDateTime(JCSJ1).ToString("yyyy-MM-dd");
                string OBDJYY = ((DataRow)obj_obd)["BY1"].ToString();
                string XSLC = ((DataRow)obj_car)["XSLC"].ToString();
                string XSJYYQ = ((DataRow)obj_obd)["XSJYYQ"].ToString();
                //表SDS_DATASECONDS   
                string[] MMTIME = ((DataRow)obj_process)["MMTIME"].ToString().Split(',');
                string[] MMOBDJQMJDKD = ((DataRow)obj_process)["MMOBDJQMJDKD"].ToString().Split(',');
                string[] MMOBDJSFHZ = ((DataRow)obj_process)["MMOBDJSFHZ"].ToString().Split(',');
                string[] MMOBDQYCGQXH = ((DataRow)obj_process)["MMOBDQYCGQXH"].ToString().Split(',');
                string[] MMOBDLAMBDA = ((DataRow)obj_process)["MMOBDLAMBDA"].ToString().Split(',');
                string[] MMOBDSPEED = ((DataRow)obj_process)["MMOBDSPEED"].ToString().Split(',');
                string[] MMOBDROTATESPEED = ((DataRow)obj_process)["MMOBDROTATESPEED"].ToString().Split(',');
                string[] MMOBDJQL = ((DataRow)obj_process)["MMOBDJQL"].ToString().Split(',');
                string[] MMOBDJQYL = ((DataRow)obj_process)["MMOBDJQYL"].ToString().Split(',');
                int count = MMTIME.Count();

                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='utf-8'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                //检测报告编号（新增）
                cxtj.Append("<check_id>" + LSH + "</check_id>");
                //车牌
                cxtj.Append("<plate>" + CLPH + "</plate>");
                //车架号
                cxtj.Append("<vin>" + VIN + "</vin>");
                //车辆型号
                cxtj.Append("<clxh>" + CLXH + "</clxh>");
                cxtj.Append("</result_data>");

                for (int i = 0; i < count; i++)
                {
                    cxtj.Append("<process_data>");
                    cxtj.Append("<time_no>" + Convert.ToDateTime(MMTIME[i]).ToString("yyyyMMddHHmmss") + "</time_no>");//全称时序，格式为YYYYMMDD24hmmss data
                    cxtj.Append("<jqmjdkd>" + MMOBDJQMJDKD[i] + "</jqmjdkd>");//节气门绝对开度(%)  number(5)
                    cxtj.Append("<jsfhz>" + MMOBDJSFHZ[i] + "</jsfhz>");//计算负荷值(%) number(5)
                    cxtj.Append("<qycgqxh>" + MMOBDQYCGQXH[i] + "</qycgqxh>");//前氧传感器信号  number(5)
                    cxtj.Append("<glkqxs>" + MMOBDLAMBDA[i] + "</glkqxs>");//过量空气系数  number(5)
                    cxtj.Append("<vehicle_speed>" + MMOBDSPEED[i] + "</vehicle_speed>");//车速 number(5)
                    cxtj.Append("<rpm>" + MMOBDROTATESPEED[i] + "</rpm>");//发动机转速 number(5)
                    cxtj.Append("<airinput>" + MMOBDJQL[i] + "</airinput>");//进气量 number(5)
                    cxtj.Append("<airinput_pressure>" + MMOBDJQYL[i] + "</airinput_pressure>");//进气压力 number(5)
                    cxtj.Append("</process_data>");
                }

                cxtj.Append("</result>");
                String strCon = cxtj.ToString();
                FileOpreate.SaveLog(strCon, "[interfacejiangsuQycOBD]：", 6);
                XmlStr = strCon;
            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog("生成江苏汽油车OBDPROCESSXML文件时出错" + ex.Message, "creat interfacejiangsu_QycOBDPROCESSXml error", 9);
                FileOpreate.SaveLog("失败", "[生成OBDPROCESSXML_interfacejiangsu]：", 9);
                XmlStr = null;
            }

            if (XmlStr != null)
            {
                string UpResult = ModPublicJiangsu.coa.UploadInspectionResult("", ModPublicJiangsu.unitid, XmlStr, 9);
                FileOpreate.SaveLog(UpResult, "[江苏上传" + "6" + "]：", 9);
            }
        }


        /// <summary>
        /// 柴油车的OBD
        /// </summary>
        /// <param name="obj_car">已检车辆信息</param>
        /// <param name="obj_data">JZJS</param>
        /// <param name="obj_process">JZJS_DATASECONDS</param>
        /// <returns></returns>
        public void CycOBD(object obj_car, object obj_obd,string reallsh)
        {
            string XmlStr = "";
            string SplitStr = "_|_";
            try
            {
                //表 obd
                string LSH = reallsh; //((DataRow)obj_car)["LSH"].ToString();
                string CLPH = ((DataRow)obj_car)["CLHP"].ToString();
                string XH = ((DataRow)obj_car)["XH"].ToString();
                string VIN = ((DataRow)obj_car)["CLSBM"].ToString();
                string inspregid = ((DataRow)obj_car)["LSH"].ToString();
                string JCSJ1 = ((DataRow)obj_car)["JCSJ"].ToString();
                string JCSJ = Convert.ToDateTime(JCSJ1).ToString("yyyy-MM-dd");
                string MILXSLC = ((DataRow)obj_obd)["XSLC"].ToString();
                string XSLC = ((DataRow)obj_car)["XSLC"].ToString();
                string[] GZDM = ((DataRow)obj_obd)["GZDM"].ToString().Split(',');
                string OBDJYY = ((DataRow)obj_obd)["BY1"].ToString();
                string XSJYYQ = ((DataRow)obj_obd)["XSJYYQ"].ToString();

                string kzdymcStr = ((DataRow)obj_obd)["KZDYMC"].ToString().Replace(SplitStr, ",");
                string calidStr = ((DataRow)obj_obd)["CALID"].ToString().Replace(SplitStr, ",");
                string cvnStr = ((DataRow)obj_obd)["CVN"].ToString().Replace(SplitStr, ",");

                string[] KZDYMC = kzdymcStr.Split(',');
                string[] CALID = calidStr.Split(',');
                string[] CVN = cvnStr.Split(',');

                string aa = CALID[0];
                int count = GZDM.Count();
                int kzdycount = KZDYMC.Count();

                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='utf-8'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<check_id>" + LSH + "</check_id>");
                cxtj.Append("<city_code>" + ModPublicJiangsu.CityCode + "</city_code>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<user_id>" + ModPublicJiangsu.LwUserName + "</user_id>");
                cxtj.Append("<uname>" + OBDJYY + "</uname>");
                cxtj.Append("<pwd>" + ModPublicJiangsu.LwUserPass + "</pwd>");
                cxtj.Append("<vin>" + VIN + "</vin>");
                cxtj.Append("<clxh>" + XH + "</clxh>");
                cxtj.Append("<plate>" + CLPH + "</plate>");
                cxtj.Append("<rlzl>B</rlzl>");
                cxtj.Append("<check_date>" + JCSJ + "</check_date>");
                cxtj.Append("<odometer>" + XSLC + "</odometer>");
                cxtj.Append("<obdstand>" + XSJYYQ + "</obdstand>");
                cxtj.Append("<communication>0</communication>");
                cxtj.Append("<communication_desc></communication_desc>");
                cxtj.Append("<ycpfzdtx>0</ycpfzdtx>");
                cxtj.Append("<isready>1</isready>");
                cxtj.Append("<isreadyfailitem></isreadyfailitem>");
                cxtj.Append("<passed>1</passed>");
                cxtj.Append("</result_data>");

                //for (int i = 0; i < count; i++)
                //{
                    /*故障代码，可以有多个 trou _data */
                    cxtj.Append("<trou_data>");
                    //故障代码
                    cxtj.Append("<troubleid>" + (count > 1 ? 0 : 1) + "</troubleid>");
                    //mil灯点亮后的行驶里程
                    cxtj.Append("<milodo>" + MILXSLC + "</milodo>");
                cxtj.Append("<gzmnum>" + count.ToString() + "</gzmnum>");
                //故障诊断描述
                cxtj.Append("<trouble_desc></trouble_desc>");
                    cxtj.Append("</trou_data>");
               // }


                for (int i = 0; i < 3; i++)
                {

                    /*控制单元，可以有多个 cal _data */
                    //obd控制单元calid
                    cxtj.Append("<cal_data>");
                    switch (i)
                    {
                        case 0:
                            //obd控制单元名称
                            cxtj.Append("<obd_con_name>" + "发动机控制单元" + "</obd_con_name>");
                            break;
                        case 1:
                            //obd控制单元名称
                            cxtj.Append("<obd_con_name>" + "后处理控制单元"  + "</obd_con_name>");
                            break;
                        case 2:
                            //obd控制单元名称
                            cxtj.Append("<obd_con_name>" +  "其他控制单元"  + "</obd_con_name>");
                            break;
                    }

                    //obd控制单元calid
                    cxtj.Append("<calid>"+ CALID[i]+"</calid>");//" + CALID[i] + "
                    //bd控制单元cvn
                    cxtj.Append("<cvn>" + CVN[i] + "</cvn>");//" + CVN[i] + "

                    cxtj.Append("</cal_data>");

                }
                /*iupr 数据，可以有多个条 iupr_data */

                //for (int i = 0; i < count; i++)
                //{
                cxtj.Append("<iupr_data>");
                //监测项目名称
                cxtj.Append("<name>1</name>");
                //监测完成次数
                cxtj.Append("<times>0</times>");
                //符合条件的监测次数
                cxtj.Append("<oktimes>0</oktimes>");
                //iupr率
                cxtj.Append("<iupr>0</iupr>");
                cxtj.Append("</iupr_data>");
                //}

                cxtj.Append("</result>");
                String strCon = cxtj.ToString();
                FileOpreate.SaveLog(strCon, "[interfacejiangsuCycOBD]：", 7);
                XmlStr = strCon;
            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog("生成江苏柴油车OBDXML文件时出错" + ex.Message, "creat interfacejiangsu_CycOBDXml error", 7);
                FileOpreate.SaveLog("失败", "[生成CycOBDXml_interfacejiangsu]：", 7);
                XmlStr = null;
            }
            if (XmlStr != null)
            {
                string UpResult = ModPublicJiangsu.coa.UploadInspectionResult("", ModPublicJiangsu.unitid, XmlStr, 7);
                FileOpreate.SaveLog(UpResult, "[江苏上传" + "7" + "]：", 7);
            }
        }


        /// <summary>
        /// 柴油车OBD过程数据
        /// </summary>
        /// <param name="obj_car"></param>
        /// <param name="obj_obd"></param>
        public void CycOBDProcess(object obj_car, object obj_process, object obj_obd,string reallsh)
        {
            string XmlStr = "";

            try
            {
                //表 obd
                string LSH = reallsh;// ((DataRow)obj_car)["LSH"].ToString();
                string CLPH = ((DataRow)obj_car)["CLHP"].ToString();
                string CLXH = ((DataRow)obj_car)["XH"].ToString();
                string VIN = ((DataRow)obj_car)["CLSBM"].ToString();
                string JCSJ1 = ((DataRow)obj_car)["JCSJ"].ToString();
                string JCSJ = Convert.ToDateTime(JCSJ1).ToString("yyyy-MM-dd");

                //表SDS_DATASECONDS   
                string[] MMTIME = ((DataRow)obj_process)["MMTIME"].ToString().Split(',');
                string[] MMOBDYMKD = ((DataRow)obj_process)["MMOBDYMKD"].ToString().Split(',');
                string[] MMOBDSPEED = ((DataRow)obj_process)["MMOBDSPEED"].ToString().Split(',');
                string[] MMOBDPOWER = ((DataRow)obj_process)["MMOBDPOWER"].ToString().Split(',');
                string[] MMOBDROTATESPEED = ((DataRow)obj_process)["MMOBDROTATESPEED"].ToString().Split(',');
                string[] MMOBDJQL = ((DataRow)obj_process)["MMOBDJQL"].ToString().Split(',');
                string[] MMOBDZYYL = ((DataRow)obj_process)["MMOBDZYYL"].ToString().Split(',');
                string[] MMOBDHYL = ((DataRow)obj_process)["MMOBDHYL"].ToString().Split(',');
                string[] MMOBDNOND = ((DataRow)obj_process)["MMOBDNOND"].ToString().Split(',');
                string[] MMOBDNSPSL = ((DataRow)obj_process)["MMOBDNSPSL"].ToString().Split(',');
                string[] MMOBDWD = ((DataRow)obj_process)["MMOBDWD"].ToString().Split(',');
                string[] MMOBDKLBJQYC = ((DataRow)obj_process)["MMOBDKLBJQYC"].ToString().Split(',');
                string[] MMOBDEGRKD = ((DataRow)obj_process)["MMOBDEGRKD"].ToString().Split(',');
                string[] MMOBDRYPSYL = ((DataRow)obj_process)["MMOBDRYPSYL"].ToString().Split(',');
                int count = MMTIME.Count();

                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='utf-8'?>");
                cxtj.Append("<result>");

                cxtj.Append("<result_data>");
                //检测报告编号（新增）
                cxtj.Append("<check_id>" + LSH + "</check_id>");
                //车牌
                cxtj.Append("<plate>" + CLPH + "</plate>");
                //车架号
                cxtj.Append("<vin>" + VIN + "</vin>");
                //车辆型号
                cxtj.Append("<clxh>" + CLXH + "</clxh>");
                cxtj.Append("</result_data>");

                for (int i = 0; i < count; i++)
                {
                    cxtj.Append("<process_data>");

                    cxtj.Append("<time_no>" + Convert.ToDateTime(MMTIME[i]).ToString("yyyyMMddHHmmss") + "</time_no>");//全称时序，格式为YYYYMMDD24hmmss data
                    cxtj.Append("<throttle_position>" + MMOBDYMKD[i] + "</throttle_position>");
                    cxtj.Append("<vehicle_speed>" + MMOBDSPEED[i] + "</vehicle_speed>");
                    cxtj.Append("<power>" + MMOBDPOWER[i] + "</power>");
                    cxtj.Append("<rpm>" + MMOBDROTATESPEED[i] + "</rpm>");
                    cxtj.Append("<airinput>" + MMOBDJQL[i] + "</airinput>");
                    cxtj.Append("<boost_pressure>" + MMOBDZYYL[i] + "</boost_pressure>");
                    cxtj.Append("<oilconsumption>" + MMOBDHYL[i] + "</oilconsumption>");
                    cxtj.Append("<nosensornd>" + MMOBDNOND[i] + "</nosensornd>");
                    cxtj.Append("<urea_in_volume>" + MMOBDNSPSL[i] + "</urea_in_volume>");
                    cxtj.Append("<exhausttemperature>" + MMOBDWD[i] + "</exhausttemperature>");
                    cxtj.Append("<klpjqyc>" + MMOBDKLBJQYC[i] + "</klpjqyc>");
                    cxtj.Append("<egrkd>" + MMOBDEGRKD[i] + "</egrkd>");
                    cxtj.Append("<fuel_in_pre>" + MMOBDRYPSYL[i] + "</fuel_in_pre>");

                    cxtj.Append("</process_data>");
                }

                cxtj.Append("</result>");
                String strCon = cxtj.ToString();
                FileOpreate.SaveLog(strCon, "[interfacejiangsuCycOBD]：", 10);
                XmlStr = strCon;
            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog("生成江苏柴油车OBDPROCESSXML文件时出错" + ex.Message, "creat interfacejiangsu_CycOBDPROCESSXml error", 10);
                FileOpreate.SaveLog("失败", "[生成OBDPROCESSXML_interfacejiangsu]：", 10);
                XmlStr = null;
            }

            if (XmlStr != null)
            {
                string UpResult = ModPublicJiangsu.coa.UploadInspectionResult("", ModPublicJiangsu.unitid, XmlStr, 10);
                FileOpreate.SaveLog(UpResult, "[江苏上传" + "10" + "]：", 10);
            }
        }

        //检测类型
        public string GetJclx(string JiangSuJclx)
        {
            string Jclx = "";
            switch (JiangSuJclx)
            {
                case "1": Jclx = "年检"; break;
                case "2": Jclx = "新注册车辆"; break;
                case "3": Jclx = "外地车转入"; break;
                case "4": Jclx = "实验比对"; break;
                case "5": Jclx = "政府部门监督抽查的复检"; break;
                default:
                    Jclx = "年检"; break;
            }
            return Jclx;
        }

        //检测方法
        public string GetJcff(string JiangSuJcff)
        {
            string Jcff = "";
            switch (JiangSuJcff)
            {
                case "1": Jcff = "SDS"; break;
                case "2": Jcff = "ASM"; break;
                case "3": Jcff = "VSM"; break;
                case "4": Jcff = "JZJS"; break;
                case "5": Jcff = "ZYJS"; break;
            }
            return Jcff;
        }

        //号牌种类
        public string GetHpzl(string JiangSuHpzl)
        {
            string Hpzl = "";
            switch (JiangSuHpzl)
            {
                case "01": Hpzl = "大型汽车"; break;
                case "02": Hpzl = "小型汽车"; break;
                case "03": Hpzl = "使馆汽车"; break;
                case "04": Hpzl = "领馆汽车"; break;
                case "05": Hpzl = "境外汽车"; break;
                case "06": Hpzl = "外籍汽车"; break;
                case "13": Hpzl = "低速货车"; break;
                case "16": Hpzl = "教练汽车"; break;
                case "23": Hpzl = "警用汽车"; break;
                case "31": Hpzl = "武警汽车"; break;
                default: Hpzl = JiangSuHpzl; break;

            }
            return Hpzl;
        }

        //车牌颜色
        public string GetCpys(string JiangSuCpys)
        {
            string Cpys = "";
            switch (JiangSuCpys)
            {
                case "1": Cpys = "蓝牌"; break;
                case "2": Cpys = "黄牌"; break;
                case "3": Cpys = "白牌"; break;
                case "4": Cpys = "黑牌"; break;
                case "99": Cpys = "其它"; break;
                default: Cpys = JiangSuCpys; break;

            }
            return Cpys;
        }

        //燃料类型
        public string GetRllx(string JiangSuRllx)
        {
            string Rllx = "";
            switch (JiangSuRllx.ToUpper())
            {
                case "A": Rllx = "汽油"; break;
                case "AC": Rllx = "汽电混合"; break;
                case "AE": Rllx = "汽油天然气混合"; break;
                case "B": Rllx = "柴油"; break;
                case "C": Rllx = "电"; break;
                case "D": Rllx = "混合油"; break;
                case "E": Rllx = "天然气"; break;
                case "F": Rllx = "液化石油气"; break;
                case "L": Rllx = "甲醇"; break;
                case "M": Rllx = "乙醇"; break;
                case "N": Rllx = "太阳能"; break;
                case "O": Rllx = "混合动力"; break;
                case "P": Rllx = "氢气"; break;
                case "Q": Rllx = "生物燃料"; break;
                default: Rllx = JiangSuRllx; break;

            }
            return Rllx;
        }

        //驱动型式
        public string GetQdxs(string JiangSuQdxs)
        {
            string Rdxs = "";
            switch (JiangSuQdxs)
            {
                case "1": Rdxs = "前驱"; break;
                case "2": Rdxs = "后驱"; break;
                case "3": Rdxs = "四驱"; break;
                case "4": Rdxs = "全时四驱"; break;
                default: Rdxs = JiangSuQdxs; break;

            }
            return Rdxs;
        }



        /// <summary>
        /// 使用性质
        /// </summary>
        /// <param name="JiangSuSyxz"></param>
        /// <returns></returns>
        public string GetSyxz(string JiangSuSyxz)
        {
            string syxzstr = string.Empty;
            switch (JiangSuSyxz)
            {
                case "A":
                    syxzstr = "非营运";
                    break;
                case "B":
                    syxzstr = "公路客运";
                    break;
                case "C":
                    syxzstr = "公交客运";
                    break;
                case "D":
                    syxzstr = "出租客运";
                    break;
                case "E":
                    syxzstr = "旅游客运";
                    break;
                case "F":
                    syxzstr = "货运";
                    break;
                case "G":
                    syxzstr = "租赁";
                    break;
                case "H":
                    syxzstr = "警用";
                    break;
                case "I":
                    syxzstr = "消防";
                    break;
                case "L":
                    syxzstr = "营转非";
                    break;
                case "M":
                    syxzstr = "出租转非";
                    break;
                case "N":
                    syxzstr = "教练";
                    break;
                case "O":
                    syxzstr = "幼儿校车";
                    break;
                case "P":
                    syxzstr = "小学生校车";
                    break;
                case "Q":
                    syxzstr = "初中生校车";
                    break;
                case "R":
                    syxzstr = "危化品运输";
                    break;
                case "S":
                    syxzstr = "中小学生校车";
                    break;
                case "T":
                    syxzstr = "中小学生校车";
                    break;
                case "U":
                    syxzstr = "预约出租转非";
                    break;
                default:
                    syxzstr = string.Empty;
                    break;
            }
            return syxzstr;
        }

        //变速箱型式
        public string GetBsxxs(string JiangSuBsxxs)
        {
            string Bxsxs = "";
            switch (JiangSuBsxxs)
            {
                case "1": Bxsxs = "手动"; break;
                case "2": Bxsxs = "自动"; break;
                case "3": Bxsxs = "手自一体"; break;
                default: Bxsxs = JiangSuBsxxs; break;
            }
            return Bxsxs;
        }
        //排放阶段
        public string GetPfbz(string JiangSupfid)
        {
            string PFBZ = "";
            switch (JiangSupfid)
            {
                case "0": PFBZ = "国0"; break;
                case "1": PFBZ = "国I"; break;
                case "2": PFBZ = "国II"; break;
                case "3": PFBZ = "国III"; break;
                case "4": PFBZ = "国IV"; break;
                case "5": PFBZ = "国V"; break;
                case "6": PFBZ = "国VI"; break;
                default: PFBZ = JiangSupfid; break;
            }
            return PFBZ;
        }
        /// <summary>
        /// 供油方式
        /// </summary>
        /// <param name="JiangSuBsxxs"></param>
        /// <returns></returns>
        public string GetGyfs(string JiangsuGyfs)
        {
            string gyfs = "";
            switch (JiangsuGyfs)
            {
                case "1": gyfs = "化油器"; break;
                case "2": gyfs = "化油器改造"; break;
                case "3": gyfs = "开环电喷"; break;
                case "4": gyfs = "闭环电喷"; break;
                case "5": gyfs = "高压共轨"; break;
                case "6": gyfs = "泵喷嘴"; break;
                case "7": gyfs = "单体泵"; break;
                case "8": gyfs = "直列泵"; break;
                case "9": gyfs = "机械泵"; break;
                case "10": gyfs = "其它"; break;
                default: gyfs = JiangsuGyfs; break;
            }
            return gyfs;
        }


        public bool uploadZj(object obj,object processdata)
        {
            string zjlx = ((DataRow)obj)["ZJLX"].ToString();
            bool result = true;
            switch (zjlx)
            {
                case "2":
                    result = Jzhx_gas(obj);
                    CheckProcess_data(obj, processdata);
                    break;
                case "2cy":
                    result = Jzhx_diesel(obj);
                    CheckProcess_data(obj, processdata);
                    break;
                case "3":
                    result = Jsgl_gas(obj);
                    CheckProcess_data(obj, processdata);
                    break;
                case "3cy":
                    result = Jsgl_diesel(obj);
                    CheckProcess_data(obj, processdata);
                    break;
                case "4":
                    {
                        Fqy_xl(obj);
                        int resultlj = 0;
                        if (!CheckRecord_data(obj, "1")) resultlj++;
                        if (!CheckRecord_data(obj, "2")) resultlj++;
                        if (!CheckRecord_data(obj, "3")) resultlj++;
                        if (!CheckRecord_data(obj, "4")) resultlj++;
                        result = (resultlj == 0);
                    }
                    break;
                case "5":
                    result = Fqy_dd_low(obj);
                    CheckProcess_data(obj, processdata);
                    break;
                case "6": result = Ydj_check(obj); break;
                case "7":
                    {
                        int resultlj = 0;
                        if (!NOfxy_xl(obj)) resultlj++;
                        if (!NOfxy_dd_low(obj)) resultlj++;
                        result = (resultlj == 0);
                        CheckProcess_data(obj, processdata);
                    }
                    break;
                
                case "11_l":
                    result = Fqy_dd_low(obj);
                    CheckProcess_data(obj, processdata); break;
                case "11_h":
                    result = Fqy_dd_high(obj);
                    CheckProcess_data(obj, processdata); break;
                case "11_z":
                    result = Fqy_dd_zero(obj);
                    CheckProcess_data(obj, processdata); break;
                case "11_x":
                    result = Fqy_xysj(obj);
                    CheckProcess_data(obj, processdata); break;
                case "12_z":
                case "12_l":
                case "12_ml":
                case "12_mh":
                case "12_h":
                    result = Fqy_wd(obj);
                    CheckProcess_data(obj, processdata);
                    break;
                case "13_l":
                    result = NOfxy_dd_low(obj);
                    CheckProcess_data(obj, processdata); break;
                case "13_h":
                    result = NOfxy_dd_high(obj);
                    CheckProcess_data(obj, processdata); break;
                case "13_z":
                    result = NOfxy_dd_zero(obj);
                    CheckProcess_data(obj, processdata); break;
                case "13_cr":
                    result = NOX_zhl(obj);
                    CheckProcess_data(obj, processdata); break;
                case "14_z":
                case "14_l":
                case "14_ml":
                case "14_mh":
                case "14_h":
                    result = NOfxy_wd(obj);
                    CheckProcess_data(obj, processdata);
                    break;
                default:break;
            }
            return result;
        }
        public bool uploadBd(object obj)
        {
            string zjlx = ((DataRow)obj)["LX"].ToString();
            bool result = true;
            switch (zjlx)
            {
                case "23":
                    result = CGJ_force(obj);
                    break;
                case "22":
                    result = CGJ_speed(obj);
                    break;
                case "28":
                    result = CGJ_diw(obj);
                    break;
                case "29":
                    result = CGJ_bzhhx(obj);
                    break;
                default: break;
            }
            return result;
        }

        public bool Jzhx_gas(object obj_data)
        {
            string FunctionName = "[加载滑行（汽油）]";
            string XmlStr = "";
            try
            {
                

                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");                
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");
                cxtj.Append("<check_date>" + DateTime.Parse(((DataRow)obj_data)["ZJSJ"].ToString()).ToString("yyyy-MM-dd") + "</check_date>");
                cxtj.Append("<hx_start_time>" 
                    +DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    +"</hx_start_time>");
                cxtj.Append("<type>" + "1" + "</type>");
                cxtj.Append("<jbgl>" + ((DataRow)obj_data)["DATA8"].ToString()+ "</jbgl>");
                cxtj.Append("<szgl_2540>" 
                    + ((DataRow)obj_data)["DATA6"].ToString() 
                    + "</szgl_2540>");
                cxtj.Append("<ssgl_40>" 
                    + ((DataRow)obj_data)["DATA11"].ToString() 
                    + "</ssgl_40>");
                cxtj.Append("<hxsj_50_30_time>" 
                    + ((DataRow)obj_data)["DATA3"].ToString()
                    + "</hxsj_50_30_time>");
                cxtj.Append("<hxmy_50_30_time>"
                    + ((DataRow)obj_data)["DATA2"].ToString()
                    + "</hxmy_50_30_time>");
                cxtj.Append("<szgl_5025>"
                    + ((DataRow)obj_data)["DATA7"].ToString()
                    + "</szgl_5025>");
                cxtj.Append("<ssgl_25>"
                    + ((DataRow)obj_data)["DATA17"].ToString()
                    + "</ssgl_25>");
                cxtj.Append("<hxsj_35_15_time>"
                    + ((DataRow)obj_data)["DATA5"].ToString()
                    + "</hxsj_35_15_time>");
                cxtj.Append("<hxmy_35_15_time>"
                    + ((DataRow)obj_data)["DATA4"].ToString()
                    + "</hxmy_35_15_time>");
                cxtj.Append("<fh_4_hx_time>"
                    + "0.0"
                    + "</fh_4_hx_time>");
                cxtj.Append("<fh_4_my_time>"
                    + "0.0"
                    + "</fh_4_my_time>");
                cxtj.Append("<fh_18_hx_time>"
                    + "0.0"
                    + "</fh_18_hx_time>");
                cxtj.Append("<fh_18_my_time>"
                    + "0.0"
                    + "</fh_18_my_time>");
                cxtj.Append("<fh_11_hx_time>"
                    + "0.0"
                    + "</fh_11_hx_time>");
                cxtj.Append("<fh_11_my_time>"
                    + "0.0"
                    + "</fh_11_my_time>");
                cxtj.Append("<hxjg_50_30>"
                    + (double.Parse(((DataRow)obj_data)["DATA10"].ToString())>7?"0":"1")
                    + "</hxjg_50_30>");
                cxtj.Append("<hxjg_35_15>"
                    + (double.Parse(((DataRow)obj_data)["DATA16"].ToString()) > 7 ? "0" : "1")
                    + "</hxjg_35_15>");
                cxtj.Append("<passed>"
                    + ((DataRow)obj_data)["ZJJG"].ToString()
                    + "</passed>");
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;

                return UploadZjBdData(XmlStr, 1);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        public bool Jsgl_gas(object obj_data)
        {
            string FunctionName = "[寄生功率（汽油）]";
            string XmlStr = "";
            try
            {


                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");
                cxtj.Append("<check_date>" + DateTime.Parse(((DataRow)obj_data)["ZJSJ"].ToString()).ToString("yyyy-MM-dd") + "</check_date>");
                cxtj.Append("<ss_start_time>"
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</ss_start_time>");
                cxtj.Append("<hx_end_time>"
                    + DateTime.Parse(((DataRow)obj_data)["JSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</hx_end_time>");
                cxtj.Append("<jbgl>" + ((DataRow)obj_data)["DATA34"].ToString() + "</jbgl>");
                cxtj.Append("<hxsj_50_30_time>" 
                    + ((double.Parse(((DataRow)obj_data)["DATA15"].ToString()) * 1000).ToString("0"))
                    + "</hxsj_50_30_time>");
                cxtj.Append("<hxsj_35_15_time>"
                    + ((double.Parse(((DataRow)obj_data)["DATA25"].ToString()) * 1000).ToString("0"))
                    + "</hxsj_35_15_time>");
                cxtj.Append("<fjssgl_40>"
                    + ((DataRow)obj_data)["DATA16"].ToString()
                    + "</fjssgl_40>");
                cxtj.Append("<fjssgl_25>"
                    + ((DataRow)obj_data)["DATA26"].ToString()
                    + "</fjssgl_25>");
                cxtj.Append("<passed>"
                    + ((DataRow)obj_data)["ZJJG"].ToString()
                    + "</passed>");
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr, 2);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        public bool Jzhx_diesel(object obj_data)
        {
            string FunctionName = "[加载滑行（柴油）]";
            string XmlStr = "";
            try
            {


                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");
                cxtj.Append("<check_date>" + DateTime.Parse(((DataRow)obj_data)["ZJSJ"].ToString()).ToString("yyyy-MM-dd") + "</check_date>");
                cxtj.Append("<hx_start_time>" 
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</hx_start_time>");
                cxtj.Append("<type>" + "1" + "</type>");
                cxtj.Append("<jbgl>" + ((DataRow)obj_data)["DATA10"].ToString() + "</jbgl>");
                if (((DataRow)obj_data)["DATA1"].ToString() == "LUGDOWN100")
                {
                    for (int ihp = 30; ihp >= 10; ihp = ihp - 10)
                    {
                        for (int v = 0; v <=7; v++)
                        {
                            string nodename = string.Format("hxsj{0}_{1}_{2}_time", ihp, 100-v*10, 80 - v*10);
                            cxtj.Append("<"+ nodename + ">"
                                + ((double.Parse(((DataRow)obj_data)["DATA6"].ToString().Split(',')[v]) * 1000).ToString("0"))
                                + "</"+ nodename + ">");
                        }
                        for (int v = 0; v <= 7; v++)
                        {
                            string nodename = string.Format("hxmy{0}_{1}_{2}_time", ihp, 100 - v * 10, 80 - v * 10);
                            cxtj.Append("<" + nodename + ">"
                                + ((double.Parse(((DataRow)obj_data)["DATA7"].ToString().Split(',')[v]) * 1000).ToString("0"))
                                + "</" + nodename + ">");
                        }
                    }
                    for (int v = 0; v <= 7; v++)
                    {
                        string nodename = string.Format("ssgl_{0}",  90 - v * 10);
                        cxtj.Append("<" + nodename + ">"
                            + (((DataRow)obj_data)["DATA4"].ToString().Split(',')[v])
                            + "</" + nodename + ">");
                    }
                }
                else
                {
                    for (int ihp = 30; ihp >= 10; ihp = ihp - 10)
                    {
                        for (int v = 0; v <= 7; v++)
                        {
                            string nodename = string.Format("hxsj{0}_{1}_{2}_time", ihp, 100 - v * 10, 80 - v * 10);
                            if (v < 2)
                            {
                                cxtj.Append("<" + nodename + ">"
                                       + "0"
                                       + "</" + nodename + ">");
                            }
                            else
                            {
                                cxtj.Append("<" + nodename + ">"
                                    + ((double.Parse(((DataRow)obj_data)["DATA6"].ToString().Split(',')[v-2]) * 1000).ToString("0"))
                                    + "</" + nodename + ">");
                            }
                        }
                        for (int v = 0; v <= 7; v++)
                        {
                            string nodename = string.Format("hxmy{0}_{1}_{2}_time", ihp, 100 - v * 10, 80 - v * 10);
                            if (v < 2)
                            {
                                cxtj.Append("<" + nodename + ">"
                                       + "0"
                                       + "</" + nodename + ">");
                            }
                            else
                            {
                                cxtj.Append("<" + nodename + ">"
                                + ((double.Parse(((DataRow)obj_data)["DATA7"].ToString().Split(',')[v - 2]) * 1000).ToString("0"))
                                + "</" + nodename + ">");
                            }
                        }
                    }
                    for (int v = 0; v <= 7; v++)
                    {
                        string nodename = string.Format("ssgl_{0}", 90 - v * 10);
                        if (v < 2)
                        {
                            cxtj.Append("<" + nodename + ">"
                                   + "0"
                                   + "</" + nodename + ">");
                        }
                        else
                        {
                            cxtj.Append("<" + nodename + ">"
                                + (((DataRow)obj_data)["DATA4"].ToString().Split(',')[v-2]) 
                                + "</" + nodename + ">");
                        }
                    }

                }
                cxtj.Append("<hxjg_100_10>"
                    + ((DataRow)obj_data)["ZJJG"].ToString()
                    + "</hxjg_100_10>");
                cxtj.Append("<hxjg_80_10>"
                    + ((DataRow)obj_data)["ZJJG"].ToString()
                    + "</hxjg_80_10>");
                cxtj.Append("<passed>"
                    + ((DataRow)obj_data)["ZJJG"].ToString()
                    + "</passed>");
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr, 3);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        public bool Jsgl_diesel(object obj_data)
        {
            string FunctionName = "[寄生功率（柴油）]";
            string XmlStr = "";
            try
            {


                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");
                cxtj.Append("<check_date>" + DateTime.Parse(((DataRow)obj_data)["ZJSJ"].ToString()).ToString("yyyy-MM-dd") + "</check_date>");
                cxtj.Append("<ssgl_start_time>"
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</ssgl_start_time>");
                cxtj.Append("<ssgl_end_time>"
                    + DateTime.Parse(((DataRow)obj_data)["JSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</ssgl_end_time>");
                string[] hxsjdata = { "DATA5", "DATA10", "DATA15", "DATA20", "DATA25", "DATA30" };
                string[] ssgldata = { "DATA6", "DATA11", "DATA16", "DATA21", "DATA26", "DATA31" };
                for (int v = 0; v <= 5; v++)
                {
                    string nodename = string.Format("hxsj_{0}_{1}",  80 - v * 10, 60 - v * 10);
                    string dataname = hxsjdata[v];
                    cxtj.Append("<" + nodename + ">"
                        + ((double.Parse(((DataRow)obj_data)[dataname].ToString()) * 1000).ToString("0"))
                        + "</" + nodename + ">");
                }
                //cxtj.Append("<hxsj_20_0>"
                //    + "0"
                //    + "</hxsj_20_0>");
                for (int v = 0; v <= 5; v++)
                {
                    string nodename = string.Format("ssgl_{0}", 80 - v * 10);
                    string dataname = ssgldata[v];
                    cxtj.Append("<" + nodename + ">"
                        + (((DataRow)obj_data)[dataname].ToString())
                        + "</" + nodename + ">");
                }
                cxtj.Append("<ssgl_20>"
                    + "0"
                    + "</ssgl_20>");
                cxtj.Append("<jbgl>" + ((DataRow)obj_data)["DATA34"].ToString() + "</jbgl>");                
                cxtj.Append("<passed>"
                    + ((DataRow)obj_data)["ZJJG"].ToString()
                    + "</passed>");
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr, 4);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        public bool Fqy_dd_low(object obj_data)
        {
            string FunctionName = "[废气仪单点检测（低标气）]";
            string XmlStr = "";
            try
            {


                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");
                cxtj.Append("<check_date>" + DateTime.Parse(((DataRow)obj_data)["ZJSJ"].ToString()).ToString("yyyy-MM-dd") + "</check_date>");
                cxtj.Append("<start_time>"
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</start_time>");
                cxtj.Append("<c3h8_nd>"
                    + (((DataRow)obj_data)["DATA2"].ToString())
                    + "</c3h8_nd>");
                cxtj.Append("<co_nd>"
                    + (((DataRow)obj_data)["DATA20"].ToString())
                    + "</co_nd>");
                cxtj.Append("<co2_nd>"
                    + (((DataRow)obj_data)["DATA27"].ToString())
                    + "</co2_nd>");
                cxtj.Append("<no_nd>"
                    + (((DataRow)obj_data)["DATA13"].ToString())
                    + "</no_nd>");
                cxtj.Append("<no2_nd>"
                    + (((DataRow)obj_data)["DATA49"].ToString())
                    + "</no2_nd>");
                cxtj.Append("<o2_nd>"
                    + (((DataRow)obj_data)["DATA34"].ToString())
                    + "</o2_nd>");
                cxtj.Append("<hc>"
                    + (((DataRow)obj_data)["DATA7"].ToString())
                    + "</hc>");
                cxtj.Append("<co>"
                    + (((DataRow)obj_data)["DATA21"].ToString())
                    + "</co>");
                cxtj.Append("<co2>"
                    + (((DataRow)obj_data)["DATA28"].ToString())
                    + "</co2>");
                cxtj.Append("<no>"
                    + (((DataRow)obj_data)["DATA14"].ToString())
                    + "</no>");
                cxtj.Append("<no2>"
                    + (((DataRow)obj_data)["DATA50"].ToString())
                    + "</no2>");
                cxtj.Append("<o2>"
                    + (((DataRow)obj_data)["DATA35"].ToString())
                    + "</o2>");
                cxtj.Append("<pef>"
                    + (((DataRow)obj_data)["DATA3"].ToString())
                    + "</pef>");
                cxtj.Append("<passed>"
                    + ((DataRow)obj_data)["ZJJG"].ToString()
                    + "</passed>");
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr, 5);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        public bool Fqy_dd_zero(object obj_data)
        {
            string FunctionName = "[废气仪单点检测（零气）]";
            string XmlStr = "";
            try
            {


                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");
                cxtj.Append("<check_date>" + DateTime.Parse(((DataRow)obj_data)["ZJSJ"].ToString()).ToString("yyyy-MM-dd") + "</check_date>");
                cxtj.Append("<start_time>"
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</start_time>");
                cxtj.Append("<c3h8_nd>"
                    + (((DataRow)obj_data)["DATA2"].ToString())
                    + "</c3h8_nd>");
                cxtj.Append("<co_nd>"
                    + (((DataRow)obj_data)["DATA20"].ToString())
                    + "</co_nd>");
                cxtj.Append("<co2_nd>"
                    + (((DataRow)obj_data)["DATA27"].ToString())
                    + "</co2_nd>");
                cxtj.Append("<no_nd>"
                    + (((DataRow)obj_data)["DATA13"].ToString())
                    + "</no_nd>");
                cxtj.Append("<no2_nd>"
                    + (((DataRow)obj_data)["DATA49"].ToString())
                    + "</no2_nd>");
                cxtj.Append("<o2_nd>"
                    + (((DataRow)obj_data)["DATA34"].ToString())
                    + "</o2_nd>");
                cxtj.Append("<hc>"
                    + (((DataRow)obj_data)["DATA7"].ToString())
                    + "</hc>");
                cxtj.Append("<co>"
                    + (((DataRow)obj_data)["DATA21"].ToString())
                    + "</co>");
                cxtj.Append("<co2>"
                    + (((DataRow)obj_data)["DATA28"].ToString())
                    + "</co2>");
                cxtj.Append("<no>"
                    + (((DataRow)obj_data)["DATA14"].ToString())
                    + "</no>");
                cxtj.Append("<no2>"
                    + (((DataRow)obj_data)["DATA50"].ToString())
                    + "</no2>");
                cxtj.Append("<o2>"
                    + (((DataRow)obj_data)["DATA35"].ToString())
                    + "</o2>");
                cxtj.Append("<pef>"
                    + (((DataRow)obj_data)["DATA3"].ToString())
                    + "</pef>");
                cxtj.Append("<passed>"
                    + ((DataRow)obj_data)["ZJJG"].ToString()
                    + "</passed>");
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr, 6);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        public bool Fqy_dd_high(object obj_data)
        {
            string FunctionName = "[废气仪单点检测（高标气）]";
            string XmlStr = "";
            try
            {


                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");
                cxtj.Append("<check_date>" + DateTime.Parse(((DataRow)obj_data)["ZJSJ"].ToString()).ToString("yyyy-MM-dd") + "</check_date>");
                cxtj.Append("<start_time>"
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</start_time>");
                cxtj.Append("<c3h8_nd>"
                    + (((DataRow)obj_data)["DATA2"].ToString())
                    + "</c3h8_nd>");
                cxtj.Append("<co_nd>"
                    + (((DataRow)obj_data)["DATA20"].ToString())
                    + "</co_nd>");
                cxtj.Append("<co2_nd>"
                    + (((DataRow)obj_data)["DATA27"].ToString())
                    + "</co2_nd>");
                cxtj.Append("<no_nd>"
                    + (((DataRow)obj_data)["DATA13"].ToString())
                    + "</no_nd>");
                cxtj.Append("<no2_nd>"
                    + (((DataRow)obj_data)["DATA49"].ToString())
                    + "</no2_nd>");
                cxtj.Append("<o2_nd>"
                    + (((DataRow)obj_data)["DATA34"].ToString())
                    + "</o2_nd>");
                cxtj.Append("<t90_no>"
                    + (((DataRow)obj_data)["DATA43"].ToString())
                    + "</t90_no>");
                cxtj.Append("<t90_no2>"
                    + (((DataRow)obj_data)["DATA47"].ToString())
                    + "</t90_no2>");
                cxtj.Append("<t90_co>"
                    + (((DataRow)obj_data)["DATA41"].ToString())
                    + "</t90_co>");
                cxtj.Append("<t90_o2>"
                    + (((DataRow)obj_data)["DATA45"].ToString())
                    + "</t90_o2>");
                cxtj.Append("<t100_no>"
                    + (((DataRow)obj_data)["DATA44"].ToString())
                    + "</t100_no>");
                cxtj.Append("<t100_no2>"
                    + (((DataRow)obj_data)["DATA48"].ToString())
                    + "</t100_no2>");
                cxtj.Append("<t100_co>"
                    + (((DataRow)obj_data)["DATA42"].ToString())
                    + "</t100_co>");
                cxtj.Append("<t100_o2>"
                    + (((DataRow)obj_data)["DATA46"].ToString())
                    + "</t100_o2>");
                cxtj.Append("<passed>"
                    + ((DataRow)obj_data)["ZJJG"].ToString()
                    + "</passed>");
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon,  FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr, 7);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        public bool NOfxy_dd_low(object obj_data)
        {
            string FunctionName = "[氮氧分析仪单点检测（低标气）]";
            string XmlStr = "";
            try
            {


                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");
                cxtj.Append("<check_date>" + DateTime.Parse(((DataRow)obj_data)["ZJSJ"].ToString()).ToString("yyyy-MM-dd") + "</check_date>");
                cxtj.Append("<start_time>"
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</start_time>");
                cxtj.Append("<c3h8_nd>"
                    + "0"
                    + "</c3h8_nd>");
                cxtj.Append("<co_nd>"
                    + "0"
                    + "</co_nd>");
                cxtj.Append("<co2_nd>"
                    + (((DataRow)obj_data)["DATA1"].ToString())
                    + "</co2_nd>");
                cxtj.Append("<no_nd>"
                    + (((DataRow)obj_data)["DATA8"].ToString())
                    + "</no_nd>");
                cxtj.Append("<no2_nd>"
                    + (((DataRow)obj_data)["DATA15"].ToString())
                    + "</no2_nd>");
                cxtj.Append("<o2_nd>"
                    + "0"
                    + "</o2_nd>");
                cxtj.Append("<hc>"
                    + "0"
                    + "</hc>");
                cxtj.Append("<co>"
                    + "0"
                    + "</co>");
                cxtj.Append("<co2>"
                    + (((DataRow)obj_data)["DATA2"].ToString())
                    + "</co2>");
                cxtj.Append("<no>"
                    + (((DataRow)obj_data)["DATA9"].ToString())
                    + "</no>");
                cxtj.Append("<no2>"
                    + (((DataRow)obj_data)["DATA16"].ToString())
                    + "</no2>");
                cxtj.Append("<o2>"
                    + "0"
                    + "</o2>");
                cxtj.Append("<pef>"
                    + "0"
                    + "</pef>");
                cxtj.Append("<passed>"
                    + ((DataRow)obj_data)["ZJJG"].ToString()
                    + "</passed>");
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr, 5);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        public bool NOfxy_dd_zero(object obj_data)
        {
            string FunctionName = "[氮氧分析仪单点检测（零气）]";
            string XmlStr = "";
            try
            {


                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");
                cxtj.Append("<check_date>" + DateTime.Parse(((DataRow)obj_data)["ZJSJ"].ToString()).ToString("yyyy-MM-dd") + "</check_date>");
                cxtj.Append("<start_time>"
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</start_time>");
                cxtj.Append("<c3h8_nd>"
                    + "0"
                    + "</c3h8_nd>");
                cxtj.Append("<co_nd>"
                    + "0"
                    + "</co_nd>");
                cxtj.Append("<co2_nd>"
                    + (((DataRow)obj_data)["DATA1"].ToString())
                    + "</co2_nd>");
                cxtj.Append("<no_nd>"
                    + (((DataRow)obj_data)["DATA8"].ToString())
                    + "</no_nd>");
                cxtj.Append("<no2_nd>"
                    + (((DataRow)obj_data)["DATA15"].ToString())
                    + "</no2_nd>");
                cxtj.Append("<o2_nd>"
                    + "0"
                    + "</o2_nd>");
                cxtj.Append("<hc>"
                    + "0"
                    + "</hc>");
                cxtj.Append("<co>"
                    + "0"
                    + "</co>");
                cxtj.Append("<co2>"
                    + (((DataRow)obj_data)["DATA2"].ToString())
                    + "</co2>");
                cxtj.Append("<no>"
                    + (((DataRow)obj_data)["DATA9"].ToString())
                    + "</no>");
                cxtj.Append("<no2>"
                    + (((DataRow)obj_data)["DATA16"].ToString())
                    + "</no2>");
                cxtj.Append("<o2>"
                    + "0"
                    + "</o2>");
                cxtj.Append("<pef>"
                    + "0"
                    + "</pef>");
                cxtj.Append("<passed>"
                    + ((DataRow)obj_data)["ZJJG"].ToString()
                    + "</passed>");
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr, 6);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        public bool NOfxy_dd_high(object obj_data)
        {
            string FunctionName = "[氮氧分析仪单点检测（高标气）]";
            string XmlStr = "";
            try
            {


                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");
                cxtj.Append("<check_date>" + DateTime.Parse(((DataRow)obj_data)["ZJSJ"].ToString()).ToString("yyyy-MM-dd") + "</check_date>");
                cxtj.Append("<start_time>"
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</start_time>");
                cxtj.Append("<c3h8_nd>"
                    + "0"
                    + "</c3h8_nd>");
                cxtj.Append("<co_nd>"
                    + "0"
                    + "</co_nd>");
                cxtj.Append("<co2_nd>"
                    + (((DataRow)obj_data)["DATA1"].ToString())
                    + "</co2_nd>");
                cxtj.Append("<no_nd>"
                    + (((DataRow)obj_data)["DATA8"].ToString())
                    + "</no_nd>");
                cxtj.Append("<no2_nd>"
                    + (((DataRow)obj_data)["DATA15"].ToString())
                    + "</no2_nd>");
                cxtj.Append("<o2_nd>"
                    + "0"
                    + "</o2_nd>");
                cxtj.Append("<t90_no>"
                    + (((DataRow)obj_data)["DATA22"].ToString())
                    + "</t90_no>");
                cxtj.Append("<t90_no2>"
                    + (((DataRow)obj_data)["DATA23"].ToString())
                    + "</t90_no2>");
                cxtj.Append("<t90_co>"
                    + "0"
                    + "</t90_co>");
                cxtj.Append("<t90_o2>"
                    + "0"
                    + "</t90_o2>");
                cxtj.Append("<t100_no>"
                    + (((DataRow)obj_data)["DATA28"].ToString())
                    + "</t100_no>");
                cxtj.Append("<t100_no2>"
                    + (((DataRow)obj_data)["DATA29"].ToString())
                    + "</t100_no2>");
                cxtj.Append("<t100_co>"
                    + "0"
                    + "</t100_co>");
                cxtj.Append("<t100_o2>"
                    + "0"
                    + "</t100_o2>");
                cxtj.Append("<passed>"
                    + ((DataRow)obj_data)["ZJJG"].ToString()
                    + "</passed>");
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr, 7);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        public bool Fqy_wd(object obj_data)
        {
            string FunctionName = "[废气仪五点检查]";
            string type = "1";
            string zjlx = ((DataRow)obj_data)["ZJLX"].ToString();
            switch (zjlx)
            {
                case "12_z":FunctionName = "[废气仪五点检查（零气)]"; type = "5"; break;
                case "12_l": FunctionName = "[废气仪五点检查（低气)]"; type = "1"; break;
                case "12_ml": FunctionName = "[废气仪五点检查（中低气)]"; type = "2"; break;
                case "12_mh": FunctionName = "[废气仪五点检查（中高气)]"; type = "3"; break;
                case "12_h": FunctionName = "[废气仪五点检查（高气)]"; type = "4"; break;
                case "14_z": FunctionName = "[氮氧分析仪五点检查（零气)]"; type = "5"; break;
                case "14_l": FunctionName = "[氮氧分析仪五点检查（低气)]"; type = "1"; break;
                case "14_ml": FunctionName = "[氮氧分析仪五点检查（中低气)]"; type = "2"; break;
                case "14_mh": FunctionName = "[氮氧分析仪五点检查（中高气)]"; type = "3"; break;
                case "14_h": FunctionName = "[氮氧分析仪五点检查（高气)]"; type = "4"; break;
                default:break;
            }
            
            string XmlStr = "";
            try
            {


                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");
                cxtj.Append("<check_date>" + DateTime.Parse(((DataRow)obj_data)["ZJSJ"].ToString()).ToString("yyyy-MM-dd") + "</check_date>");
                cxtj.Append("<type>"
                    + type
                    + "</type>");
                cxtj.Append("<start_time>"
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</start_time>");
                cxtj.Append("<c3h8_nd>"
                    + (((DataRow)obj_data)["DATA2"].ToString())
                    + "</c3h8_nd>");
                cxtj.Append("<co_nd>"
                    + (((DataRow)obj_data)["DATA20"].ToString())
                    + "</co_nd>");
                cxtj.Append("<co2_nd>"
                    + (((DataRow)obj_data)["DATA27"].ToString())
                    + "</co2_nd>");
                cxtj.Append("<no_nd>"
                    + (((DataRow)obj_data)["DATA13"].ToString())
                    + "</no_nd>");
                cxtj.Append("<no2_nd>"
                    + (((DataRow)obj_data)["DATA49"].ToString())
                    + "</no2_nd>");
                cxtj.Append("<o2_nd>"
                    + (((DataRow)obj_data)["DATA34"].ToString())
                    + "</o2_nd>");
                cxtj.Append("<hc>"
                    + (((DataRow)obj_data)["DATA7"].ToString())
                    + "</hc>");
                cxtj.Append("<co>"
                    + (((DataRow)obj_data)["DATA21"].ToString())
                    + "</co>");
                cxtj.Append("<co2>"
                    + (((DataRow)obj_data)["DATA28"].ToString())
                    + "</co2>");
                cxtj.Append("<no>"
                    + (((DataRow)obj_data)["DATA14"].ToString())
                    + "</no>");
                cxtj.Append("<no2>"
                    + (((DataRow)obj_data)["DATA50"].ToString())
                    + "</no2>");
                cxtj.Append("<o2>"
                    + (((DataRow)obj_data)["DATA35"].ToString())
                    + "</o2>");
                cxtj.Append("<pef>"
                    + (((DataRow)obj_data)["DATA3"].ToString())
                    + "</pef>");
                cxtj.Append("<passed>"
                    + ((DataRow)obj_data)["ZJJG"].ToString()
                    + "</passed>");
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr, 8);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        public bool NOfxy_wd(object obj_data)
        {
            string FunctionName = "[废气仪五点检查]";
            string type = "1";
            string zjlx = ((DataRow)obj_data)["ZJLX"].ToString();
            switch (zjlx)
            {
                case "12_z": FunctionName = "[废气仪五点检查（零气)]"; type = "5"; break;
                case "12_l": FunctionName = "[废气仪五点检查（低气)]"; type = "1"; break;
                case "12_ml": FunctionName = "[废气仪五点检查（中低气)]"; type = "2"; break;
                case "12_mh": FunctionName = "[废气仪五点检查（中高气)]"; type = "3"; break;
                case "12_h": FunctionName = "[废气仪五点检查（高气)]"; type = "4"; break;
                case "14_z": FunctionName = "[氮氧分析仪五点检查（零气)]"; type = "5"; break;
                case "14_l": FunctionName = "[氮氧分析仪五点检查（低气)]"; type = "1"; break;
                case "14_ml": FunctionName = "[氮氧分析仪五点检查（中低气)]"; type = "2"; break;
                case "14_mh": FunctionName = "[氮氧分析仪五点检查（中高气)]"; type = "3"; break;
                case "14_h": FunctionName = "[氮氧分析仪五点检查（高气)]"; type = "4"; break;
                default: break;
            }

            string XmlStr = "";
            try
            {


                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");
                cxtj.Append("<check_date>" + DateTime.Parse(((DataRow)obj_data)["ZJSJ"].ToString()).ToString("yyyy-MM-dd") + "</check_date>");
                cxtj.Append("<type>"
                    + type
                    + "</type>");
                cxtj.Append("<start_time>"
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</start_time>");
                cxtj.Append("<c3h8_nd>"
                    + "0"
                    + "</c3h8_nd>");
                cxtj.Append("<co_nd>"
                    + "0"
                    + "</co_nd>");
                cxtj.Append("<co2_nd>"
                    + (((DataRow)obj_data)["DATA1"].ToString())
                    + "</co2_nd>");
                cxtj.Append("<no_nd>"
                    + (((DataRow)obj_data)["DATA8"].ToString())
                    + "</no_nd>");
                cxtj.Append("<no2_nd>"
                    + (((DataRow)obj_data)["DATA15"].ToString())
                    + "</no2_nd>");
                cxtj.Append("<o2_nd>"
                    + "0"
                    + "</o2_nd>");
                cxtj.Append("<hc>"
                    + "0"
                    + "</hc>");
                cxtj.Append("<co>"
                    + "0"
                    + "</co>");
                cxtj.Append("<co2>"
                    + (((DataRow)obj_data)["DATA2"].ToString())
                    + "</co2>");
                cxtj.Append("<no>"
                    + (((DataRow)obj_data)["DATA9"].ToString())
                    + "</no>");
                cxtj.Append("<no2>"
                    + (((DataRow)obj_data)["DATA16"].ToString())
                    + "</no2>");
                cxtj.Append("<o2>"
                    + "0"
                    + "</o2>");
                cxtj.Append("<pef>"
                    + "0"
                    + "</pef>");
                cxtj.Append("<passed>"
                    + ((DataRow)obj_data)["ZJJG"].ToString()
                    + "</passed>");
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr,8);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        public bool Fqy_xl(object obj_data)
        {
            string FunctionName = "[废气仪泄漏]";
            string type = "1"; 
            string XmlStr = "";
            try
            {


                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");
                cxtj.Append("<type>"
                    + type
                    + "</type>");
                cxtj.Append("<check_date>" + DateTime.Parse(((DataRow)obj_data)["ZJSJ"].ToString()).ToString("yyyy-MM-dd") + "</check_date>");
                
                cxtj.Append("<start_time>"
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</start_time>");
                
                cxtj.Append("<passed>"
                    + ((DataRow)obj_data)["DATA1"].ToString()
                    + "</passed>");
                cxtj.Append("<unpass_desc>"
                    +(((DataRow)obj_data)["DATA1"].ToString()=="0"?"取样系统":"")
                    + "</unpass_desc>");
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr, 9);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        public bool NOfxy_xl(object obj_data)
        {
            string FunctionName = "[氮氧分析仪泄漏]";
            string type = "2";
            string XmlStr = "";
            try
            {


                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");
                cxtj.Append("<type>"
                    + type
                    + "</type>");
                cxtj.Append("<check_date>" + DateTime.Parse(((DataRow)obj_data)["ZJSJ"].ToString()).ToString("yyyy-MM-dd") + "</check_date>");

                cxtj.Append("<start_time>"
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</start_time>");

                cxtj.Append("<passed>"
                    + "1"
                    + "</passed>");
                cxtj.Append("<unpass_desc>"
                    + ""
                    + "</unpass_desc>");
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr, 9);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        public bool Ydj_check(object obj_data)
        {
            string FunctionName = "[烟度计检查]";
            string XmlStr = "";
            try
            {


                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");
                cxtj.Append("<check_date>" + DateTime.Parse(((DataRow)obj_data)["ZJSJ"].ToString()).ToString("yyyy-MM-dd") + "</check_date>");

                cxtj.Append("<start_time>"
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</start_time>");

                cxtj.Append("<gxdwc>"
                    + ((DataRow)obj_data)["DATA6"].ToString()
                    + "</gxdwc>");
                cxtj.Append("<response_time>"
                    + "0"
                    + "</response_time>");
                cxtj.Append("<smokewc>"
                    + ((DataRow)obj_data)["DATA6"].ToString()
                    + "</smokewc>");
                cxtj.Append("<passed>"
                    + ((DataRow)obj_data)["ZJJG"].ToString()
                    + "</passed>");
                cxtj.Append("<unpass_desc>"
                    + ""
                    + "</unpass_desc>");
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr, 10);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }        
        public bool CheckProcess_data(object obj_data,object obj_process)
        {
            string FunctionName = "[设备检查过程数据]";
            string zjlx = ((DataRow)obj_data)["ZJLX"].ToString();
            string type = "1";
            string velhp = "DATA2";
            string torque = "DATA3";
            string hc_nd = "DATA3";
            string co_nd = "DATA1";
            string co2_nd = "DATA2";
            string no_nd = "DATA4";
            string no2_nd = "DATA5";
            string o2_nd = "DATA6";
            string nofxy_co2_nd = "DATA3";
            string nofxy_no_nd = "DATA1";
            string nofxy_no2_nd = "DATA2";
            //表——设备自检过程数据
            if (obj_process == null)
            {
                return false;
            }
            string[] velhp_array = ((DataRow)obj_process)[velhp].ToString().Split(',');
            string[] torque_array = ((DataRow)obj_process)[torque].ToString().Split(',');
            string[] hc_nd_array = ((DataRow)obj_process)[hc_nd].ToString().Split(',');
            string[] co_nd_array = ((DataRow)obj_process)[co_nd].ToString().Split(',');
            string[] co2_nd_array = ((DataRow)obj_process)[co2_nd].ToString().Split(',');
            string[] no_nd_array = ((DataRow)obj_process)[no_nd].ToString().Split(',');
            string[] no2_nd_array = ((DataRow)obj_process)[no2_nd].ToString().Split(',');
            string[] o2_nd_array = ((DataRow)obj_process)[o2_nd].ToString().Split(',');
            string[] nofxy_co2_nd_array = ((DataRow)obj_process)[nofxy_co2_nd].ToString().Split(',');
            string[] nofxy_no_nd_array = ((DataRow)obj_process)[nofxy_no_nd].ToString().Split(',');
            string[] nofxy_no2_nd_array = ((DataRow)obj_process)[nofxy_no2_nd].ToString().Split(',');
            switch (zjlx)
            {
                case "2":
                case "2cy":
                    FunctionName = "[设备检查过程数据(加载滑行)]";
                    type = "1";
                    break;
                case "3":
                case "3cy":
                    FunctionName = "[设备检查过程数据(附加损失)]";
                    type = "2";
                    break;
                case "11_l":
                case "13_l":
                    FunctionName = "[单点检查（低标气）]";
                    type = "3";
                    break;
                case "11_z":
                case "13_z":
                    FunctionName = "[单点检查（零气）]";
                    type = "4";
                    break;
                case "11_h":
                case "13_h":
                    FunctionName = "[单点检查（高标气）]";
                    type = "5";
                    break;
                case "12_z":
                case "12_l":
                case "12_ml":
                case "12_mh":
                case "12_h":
                case "14_z":
                case "14_l":
                case "14_ml":
                case "14_mh":
                case "14_h":
                    FunctionName = "[五点检查]";
                    type = "6";
                    break;
                default:return false;
            }
            string XmlStr = "";
            try
            {


                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");
                cxtj.Append("<check_date>" + DateTime.Parse(((DataRow)obj_data)["ZJSJ"].ToString()).ToString("yyyy-MM-dd") + "</check_date>");
                cxtj.Append("<type>"
                    + type
                    + "</type>");
                cxtj.Append("<start_time>"
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</start_time>");
                cxtj.Append("<end_time>"
                    + DateTime.Parse(((DataRow)obj_data)["JSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</end_time>");
                if (zjlx.StartsWith("2") || zjlx.StartsWith("3"))
                {
                    int count = velhp_array.Count();
                    for (int i = 0; i < count-1; i++)//减1是因为过程数据最后有一个逗号
                    {
                        cxtj.Append("<item>");
                        cxtj.Append("<timeno>" + (i+1).ToString() + "</time>");
                        cxtj.Append("<velhp>" +velhp_array[i] + "</velhp>");
                        cxtj.Append("<torque>" + torque_array[i] + "</torque>");
                        cxtj.Append("<hc_nd>" + "" + "</hc_nd>");
                        cxtj.Append("<co_nd>" + "" + "</co_nd>");
                        cxtj.Append("<co2_nd>" + "" + "</co2_nd>");
                        cxtj.Append("<no_nd>" + "" + "</no_nd>");
                        cxtj.Append("<no2_nd>" + "" + "</no2_nd>");
                        cxtj.Append("<o2_nd>" + "" + "</o2_nd>");
                        cxtj.Append("</item>");
                    }
                }
                else if (zjlx.StartsWith("11") || zjlx.StartsWith("12"))
                {
                    int count = velhp_array.Count();
                    for (int i = 0; i < count - 1; i++)//减1是因为过程数据最后有一个逗号
                    {
                        cxtj.Append("<item>");
                        cxtj.Append("<timeno>" + (i + 1).ToString() + "</time>");
                        cxtj.Append("<velhp>" + "" + "</velhp>");
                        cxtj.Append("<torque>" + "" + "</torque>");
                        cxtj.Append("<hc_nd>" + hc_nd_array[i] + "</hc_nd>");
                        cxtj.Append("<co_nd>" + co_nd_array[i] + "</co_nd>");
                        cxtj.Append("<co2_nd>" + co2_nd_array[i] + "</co2_nd>");
                        cxtj.Append("<no_nd>" + no_nd_array[i] + "</no_nd>");
                        cxtj.Append("<no2_nd>" + no2_nd_array[i] + "</no2_nd>");
                        cxtj.Append("<o2_nd>" + o2_nd_array[i] + "</o2_nd>");
                        cxtj.Append("</item>");
                    }
                }
                else if (zjlx.StartsWith("13") || zjlx.StartsWith("14"))
                {
                    int count = velhp_array.Count();
                    for (int i = 0; i < count - 1; i++)//减1是因为过程数据最后有一个逗号
                    {
                        cxtj.Append("<item>");
                        cxtj.Append("<timeno>" + (i + 1).ToString() + "</time>");
                        cxtj.Append("<velhp>" + "" + "</velhp>");
                        cxtj.Append("<torque>" + "" + "</torque>");
                        cxtj.Append("<hc_nd>" + "" + "</hc_nd>");
                        cxtj.Append("<co_nd>" + "" + "</co_nd>");
                        cxtj.Append("<co2_nd>" + nofxy_co2_nd_array[i] + "</co2_nd>");
                        cxtj.Append("<no_nd>" + nofxy_no_nd_array[i] + "</no_nd>");
                        cxtj.Append("<no2_nd>" + nofxy_no2_nd_array[i] + "</no2_nd>");
                        cxtj.Append("<o2_nd>" + "" + "</o2_nd>");
                        cxtj.Append("</item>");
                    }
                }
                else
                {
                    return false;
                }
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr, 11);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        
        public bool Fqy_xysj(object obj_data)
        {
            string FunctionName = "[排气分析仪响应时间]";
            string XmlStr = "";
            try
            {
                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");

                cxtj.Append("<fuel_type>" + "1" + "</fuel_type>");                
                cxtj.Append("<start_time>"
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</start_time>");
                cxtj.Append("<end_time>"
                    + DateTime.Parse(((DataRow)obj_data)["JSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</end_time>");                                
                cxtj.Append("<passed>"
                    + ((DataRow)obj_data)["ZJJG"].ToString()
                    + "</passed>");

                cxtj.Append("<t90_hc>"
                    + ((DataRow)obj_data)["DATA2"].ToString()
                    + "</t90_hc>");
                cxtj.Append("<t90_co>"
                    + ((DataRow)obj_data)["DATA6"].ToString()
                    + "</t90_co>");
                cxtj.Append("<t90_co2>"
                    + ((DataRow)obj_data)["DATA10"].ToString()
                    + "</t90_co2>");
                cxtj.Append("<t90_no>"
                    + ((DataRow)obj_data)["DATA14"].ToString()
                    + "</t90_no>");
                cxtj.Append("<t90_nox>"
                    + ((DataRow)obj_data)["DATA14"].ToString()
                    + "</t90_nox>");

                cxtj.Append("<t95_hc>"
                    + ((DataRow)obj_data)["DATA3"].ToString()
                    + "</t95_hc>");
                cxtj.Append("<t95_co>"
                    + ((DataRow)obj_data)["DATA7"].ToString()
                    + "</t95_co>");
                cxtj.Append("<t95_co2>"
                    + ((DataRow)obj_data)["DATA11"].ToString()
                    + "</t95_co2>");
                cxtj.Append("<t95_no>"
                    + ((DataRow)obj_data)["DATA15"].ToString()
                    + "</t95_no>");
                cxtj.Append("<t95_nox>"
                    + ((DataRow)obj_data)["DATA15"].ToString()
                    + "</t95_nox>");

                cxtj.Append("<t10_hc>"
                    + ((DataRow)obj_data)["DATA4"].ToString()
                    + "</t10_hc>");
                cxtj.Append("<t10_co>"
                    + ((DataRow)obj_data)["DATA8"].ToString()
                    + "</t10_co>");
                cxtj.Append("<t10_co2>"
                    + ((DataRow)obj_data)["DATA12"].ToString()
                    + "</t10_co2>");
                cxtj.Append("<t10_no>"
                    + ((DataRow)obj_data)["DATA16"].ToString()
                    + "</t10_no>");
                cxtj.Append("<t10_nox>"
                    + ((DataRow)obj_data)["DATA16"].ToString()
                    + "</t10_nox>");

                cxtj.Append("<t5_hc>"
                    + ((DataRow)obj_data)["DATA5"].ToString()
                    + "</t5_hc>");
                cxtj.Append("<t5_co>"
                    + ((DataRow)obj_data)["DATA9"].ToString()
                    + "</t5_co>");
                cxtj.Append("<t5_co2>"
                    + ((DataRow)obj_data)["DATA13"].ToString()
                    + "</t5_co2>");
                cxtj.Append("<t5_no>"
                    + ((DataRow)obj_data)["DATA17"].ToString()
                    + "</t5_no>");
                cxtj.Append("<t5_nox>"
                    + ((DataRow)obj_data)["DATA17"].ToString()
                    + "</t5_nox>");

                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr, 13);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        public bool CheckRecord_data(object obj_data, string type)
        {
            string FunctionName = "[设备自检记录]";
            string XmlStr = "";
            try
            {
                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");

                cxtj.Append("<type>" + type + "</type>");
                cxtj.Append("<start_time>"
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</start_time>");
                cxtj.Append("<end_time>"
                    + DateTime.Parse(((DataRow)obj_data)["JSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</end_time>");
                cxtj.Append("<passed>"
                    + ((DataRow)obj_data)["ZJJG"].ToString()
                    + "</passed>");
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr, 12);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        public bool CGJ_bzhhx(object obj_data)
        {
            string FunctionName = "[测功机变载荷滑行]";
            string XmlStr = "";
            try
            {
                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");
                DateTime kssj = DateTime.Parse(((DataRow)obj_data)["BDSJ"].ToString());
                cxtj.Append("<check_date>"
                    + DateTime.Parse(((DataRow)obj_data)["BDSJ"].ToString()).ToString("yyyy-MM-dd")
                    + "</check_date>");
                cxtj.Append("<start_time>"
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</start_time>");
                string[] speed= ((DataRow)obj_data)["DATA1"].ToString().Split(',');
                //string fh = ((DataRow)obj_data)["DATA1"].ToString();
                string[] time = ((DataRow)obj_data)["DATA2"].ToString().Split(',');
                string[] fh = { "3.7", "7.4", "14.7" };
                int count = speed.Count();
                for (int i = 0; i < count; i++)
                {
                    //cxtj.Append("<item>");
                    //cxtj.Append("<speed>"
                    //    + (speed[i].Split('-')[1])
                    //    + "</speed>");
                    //cxtj.Append("<fh>"
                    //    + fh[i]
                    //    + "</fh>");
                    //cxtj.Append("<time>"
                    //    + (kssj.AddSeconds(double.Parse(time[i])).ToString("yyyy-MM-dd HH:mm:ss"))
                    //    + "</time>");
                    //cxtj.Append("</item>");
                }
                cxtj.Append("<passed>"
                    + (((DataRow)obj_data)["BDJG"].ToString()=="合格"?"1":"0")
                    + "</passed>");
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr, 14);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        public bool NOX_zhl(object obj_data)
        {
            string FunctionName = "[NOx转换率]";
            string XmlStr = "";
            try
            {
                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");
                cxtj.Append("<check_date>"
                    + DateTime.Parse(((DataRow)obj_data)["ZJSJ"].ToString()).ToString("yyyy-MM-dd")
                    + "</check_date>");
                cxtj.Append("<start_time>"
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</start_time>");
                cxtj.Append("<no>"
                    + ((DataRow)obj_data)["DATA1"].ToString()
                    + "</no>");
                cxtj.Append("<no_sc>"
                    + ((DataRow)obj_data)["DATA2"].ToString()
                    + "</no_sc>");
                cxtj.Append("<no_zhl>"
                    + (double.Parse(((DataRow)obj_data)["DATA2"].ToString())*100.0/ double.Parse(((DataRow)obj_data)["DATA1"].ToString())).ToString("0.0")
                    + "</no_zhl>");
                cxtj.Append("<no2>"
                    + ((DataRow)obj_data)["DATA3"].ToString()
                    + "</no2>");
                cxtj.Append("<no2_sc>"
                    + ((DataRow)obj_data)["DATA4"].ToString()
                    + "</no2_sc>");
                cxtj.Append("<no2_zhl>"
                    + ((DataRow)obj_data)["DATA5"].ToString()
                    + "</no2_zhl>");
                cxtj.Append("<passed>"
                    + ((DataRow)obj_data)["ZJJG"].ToString()
                    + "</passed>");
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr, 15);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        public bool CGJ_force(object obj_data)
        {
            string FunctionName = "[测功机力传感器检查]";
            string XmlStr = "";
            try
            {
                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");
                cxtj.Append("<check_date>"
                    + DateTime.Parse(((DataRow)obj_data)["BDSJ"].ToString()).ToString("yyyy-MM-dd")
                    + "</check_date>");
                cxtj.Append("<start_time>"
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</start_time>");
                cxtj.Append("<zero_rela>"
                    + "0"
                    + "</zero_rela>");
                cxtj.Append("<zero_abso>"
                    + "0"
                    + "</zero_abso>");
                cxtj.Append("<one_rela>"
                    + ((DataRow)obj_data)["DATA5"].ToString()
                    + "</one_rela>");
                cxtj.Append("<one_abso>"
                    + ((DataRow)obj_data)["DATA4"].ToString()
                    + "</one_abso>");
                cxtj.Append("<two_rela>"
                    + ((DataRow)obj_data)["DATA12"].ToString()
                    + "</two_rela>");
                cxtj.Append("<two_abso>"
                    + ((DataRow)obj_data)["DATA11"].ToString()
                    + "</two_abso>");
                cxtj.Append("<three_rela>"
                    + ((DataRow)obj_data)["DATA19"].ToString()
                    + "</three_rela>");
                cxtj.Append("<three_abso>"
                    + ((DataRow)obj_data)["DATA18"].ToString()
                    + "</three_abso>");
                cxtj.Append("<four_rela>"
                    + ((DataRow)obj_data)["DATA26"].ToString()
                    + "</four_rela>");
                cxtj.Append("<four_abso>"
                    + ((DataRow)obj_data)["DATA25"].ToString()
                    + "</four_abso>");
                cxtj.Append("<five_rela>"
                    + ((DataRow)obj_data)["DATA33"].ToString()
                    + "</five_rela>");
                cxtj.Append("<five_abso>"
                    + ((DataRow)obj_data)["DATA32"].ToString()
                    + "</five_abso>");

                cxtj.Append("<cgjl_80>"
                    + ((DataRow)obj_data)["DATA23"].ToString()
                    + "</cgjl_80>");
                cxtj.Append("<lc80_abso>"
                    + ((DataRow)obj_data)["DATA25"].ToString()
                    + "</lc80_abso>");
                cxtj.Append("<lc80_rela>"
                    + ((DataRow)obj_data)["DATA26"].ToString()
                    + "</lc80_rela>");
                cxtj.Append("<passed_80>"
                    + (((DataRow)obj_data)["DATA28"].ToString()=="合格"?"1":"0")
                    + "</passed_80>");

                cxtj.Append("<cgjl_m>"
                    + ((DataRow)obj_data)["DATA30"].ToString()
                    + "</cgjl_m>");
                cxtj.Append("<mlc_abso>"
                    + ((DataRow)obj_data)["DATA32"].ToString()
                    + "</mlc_abso>");
                cxtj.Append("<mlc_rela>"
                    + ((DataRow)obj_data)["DATA33"].ToString()
                    + "</mlc_rela>");
                cxtj.Append("<passed_m>"
                    + (((DataRow)obj_data)["DATA35"].ToString() == "合格" ? "1" : "0")
                    + "</passed_m>");

                cxtj.Append("<passed>"
                    + (((DataRow)obj_data)["BDJG"].ToString() == "合格" ? "1" : "0")
                    + "</passed>");
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr, 16);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        public bool CGJ_speed(object obj_data)
        {
            string FunctionName = "[测功机转鼓转速检查]";
            string XmlStr = "";
            try
            {
                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");
                cxtj.Append("<check_date>"
                    + DateTime.Parse(((DataRow)obj_data)["BDSJ"].ToString()).ToString("yyyy-MM-dd")
                    + "</check_date>");
                cxtj.Append("<start_time>"
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</start_time>");
                cxtj.Append("<cgj_speed>"
                    + ((DataRow)obj_data)["DATA3"].ToString()
                    + "</cgj_speed>");
                cxtj.Append("<cgj_rpm>"
                    + (double.Parse(((DataRow)obj_data)["DATA3"].ToString())*60.0/(3.6*3.1415*0.218)).ToString("0")
                    + "</cgj_rpm>");
                cxtj.Append("<wsb_speed>"
                    + ((DataRow)obj_data)["DATA2"].ToString()
                    + "</wsb_speed>");
                cxtj.Append("<wsb_rpm>"
                    + (double.Parse(((DataRow)obj_data)["DATA2"].ToString()) * 60.0 / (3.6 * 3.1415 * 0.218)).ToString("0")
                    + "</wsb_rpm>");
                cxtj.Append("<sped_wc>"
                    + ((DataRow)obj_data)["DATA4"].ToString()
                    + "</sped_wc>");

                cxtj.Append("<passed>"
                    + (((DataRow)obj_data)["BDJG"].ToString() == "合格" ? "1" : "0")
                    + "</passed>");
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr, 17);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        public bool CGJ_diw(object obj_data)
        {
            string FunctionName = "[测功机惯量测试]";
            string XmlStr = "";
            try
            {
                StringBuilder cxtj = new StringBuilder();
                cxtj.Append("<?xml version='1.0' encoding='GBK'?>");
                cxtj.Append("<result>");
                cxtj.Append("<result_data>");
                cxtj.Append("<unit_id>" + ModPublicJiangsu.unitid + "</unit_id>");
                cxtj.Append("<line_id>" + ModPublicJiangsu.DicLineid.GetValue(((DataRow)obj_data)["JCGWH"].ToString(), "00000000000") + "</line_id>");
                cxtj.Append("<check_date>"
                    + DateTime.Parse(((DataRow)obj_data)["BDSJ"].ToString()).ToString("yyyy-MM-dd")
                    + "</check_date>");
                cxtj.Append("<start_time>"
                    + DateTime.Parse(((DataRow)obj_data)["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    + "</start_time>");
                cxtj.Append("<jbgl>"
                    + ((DataRow)obj_data)["DATA24"].ToString()
                    + "</jbgl>");
                cxtj.Append("<csgl>"
                    + ((DataRow)obj_data)["DATA25"].ToString()
                    + "</csgl>");
                cxtj.Append("<gl_wc>"
                    + ((DataRow)obj_data)["DATA26"].ToString()
                    + "</gl_wc>");
                cxtj.Append("<gl_limit>"
                    + "4.5"
                    + "</gl_limit>");

                cxtj.Append("<passed>"
                    + (((DataRow)obj_data)["BDJG"].ToString() == "合格" ? "1" : "0")
                    + "</passed>");
                cxtj.Append("<jcry>"
                    + ((DataRow)obj_data)["CZY"].ToString()
                    + "</jcry>");
                cxtj.Append("</result_data>");
                cxtj.Append("</result>");
                String strCon = cxtj.ToString();

                FileOpreate.SaveLog(strCon, FunctionName, 3);
                XmlStr = strCon;
                return UploadZjBdData(XmlStr, 18);

            }
            catch (Exception ex)
            {
                FileOpreate.SaveLog(ex.Message, FunctionName, 4);
                FileOpreate.SaveLog("失败", FunctionName, 3);
                XmlStr = null;
                return false;
            }

        }
        public bool UploadZjBdData(string XmlStr,int action)
        {
            if (!string.IsNullOrEmpty(XmlStr))
            {
                try
                {
                    FileOpreate.SaveLog("标定数据发送：\r\n"+"XmlStr=" + XmlStr+"\r\n"+"action="+action.ToString(), "[江苏联网]", 3);
                    string UpResult = ModPublicJiangsu.cobd.UploadBDResult( XmlStr, action);
                    FileOpreate.SaveLog("标定数据返回：\r\n" + UpResult, "[江苏联网]", 3);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(UrlDecode(UpResult));
                    XmlNode node1 = xmlDoc.SelectSingleNode("xml");
                    String state = node1.SelectSingleNode("status").InnerText;
                    return state == "true" ? true : false;
                }
                catch(Exception er)
                {
                    FileOpreate.SaveLog(er.Message, "[江苏上传标定数据发生异常", 3);
                    return false;
                }
            }
            else
                return false;
        }
    }
}
