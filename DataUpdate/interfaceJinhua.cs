using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Windows.Forms;
using DataUpdate;
using System.Data;

namespace DataUpdate
{

    public class interfaceJinhua
    {
        public static Dictionary<string, string> DicCpys = new Dictionary<string, string>();
        public static Dictionary<string, string> DicCpysR = new Dictionary<string, string>();
        public static Dictionary<string, string> DicHpzl = new Dictionary<string, string>();
        public static Dictionary<string, string> DicHpzlR = new Dictionary<string, string>();
        public static Dictionary<string, string> DicRlzl = new Dictionary<string, string>();
        public static Dictionary<string, string> DicRlzlR = new Dictionary<string, string>();
        public static Dictionary<string, string> DicJcff = new Dictionary<string, string>();
        public static Dictionary<string, string> DicJcffR = new Dictionary<string, string>();
        public static Dictionary<string, string> DicResult = new Dictionary<string, string>();
        public static Dictionary<string, string> DicResultR = new Dictionary<string, string>();

        public static Dictionary<string, string> DicSyxz = new Dictionary<string, string>();
        public static Dictionary<string, string> DicSyxzR = new Dictionary<string, string>();
        public static Dictionary<string, string> DicCsys = new Dictionary<string, string>();
        public static Dictionary<string, string> DicJclx = new Dictionary<string, string>();

        public static Dictionary<string, string> DicJqfs = new Dictionary<string, string>();
        public static Dictionary<string, string> DicQdfs = new Dictionary<string, string>();
        public static Dictionary<string, string> DicBsqxs = new Dictionary<string, string>();
        public static Dictionary<string, string> DicHpys = new Dictionary<string, string>();
        public static Dictionary<string, string> DicPfbz = new Dictionary<string, string>();
        public static Dictionary<string, string> DicOBD = new Dictionary<string, string>();
        public static Dictionary<string, string> DicGyfs = new Dictionary<string, string>();
        public static Dictionary<string, string> DicJHZZ = new Dictionary<string, string>();

        public static Dictionary<string, string> DicDeviceType = new Dictionary<string, string>();
        public interfaceJinhua()
        {
            DicHpzl.Add("01", "大型汽车");
            DicHpzl.Add("02", "小型汽车");
            DicHpzl.Add("03", "使馆汽车");
            DicHpzl.Add("04", "领馆汽车");
            DicHpzl.Add("05", "境外汽车");
            DicHpzl.Add("06", "外籍汽车");
            DicHpzl.Add("07", "两、三轮摩托车");
            DicHpzl.Add("08", "轻便摩托车");
            DicHpzl.Add("09", "使馆摩托车");
            DicHpzl.Add("10", "领馆摩托车");
            DicHpzl.Add("11", "境外摩托车");
            DicHpzl.Add("12", "外籍摩托车");
            DicHpzl.Add("13", "农用运输车");
            DicHpzl.Add("14", "拖拉机");
            DicHpzl.Add("15", "挂车");
            DicHpzl.Add("16", "教练汽车");
            DicHpzl.Add("17", "教练摩托车");
            DicHpzl.Add("18", "试验汽车");
            DicHpzl.Add("19", "试验摩托车");
            DicHpzl.Add("20", "临时人境汽车");
            DicHpzl.Add("21", "临时人境摩托车");
            DicHpzl.Add("22", "临时行驶车");
            DicHpzl.Add("23", "警用汽车");
            DicHpzl.Add("24", "警用摩托");
            DicHpzl.Add("99", "其他");

            DicHpzlR.Add("大型汽车", "01");
            DicHpzlR.Add("小型汽车", "02");
            DicHpzlR.Add("使馆汽车", "03");
            DicHpzlR.Add("领馆汽车", "04");
            DicHpzlR.Add("境外汽车", "05");
            DicHpzlR.Add("外籍汽车", "06");
            DicHpzlR.Add("两、三轮摩托车", "07");
            DicHpzlR.Add("轻便摩托车", "08");
            DicHpzlR.Add("使馆摩托车", "09");
            DicHpzlR.Add("领馆摩托车", "10");
            DicHpzlR.Add("境外摩托车", "11");
            DicHpzlR.Add("外籍摩托车", "12");
            DicHpzlR.Add("农用运输车", "13");
            DicHpzlR.Add("拖拉机", "14");
            DicHpzlR.Add("挂车", "15");
            DicHpzlR.Add("教练汽车", "16");
            DicHpzlR.Add("教练摩托车", "17");
            DicHpzlR.Add("试验汽车", "18");
            DicHpzlR.Add("试验摩托车", "19");
            DicHpzlR.Add("临时人境汽车", "20");
            DicHpzlR.Add("临时人境摩托车", "21");
            DicHpzlR.Add("临时行驶车", "22");
            DicHpzlR.Add("警用汽车", "23");
            DicHpzlR.Add("警用摩托", "24");
            DicHpzlR.Add("其他", "99");

            DicRlzl.Add("A", "汽油");
            DicRlzl.Add("B", "柴油");
            DicRlzl.Add("C", "电");
            DicRlzl.Add("D", "混合油");
            DicRlzl.Add("E", "天然气");
            DicRlzl.Add("F", "液化石油气");
            DicRlzl.Add("L", "甲醇");
            DicRlzl.Add("M", "乙醇");
            DicRlzl.Add("N", "太阳能");
            DicRlzl.Add("O", "混合动力");
            DicRlzl.Add("X", "氢");
            DicRlzl.Add("Y", "无");
            DicRlzl.Add("Z", "其他");
            DicRlzl.Add("AE", "汽油天然气");
            DicRlzl.Add("BO", "柴油混合动力");
            DicRlzl.Add("BC", "柴油电动混合");
            DicRlzl.Add("AC", "汽油电动混合");
            DicRlzl.Add("AO", "汽油混合动力");
            DicRlzl.Add("A1", "油改气");

            DicRlzlR.Add("汽油", "A");
            DicRlzlR.Add("柴油", "B");
            DicRlzlR.Add("电", "C");
            DicRlzlR.Add("混合油", "D");
            DicRlzlR.Add("天然气", "E");
            DicRlzlR.Add("液化石油气", "F");
            DicRlzlR.Add("甲醇", "L");
            DicRlzlR.Add("乙醇", "M");
            DicRlzlR.Add("太阳能", "N");
            DicRlzlR.Add("混合动力", "O");
            DicRlzlR.Add("氢", "X");
            DicRlzlR.Add("无", "Y");
            DicRlzlR.Add("其他", "Z");
            DicRlzlR.Add("汽油天然气", "AE");
            DicRlzlR.Add("柴油混合动力", "BO");
            DicRlzlR.Add("柴油电动混合", "BC");
            DicRlzlR.Add("汽油电动混合", "AC");
            DicRlzlR.Add("汽油混合动力", "AO");
            DicRlzlR.Add("油改气", "A1");

            DicCpys.Add("1", "蓝牌");
            DicCpys.Add("2", "黄牌");
            DicCpys.Add("3", "黑牌");
            DicCpys.Add("4", "白牌");
            DicCpys.Add("5", "绿牌");
            DicCpys.Add("0", "未知");

            DicCpysR.Add("蓝牌", "1");
            DicCpysR.Add("黄牌", "2");
            DicCpysR.Add("黑牌", "3");
            DicCpysR.Add("白牌", "4");
            DicCpysR.Add("绿牌", "5");
            DicCpysR.Add("未知", "0");


            DicJcff.Add("2", "SDS");
            DicJcff.Add("4", "ZYJS");
            DicJcff.Add("5", "ASM");
            DicJcff.Add("6", "VMAS");
            DicJcff.Add("7", "JZJS");
            DicJcffR.Add("SDS", "2");
            DicJcffR.Add("ZYJS", "4");
            DicJcffR.Add("ASM", "5");
            DicJcffR.Add("VMAS", "6");
            DicJcffR.Add("JZJS", "7");

            DicResult.Add("1", "合格");
            DicResult.Add("0", "不合格");
            DicResult.Add("9", "");
            DicResultR.Add("合格", "1");
            DicResultR.Add("不合格", "0");
            DicResultR.Add("", "9");
            DicDeviceType.Add("测功机", "1");
            DicDeviceType.Add("五气分析仪", "2");
            DicDeviceType.Add("烟度计", "3");
            DicDeviceType.Add("电子环境信息仪", "4");
            DicDeviceType.Add("发动机转速仪", "5");
            DicDeviceType.Add("流量计", "6");
            DicDeviceType.Add("滤纸烟度计", "7");
            DicDeviceType.Add("工控机", "8");

            DicCsys.Add("A", "白");
            DicCsys.Add("B", "灰");
            DicCsys.Add("C", "黄");
            DicCsys.Add("D", "粉");
            DicCsys.Add("E", "红");
            DicCsys.Add("F", "紫");
            DicCsys.Add("G", "绿");
            DicCsys.Add("H", "蓝");
            DicCsys.Add("I", "棕");
            DicCsys.Add("J", "黑");
            DicCsys.Add("Z", "其他");

            DicJclx.Add("1", "定期检验");
            DicJclx.Add("2", "抽检复检");
            DicJclx.Add("3", "实验比对");
            DicJclx.Add("4", "外地车委托检验");
            DicJclx.Add("5", "外地车转入检验");
            DicJclx.Add("6", "特殊检验");
            DicJclx.Add("7", "临时检验");
            DicJclx.Add("8", "新车上牌检验");
            DicJclx.Add("9", "发动机变更检验");
            DicJclx.Add("0", "其他");

            DicSyxz.Add("A", "非营运");
            DicSyxz.Add("B", "公路客运");
            DicSyxz.Add("C", "公交客运");
            DicSyxz.Add("D", "出租客运");
            DicSyxz.Add("E", "旅游客运");
            DicSyxz.Add("F", "货运");
            DicSyxz.Add("G", "租赁");
            DicSyxz.Add("H", "警用");
            DicSyxz.Add("I", "消防");
            DicSyxz.Add("J", "救护");
            DicSyxz.Add("K", "工程抢险");
            DicSyxz.Add("L", "营转非");
            DicSyxz.Add("M", "出租转非");
            DicSyxz.Add("N", "教练");
            DicSyxz.Add("O", "幼儿校车");
            DicSyxz.Add("P", "小学生校车");
            DicSyxz.Add("Q", "初中生校车");
            DicSyxz.Add("R", "危险品运输");
            DicSyxz.Add("S", "中小学生校车");
            DicSyxz.Add("Z", "其他");

            DicJqfs.Add("1", "自然吸气");
            DicJqfs.Add("2", "涡轮增压");

            DicQdfs.Add("1", "前驱");
            DicQdfs.Add("2", "后驱");
            DicQdfs.Add("3", "四驱");
            DicQdfs.Add("4", "全时四驱");

            DicBsqxs.Add("1", "手动");
            DicBsqxs.Add("2", "自动");

            DicHpys.Add("1", "蓝牌");
            DicHpys.Add("2", "黄牌");
            DicHpys.Add("3", "黑牌");
            DicHpys.Add("4", "白牌");
            DicHpys.Add("5", "绿牌");
            DicHpys.Add("0", "未知");

            DicPfbz.Add("1", "国Ⅰ");
            DicPfbz.Add("2", "国Ⅱ");
            DicPfbz.Add("3", "国Ⅲ");
            DicPfbz.Add("4", "国Ⅳ");
            DicPfbz.Add("5", "国Ⅴ");
            DicPfbz.Add("6", "国Ⅵ");
            DicPfbz.Add("0", "国0");

            DicOBD.Add("1", "有");
            DicOBD.Add("0", "无");

            DicGyfs.Add("1", "开环电喷");
            DicGyfs.Add("2", "闭环电喷");
            DicGyfs.Add("3", "直喷");
            DicGyfs.Add("4", "高压共轨");
            DicGyfs.Add("5", "泵喷式");
            DicGyfs.Add("9", "化油器");
            DicGyfs.Add("0", "其他");

            DicJHZZ.Add("1", "汽油车有三元催化");
            DicJHZZ.Add("2", "柴油车有尾气过滤装置");
        }
        
        public bool synSystemTime(out DateTime syntime)
        {
            //创建JSON串
            string modeljson = createSynctime();
            syntime = DateTime.Now;
            //GET数据 提取时间信息
            if (GetSynctime(ModPublicJHHB.webAddress, modeljson, "GET", out syntime))
            {
                FileOpreate.SaveLog( "成功","[downInspSynctime_Parser]：", 3);
                //Console.WriteLine("POST success!");
                return true;
            }
            else
            {
                FileOpreate.SaveLog("失败", "[downInspSynctime_Parser]：",  3);
                Console.WriteLine("POST fail!");
                return false;
            }
        }
        public string createSynctime()
        {
            ModPublicJHHB.downInspSynctime model = new ModPublicJHHB.downInspSynctime();

            model.jkid = "downInspSynctime";
            model.jksqm = ModPublicJHHB.jksqm;
            model.deviceCode = "20190201-01-1";

            string json = JsonConvert.SerializeObject(model);
            FileOpreate.SaveLog(json,"[downInspSynctime]：",  3);
            return json;
        }

        //返回信息
        bool GetSynctime(string webadd, string content, string model, out DateTime time)
        {
            time = DateTime.Now;
            if (content == "")
            {
                MessageBox.Show("数据不能为空"); return false;
            }
            try
            {
                byte[] d = System.Text.Encoding.UTF8.GetBytes(content);
                System.Net.WebClient aaa = new System.Net.WebClient();
                aaa.Headers.Add("Content-Type", "application/json; charset=UTF-8");
                byte[] res;
                if (model == "GET")
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                else
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                String ReadData = System.Text.Encoding.UTF8.GetString(res);
                //textBoxResult.Text = ReadData;
                string jsonText = ReadData;
                FileOpreate.SaveLog(jsonText,"[downInspSynctime_Ack]：",  3);
                ModPublicJHHB.uploadAck ack = JsonConvert.DeserializeObject<ModPublicJHHB.uploadAck>(jsonText);
                if (ack.result[0].code != null && ack.result[0].code != "" && ack.result[0].code != "1" && ack.result[0].code != "2")
                {
                    MessageBox.Show("Code:" + ack.result[0].code + "\r\n;Message:" + ack.result[0].message);
                    return false;
                }
                Newtonsoft.Json.Linq.JObject JsonObj;
                JsonObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonText); //将json字符串转化为json对象 
                string timestring = GetRegexStr(jsonText, "synctime");
                timestring = timestring.Substring(0, 4) + "/" + timestring.Substring(4, 2) + "/" + timestring.Substring(6, 2) + " " + timestring.Substring(8, 2) + ":" + timestring.Substring(10, 2) + ":" + timestring.Substring(12, 6);
                time = DateTime.Parse(timestring);
                return true;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog(er.Message,"[downInspSynctime_Eception]：",  3);
                Console.WriteLine("exception occured:" + er.Message);
                //labelStatus.Text = "exception occured:" + er.Message;
                //textBoxResult.Text = er.Message;
                return false;
            }
        }
        //正则提取嵌套内容节点 
        public DataTable GetVehicleList()
        {
            //创建JSON串
            string modeljson = createCanInfo();
            //textBoxJson.Text = modeljson;

            //GET数据  
            List<ModPublicJHHB.uploadRegResult> vlist;
            if (GetCarInfo(ModPublicJHHB.webAddress, modeljson, "GET",out vlist))
            {
                FileOpreate.SaveLog("成功", "[downInspRegInfo_Parser]：", 3);
                //labelStatus.Text = "POST success!";
                Console.WriteLine("POST success!");
                DataTable dt = new DataTable();
                dt.Columns.Add("jylsh");
                dt.Columns.Add("jycs");
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
                foreach (ModPublicJHHB.uploadRegResult child in vlist)
                {
                    ModPublicJHHB.uploadRegBody child1 = child.body[0];
                    int a;
                    double b;
                    string timestring;
                    try
                    {
                        DataRow dr = dt.NewRow();
                        dr["jylsh"] = child1.insp.inspregid;
                        dr["jycs"] = child1.insp.TestTimes;
                        dr["hphm"] = child1.insp_vehicle.License;
                        dr["hpys"] = DicHpys.GetValue(child1.insp_vehicle.LicenseType, "");
                        dr["hpzl"] = DicHpzl.GetValue(child1.insp_vehicle.LicenseCode, "");
                        dr["cllx"] = child1.insp_vehicle.VehicleType;
                        dr["cpxh"] = child1.insp_vehicle.Brand;
                        dr["cpmc"] = child1.insp_vehicle.Brand;
                        dr["clsbm"] = child1.insp_vehicle.VIN;
                        dr["clscqy"] = child1.insp_vehicle.Manuf;
                        dr["fdjxh"] = child1.insp_vehicle.Engine;
                        dr["fdjscqy"] = "";
                        dr["fdjh"] = child1.insp_vehicle.EngineNo;
                        dr["fdjpl"] = child1.insp_vehicle.ED;
                        if (double.TryParse(child1.insp_vehicle.GVM, out b))
                            dr["fdjedzs"] = b.ToString("0");
                        else
                            dr["fdjedzs"] = "3000";
                        
                        dr["fdjedgl"] = child1.insp_vehicle.EnginePower;
                        
                        dr["syxz"] = "";
                        try
                        {
                            timestring = child1.insp_vehicle.MDate;
                            timestring = timestring.Substring(0, 4) + "/" + timestring.Substring(4, 2) + "/" + timestring.Substring(6, 2) + " " + timestring.Substring(8, 2) + ":" + timestring.Substring(10, 2) + ":" + timestring.Substring(12, 2);
                            dr["ccdjrq"] = DateTime.Parse(timestring).ToString("yyyy-MM-dd");
                        }
                        catch
                        {
                            dr["ccdjrq"] = DateTime.Now;
                        }
                        dr["ccrq"] = dr["ccdjrq"];
                        dr["czmc"] = child1.insp_vehicle.Owner;
                        dr["lxdh"] = child1.insp_vehicle.OwnerTel;
                        dr["lxdz"] = child1.insp_vehicle.OwnerAddr;
                        dr["pfbz"] = DicPfbz.GetValue(child1.insp_vehicle.Standard, "");
                        dr["bsqxs"] = DicBsqxs.GetValue(child1.insp_vehicle.Gear, "");
                        dr["jqfs"] = DicJqfs.GetValue(child1.insp_vehicle.AirIn, "");
                        dr["ryzl"] = DicRlzl.GetValue(child1.insp_vehicle.FuelType, "");
                        dr["gyfs"] = DicGyfs.GetValue(child1.insp_vehicle.FuelWay, "");
                        dr["qdfs"] = DicQdfs.GetValue(child1.insp_vehicle.DriveMode, "");
                        if (int.TryParse(child1.insp_vehicle.GVM, out a))
                            dr["zdzzl"] = child1.insp_vehicle.GVM;
                        else
                            dr["zdzzl"] = "1500";
                        if (int.TryParse(child1.insp_vehicle.RM, out a))
                            dr["jzzl"] = child1.insp_vehicle.RM;
                        else
                            dr["jzzl"] = "1000";

                        dr["zbzl"] = (int.Parse(dr["jzzl"].ToString()) - 100).ToString();// child1.insp_vehicle.VIN;
                        dr["qgs"] = child1.insp_vehicle.Cylinders;
                        dr["qdltqy"] = "";
                        dr["chzhq"] = "";
                        dr["rygg"] = "";
                        dr["sjcys"] = child1.insp_vehicle.Passcap;
                        dr["ssxq"] = "";
                        dr["dws"] = "";
                        dr["jclx"] = "";
                        dr["jcff"] = DicJcff.GetValue(child1.insp.TestType, "");
                        dr["obd"] = DicOBD.GetValue(child1.insp_vehicle.isOBD, "");
                        dr["jczbh"] = child1.insp.TsNo;
                        dr["ljxslc"] = child1.insp_vehicle.Odometer;
                        try
                        {
                            timestring = child1.insp.TestType;
                            timestring = timestring.Substring(0, 4) + "/" + timestring.Substring(4, 2) + "/" + timestring.Substring(6, 2) + " " + timestring.Substring(8, 2) + ":" + timestring.Substring(10, 2) + ":" + timestring.Substring(12, 2);
                            dr["dlsj"] = DateTime.Parse(timestring).ToString("yyyy-MM-dd");
                        }
                        catch
                        {
                            dr["dlsj"] = DateTime.Now;
                        }
                        dt.Rows.Add(dr);
                    }
                    catch(Exception er)
                    {
                        FileOpreate.SaveLog("解析车辆信息失败，license="+child1.insp_vehicle.License+",exception:"+er.Message, "[downInspRegInfo_Parser]：", 3);
                    }
                }
                return dt;
            }
            else
            {
                FileOpreate.SaveLog("失败", "[downInspRegInfo_Parser]：", 3);
                //labelStatus.Text = "POST fail!";
                Console.WriteLine("POST fail!");
                return null;
            }
        }

        string createCanInfo()
        {
            ModPublicJHHB.downInspRegInfo model = new ModPublicJHHB.downInspRegInfo();
            model.jkid = "downInspRegInfo";
            model.jksqm = ModPublicJHHB.jksqm;
            model.VIN = "";
            model.EngineNo = "";
            model.TsNo = ModPublicJHHB.TsNo;
            string json = JsonConvert.SerializeObject(model);
            FileOpreate.SaveLog("[downInspRegInfo]：", json, 3);
            return json;
        }


        // 车辆信息获取
        bool GetCarInfo(string webadd, string content, string model,out List<ModPublicJHHB.uploadRegResult> vlist)
        {
            vlist = new List<ModPublicJHHB.uploadRegResult>();
            if (content == "")
            {
                MessageBox.Show("数据不能为空"); return false;
            }
            try
            {
                byte[] d = System.Text.Encoding.UTF8.GetBytes(content);
                System.Net.WebClient aaa = new System.Net.WebClient();
                aaa.Headers.Add("Content-Type", "application/json; charset=UTF-8");
                byte[] res;
                if (model == "GET")
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                else
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                String ReadData = System.Text.Encoding.UTF8.GetString(res);
                //textBoxResult.Text = ReadData;
                string jsonText = ReadData;
                FileOpreate.SaveLog(jsonText, "[downInspRegInfo_Ack]：", 3);
                ModPublicJHHB.uploadAck ack = JsonConvert.DeserializeObject<ModPublicJHHB.uploadAck>(jsonText);
                if (ack.result[0].code != null && ack.result[0].code != "" && ack.result[0].code != "1" && ack.result[0].code != "2")
                {
                    MessageBox.Show("Code:" + ack.result[0].code + "\r\n;Message:" + ack.result[0].message);
                    return false;
                }
                ModPublicJHHB.uploadReg reg = JsonConvert.DeserializeObject<ModPublicJHHB.uploadReg>(jsonText);
                vlist = reg.result;
                return true;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog(er.Message, "[downInspRegInfo_Eception]：", 3);
                Console.WriteLine("exception occured:" + er.Message);
                //labelStatus.Text = "exception occured:" + er.Message;
                //textBoxResult.Text = er.Message;
                return false;
            }
        }
        private string GetRegexStr(string jsonText, string reString)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("\"" + reString + "\":\"([^\"]*)\"");
            return reg.Match(jsonText).Groups[1].Value;
        }
        public bool GetObdResultId(string inspregid, out string obdresultid)
        {
            //创建JSON串
            obdresultid = "";
            string modeljson = createOBDResultInfo(inspregid);
            //textBoxJson.Text = modeljson;

            //GET数据  
            if (GetOBDResultInfo(ModPublicJHHB.webAddress, modeljson, "GET", out obdresultid))
            {
                //labelStatus.Text = "POST success!";

                FileOpreate.SaveLog( "成功","[downInspOBDResultInfo_Parser]：", 3);
                Console.WriteLine("POST success!");
                return true;
            }
            else
            {
                FileOpreate.SaveLog( "失败","[downInspOBDResultInfo_Parser]：", 3);
                //labelStatus.Text = "POST fail!";
                Console.WriteLine("POST fail!");
                return false;
            }
        }
        // obd检测结果id获取报文生成
        public string createOBDResultInfo(string obj)
        {
            ModPublicJHHB.downInspOBDResultInfo model = new ModPublicJHHB.downInspOBDResultInfo();

            model.jkid = "downInspOBDResultInfo";
            model.jksqm = ModPublicJHHB.jksqm;
            model.inspregid = obj;

            string json = JsonConvert.SerializeObject(model);
            FileOpreate.SaveLog("[downInspOBDResultInfo]：", json, 3);
            return json;
        }


        //返回obd检测结果id
        bool GetOBDResultInfo(string webadd, string content, string model, out string obdresultid)
        {
            obdresultid = "";
            if (content == "")
            {
                MessageBox.Show("数据不能为空"); return false;
            }
            try
            {
                byte[] d = System.Text.Encoding.UTF8.GetBytes(content);
                System.Net.WebClient aaa = new System.Net.WebClient();
                aaa.Headers.Add("Content-Type", "application/json; charset=UTF-8");
                byte[] res;
                if (model == "GET")
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                else
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                String ReadData = System.Text.Encoding.UTF8.GetString(res);
                //textBoxResult.Text = ReadData;
                string jsonText = ReadData;
                FileOpreate.SaveLog(jsonText,"[downInspOBDResultInfo_Ack]：",  3);
                Newtonsoft.Json.Linq.JObject JsonObj;
                JsonObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonText); //将json字符串转化为json对象 

                //读取json对象里的字符串对应值
                //Code = JsonObj["jkid"].ToString();
                //Status = JsonObj["jksqm"].ToString();

                //正则提取嵌套内容节点 --- 车辆OBD检测结果id（obdresultid）	
                obdresultid = GetRegexStr(jsonText, "obdresultid");
                //obdresultid= ModPublicJHHB.obdresultid;

                return true;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog( er.Message,"[downInspOBDResultInfo_Eception]：", 3);
                Console.WriteLine("exception occured:" + er.Message);
                //labelStatus.Text = "exception occured:" + er.Message;
                //textBoxResult.Text = er.Message;
                return false;
            }
        }
        public bool GetTestingId(string inspregid, out string testingid)
        {
            //创建JSON串
            testingid = "";
            string modeljson = createOBDResultInfo(inspregid);
            //textBoxJson.Text = modeljson;

            //GET数据  
            if (GetOBDResultInfo(ModPublicJHHB.webAddress, modeljson, "GET", out testingid))
            {
                //labelStatus.Text = "POST success!";

                FileOpreate.SaveLog("成功","[downInspTestingResultInfo_Parser]：",  3);
                Console.WriteLine("POST success!");
                return true;
            }
            else
            {
                FileOpreate.SaveLog("失败","[downInspTestingResultInfo_Parser]：",  3);
                //labelStatus.Text = "POST fail!";
                Console.WriteLine("POST fail!");
                return false;
            }
        }
        // obd检测结果id获取报文生成
        public string createTestingResultInfo(string obj)
        {
            ModPublicJHHB.downInspTestingResultInfo model = new ModPublicJHHB.downInspTestingResultInfo();

            model.jkid = "downInspTestingResultInfo";
            model.jksqm = ModPublicJHHB.jksqm;
            model.inspregid = obj;

            string json = JsonConvert.SerializeObject(model);
            FileOpreate.SaveLog(json,"[downInspTestingResultInfo]：",  3);
            return json;
        }


        //返回obd检测结果id
        bool GetTestingResultInfo(string webadd, string content, string model, out string testingid)
        {
            testingid = "";
            if (content == "")
            {
                MessageBox.Show("数据不能为空"); return false;
            }
            try
            {
                byte[] d = System.Text.Encoding.UTF8.GetBytes(content);
                System.Net.WebClient aaa = new System.Net.WebClient();
                aaa.Headers.Add("Content-Type", "application/json; charset=UTF-8");
                byte[] res;
                if (model == "GET")
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                else
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                String ReadData = System.Text.Encoding.UTF8.GetString(res);
                //textBoxResult.Text = ReadData;
                string jsonText = ReadData;
                FileOpreate.SaveLog(jsonText,"[downInspTestingResultInfo_Ack]：",  3);
                Newtonsoft.Json.Linq.JObject JsonObj;
                JsonObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonText); //将json字符串转化为json对象 

                //读取json对象里的字符串对应值
                //Code = JsonObj["jkid"].ToString();
                //Status = JsonObj["jksqm"].ToString();

                //正则提取嵌套内容节点 --- 车辆OBD检测结果id（obdresultid）	
                testingid = GetRegexStr(jsonText, "testingid");
                //obdresultid= ModPublicJHHB.obdresultid;

                return true;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog( er.Message, "[downInspTestingResultInfo_Eception]：",3);
                Console.WriteLine("exception occured:" + er.Message);
                //labelStatus.Text = "exception occured:" + er.Message;
                //textBoxResult.Text = er.Message;
                return false;
            }
        }
        public bool uploadOBDResult(object obj_car, object obj_obd,out string obdresultid)
        {
            string modeljson = createResultOBD(obj_car,obj_obd);
            //textBoxJson.Text = modeljson;
            obdresultid = "";
            //GET数据 
            if (GetResultOBD(ModPublicJHHB.webAddress, modeljson, "GET",out obdresultid))
            {
                FileOpreate.SaveLog("成功","[uploadInspResultOBD_Parser]：",  3);
                //labelStatus.Text = "POST success!";
                Console.WriteLine("POST success!");
                return true;
            }
            else
            {
                FileOpreate.SaveLog( "失败","[uploadInspResultOBD_Parser]：", 3);
                //labelStatus.Text = "POST fail!";
                Console.WriteLine("POST fail!");
                return false;
            }
        }
        // 报文生成
        //根据已检车辆信息和OBD信息生成报文
        //说明，OBD表中使用BY1作为operatorName(检测人姓名)
        public string createResultOBD(object obj_car, object obj_obd)
        {

            ModPublicJHHB.uploadInspResultOBD model = new ModPublicJHHB.uploadInspResultOBD();
            model.jkid = "uploadInspResultOBD";
            model.jksqm = ModPublicJHHB.jksqm;

            List<ModPublicJHHB.ResultOBDDataItem> data = new List<ModPublicJHHB.ResultOBDDataItem>();
            ModPublicJHHB.ResultOBDDataItem dt = new ModPublicJHHB.ResultOBDDataItem();
            data.Add(dt);
            model.data = data;

            List<ModPublicJHHB.ResultOBDbodyItem> body = new List<ModPublicJHHB.ResultOBDbodyItem>();
            ModPublicJHHB.ResultOBDbodyItem bo = new ModPublicJHHB.ResultOBDbodyItem();

            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");                   //id Odb检测记录id   字符(32)  检测线obd检测结果私有唯一标识 是
            bo.TsNo = ModPublicJHHB.TsNo;                 //TsNo 检测机构编号  字符(16)  格式详见3定义部分 是
            bo.inspregid = ((DataRow)obj_car)["JYLSH"].ToString();            //inspregid 检验登录ID  字符(32)  由环保局返回的检测登录唯一标识 是
            bo.VIN = ((DataRow)obj_car)["CLSBM"].ToString();                  //VIN 车辆识别代号  字符(32)  车辆识别代号VIN（须完整上报）	是
            bo.EngineNo = ((DataRow)obj_car)["FDJHM"].ToString();             //EngineNo    发动机编号 字符(32)      是
            bo.odo = ((DataRow)obj_obd)["XSLC"].ToString();                  //odo 车辆累计行驶里程（ODO）	数值(4)       是
            bo.ecu1 = "发动机控制控制单元";                 //ecu1    控制单元名字 字符(32)  默认值“发动机控制单元”	是
            bo.calid1 = ((DataRow)obj_obd)["CALID"].ToString();               //calid1  发动机控制控制单元 CAL ID 字符(32)      是
            bo.cvn1 = ((DataRow)obj_obd)["CVN"].ToString();                 //cvn1    发动机控制控制单元 CVN   字符(32)
            bo.ecu2 = "";                 //ecu2 控制单元名字  字符(32)  默认值“后处理控制单元”	
            bo.calid2 = "";               //calid2 后处理器控制控制单元 CAL ID   字符(32)
            bo.cvn2 = "";                 //cvn2 后处理器控制单元 CVN 字符(32)
            bo.ecu3 = "";                 //ecu3 其他控制单元名称    字符(32)
            bo.calid3 = "";               //calid3 其他控制单元CAL ID 字符(32)
            bo.cvn3 = "";                 //cvn3 其他控制单元CVN   字符(32)
            bo.indicatorCheck = "1";       //indicatorCheck OBD故障指示器检测  字符(1)   1：合格 0：不合格 是
            bo.communicationCheck = "1";   //communicationCheck OBD诊断仪通讯情况  字符(1)   1：成功 0：不成功 是
            bo.communicationFalseRes = "";//communicationFalseRes OBD诊断仪通讯不合格原因: 1：接口损坏 2：找不到接口 3：连接后不能通讯 多选项使用“,”分割
            //string noReadyStatePros= ((DataRow)obj_obd)["ZDJXZT"].ToString();            
            bo.faultCodes = ((DataRow)obj_obd)["GZDM"].ToString().Replace(";", ",");           //faultCodes  故障代码 多故障代码用“,”隔开
            string[] noReadyDate = ((DataRow)obj_obd)["ZDJXZT"].ToString().Split('/');
            string noReadyStatePros ="";
            string jcff = ((DataRow)obj_car)["JCFF"].ToString();
            if (jcff == "ASM" || jcff == "SDS"||jcff=="VMAS")
            {
                if (noReadyDate[1] == "0") noReadyStatePros += "5,";
                if (noReadyDate[2] == "0") noReadyStatePros += "6,";
                if (noReadyDate[3] == "0") noReadyStatePros += "7,";
                if (noReadyDate[4] == "0") noReadyStatePros += "8,";
                if (noReadyStatePros != "") noReadyStatePros.Remove(noReadyStatePros.Length - 1, 1);
            }
            else
            {
                if (noReadyDate[1] == "0") noReadyStatePros += "1,";
                if (noReadyDate[2] == "0") noReadyStatePros += "2,";
                if (noReadyDate[3] == "0") noReadyStatePros += "3,";
                if (noReadyDate[4] == "0") noReadyStatePros += "4,";
                if (noReadyDate[5] == "0") noReadyStatePros += "8,";
                if (noReadyStatePros != "") noReadyStatePros.Remove(noReadyStatePros.Length - 1, 1);
            }
            bo.noReadyStatePros = noReadyStatePros;     //noReadyStatePros    就绪状态未完成项目		1：SCR 2：POC 3：DOC 4：DPF5：催化器 6：氧传感器7：氧传感器加热器8：废气再循环(EGR)多选项使用“,”分割
            bo.mileage = ((DataRow)obj_obd)["MILXSLC"].ToString();              //mileage MIL灯点亮后行驶里程(km)    数值(4)   Km
            bo.operatorName = ((DataRow)obj_obd)["BY1"].ToString()==""?"为空": ((DataRow)obj_obd)["BY1"].ToString();         //operatorName    检测人姓名 字符(32)      是
            bo.teststime =DateTime.Parse( ((DataRow)obj_obd)["StartTime"].ToString()).ToString("yyyyMMddHHmmss"); //teststime   检测开始时间 时间  yyyyMMddHHmmss 是
            bo.testetime = DateTime.Parse(((DataRow)obj_obd)["EndTime"].ToString()).ToString("yyyyMMddHHmmss");//((DataRow)obj_obd)["EndTime"].ToString(); //testetime 检测结束时间  时间 yyyyMMddHHmmss  是
            bo.judge = ((DataRow)obj_obd)["PDJG"].ToString();                //judge   检测结果 字符(1)   1：合格 0：不合格 是
            bo.isRecheck = "0";            //isRecheck 是否需要复检  字符(1)   1：需要 0：不需要
            bo.reJudge = "1";              //reJudge 复检结果 字符(1)   1：合格 0：不合格

            body.Add(bo);
            dt.body = body;

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            FileOpreate.SaveLog( json, "[uploadInspResultOBD]：",3);

            return json;
        }

        //返回
        bool GetResultOBD(string webadd, string content, string model,out string obdresultid)
        {
            obdresultid = "";
            if (content == "")
            {
                MessageBox.Show("数据不能为空"); return false;
            }
            try
            {
                byte[] d = System.Text.Encoding.UTF8.GetBytes(content);
                System.Net.WebClient aaa = new System.Net.WebClient();
                aaa.Headers.Add("Content-Type", "application/json; charset=UTF-8");
                byte[] res;
                if (model == "GET")
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                else
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                String ReadData = System.Text.Encoding.UTF8.GetString(res);
                //textBoxResult.Text = ReadData;
                string jsonText = ReadData;
                FileOpreate.SaveLog(jsonText,"[uploadInspResultOBD_Ack]：",  3);
                Newtonsoft.Json.Linq.JObject JsonObj;
                JsonObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonText); //将json字符串转化为json对象 

                //读取json对象里的字符串对应值
                //Code = JsonObj["jkid"].ToString();
                //Status = JsonObj["jksqm"].ToString();

                //正则提取嵌套内容解析信息
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("\"obdresultid\":\"([^\"]*)\"");
                obdresultid = "" + reg.Match(jsonText).Groups[1].Value;
                if (obdresultid == "")
                {
                    FileOpreate.SaveLog("获取obdresultid为空", "[uploadInspResultOBD_Faile]：", 3);
                    return false;
                }
                //textBox1.Text = ModPublicJHHB.obdresultid;
                return true;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog( er.Message,"[uploadInspResultOBD_Eception]：", 3);
                Console.WriteLine("exception occured:" + er.Message);
                //labelStatus.Text = "exception occured:" + er.Message;
                //textBoxResult.Text = er.Message;
                return false;
            }
        }
        //返回

        public bool uploadOBDProcess(object obj_car, object obj_obd,string obdresultid)
        {
            string modeljson = createOBDProcess(obj_car, obj_obd,obdresultid);
            //textBoxJson.Text = modeljson;
            obdresultid = "";
            //GET数据 
            if (GetOBDProcess(ModPublicJHHB.webAddress, modeljson, "GET"))
            {
                FileOpreate.SaveLog( "成功","[uploadInspProcessOBD_Parser]：", 3);
                //labelStatus.Text = "POST success!";
                Console.WriteLine("POST success!");
                return true;
            }
            else
            {
                FileOpreate.SaveLog( "失败","[uploadInspProcessOBD_Parser]：", 3);
                //labelStatus.Text = "POST fail!";
                Console.WriteLine("POST fail!");
                return false;
            }
        }
        /// <summary>
        /// 生成OBD过程数据报文
        /// </summary>
        /// <param name="obj_car">已检车辆信息表</param>
        /// <param name="obj_obd">对应检测方法过程数据表</param>
        /// <param name="obdresultid">上传OBD检测结果返回的obdresultid</param>
        /// <returns></returns>
        public string createOBDProcess(object obj_car, object obj_obd,string obdresultid)
        {
            string inspregid = ((DataRow)obj_car)["JYLSH"].ToString();
            string jcff = ((DataRow)obj_car)["JCFF"].ToString();

            if (jcff == "ASM" || jcff == "SDS" || jcff == "VMAS")
            {
                ModPublicJHHB.uploadInspProcessOBDQY model = new ModPublicJHHB.uploadInspProcessOBDQY();
                model.jkid = "uploadInspProcessOBD";
                model.jksqm = ModPublicJHHB.jksqm;
                
                List<ModPublicJHHB.ProcessOBDDataItemQY> data = new List<ModPublicJHHB.ProcessOBDDataItemQY>();
                ModPublicJHHB.ProcessOBDDataItemQY dt = new ModPublicJHHB.ProcessOBDDataItemQY();
                data.Add(dt);
                model.data = data;

                List<ModPublicJHHB.ProcessOBDbodyQYItem> body = new List<ModPublicJHHB.ProcessOBDbodyQYItem>();
                string[] MMTIME = ((DataRow)obj_obd)["MMTIME"].ToString().Split(',');
                string[] MMOBDJQMJDKD = ((DataRow)obj_obd)["MMOBDJQMJDKD"].ToString().Split(',');
                string[] MMOBDJSFHZ = ((DataRow)obj_obd)["MMOBDJSFHZ"].ToString().Split(',');
                string[] MMOBDQYCGQXH = ((DataRow)obj_obd)["MMOBDQYCGQXH"].ToString().Split(',');
                string[] MMOBDLAMBDA = ((DataRow)obj_obd)["MMOBDLAMBDA"].ToString().Split(',');
                string[] MMOBDSPEED = ((DataRow)obj_obd)["MMOBDSPEED"].ToString().Split(',');
                string[] MMOBDROTATESPEED = ((DataRow)obj_obd)["MMOBDROTATESPEED"].ToString().Split(',');
                string[] MMOBDJQL = ((DataRow)obj_obd)["MMOBDJQL"].ToString().Split(',');
                string[] MMOBDJQYL = ((DataRow)obj_obd)["MMOBDJQYL"].ToString().Split(',');

                int processcount = MMOBDJQMJDKD.Count();
                for (int i = 0; i < processcount; i++)
                {
                    ModPublicJHHB.ProcessOBDbodyQYItem bo = new ModPublicJHHB.ProcessOBDbodyQYItem();
                    
                    bo.id = System.Guid.NewGuid().ToString().Replace("-", "");//id Odb检测过程记录id 字符(32)  检测站检测obd检测过程私有唯一标识 是
                    bo.inspregid = inspregid;//inspregid 检验登录ID  字符(32)  由环保局返回的检测登录唯一标识 是
                    bo.obdresultid = obdresultid;//obdresultid Odb检测结果id   字符(32)  由环保局返回的Odb检测结果唯一标识
                    bo.tsno = ModPublicJHHB.TsNo;//tsno    检测机构编号 字符(16)  格式详见3定义部分   是
                    bo.serialno = Convert.ToString(i);//serialno    采样时序    数值(4)   逐秒，从1开始，每条递增1   是
                    bo.optime = Convert.ToDateTime(MMTIME[i]).ToString("yyyyMMddHHmmss");//optime         采样时间    时间类型    yyyyMMddHHmmss
                    bo.throttleOpen = MMOBDJQMJDKD[i];//throttleOpen    节气门绝对开度 数值(12, 4)    汽油车（%）	
                    bo.loadingValue = MMOBDJSFHZ[i];//loadingValue    计算负荷值   数值(12, 4)    汽油车（%）	
                    bo.qyzhq = MMOBDQYCGQXH[i];//qyzhq   前氧传感器信号 数值(12, 4)	（mV / mA）	
                    bo.Lambda = MMOBDLAMBDA[i];//Lambda  过量空气系数  数值(12, 4)	（λ）	
                    bo.vehiclespeed = MMOBDSPEED[i];//vehiclespeed    车速  数值(12, 4)	（km / h）	
                    bo.enginespeed = MMOBDROTATESPEED[i];//enginespeed 发动机转速   数值(12, 4)	（r / min）	
                    bo.airin = MMOBDJQL[i];//airin   进气量 数值(12, 4)	（g / s）	
                    bo.pressure = MMOBDJQYL[i];//pressure    进气压力    数值(12, 4)	（kPa）	
                    body.Add(bo);
                }
                dt.body = body;
                //传入初始化好的对象
                string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
                FileOpreate.SaveLog(json,"[uploadInspProcessOBD]：",  3);

                return json;
            }
            else
            {
                ModPublicJHHB.uploadInspProcessOBDCY model = new ModPublicJHHB.uploadInspProcessOBDCY();
                model.jkid = "uploadInspProcessOBD";
                model.jksqm = ModPublicJHHB.jksqm;

                List<ModPublicJHHB.ProcessOBDDataItemCY> data = new List<ModPublicJHHB.ProcessOBDDataItemCY>();
                ModPublicJHHB.ProcessOBDDataItemCY dt = new ModPublicJHHB.ProcessOBDDataItemCY();
                data.Add(dt);
                model.data = data;

                List<ModPublicJHHB.ProcessOBDbodyCYItem> body = new List<ModPublicJHHB.ProcessOBDbodyCYItem>();
                string[] MMTIME = ((DataRow)obj_obd)["MMTIME"].ToString().Split(',');
                string[] MMOBDYMKD = ((DataRow)obj_obd)["MMOBDYMKD"].ToString().Split(',');
                string[] MMOBDSPEED = ((DataRow)obj_obd)["MMOBDSPEED"].ToString().Split(',');
                string[] MMOBDPOWER = ((DataRow)obj_obd)["MMOBDPOWER"].ToString().Split(',');
                string[] MMOBDROTATESPEED = ((DataRow)obj_obd)["MMOBDROTATESPEED"].ToString().Split(',');
                string[] MMOBDJQL = ((DataRow)obj_obd)["MMOBDJQL"].ToString().Split(',');
                string[] MMOBDZYYL = ((DataRow)obj_obd)["MMOBDZYYL"].ToString().Split(',');
                string[] MMOBDHYL = ((DataRow)obj_obd)["MMOBDHYL"].ToString().Split(',');
                string[] MMOBDNOND = ((DataRow)obj_obd)["MMOBDNOND"].ToString().Split(',');
                string[] MMOBDNSPSL = ((DataRow)obj_obd)["MMOBDNSPSL"].ToString().Split(',');
                string[] MMOBDWD = ((DataRow)obj_obd)["MMOBDWD"].ToString().Split(',');
                string[] MMOBDKLBJQYC = ((DataRow)obj_obd)["MMOBDKLBJQYC"].ToString().Split(',');
                string[] MMOBDEGRKD = ((DataRow)obj_obd)["MMOBDEGRKD"].ToString().Split(',');
                string[] MMOBDRYPSYL = ((DataRow)obj_obd)["MMOBDRYPSYL"].ToString().Split(',');
                

                int processcount = MMOBDYMKD.Count();
                for (int i = 0; i < processcount; i++)
                {
                    ModPublicJHHB.ProcessOBDbodyCYItem bo = new ModPublicJHHB.ProcessOBDbodyCYItem();
                    bo.id = System.Guid.NewGuid().ToString().Replace("-", "");//id Odb检测过程记录id 字符(32)  检测站检测obd检测过程私有唯一标识 是
                    bo.inspregid = inspregid;//inspregid 检验登录ID  字符(32)  由环保局返回的检测登录唯一标识 是
                    bo.obdresultid = obdresultid;//obdresultid Odb检测结果id   字符(32)  由环保局返回的Odb检测结果唯一标识
                    bo.tsno = ModPublicJHHB.TsNo;//tsno    检测机构编号 字符(16)  格式详见3定义部分   是
                    bo.serialno = Convert.ToString(i);//serialno    采样时序    数值(4)   逐秒，从1开始，每条递增1   是
                    bo.optime =Convert.ToDateTime(MMTIME[i]).ToString("yyyyMMddHHmmss");//optime         采样时间    时间类型    yyyyMMddHHmmss
                    bo.throttleOpen = MMOBDYMKD[i];//throttleOpen    油门开度    数值(12, 4)	（%）	
                    bo.vehiclespeed = MMOBDSPEED[i];//vehiclespeed    车速  数值(12, 4)	（km / h）	
                    bo.enginepower = MMOBDPOWER[i];//enginepower 发动机输出功率 数值(12, 4)	（kw）	
                    bo.enginespeed = MMOBDROTATESPEED[i];//enginespeed 发动机转速   数值(4)	（r / min）	
                    bo.airin = MMOBDJQL[i];//airin   进气量 数值(12, 4)	（g / s）	
                    bo.pressure = MMOBDZYYL[i];//pressure    增压压力    数值(12, 4)	（kPa）	
                    bo.oilConsumption = MMOBDHYL[i];//oilConsumption  耗油量 数值(12, 4)	（L / 100km）	
                    bo.NOx = MMOBDNOND[i];//NOx 氮氧传感器浓度 数值(12, 4)	（ppm）	
                    bo.urea = MMOBDNSPSL[i];//urea    尿素喷射量   数值(12, 4)	（L / h）	
                    bo.DischargeTemperature = MMOBDWD[i];//DischargeTemperature    排气温度    数值(12, 4)(℃)
                    bo.klPressure = MMOBDKLBJQYC[i];//klPressure  颗粒捕集器压差 数值(12, 4)	（kpa）	
                    bo.ECR = MMOBDEGRKD[i];//ECR EGR 开度  数值(12, 4)	（%）	
                    bo.FuelInPressure = MMOBDRYPSYL[i];//FuelInPressure  燃油喷射压力  数值(12, 4)	（bar）	
                    body.Add(bo);
                }
                dt.body = body;
                //传入初始化好的对象
                string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
                FileOpreate.SaveLog(json, "[uploadInspProcessOBD]：", 3);

                return json;
            }

        }

        //返回
        bool GetOBDProcess(string webadd, string content, string model)
        {
            if (content == "")
            {
                MessageBox.Show("数据不能为空"); return false;
            }
            try
            {
                byte[] d = System.Text.Encoding.UTF8.GetBytes(content);
                System.Net.WebClient aaa = new System.Net.WebClient();
                aaa.Headers.Add("Content-Type", "application/json; charset=UTF-8");
                byte[] res;
                if (model == "GET")
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                else
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                String ReadData = System.Text.Encoding.UTF8.GetString(res);
                string jsonText = ReadData;
                FileOpreate.SaveLog(jsonText,"[uploadInspProcessOBD_Ack]：",  3);
                ModPublicJHHB.uploadAck ack = JsonConvert.DeserializeObject<ModPublicJHHB.uploadAck>(jsonText);
                if (ack.result[0].code != "1" && ack.result[0].code != "2")
                {
                    //MessageBox.Show("Code:" + ack.result[0].code + "\r\n;Message:" + ack.result[0].message);
                    return false;
                }
                return true;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog(er.Message,"[uploadInspProcessOBD_Eception]：",  3);
                Console.WriteLine("exception occured:" + er.Message);
                return false;
            }
        }
        //返回
        /// <summary>
        /// 上传OBD IUPR信息
        /// </summary>
        /// <param name="obj_car">已检车辆信息</param>
        /// <param name="obj_obd">OBD结果信息表</param>
        /// <param name="obdresultid">obdresultid</param>
        /// <returns></returns>

        public bool uploadOBDIUPR(object obj_car, object obj_obd, string obdresultid)
        {
            string modeljson = createOBDIUPR(obj_car, obj_obd, obdresultid);
            //textBoxJson.Text = modeljson;
            obdresultid = "";
            //GET数据 
            if (GetIuprOBD(ModPublicJHHB.webAddress, modeljson, "GET"))
            {
                FileOpreate.SaveLog("成功","[uploadInspIuprOBD_Parser]：",  3);
                //labelStatus.Text = "POST success!";
                Console.WriteLine("POST success!");
                return true;
            }
            else
            {
                FileOpreate.SaveLog("失败","[uploadInspIuprOBD_Parser]：",  3);
                //labelStatus.Text = "POST fail!";
                Console.WriteLine("POST fail!");
                return false;
            }
        }
        /// <summary>
        /// 生成OBD过程数据报文
        /// </summary>
        /// <param name="obj_car">已检车辆信息表</param>
        /// <param name="obj_obd">对应检测方法过程数据表</param>
        /// <param name="obdresultid">上传OBD检测结果返回的obdresultid</param>
        /// <returns></returns>
        public string createOBDIUPR(object obj_car, object obj_obd, string obdresultid)
        {
            string inspregid = ((DataRow)obj_car)["JYLSH"].ToString();
            string jcff = ((DataRow)obj_car)["JCFF"].ToString();
            ModPublicJHHB.uploadInspIuprOBD model = new ModPublicJHHB.uploadInspIuprOBD();
            model.jkid = "uploadInspIuprOBD";
            model.jksqm = ModPublicJHHB.jksqm;


            List<ModPublicJHHB.IuprOBDDataItem> data = new List<ModPublicJHHB.IuprOBDDataItem>();
            ModPublicJHHB.IuprOBDDataItem dt = new ModPublicJHHB.IuprOBDDataItem();
            data.Add(dt);
            model.data = data;

            List<ModPublicJHHB.IuprOBDbodyItem> body = new List<ModPublicJHHB.IuprOBDbodyItem>();
            if (jcff == "ASM" || jcff == "SDS" || jcff == "VMAS")
            {
                ModPublicJHHB.IuprOBDbodyItem[] bo = new ModPublicJHHB.IuprOBDbodyItem[9];
                string[] IUPRNAME = { "催化器组1", "催化器组2", "氧传感器组1", "氧传感器组2", "EVAP", "EGR和VVT", "GPF组1", "GPF组2", "二次空气喷射系统" };
                for (int i = 0; i < 9; i++)
                {
                    int j = i + 1;
                    bo[i].id = System.Guid.NewGuid().ToString().Replace("-", "");//id Odb检测过程记录id 字符(32)  检测线检测obd检测过程私有唯一标识 是
                    bo[i].inspregid = inspregid;//inspregid 检验登录ID  字符(32)  由环保局返回的检测登录唯一标识 是
                    bo[i].obdresultid = obdresultid;//obdresultid Odb检测结果id   字符(32)  由环保局返回的Odb检测结果唯一标识 是
                    bo[i].tsno = ModPublicJHHB.TsNo;//tsno 检测机构编号  字符(16)  格式详见3定义部分 是
                    bo[i].projectcode = "项目"+j.ToString();//projectcode 监测项目代码  字符(32)      是
                    bo[i].projectname = IUPRNAME[i];//projectname 监测项目名称 字符(32)      是
                    string denominator = ((DataRow)obj_obd)["IUPR" + j.ToString()].ToString().Split('_')[0];
                    string molecule = ((DataRow)obj_obd)["IUPR" + j.ToString()].ToString().Split('_')[2];
                    string iuprrate = ((DataRow)obj_obd)["IUPR" + j.ToString()].ToString().Split('_')[4];
                    bo[i].denominator = (denominator == "" ? "0" : denominator);// ((DataRow)obj_obd)["IUPR"+j.ToString()].ToString().Split('_')[0];//denominator 监测完成次数  数字（4）		是
                    bo[i].molecule = (molecule == "" ? "0" : molecule);//((DataRow)obj_obd)["IUPR" + j.ToString()].ToString().Split('_')[2];//molecule    符合监测条件次数    数字（4）		是
                    bo[i].iuprrate =( iuprrate == "" ? "0" : iuprrate);// ((DataRow)obj_obd)["IUPR" + j.ToString()].ToString().Split('_')[4];//iuprrate    IUPR率   数字（12, 4）	（%）	是
                    body.Add(bo[i]);
                }
            }
            else
            {
                ModPublicJHHB.IuprOBDbodyItem[] bo = new ModPublicJHHB.IuprOBDbodyItem[7];
                string[] IUPRNAME = { "NMHC催化器监测", "NOx催化器监测", "NOx吸附器监测", "PM捕集器监测", "废气传感器监测", "EGR和VVT监测", "增压压力监测"};
                for (int i = 0; i < 7; i++)
                {
                    int j = i + 1;
                    bo[i].id = System.Guid.NewGuid().ToString().Replace("-", "");//id Odb检测过程记录id 字符(32)  检测线检测obd检测过程私有唯一标识 是
                    bo[i].inspregid = inspregid;//inspregid 检验登录ID  字符(32)  由环保局返回的检测登录唯一标识 是
                    bo[i].obdresultid = obdresultid;//obdresultid Odb检测结果id   字符(32)  由环保局返回的Odb检测结果唯一标识 是
                    bo[i].tsno = ModPublicJHHB.TsNo;//tsno 检测机构编号  字符(16)  格式详见3定义部分 是
                    bo[i].projectcode = "项目" + j.ToString();//projectcode 监测项目代码  字符(32)      是
                    bo[i].projectname = IUPRNAME[i];//projectname 监测项目名称 字符(32)      是
                    string denominator = ((DataRow)obj_obd)["IUPR" + j.ToString()].ToString().Split('_')[0];
                    string molecule = ((DataRow)obj_obd)["IUPR" + j.ToString()].ToString().Split('_')[2];
                    string iuprrate = ((DataRow)obj_obd)["IUPR" + j.ToString()].ToString().Split('_')[4];
                    bo[i].denominator = (denominator == "" ? "0" : denominator);// ((DataRow)obj_obd)["IUPR"+j.ToString()].ToString().Split('_')[0];//denominator 监测完成次数  数字（4）		是
                    bo[i].molecule = (molecule == "" ? "0" : molecule);//((DataRow)obj_obd)["IUPR" + j.ToString()].ToString().Split('_')[2];//molecule    符合监测条件次数    数字（4）		是
                    bo[i].iuprrate = (iuprrate == "" ? "0" : iuprrate);// ((DataRow)obj_obd)["IUPR" + j.ToString()].ToString().Split('_')[4];//iuprrate    IUPR率   数字（12, 4）	（%）	是
                    body.Add(bo[i]);
                }

            }
            dt.body = body;

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);
            FileOpreate.SaveLog( json, "[uploadInspIuprOBD]：",3);

            return json;

        }

        //返回
        bool GetIuprOBD(string webadd, string content, string model)
        {
            if (content == "")
            {
                MessageBox.Show("数据不能为空"); return false;
            }
            try
            {
                byte[] d = System.Text.Encoding.UTF8.GetBytes(content);
                System.Net.WebClient aaa = new System.Net.WebClient();
                aaa.Headers.Add("Content-Type", "application/json; charset=UTF-8");
                byte[] res;
                if (model == "GET")
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                else
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                String ReadData = System.Text.Encoding.UTF8.GetString(res);
                //textBoxResult.Text = ReadData;
                string jsonText = ReadData;
                FileOpreate.SaveLog(jsonText,"[uploadInspIuprOBD_Ack]：",  3);
                ModPublicJHHB.uploadAck ack = JsonConvert.DeserializeObject<ModPublicJHHB.uploadAck>(jsonText);
                if (ack.result[0].code != "1" && ack.result[0].code != "2")
                {
                    //MessageBox.Show("Code:" + ack.result[0].code + "\r\n;Message:" + ack.result[0].message);
                    return false;
                }
                return true;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog( er.Message,"[uploadInspIuprOBD_Eception]：", 3);
                Console.WriteLine("exception occured:" + er.Message);
                return false;
            }
        }

        public string createResultLIGHTPROOF(object obj_car,object obj_result)
        {

            string inspregid = ((DataRow)obj_car)["JYLSH"].ToString();
            ModPublicJHHB.uploadInspResultLIGHTPROOF model = new ModPublicJHHB.uploadInspResultLIGHTPROOF();
            model.jkid = "uploadInspResultLIGHTPROOF";
            model.jksqm = ModPublicJHHB.jksqm;

            List<ModPublicJHHB.ResultLIGHTPROOFDataItem> data = new List<ModPublicJHHB.ResultLIGHTPROOFDataItem>();
            ModPublicJHHB.ResultLIGHTPROOFDataItem dt = new ModPublicJHHB.ResultLIGHTPROOFDataItem();
            data.Add(dt);
            model.data = data;

            List<ModPublicJHHB.ResultLIGHTPROOFbodyItem> body = new List<ModPublicJHHB.ResultLIGHTPROOFbodyItem>();
            ModPublicJHHB.ResultLIGHTPROOFbodyItem bo = new ModPublicJHHB.ResultLIGHTPROOFbodyItem();

            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.inspregid = inspregid;  //inspregid 检验登录ID  字符(32)  由环保局返回的检测登录唯一标识 是
            bo.License = ((DataRow)obj_car)["CLHP"].ToString();  //License 号牌号码    字符(10)  格式详见3定义部分 FALSE
            bo.LicenseType = DicCpysR.GetValue(((DataRow)obj_car)["CPYS"].ToString(), "");   //LicenseType 车牌颜色    数值(1)   详见数据字典8.2序11 FALSE
            bo.LicenseCode = DicHpzlR.GetValue(((DataRow)obj_car)["HPZL"].ToString(), "");  //LicenseCode 号牌种类(GA)字符(2)   详见数据字典8.1序1 FALSE
            bo.VIN = ((DataRow)obj_car)["CLSBM"].ToString();  //VIN 车辆识别代号  字符(20)  行驶证上的车辆识别代号VIN（须完整上报）	是
            bo.EngineNo = ((DataRow)obj_car)["FDJHM"].ToString();  //EngineNo    发动机号    字符(20)      是
            bo.TsNo = ModPublicJHHB.TsNo;  //TsNo    检测机构编号  字符(16)  格式详见3定义部分   是
            bo.FuelType = DicRlzlR.GetValue(((DataRow)obj_car)["RLZL"].ToString(), "");  //FuelType    燃料种类(GA)    字符(4)   详见数据字典8.1序6 是
            bo.TestLineNo = ((DataRow)obj_car)["LINEID"].ToString();  //TestLineNo  检测线代码   字符(8)   格式详见3定义部分   是
            bo.OperatorName = ((DataRow)obj_car)["CZY"].ToString();  //OperatorName    操作员 字符(32)      是
            bo.DriverName = ((DataRow)obj_car)["JSY"].ToString();  //DriverName  引车员 字符(32)      是
            bo.Teststime =Convert.ToDateTime( ((DataRow)obj_result)["JCKSSJ"].ToString()).ToString("yyyyMMddHHmmss");  //Teststime   检测开始时间  日期格式    YYYYMMDDHHmmss  是
            bo.Testetime = Convert.ToDateTime(((DataRow)obj_result)["JCJSSJ"].ToString()).ToString("yyyyMMddHHmmss");  //Testetime   检测结束时间  日期格式    YYYYMMDDHHmmss  是
            bo.TestType = DicJcffR.GetValue(((DataRow)obj_car)["JCFF"].ToString(), "");  //TestType    检测方法    字符(1)   详见数据字典8.2序4 是
            bo.TestJudge = DicResultR.GetValue(((DataRow)obj_car)["JCJG"].ToString(), "");  //TestJudge   检测结果    字符(1)   1:合格0：不合格，上报值为（0或1）	是
            bo.Samplingdepth = "400";  //Samplingdepth   采样深度 数值(4)       是
            bo.Temperature = ((DataRow)obj_result)["WD"].ToString();  //Temperature 温度  数值(12, 4)	（Co）	是
            bo.Humidity = ((DataRow)obj_result)["SD"].ToString();  //Humidity    湿度  数值(12, 4)	（%）	是
            bo.Atpressure = ((DataRow)obj_result)["DQY"].ToString();  //Atpressure  气压  数值(12, 4)	（kPa）	是
            bo.IdleRev = ((DataRow)obj_result)["DSZS"].ToString();  //IdleRev 实测转速    数值(8)	（r / min）	是
            bo.RPM = ((DataRow)obj_car)["EDZS"].ToString();  //RPM 额定转速    数值(8)	（r / min）	是
            bo.SmokeK1 = ((DataRow)obj_result)["FOURTHDATA"].ToString();  //SmokeK1 光吸收系数1  数值(12, 4)	（m - 1），倒数第一次 是
            bo.SmokeK2 = ((DataRow)obj_result)["FIRSTDATA"].ToString();  //SmokeK2 光吸收系数2  数值(12, 4)	（m - 1），倒数第二次 是
            bo.SmokeK3 = ((DataRow)obj_result)["SECONDDATA"].ToString();  //SmokeK3 光吸收系数3  数值(12, 4)	（m - 1），倒数第三次 是
            bo.SmokeK4 = ((DataRow)obj_result)["THIRDDATA"].ToString();  //SmokeK4 光吸收系数4  数值(12, 4)    预留  FALSE
            bo.SmokeAvg = ((DataRow)obj_result)["AVERAGEDATA"].ToString();  //SmokeAvg    排放结果平均值 数值(12, 4)	（m - 1）	是
            bo.SmokeKLimit = ((DataRow)obj_result)["YDXZ"].ToString();  //SmokeKLimit 排放限值    数值(12, 4)	（m - 1）	是
            bo.SmokeKJudge = ((DataRow)obj_result)["ZHPD"].ToString()=="不合格"?"0":"1"; //DicResultR.GetValue(((DataRow)obj_car)["ZHPD"].ToString(), "");  //SmokeKJudge 排放判定    字符(1)   0 - 不合格，1 - 合格  是

            body.Add(bo);
            dt.body = body;

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            FileOpreate.SaveLog( json, "[uploadInspResultLIGHTPROOF]：",3);

            return json;
        }

        // 7.2.2.1.3 加载减速法检测结果数据报文生成
        public string createResultLUGDOWN(object obj_car, object obj_result)
        {

            string inspregid = ((DataRow)obj_car)["JYLSH"].ToString();

            ModPublicJHHB.uploadInspResultLUGDOWN model = new ModPublicJHHB.uploadInspResultLUGDOWN();
            model.jkid = "uploadInspResultLUGDOWN";
            model.jksqm = ModPublicJHHB.jksqm;

            List<ModPublicJHHB.ResultLUGDOWNDataItem> data = new List<ModPublicJHHB.ResultLUGDOWNDataItem>();
            ModPublicJHHB.ResultLUGDOWNDataItem dt = new ModPublicJHHB.ResultLUGDOWNDataItem();
            data.Add(dt);
            model.data = data;

            List<ModPublicJHHB.ResultLUGDOWNbodyItem> body = new List<ModPublicJHHB.ResultLUGDOWNbodyItem>();
            ModPublicJHHB.ResultLUGDOWNbodyItem bo = new ModPublicJHHB.ResultLUGDOWNbodyItem();

            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //Id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.inspregid = inspregid;  //inspregid 检验登录ID  字符(32)  由环保局返回的检测登录唯一标识 是
            bo.License = ((DataRow)obj_car)["CLHP"].ToString();  //License 号牌号码    字符(10)  格式详见3定义部分 FALSE
            bo.LicenseType = DicCpysR.GetValue(((DataRow)obj_car)["CPYS"].ToString(), "");   //LicenseType 车牌颜色    数值(1)   详见数据字典8.2序11 FALSE
            bo.LicenseCode = DicHpzlR.GetValue(((DataRow)obj_car)["HPZL"].ToString(), "");  //LicenseCode 号牌种类(GA)字符(2)   详见数据字典8.1序1 FALSE
            bo.VIN = ((DataRow)obj_car)["CLSBM"].ToString();  //VIN 车辆识别代号  字符(20)  行驶证上的车辆识别代号VIN（须完整上报）	是
            bo.EngineNo = ((DataRow)obj_car)["FDJHM"].ToString();  //EngineNo    发动机号    字符(20)      是
            bo.TsNo = ModPublicJHHB.TsNo;  //TsNo    检测机构编号  字符(16)  格式详见3定义部分   是
            bo.FuelType = DicRlzlR.GetValue(((DataRow)obj_car)["RLZL"].ToString(), "");  //FuelType    燃料种类(GA)    字符(4)   详见数据字典8.1序6 是
            bo.TestLineNo = ((DataRow)obj_car)["LINEID"].ToString();  //TestLineNo  检测线代码   字符(8)   格式详见3定义部分   是
            bo.OperatorName = ((DataRow)obj_car)["CZY"].ToString();  //OperatorName    操作员 字符(32)      是
            bo.DriverName = ((DataRow)obj_car)["JSY"].ToString();  //DriverName  引车员 字符(32)      是
            bo.Teststime = Convert.ToDateTime(((DataRow)obj_result)["JCKSSJ"].ToString()).ToString("yyyyMMddHHmmss");  //Teststime   检测开始时间  日期格式    YYYYMMDDHHmmss  是
            bo.Testetime = Convert.ToDateTime(((DataRow)obj_result)["JCJSSJ"].ToString()).ToString("yyyyMMddHHmmss");  //Testetime   检测结束时间  日期格式    YYYYMMDDHHmmss  是
            bo.TestType = DicJcffR.GetValue(((DataRow)obj_car)["JCFF"].ToString(), "");  //TestType    检测方法    字符(1)   详见数据字典8.2序4 是
            bo.TestJudge = DicResultR.GetValue(((DataRow)obj_car)["JCJG"].ToString(), "");  //TestJudge   检测结果    字符(1)   1:合格0：不合格，上报值为（0或1）	是
            bo.Samplingdepth = "400";  //Samplingdepth   采样深度 数值(4)       是
            bo.Temperature = ((DataRow)obj_result)["WD"].ToString();  //Temperature 温度  数值(12, 4)	（Co）	是
            bo.Humidity = ((DataRow)obj_result)["SD"].ToString();  //Humidity    湿度  数值(12, 4)	（%）	是
            bo.Atpressure = ((DataRow)obj_result)["DQY"].ToString();  //Atpressure  气压  数值(12, 4)	（kPa）	是
            bo.RPM = ((DataRow)obj_car)["EDZS"].ToString();  //RPM 发动机额定转速 数值(8)	（r / min）	是
            bo.MaxRPM = ((DataRow)obj_result)["VELMAXHPZS"].ToString();  //MaxRPM  发动机最大转速 数值(8)	（r / min）	是
            bo.CorVelMaxhp = ((DataRow)obj_result)["VELMAXHP"].ToString();  //CorVelMaxhp 修正MAXHP时滚筒线速度   数值(8)	（r / min）	是
            bo.ActVelMaxhp = ((DataRow)obj_result)["REALVELMAXHP"].ToString();  //ActVelMaxhp 实际MAXHP时滚筒线速度   数值(8)	（r / min）	是
            bo.CorMaxhp = ((DataRow)obj_result)["MAXLBGL"].ToString();  //CorMaxhp    修正最大轮边功率    数值(12, 4)	（KW）	是
            bo.ActMaxhp = ((DataRow)obj_result)["ACTMAXHP"].ToString();  //ActMaxhp    实测最大轮边功率    数值(12, 4)	（KW）	是
            bo.Minhp = ((DataRow)obj_result)["GLXZ"].ToString();  //Minhp   所需最小轮边功率    数值(12, 4)	（KW）	是
            bo.K100 = ((DataRow)obj_result)["HK"].ToString();  //K100    100 % 光吸收系数   数值(12, 4)	（m - 1）	是
            bo.K80 = ((DataRow)obj_result)["EK"].ToString();  //K80 80 % 光吸收系数    数值(12, 4)	（m - 1）	是
            bo.NOx80 = ((DataRow)obj_result)["ENO"].ToString();  //NOx80   80 % NOX浓度    数值(12, 4)	（10 - 6）	是
            bo.SmokeK100Limit = ((DataRow)obj_result)["YDXZ"].ToString();  //SmokeK100Limit  排放限值    数值(12, 4)	（m - 1）	是
            bo.SmokeK80Limit = ((DataRow)obj_result)["YDXZ"].ToString();  //SmokeK80Limit   排放限值    数值(12, 4)	（m - 1）	是
            bo.NOx80Limit = ((DataRow)obj_result)["ENOXZ"].ToString();  //NOx80Limit  80 % NOX排放限值  数值(12, 4)	（10 - 6）	是
            bo.MaxhpLimit = ((DataRow)obj_result)["GLXZ"].ToString();  //MaxhpLimit  最大轮边功率限值    数值(12, 4)	（KW）	是
            bo.MaxRPMJudge = DicResultR.GetValue(((DataRow)obj_result)["ZSPD"].ToString(), "");  //MaxRPMJudge 最大转速判定  字符(1)   0 - 不合格，1 - 合格  是
            bo.SmokeK100Judge = DicResultR.GetValue(((DataRow)obj_result)["HKPD"].ToString(), "");  //SmokeK100Judge  排放判定    字符(1)   0 - 不合格，1 - 合格  是
            bo.SmokeK80Judge = DicResultR.GetValue(((DataRow)obj_result)["EKPD"].ToString(), "");  //SmokeK80Judge   排放判定    字符(1)   0 - 不合格，1 - 合格  是
            bo.NOx80Judge = DicResultR.GetValue(((DataRow)obj_result)["ENOPD"].ToString(), "");  //NOx80Judge  80 % NOX排放判定  字符(1)   0 - 不合格，1 - 合格  是
            bo.MaxhpJudge = DicResultR.GetValue(((DataRow)obj_result)["GLPD"].ToString(), "");  //MaxhpJudge  最大轮边功率判定    字符(1)   0 - 不合格，1 - 合格  是
            bo.pcf = ((DataRow)obj_result)["GLXZXS"].ToString();  //pcf 功率修正系数  数值(12, 4)        是
            bo.RateRevUp = ((DataRow)obj_result)["RATEREVUP"].ToString();  //RateRevUp   发动机额定转速上限   数值(8)	（r / min）	FALSE
            bo.RateRevDown = ((DataRow)obj_result)["RATEREVDOWN"].ToString();  //RateRevDown 发动机额定转速下限   数值(8)	（r / min）	FALSE

            body.Add(bo);
            dt.body = body;

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            FileOpreate.SaveLog( json,"[uploadInspResultLUGDOWN]：", 3);

            return json;
        }

        // 7.2.2.1.4 双怠速法检测结果数据报文生成
        public string createResultDIDLE(object obj_car, object obj_result)
        {

            string inspregid = ((DataRow)obj_car)["JYLSH"].ToString();

            ModPublicJHHB.uploadInspResultDIDLE model = new ModPublicJHHB.uploadInspResultDIDLE();
            model.jkid = "uploadInspResultDIDLE";
            model.jksqm = ModPublicJHHB.jksqm;

            List<ModPublicJHHB.ResultDIDLEDataItem> data = new List<ModPublicJHHB.ResultDIDLEDataItem>();
            ModPublicJHHB.ResultDIDLEDataItem dt = new ModPublicJHHB.ResultDIDLEDataItem();
            data.Add(dt);
            model.data = data;

            List<ModPublicJHHB.ResultDIDLEbodyItem> body = new List<ModPublicJHHB.ResultDIDLEbodyItem>();
            ModPublicJHHB.ResultDIDLEbodyItem bo = new ModPublicJHHB.ResultDIDLEbodyItem();

            bo.Id = System.Guid.NewGuid().ToString().Replace("-", "");  //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.inspregid = inspregid;  //inspregid 检验登录ID  字符(32)  由环保局返回的检测登录唯一标识 是
            bo.License = ((DataRow)obj_car)["CLHP"].ToString();  //License 号牌号码    字符(10)  格式详见3定义部分 FALSE
            bo.LicenseType = DicCpysR.GetValue(((DataRow)obj_car)["CPYS"].ToString(), "");   //LicenseType 车牌颜色    数值(1)   详见数据字典8.2序11 FALSE
            bo.LicenseCode = DicHpzlR.GetValue(((DataRow)obj_car)["HPZL"].ToString(), "");  //LicenseCode 号牌种类(GA)字符(2)   详见数据字典8.1序1 FALSE
            bo.VIN = ((DataRow)obj_car)["CLSBM"].ToString();  //VIN 车辆识别代号  字符(20)  行驶证上的车辆识别代号VIN（须完整上报）	是
            bo.EngineNo = ((DataRow)obj_car)["FDJHM"].ToString();  //EngineNo    发动机号    字符(20)      是
            bo.TsNo = ModPublicJHHB.TsNo;  //TsNo    检测机构编号  字符(16)  格式详见3定义部分   是
            bo.FuelType =DicRlzlR.GetValue( ((DataRow)obj_car)["RLZL"].ToString(),"");  //FuelType    燃料种类(GA)    字符(4)   详见数据字典8.1序6 是
            bo.TestLineNo = ((DataRow)obj_car)["LINEID"].ToString();  //TestLineNo  检测线代码   字符(8)   格式详见3定义部分   是
            bo.OperatorName = ((DataRow)obj_car)["CZY"].ToString();  //OperatorName    操作员 字符(32)      是
            bo.DriverName = ((DataRow)obj_car)["JSY"].ToString();  //DriverName  引车员 字符(32)      是
            bo.Teststime = Convert.ToDateTime(((DataRow)obj_result)["JCKSSJ"].ToString()).ToString("yyyyMMddHHmmss");  //Teststime   检测开始时间  日期格式    YYYYMMDDHHmmss  是
            bo.Testetime = Convert.ToDateTime(((DataRow)obj_result)["JCJSSJ"].ToString()).ToString("yyyyMMddHHmmss");  //Testetime   检测结束时间  日期格式    YYYYMMDDHHmmss  是
            bo.TestType = DicJcffR.GetValue(((DataRow)obj_car)["JCFF"].ToString(), "");  //TestType    检测方法    字符(1)   详见数据字典8.2序4 是
            bo.TestJudge = DicResultR.GetValue(((DataRow)obj_car)["JCJG"].ToString(), "");  //TestJudge   检测结果    字符(1)   1:合格0：不合格，上报值为（0或1）	是
            bo.Samplingdepth = "400";  //Samplingdepth   采样深度 数值(4)       是
            bo.Temperature = ((DataRow)obj_result)["WD"].ToString();  //Temperature 温度  数值(12, 4)	（Co）	是
            bo.Humidity = ((DataRow)obj_result)["SD"].ToString();  //Humidity    湿度  数值(12, 4)	（%）	是
            bo.Atpressure = ((DataRow)obj_result)["DQY"].ToString();  //Atpressure  气压  数值(12, 4)	（kPa）	是

            bo.Gdszs = ((DataRow)obj_result)["ZSHIGH"].ToString();  //Gdszs   高怠速转速值  数值(8)	（r / min）	是
            bo.Dszs = ((DataRow)obj_result)["ZSLOW"].ToString();  //Dszs    低怠速转速值  数值(8)	（r / min）	是
            bo.Lambda = ((DataRow)obj_result)["LAMDAHIGHCLZ"].ToString();  //Lambda  过量空气系数结果    数值(12, 4)        是
            bo.LSCOResult = ((DataRow)obj_result)["COLOWCLZ"].ToString();  //LSCOResult  低怠速CO结果 数值(12, 4)	（%）	是
            bo.LSHCResult = ((DataRow)obj_result)["HCLOWCLZ"].ToString();  //LSHCResult  低怠速HC结果 数值(12, 4)	（10 - 6）	是
            bo.HSCOResult = ((DataRow)obj_result)["COHIGHCLZ"].ToString();  //HSCOResult  高怠速CO结果 数值(12, 4)	（%）	是
            bo.HSHCResult = ((DataRow)obj_result)["HCHIGHCLZ"].ToString();  //HSHCResult  高怠速HC结果 数值(12, 4)	（10 - 6）	是
            bo.LambdaUp = "1.05";  //LambdaUp    过量空气系数限值上限  数值(12, 4)        是
            bo.LambdaDown = "0.95";  //LambdaDown  过量空气系数限值下限  数值(12, 4)        是
            bo.LSCOLimit = ((DataRow)obj_result)["COLOWXZ"].ToString();  //LSCOLimit   低怠速CO限值 数值(12, 4)	（%）	是
            bo.LSHCLimit = ((DataRow)obj_result)["HCLOWXZ"].ToString();  //LSHCLimit   低怠速HC限值 数值(12, 4)	（10 - 6）	是
            bo.HSCOLimit = ((DataRow)obj_result)["COHIGHXZ"].ToString();  //HSCOLimit   高怠速CO限值 数值(12, 4)	（%）	是
            bo.HSHCLimit = ((DataRow)obj_result)["HCHIGHXZ"].ToString();  //HSHCLimit   高怠速HC限值 数值(12, 4)	（10 - 6）	是
            bo.LambdaJudge = DicResultR.GetValue(((DataRow)obj_result)["LAMDAHIGHPD"].ToString(), "");  //LambdaJudge 过量空气系数判定    字符(1)   0 - 不合格，1 - 合格  是
            bo.LSCOJudge = DicResultR.GetValue(((DataRow)obj_result)["COLOWPD"].ToString(), "");  //LSCOJudge   低怠速CO判定 字符(1)   0 - 不合格，1 - 合格  是
            bo.LSHCJudge = DicResultR.GetValue(((DataRow)obj_result)["HCLOWPD"].ToString(), "");  //LSHCJudge   低怠速HC判定 字符(1)   0 - 不合格，1 - 合格  是
            bo.HSCOJudge = DicResultR.GetValue(((DataRow)obj_result)["COHIGHPD"].ToString(), "");  //HSCOJudge   高怠速CO判定 字符(1)   0 - 不合格，1 - 合格  是
            bo.HSHCJudge = DicResultR.GetValue(((DataRow)obj_result)["HCHIGHPD"].ToString(), "");  //HSHCJudge   高怠速HC判定 字符(1)   0 - 不合格，1 - 合格  是

            body.Add(bo);
            dt.body = body;

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            FileOpreate.SaveLog(json,"[uploadInspResultDIDLE]：",  3);

            return json;
        }


        // 7.2.2.1.4 简易瞬态工况法检测结果数据报文生成
        public string createResultVMAS(object obj_car, object obj_result)
        {

            string inspregid = ((DataRow)obj_car)["JYLSH"].ToString();

            ModPublicJHHB.uploadInspResultVMAS model = new ModPublicJHHB.uploadInspResultVMAS();
            model.jkid = "uploadInspResultVMAS";
            model.jksqm = ModPublicJHHB.jksqm;

            List<ModPublicJHHB.ResultVMASDataItem> data = new List<ModPublicJHHB.ResultVMASDataItem>();
            ModPublicJHHB.ResultVMASDataItem dt = new ModPublicJHHB.ResultVMASDataItem();
            data.Add(dt);
            model.data = data;

            List<ModPublicJHHB.ResultVMASbodyItem> body = new List<ModPublicJHHB.ResultVMASbodyItem>();
            ModPublicJHHB.ResultVMASbodyItem bo = new ModPublicJHHB.ResultVMASbodyItem();

            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //Id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.inspregid = inspregid;  //inspregid 检验登录ID  字符(32)  由环保局返回的检测登录唯一标识 是
            bo.License = ((DataRow)obj_car)["CLHP"].ToString();  //License 号牌号码    字符(10)  格式详见3定义部分 FALSE
            bo.LicenseType = DicCpysR.GetValue(((DataRow)obj_car)["CPYS"].ToString(), "");   //LicenseType 车牌颜色    数值(1)   详见数据字典8.2序11 FALSE
            bo.LicenseCode = DicHpzlR.GetValue(((DataRow)obj_car)["HPZL"].ToString(), "");  //LicenseCode 号牌种类(GA)字符(2)   详见数据字典8.1序1 FALSE
            bo.VIN = ((DataRow)obj_car)["CLSBM"].ToString();  //VIN 车辆识别代号  字符(20)  行驶证上的车辆识别代号VIN（须完整上报）	是
            bo.EngineNo = ((DataRow)obj_car)["FDJHM"].ToString();  //EngineNo    发动机号    字符(20)      是
            bo.TsNo = ModPublicJHHB.TsNo;  //TsNo    检测机构编号  字符(16)  格式详见3定义部分   是
            bo.FuelType = DicRlzlR.GetValue(((DataRow)obj_car)["RLZL"].ToString(), "");   //FuelType    燃料种类(GA)    字符(4)   详见数据字典8.1序6 是
            bo.TestLineNo = ((DataRow)obj_car)["LINEID"].ToString();  //TestLineNo  检测线代码   字符(8)   格式详见3定义部分   是
            bo.OperatorName = ((DataRow)obj_car)["CZY"].ToString();  //OperatorName    操作员 字符(32)      是
            bo.DriverName = ((DataRow)obj_car)["JSY"].ToString();  //DriverName  引车员 字符(32)      是
            bo.Teststime = Convert.ToDateTime(((DataRow)obj_result)["JCKSSJ"].ToString()).ToString("yyyyMMddHHmmss");  //Teststime   检测开始时间  日期格式    YYYYMMDDHHmmss  是
            bo.Testetime = Convert.ToDateTime(((DataRow)obj_result)["JCJSSJ"].ToString()).ToString("yyyyMMddHHmmss");  //Testetime   检测结束时间  日期格式    YYYYMMDDHHmmss  是
            bo.TestType = DicJcffR.GetValue(((DataRow)obj_car)["JCFF"].ToString(), "");  //TestType    检测方法    字符(1)   详见数据字典8.2序4 是
            bo.TestJudge = DicResultR.GetValue(((DataRow)obj_car)["JCJG"].ToString(), "");  //TestJudge   检测结果    字符(1)   1:合格0：不合格，上报值为（0或1）	是
            bo.Samplingdepth = "400";  //Samplingdepth   采样深度 数值(4)       是
            bo.Temperature = ((DataRow)obj_result)["WD"].ToString();  //Temperature 温度  数值(12, 4)	（Co）	是
            bo.Humidity = ((DataRow)obj_result)["SD"].ToString();  //Humidity    湿度  数值(12, 4)	（%）	是
            bo.Atpressure = ((DataRow)obj_result)["DQY"].ToString();  //Atpressure  气压  数值(12, 4)	（kPa）	是
            bo.HC = ((DataRow)obj_result)["HCZL"].ToString();  //HC  HC排放结果  数值(12, 4)	（g / km）	是
            bo.CO = ((DataRow)obj_result)["COZL"].ToString();  //CO  CO排放结果  数值(12, 4)	（g / km）	是
            bo.NOx = ((DataRow)obj_result)["NOXZL"].ToString();  //NOx NOX排放结果 数值(12, 4)	（g / km）	是
            bo.HCNOx = (Convert.ToDouble(bo.HC) + Convert.ToDouble(bo.NOx)).ToString("0.00");  //HCNOx   HC + NOX排放结果  数值(12, 4)	（g / km）	FALSE
            bo.Lambda = ((DataRow)obj_result)["LAMBDA"].ToString();
            bo.LambdaUp = "0";
            bo.LambdaDown = "0";
            bo.LambdaJudge = "1";
            bo.HCLimit = ((DataRow)obj_result)["HCXZ"].ToString();  //HCLimit HC排放限值  数值(12, 4)	（g / km）	是
            bo.COLimit = ((DataRow)obj_result)["COXZ"].ToString();  //COLimit CO排放限值  数值(12, 4)	（g / km）	是
            bo.NOxLimit = ((DataRow)obj_result)["NOXXZ"].ToString();  //NOxLimit    NOX排放限值 数值(12, 4)	（g / km）	是
            bo.HCNOxLimit = (Convert.ToDouble(bo.HCLimit) + Convert.ToDouble(bo.NOxLimit)).ToString("0.00"); //HCNOxLimit  HC + NOX排放限值  数值(12, 4)	（g / km）	FALSE
            bo.HCJudge = DicResultR.GetValue(((DataRow)obj_result)["HCPD"].ToString(), "");  //HCJudge HC排放判定  字符(1)   0 - 不合格，1 - 合格  是
            bo.COJudge = DicResultR.GetValue(((DataRow)obj_result)["COPD"].ToString(), "");  //COJudge CO排放判定  字符(1)   0 - 不合格，1 - 合格  是
            bo.NOxJudge = DicResultR.GetValue(((DataRow)obj_result)["NOXPD"].ToString(), "");  //NOxJudge    NOX排放判定 字符(1)   0 - 不合格，1 - 合格  是
            bo.HCNOxJudge = (Convert.ToDouble(bo.HCNOx)<=Convert.ToDouble(bo.HCNOxLimit))?"1":"0";// DicResultR.GetValue(((DataRow)obj_result)["ZSPD"].ToString(), "");  //HCNOxJudge  HC + NOX排放判定  字符(1)   0 - 不合格，1 - 合格  FALSE

            body.Add(bo);
            dt.body = body;

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            FileOpreate.SaveLog( json,"[uploadInspResultVMAS]：", 3);

            return json;
        }
        public bool uploadExhaustResult(object obj_car, object obj_result,out string testingid)
        {
            string jcff = ((DataRow)obj_car)["JCFF"].ToString();
            string modeljson = "";

            switch (jcff)
            {
                case "ZYJS":
                    modeljson = createResultLIGHTPROOF(obj_car,obj_result);
                    break;
                case "JZJS":
                    modeljson = createResultLUGDOWN(obj_car, obj_result);
                    break;
                case "SDS":
                    modeljson = createResultDIDLE(obj_car, obj_result);
                    break;
                case "VMAS":
                    modeljson = createResultVMAS(obj_car, obj_result);
                    break;
            }

            //textBoxJson.Text = modeljson;

            //GET数据 提取时间信息
            if (Get尾气检测结果(ModPublicJHHB.webAddress, modeljson, "GET",out testingid))
            {
                FileOpreate.SaveLog( "成功","[尾气检测结果上传_Parser]：", 3);
                //labelStatus.Text = "POST success!";
                Console.WriteLine("POST success!");
                return true;
            }
            else
            {
                FileOpreate.SaveLog("[尾气检测结果上传_Parser]：", "成功", 3);
                //labelStatus.Text = "POST fail!";
                Console.WriteLine("POST fail!");
                return false;
            }

        }
        bool Get尾气检测结果(string webadd, string content, string model,out string testingid)
        {
            testingid = "";
            if (content == "")
            {
                MessageBox.Show("数据不能为空"); return false;
            }
            try
            {
                byte[] d = System.Text.Encoding.UTF8.GetBytes(content);
                System.Net.WebClient aaa = new System.Net.WebClient();
                aaa.Headers.Add("Content-Type", "application/json; charset=UTF-8");
                byte[] res;
                if (model == "GET")
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                else
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                String ReadData = System.Text.Encoding.UTF8.GetString(res);
                //textBoxResult.Text = ReadData;
                string jsonText = ReadData;
                FileOpreate.SaveLog( jsonText,"[尾气检测结果上传_Ack]：", 3);
                ModPublicJHHB.uploadAck ack = JsonConvert.DeserializeObject<ModPublicJHHB.uploadAck>(jsonText);
                if (ack.result[0].code != "1" && ack.result[0].code != "2")
                {
                    //MessageBox.Show("Code:" + ack.result[0].code + "\r\n;Message:" + ack.result[0].message);
                    return false;
                }
                if(ack.result[0].testingId==null|| ack.result[0].testingId=="")
                {
                    Console.WriteLine("获取tesingId失败");
                    return false;
                }
                testingid = ack.result[0].testingId;
                return true;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog( er.Message, "[尾气检测结果上传_Exception]：",3);
                Console.WriteLine("exception occured:" + er.Message);
                //labelStatus.Text = "exception occured:" + er.Message;
                //textBoxResult.Text = er.Message;
                return false;
            }
        }
        public bool uploadExhaustProcess(object obj_car, object obj_data,  string testingid)
        {
            string jcff = ((DataRow)obj_car)["JCFF"].ToString();
            string modeljson = "";

            switch (jcff)
            {
                case "ZYJS":
                    modeljson = createProcessLIGHTPROOF(obj_car, obj_data,testingid);
                    break;
                case "JZJS":
                    modeljson = createProcessLUGDOWN(obj_car, obj_data, testingid);
                    break;
                case "SDS":
                    modeljson = createProcessDIDLE(obj_car, obj_data, testingid);
                    break;
                case "VMAS":
                    modeljson = createProcessVMAS(obj_car, obj_data, testingid);
                    break;
            }

            //textBoxJson.Text = modeljson;

            //GET数据 提取时间信息
            if (Get尾气检测过程(ModPublicJHHB.webAddress, modeljson, "GET"))
            {
                FileOpreate.SaveLog( "成功","[尾气过程数据上传_Parser]：",3);
                //labelStatus.Text = "POST success!";
                Console.WriteLine("POST success!");
                return true;
            }
            else
            {
                FileOpreate.SaveLog("成功","[尾气过程数据上传_Parser]：",  3);
                //labelStatus.Text = "POST fail!";
                Console.WriteLine("POST fail!");
                return false;
            }

        }
        public string createProcessLIGHTPROOF(object obj_car,object obj_data,string testingid)
        {

            string inspregid = ((DataRow)obj_data)["JYLSH"].ToString();
            string[] MMTIME = ((DataRow)obj_data)["MMTIME"].ToString().Split(',');
            string[] MMLB = ((DataRow)obj_data)["MMLB"].ToString().Split(',');
            string[] MMK = ((DataRow)obj_data)["MMK"].ToString().Split(',');
            string[] MMZS = ((DataRow)obj_data)["MMZS"].ToString().Split(',');
            string[] MMYW = ((DataRow)obj_data)["MMYW"].ToString().Split(',');
            string[] MMWD = ((DataRow)obj_data)["MMWD"].ToString().Split(',');
            string[] MMSD = ((DataRow)obj_data)["MMSD"].ToString().Split(',');
            string[] MMDQY = ((DataRow)obj_data)["MMDQY"].ToString().Split(',');
            int count = MMLB.Count();
            int count0 = 1;
            int count1 = 1;
            int count2 = 1;
            int count3 = 1;
            ModPublicJHHB.uploadInspProcessLIGHTPROOF model = new ModPublicJHHB.uploadInspProcessLIGHTPROOF();
            model.jkid = "uploadInspProcessLIGHTPROOF";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.ProcessLIGHTPROOFDataItem>();
            
            for (int i = 0; i < count; i++)
            {
                ModPublicJHHB.ProcessLIGHTPROOFDataItem dt = new ModPublicJHHB.ProcessLIGHTPROOFDataItem();
                dt.body = new List<ModPublicJHHB.ProcessLIGHTPROOFbodyItem>();
                ModPublicJHHB.ProcessLIGHTPROOFbodyItem bo = new ModPublicJHHB.ProcessLIGHTPROOFbodyItem();

                bo.id = System.Guid.NewGuid().ToString().Replace("-", "");   //id ID  字符(32)  检测线尾气检测过程私有唯一标识 是
                bo.testingid = testingid;   //testingid 检测记录ID  字符(32)  由环保局返回的检测登录唯一标识 是
                bo.inspregid = inspregid;   //inspregid 检测结果id  字符(32)  由环保局返回的检测结果唯一标识 是
                bo.tsno = ModPublicJHHB.TsNo;   //tsno 检测机构编号  字符(16)  格式详见3定义部分
                bo.serialno =(i+1).ToString();   //serialno    采样时序 数值(4)   取样点序号，对应同一个检测记录ID，从1开始顺序编号  是
                bo.opcode = MMLB[i];   //opcode  工况类型    字符(8)   0：最后三次测量前过程数据 1 - 3：最后三次测量时过程数据 是
                if (bo.opcode == "0")
                {
                    bo.opno = count0.ToString();
                    count0++;                
                }
                else if (bo.opcode == "1")
                {
                    bo.opno = count1.ToString();
                    count1++;
                }
                else if (bo.opcode == "2")
                {
                    bo.opno = count2.ToString();
                    count2++;
                }
                else if (bo.opcode == "3")
                {
                    bo.opno = count3.ToString();
                    count3++;
                }
                else
                {
                    continue;
                }
                bo.samplingdepth = "400";   //samplingdepth   采样深度    数值(4)(取样探头插管深度)毫米，必须为传感器实测值  是
                bo.optime = Convert.ToDateTime(MMTIME[i]).ToString("yyyyMMddHHmmss");   //optime  采样时间    日期格式    yyyyMMddHHmmss  是
                bo.posno = "0";   //posno   档位数 数值(2)       FALSE
                bo.vehiclespeed = "0";   //vehiclespeed    车速  数值(12, 4)        FALSE
                bo.enginespeed = MMZS[i];   //enginespeed 发动机转速   数值(4)       是
                bo.enginepower = "0";   //enginepower 发动机功率   数值(12, 4)        FALSE
                bo.sfk = MMK[i];   //sfk 光吸收系数   数值(12, 4)        是
                bo.sfn = MMK[i];   //sfn 线性分度单位  数值(12, 4)        FALSE
                bo.temperature = MMWD[i];   //temperature 温度  数值(12, 4)        是
                bo.humidity = MMSD[i];   //humidity    湿度  数值(12, 4)        是
                bo.pressure = MMDQY[i];   //pressure    气压  数值(12, 4)        是
                bo.kjudge = DicResultR.GetValue(((DataRow)obj_car)["JCJG"].ToString(), "");   //kjudge  排放判定    字符(1)       是

                dt.body.Add(bo);
                model.data.Add(dt);
            }
            
            

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            FileOpreate.SaveLog( json,"[uploadInspProcessLIGHTPROOF]：", 3);

            return json;
        }


        // 7.2.2.2.3 加载减速法检测过程数据报文生成
        public string createProcessLUGDOWN(object obj_car, object obj_data, string testingid)
        {
            string inspregid = ((DataRow)obj_data)["JYLSH"].ToString();
            string[] MMTIME = ((DataRow)obj_data)["MMTIME"].ToString().Split(',');
            string[] MMLB = ((DataRow)obj_data)["MMLB"].ToString().Split(',');
            string[] MMCS = ((DataRow)obj_data)["MMCS"].ToString().Split(',');
            string[] MMGL = ((DataRow)obj_data)["MMGL"].ToString().Split(',');
            string[] MMK = ((DataRow)obj_data)["MMK"].ToString().Split(',');
            string[] MMZS = ((DataRow)obj_data)["MMZS"].ToString().Split(',');
            string[] MMZGL = ((DataRow)obj_data)["MMZGL"].ToString().Split(',');
            string[] MMZSGL = ((DataRow)obj_data)["MMZSGL"].ToString().Split(',');
            string[] MMGLXZXS = ((DataRow)obj_data)["MMGLXZXS"].ToString().Split(',');
            string[] MMJSGL = ((DataRow)obj_data)["MMJSGL"].ToString().Split(',');
            string[] MMBTGD = ((DataRow)obj_data)["MMBTGD"].ToString().Split(',');
            string[] MMNL = ((DataRow)obj_data)["MMNL"].ToString().Split(',');
            string[] MMNO = ((DataRow)obj_data)["MMNO"].ToString().Split(',');
            string[] MMCO2 = ((DataRow)obj_data)["MMCO2"].ToString().Split(',');
            string[] MMYW = ((DataRow)obj_data)["MMYW"].ToString().Split(',');
            string[] MMWD = ((DataRow)obj_data)["MMHJWD"].ToString().Split(',');
            string[] MMSD = ((DataRow)obj_data)["MMXDSD"].ToString().Split(',');
            string[] MMDQY = ((DataRow)obj_data)["MMDQYL"].ToString().Split(',');
            string[] MMOPNO = ((DataRow)obj_data)["MMOPNO"].ToString().Split(',');
            string[] MMOPCODE = ((DataRow)obj_data)["MMOPCODE"].ToString().Split(',');
            int count = MMLB.Count();
            int count0 = 1;
            int count1 = 1;
            int count2 = 1;
            int count3 = 1;
            int count4 = 1;
            int count5 = 1;
            ModPublicJHHB.uploadInspProcessLUGDOWN model = new ModPublicJHHB.uploadInspProcessLUGDOWN();
            model.jkid = "uploadInspProcessLUGDOWN";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.ProcessLUGDOWNDataItem>();
            for (int i = 0; i < count; i++)
            {
                ModPublicJHHB.ProcessLUGDOWNDataItem dt = new ModPublicJHHB.ProcessLUGDOWNDataItem();
                dt.body = new List<ModPublicJHHB.ProcessLUGDOWNbodyItem>();
                
                ModPublicJHHB.ProcessLUGDOWNbodyItem bo = new ModPublicJHHB.ProcessLUGDOWNbodyItem();

                bo.id = System.Guid.NewGuid().ToString().Replace("-", "");   //id ID  字符(32)  检测线尾气检测过程私有唯一标识 是
                bo.testingid = testingid;   //testingid 检测记录ID  字符(32)  由环保局返回的检测登录唯一标识 是
                bo.inspregid = inspregid;   //inspregid 检测结果id  字符(32)  由环保局返回的检测结果唯一标识 是
                bo.tsno = ModPublicJHHB.TsNo;   //tsno 检测机构编号  字符(16)  格式详见3定义部分
                bo.serialno = (i + 1).ToString();   //serialno    采样时序 数值(4)   取样点序号，对应同一个检测记录ID，从1开始顺序编号  是
                bo.opcode = MMOPNO[i];   //opcode  工况类型    字符(8)   0：最后三次测量前过程数据 1 - 3：最后三次测量时过程数据 是
                bo.opno = MMOPNO[i];                
                bo.Samplingdepth = "400";   //Samplingdepth 采样深度    数值(4)(取样探头插管深度)毫米，必须为传感器实测值 是
                bo.optime = Convert.ToDateTime(MMTIME[i]).ToString("yyyyMMddHHmmss");// optime 采样时间    日期格式 yyyyMMddHHmmss  是
                bo.posno = "3";   //posno   档位数 数值(2)       FALSE
                bo.vehiclespeed = MMCS[i];   //vehiclespeed    车速  数值(12, 4)        是
                bo.enginespeed = MMZS[i];   //enginespeed 发动机转速   数值(4)       是
                bo.enginepower = MMGL[i];   //enginepower 发动机功率   数值(12, 4)        是
                bo.sfk = MMK[i];   //sfk 光吸收系数   数值(12, 4)        是
                bo.sfn = MMBTGD[i];   //sfn 线性分度单位  数值(12, 4)        FALSE
                bo.temperature =MMWD[i];   //temperature 温度  数值(12, 4)        是
                bo.humidity = MMSD[i];   //humidity    湿度  数值(12, 4)        是
                bo.pressure = MMDQY[i];   //pressure    气压  数值(12, 4)        是
                bo.dynpa = MMZGL[i];   //dynpa   底盘测功机总加载功率  数值(12, 4)        是
                bo.dynplhp = MMJSGL[i];   //dynplhp 底盘测功机寄生功率   数值(12, 4)        是
                bo.dynihp = MMZSGL[i];   //dynihp  底盘测功机指示功率   数值(12, 4)        是
                bo.dynn = MMNL[i];   //dynn    底盘测功机扭力 数值(12, 4)        是
                bo.pcf = MMGLXZXS[i];   //pcf 功率修正系数  数值(12, 4)        是
                bo.volco2 = MMCO2[i];   //volco2  二氧化碳浓度  数值(12, 4)	（%）	是
                bo.volnox = MMNO[i];   //volnox  氮氧化合物浓度 数值(12, 4)	（10 - 6）	是
                bo.kjudge = DicResultR.GetValue(((DataRow)obj_car)["JCJG"].ToString(), "");   //kjudge  排放判定    字符(1)       是


                dt.body.Add(bo);
                model.data.Add(dt);
            }

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            FileOpreate.SaveLog( json,"[uploadInspProcessLUGDOWN]：", 3);

            return json;
        }


        // 7.2.2.2.4 双怠速法检测过程数据报文生成
        public string createProcessDIDLE(object obj_car, object obj_data, string testingid)
        {

            string inspregid = ((DataRow)obj_data)["JYLSH"].ToString();
            string[] MMTIME = ((DataRow)obj_data)["MMTIME"].ToString().Split(',');
            string[] MMLB = ((DataRow)obj_data)["MMLB"].ToString().Split(',');
            string[] MMHC = ((DataRow)obj_data)["MMHC"].ToString().Split(',');
            string[] MMCO = ((DataRow)obj_data)["MMCO"].ToString().Split(',');
            string[] MMNO = ((DataRow)obj_data)["MMNO"].ToString().Split(',');
            string[] MMO2 = ((DataRow)obj_data)["MMO2"].ToString().Split(',');
            string[] MMCO2 = ((DataRow)obj_data)["MMCO2"].ToString().Split(',');
            string[] MMLAMDA = ((DataRow)obj_data)["MMLAMDA"].ToString().Split(',');
            string[] MMZS = ((DataRow)obj_data)["MMZS"].ToString().Split(',');
            string[] MMYW = ((DataRow)obj_data)["MMYW"].ToString().Split(',');
            string[] MMWD = ((DataRow)obj_data)["MMWD"].ToString().Split(',');
            string[] MMSD = ((DataRow)obj_data)["MMSD"].ToString().Split(',');
            string[] MMDQY = ((DataRow)obj_data)["MMDQY"].ToString().Split(',');
            int count = MMLB.Count();
            int count0 = 1;
            int count1 = 1;
            int count2 = 1;
            int count3 = 1;
            int count4 = 1;
            int count5 = 1;
            ModPublicJHHB.uploadInspProcessDIDLE model = new ModPublicJHHB.uploadInspProcessDIDLE();
            model.jkid = "uploadInspProcessDIDLE";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.ProcessDIDLEDataItem>();
           
            for (int i = 0; i < count; i++)
            {
                ModPublicJHHB.ProcessDIDLEDataItem dt = new ModPublicJHHB.ProcessDIDLEDataItem();
                dt.body = new List<ModPublicJHHB.ProcessDIDLEbodyItem>();

                ModPublicJHHB.ProcessDIDLEbodyItem bo = new ModPublicJHHB.ProcessDIDLEbodyItem();

                //  bo.Id = "20191111";  //Id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
                //bo.inspregid = System.Guid.NewGuid().ToString().Replace("-", "");  //inspregid 检验登录ID  字符(32)  由环保局返回的检测登录唯一标识 是

                bo.id = System.Guid.NewGuid().ToString().Replace("-", "");   //id ID  字符(32)  检测线尾气检测过程私有唯一标识 是
                bo.testingid = testingid;   //testingid 检测记录ID  字符(32)  由环保局返回的检测登录唯一标识 是
                bo.inspregid = inspregid;   //inspregid 检测结果id  字符(32)  由环保局返回的检测结果唯一标识 是
                bo.tsno = ModPublicJHHB.TsNo;    //tsno 检测机构编号  字符(16)  格式详见3定义部分
                bo.serialno = (i + 1).ToString();   //serialno    采样时序 数值(4)   逐秒，从1开始，每条递增1   是 
                bo.opcode = MMLB[i];   //opcode  工况类型    字符(8)   0 - 70 % 额定转速 bo.id = "20191111";   //1 - 高怠速准备 bo.id = "20191111";   //2 - 高怠速检测 bo.id = "20191111";   //3 - 怠速准备 bo.id = "20191111";   //4 - 怠速检测    是

                if (bo.opcode == "0")
                {
                    bo.opno = count0.ToString();
                    count0++;
                }
                else if (bo.opcode == "1")
                {
                    bo.opno = count1.ToString();
                    count1++;
                }
                else if (bo.opcode == "2")
                {
                    bo.opno = count2.ToString();
                    count2++;
                }
                else if (bo.opcode == "3")
                {
                    bo.opno = count3.ToString();
                    count3++;
                }
                else if (bo.opcode == "4")
                {
                    bo.opno = count4.ToString();
                    count4++;
                }
                else if (bo.opcode == "5")
                {
                    bo.opno = count5.ToString();
                    count5++;
                }
                else
                {
                    continue;
                }
                bo.samplingdepth = "400";   //samplingdepth   采样深度    数值(4)(取样探头插管深度)毫米，必须为传感器实测值  是
                bo.Optime = Convert.ToDateTime(MMTIME[i]).ToString("yyyyMMddHHmmss");   //Optime  采样时间    日期格式    YYYYMMDDHHmmss  是
                bo.Posno = "0";   //Posno   档位数 数值(2)   0默认为零   FALSE
                bo.vehiclespeed = "0";   //vehiclespeed    车速  数值(12, 4)    0默认为零   FALSE
                bo.enginespeed = MMZS[i];   //enginespeed 发动机转速   数值(4)       是
                bo.Volco = MMCO[i];   //Volco   一氧化碳浓度CO    数值(12, 4)        是
                bo.volco2 = MMCO2[i];   //volco2  二氧化碳浓度CO2   数值(12, 4)        是
                bo.Volhc = MMHC[i];   //Volhc   碳氢化合物浓度HC   数值(12, 4)        是
                bo.Volnox = MMNO[i];   //Volnox  氮氧化物浓度NOX   数值(12, 4)        FALSE
                bo.volo2 = MMO2[i];   //volo2   原始氧浓度O2 数值(12, 4)        是
                bo.temperature = MMWD[i];   //temperature 温度  数值(12, 4)        是
                bo.humidity = MMSD[i];   //humidity    湿度  数值(12, 4)        是
                bo.pressure = MMDQY[i];   //pressure    气压  数值(12, 4)        是
                bo.kjudge = DicResultR.GetValue(((DataRow)obj_car)["JCJG"].ToString(), "");   //kjudge  排放判定    字符(1)       是

                dt.body.Add(bo);
                model.data.Add(dt);
            }

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            FileOpreate.SaveLog(json,"[uploadInspProcessDIDLE]：",  3);

            return json;
        }


        // 7.2.2.2.6 简易瞬态工况法检测过程数据报文生成
        public string createProcessVMAS(object obj_car, object obj_data, string testingid)
        {
            string inspregid = ((DataRow)obj_data)["JYLSH"].ToString();
            string[] MMTIME = ((DataRow)obj_data)["MMTIME"].ToString().Split(',');
            string[] MMLB = ((DataRow)obj_data)["MMLB"].ToString().Split(',');
            string[] MMHC = ((DataRow)obj_data)["MMHC"].ToString().Split(',');
            string[] MMCO = ((DataRow)obj_data)["MMCO"].ToString().Split(',');
            string[] MMNO = ((DataRow)obj_data)["MMNO"].ToString().Split(',');
            string[] MMO2 = ((DataRow)obj_data)["MMO2"].ToString().Split(',');
            string[] MMCO2 = ((DataRow)obj_data)["MMCO2"].ToString().Split(',');
            string[] MMLAMBDA = ((DataRow)obj_data)["MMLAMBDA"].ToString().Split(',');
            string[] MMHCZL = ((DataRow)obj_data)["MMHCZL"].ToString().Split(',');
            string[] MMCOZL = ((DataRow)obj_data)["MMCOZL"].ToString().Split(',');
            string[] MMNOZL = ((DataRow)obj_data)["MMNOZL"].ToString().Split(',');
            string[] MMCO2ZL = ((DataRow)obj_data)["MMCO2ZL"].ToString().Split(',');

            string[] MMXSO2 = ((DataRow)obj_data)["MMXSO2"].ToString().Split(',');
            string[] MMHJO2 = ((DataRow)obj_data)["MMHJO2"].ToString().Split(',');
            string[] MMSJLL = ((DataRow)obj_data)["MMSJLL"].ToString().Split(',');
            string[] MMBZLL = ((DataRow)obj_data)["MMBZLL"].ToString().Split(',');
            string[] MMWQLL = ((DataRow)obj_data)["MMWQLL"].ToString().Split(',');
            string[] MMXSB = ((DataRow)obj_data)["MMXSB"].ToString().Split(',');

            string[] MMWD = ((DataRow)obj_data)["MMWD"].ToString().Split(',');
            string[] MMSD = ((DataRow)obj_data)["MMSD"].ToString().Split(',');
            string[] MMDQY = ((DataRow)obj_data)["MMDQY"].ToString().Split(',');
            string[] MMLLJWD = ((DataRow)obj_data)["MMLLJWD"].ToString().Split(',');
            string[] MMLLJYL = ((DataRow)obj_data)["MMLLJYL"].ToString().Split(',');
            string[] MMCS = ((DataRow)obj_data)["MMCS"].ToString().Split(',');
            string[] MMBZCS = ((DataRow)obj_data)["MMBZCS"].ToString().Split(',');
            string[] MMXSXZ = ((DataRow)obj_data)["MMXSXZ"].ToString().Split(',');
            string[] MMSDXZ = ((DataRow)obj_data)["MMSDXZ"].ToString().Split(',');
            string[] MMYW = ((DataRow)obj_data)["MMYW"].ToString().Split(',');
            string[] MMJSGL = ((DataRow)obj_data)["MMJSGL"].ToString().Split(',');
            string[] MMNJ = ((DataRow)obj_data)["MMNJ"].ToString().Split(',');
            string[] MMGL = ((DataRow)obj_data)["MMGL"].ToString().Split(',');
            string[] MMZS = ((DataRow)obj_data)["MMZS"].ToString().Split(',');
            string[] MMZSGL = ((DataRow)obj_data)["MMZSGL"].ToString().Split(',');
            string[] MMJZGL = ((DataRow)obj_data)["MMJZGL"].ToString().Split(',');

            int count = MMLB.Count();
            
            ModPublicJHHB.uploadInspProcessVMAS model = new ModPublicJHHB.uploadInspProcessVMAS();
            model.jkid = "uploadInspProcessVMAS";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.ProcessVMASDataItem>();

            int fqydelaytime = count - 195;
            for (int i = fqydelaytime; i < count; i++)
            {
                ModPublicJHHB.ProcessVMASDataItem dt = new ModPublicJHHB.ProcessVMASDataItem();
                dt.body = new List<ModPublicJHHB.ProcessVMASbodyItem>();

                int index = i - fqydelaytime+1;
                ModPublicJHHB.ProcessVMASbodyItem bo = new ModPublicJHHB.ProcessVMASbodyItem();
                int opno, opcode;
                if (index <= 11)
                {
                    opno = 1;
                    opcode = 1;
                }
                else if (index <= 15)
                {
                    opno = 2;
                    opcode = 2;
                }
                else if (index <= 23)
                {
                    opno = 3;
                    opcode = 3;
                }
                else if (index <= 25)
                {
                    opno = 4;
                    opcode = 4;
                }
                else if (index <= 28)
                {
                    opno = 5;
                    opcode = 4;
                }
                else if (index <= 49)
                {
                    opno = 6;
                    opcode = 5;
                }
                else if (index <= 54)
                {
                    opno = 7;
                    opcode = 6;
                }
                else if (index <= 56)
                {
                    opno = 8;
                    opcode = 6;
                }
                else if (index <= 61)
                {
                    opno = 9;
                    opcode = 6;
                }
                else if (index <= 85)
                {
                    opno = 10;
                    opcode = 7;
                }
                else if (index <= 93)
                {
                    opno = 11;
                    opcode = 8;
                }
                else if (index <= 96)
                {
                    opno = 12;
                    opcode = 8;
                }
                else if (index <= 117)
                {
                    opno = 13;
                    opcode = 9;
                }
                else if (index <= 122)
                {
                    opno = 14;
                    opcode = 10;
                }
                else if (index <= 124)
                {
                    opno = 15;
                    opcode = 10;
                }
                else if (index <= 133)
                {
                    opno = 16;
                    opcode = 10;
                }
                else if (index <= 135)
                {
                    opno = 17;
                    opcode = 10;
                }
                else if (index <= 143)
                {
                    opno = 18;
                    opcode = 10;
                }
                else if (index <= 155)
                {
                    opno = 19;
                    opcode = 11;
                }
                else if (index <= 163)
                {
                    opno = 20;
                    opcode = 12;
                }
                else if (index <= 176)
                {
                    opno = 21;
                    opcode = 13;
                }
                else if (index <= 178)
                {
                    opno = 22;
                    opcode = 14;
                }
                else if (index <= 185)
                {
                    opno = 23;
                    opcode = 14;
                }
                else if (index <= 188)
                {
                    opno = 24;
                    opcode = 14;
                }
                else if (index <= 195)
                {
                    opno = 25;
                    opcode = 15;
                }
                else
                {
                    continue;
                }

                bo.Id = System.Guid.NewGuid().ToString().Replace("-", "");   //Id ID  字符(32)  检测线尾气检测过程私有唯一标识 是
                bo.testingid = testingid;   //testingid 检测记录ID  字符(32)  由环保局返回的检测登录唯一标识 是
                bo.inspregid = inspregid;   //inspregid 检测结果id  字符(32)  由环保局返回的检测结果唯一标识 是
                bo.tsno = ModPublicJHHB.TsNo;   //tsno 检测机构编号  字符(16)  格式详见3定义部分
                bo.serialno = (index).ToString();   //serialno    采样时序 数值(4)   逐秒，从1开始，每条递增1，总共195秒    是
                bo.opno = opno.ToString();   //opno    操作顺序号   数值(4)   见下述简易瞬态工况法操作码表  是
                bo.opcode = opcode.ToString();   //opcode  操作码 字符(8)   见下述简易瞬态工况法操作码表  是
                bo.samplingdepth = "400";   //samplingdepth   采样深度    数值(4)(取样探头插管深度)毫米，必须为传感器实测值  是
                bo.optime = Convert.ToDateTime(MMTIME[i]).ToString("yyyyMMddHHmmss");  //optime  采样时间    日期格式    yyyyMMddHHmmss  是
                bo.posno = "3";   //posno   档位数 数值(2)       FALSE
                bo.vehiclespeed = MMCS[i];   //vehiclespeed    车速  数值(12, 4)        是
                bo.enginespeed = MMZS[i];   //enginespeed 发动机转速   数值(4)       FALSE
                bo.volco = MMCO[i];   //volco   一氧化碳浓度CO    数值(12, 4)        是
                bo.volco2 = MMCO2[i];   //volco2  二氧化碳浓度CO2   数值(12, 4)        是
                bo.volhc = MMHC[i];   //volhc   碳氢化合物浓度HC   数值(12, 4)        是
                bo.volnox = MMNO[i];   //volnox  氮氧化物浓度NOX   数值(12, 4)        是
                bo.volo2raw = MMO2[i];   //volo2raw    原始氧浓度O2 数值(12, 4)        是
                bo.volo2dil = MMXSO2[i];   //volo2dil    稀释氧浓度O2 数值(12, 4)        是
                bo.volo2amb = MMHJO2[i];   //volo2amb    环境氧浓度O2 数值(12, 4)        是
                bo.flowactual = MMSJLL[i];   //flowactual  实际流量    数值(12, 4)        是
                bo.flowexhaust = MMWQLL[i];   //flowexhaust 尾气流量    数值(12, 4)        是
                bo.flowstd = MMBZLL[i];   //flowstd 标准流量    数值(12, 4)        是
                bo.temperature = MMWD[i];   //temperature 温度  数值(12, 4)        是
                bo.humidity = MMSD[i];   //humidity    湿度  数值(12, 4)        是
                bo.pressure = MMDQY[i];   //pressure    气压  数值(12, 4)        是
                bo.flowmetert = MMLLJWD[i];   //flowmetert  流量计温度   数值(12, 4)        是
                bo.flowmeterp = MMLLJYL[i];   //flowmeterp  流量计压力   数值(12, 4)        是
                bo.dynpa = MMJZGL[i];   //dynpa   底盘测功机总加载功率  数值(12, 4)        是
                bo.dynplhp = MMJSGL[i];   //dynplhp 底盘测功机寄生功率   数值(12, 4)        是
                bo.dynihp = MMZSGL[i];   //dynihp  底盘测功机指示功率   数值(12, 4)        是
                bo.dynn = MMNJ[i];   //dynn    底盘测功机扭力 数值(12, 4)        是
                bo.dcf = MMXSXZ[i];   //dcf 稀释修正系数  数值(12, 4)        是
                bo.kh = MMSDXZ[i];   //kh  湿度修正系数  数值(12, 4)        是
                bo.dr = MMXSB[i];   //dr  稀释比 数值(12, 4)        是
                bo.massco = MMCOZL[i];   //massco  一氧化碳质量CO    数值(12, 4)        是
                bo.massco2 = MMCO2ZL[i];   //massco2 二氧化碳质量CO2   数值(12, 4)        是
                bo.masshc = MMHCZL[i];   //masshc  碳氢化合物质量HC   数值(12, 4)        是
                bo.massnox = MMNOZL[i];   //massnox 氮氧化物质量NOX   数值(12, 4)        是
                bo.kjudge = DicResultR.GetValue(((DataRow)obj_car)["JCJG"].ToString(), "");   //kjudge  排放判定    字符(1)       FALSE

                dt.body.Add(bo);
                model.data.Add(dt);
            }

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            FileOpreate.SaveLog( json, "[uploadInspProcessVMAS]：",3);

            return json;
        }


        bool Get尾气检测过程(string webadd, string content, string model)
        {
            if (content == "")
            {
                MessageBox.Show("数据不能为空"); return false;
            }
            try
            {
                byte[] d = System.Text.Encoding.UTF8.GetBytes(content);
                System.Net.WebClient aaa = new System.Net.WebClient();
                aaa.Headers.Add("Content-Type", "application/json; charset=UTF-8");
                byte[] res;
                if (model == "GET")
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                else
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                String ReadData = System.Text.Encoding.UTF8.GetString(res);
                //textBoxResult.Text = ReadData;
                string jsonText = ReadData;
                FileOpreate.SaveLog(jsonText,"[尾气过程数据_Ack]：",  3);
                ModPublicJHHB.uploadAck ack = JsonConvert.DeserializeObject<ModPublicJHHB.uploadAck>(jsonText);
                if (ack.result[0].code != "1" && ack.result[0].code != "2")
                {
                    //MessageBox.Show("Code:" + ack.result[0].code + "\r\n;Message:" + ack.result[0].message);
                    return false;
                }
                return true;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog(er.Message,"[尾气过程数据_Excetion]：",  3);
                Console.WriteLine("exception occured:" + er.Message);
                //labelStatus.Text = "exception occured:" + er.Message;
                //textBoxResult.Text = er.Message;
                return false;
            }
        }
        public bool getTestNo(string testingid,out string jcbgbh)
        {
            jcbgbh = "";
            string modeljson = createTestingCheck(testingid);
            //textBoxJson.Text = modeljson;

            //GET数据 提取时间信息
            if (GetTestingCheck(ModPublicJHHB.webAddress, modeljson, "GET",out jcbgbh))
            {
                FileOpreate.SaveLog( "成功","[downInspTestingCheck_Parser]：", 3);
                //labelStatus.Text = "POST success!";
                Console.WriteLine("POST success!");
                return true;
            }
            else
            {
                FileOpreate.SaveLog("失败","[downInspTestingCheck_Parser]：",  3);
                //labelStatus.Text = "POST fail!";
                Console.WriteLine("POST fail!");
                return false;
            }

        }
        public string createTestingCheck(string testingid)
        {
            ModPublicJHHB.downInspTestingCheck model = new ModPublicJHHB.downInspTestingCheck();

            model.jkid = "downInspTestingCheck";
            model.jksqm = ModPublicJHHB.jksqm;
            model.testingid = testingid;

            string json = JsonConvert.SerializeObject(model);
            FileOpreate.SaveLog( json,"[downInspTestingCheck]：", 3);
            return json;
        }

        //返回获取检测报告单编号
        bool GetTestingCheck(string webadd, string content, string model,out string jcbgbh)
        {
            jcbgbh = "";
            if (content == "")
            {
                MessageBox.Show("数据不能为空"); return false;
            }
            try
            {
                byte[] d = System.Text.Encoding.UTF8.GetBytes(content);
                System.Net.WebClient aaa = new System.Net.WebClient();
                aaa.Headers.Add("Content-Type", "application/json; charset=UTF-8");
                byte[] res;
                if (model == "GET")
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                else
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                String ReadData = System.Text.Encoding.UTF8.GetString(res);
                //textBoxResult.Text = ReadData;
                string jsonText = ReadData;
                FileOpreate.SaveLog( jsonText, "[downInspTestingCheck_Ack]：",3);
                ModPublicJHHB.uploadAck ack = JsonConvert.DeserializeObject<ModPublicJHHB.uploadAck>(jsonText);
                if (ack.result[0].code != null && ack.result[0].code != "" && ack.result[0].code != "1" && ack.result[0].code != "2")
                {
                    FileOpreate.SaveLog("Code:" + ack.result[0].code + "\r\n;Message:" + ack.result[0].message,"读取检测单信息失败",1);
                    return false;
                }
                
                Newtonsoft.Json.Linq.JObject JsonObj;
                JsonObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonText); //将json字符串转化为json对象 
                jcbgbh = GetRegexStr(jsonText, "testno");
                if (jcbgbh == null || jcbgbh == "")
                    return false;
                return true;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog( er.Message,"[downInspTestingCheck_Exception]：", 3);
                Console.WriteLine("exception occured:" + er.Message);
                //labelStatus.Text = "exception occured:" + er.Message;
                //textBoxResult.Text = er.Message;
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">lx:1-测功机自检 2-附加功率损失 3-五气分析仪 4-泄漏检查 5-分析仪氧量程检查
        /// 6-低标气检查 7-流量计检查 8-转速计 9-电子环境检查 10-NOx分析仪 11-烟度计检查 12-加载滑行检查
        /// 20-加载滑行标定 21-寄生功率校准 22-车速校准 23-扭力校准 24-废气仪标准 25-烟度计校准 26-NOx分析仪校准 27-气象站校准
        /// 30-同时时间
        /// 0-自检结束标志</param>
        /// <returns></returns>
        public bool uploadInspStmDeviceRecord(object obj)
        {
            string zjlx = ((DataRow)obj)["LX"].ToString();
            string modeljson="";
            
            switch(zjlx)
            {
                case "1":modeljson = create测功机自检(obj); break;
                case "2": modeljson = create附加功率损失检查(obj); break;
                case "3":modeljson = create五气分析仪自检(obj); break;
                case "4": modeljson = create泄漏检查(obj); break;
                case "5": modeljson = create氧量程检查(obj); break;
                case "6": modeljson = create分析仪检查(obj); break;
                case "7": modeljson = create流量计自检(obj); break;
                case "8": modeljson = create转速计自检(obj); break;
                case "9": modeljson = create电子环境自检(obj); break;
                case "11": modeljson = create烟度计自检(obj); break;
                case "12": modeljson = create加载滑行检查(obj); break;
                case "20": modeljson = create加载滑行标定(obj); break;
                case "21": modeljson = create寄生功率标定(obj); break;
                case "22": modeljson = create车速标定(obj); break;
                case "23": modeljson = create扭力标定(obj); break;
                case "24": modeljson = create废气仪标定(obj); break;
                case "25": modeljson = create烟度标定(obj); break;
                case "40": modeljson = create设备时钟同步(obj); break;
                default:return true;
            }
            if (Get检测设备运行记录(ModPublicJHHB.webAddress, modeljson, "GET"))
            {
                FileOpreate.SaveLog( "成功","[检测设备运行记录_Parser]：", 3);
                //labelStatus.Text = "POST success!";
                Console.WriteLine("POST success!");
                return true;
            }
            else
            {
                FileOpreate.SaveLog("失败","[检测设备运行记录_Parser]：",  3);
                //labelStatus.Text = "POST fail!";
                Console.WriteLine("POST fail!");
                return false;
            }
        }
        
        bool Get检测设备运行记录(string webadd, string content, string model)
        {
            if (content == "")
            {
                MessageBox.Show("数据不能为空"); return false;
            }
            try
            {
                byte[] d = System.Text.Encoding.UTF8.GetBytes(content);
                System.Net.WebClient aaa = new System.Net.WebClient();
                aaa.Headers.Add("Content-Type", "application/json; charset=UTF-8");
                byte[] res;
                if (model == "GET")
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                else
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                String ReadData = System.Text.Encoding.UTF8.GetString(res);
                //textBoxResult.Text = ReadData;
                string jsonText = ReadData;
                FileOpreate.SaveLog( jsonText, "[检测设备运行记录_Ack]：",3);
                ModPublicJHHB.uploadAck ack = JsonConvert.DeserializeObject<ModPublicJHHB.uploadAck>(jsonText);
                if (ack.result[0].code != "1" && ack.result[0].code != "2")
                {
                    //MessageBox.Show("Code:" + ack.result[0].code + "\r\n;Message:" + ack.result[0].message);
                    return false;
                }
                return true;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog(er.Message,"[检测设备运行记录_Excetion]：",  3);
                Console.WriteLine("exception occured:" + er.Message);
                //labelStatus.Text = "exception occured:" + er.Message;
                //textBoxResult.Text = er.Message;
                return false;
            }
        }
        string create测功机自检(object obj)
        {

            ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem测功机自检> model = new ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem测功机自检>();
            model.jkid = "uploadInspStmDeviceRecord";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.StmDeviceRecordDataItem测功机自检>();
            List<ModPublicJHHB.StmDeviceRecordDataItem测功机自检> data = new List<ModPublicJHHB.StmDeviceRecordDataItem测功机自检>();

            ModPublicJHHB.StmDeviceRecordDataItem测功机自检 dt = new ModPublicJHHB.StmDeviceRecordDataItem测功机自检();
            dt.body = new List<ModPublicJHHB.测功机自检bodyItem>();
            ModPublicJHHB.测功机自检bodyItem bo = new ModPublicJHHB.测功机自检bodyItem();
            #region
            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.typecode = "1";// 运行类型    字符(2)   1：自检 是
            bo.TsNo = ModPublicJHHB.TsNo;// 检测机构编号  字符(16)      是
            bo.TestLineNo = ((DataRow)obj)["JCGWH"].ToString();//  检测线 字符(16)      是
            bo.testdevicecode = bo.TsNo + "-" + bo.TestLineNo + "-" + ((DataRow)obj)["SBBH"].ToString();// 设备编号    字符(40)  参考3.7   是
            bo.testdevicetype =DicDeviceType.GetValue( "测功机","");//  设备类型    字符(40)      是
            bo.testdevicename = "底盘测功机";//   设备名称    字符(40)      是
            bo.opcode = "1";//    操作项 字符(40)  详见数据字典8.2序24    是
            bo.Operator=((DataRow)obj)["CZY"].ToString();//操作人 字符(40)  操作人姓名   是
            bo.reporttime = DateTime.Now.ToString("yyyyMMddHHmmss");// 上报时间    日期格式    yyyyMMddHHmmss  是
            bo.optime = Convert.ToDateTime(((DataRow)obj)["ZJSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  自检时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opstime = Convert.ToDateTime(((DataRow)obj)["KSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热开始时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opetime = Convert.ToDateTime(((DataRow)obj)["JSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热结束时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.status = "15";// 设备状态    字符(2)   详见数据字典8.2序9 是
            bo.result = ((DataRow)obj)["ZJJG"].ToString();//  运行结果    字符(1)   1：自检成功；0：自检失败   是
            bo.memo = "";//    备注  字符(255)     FALSE
            bo.exti01 = "1";//  通讯检查结果  字符(1)   1：成功 0：失败   是
            bo.exti02 = "1";// 举升器检查结果 字符(1)   1：成功 0：失败   是
            bo.exti03 = "2";// 加载滑行点数  数字（4）		是
            bo.extn01 = ((DataRow)obj)["DATA8"].ToString();//  惯性当量    数字（10, 2）	Kg  是
            bo.extn02 = "2";//  加载滑行点数  数字（4）	加载滑行点数  是
            bo.extn03 = ((DataRow)obj)["DATA9"].ToString().Split(',')[0];// 车速区间（起）	数字（10, 2）	km / h    是
            bo.extn04 = ((DataRow)obj)["DATA9"].ToString().Split(',')[1];//  车速区间（止）	数字（10, 2）	km / h    是
            bo.extn05 = ((DataRow)obj)["DATA6"].ToString();//  加载功率1   数字（10, 2）	kw  是
            bo.extn06 = ((DataRow)obj)["DATA11"].ToString();//  寄生功率1   数字（10, 2）	kw  是
            bo.extn07 = ((DataRow)obj)["DATA2"].ToString();// 理论滑行时间1 数字（10, 2）	S   是
            bo.extn08 = ((DataRow)obj)["DATA3"].ToString();//  实际滑行时间1 数字（10, 2）	S   是
            bo.extn09 = ((DataRow)obj)["DATA10"].ToString();//  偏差1 数字（10, 2）	% 是
            bo.Extn10 = ((DataRow)obj)["DATA15"].ToString().Split(',')[0];//  车速区间（起）	数字（10, 2）	kw  是
            bo.extn11 = ((DataRow)obj)["DATA15"].ToString().Split(',')[1];//  车速区间（止）	数字（10, 2）	kw  是
            bo.extn12 = ((DataRow)obj)["DATA7"].ToString();//  加载功率2   数字（10, 2）	kw  是
            bo.extn13 = ((DataRow)obj)["DATA17"].ToString();//  寄生功率2   数字（10, 2）	kw  是
            bo.extn14 = ((DataRow)obj)["DATA4"].ToString();//   理论滑行时间2 数字（10, 2）	S   是
            bo.extn15 = ((DataRow)obj)["DATA5"].ToString();//   实际滑行时间2 数字（10, 2）	S   是
            bo.extn16 = ((DataRow)obj)["DATA16"].ToString();//  偏差2 数字（10, 2）	% 是
            #endregion

            dt.body.Add(bo);
            
            data.Add(dt);

            model.data = data;

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            json = json.Replace("Operator", "operator");
            FileOpreate.SaveLog( json, "[uploadInspStmDeviceRecord-测功机]：",3);
            return json;
        }
        string create五气分析仪自检(object obj)
        {

            ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem五气分析仪自检> model = new ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem五气分析仪自检>();
            model.jkid = "uploadInspStmDeviceRecord";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.StmDeviceRecordDataItem五气分析仪自检>();
            List<ModPublicJHHB.StmDeviceRecordDataItem五气分析仪自检> data = new List<ModPublicJHHB.StmDeviceRecordDataItem五气分析仪自检>();

            ModPublicJHHB.StmDeviceRecordDataItem五气分析仪自检 dt = new ModPublicJHHB.StmDeviceRecordDataItem五气分析仪自检();
            dt.body = new List<ModPublicJHHB.五气分析仪自检bodyItem>();
            ModPublicJHHB.五气分析仪自检bodyItem bo = new ModPublicJHHB.五气分析仪自检bodyItem();
            #region
            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.typecode = "1";// 运行类型    字符(2)   1：自检 是
            bo.TsNo = ModPublicJHHB.TsNo;// 检测机构编号  字符(16)      是
            bo.TestLineNo = ((DataRow)obj)["JCGWH"].ToString();//  检测线 字符(16)      是
            bo.testdevicecode = bo.TsNo + "-" + bo.TestLineNo + "-" + ((DataRow)obj)["SBBH"].ToString();// 设备编号    字符(40)  参考3.7   是
            bo.testdevicetype = DicDeviceType.GetValue("五气分析仪", "");// "五气分析仪";//  设备类型    字符(40)      是
            bo.testdevicename = "五气分析仪";//   设备名称    字符(40)      是
            bo.opcode = "2";//    操作项 字符(40)  详见数据字典8.2序24    是
            bo.Operator = ((DataRow)obj)["CZY"].ToString();//操作人 字符(40)  操作人姓名   是
            bo.reporttime = DateTime.Now.ToString("yyyyMMddHHmmss");// 上报时间    日期格式    yyyyMMddHHmmss  是
            bo.optime = Convert.ToDateTime(((DataRow)obj)["ZJSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  自检时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opstime = Convert.ToDateTime(((DataRow)obj)["KSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热开始时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opetime = Convert.ToDateTime(((DataRow)obj)["JSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热结束时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.status = "15";// 设备状态    字符(2)   详见数据字典8.2序9 是
            bo.result = ((DataRow)obj)["ZJJG"].ToString();//  运行结果    字符(1)   1：自检成功；0：自检失败   是
            bo.memo = "";//    备注  字符(255)     FALSE
            bo.exti01 = "1";// 通讯检查结果  字符(1)   1：成功 0：失败 是
            bo.exti02 = "1";// 仪器预热    字符(1)   1：成功 0：失败 是
            bo.exti03 = ((DataRow)obj)["DATA1"].ToString();// 仪器检漏    字符(1)   1：成功 0：失败 是
            bo.exti04 ="1";// 仪器调零    字符(1)   1：成功 0：失败 是
            bo.extn01 = ((DataRow)obj)["DATA2"].ToString();// 仪器流量    数字(10, 2)    L / S 是
            bo.extn02 = ((DataRow)obj)["DATA3"].ToString();//  仪器氧气 数字(10, 2)	% 是

            #endregion

            dt.body.Add(bo);

            data.Add(dt);

            model.data = data;

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            json = json.Replace("Operator", "operator");
            FileOpreate.SaveLog( json,"[uploadInspStmDeviceRecord-五气分析仪]：", 3);
            return json;
        }
        string create烟度计自检(object obj)
        {

            ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem烟度计自检> model = new ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem烟度计自检>();
            model.jkid = "uploadInspStmDeviceRecord";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.StmDeviceRecordDataItem烟度计自检>();
            List<ModPublicJHHB.StmDeviceRecordDataItem烟度计自检> data = new List<ModPublicJHHB.StmDeviceRecordDataItem烟度计自检>();

            ModPublicJHHB.StmDeviceRecordDataItem烟度计自检 dt = new ModPublicJHHB.StmDeviceRecordDataItem烟度计自检();
            dt.body = new List<ModPublicJHHB.烟度计自检bodyItem>();
            ModPublicJHHB.烟度计自检bodyItem bo = new ModPublicJHHB.烟度计自检bodyItem();
            #region
            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.typecode = "1";// 运行类型    字符(2)   1：自检 是
            bo.TsNo = ModPublicJHHB.TsNo;// 检测机构编号  字符(16)      是
            bo.TestLineNo = ((DataRow)obj)["JCGWH"].ToString();//  检测线 字符(16)      是
            bo.testdevicecode = bo.TsNo + "-" + bo.TestLineNo + "-" + ((DataRow)obj)["SBBH"].ToString();// 设备编号    字符(40)  参考3.7   是
            bo.testdevicetype = DicDeviceType.GetValue("烟度计", "");//"烟度计";//  设备类型    字符(40)      是
            bo.testdevicename = "烟度计";//   设备名称    字符(40)      是
            bo.opcode = "3";//    操作项 字符(40)  详见数据字典8.2序24    是
            bo.Operator = ((DataRow)obj)["CZY"].ToString();//操作人 字符(40)  操作人姓名   是
            bo.reporttime = DateTime.Now.ToString("yyyyMMddHHmmss");// 上报时间    日期格式    yyyyMMddHHmmss  是
            bo.optime = Convert.ToDateTime(((DataRow)obj)["ZJSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  自检时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opstime = Convert.ToDateTime(((DataRow)obj)["KSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热开始时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opetime = Convert.ToDateTime(((DataRow)obj)["JSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热结束时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.status = "15";// 设备状态    字符(2)   详见数据字典8.2序9 是
            bo.result = ((DataRow)obj)["ZJJG"].ToString();//  运行结果    字符(1)   1：自检成功；0：自检失败   是
            bo.memo = "";//    备注  字符(255)     FALSE
            bo.exti01 = "1";//  通讯检查结果  字符(1)   1：成功 0：失败 是
            bo.exti02 = "1";//  仪器预热    字符(1)   1：成功 0：失败 是
            bo.exti03 = "1";//  仪器调零    字符(1)   1：成功 0：失败 是
            bo.exti04 = ((DataRow)obj)["ZJJG"].ToString();//  量程检查    字符(1)   1：成功 0：失败 是
            bo.exti05 = "2";//  检查点数    数字(4)       是
            bo.extn01 = ((DataRow)obj)["DATA4"].ToString();//   不透光设定值1 数字(10, 2)	% 是
            bo.extn02 = ((DataRow)obj)["DATA5"].ToString();//  不透光实测值1 数字(10, 2) % 是
            bo.extn03 = ((DataRow)obj)["DATA6"].ToString();//  不透光偏差1  数字(10, 2) % 是
            bo.extn04 = ((DataRow)obj)["DATA7"].ToString();// 不透光设定值2 数字(10, 2) % 是
            bo.extn05 = ((DataRow)obj)["DATA8"].ToString();//  不透光实测值2 数字(10, 2) % 是
            bo.extn06 = ((DataRow)obj)["DATA9"].ToString();//  不透光偏差2  数字(10, 2) % 是


            #endregion

            dt.body.Add(bo);

            data.Add(dt);

            model.data = data;

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            json = json.Replace("Operator", "operator");
            FileOpreate.SaveLog(json,"[uploadInspStmDeviceRecord-烟度计]：",  3);
            return json;
        }
        string create流量计自检(object obj)
        {

            ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem流量计自检> model = new ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem流量计自检>();
            model.jkid = "uploadInspStmDeviceRecord";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.StmDeviceRecordDataItem流量计自检>();
            List<ModPublicJHHB.StmDeviceRecordDataItem流量计自检> data = new List<ModPublicJHHB.StmDeviceRecordDataItem流量计自检>();

            ModPublicJHHB.StmDeviceRecordDataItem流量计自检 dt = new ModPublicJHHB.StmDeviceRecordDataItem流量计自检();
            dt.body = new List<ModPublicJHHB.流量计自检bodyItem>();
            ModPublicJHHB.流量计自检bodyItem bo = new ModPublicJHHB.流量计自检bodyItem();
            #region
            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.typecode = "1";// 运行类型    字符(2)   1：自检 是
            bo.TsNo = ModPublicJHHB.TsNo;// 检测机构编号  字符(16)      是
            bo.TestLineNo = ((DataRow)obj)["JCGWH"].ToString();//  检测线 字符(16)      是
            bo.testdevicecode = bo.TsNo + "-" + bo.TestLineNo + "-" + ((DataRow)obj)["SBBH"].ToString();// 设备编号    字符(40)  参考3.7   是
            bo.testdevicetype = DicDeviceType.GetValue("流量计", "");// "流量计";//  设备类型    字符(40)      是
            bo.testdevicename = "流量计";//   设备名称    字符(40)      是
            bo.opcode = "6";//    操作项 字符(40)  详见数据字典8.2序24    是
            bo.Operator = ((DataRow)obj)["CZY"].ToString();//操作人 字符(40)  操作人姓名   是
            bo.reporttime = DateTime.Now.ToString("yyyyMMddHHmmss");// 上报时间    日期格式    yyyyMMddHHmmss  是
            bo.optime = Convert.ToDateTime(((DataRow)obj)["ZJSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  自检时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opstime = Convert.ToDateTime(((DataRow)obj)["KSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热开始时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opetime = Convert.ToDateTime(((DataRow)obj)["JSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热结束时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.status = "15";// 设备状态    字符(2)   详见数据字典8.2序9 是
            bo.result = ((DataRow)obj)["ZJJG"].ToString();//  运行结果    字符(1)   1：自检成功；0：自检失败   是
            bo.memo = "";//    备注  字符(255)     FALSE
            bo.exti01 = "1";// 通讯检查结果  字符(1)   1：成功 0：失败 是
            bo.exti02 = "1";// 仪器预热    字符(1)   1：成功 0：失败 是



            #endregion

            dt.body.Add(bo);

            data.Add(dt);

            model.data = data;

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            json = json.Replace("Operator", "operator");
            FileOpreate.SaveLog( json,"[uploadInspStmDeviceRecord-流量计]：", 3);
            return json;
        }
        string create转速计自检(object obj)
        {

            ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem发动机转速仪自检> model = new ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem发动机转速仪自检>();
            model.jkid = "uploadInspStmDeviceRecord";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.StmDeviceRecordDataItem发动机转速仪自检>();
            List<ModPublicJHHB.StmDeviceRecordDataItem发动机转速仪自检> data = new List<ModPublicJHHB.StmDeviceRecordDataItem发动机转速仪自检>();

            ModPublicJHHB.StmDeviceRecordDataItem发动机转速仪自检 dt = new ModPublicJHHB.StmDeviceRecordDataItem发动机转速仪自检();
            dt.body = new List<ModPublicJHHB.发动机转速仪自检bodyItem>();
            ModPublicJHHB.发动机转速仪自检bodyItem bo = new ModPublicJHHB.发动机转速仪自检bodyItem();
            #region
            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.typecode = "1";// 运行类型    字符(2)   1：自检 是
            bo.TsNo = ModPublicJHHB.TsNo;// 检测机构编号  字符(16)      是
            bo.TestLineNo = ((DataRow)obj)["JCGWH"].ToString();//  检测线 字符(16)      是
            bo.testdevicecode = bo.TsNo + "-" + bo.TestLineNo + "-" + ((DataRow)obj)["SBBH"].ToString();// 设备编号    字符(40)  参考3.7   是
            bo.testdevicetype = DicDeviceType.GetValue("发动机转速仪", "");//"发动机转速仪";//  设备类型    字符(40)      是
            bo.testdevicename = "发动机转速仪";//   设备名称    字符(40)      是
            bo.opcode = "5";//    操作项 字符(40)  详见数据字典8.2序24    是
            bo.Operator = ((DataRow)obj)["CZY"].ToString();//操作人 字符(40)  操作人姓名   是
            bo.reporttime = DateTime.Now.ToString("yyyyMMddHHmmss");// 上报时间    日期格式    yyyyMMddHHmmss  是
            bo.optime = Convert.ToDateTime(((DataRow)obj)["ZJSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  自检时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opstime = Convert.ToDateTime(((DataRow)obj)["KSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热开始时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opetime = Convert.ToDateTime(((DataRow)obj)["JSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热结束时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.status = "15";// 设备状态    字符(2)   详见数据字典8.2序9 是
            bo.result = ((DataRow)obj)["ZJJG"].ToString();//  运行结果    字符(1)   1：自检成功；0：自检失败   是
            bo.memo = "";//    备注  字符(255)     FALSE
            bo.exti01 = "1";// 通讯检查结果	字符(1)	1：成功 0：失败	是
            bo.exti02 = ((DataRow)obj)["DATA1"].ToString();// 怠速转速	字符(4)		是



            #endregion

            dt.body.Add(bo);

            data.Add(dt);

            model.data = data;

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            json = json.Replace("Operator", "operator");
            FileOpreate.SaveLog( json,"[uploadInspStmDeviceRecord-发动机转速仪]：", 3);
            return json;
        }
        string create电子环境自检(object obj)
        {

            ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem电子环境信息仪自检> model = new ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem电子环境信息仪自检>();
            model.jkid = "uploadInspStmDeviceRecord";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.StmDeviceRecordDataItem电子环境信息仪自检>();
            List<ModPublicJHHB.StmDeviceRecordDataItem电子环境信息仪自检> data = new List<ModPublicJHHB.StmDeviceRecordDataItem电子环境信息仪自检>();

            ModPublicJHHB.StmDeviceRecordDataItem电子环境信息仪自检 dt = new ModPublicJHHB.StmDeviceRecordDataItem电子环境信息仪自检();
            dt.body = new List<ModPublicJHHB.电子环境信息仪自检bodyItem>();
            ModPublicJHHB.电子环境信息仪自检bodyItem bo = new ModPublicJHHB.电子环境信息仪自检bodyItem();
            #region
            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.typecode = "1";// 运行类型    字符(2)   1：自检 是
            bo.TsNo = ModPublicJHHB.TsNo;// 检测机构编号  字符(16)      是
            bo.TestLineNo = ((DataRow)obj)["JCGWH"].ToString();//  检测线 字符(16)      是
            bo.testdevicecode = bo.TsNo + "-" + bo.TestLineNo + "-" + ((DataRow)obj)["SBBH"].ToString();// 设备编号    字符(40)  参考3.7   是
            bo.testdevicetype = DicDeviceType.GetValue("电子环境信息仪", "");//"电子环境信息仪";//  设备类型    字符(40)      是
            bo.testdevicename = "电子环境信息仪";//   设备名称    字符(40)      是
            bo.opcode = "1";//    操作项 字符(40)  详见数据字典8.2序24    是
            bo.Operator = ((DataRow)obj)["CZY"].ToString();//操作人 字符(40)  操作人姓名   是
            bo.reporttime = DateTime.Now.ToString("yyyyMMddHHmmss");// 上报时间    日期格式    yyyyMMddHHmmss  是
            bo.optime = Convert.ToDateTime(((DataRow)obj)["ZJSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  自检时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opstime = Convert.ToDateTime(((DataRow)obj)["KSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热开始时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opetime = Convert.ToDateTime(((DataRow)obj)["JSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热结束时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.status = "15";// 设备状态    字符(2)   详见数据字典8.2序9 是
            bo.result = ((DataRow)obj)["ZJJG"].ToString();//  运行结果    字符(1)   1：自检成功；0：自检失败   是
            bo.memo = "";//    备注  字符(255)     FALSE
            bo.exti01 = "1";//    通讯检查结果  字符(1)   1：成功 0：失败 是
            bo.exti02 = "1";//    仪器预热    字符(1)   1：成功 0：失败 是
            bo.extn01 = ((DataRow)obj)["DATA1"].ToString();//    环境温度    数字(10, 2)    摄氏度 是
            bo.extn02 = ((DataRow)obj)["DATA2"].ToString();//   仪器温度    数字(10, 2)    摄氏度 是
            bo.extn03 = ((DataRow)obj)["DATA3"].ToString();//    环境湿度    数字(10, 2) % 是
            bo.extn04 = ((DataRow)obj)["DATA4"].ToString();//    仪器湿度    数字(10, 2) % 是
            bo.extn05 = ((DataRow)obj)["DATA5"].ToString();//   环境气压    数字(10, 2)    kpa 是
            bo.extn06 = ((DataRow)obj)["DATA6"].ToString();//    仪器气压    数字(10, 2)    kpa 是




            #endregion

            dt.body.Add(bo);

            data.Add(dt);

            model.data = data;

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            json = json.Replace("Operator", "operator");
            FileOpreate.SaveLog( json,"[uploadInspStmDeviceRecord-电子环境信息仪]：", 3);
            return json;
        }

        string create扭力标定(object obj)
        {

            ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem扭力标定> model = new ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem扭力标定>();
            model.jkid = "uploadInspStmDeviceRecord";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.StmDeviceRecordDataItem扭力标定>();
            List<ModPublicJHHB.StmDeviceRecordDataItem扭力标定> data = new List<ModPublicJHHB.StmDeviceRecordDataItem扭力标定>();

            ModPublicJHHB.StmDeviceRecordDataItem扭力标定 dt = new ModPublicJHHB.StmDeviceRecordDataItem扭力标定();
            dt.body = new List<ModPublicJHHB.扭力标定bodyItem>();
            ModPublicJHHB.扭力标定bodyItem bo = new ModPublicJHHB.扭力标定bodyItem();
            #region
            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.typecode = "2";// 运行类型    字符(2)   1：自检 是
            bo.TsNo = ModPublicJHHB.TsNo;// 检测机构编号  字符(16)      是
            bo.TestLineNo = ((DataRow)obj)["JCGWH"].ToString();//  检测线 字符(16)      是
            bo.testdevicecode =bo.TsNo+"-"+bo.TestLineNo+"-"+ ((DataRow)obj)["SBBH"].ToString();// 设备编号    字符(40)  参考3.7   是
            bo.testdevicetype = DicDeviceType.GetValue("测功机", "");// "底盘测功机";//  设备类型    字符(40)      是
            bo.testdevicename = "底盘测功机";//   设备名称    字符(40)      是
            bo.opcode = "2";//    操作项 字符(40)  详见数据字典8.2序24    是
            bo.Operator = ((DataRow)obj)["CZY"].ToString();//操作人 字符(40)  操作人姓名   是
            bo.reporttime = DateTime.Now.ToString("yyyyMMddHHmmss");// 上报时间    日期格式    yyyyMMddHHmmss  是
            bo.optime = Convert.ToDateTime(((DataRow)obj)["BDSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  自检时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opstime = Convert.ToDateTime(((DataRow)obj)["KSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热开始时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opetime = Convert.ToDateTime(((DataRow)obj)["JSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热结束时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.status = "14";// 设备状态    字符(2)   详见数据字典8.2序9 是
            bo.result = ((DataRow)obj)["BDJG"].ToString()=="合格"?"1":"0";//  运行结果    字符(1)   1：自检成功；0：自检失败   是
            bo.memo = "";//    备注  字符(255)     FALSE
            bo.exti01 = "3";//   标定点数    字符(4)       是
            bo.extn01 = ((DataRow)obj)["DATA2"].ToString();//     设定值1 数字（10,2）	km / h    是
            bo.extn02 = ((DataRow)obj)["DATA3"].ToString();//   实测值1 数字（10,2）	km / h    是
            bo.extn03 = ((DataRow)obj)["DATA5"].ToString();//    误差1 数字（10,2）	% 是
            bo.extn04 = ((DataRow)obj)["DATA9"].ToString();//    设定值2    数字（10,2）	km / h    是
            bo.extn05 = ((DataRow)obj)["DATA10"].ToString();//    实测值2 数字（10,2）	km / h    是
            bo.extn06 = ((DataRow)obj)["DATA12"].ToString();//     误差2 数字（10,2）	% 是
            bo.extn07 = ((DataRow)obj)["DATA16"].ToString();//   设定值3    数字（10,2）	km / h    是
            bo.extn08 = ((DataRow)obj)["DATA17"].ToString();//     实测值3 数字（10,2）	km / h    是
            bo.extn09 = ((DataRow)obj)["DATA19"].ToString();//     误差3 数字（10,2）	% 是
            bo.extn10 = ((DataRow)obj)["DATA23"].ToString();//  设定值4    数字（10,2）	
            bo.extn11 = ((DataRow)obj)["DATA24"].ToString();//  实测值4    数字（10,2）	
            bo.extn12 = ((DataRow)obj)["DATA26"].ToString();//  误差4 数字（10,2）	
            bo.extn13 = ((DataRow)obj)["DATA30"].ToString();// 设定值5    数字（10,2）	
            bo.extn14 = ((DataRow)obj)["DATA31"].ToString();// 实测值5    数字（10,2）	
            bo.extn15 = ((DataRow)obj)["DATA33"].ToString();//  误差5 数字（10,2）	




            #endregion

            dt.body.Add(bo);

            data.Add(dt);

            model.data = data;

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            json = json.Replace("Operator", "operator");
            FileOpreate.SaveLog( json,"[uploadInspStmDeviceRecord-扭力标定]：", 3);
            return json;
        }
        string create车速标定(object obj)
        {

            ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem车速标定> model = new ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem车速标定>();
            model.jkid = "uploadInspStmDeviceRecord";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.StmDeviceRecordDataItem车速标定>();
            List<ModPublicJHHB.StmDeviceRecordDataItem车速标定> data = new List<ModPublicJHHB.StmDeviceRecordDataItem车速标定>();

            ModPublicJHHB.StmDeviceRecordDataItem车速标定 dt = new ModPublicJHHB.StmDeviceRecordDataItem车速标定();
            dt.body = new List<ModPublicJHHB.车速标定bodyItem>();
            ModPublicJHHB.车速标定bodyItem bo = new ModPublicJHHB.车速标定bodyItem();
            #region
            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.typecode = "2";// 运行类型    字符(2)   1：自检 是
            bo.TsNo = ModPublicJHHB.TsNo;// 检测机构编号  字符(16)      是
            bo.TestLineNo = ((DataRow)obj)["JCGWH"].ToString();//  检测线 字符(16)      是
            bo.testdevicecode = bo.TsNo + "-" + bo.TestLineNo + "-" + ((DataRow)obj)["SBBH"].ToString();// 设备编号    字符(40)  参考3.7   是
            bo.testdevicetype = DicDeviceType.GetValue("测功机", "");//"底盘测功机";//  设备类型    字符(40)      是
            bo.testdevicename = "底盘测功机";//   设备名称    字符(40)      是
            bo.opcode = "1";//    操作项 字符(40)  详见数据字典8.2序24    是
            bo.Operator = ((DataRow)obj)["CZY"].ToString();//操作人 字符(40)  操作人姓名   是
            bo.reporttime = DateTime.Now.ToString("yyyyMMddHHmmss");// 上报时间    日期格式    yyyyMMddHHmmss  是
            bo.optime = Convert.ToDateTime(((DataRow)obj)["BDSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  自检时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opstime = Convert.ToDateTime(((DataRow)obj)["KSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热开始时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opetime = Convert.ToDateTime(((DataRow)obj)["JSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热结束时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.status = "14";// 设备状态    字符(2)   详见数据字典8.2序9 是
            bo.result = ((DataRow)obj)["BDJG"].ToString()=="合格"?"1":"0";//  运行结果    字符(1)   1：自检成功；0：自检失败   是
            bo.memo = "";//    备注  字符(255)     FALSE
            bo.exti01 = "3";//   标定点数    字符(4)       是
            bo.extn01 = ((DataRow)obj)["DATA2"].ToString();//     设定值1 数字（10,2）	km / h    是
            bo.extn02 = ((DataRow)obj)["DATA3"].ToString();//   实测值1 数字（10,2）	km / h    是
            bo.extn03 = ((DataRow)obj)["DATA5"].ToString();//    误差1 数字（10,2）	% 是
            bo.extn04 = ((DataRow)obj)["DATA9"].ToString();//    设定值2    数字（10,2）	km / h    是
            bo.extn05 = ((DataRow)obj)["DATA10"].ToString();//    实测值2 数字（10,2）	km / h    是
            bo.extn06 = ((DataRow)obj)["DATA12"].ToString();//     误差2 数字（10,2）	% 是
            bo.extn07 = ((DataRow)obj)["DATA16"].ToString();//   设定值3    数字（10,2）	km / h    是
            bo.extn08 = ((DataRow)obj)["DATA17"].ToString();//     实测值3 数字（10,2）	km / h    是
            bo.extn09 = ((DataRow)obj)["DATA19"].ToString();//     误差3 数字（10,2）	% 是




            #endregion

            dt.body.Add(bo);

            data.Add(dt);

            model.data = data;

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            json = json.Replace("Operator", "operator");
            FileOpreate.SaveLog( json, "[uploadInspStmDeviceRecord-车速标定]：",3);
            return json;
        }
        string create寄生功率标定(object obj)
        {

            ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem寄生功率标定> model = new ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem寄生功率标定>();
            model.jkid = "uploadInspStmDeviceRecord";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.StmDeviceRecordDataItem寄生功率标定>();
            List<ModPublicJHHB.StmDeviceRecordDataItem寄生功率标定> data = new List<ModPublicJHHB.StmDeviceRecordDataItem寄生功率标定>();

            ModPublicJHHB.StmDeviceRecordDataItem寄生功率标定 dt = new ModPublicJHHB.StmDeviceRecordDataItem寄生功率标定();
            dt.body = new List<ModPublicJHHB.寄生功率标定bodyItem>();
            ModPublicJHHB.寄生功率标定bodyItem bo = new ModPublicJHHB.寄生功率标定bodyItem();
            #region
            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.typecode = "2";// 运行类型    字符(2)   1：自检 是
            bo.TsNo = ModPublicJHHB.TsNo;// 检测机构编号  字符(16)      是
            bo.TestLineNo = ((DataRow)obj)["JCGWH"].ToString();//  检测线 字符(16)      是
            bo.testdevicecode = bo.TsNo + "-" + bo.TestLineNo + "-" + ((DataRow)obj)["SBBH"].ToString();// 设备编号    字符(40)  参考3.7   是
            bo.testdevicetype = DicDeviceType.GetValue("测功机", "");// "底盘测功机";//  设备类型    字符(40)      是
            bo.testdevicename = "底盘测功机";//   设备名称    字符(40)      是
            bo.opcode = "3";//    操作项 字符(40)  详见数据字典8.2序24    是
            bo.Operator = ((DataRow)obj)["CZY"].ToString();//操作人 字符(40)  操作人姓名   是
            bo.reporttime = DateTime.Now.ToString("yyyyMMddHHmmss");// 上报时间    日期格式    yyyyMMddHHmmss  是
            bo.optime = Convert.ToDateTime(((DataRow)obj)["BDSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  自检时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opstime = Convert.ToDateTime(((DataRow)obj)["KSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热开始时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opetime = Convert.ToDateTime(((DataRow)obj)["JSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热结束时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.status = "14";// 设备状态    字符(2)   详见数据字典8.2序9 是
            bo.result = ((DataRow)obj)["BDJG"].ToString()=="合格"?"1":"0";//  运行结果    字符(1)   1：自检成功；0：自检失败   是
            bo.memo = "";//    备注  字符(255)     FALSE
            bo.extnum = ((DataRow)obj)["DATA11"].ToString();//惯性当量	数字（10,2）	Kg
            bo.exti01 = "5";//   标定点数    字符(4)       是
            bo.extn01 = ((DataRow)obj)["DATA2"].ToString().Split(',')[0].Split('-')[0];// 车速区间（起）	数字（10,2）	km / h    是
            bo.extn02 = ((DataRow)obj)["DATA2"].ToString().Split(',')[0].Split('-')[1];// 车速区间（止）	数字（10,2）	km / h    是
            bo.extn03 = ((DataRow)obj)["DATA4"].ToString().Split(',')[0];//  滑行时间1 数字（10,2）	s 是
            bo.extn04 = ((DataRow)obj)["DATA5"].ToString().Split(',')[0];//寄生功率1   数字（10,2）	kw 是
            bo.extn05 = ((DataRow)obj)["DATA2"].ToString().Split(',')[1].Split('-')[0];//车速区间2（起）	数字（10,2）	km / h    是
            bo.extn06 = ((DataRow)obj)["DATA2"].ToString().Split(',')[1].Split('-')[1];//  车速区间2（止）	数字（10,2）	km / h    是
            bo.extn07 = ((DataRow)obj)["DATA4"].ToString().Split(',')[1];// 滑行时间2 数字（10,2）	s 是
            bo.extn08 = ((DataRow)obj)["DATA5"].ToString().Split(',')[1];// 寄生功率2   数字（10,2）	kw 是
            bo.extn09 = ((DataRow)obj)["DATA2"].ToString().Split(',')[2].Split('-')[0];// 车速区间3（起）	数字（10,2）	km / h    是
            bo.extn10 = ((DataRow)obj)["DATA2"].ToString().Split(',')[2].Split('-')[1];//  车速区间3（止）	数字（10,2）	km / h    是
            bo.extn11 = ((DataRow)obj)["DATA4"].ToString().Split(',')[2];//  滑行时间3 数字（10,2）	s 是
            bo.extn12 = ((DataRow)obj)["DATA5"].ToString().Split(',')[2];//寄生功率3   数字（10,2）	kw 是
            bo.extn13 = ((DataRow)obj)["DATA2"].ToString().Split(',')[3].Split('-')[0];//车速区间4（起）	数字（10,2）	km / h    是
            bo.extn14 = ((DataRow)obj)["DATA2"].ToString().Split(',')[3].Split('-')[1];// 车速区间4（止）	数字（10,2）	km / h    是
            bo.extn15 = ((DataRow)obj)["DATA4"].ToString().Split(',')[3];// 滑行时间4 数字（10,2）	s 是
            bo.extn16 = ((DataRow)obj)["DATA5"].ToString().Split(',')[3];// 寄生功率4   数字（10,2）	kw 是
            bo.extn17 = ((DataRow)obj)["DATA2"].ToString().Split(',')[4].Split('-')[0];// 车速区间5（起）	数字（10,2）	km / h    是
            bo.extn18 = ((DataRow)obj)["DATA2"].ToString().Split(',')[4].Split('-')[1];//  车速区间5（止）	数字（10,2）	km / h    是
            bo.extn19 = ((DataRow)obj)["DATA4"].ToString().Split(',')[4];// 滑行时间5 数字（10,2）	s 是
            bo.extn20 = ((DataRow)obj)["DATA5"].ToString().Split(',')[4];// 寄生功率5   数字（10,2）	kw 是
            #endregion

            dt.body.Add(bo);

            data.Add(dt);

            model.data = data;

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            json = json.Replace("Operator", "operator");
            FileOpreate.SaveLog(json,"[uploadInspStmDeviceRecord-寄生功率标定]：",  3);
            return json;
        }
        string create加载滑行标定(object obj)
        {

            ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem加载滑行率标定> model = new ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem加载滑行率标定>();
            model.jkid = "uploadInspStmDeviceRecord";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.StmDeviceRecordDataItem加载滑行率标定>();
            List<ModPublicJHHB.StmDeviceRecordDataItem加载滑行率标定> data = new List<ModPublicJHHB.StmDeviceRecordDataItem加载滑行率标定>();

            ModPublicJHHB.StmDeviceRecordDataItem加载滑行率标定 dt = new ModPublicJHHB.StmDeviceRecordDataItem加载滑行率标定();
            dt.body = new List<ModPublicJHHB.加载滑行率标定bodyItem>();
            ModPublicJHHB.加载滑行率标定bodyItem bo = new ModPublicJHHB.加载滑行率标定bodyItem();
            #region
            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.typecode = "2";// 运行类型    字符(2)   1：自检 是
            bo.TsNo = ModPublicJHHB.TsNo;// 检测机构编号  字符(16)      是
            bo.TestLineNo = ((DataRow)obj)["JCGWH"].ToString();//  检测线 字符(16)      是
            bo.testdevicecode = bo.TsNo + "-" + bo.TestLineNo + "-" + ((DataRow)obj)["SBBH"].ToString();// 设备编号    字符(40)  参考3.7   是
            bo.testdevicetype = DicDeviceType.GetValue("测功机", "");// "底盘测功机";//  设备类型    字符(40)      是
            bo.testdevicename = "底盘测功机";//   设备名称    字符(40)      是
            bo.opcode = "4";//    操作项 字符(40)  详见数据字典8.2序24    是
            bo.Operator = ((DataRow)obj)["CZY"].ToString();//操作人 字符(40)  操作人姓名   是
            bo.reporttime = DateTime.Now.ToString("yyyyMMddHHmmss");// 上报时间    日期格式    yyyyMMddHHmmss  是
            bo.optime = Convert.ToDateTime(((DataRow)obj)["BDSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  自检时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opstime = Convert.ToDateTime(((DataRow)obj)["KSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热开始时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opetime = Convert.ToDateTime(((DataRow)obj)["JSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热结束时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.status = "14";// 设备状态    字符(2)   详见数据字典8.2序9 是
            bo.result = ((DataRow)obj)["BDJG"].ToString() == "合格" ? "1" : "0";//  运行结果    字符(1)   1：自检成功；0：自检失败   是
            bo.memo = "";//    备注  字符(255)     FALSE
            bo.extnum = ((DataRow)obj)["DATA25"].ToString();//惯性当量	数字（10,2）	Kg
            bo.exti01 = "2";//   标定点数    字符(4)       是
            bo.extn01 = ((DataRow)obj)["DATA2"].ToString().Split(',')[0].Split('-')[0];//  车速区间1（起）	数字（10,2）	km / h    是
            bo.extn02 = ((DataRow)obj)["DATA2"].ToString().Split(',')[0].Split('-')[1];//  车速区间1（止）	数字（10,2）	km / h    是
            bo.extn03 = ((DataRow)obj)["DATA5"].ToString().Split(',')[0];//   加载功率1 数字（10,2）	kw 是
            bo.extn04 = ((DataRow)obj)["DATA4"].ToString().Split(',')[0];//  寄生功率1   数字（10,2）	kw 是
            bo.extn05 = ((DataRow)obj)["DATA7"].ToString().Split(',')[0];//  理论滑行时间1 数字（10,2）	S 是
            bo.extn06 = ((DataRow)obj)["DATA6"].ToString().Split(',')[0];//  实际滑行时间1 数字（10,2）	S 是
            bo.extn07 = ((DataRow)obj)["DATA8"].ToString().Split(',')[0];//  偏差1 数字（10,2）	% 是
            bo.extn08 = ((DataRow)obj)["DATA2"].ToString().Split(',')[1].Split('-')[0];//  车速区间2（起）	数字（10,2）	km / h    是
            bo.extn09 = ((DataRow)obj)["DATA2"].ToString().Split(',')[1].Split('-')[1];//  车速区间2（止）	数字（10,2）	km / h    是
            bo.extn10 = ((DataRow)obj)["DATA5"].ToString().Split(',')[1];//   加载功率2 数字（10,2）	kw 是
            bo.extn11 = ((DataRow)obj)["DATA4"].ToString().Split(',')[1];//  寄生功率2   数字（10,2）	kw 是
            bo.extn12 = ((DataRow)obj)["DATA5"].ToString().Split(',')[1];//  理论滑行时间2 数字（10,2）	s 是
            bo.extn13 = ((DataRow)obj)["DATA6"].ToString().Split(',')[1];// 实际滑行时间2 数字（10,2）	s 是
            bo.extn14 = ((DataRow)obj)["DATA8"].ToString().Split(',')[1];//  误差2 数字（10,2）		是






            #endregion

            dt.body.Add(bo);

            data.Add(dt);

            model.data = data;

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            json = json.Replace("Operator", "operator");
            FileOpreate.SaveLog(json,"[uploadInspStmDeviceRecord-加载滑行标定]：",  3);
            return json;
        }
        string create废气仪标定(object obj)
        {

            ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem废气标定> model = new ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem废气标定>();
            model.jkid = "uploadInspStmDeviceRecord";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.StmDeviceRecordDataItem废气标定>();
            List<ModPublicJHHB.StmDeviceRecordDataItem废气标定> data = new List<ModPublicJHHB.StmDeviceRecordDataItem废气标定>();

            ModPublicJHHB.StmDeviceRecordDataItem废气标定 dt = new ModPublicJHHB.StmDeviceRecordDataItem废气标定();
            dt.body = new List<ModPublicJHHB.废气标定bodyItem>();
            ModPublicJHHB.废气标定bodyItem bo = new ModPublicJHHB.废气标定bodyItem();
            #region
            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.typecode = "2";// 运行类型    字符(2)   1：自检 是
            bo.TsNo = ModPublicJHHB.TsNo;// 检测机构编号  字符(16)      是
            bo.TestLineNo = ((DataRow)obj)["JCGWH"].ToString();//  检测线 字符(16)      是
            bo.testdevicecode = bo.TsNo + "-" + bo.TestLineNo + "-" + ((DataRow)obj)["SBBH"].ToString();// 设备编号    字符(40)  参考3.7   是
            bo.testdevicetype = DicDeviceType.GetValue("五气分析仪", "");// "底盘测功机";//  设备类型    字符(40)      是
            bo.testdevicename = "五气分析仪";//   设备名称    字符(40)      是
            bo.opcode = "5";//    操作项 字符(40)  详见数据字典8.2序24    是
            bo.Operator = ((DataRow)obj)["CZY"].ToString();//操作人 字符(40)  操作人姓名   是
            bo.reporttime = DateTime.Now.ToString("yyyyMMddHHmmss");// 上报时间    日期格式    yyyyMMddHHmmss  是
            bo.optime = Convert.ToDateTime(((DataRow)obj)["BDSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  自检时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opstime = Convert.ToDateTime(((DataRow)obj)["KSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热开始时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opetime = Convert.ToDateTime(((DataRow)obj)["JSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热结束时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.status = "14";// 设备状态    字符(2)   详见数据字典8.2序9 是
            bo.result = ((DataRow)obj)["BDJG"].ToString() == "合格" ? "1" : "0";//  运行结果    字符(1)   1：自检成功；0：自检失败   是
            bo.memo = "";//    备注  字符(255)     FALSE
            bo.extnum = "0";// 标定气浓度   数字（10,2）		
            bo.exti01 ="4";// 标定点数    字符(4)   第N次标定点数，第二次为一个新记录 是
            bo.extn01 = ((DataRow)obj)["DATA6"].ToString();// HC设定值1  数字（10,2）		是
            bo.extn02 = ((DataRow)obj)["DATA7"].ToString();//  HC实测值1 数字（10,2）		是
            bo.extn03 = ((DataRow)obj)["DATA8"].ToString();//  HC偏差1 数字（10,2）	% 是
            bo.extn04 = ((DataRow)obj)["DATA20"].ToString();// CO设定值1  数字（10,2）		是
            bo.extn05 = ((DataRow)obj)["DATA21"].ToString();// CO实测值1 数字（10,2）		是
            bo.extn06 = ((DataRow)obj)["DATA22"].ToString();//  CO偏差1 数字（10,2）	% 是
            bo.extn07 = ((DataRow)obj)["DATA27"].ToString();//CO2设定值1 数字（10,2）		是
            bo.extn08 = ((DataRow)obj)["DATA28"].ToString();// CO2实测值1 数字（10,2）		是
            bo.extn09 = ((DataRow)obj)["DATA29"].ToString();// CO2偏差1 数字（10,2）	% 是
            bo.extn10 = ((DataRow)obj)["DATA14"].ToString();// NO设定值1  数字（10,2）		是
            bo.extn11 = ((DataRow)obj)["DATA15"].ToString();// NO实测值1 数字（10,2）		是
            bo.extn12 = ((DataRow)obj)["DATA16"].ToString();//  NO偏差1 数字（10,2）	% 是
            bo.extn13 = "0";// HC设定值2  数字（10,2）		是
            bo.extn14 = "0";//  HC实测值2 数字（10,2）		是
            bo.extn15 = "0";// HC偏差2 数字（10,2）	% 是
            #endregion

            dt.body.Add(bo);
            data.Add(dt);
            model.data = data;
            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            json = json.Replace("Operator", "operator");
            FileOpreate.SaveLog(json, "[uploadInspStmDeviceRecord-废气标定]：", 3);
            return json;
        }
        string create烟度标定(object obj)
        {

            ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem烟度标定> model = new ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem烟度标定>();
            model.jkid = "uploadInspStmDeviceRecord";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.StmDeviceRecordDataItem烟度标定>();
            List<ModPublicJHHB.StmDeviceRecordDataItem烟度标定> data = new List<ModPublicJHHB.StmDeviceRecordDataItem烟度标定>();

            ModPublicJHHB.StmDeviceRecordDataItem烟度标定 dt = new ModPublicJHHB.StmDeviceRecordDataItem烟度标定();
            dt.body = new List<ModPublicJHHB.烟度标定bodyItem>();
            ModPublicJHHB.烟度标定bodyItem bo = new ModPublicJHHB.烟度标定bodyItem();
            #region
            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.typecode = "2";// 运行类型    字符(2)   1：自检 是
            bo.TsNo = ModPublicJHHB.TsNo;// 检测机构编号  字符(16)      是
            bo.TestLineNo = ((DataRow)obj)["JCGWH"].ToString();//  检测线 字符(16)      是
            bo.testdevicecode = bo.TsNo + "-" + bo.TestLineNo + "-" + ((DataRow)obj)["SBBH"].ToString();// 设备编号    字符(40)  参考3.7   是
            bo.testdevicetype = DicDeviceType.GetValue("烟度计", "");// "底盘测功机";//  设备类型    字符(40)      是
            bo.testdevicename = "烟度计";//   设备名称    字符(40)      是
            bo.opcode = "6";//    操作项 字符(40)  详见数据字典8.2序24    是
            bo.Operator = ((DataRow)obj)["CZY"].ToString();//操作人 字符(40)  操作人姓名   是
            bo.reporttime = DateTime.Now.ToString("yyyyMMddHHmmss");// 上报时间    日期格式    yyyyMMddHHmmss  是
            bo.optime = Convert.ToDateTime(((DataRow)obj)["BDSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  自检时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opstime = Convert.ToDateTime(((DataRow)obj)["KSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热开始时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opetime = Convert.ToDateTime(((DataRow)obj)["JSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热结束时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.status = "14";// 设备状态    字符(2)   详见数据字典8.2序9 是
            bo.result = ((DataRow)obj)["BDJG"].ToString() == "合格" ? "1" : "0";//  运行结果    字符(1)   1：自检成功；0：自检失败   是
            bo.memo = "";//    备注  字符(255)     FALSE
            bo.exti01 = "1";// 标定点数    字符(4)       是
            bo.extn01 = ((DataRow)obj)["DATA1"].ToString();// 不透光设定值1 数字（10,2）	% 是
            bo.extn02 = ((DataRow)obj)["DATA2"].ToString();// 不透光实测值1 数字（10,2）	% 是
            bo.extn03 = ((DataRow)obj)["DATA3"].ToString();//不透光偏差1  数字（10,2）	% 是
            bo.extn04 = ((DataRow)obj)["DATA1"].ToString();// 光吸收数设定值1    数字（10,2）	m 是
            bo.extn05 = ((DataRow)obj)["DATA2"].ToString();// 光吸收数实测值1    数字（10,2）	m 是
            bo.extn06 = ((DataRow)obj)["DATA3"].ToString();// 光吸收数偏差1 数字（10,2）	% 是
            bo.extn07 = ((DataRow)obj)["DATA6"].ToString();// 不透光设定值2 数字（10,2）		是
            bo.extn08 = ((DataRow)obj)["DATA7"].ToString();// 不透光实测值2 数字（10,2）		是
            bo.extn09 = ((DataRow)obj)["DATA8"].ToString();// 不透光偏差2 数字（10,2）	% 是
            bo.extn10 = ((DataRow)obj)["DATA6"].ToString();//光吸收数设定2 数字（10,2）		是
            bo.extn11 = ((DataRow)obj)["DATA7"].ToString();// 光吸收数实测2 数字（10,2）		是
            bo.extn12 = ((DataRow)obj)["DATA8"].ToString();//光吸收偏差2 数字（10,2）		是

            #endregion

            dt.body.Add(bo);
            data.Add(dt);
            model.data = data;
            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            json = json.Replace("Operator", "operator");
            FileOpreate.SaveLog( json,"[uploadInspStmDeviceRecord-烟度标定]：", 3);
            return json;
        }
        string create加载滑行检查(object obj)
        {

            ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem加载滑行检查> model = new ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem加载滑行检查>();
            model.jkid = "uploadInspStmDeviceRecord";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.StmDeviceRecordDataItem加载滑行检查>();
            List<ModPublicJHHB.StmDeviceRecordDataItem加载滑行检查> data = new List<ModPublicJHHB.StmDeviceRecordDataItem加载滑行检查>();

            ModPublicJHHB.StmDeviceRecordDataItem加载滑行检查 dt = new ModPublicJHHB.StmDeviceRecordDataItem加载滑行检查();
            dt.body = new List<ModPublicJHHB.加载滑行检查bodyItem>();
            ModPublicJHHB.加载滑行检查bodyItem bo = new ModPublicJHHB.加载滑行检查bodyItem();
            #region
            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.typecode = "4";// 运行类型    字符(2)   1：自检 是
            bo.TsNo = ModPublicJHHB.TsNo;// 检测机构编号  字符(16)      是
            bo.TestLineNo = ((DataRow)obj)["JCGWH"].ToString();//  检测线 字符(16)      是
            bo.testdevicecode = bo.TsNo + "-" + bo.TestLineNo + "-" + ((DataRow)obj)["SBBH"].ToString();// 设备编号    字符(40)  参考3.7   是
            bo.testdevicetype = DicDeviceType.GetValue("测功机", "");//  设备类型    字符(40)      是
            bo.testdevicename = "底盘测功机";//   设备名称    字符(40)      是
            bo.opcode = "11";//    操作项 字符(40)  详见数据字典8.2序24    是
            bo.Operator = ((DataRow)obj)["CZY"].ToString();//操作人 字符(40)  操作人姓名   是
            bo.reporttime = DateTime.Now.ToString("yyyyMMddHHmmss");// 上报时间    日期格式    yyyyMMddHHmmss  是
            bo.optime = Convert.ToDateTime(((DataRow)obj)["ZJSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  自检时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opstime = Convert.ToDateTime(((DataRow)obj)["KSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热开始时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opetime = Convert.ToDateTime(((DataRow)obj)["JSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热结束时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.status = "15";// 设备状态    字符(2)   详见数据字典8.2序9 是
            bo.result = ((DataRow)obj)["ZJJG"].ToString();//  运行结果    字符(1)   1：自检成功；0：自检失败   是
            bo.memo = "";//    备注  字符(255)     FALSE
            bo.extnum = ((DataRow)obj)["DATA8"].ToString();// 基本惯性    数字(10, 2)    Kg 是
            bo.exti01 = Convert.ToDouble(((DataRow)obj)["DATA10"].ToString())>7?"0":"1";//  48 - 32km / h滑行检查结果 字符(1)   0 - 不合格、1 - 合格  是
            bo.exti02 = Convert.ToDouble(((DataRow)obj)["DATA16"].ToString()) > 7 ? "0" : "1";// 32 - 16km / h滑行检查结果 字符(1)   0 - 不合格、1 - 合格  是
            bo.extn01 = ((DataRow)obj)["DATA3"].ToString();//  48 - 32km / h实际滑行时间 数字(10, 2)    ACDT40，ms 是
            bo.extn02 = ((DataRow)obj)["DATA5"].ToString();//  32 - 16km / h实际滑行时间 数字(10, 2)    ACDT25，ms 是
            bo.extn03 = ((DataRow)obj)["DATA11"].ToString();// 40km / h时的内损  数字(10, 2)    PLHP40，kW 是
            bo.extn04 = ((DataRow)obj)["DATA17"].ToString();//  25km / h时的内损  数字(10, 2)    PLHP25，kW 是
            bo.extn05 = ((DataRow)obj)["DATA2"].ToString();//  48 - 32km / h名义滑行时间 数字(10, 2)    CCDT40，ms 是
            bo.extn06 = ((DataRow)obj)["DATA4"].ToString();// 32 - 16km / h名义滑行时间 数字(10, 2)    CCDT25，ms 是
            bo.extn07 = ((DataRow)obj)["DATA6"].ToString();// 48 - 32km / h滑行指示功率 数字(10, 2)    IHP40，kW 是
            bo.extn08 = ((DataRow)obj)["DATA7"].ToString();// 32 - 16km / h滑行指示功率 数字(10, 2)    IHP25，kW 是

            #endregion

            dt.body.Add(bo);

            data.Add(dt);

            model.data = data;

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            json = json.Replace("Operator", "operator");
            FileOpreate.SaveLog(json, "[uploadInspStmDeviceRecord-加载滑行检查]：", 3);
            return json;
        }
        string create附加功率损失检查(object obj)
        {

            ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem附加功率损失检查> model = new ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem附加功率损失检查>();
            model.jkid = "uploadInspStmDeviceRecord";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.StmDeviceRecordDataItem附加功率损失检查>();
            List<ModPublicJHHB.StmDeviceRecordDataItem附加功率损失检查> data = new List<ModPublicJHHB.StmDeviceRecordDataItem附加功率损失检查>();

            ModPublicJHHB.StmDeviceRecordDataItem附加功率损失检查 dt = new ModPublicJHHB.StmDeviceRecordDataItem附加功率损失检查();
            dt.body = new List<ModPublicJHHB.附加功率损失检查bodyItem>();
            ModPublicJHHB.附加功率损失检查bodyItem bo = new ModPublicJHHB.附加功率损失检查bodyItem();
            #region
            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.typecode = "4";// 运行类型    字符(2)   1：自检 是
            bo.TsNo = ModPublicJHHB.TsNo;// 检测机构编号  字符(16)      是
            bo.TestLineNo = ((DataRow)obj)["JCGWH"].ToString();//  检测线 字符(16)      是
            bo.testdevicecode = bo.TsNo + "-" + bo.TestLineNo + "-" + ((DataRow)obj)["SBBH"].ToString();// 设备编号    字符(40)  参考3.7   是
            bo.testdevicetype = DicDeviceType.GetValue("测功机", "");//  设备类型    字符(40)      是
            bo.testdevicename = "底盘测功机";//   设备名称    字符(40)      是
            bo.opcode = "12";//    操作项 字符(40)  详见数据字典8.2序24    是
            bo.Operator = ((DataRow)obj)["CZY"].ToString();//操作人 字符(40)  操作人姓名   是
            bo.reporttime = DateTime.Now.ToString("yyyyMMddHHmmss");// 上报时间    日期格式    yyyyMMddHHmmss  是
            bo.optime = Convert.ToDateTime(((DataRow)obj)["ZJSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  自检时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opstime = Convert.ToDateTime(((DataRow)obj)["KSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热开始时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opetime = Convert.ToDateTime(((DataRow)obj)["JSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热结束时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.status = "15";// 设备状态    字符(2)   详见数据字典8.2序9 是
            bo.result = ((DataRow)obj)["ZJJG"].ToString();//  运行结果    字符(1)   1：自检成功；0：自检失败   是
            bo.memo = "";//    备注  字符(255)     FALSE
            bo.extnum = ((DataRow)obj)["DATA34"].ToString();// 基本惯性    数字(10, 2)    Kg 是
            bo.extn01 = ((DataRow)obj)["DATA15"].ToString();//  48 - 32km / h实际滑行时间 字符(1)   ACDT40，ms 是
            bo.extn02 = ((DataRow)obj)["DATA25"].ToString();// 32 - 16km / h实际滑行时间 字符(1)   ACDT25，ms 是
            bo.extn03 = ((DataRow)obj)["DATA16"].ToString();//  40km / h时的内损  数字(10, 2)    PLHP40，kW 是
            bo.extn04 = ((DataRow)obj)["DATA26"].ToString();//  25km / h时的内损  数字(10, 2)    PLHP25，kW 是


            #endregion

            dt.body.Add(bo);

            data.Add(dt);

            model.data = data;

            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            json = json.Replace("Operator", "operator");
            FileOpreate.SaveLog( json,"[uploadInspStmDeviceRecord-附加功率损失检查]：", 3);
            return json;
        }
        string create分析仪检查(object obj)
        {

            ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem分析仪检查> model = new ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem分析仪检查>();
            model.jkid = "uploadInspStmDeviceRecord";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.StmDeviceRecordDataItem分析仪检查>();
            List<ModPublicJHHB.StmDeviceRecordDataItem分析仪检查> data = new List<ModPublicJHHB.StmDeviceRecordDataItem分析仪检查>();

            ModPublicJHHB.StmDeviceRecordDataItem分析仪检查 dt = new ModPublicJHHB.StmDeviceRecordDataItem分析仪检查();
            dt.body = new List<ModPublicJHHB.分析仪检查bodyItem>();
            ModPublicJHHB.分析仪检查bodyItem bo = new ModPublicJHHB.分析仪检查bodyItem();
            #region
            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.typecode = "4";// 运行类型    字符(2)   1：自检 是
            bo.TsNo = ModPublicJHHB.TsNo;// 检测机构编号  字符(16)      是
            bo.TestLineNo = ((DataRow)obj)["JCGWH"].ToString();//  检测线 字符(16)      是
            bo.testdevicecode = bo.TsNo + "-" + bo.TestLineNo + "-" + ((DataRow)obj)["SBBH"].ToString();// 设备编号    字符(40)  参考3.7   是
            bo.testdevicetype = DicDeviceType.GetValue("五气分析仪", "");// "底盘测功机";//  设备类型    字符(40)      是
            bo.testdevicename = "五气分析仪";//   设备名称    字符(40)      是
            bo.opcode = "13";//    操作项 字符(40)  详见数据字典8.2序24    是
            bo.Operator = ((DataRow)obj)["CZY"].ToString();//操作人 字符(40)  操作人姓名   是
            bo.reporttime = DateTime.Now.ToString("yyyyMMddHHmmss");// 上报时间    日期格式    yyyyMMddHHmmss  是
            bo.optime = Convert.ToDateTime(((DataRow)obj)["ZJSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  自检时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opstime = Convert.ToDateTime(((DataRow)obj)["KSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热开始时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opetime = Convert.ToDateTime(((DataRow)obj)["JSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热结束时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.status = "14";// 设备状态    字符(2)   详见数据字典8.2序9 是
            bo.result = ((DataRow)obj)["ZJJG"].ToString();//  运行结果    字符(1)   1：自检成功；0：自检失败   是
            bo.memo = "";//    备注  字符(255)     FALSE
            bo.extnum = "1";// 标定气浓度   数字（10,2）		
            //bo.exti01 = "1";// 标定点数    字符(4)   第N次标定点数，第二次为一个新记录 是
            bo.extn01 = ((DataRow)obj)["DATA2"].ToString();//  标准气C3H8浓度   数字(10, 2)    10 - 6    是
            bo.extn02 = ((DataRow)obj)["DATA20"].ToString();//   标准气CO浓度 数字(10, 2)	% 是
            bo.extn03 = ((DataRow)obj)["DATA27"].ToString();//  标准气CO2浓度    数字(10, 2) % 是
            bo.extn04 = ((DataRow)obj)["DATA13"].ToString();//  标准气NO浓度 数字(10, 2)    10 - 6    是
            bo.extn05 = "0";//  标准气O2浓度 数字(10, 2)	% 是
            bo.extn06 = ((DataRow)obj)["DATA3"].ToString();// HC检查结果值 数字(10, 2)    10 - 6    是
            bo.extn07 = ((DataRow)obj)["DATA21"].ToString();//  CO检查结果值 数字(10, 2)	% 是
            bo.extn08 = ((DataRow)obj)["DATA28"].ToString();//  CO2检查结果值    数字(10, 2) % 是
            bo.extn09 = ((DataRow)obj)["DATA14"].ToString();// NO检查结果值 数字(10, 2)    10 - 6    是
            bo.extn10 = "0";//  O2检查结果值 数字(10, 2)	% 是
            bo.extn11 = ((DataRow)obj)["DATA3"].ToString();// PEF值    数字(10, 2)        是

            //test
            #endregion

            dt.body.Add(bo);
            data.Add(dt);
            model.data = data;
            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            json = json.Replace("Operator", "operator");
            FileOpreate.SaveLog(json,"[uploadInspStmDeviceRecord-分析仪检查]：",  3);
            return json;
        }
        string create泄漏检查(object obj)
        {

            ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem泄露检查> model = new ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem泄露检查>();
            model.jkid = "uploadInspStmDeviceRecord";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.StmDeviceRecordDataItem泄露检查>();
            List<ModPublicJHHB.StmDeviceRecordDataItem泄露检查> data = new List<ModPublicJHHB.StmDeviceRecordDataItem泄露检查>();

            ModPublicJHHB.StmDeviceRecordDataItem泄露检查 dt = new ModPublicJHHB.StmDeviceRecordDataItem泄露检查();
            dt.body = new List<ModPublicJHHB.泄露检查bodyItem>();
            ModPublicJHHB.泄露检查bodyItem bo = new ModPublicJHHB.泄露检查bodyItem();
            #region
            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.typecode = "4";// 运行类型    字符(2)   1：自检 是
            bo.TsNo = ModPublicJHHB.TsNo;// 检测机构编号  字符(16)      是
            bo.TestLineNo = ((DataRow)obj)["JCGWH"].ToString();//  检测线 字符(16)      是
            bo.testdevicecode = bo.TsNo + "-" + bo.TestLineNo + "-" + ((DataRow)obj)["SBBH"].ToString();// 设备编号    字符(40)  参考3.7   是
            bo.testdevicetype = DicDeviceType.GetValue("五气分析仪", "");// "底盘测功机";//  设备类型    字符(40)      是
            bo.testdevicename = "五气分析仪";//   设备名称    字符(40)      是
            bo.opcode = "14";//    操作项 字符(40)  详见数据字典8.2序24    是
            bo.Operator = ((DataRow)obj)["CZY"].ToString();//操作人 字符(40)  操作人姓名   是
            bo.reporttime = DateTime.Now.ToString("yyyyMMddHHmmss");// 上报时间    日期格式    yyyyMMddHHmmss  是
            bo.optime = Convert.ToDateTime(((DataRow)obj)["ZJSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  自检时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opstime = Convert.ToDateTime(((DataRow)obj)["KSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热开始时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opetime = Convert.ToDateTime(((DataRow)obj)["JSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热结束时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.status = "14";// 设备状态    字符(2)   详见数据字典8.2序9 是
            bo.result = ((DataRow)obj)["ZJJG"].ToString();//  运行结果    字符(1)   1：自检成功；0：自检失败   是
            bo.memo = "";//    备注  字符(255)     FALSE
            

            #endregion

            dt.body.Add(bo);
            data.Add(dt);
            model.data = data;
            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            json = json.Replace("Operator", "operator");
            FileOpreate.SaveLog( json,"[uploadInspStmDeviceRecord-泄漏检查]：", 3);
            return json;
        }
        string create氧量程检查(object obj)
        {

            ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem分析仪氧量程检查> model = new ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem分析仪氧量程检查>();
            model.jkid = "uploadInspStmDeviceRecord";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.StmDeviceRecordDataItem分析仪氧量程检查>();
            List<ModPublicJHHB.StmDeviceRecordDataItem分析仪氧量程检查> data = new List<ModPublicJHHB.StmDeviceRecordDataItem分析仪氧量程检查>();

            ModPublicJHHB.StmDeviceRecordDataItem分析仪氧量程检查 dt = new ModPublicJHHB.StmDeviceRecordDataItem分析仪氧量程检查();
            dt.body = new List<ModPublicJHHB.分析仪氧量程检查bodyItem>();
            ModPublicJHHB.分析仪氧量程检查bodyItem bo = new ModPublicJHHB.分析仪氧量程检查bodyItem();
            #region
            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.typecode = "4";// 运行类型    字符(2)   1：自检 是
            bo.TsNo = ModPublicJHHB.TsNo;// 检测机构编号  字符(16)      是
            bo.TestLineNo = ((DataRow)obj)["JCGWH"].ToString();//  检测线 字符(16)      是
            bo.testdevicecode = bo.TsNo + "-" + bo.TestLineNo + "-" + ((DataRow)obj)["SBBH"].ToString();// 设备编号    字符(40)  参考3.7   是
            bo.testdevicetype = DicDeviceType.GetValue("五气分析仪", "");// "底盘测功机";//  设备类型    字符(40)      是
            bo.testdevicename = "五气分析仪";//   设备名称    字符(40)      是
            bo.opcode = "15";//    操作项 字符(40)  详见数据字典8.2序24    是
            bo.Operator = ((DataRow)obj)["CZY"].ToString();//操作人 字符(40)  操作人姓名   是
            bo.reporttime = DateTime.Now.ToString("yyyyMMddHHmmss");// 上报时间    日期格式    yyyyMMddHHmmss  是
            bo.optime = Convert.ToDateTime(((DataRow)obj)["ZJSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  自检时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opstime = Convert.ToDateTime(((DataRow)obj)["KSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热开始时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opetime = Convert.ToDateTime(((DataRow)obj)["JSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热结束时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.status = "14";// 设备状态    字符(2)   详见数据字典8.2序9 是
            bo.result = ((DataRow)obj)["ZJJG"].ToString();//  运行结果    字符(1)   1：自检成功；0：自检失败   是
            bo.memo = "";//    备注  字符(255)     FALSE
            bo.extn01 = ((DataRow)obj)["DATA1"].ToString();//   氧气量程标值下限    数字(10, 2)     是
            bo.extn02 = ((DataRow)obj)["DATA2"].ToString();//   氧气量程标值上限 数字(10, 2)		是
            bo.extn03 = ((DataRow)obj)["DATA3"].ToString();//   氧气量程测量值下限 数字(10, 2)		是
            bo.extn04 = ((DataRow)obj)["DATA4"].ToString();//   氧气量程测量值上限 数字(10, 2)		是
            bo.extn05 = ((DataRow)obj)["DATA5"].ToString();//   氧气量程下限误差 数字(10, 2)		是
            bo.extn06 = ((DataRow)obj)["DATA6"].ToString();//   氧气量程上限误差 数字(10, 2)		是


            #endregion

            dt.body.Add(bo);
            data.Add(dt);
            model.data = data;
            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            json = json.Replace("Operator", "operator");
            FileOpreate.SaveLog( json,"[uploadInspStmDeviceRecord-氧量程检查]：", 3);
            return json;
        }
        string create流量计检查(object obj)
        {

            ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem流量计检查> model = new ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem流量计检查>();
            model.jkid = "uploadInspStmDeviceRecord";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.StmDeviceRecordDataItem流量计检查>();
            List<ModPublicJHHB.StmDeviceRecordDataItem流量计检查> data = new List<ModPublicJHHB.StmDeviceRecordDataItem流量计检查>();

            ModPublicJHHB.StmDeviceRecordDataItem流量计检查 dt = new ModPublicJHHB.StmDeviceRecordDataItem流量计检查();
            dt.body = new List<ModPublicJHHB.流量计检查bodyItem>();
            ModPublicJHHB.流量计检查bodyItem bo = new ModPublicJHHB.流量计检查bodyItem();
            #region
            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.typecode = "4";// 运行类型    字符(2)   1：自检 是
            bo.TsNo = ModPublicJHHB.TsNo;// 检测机构编号  字符(16)      是
            bo.TestLineNo = ((DataRow)obj)["JCGWH"].ToString();//  检测线 字符(16)      是
            bo.testdevicecode = bo.TsNo + "-" + bo.TestLineNo + "-" + ((DataRow)obj)["SBBH"].ToString();// 设备编号    字符(40)  参考3.7   是
            bo.testdevicetype = DicDeviceType.GetValue("流量计", "");// "底盘测功机";//  设备类型    字符(40)      是
            bo.testdevicename = "流量计";//   设备名称    字符(40)      是
            bo.opcode = "17";//    操作项 字符(40)  详见数据字典8.2序24    是
            bo.Operator = ((DataRow)obj)["CZY"].ToString();//操作人 字符(40)  操作人姓名   是
            bo.reporttime = DateTime.Now.ToString("yyyyMMddHHmmss");// 上报时间    日期格式    yyyyMMddHHmmss  是
            bo.optime = Convert.ToDateTime(((DataRow)obj)["BDSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  自检时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opstime = Convert.ToDateTime(((DataRow)obj)["KSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热开始时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opetime = Convert.ToDateTime(((DataRow)obj)["JSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  台体预热结束时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.status = "14";// 设备状态    字符(2)   详见数据字典8.2序9 是
            bo.result = ((DataRow)obj)["BDJG"].ToString() == "合格" ? "1" : "0";//  运行结果    字符(1)   1：自检成功；0：自检失败   是
            bo.memo = "";//    备注  字符(255)     FALSE
            bo.extn01 = ((DataRow)obj)["DATA4"].ToString();// 低量程标值   数字(10, 2)        是
            bo.extn02 = ((DataRow)obj)["DATA1"].ToString();//  高量程标值 数字(10, 2)		是
            bo.extn03 = ((DataRow)obj)["DATA5"].ToString();//  低量程测量值 数字(10, 2)		是
            bo.extn04 = ((DataRow)obj)["DATA2"].ToString();//  高量程测量值 数字(10, 2)		是
            bo.extn05 = ((DataRow)obj)["DATA6"].ToString();//  低量程误差 数字(10, 2)		是
            bo.extn06 = ((DataRow)obj)["DATA3"].ToString();//  高量程误差 数字(10, 2)		是
            #endregion

            dt.body.Add(bo);
            data.Add(dt);
            model.data = data;
            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            json = json.Replace("Operator", "operator");
            FileOpreate.SaveLog( json,"[uploadInspStmDeviceRecord-流量计检查]：", 3);
            return json;
        }
        string create设备时钟同步(object obj)
        {

            ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem设备时钟同步数据> model = new ModPublicJHHB.uploadInspStmDeviceRecord<ModPublicJHHB.StmDeviceRecordDataItem设备时钟同步数据>();

            model.jkid = "uploadInspStmDeviceRecord";
            model.jksqm = ModPublicJHHB.jksqm;
            model.data = new List<ModPublicJHHB.StmDeviceRecordDataItem设备时钟同步数据>();
            List<ModPublicJHHB.StmDeviceRecordDataItem设备时钟同步数据> data = new List<ModPublicJHHB.StmDeviceRecordDataItem设备时钟同步数据>();

            ModPublicJHHB.StmDeviceRecordDataItem设备时钟同步数据 dt = new ModPublicJHHB.StmDeviceRecordDataItem设备时钟同步数据();
            dt.body = new List<ModPublicJHHB.设备时钟同步数据bodyItem>();
            ModPublicJHHB.设备时钟同步数据bodyItem bo = new ModPublicJHHB.设备时钟同步数据bodyItem();
            #region
            bo.id = System.Guid.NewGuid().ToString().Replace("-", "");  //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            bo.typecode = "3";// 运行类型    字符(2)   1：自检 是
            bo.TsNo = ModPublicJHHB.TsNo;// 检测机构编号  字符(16)      是
            bo.TestLineNo = ((DataRow)obj)["JCGWH"].ToString();//  检测线 字符(16)      是
            bo.testdevicecode = bo.TsNo + "-" + bo.TestLineNo + "-" + ((DataRow)obj)["SBBH"].ToString();// 设备编号    字符(40)  参考3.7   是
            bo.testdevicetype = DicDeviceType.GetValue("工控机", "");// "底盘测功机";//  设备类型    字符(40)      是
            bo.testdevicename = "工控机";//   设备名称    字符(40)      是
            bo.opcode = "9";//    操作项 字符(40)  详见数据字典8.2序24    是
            bo.Operator = ((DataRow)obj)["CZY"].ToString();//操作人 字符(40)  操作人姓名   是
            bo.reporttime = DateTime.Now.ToString("yyyyMMddHHmmss");// 上报时间    日期格式    yyyyMMddHHmmss  是
            bo.optime = Convert.ToDateTime(((DataRow)obj)["ZJSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  自检时间    日期格式    yyyyMMddHHmmss.SSS  是
            bo.opstime = Convert.ToDateTime(((DataRow)obj)["KSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");// 同步前时间
            bo.opetime = Convert.ToDateTime(((DataRow)obj)["JSSJ"].ToString()).ToString("yyyyMMddHHmmss.fff");//  同步后时间
            bo.status = "14";// 设备状态    字符(2)   详见数据字典8.2序9 是
            bo.result = "1";//  运行结果    字符(1)   1：自检成功；0：自检失败   是
            bo.memo = "";//    备注  字符(255)     FALSE
            
            #endregion

            dt.body.Add(bo);
            data.Add(dt);
            model.data = data;
            //传入初始化好的对象
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            json = json.Replace("Operator", "operator");
            FileOpreate.SaveLog(json,"[uploadInspStmDeviceRecord-设备时间同步]：",  3);
            return json;
        }
        /*
        public bool uploadTestingCheck(string testingid,out string tsno)
        {
            string modeljson = createResultOBD(obj_car, obj_obd);
            //textBoxJson.Text = modeljson;
            obdresultid = "";
            //GET数据 
            if (GetResultOBD(ModPublicJHHB.webAddress, modeljson, "GET", out obdresultid))
            {
                FileOpreate.SaveLog("成功", "[uploadInspResultOBD_Parser]：", 3);
                //labelStatus.Text = "POST success!";
                Console.WriteLine("POST success!");
                return true;
            }
            else
            {
                FileOpreate.SaveLog("失败", "[uploadInspResultOBD_Parser]：", 3);
                //labelStatus.Text = "POST fail!";
                Console.WriteLine("POST fail!");
                return false;
            }
        }
        // 报文生成
        //根据已检车辆信息和OBD信息生成报文
        //说明，OBD表中使用BY1作为operatorName(检测人姓名)
        public string createTestingcheck(string testingid)
        {

            ModPublicJHHB.downInspTestingCheck model = new ModPublicJHHB.downInspTestingCheck();
            model.jkid = "downInspTestingCheck";
            model.jksqm = ModPublicJHHB.jksqm;
            model.testingid = testingid;
            string json = JsonConvert.SerializeObject(model);//.Replace("\\", "");
            FileOpreate.SaveLog(json, "[downInspTestingCheck]：", 3);

            return json;
        }

        //返回
        bool GetTestingCheck(string webadd, string content, string model, out string tsno)
        {
            tsno = "";
            if (content == "")
            {
                MessageBox.Show("数据不能为空"); return false;
            }
            try
            {
                byte[] d = System.Text.Encoding.UTF8.GetBytes(content);
                System.Net.WebClient aaa = new System.Net.WebClient();
                aaa.Headers.Add("Content-Type", "application/json; charset=UTF-8");
                byte[] res;
                if (model == "GET")
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                else
                {
                    res = aaa.UploadData(webadd, "POST", d);
                }
                String ReadData = System.Text.Encoding.UTF8.GetString(res);
                //textBoxResult.Text = ReadData;
                string jsonText = ReadData;
                FileOpreate.SaveLog(jsonText, "[downInspTestingCheck_Ack]：", 3);
                Newtonsoft.Json.Linq.JObject JsonObj;
                JsonObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonText); //将json字符串转化为json对象 

                //读取json对象里的字符串对应值
                //Code = JsonObj["jkid"].ToString();
                //Status = JsonObj["jksqm"].ToString();

                //正则提取嵌套内容解析信息
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("\"odbresultid\":\"([^\"]*)\"");
                obdresultid = "" + reg.Match(jsonText).Groups[1].Value;
                if (obdresultid == "")
                {
                    FileOpreate.SaveLog("获取obdresultid为空", "[uploadInspResultOBD_Faile]：", 3);
                    return false;
                }
                //textBox1.Text = ModPublicJHHB.obdresultid;
                return true;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog(er.Message, "[uploadInspResultOBD_Eception]：", 3);
                Console.WriteLine("exception occured:" + er.Message);
                //labelStatus.Text = "exception occured:" + er.Message;
                //textBoxResult.Text = er.Message;
                return false;
            }
        }*/
    }
    public static class DictionaryExtensionMethodClass
    {
        /// <summary>
        /// 尝试将键和值添加到字典中：如果不存在，才添加；存在，不添加也不抛导常
        /// </summary>
        public static Dictionary<TKey, TValue> TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key) == false)
                dict.Add(key, value);
            return dict;
        }

        /// <summary>
        /// 将键和值添加或替换到字典中：如果不存在，则添加；存在，则替换
        /// </summary>
        public static Dictionary<TKey, TValue> AddOrPeplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict[key] = value;
            return dict;
        }

        /// <summary>
        /// 获取与指定的键相关联的值，如果没有则返回输入的默认值
        /// </summary>
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
        {
            return dict.ContainsKey(key) ? dict[key] : defaultValue;
        }

        /// <summary>
        /// 向字典中批量添加键值对
        /// </summary>
        /// <param name="replaceExisted">如果已存在，是否替换</param>
        public static Dictionary<TKey, TValue> AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dict, IEnumerable<KeyValuePair<TKey, TValue>> values, bool replaceExisted)
        {
            foreach (var item in values)
            {
                if (dict.ContainsKey(item.Key) == false || replaceExisted)
                    dict[item.Key] = item.Value;
            }
            return dict;
        }


    }
}
