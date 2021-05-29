using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataUpdate
{
    public class ModPublicJHHB
    {

        # region 联网信息
        public static string webAddress;  //接口地址
        public static string jksqm;       //验证码
        public static string TsNo;        //检测机构编号
        public static Dictionary<string, string> Dic_LineID;
        #endregion

        # region  检测信息
        public static string inspregid;   //检测登录id
        public static string testingid;   //车辆尾气检测结果id
        public static string obdresultid; //车辆OBD检测结果id
        public static string NetBGid;     //检测报告id
        #endregion

        #region  车辆信息
        public static string FuelType;    //燃油类型
        #endregion

        #region 7.1.1 车辆登录信息调取(车辆信息获取) 
        public class downInspRegInfo
        {   
            public string jkid { get; set; }//1   jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2   jksqm 验证码 是 验证码由8位长度数据组成。
            public string VIN { get; set; } //3   VIN 车辆识别码   是 VIN号或车架号
            public string EngineNo { get; set; }//4   EngineNo 发动机编号   是
            public string TsNo { get; set; }//5   TsNo 检测机构编号  是
        }

        //返回车辆信息
        public class GetRegInfo
        {
            public string jkid { get; set; }//1   jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2   jksqm 验证码 是 验证码由8位长度数据组成。
             //public List<resultItem> result { get; set; }  

        }
        //public class resultItem
        //{

        //     public List<bodyItem> body { get; set; }
        //}

        //public class bodyItem
        //{
        //          public string insp { get; set; }
        //    //public string insp_vehicle { get; set; }
        //}

        //public class insp
        //{
        //    public string inspregid { get; set; }//inspregid 车辆检测登录id    如果epdesc值为ALLOW或为空，则返回inspregid信息。该字段是环保局检测登录id唯一编号。与该车辆检测相关数据必须包含该字段内容
        //}

        //public class insp
        //{
        //    public string inspregid { get; set; }//inspregid 车辆检测登录id    如果epdesc值为ALLOW或为空，则返回inspregid信息。该字段是环保局检测登录id唯一编号。与该车辆检测相关数据必须包含该字段内容
        //    public string TsNo { get; set; }//TsNo    检测站编号 由市环保局提供，检测站唯一编号。该检测站上报的所有数据都必须包含该字段内容
        //    public string epdesc { get; set; }//epdesc  车辆是否可以在本检测站进行检测 如果值为ALLOW或为空则表示可以正常登陆检测。如果不为空，则表示该车辆被中心系统锁定，站内登录系统需把该<环保说明epdesc> 信息反馈给登录人员。（参看epdesc节点说明）。
        //    public string TestType { get; set; }//TestType 常规检测方法  最终检测方法要检测人员根据车辆实际情况进行确定
        //    public string TestTypeDesc { get; set; }//TestTypeDesc    检测方法补充说明 对建议检测方法进行补充说明。
        //    public string TestLineNo { get; set; }//TestLineNo 检测线代码   yyyyMMddHHmmss
        //    public string inspfueltype { get; set; }//inspfueltype    应检燃料类型 详见数据字典8.1序6
        //    public string RegisterTime { get; set; }//RegisterTime    车辆登记时间
        //    public string Inspperiod { get; set; }//Inspperiod  年检周期
        //    public string TestTimes { get; set; }//TestTimes   检验次数
        //}

        //public class insp_vehicle
        //{
        //    public string License { get; set; }//License 号牌号码
        //    public string LicenseCode { get; set; }//LicenseCode 号牌种类(GA)    详见数据字典8.1序1
        //    public string LicenseType { get; set; }//LicenseType 车牌颜色 详见数据字典8.2序11
        //    public string VIN { get; set; }//VIN 车辆识别代号
        //    public string vehicleid { get; set; }//vehicleid   车辆id 系统内部使用
        //    public string Engine { get; set; }//Engine 发动机型号
        //    public string EngineNo { get; set; }//EngineNo 发动机编号
        //    public string VehicleModel { get; set; }//VehicleModel 车辆型号
        //    public string FuelType { get; set; }//FuelType 燃料种类(GA)    详见数据字典8.1序6
        //    public string Cylinders { get; set; }//Cylinders   汽缸数
        //    public string Odometer { get; set; }//Odometer    里程表读数 KM
        //    public string Standard { get; set; }//Standard 排放标准    参考8.2 序15
        //    public string RM { get; set; }//RM  基准质量 KG
        //    public string GVM { get; set; }//GVM 最大总质量   KG
        //    public string ED { get; set; }//ED  发动机排量 L
        //    public string EngineSpeed { get; set; }//EngineSpeed 发动机额定转速
        //    public string EnginePower { get; set; }//EnginePower 发动额定机功率 Kw
        //    public string Passcap { get; set; }//Passcap 座位数
        //    public string Manuf { get; set; }//Manuf   车辆生产企业
        //    public string MDate { get; set; }//MDate   车辆出厂日期 yyyyMMddHHmmss
        //    public string FuelWay { get; set; }//FuelWay 供油方式    详见数据字典8.2 序20
        //    public string DriveMode { get; set; }//DriveMode   驱动方式 详见数据字典8.2 序7
        //    public string Brand { get; set; }//Brand   品牌/型号
        //    public string Gear { get; set; }//Gear    变速器型式 详见数据字典8.2 序8
        //    public string AirIn { get; set; }//AirIn   进气方式 详见数据字典8.2 序6
        //    public string isOBD { get; set; }//isOBD   是否有OBD
        //    public string VehicleDesc { get; set; }//VehicleDesc 特殊车况说明	
        //}

        #endregion

        #region   7.1.2 OBD检测结果信息调取（obd检测结果id获取）
        public class downInspOBDResultInfo
        {
            public string jkid { get; set; }  //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public string inspregid { get; set; } //3	inspregid 检测登录id  是 由环保中心返回的检测登录唯一id
        }

        //返回obd检测结果id
        public class GetOBDResultInfo
        {
            public string inspregid { get; set; }//inspregid 车辆登记id
            public string obdresultid { get; set; }//obdresultid 车辆OBD检测结果id
            public string TsNo { get; set; }//TsNo 检测机构编号
        }

        #endregion

        #region   7.1.3 尾气检测结果信息调取（尾气检测结果id获取）
        public class downInspTestingResultInfo
        {
           public string jkid { get; set; }//1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
           public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
           public string inspregid { get; set; }//3	inspregid 检测登录id  是 由环保中心返回的检测登录唯一id
        }

        //返回obd检测结果id
        public class GetTestingResultInfo
        {
            public string inspregid { get; set; }//inspregid 车辆登记id
            public string testingid { get; set; }//testingid  车辆尾气检测结果id
            public string TsNo { get; set; }//TsNo 检测机构编号
        }

        #endregion

        #region   7.1.4 时间同步接口（获取系统时间）
        public class downInspSynctime
        {
           public string jkid { get; set; }   //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
           public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
           public string deviceCode { get; set; } //3	deviceCode 设备编号    是 参考3.7
        }

        //返回系统时间
        public class GetSynctime
        {
            public string synctime { get; set; }    //synctime 服务器返回同步时间   yyyyMMddHHmmss.SSS
        }

        #endregion

        #region   7.1.5 检测报告单信息调取（获取检测报告单编号） 
        public class downInspTestingCheck
        {
            public string jkid { get; set; }   //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public string testingid { get; set; }//3 testingid检测结果id	是	参考3.7
        }

        // 返回检测报告单编号
        public class GetTestingCheck
        {
            public string testno { get; set; }   //testno 检测报告单编号 不为空，则可以打印报告单
            public string testingid { get; set; }//testingid检测结果id	是	参考3.7
        }

        #endregion

        #region   7.2.1.1 OBD检测结果信息接口
        public class uploadInspResultOBD
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
           public List<ResultOBDDataItem> data { get; set; }
        }

         public class ResultOBDDataItem
        {
            public List<ResultOBDbodyItem> body { get; set; }
        }
        
        public class ResultOBDbodyItem
        {
            public string id { get; set; } //id Odb检测记录id   字符(32)  检测线obd检测结果私有唯一标识 是
            public string TsNo { get; set; } //TsNo 检测机构编号  字符(16)  格式详见3定义部分 是
            public string inspregid { get; set; } //inspregid 检验登录ID  字符(32)  由环保局返回的检测登录唯一标识 是
            public string VIN { get; set; } //VIN 车辆识别代号  字符(32)  车辆识别代号VIN（须完整上报）	是
            public string EngineNo { get; set; } //EngineNo    发动机编号 字符(32)      是
            public string odo { get; set; } //odo 车辆累计行驶里程（ODO）	数值(4)       是
            public string ecu1 { get; set; } //ecu1    控制单元名字 字符(32)  默认值“发动机控制单元”	是
            public string calid1 { get; set; } //calid1  发动机控制控制单元 CAL ID 字符(32)      是
            public string cvn1 { get; set; } //cvn1    发动机控制控制单元 CVN   字符(32)
            public string ecu2 { get; set; } //ecu2 控制单元名字  字符(32)  默认值“后处理控制单元”	
            public string calid2 { get; set; } //calid2 后处理器控制控制单元 CAL ID   字符(32)
            public string cvn2 { get; set; } //cvn2 后处理器控制单元 CVN 字符(32)
            public string ecu3 { get; set; } //ecu3 其他控制单元名称    字符(32)
            public string calid3 { get; set; } //calid3 其他控制单元CAL ID 字符(32)
            public string cvn3 { get; set; } //cvn3 其他控制单元CVN   字符(32)
            public string indicatorCheck { get; set; } //indicatorCheck OBD故障指示器检测  字符(1)   1：合格 0：不合格 是
            public string communicationCheck { get; set; } //communicationCheck OBD诊断仪通讯情况  字符(1)   1：成功 0：不成功 是
            public string communicationFalseRes { get; set; } //communicationFalseRes OBD诊断仪通讯不合格原因: 1：接口损坏 2：找不到接口 3：连接后不能通讯 多选项使用“,”分割
            public string faultCodes { get; set; } //faultCodes  故障代码 多故障代码用“,”隔开
            public string noReadyStatePros { get; set; } //noReadyStatePros    就绪状态未完成项目		1：SCR 2：POC 3：DOC 4：DPF5：催化器 6：氧传感器7：氧传感器加热器8：废气再循环(EGR)多选项使用“,”分割
            public string mileage { get; set; } //mileage MIL灯点亮后行驶里程(km)    数值(4)   Km
            public string operatorName { get; set; } //operatorName    检测人姓名 字符(32)      是
            public string teststime { get; set; } //teststime   检测开始时间 时间  yyyyMMddHHmmss 是
            public string testetime { get; set; } //testetime 检测结束时间  时间 yyyyMMddHHmmss  是
            public string judge { get; set; } //judge   检测结果 字符(1)   1：合格 0：不合格 是
            public string isRecheck { get; set; } //isRecheck 是否需要复检  字符(1)   1：需要 0：不需要
            public string reJudge { get; set; } //reJudge 复检结果 字符(1)   1：合格 0：不合格

        }
 

        // OBD检测信息上报返回
        public class GetResultOBD
        {
            public string code { get; set; }  //code 回执状态代码  参考 6.2.4	 code定义
            public string message { get; set; }  //message 回执状态代码说明 参考 6.2.4	 code定义
            public string responseTime { get; set; }  //responseTime    回执时间
            public string obdresultid { get; set; }  //obdresultid    回执OBD检测结果id
            public string inspregid { get; set; }  //inspregid       回执车辆登录id 同 7.1.1 返回值
            //说明：obdresultid --OBD检测结果id, 上传该检测车辆OBD检测过程数据必须封装该字段内容。inspregid--检测登录id, 上传与该检测车辆相关检测数据必须封装该字段内容。"//回执内容说明
        }

        #endregion

        #region   7.2.1.2 OBD检测过程信息接口

        #region   汽油OBD检测过程
        public class uploadInspProcessOBDQY
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<ProcessOBDDataItemQY> data { get; set; }
        }

        public class ProcessOBDDataItemQY
        {
            public List<ProcessOBDbodyQYItem> body { get; set; }
        }

        public struct ProcessOBDbodyQYItem
        {
            public string id { get; set; } //id Odb检测过程记录id 字符(32)  检测站检测obd检测过程私有唯一标识 是
            public string inspregid { get; set; } //inspregid 检验登录ID  字符(32)  由环保局返回的检测登录唯一标识 是
            public string obdresultid { get; set; } //obdresultid Odb检测结果id   字符(32)  由环保局返回的Odb检测结果唯一标识
            public string tsno { get; set; }  //tsno    检测机构编号 字符(16)  格式详见3定义部分 是
            public string serialno { get; set; }  //serialno 采样时序    数值(4)   逐秒，从1开始，每条递增1 是
            public string optime { set; get; }
            public string throttleOpen { get; set; }  //throttleOpen 节气门绝对开度 数值(12,4)    汽油车（%）	
            public string loadingValue { get; set; }  //loadingValue 计算负荷值   数值(12,4)    汽油车（%）	
            public string qyzhq { get; set; }  //qyzhq 前氧传感器信号 数值(12,4)    （mV/mA）	
            public string Lambda { get; set; }  //Lambda 过量空气系数  数值(12,4)    （λ）	
            public string vehiclespeed { get; set; }  //vehiclespeed 车速  数值(12,4)    （km/h）	
            public string enginespeed { get; set; }  //enginespeed 发动机转速   数值(12,4)    （r/min）	
            public string airin { get; set; }  //airin 进气量 数值(12,4)    （g/s）	
            public string pressure { get; set; }  //pressure 进气压力    数值(12,4)    （kPa）	
        }
        #endregion

        # region 柴油OBD检测过程
        public class uploadInspProcessOBDCY
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<ProcessOBDDataItemCY> data { get; set; }
        }

        public class ProcessOBDDataItemCY
        {
            public List<ProcessOBDbodyCYItem> body { get; set; }
        }
               
        public struct ProcessOBDbodyCYItem
        {
            public string id { get; set; }  //id Odb检测过程记录id 字符(32)  检测线检测obd检测过程私有唯一标识 是
            public string inspregid { get; set; }  //inspregid 检验登录ID  字符(32)  由环保局返回的检测登录唯一标识 是
            public string obdresultid { get; set; }  //obdresultid Odb检测结果id   字符(32)  由环保局返回的Odb检测结果唯一标识
            public string tsno { get; set; }  //tsno    检测机构编号 字符(16)  格式详见3定义部分 是
            public string serialno { get; set; }  //serialno 采样时序    数值(4)   逐秒，从1开始，每条递增1 是
            public string optime { get; set; }  // optime 采样时间    时间类型 yyyyMMddHHmmss
            public string throttleOpen { get; set; }  //throttleOpen 油门开度    数值(12,4)    （%）	
            public string vehiclespeed { get; set; }  //vehiclespeed 车速  数值(12,4)    （km/h）	
            public string enginepower { get; set; }  //enginepower 发动机输出功率 数值(12,4)    （kw）	
            public string enginespeed { get; set; }  //enginespeed 发动机转速   数值(4)   （r/min）	
            public string airin { get; set; }  //airin 进气量 数值(12,4)    （g/s）	
            public string pressure { get; set; }  //pressure 增压压力    数值(12,4)    （kPa）	
            public string oilConsumption { get; set; }  //oilConsumption 耗油量 数值(12,4)    （L/100km）	
            public string NOx { get; set; }  //NOx 氮氧传感器浓度 数值(12,4)    （ppm）	
            public string urea { get; set; }  //urea 尿素喷射量   数值(12,4)    （L/h）	
            public string DischargeTemperature { get; set; }  //DischargeTemperature 排气温度    数值(12,4)    (℃)	
            public string klPressure { get; set; }  //klPressure 颗粒捕集器压差 数值(12,4)    （kpa）	
            public string ECR { get; set; }  //ECR EGR 开度 数值(12,4)    （%）	
            public string FuelInPressure { get; set; }  //FuelInPressure 燃油喷射压力  数值(12,4)    （bar）	
        }
        #endregion

        #endregion

        #region   7.2.1.3 OBD检测IUPR 相关信息接口
        public class uploadInspIuprOBD
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<IuprOBDDataItem> data { get; set; }
        }

        public class IuprOBDDataItem
        {
            public List<IuprOBDbodyItem> body { get; set; }
        }

        public struct IuprOBDbodyItem
        {
            public string id { get; set; } //id Odb检测过程记录id 字符(32)  检测线检测obd检测过程私有唯一标识 是
            public string inspregid { get; set; } //inspregid 检验登录ID  字符(32)  由环保局返回的检测登录唯一标识 是
            public string obdresultid { get; set; } //obdresultid Odb检测结果id   字符(32)  由环保局返回的Odb检测结果唯一标识 是
            public string tsno { get; set; } //tsno 检测机构编号  字符(16)  格式详见3定义部分 是
            public string projectcode { get; set; } //projectcode 监测项目代码  字符(32)      是
            public string projectname { get; set; } //projectname 监测项目名称 字符(32)      是
            public string denominator { get; set; } //denominator 监测完成次数 数字（4）		是
            public string molecule { get; set; } //molecule    符合监测条件次数 数字（4）		是
            public string iuprrate { get; set; } //iuprrate    IUPR率 数字（12,4）	（%）	是
        }

        #endregion

        #region   7.2.2.1	尾气检测结果信息接口

        #region   7.2.2.1.2 自由加速法（不透光烟度）检测结果数据接口
        public class uploadInspResultLIGHTPROOF
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<ResultLIGHTPROOFDataItem> data { get; set; }
        }

        public class ResultLIGHTPROOFDataItem
        {
            public List<ResultLIGHTPROOFbodyItem> body { get; set; }
        }

        public class ResultLIGHTPROOFbodyItem
        {
            public string id { get; set; } //id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            public string inspregid { get; set; } //inspregid 检验登录ID  字符(32)  由环保局返回的检测登录唯一标识 是
            public string License { get; set; } //License 号牌号码    字符(10)  格式详见3定义部分 FALSE
            public string LicenseType { get; set; } //LicenseType 车牌颜色    数值(1)   详见数据字典8.2序11 FALSE
            public string LicenseCode { get; set; } //LicenseCode 号牌种类(GA)    字符(2)   详见数据字典8.1序1 FALSE
            public string VIN { get; set; } //VIN 车辆识别代号  字符(20)  行驶证上的车辆识别代号VIN（须完整上报）	是
            public string EngineNo { get; set; } //EngineNo    发动机号 字符(20)      是
            public string TsNo { get; set; } //TsNo    检测机构编号 字符(16)  格式详见3定义部分 是
            public string FuelType { get; set; } //FuelType 燃料种类(GA)    字符(4)   详见数据字典8.1序6 是
            public string TestLineNo { get; set; } //TestLineNo 检测线代码   字符(8)   格式详见3定义部分 是
            public string OperatorName { get; set; } //OperatorName 操作员 字符(32)      是
            public string DriverName { get; set; } //DriverName  引车员 字符(32)      是
            public string Teststime { get; set; } //Teststime   检测开始时间 日期格式    YYYYMMDDHHmmss 是
            public string Testetime { get; set; } //Testetime 检测结束时间  日期格式 YYYYMMDDHHmmss  是
            public string TestType { get; set; } //TestType    检测方法 字符(1)   详见数据字典8.2序4 是
            public string TestJudge { get; set; } //TestJudge 检测结果    字符(1)   1:合格0：不合格，上报值为（0或1）	是
            public string Samplingdepth { get; set; } //Samplingdepth   采样深度 数值(4)       是
            public string Temperature { get; set; } //Temperature 温度 数值(12,4)    （Co）	是
            public string Humidity { get; set; } //Humidity    湿度 数值(12,4)    （%）	是
            public string Atpressure { get; set; } //Atpressure  气压 数值(12,4)    （kPa）	是
            public string IdleRev { get; set; } //IdleRev 实测转速 数值(8)   （r/min）	是
            public string RPM { get; set; } //RPM 额定转速 数值(8)   （r/min）	是
            public string SmokeK1 { get; set; } //SmokeK1 光吸收系数1 数值(12,4)    （m-1），倒数第一次 是
            public string SmokeK2 { get; set; } //SmokeK2 光吸收系数2  数值(12,4)    （m-1），倒数第二次 是
            public string SmokeK3 { get; set; } //SmokeK3 光吸收系数3  数值(12,4)    （m-1），倒数第三次 是
            public string SmokeK4 { get; set; } //SmokeK4 光吸收系数4  数值(12,4)    预留 FALSE
            public string SmokeAvg { get; set; } //SmokeAvg 排放结果平均值 数值(12,4)    （m-1）	是
            public string SmokeKLimit { get; set; } //SmokeKLimit 排放限值 数值(12,4)    （m-1）	是
            public string SmokeKJudge { get; set; } //SmokeKJudge 排放判定 字符(1)   0-不合格，1-合格 是
        }

        #endregion

        #region   7.2.2.1.3 加载减速法检测结果数据接口
        public class uploadInspResultLUGDOWN
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<ResultLUGDOWNDataItem> data { get; set; }
        }

        public class ResultLUGDOWNDataItem
        {
            public List<ResultLUGDOWNbodyItem> body { get; set; }
        }

        public class ResultLUGDOWNbodyItem
        {
             public string id { get; set; } //Id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
             public string inspregid { get; set; } //inspregid 检验登录ID  字符(32)  由环保局返回的检测登录唯一标识 是
             public string License { get; set; } //License 号牌号码    字符(10)  格式详见3定义部分 FALSE
             public string LicenseType { get; set; } //LicenseType 车牌颜色    数值(1)   详见数据字典8.2序11 FALSE
             public string LicenseCode { get; set; } //LicenseCode 号牌种类(GA)    字符(2)   详见数据字典8.1序1 FALSE
             public string VIN { get; set; } //VIN 车辆识别代号  字符(20)  行驶证上的车辆识别代号VIN（须完整上报）	是
             public string EngineNo { get; set; } //EngineNo    发动机号 字符（20）		是
             public string TsNo { get; set; } //TsNo    检测机构编号 字符(16)  格式详见3定义部分 是
             public string TestType { get; set; } //TestType 检测方法    字符(1)   详见数据字典8.2序4 是
             public string FuelType { get; set; } //FuelType 燃料种类(GA)    字符(4)   详见数据字典8.1序6 是
             public string TestLineNo { get; set; } //TestLineNo 检测线代码   字符(8)   格式详见3定义部分 是
             public string OperatorName { get; set; } //OperatorName 操作员 字符(32)      是
             public string DriverName { get; set; } //DriverName  引车员 字符(32)      是
             public string Teststime { get; set; } //Teststime   检测开始时间 日期格式    YYYYMMDDHHmmSS 是
             public string Testetime { get; set; } //Testetime 检测结束时间  日期格式 YYYYMMDDHHmmSS  是
             public string TestJudge { get; set; } //TestJudge   检测结果 字符(1)   1:合格 0：不合格 是
             public string Samplingdepth { get; set; } //Samplingdepth 采样深度    数值(4)       是
             public string Temperature { get; set; } //Temperature 温度 数值(12,4)    （Co）	是
             public string Humidity { get; set; } //Humidity    湿度 数值(12,4)    （%）	是
             public string Atpressure { get; set; } //Atpressure  气压 数值(12,4)    （kPa）	是
             public string RPM { get; set; } //RPM 发动机额定转速 数值(8)   （r/min）	是
             public string MaxRPM { get; set; } //MaxRPM  发动机最大转速 数值(8)   （r/min）	是
             public string CorVelMaxhp { get; set; } //CorVelMaxhp 修正MAXHP时滚筒线速度 数值(8)   （r/min）	是
             public string ActVelMaxhp { get; set; } //ActVelMaxhp 实际MAXHP时滚筒线速度 数值(8)   （r/min）	是
             public string CorMaxhp { get; set; } //CorMaxhp    修正最大轮边功率 数值(12,4)    （KW）	是
             public string ActMaxhp { get; set; } //ActMaxhp    实测最大轮边功率 数值(12,4)    （KW）	是
             public string Minhp { get; set; } //Minhp   所需最小轮边功率 数值(12,4)    （KW）	是
             public string K100 { get; set; } //K100	100%光吸收系数 数值(12,4)    （m-1）	是
             public string K80 { get; set; } //K80	80%光吸收系数 数值(12,4)    （m-1）	是
             public string NOx80 { get; set; } //NOx80	80%NOX浓度 数值(12,4)    （10-6）	是
             public string SmokeK100Limit { get; set; } //SmokeK100Limit  排放限值 数值(12,4)    （m-1）	是
             public string SmokeK80Limit { get; set; } //SmokeK80Limit   排放限值 数值(12,4)    （m-1）	是
             public string NOx80Limit { get; set; } //NOx80Limit	80%NOX排放限值 数值(12,4)    （10-6）	是
             public string MaxhpLimit { get; set; } //MaxhpLimit  最大轮边功率限值 数值(12,4)    （KW）	是
             public string MaxRPMJudge { get; set; } //MaxRPMJudge 最大转速判定 字符(1)   0-不合格，1-合格 是
             public string SmokeK100Judge { get; set; } //SmokeK100Judge 排放判定    字符(1)   0-不合格，1-合格 是
             public string SmokeK80Judge { get; set; } //SmokeK80Judge 排放判定    字符(1)   0-不合格，1-合格 是
             public string NOx80Judge { get; set; } //NOx80Judge	80%NOX排放判定 字符(1)   0-不合格，1-合格 是
             public string MaxhpJudge { get; set; } //MaxhpJudge 最大轮边功率判定    字符(1)   0-不合格，1-合格 是
             public string pcf { get; set; } //pcf 功率修正系数  数值(12,4)        是
             public string RateRevUp { get; set; } //RateRevUp   发动机额定转速上限 数值(8)   （r/min）	FALSE
             public string RateRevDown { get; set; } //RateRevDown 发动机额定转速下限 数值(8)   （r/min）	FALSE
        }
        #endregion

        #region 7.2.2.1.4 双怠速法检测结果数据接口
        public class uploadInspResultDIDLE
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<ResultDIDLEDataItem> data { get; set; }
        }

        public class ResultDIDLEDataItem
        {
            public List<ResultDIDLEbodyItem> body { get; set; }
        }

        public class ResultDIDLEbodyItem
        {
            public string Id { get; set; } //Id 检测结果id  字符(32)  检测线尾气检测结果私有唯一标识 是
            public string inspregid { get; set; } //inspregid 检验登录ID  字符(32)  由环保局返回的检测登录唯一标识 是
            public string License { get; set; } //License 号牌号码    字符(10)  格式详见3定义部分 FALSE
            public string LicenseType { get; set; } //LicenseType 车牌颜色    数值(1)   详见数据字典8.2序11 FALSE
            public string LicenseCode { get; set; } //LicenseCode 号牌种类(GA)    字符(2)   详见数据字典8.1序1 FALSE
            public string VIN { get; set; } //VIN 车辆识别代号  字符(20)  行驶证上的车辆识别代号VIN（须完整上报）	是
            public string EngineNo { get; set; } //EngineNo    发动机号 字符(20)      是
            public string TsNo { get; set; } //TsNo    检测机构编号 字符(16)  格式详见3定义部分 是
            public string TestType { get; set; } //TestType 检测方法    字符(1)   详见数据字典8.2序4 是
            public string FuelType { get; set; } //FuelType 燃料种类(GA)    字符(4)   详见数据字典8.1序6 是
            public string TestLineNo { get; set; } //TestLineNo 检测线代码   字符(8)   格式详见3定义部分 是
            public string OperatorName { get; set; } //OperatorName 操作员 字符(32)      是
            public string DriverName { get; set; } //DriverName  引车员 字符(32)      是
            public string Teststime { get; set; } //Teststime   检测开始时间 日期格式    YYYYMMDDHHmmSS 是
            public string Testetime { get; set; } //Testetime 检测结束时间  日期格式 YYYYMMDDHHmmSS  是
            public string TestJudge { get; set; } //TestJudge   检测结果 字符(1)   1:合格   0：不合格，上报值为（0或1）	是
            public string Samplingdepth { get; set; } //Samplingdepth   采样深度 数值(4)       是
            public string Temperature { get; set; } //Temperature 温度 数值(12,4)    （Co）	是
            public string Humidity { get; set; } //Humidity    湿度 数值(12,4)    （%）	是
            public string Atpressure { get; set; } //Atpressure  气压 数值(12,4)    （kPa）	是
            public string Gdszs { get; set; } //Gdszs   高怠速转速值 数值(8)   （r/min）	是
            public string Dszs { get; set; } //Dszs    低怠速转速值 数值(8)   （r/min）	是
            public string Lambda { get; set; } //Lambda  过量空气系数结果 数值(12,4)        是
            public string LSCOResult { get; set; } //LSCOResult  低怠速CO结果 数值(12,4)    （%）	是
            public string LSHCResult { get; set; } //LSHCResult  低怠速HC结果 数值(12,4)    （10-6）	是
            public string HSCOResult { get; set; } //HSCOResult  高怠速CO结果 数值(12,4)    （%）	是
            public string HSHCResult { get; set; } //HSHCResult  高怠速HC结果 数值(12,4)    （10-6）	是
            public string LambdaUp { get; set; } //LambdaUp    过量空气系数限值上限 数值(12,4)        是
            public string LambdaDown { get; set; } //LambdaDown  过量空气系数限值下限 数值(12,4)        是
            public string LSCOLimit { get; set; } //LSCOLimit   低怠速CO限值 数值(12,4)    （%）	是
            public string LSHCLimit { get; set; } //LSHCLimit   低怠速HC限值 数值(12,4)    （10-6）	是
            public string HSCOLimit { get; set; } //HSCOLimit   高怠速CO限值 数值(12,4)    （%）	是
            public string HSHCLimit { get; set; } //HSHCLimit   高怠速HC限值 数值(12,4)    （10-6）	是
            public string LambdaJudge { get; set; } //LambdaJudge 过量空气系数判定 字符(1)   0-不合格，1-合格 是
            public string LSCOJudge { get; set; } //LSCOJudge 低怠速CO判定 字符(1)   0-不合格，1-合格 是
            public string LSHCJudge { get; set; } //LSHCJudge 低怠速HC判定 字符(1)   0-不合格，1-合格 是
            public string HSCOJudge { get; set; } //HSCOJudge 高怠速CO判定 字符(1)   0-不合格，1-合格 是
            public string HSHCJudge { get; set; } //HSHCJudge 高怠速HC判定 字符(1)   0-不合格，1-合格 是
        }
        #endregion

        #region 7.2.2.1.6 简易瞬态工况法检测结果数据接口
        public class uploadInspResultVMAS
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<ResultVMASDataItem> data { get; set; }
        }

        public class ResultVMASDataItem
        {
            public List<ResultVMASbodyItem> body { get; set; }
        }

        public class ResultVMASbodyItem
        {
            public string id { get; set; } //id 唯一id    字符(32)  检测线尾气检测结果私有唯一标识 是
            public string inspregid { get; set; } //inspregid 检验登录ID  字符(32)  由环保局返回的检测登录唯一标识 是
            public string License { get; set; } //License 号牌号码    字符(10)  格式详见3定义部分 FALSE
            public string LicenseType { get; set; } //LicenseType 车牌颜色    数值(1)   详见数据字典8.2序11 FALSE
            public string LicenseCode { get; set; } //LicenseCode 号牌种类(GA)    字符(2)   详见数据字典8.1序1 FALSE
            public string VIN { get; set; } //VIN 车辆识别代号  字符(20)  行驶证上的车辆识别代号VIN（须完整上报）	是
            public string EngineNo { get; set; } //EngineNo    发动机号 字符(20)      是
            public string TsNo { get; set; } //TsNo    检测机构编号 字符(16)  格式详见3定义部分 是
            public string FuelType { get; set; } //FuelType 燃料种类(GA)    字符(4)   详见数据字典8.1序6 是
            public string TestLineNo { get; set; } //TestLineNo 检测线代码   字符(8)   格式详见3定义部分 是
            public string OperatorName { get; set; } //OperatorName 操作员 字符(32)      是
            public string DriverName { get; set; } //DriverName  引车员 字符(32)      是
            public string Teststime { get; set; } //Teststime   检测开始时间 日期格式    YYYYMMDDHHmmSS 是
            public string Testetime { get; set; } //Testetime 检测结束时间  日期格式 YYYYMMDDHHmmSS  是
            public string TestType { get; set; } //TestType    检测方法 字符(1)   详见数据字典8.2序4 是
            public string TestJudge { get; set; } //TestJudge 检测结果    字符(1)   1:合格0：不合格，上报值为（0或1）	是
            public string Samplingdepth { get; set; } //Samplingdepth   采样深度 数值(4)       是
            public string Temperature { get; set; } //Temperature 温度 数值(12,4)    （Co）	是
            public string Humidity { get; set; } //Humidity    湿度 数值(12,4)    （%）	是
            public string Atpressure { get; set; } //Atpressure  气压 数值(12,4)    （kPa）	是
            public string HC { get; set; } //HC  HC排放结果 数值(12,4)    （g/km）	是
            public string CO { get; set; } //CO  CO排放结果 数值(12,4)    （g/km）	是
            public string NOx { get; set; } //NOx NOX排放结果 数值(12,4)    （g/km）	是
            public string HCNOx { get; set; } //HCNOx   HC+NOX排放结果 数值(12,4)    （g/km）	FALSE
            public string Lambda { set; get; }
            public string LambdaUp { get; set; } //过量空气系数上限值   数值(12,4)        是
            public string LambdaDown { get; set; } // 过量空气系数下限值 数值(12,4)        是

            public string LambdaJudge { get; set; } //过量空气系数判定    数值(1)       是

            public string HCLimit { get; set; } //HCLimit HC排放限值 数值(12,4)    （g/km）	是
            public string COLimit { get; set; } //COLimit CO排放限值 数值(12,4)    （g/km）	是
            public string NOxLimit { get; set; } //NOxLimit    NOX排放限值 数值(12,4)    （g/km）	是
            public string HCNOxLimit { get; set; } //HCNOxLimit  HC+NOX排放限值 数值(12,4)    （g/km）	FALSE
            public string HCJudge { get; set; } //HCJudge HC排放判定 字符(1)   0-不合格，1-合格 是
            public string COJudge { get; set; } //COJudge CO排放判定  字符(1)   0-不合格，1-合格 是
            public string NOxJudge { get; set; } //NOxJudge NOX排放判定 字符(1)   0-不合格，1-合格 是
            public string HCNOxJudge { get; set; } //HCNOxJudge HC+NOX排放判定 字符(1)   0-不合格，1-合格 FALSE

        }
        #endregion

        #region 7.2.2.1.7 回执数据说明---尾气检测结果上报返回
        public class Gettestingid
        {
            public string code { get; set; } //code 回执状态代码  参考 6.2.4	 code定义
            public string message { get; set; }  //message 回执状态代码说明 参考 6.2.4	 code定义
            public string responseTime { get; set; }  //responseTime    回执时间
            public string testingid { get; set; }    //testingid   尾气检测结果id
            public string inspregid { get; set; }   //inspregid   车辆登录id 同 7.1.1 返回值
        }
        #endregion

        #endregion

        #region   7.2.2.2 尾气检测过程信息接口

        #region   7.2.2.2.2 自由加速（不透光烟度）检测过程数据接口
        public class uploadInspProcessLIGHTPROOF
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<ProcessLIGHTPROOFDataItem> data { get; set; }
        }

        public class ProcessLIGHTPROOFDataItem
        {
            public List<ProcessLIGHTPROOFbodyItem> body { get; set; }
        }

        public struct ProcessLIGHTPROOFbodyItem
        {
            public string id { get; set; } //id ID  字符(32)  检测线尾气检测过程私有唯一标识 是
            public string testingid { get; set; } //testingid 检测记录ID  字符(32)  由环保局返回的检测登录唯一标识 是
            public string inspregid { get; set; } //inspregid 检测结果id  字符(32)  由环保局返回的检测结果唯一标识 是
            public string tsno { get; set; } //tsno 检测机构编号  字符(16)  格式详见3定义部分
            public string serialno { get; set; } //serialno    采样时序 数值(4)   取样点序号，对应同一个检测记录ID，从1开始顺序编号 是
            public string opno { get; set; } //opno 操作顺序号   数值(4)   以工况类型OPCODE分类按顺序从1开始的递增号 是
            public string opcode { get; set; } //opcode 工况类型    字符(8)   0：最后三次测量前过程数据 1-3：最后三次测量时过程数据 是
            public string samplingdepth { get; set; } //samplingdepth 采样深度    数值(4)   (取样探头插管深度)毫米，必须为传感器实测值 是
            public string optime { get; set; } //optime 采样时间    日期格式 yyyyMMddHHmmss  是
            public string posno { get; set; } //posno   档位数 数值(2)       FALSE
            public string vehiclespeed { get; set; } //vehiclespeed    车速 数值(12,4)        FALSE
            public string enginespeed { get; set; } //enginespeed 发动机转速 数值(4)       是
            public string enginepower { get; set; } //enginepower 发动机功率 数值(12,4)        FALSE
            public string sfk { get; set; } //sfk 光吸收系数 数值(12,4)        是
            public string sfn { get; set; } //sfn 线性分度单位 数值(12,4)        FALSE
            public string temperature { get; set; } //temperature 温度 数值(12,4)        是
            public string humidity { get; set; } //humidity    湿度 数值(12,4)        是
            public string pressure { get; set; } //pressure    气压 数值(12,4)        是
            public string kjudge { get; set; } //kjudge  排放判定 字符(1)       是
        }

        #endregion

        #region   7.2.2.2.3 加载减速法检测过程数据接口
        public class uploadInspProcessLUGDOWN
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<ProcessLUGDOWNDataItem> data { get; set; }
        }

        public class ProcessLUGDOWNDataItem
        {
            public List<ProcessLUGDOWNbodyItem> body { get; set; }
        }

        public struct ProcessLUGDOWNbodyItem
        {

            public string id { get; set; } //id ID  字符(32)  检测线尾气检测过程私有唯一标识 是
            public string testingid { get; set; } //testingid 检测记录ID  字符(32)  由环保局返回的检测登录唯一标识 是
            public string tsno { get; set; } //tsno 检测机构编号  字符(16)  格式详见3定义部分 是
            public string inspregid { get; set; } //inspregid 检测结果id  字符(32)  由环保局返回的检测结果唯一标识 是
            public string serialno { get; set; } //serialno 采样时序    数值(4)   逐秒，从1开始，每条递增1 是
            public string opno { get; set; } //opno 操作顺序号   数值(4)   见下述加载减速操作码表 是
            public string opcode { get; set; } //opcode 工况类型    字符(8)   见下述加载减速操作码表 是
            public string Samplingdepth { get; set; } //Samplingdepth 采样深度    数值(4)   (取样探头插管深度)毫米，必须为传感器实测值 是
            public string optime { get; set; } //optime 采样时间    日期格式 yyyyMMddHHmmss  是
            public string posno { get; set; } //posno   档位数 数值(2)       FALSE
            public string vehiclespeed { get; set; } //vehiclespeed    车速 数值(12,4)        是
            public string enginespeed { get; set; } //enginespeed 发动机转速 数值(4)       是
            public string enginepower { get; set; } //enginepower 发动机功率 数值(12,4)        是
            public string sfk { get; set; } //sfk 光吸收系数 数值(12,4)        是
            public string sfn { get; set; } //sfn 线性分度单位 数值(12,4)        FALSE
            public string temperature { get; set; } //temperature 温度 数值(12,4)        是
            public string humidity { get; set; } //humidity    湿度 数值(12,4)        是
            public string pressure { get; set; } //pressure    气压 数值(12,4)        是
            public string dynpa { get; set; } //dynpa   底盘测功机总加载功率 数值(12,4)        是
            public string dynplhp { get; set; } //dynplhp 底盘测功机寄生功率 数值(12,4)        是
            public string dynihp { get; set; } //dynihp  底盘测功机指示功率 数值(12,4)        是
            public string dynn { get; set; } //dynn    底盘测功机扭力 数值(12,4)        是
            public string pcf { get; set; } //pcf 功率修正系数 数值(12,4)        是
            public string volco2 { get; set; } //volco2  二氧化碳浓度 数值(12,4)    （%）	是
            public string volnox { get; set; } //volnox  氮氧化合物浓度 数值(12,4)    （10-6）	是
            public string kjudge { get; set; } //kjudge  排放判定 字符(1)       是
        }

        #endregion

        #region   7.2.2.2.4 双怠速法检测过程数据接口
        public class uploadInspProcessDIDLE
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<ProcessDIDLEDataItem> data { get; set; }
        }

        public class ProcessDIDLEDataItem
        {
            public List<ProcessDIDLEbodyItem> body { get; set; }
        }

        public struct ProcessDIDLEbodyItem
        {
            public string id { get; set; } //id ID  字符(32)  检测线尾气检测过程私有唯一标识 是
            public string testingid { get; set; } //testingid 检测记录ID  字符(32)  由环保局返回的检测登录唯一标识 是
            public string inspregid { get; set; } //inspregid 检测结果id  字符(32)  由环保局返回的检测结果唯一标识 是
            public string tsno { get; set; } //tsno 检测机构编号  字符(16)  格式详见3定义部分
            public string serialno { get; set; } //serialno    采样时序 数值(4)   逐秒，从1开始，每条递增1 是
            public string opno { get; set; } //opno 运转次序    数值(4)   低怠速时1,2,3…；高怠速时1,2,3…	是
            public string opcode { get; set; } //opcode  工况类型 字符(8)   0 -70%额定转速 1-高怠速准备 2-高怠速检测3-怠速准备  4-怠速检测 是
            public string samplingdepth { get; set; } //samplingdepth 采样深度    数值(4)   (取样探头插管深度)毫米，必须为传感器实测值 是
            public string Optime { get; set; } //Optime 采样时间    日期格式 YYYYMMDDHHmmss  是
            public string Posno { get; set; } //Posno   档位数 数值(2)   0默认为零 FALSE
            public string vehiclespeed { get; set; } //vehiclespeed 车速  数值(12,4)    0默认为零 FALSE
            public string enginespeed { get; set; } //enginespeed 发动机转速   数值(4)       是
            public string Volco { get; set; } //Volco   一氧化碳浓度CO 数值(12,4)        是
            public string volco2 { get; set; } //volco2  二氧化碳浓度CO2 数值(12,4)        是
            public string Volhc { get; set; } //Volhc   碳氢化合物浓度HC 数值(12,4)        是
            public string Volnox { get; set; } //Volnox  氮氧化物浓度NOX 数值(12,4)        FALSE
            public string volo2 { get; set; } //volo2   原始氧浓度O2 数值(12,4)        是
            public string temperature { get; set; } //temperature 温度 数值(12,4)        是
            public string humidity { get; set; } //humidity    湿度 数值(12,4)        是
            public string pressure { get; set; } //pressure    气压 数值(12,4)        是
            public string kjudge { get; set; } //kjudge  排放判定 字符(1)       是
        }

        #endregion

        #region   7.2.2.2.6 简易瞬态工况法检测过程数据接口
        public class uploadInspProcessVMAS
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<ProcessVMASDataItem> data { get; set; }
        }

        public class ProcessVMASDataItem
        {
            public List<ProcessVMASbodyItem> body { get; set; }
        }

        public struct ProcessVMASbodyItem
        {
            public string Id { get; set; } //Id ID  字符(32)  检测线尾气检测过程私有唯一标识 是
            public string testingid { get; set; } //testingid 检测记录ID  字符(32)  由环保局返回的检测登录唯一标识 是
            public string inspregid { get; set; } //inspregid 检测结果id  字符(32)  由环保局返回的检测结果唯一标识 是
            public string tsno { get; set; } //tsno 检测机构编号  字符(16)  格式详见3定义部分
            public string serialno { get; set; } //serialno    采样时序 数值(4)   逐秒，从1开始，每条递增1，总共195秒 是
            public string opno { get; set; } //opno 操作顺序号   数值(4)   见下述简易瞬态工况法操作码表 是
            public string opcode { get; set; } //opcode 操作码 字符(8)   见下述简易瞬态工况法操作码表 是
            public string samplingdepth { get; set; } //samplingdepth 采样深度    数值(4)   (取样探头插管深度)毫米，必须为传感器实测值 是
            public string optime { get; set; } //optime 采样时间    日期格式 yyyyMMddHHmmss  是
            public string posno { get; set; } //posno   档位数 数值(2)       FALSE
            public string vehiclespeed { get; set; } //vehiclespeed    车速 数值(12,4)        是
            public string enginespeed { get; set; } //enginespeed 发动机转速 数值(4)       FALSE
            public string volco { get; set; } //volco   一氧化碳浓度CO 数值(12,4)        是
            public string volco2 { get; set; } //volco2  二氧化碳浓度CO2 数值(12,4)        是
            public string volhc { get; set; } //volhc   碳氢化合物浓度HC 数值(12,4)        是
            public string volnox { get; set; } //volnox  氮氧化物浓度NOX 数值(12,4)        是
            public string volo2raw { get; set; } //volo2raw    原始氧浓度O2 数值(12,4)        是
            public string volo2dil { get; set; } //volo2dil    稀释氧浓度O2 数值(12,4)        是
            public string volo2amb { get; set; } //volo2amb    环境氧浓度O2 数值(12,4)        是
            public string flowactual { get; set; } //flowactual  实际流量 数值(12,4)        是
            public string flowexhaust { get; set; } //flowexhaust 尾气流量 数值(12,4)        是
            public string flowstd { get; set; } //flowstd 标准流量 数值(12,4)        是
            public string temperature { get; set; } //temperature 温度 数值(12,4)        是
            public string humidity { get; set; } //humidity    湿度 数值(12,4)        是
            public string pressure { get; set; } //pressure    气压 数值(12,4)        是
            public string flowmetert { get; set; } //flowmetert  流量计温度 数值(12,4)        是
            public string flowmeterp { get; set; } //flowmeterp  流量计压力 数值(12,4)        是
            public string dynpa { get; set; } //dynpa   底盘测功机总加载功率 数值(12,4)        是
            public string dynplhp { get; set; } //dynplhp 底盘测功机寄生功率 数值(12,4)        是
            public string dynihp { get; set; } //dynihp  底盘测功机指示功率 数值(12,4)        是
            public string dynn { get; set; } //dynn    底盘测功机扭力 数值(12,4)        是
            public string dcf { get; set; } //dcf 稀释修正系数 数值(12,4)        是
            public string kh { get; set; } //kh  湿度修正系数 数值(12,4)        是
            public string dr { get; set; } //dr  稀释比 数值(12,4)        是
            public string massco { get; set; } //massco  一氧化碳质量CO 数值(12,4)        是
            public string massco2 { get; set; } //massco2 二氧化碳质量CO2 数值(12,4)        是
            public string masshc { get; set; } //masshc  碳氢化合物质量HC 数值(12,4)        是
            public string massnox { get; set; } //massnox 氮氧化物质量NOX 数值(12,4)        是
            public string kjudge { get; set; } //kjudge  排放判定 字符(1)       FALSE
        }

        #endregion
        
        #endregion

        #region   7.2.3.1 燃油蒸发测试结果信息接口
        public class uploadInspResultEvap
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<ResultEvapDataItem> data { get; set; }
        }

        public class ResultEvapDataItem
        {
            public List<ResultEvapbodyItem> body { get; set; }
        }

        public class ResultEvapbodyItem
        {
            public string id { get; set; } //id 燃油蒸发测试结果记录id    字符(32)  燃油蒸发测试结果记录私有唯一标识 是
            public string TsNo { get; set; } //TsNo 检测机构编号  字符(16)  格式详见3定义部分 是
            public string inspregid { get; set; } //inspregid 检验登录ID  字符(32)  由环保局返回的检测登录唯一标识 是
            public string VIN { get; set; } //VIN 车辆识别代号  字符(32)  车辆识别代号VIN（须完整上报）	是
            public string EngineNo { get; set; } //EngineNo    发动机编号 字符(32)      是
            public string omitest { get; set; } //omitest 进口油测试 字符(1)   1：合格 0：不合格 是
            public string cascuptest { get; set; } //cascuptest 油箱盖测试   字符(1)   1：合格 0：不合格 是
            public string operatorName { get; set; } //operatorName 检测人姓名   字符(32)      是
            public string teststime { get; set; } //teststime   检测开始时间 时间  yyyyMMddHHmmss 是
            public string testetime { get; set; } //testetime 检测结束时间  时间 yyyyMMddHHmmss  是
            public string judge { get; set; } //judge   检测结果 字符(1)   1：合格 0：不合格 是
        }

        #endregion

        #region   7.2.4.1 检测设备基本信息接口
        public class uploadInspStmDevice
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<StmDeviceDataItem> data { get; set; }
        }

        public class StmDeviceDataItem
        {
            public List<StmDevicebodyItem> body { get; set; }
        }

        public class StmDevicebodyItem
        {
            public string id { get; set; } //id 唯一id    字符(32)      是
            public string TsNo { get; set; } //TsNo    检验机构编号 字符(16)      是
            public string TestLineNo { get; set; } //TestLineNo  检测线代编号 字符(16)      是
            public string deviceCode { get; set; } //deviceCode  设备编号 字符(40)  详见3.7	是
            public string name { get; set; } //name    设备名称 字符(60)      是
            public string testdevicetype { get; set; } //testdevicetype  检测设备类型 字符(1)   详见数据字典8.2序10 是
            public string series { get; set; } //series 系列  字符(40)      False
            public string model { get; set; } //model   型号 字符(40)      是
            public string spec { get; set; } //spec    规格 字符(255)     False
            public string manufcode { get; set; } //manufcode   制造商编码 字符(40)      False
            public string manufname { get; set; } //manufname   制造商名称 字符(255)     是
            public string manufdate { get; set; } //manufdate   生产日期 日期格式    yyyyMMdd 是
            public string vendorcode { get; set; } //vendorcode 供应商编码   字符(40)      是
            public string vendorname { get; set; } //vendorname  供应商名称 字符(255)     是
            public string boughtdate { get; set; } //boughtdate  买入时间 日期格式        是
            public string keeper { get; set; } //keeper  负责人 字符(40)      False
            public string certcode { get; set; } //certcode    认证编号 字符(40)      是
            public string certdate { get; set; } //certdate    认证时间 日期格式        是
            public string validdate { get; set; } //validdate   有效期止 日期格式        是
            public string score { get; set; } //score   得分 数值(8)       False
            public string ifcheck { get; set; } //ifcheck 是否需要自检 字符(1)   1：需要自检，2，不需要自检 是
            public string testDeviceCode { get; set; }
            public string testDeviceName { get; set; }

        }

        #endregion

        #region   7.2.4.2 检测设备运行记录接口
        //设备运行记录（包括1：自检 2：标定 3：时钟同步 4：检测）统一使用一个接口，字段说明参考 “7.2.3.2.3设备自检数据上报字段”、
        //“7.2.3.2.4设备标定数据上报字段”、“7.2.3.2.5设备检测数据上报字段”、“7.2.3.2.6设备时钟同步数据上报字段”。

        #region   设备自检
        public class uploadInspStmDeviceRecord<T> where T: class
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<T> data { get; set; }
        }
        public class uploadInspStmDeviceRecord测功机自检
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<StmDeviceRecordDataItem测功机自检> data { get; set; }
        }

        public class StmDeviceRecordDataItem测功机自检
        {
            public List<测功机自检bodyItem> body { get; set; }
        }
        public class 测功机自检bodyItem
        {
            public string id { get; set; } //id 唯一代码    字符(32)  主键，32位GUID 是
            public string typecode { get; set; } //typecode 运行类型    字符(2)   1：自检 是
            public string TsNo { get; set; } //TsNo 检测机构编号  字符(16)      是
            public string TestLineNo { get; set; } //TestLineNo  检测线 字符(16)      是
            public string testdevicecode { get; set; } //testdevicecode  设备编号 字符(40)  参考3.7	是
            public string testdevicetype { get; set; } //testdevicetype  设备类型 字符(40)      是
            public string testdevicename { get; set; } //testdevicename  设备名称 字符(40)      是
            public string opcode { get; set; } //opcode  操作项 字符(40)  详见数据字典8.2序24 是
            public string Operator { get; set; } //operator	操作人 字符(40)  操作人姓名 是
            public string reporttime { get; set; } //reporttime 上报时间    日期格式 yyyyMMddHHmmss  是
            public string optime { get; set; } //optime  自检时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string opstime { get; set; } //opstime 台体预热开始时间    日期格式 yyyyMMddHHmmss.SSS  是
            public string opetime { get; set; } //opetime 台体预热结束时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string status { get; set; } //status 设备状态    字符(2)   详见数据字典8.2序9 是
            public string result { get; set; } //result 运行结果    字符(1)   1：自检成功；0：自检失败 是
            public string memo { get; set; } //memo 备注  字符(255)     FALSE
            public string exti01 { get; set; } //exti01  通讯检查结果 字符(1)   1：成功 0：失败 是
            public string exti02 { get; set; } //exti02 举升器检查结果 字符(1)   1：成功 0：失败 是
            public string exti03 { get; set; } //exti03 加载滑行点数  数字（4）		是
            public string extn01 { get; set; } //extn01  惯性当量 数字（10,2）	Kg 是
            public string extn02 { get; set; } //extn02 加载滑行点数  数字（4）	加载滑行点数 是
            public string extn03 { get; set; } //extn03 车速区间（起）	数字（10,2）	km/h 是
            public string extn04 { get; set; } //extn04 车速区间（止）	数字（10,2）	km/h 是
            public string extn05 { get; set; } //extn05 加载功率1   数字（10,2）	kw 是
            public string extn06 { get; set; } //extn06 寄生功率1   数字（10,2）	kw 是
            public string extn07 { get; set; } //extn07 理论滑行时间1 数字（10,2）	S 是
            public string extn08 { get; set; } //extn08 实际滑行时间1 数字（10,2）	S 是
            public string extn09 { get; set; } //extn09 偏差1 数字（10,2）	%	是
            public string Extn10 { get; set; } //Extn10  车速区间（起）	数字（10,2）	kw 是
            public string extn11 { get; set; } //extn11 车速区间（止）	数字（10,2）	kw 是
            public string extn12 { get; set; } //extn12 加载功率2   数字（10,2）	kw 是
            public string extn13 { get; set; } //extn13 寄生功率2   数字（10,2）	kw 是
            public string extn14 { get; set; } //extn14 理论滑行时间2 数字（10,2）	S 是
            public string extn15 { get; set; } //extn15 实际滑行时间2 数字（10,2）	S 是
            public string extn16 { get; set; } //extn16 偏差2 数字（10,2）	%	是
        }

        public class uploadInspStmDeviceRecord五气分析仪自检
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public string lx { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<StmDeviceRecordDataItem五气分析仪自检> data { get; set; }
        }

        public class StmDeviceRecordDataItem五气分析仪自检
        {
            public List<五气分析仪自检bodyItem> body { get; set; }
        }
        public class 五气分析仪自检bodyItem
        {
            public string id { get; set; } //            id 唯一代码    字符(32)  主键，32位GUID 是
            public string typecode { get; set; } //typecode 运行类型    字符(2)   1：自检 是
            public string TsNo { get; set; } //TsNo 检测机构编号  字符(16)      是
            public string TestLineNo { get; set; } //TestLineNo  检测线 字符(16)      是
            public string testdevicecode { get; set; } //testdevicecode  设备编号 字符(40)  参考3.7	是
            public string testdevicetype { get; set; } //testdevicetype  设备类型 字符(40)      是
            public string testdevicename { get; set; } //testdevicename  设备名称 字符(40)      是
            public string opcode { get; set; } //opcode  操作项 字符(40)  详见数据字典8.2序24 是
            public string Operator { get; set; } //operator	操作人 字符(40)  操作人姓名 是
            public string reporttime { get; set; } //reporttime 上报时间    日期格式 yyyyMMddHHmmss  是
            public string optime { get; set; } //optime  自检时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string opstime { get; set; } //opstime 台体预热开始时间    日期格式 yyyyMMddHHmmss.SSS  是
            public string opetime { get; set; } //opetime 台体预热结束时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string status { get; set; } //status 设备状态    字符(2)   详见数据字典8.2序9 是
            public string result { get; set; } //result 运行结果    字符(1)   1：自检成功；0：自检失败 是
            public string memo { get; set; } //memo 备注  字符(255)     FALSE
            public string exti01 { get; set; } //exti01  通讯检查结果 字符(1)   1：成功 0：失败 是
            public string exti02 { get; set; } //exti02 仪器预热    字符(1)   1：成功 0：失败 是
            public string exti03 { get; set; } //exti03 仪器检漏    字符(1)   1：成功 0：失败 是
            public string exti04 { get; set; } //exti04 仪器调零    字符(1)   1：成功 0：失败 是
            public string extn01 { get; set; } //extn01 仪器流量    数字(10,2)    L/S 是
            public string extn02 { get; set; } //extn02 仪器氧气    数字(10,2)    %	是
        }

        public class uploadInspStmDeviceRecord烟度计自检
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public string lx { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<StmDeviceRecordDataItem烟度计自检> data { get; set; }
        }

        public class StmDeviceRecordDataItem烟度计自检
        {
            public List<烟度计自检bodyItem> body { get; set; }
        }
        public class 烟度计自检bodyItem
        {
            public string id { get; set; } //id 唯一代码    字符(32)  主键，32位GUID 是
            public string typecode { get; set; } //typecode 运行类型    字符(2)   1：自检 是
            public string TsNo { get; set; } //TsNo 检测机构编号  字符(16)      是
            public string TestLineNo { get; set; } //TestLineNo  检测线 字符(16)      是
            public string testdevicecode { get; set; } //testdevicecode  设备编号 字符(40)  参考3.7	是
            public string testdevicetype { get; set; } //testdevicetype  设备类型 字符(40)      是
            public string testdevicename { get; set; } //testdevicename  设备名称 字符(40)      是
            public string opcode { get; set; } //opcode  操作项 字符(40)  详见数据字典8.2序24 是
            public string Operator { get; set; } //operator	操作人 字符(40)  操作人姓名 是
            public string reporttime { get; set; } //reporttime 上报时间    日期格式 yyyyMMddHHmmss  是
            public string optime { get; set; } //optime  自检时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string opstime { get; set; } //opstime 台体预热开始时间    日期格式 yyyyMMddHHmmss.SSS  是
            public string opetime { get; set; } //opetime 台体预热结束时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string status { get; set; } //status 设备状态    字符(2)   详见数据字典8.2序9 是
            public string result { get; set; } //result 运行结果    字符(1)   1：自检成功；0：自检失败 是
            public string memo { get; set; } //memo 备注  字符(255)     FALSE
            public string exti01 { get; set; } //exti01  通讯检查结果 字符(1)   1：成功 0：失败 是
            public string exti02 { get; set; } //exti02 仪器预热    字符(1)   1：成功 0：失败 是
            public string exti03 { get; set; } //exti03 仪器调零    字符(1)   1：成功 0：失败 是
            public string exti04 { get; set; } //exti04 量程检查    字符(1)   1：成功 0：失败 是
            public string exti05 { get; set; } //exti05 检查点数    数字(4)       是
            public string extn01 { get; set; } //extn01  不透光设定值1 数字(10,2)    %	是
            public string extn02 { get; set; } //extn02  不透光实测值1 数字(10,2)    %	是
            public string extn03 { get; set; } //extn03  不透光偏差1 数字(10,2)    %	是
            public string extn04 { get; set; } //extn04  不透光设定值2 数字(10,2)    %	是
            public string extn05 { get; set; } //extn05  不透光实测值2 数字(10,2)    %	是
            public string extn06 { get; set; } //extn06  不透光偏差2 数字(10,2)    %	是
        }

        public class uploadInspStmDeviceRecord流量计自检
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public string lx { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<StmDeviceRecordDataItem流量计自检> data { get; set; }
        }

        public class StmDeviceRecordDataItem流量计自检
        {
            public List<流量计自检bodyItem> body { get; set; }
        }
        public class 流量计自检bodyItem
        {
            public string id { get; set; } //id 唯一代码    字符(32)  主键，32位GUID 是
            public string typecode { get; set; } //typecode 运行类型    字符(2)   1：自检 是
            public string TsNo { get; set; } //TsNo 检测机构编号  字符(16)      是
            public string TestLineNo { get; set; } //TestLineNo  检测线 字符(16)      是
            public string testdevicecode { get; set; } //testdevicecode  设备编号 字符(40)  参考3.7	是
            public string testdevicetype { get; set; } //testdevicetype  设备类型 字符(40)      是
            public string testdevicename { get; set; } //testdevicename  设备名称 字符(40)      是
            public string opcode { get; set; } //opcode  操作项 字符(40)  详见数据字典8.2序24 是
            public string Operator { get; set; } //operator	操作人 字符(40)  操作人姓名 是
            public string reporttime { get; set; } //reporttime 上报时间    日期格式 yyyyMMddHHmmss  是
            public string optime { get; set; } //optime  自检时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string opstime { get; set; } //opstime 台体预热开始时间    日期格式 yyyyMMddHHmmss.SSS  是
            public string opetime { get; set; } //opetime 台体预热结束时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string status { get; set; } //status 设备状态    字符(2)   详见数据字典8.2序9 是
            public string result { get; set; } //result 运行结果    字符(1)   1：自检成功；0：自检失败 是
            public string memo { get; set; } //memo 备注  字符(255)     FALSE
            public string exti01 { get; set; } //exti01  通讯检查结果 字符(1)   1：成功 0：失败 是
            public string exti02 { get; set; } //exti02 仪器预热    字符(1)   1：成功 0：失败 是
        }

        public class uploadInspStmDeviceRecord发动机转速仪自检
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public string lx { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<StmDeviceRecordDataItem发动机转速仪自检> data { get; set; }
        }

        public class StmDeviceRecordDataItem发动机转速仪自检
        {
            public List<发动机转速仪自检bodyItem> body { get; set; }
        }
        public class 发动机转速仪自检bodyItem
        {
            public string id { get; set; } //id 唯一代码    字符(32)  主键，32位GUID 是
            public string typecode { get; set; } //typecode 运行类型    字符(2)   1：自检 是
            public string TsNo { get; set; } //TsNo 检测机构编号  字符(16)      是
            public string TestLineNo { get; set; } //TestLineNo  检测线 字符(16)      是
            public string testdevicecode { get; set; } //testdevicecode  设备编号 字符(40)  参考3.7	是
            public string testdevicetype { get; set; } //testdevicetype  设备类型 字符(40)      是
            public string testdevicename { get; set; } //testdevicename  设备名称 字符(40)      是
            public string opcode { get; set; } //opcode  操作项 字符(40)  详见数据字典8.2序24 是
            public string Operator { get; set; } //Operator	操作人 字符(40)  操作人姓名 是
            public string reporttime { get; set; } //reporttime 上报时间    日期格式 yyyyMMddHHmmss  是
            public string optime { get; set; } //optime  自检时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string opstime { get; set; } //opstime 台体预热开始时间    日期格式 yyyyMMddHHmmss.SSS  是
            public string opetime { get; set; } //opetime 台体预热结束时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string status { get; set; } //status 设备状态    字符(2)   详见数据字典8.2序9 是
            public string result { get; set; } //result 运行结果    字符(1)   1：自检成功；0：自检失败 是
            public string memo { get; set; } //memo 备注  字符(255)     FALSE
            public string exti01 { get; set; } //exti01  通讯检查结果 字符(1)   1：成功 0：失败 是
            public string exti02 { get; set; } //exti02 怠速转速    字符(4)       是

        }


         public class uploadInspStmDeviceRecord电子环境信息仪自检
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public string lx { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<StmDeviceRecordDataItem电子环境信息仪自检> data { get; set; }
        }

        public class StmDeviceRecordDataItem电子环境信息仪自检
        {
            public List<电子环境信息仪自检bodyItem> body { get; set; }
        }
        public class 电子环境信息仪自检bodyItem
        {
            public string id { get; set; } //            id 唯一代码    字符(32)  主键，32位GUID 是
            public string typecode { get; set; } //typecode 运行类型    字符(2)   1：自检 是
            public string TsNo { get; set; } //TsNo 检测机构编号  字符(16)      是
            public string TestLineNo { get; set; } //TestLineNo  检测线 字符(16)      是
            public string testdevicecode { get; set; } //testdevicecode  设备编号 字符(40)  参考3.7	是
            public string testdevicetype { get; set; } //testdevicetype  设备类型 字符(40)      是
            public string testdevicename { get; set; } //testdevicename  设备名称 字符(40)      是
            public string opcode { get; set; } //opcode  操作项 字符(40)  详见数据字典8.2序24 是
            public string Operator { get; set; } //operator	操作人 字符(40)  操作人姓名 是
            public string reporttime { get; set; } //reporttime 上报时间    日期格式 yyyyMMddHHmmss  是
            public string optime { get; set; } //optime  自检时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string opstime { get; set; } //opstime 台体预热开始时间    日期格式 yyyyMMddHHmmss.SSS  是
            public string opetime { get; set; } //opetime 台体预热结束时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string status { get; set; } //status 设备状态    字符(2)   详见数据字典8.2序9 是
            public string result { get; set; } //result 运行结果    字符(1)   1：自检成功；0：自检失败 是
            public string memo { get; set; } //memo 备注  字符(255)     FALSE
            public string exti01 { get; set; } //exti01  通讯检查结果 字符(1)   1：成功 0：失败 是
            public string exti02 { get; set; } //exti02 仪器预热    字符(1)   1：成功 0：失败 是
            public string extn01 { get; set; } //extn01 环境温度    数字(10,2)    摄氏度 是
            public string extn02 { get; set; } //extn02 仪器温度    数字(10,2)    摄氏度 是
            public string extn03 { get; set; } //extn03 环境湿度    数字(10,2)    %	是
            public string extn04 { get; set; } //extn04  仪器湿度 数字(10,2)    %	是
            public string extn05 { get; set; } //extn05  环境气压 数字(10,2)    kpa 是
            public string extn06 { get; set; } //extn06 仪器气压    数字(10,2)    kpa 是
        }

        #endregion
        
        #region   设备标定
        public class uploadInspStmDeviceRecord车速标定
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public string lx { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<StmDeviceRecordDataItem车速标定> data { get; set; }
        }

        public class StmDeviceRecordDataItem车速标定
        {
            public List<车速标定bodyItem> body { get; set; }
        }
        public class 车速标定bodyItem
        {
            public string id { get; set; } //            id 唯一代码    字符(32)  主键，32位GUID 是
            public string typecode { get; set; } //typecode 运行类型    字符(2)   2：标定 是
            public string TsNo { get; set; } //TsNo 检测机构编号  字符(16)      是
            public string TestLineNo { get; set; } //TestLineNo  检测线 字符(16)      是
            public string testdevicecode { get; set; } //testdevicecode  设备编号 字符(40)  参考3.7	是
            public string testdevicetype { get; set; } //testdevicetype  设备类型 字符(40)      是
            public string testdevicename { get; set; } //testdevicename  设备名称 字符(40)      是
            public string opcode { get; set; } //opcode  操作项 字符(40)  详见数据字典8.2序24 是
            public string Operator { get; set; } //operator	操作人 字符(40)  操作人姓名 是
            public string reporttime { get; set; } //reporttime 上报时间    日期格式 yyyyMMddHHmmss  是
            public string optime { get; set; } //optime  标定时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string opstime { get; set; } //opstime 标定开始时间  日期格式 yyyyMMddHHmmss.SSS  是
            public string opetime { get; set; } //opetime 标定结束时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string status { get; set; } //status 设备状态    字符(2)   详见数据字典8.2序9 是
            public string result { get; set; } //result 运行结果    字符(1)   1：标定成功；0：标定失败 是
            public string memo { get; set; } //memo 备注  字符(255) 不成功原因 FALSE
            public string exti01 { get; set; } //exti01 标定点数    字符(4)       是
            public string extn01 { get; set; } //extn01  设定值1 数字（10,2）		是
            public string extn02 { get; set; } //extn02  实测值1 数字（10,2）		是
            public string extn03 { get; set; } //extn03  误差1 数字（10,2）		是
            public string extn04 { get; set; } //extn04  设定值2 数字（10,2）		是
            public string extn05 { get; set; } //extn05  实测值2 数字（10,2）		是
            public string extn06 { get; set; } //extn06  误差2 数字（10,2）		是
            public string extn07 { get; set; } //extn07  设定值3 数字（10,2）		是
            public string extn08 { get; set; } //extn08  实测值3 数字（10,2）		是
            public string extn09 { get; set; } //extn09  误差3 数字（10,2）		是
        }


        public class uploadInspStmDeviceRecord扭力标定
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public string lx { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<StmDeviceRecordDataItem扭力标定> data { get; set; }
        }

        public class StmDeviceRecordDataItem扭力标定
        {
            public List<扭力标定bodyItem> body { get; set; }
        }

        public class 扭力标定bodyItem
        {
            public string id { get; set; } //id 唯一代码    字符(32)  主键，32位GUID 是
            public string typecode { get; set; } //typecode 运行类型    字符(2)   2：标定 是
            public string TsNo { get; set; } //TsNo 检测机构编号  字符(16)      是
            public string TestLineNo { get; set; } //TestLineNo  检测线 字符(16)      是
            public string testdevicecode { get; set; } //testdevicecode  设备编号 字符(40)  参考3.7	是
            public string testdevicetype { get; set; } //testdevicetype  设备类型 字符(40)      是
            public string testdevicename { get; set; } //testdevicename  设备名称 字符(40)      是
            public string opcode { get; set; } //opcode  操作项 字符(40)  详见数据字典8.2序24 是
            public string Operator { get; set; } //operator	操作人 字符(40)  操作人姓名 是
            public string reporttime { get; set; } //reporttime 上报时间    日期格式 yyyyMMddHHmmss  是
            public string optime { get; set; } //optime  标定时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string opstime { get; set; } //opstime 标定开始时间  日期格式 yyyyMMddHHmmss.SSS  是
            public string opetime { get; set; } //opetime 标定结束时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string status { get; set; } //status 设备状态    字符(2)   详见数据字典8.2序9 是
            public string result { get; set; } //result 运行结果    字符(1)   1：标定成功；0：标定失败 是
            public string memo { get; set; } //memo 备注  字符(255) 不成功原因 FALSE
            public string exti01 { get; set; } //exti01 标定点数    字符(4)       是
            public string extn01 { get; set; } //extn01  设定值1 数字（10,2）		是
            public string extn02 { get; set; } //extn02  实测值1 数字（10,2）		是
            public string extn03 { get; set; } //extn03  误差1 数字（10,2）		是
            public string extn04 { get; set; } //extn04  设定值2 数字（10,2）		是
            public string extn05 { get; set; } //extn05  实测值2 数字（10,2）		是
            public string extn06 { get; set; } //extn06  误差2 数字（10,2）		是
            public string extn07 { get; set; } //extn07  设定值3 数字（10,2）		是
            public string extn08 { get; set; } //extn08  实测值3 数字（10,2）		是
            public string extn09 { get; set; } //extn09  误差3 数字（10,2）		是
            public string extn10 { get; set; } //extn10  设定值4 数字（10,2）		是
            public string extn11 { get; set; } //extn11  实测值4 数字（10,2）		是
            public string extn12 { get; set; } //extn12  误差4 数字（10,2）		是
            public string extn13 { get; set; } //extn13  设定值5 数字（10,2）		是
            public string extn14 { get; set; } //extn14  实测值5 数字（10,2）		是
            public string extn15 { get; set; } //extn15  误差5 数字（10,2）		是
        }

        
        public class uploadInspStmDeviceRecord寄生功率标定
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public string lx { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<StmDeviceRecordDataItem寄生功率标定> data { get; set; }
        }

        public class StmDeviceRecordDataItem寄生功率标定
        {
            public List<寄生功率标定bodyItem> body { get; set; }
        }
        
        public class 寄生功率标定bodyItem
        {
            public string id { get; set; }//            唯一代码    字符(32)  主键，32位GUID 是
            public string typecode { get; set; }// 运行类型    字符(2)   2：标定 是
            public string TsNo { get; set; }// 检测机构编号  字符(16)      是
            public string TestLineNo { get; set; }//  检测线 字符(16)      是
            public string testdevicecode { get; set; }//  设备编号 字符(40)  参考3.7	是
            public string testdevicetype { get; set; }// 设备类型 字符(40)      是
            public string testdevicename { get; set; }// 设备名称 字符(40)      是
            public string opcode { get; set; }//  操作项 字符(40)  详见数据字典8.2序24 是
            public string Operator{ get; set; }//  操作人 字符(40)  操作人姓名 是
            public string reporttime { get; set; }// 上报时间    日期格式 yyyyMMddHHmmss  是
            public string optime { get; set; }// 标定时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string opstime { get; set; }// 标定开始时间  日期格式 yyyyMMddHHmmss.SSS  是
            public string opetime { get; set; }// 标定结束时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string status { get; set; }//设备状态    字符(2)   详见数据字典8.2序9 是
            public string result { get; set; }// 运行结果    字符(1)   1：标定成功；0：标定失败 是
            public string memo { get; set; }//备注  字符(255) 不成功原因 FALSE
            public string extnum { get; set; }//惯性当量    数字（10,2）	Kg
            public string exti01 { get; set; }// 标定点数 字符(4)       是
            public string extn01 { get; set; }// 车速区间（起）	数字（10,2）	km/h 是
            public string extn02 { get; set; }// 车速区间（止）	数字（10,2）	km/h 是
            public string extn03 { get; set; }// 滑行时间1   数字（10,2）	s 是
            public string extn04 { get; set; }// 寄生功率1   数字（10,2）	kw 是
            public string extn05 { get; set; }// 车速区间2（起）	数字（10,2）	km/h 是
            public string extn06 { get; set; }// 车速区间2（止）	数字（10,2）	km/h 是
            public string extn07 { get; set; }// 滑行时间2   数字（10,2）	s 是
            public string extn08 { get; set; }// 寄生功率2   数字（10,2）	kw 是
            public string extn09 { get; set; }//车速区间3（起）	数字（10,2）	km/h 是
            public string extn10 { get; set; }//车速区间3（止）	数字（10,2）	km/h 是
            public string extn11 { get; set; }// 滑行时间3   数字（10,2）	s 是
            public string extn12 { get; set; }//寄生功率3   数字（10,2）	kw 是
            public string extn13 { get; set; }// 车速区间4（起）	数字（10,2）	km/h 是
            public string extn14 { get; set; }// 车速区间4（止）	数字（10,2）	km/h 是
            public string extn15 { get; set; }// 滑行时间4   数字（10,2）	s 是
            public string extn16 { get; set; }// 寄生功率4   数字（10,2）	kw 是
            public string extn17 { get; set; }// 车速区间5（起）	数字（10,2）	km/h 是
            public string extn18 { get; set; }// 车速区间5（止）	数字（10,2）	km/h 是
            public string extn19 { get; set; }//滑行时间5   数字（10,2）	s 是
            public string extn20 { get; set; }// 寄生功率5   数字（10,2）	kw 是

        }

        public class uploadInspStmDeviceRecord加载滑行率标定
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public string lx { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<StmDeviceRecordDataItem加载滑行率标定> data { get; set; }
        }

        public class StmDeviceRecordDataItem加载滑行率标定
        {
            public List<加载滑行率标定bodyItem> body { get; set; }
        }
        public class 加载滑行率标定bodyItem
        {
            public string id { get; set; } //id 唯一代码    字符(32)  主键，32位GUID 是
            public string typecode { get; set; } //typecode 运行类型    字符(2)   2：标定 是
            public string TsNo { get; set; } //TsNo 检测机构编号  字符(16)      是
            public string TestLineNo { get; set; } //TestLineNo  检测线 字符(16)      是
            public string testdevicecode { get; set; } //testdevicecode  设备编号 字符(40)  参考3.7	是
            public string testdevicetype { get; set; } //testdevicetype  设备类型 字符(40)      是
            public string testdevicename { get; set; } //testdevicename  设备名称 字符(40)      是
            public string opcode { get; set; } //opcode  操作项 字符(40)  详见数据字典8.2序24 是
            public string Operator { get; set; } //operator	操作人 字符(40)  操作人姓名 是
            public string reporttime { get; set; } //reporttime 上报时间    日期格式 yyyyMMddHHmmss  是
            public string optime { get; set; } //optime  标定时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string opstime { get; set; } //opstime 标定开始时间  日期格式 yyyyMMddHHmmss.SSS  是
            public string opetime { get; set; } //opetime 标定结束时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string status { get; set; } //status 设备状态    字符(2)   详见数据字典8.2序9 是
            public string result { get; set; } //result 运行结果    字符(1)   1：标定成功；0：标定失败 是
            public string memo { get; set; } //memo 备注  字符(255) 不成功原因 FALSE
            public string extnum { get; set; } //extnum 惯性当量    数字（10,2）	Kg
            public string exti01 { get; set; } //exti01  标定点数 字符(4)       是
            public string extn01 { get; set; } //extn01  车速区间（起）	数字（10,2）	km/h 是
            public string extn02 { get; set; } //extn02 车速区间（止）	数字（10,2）	km/h 是
            public string extn03 { get; set; } //extn03 滑行时间1   数字（10,2）	s 是
            public string extn04 { get; set; } //extn04 寄生功率1   数字（10,2）	kw 是
            public string extn05 { get; set; } //extn05 车速区间2（起）	数字（10,2）	km/h 是
            public string extn06 { get; set; } //extn06 车速区间2（止）	数字（10,2）	km/h 是
            public string extn07 { get; set; } //extn07 滑行时间2   数字（10,2）	s 是
            public string extn08 { get; set; } //extn08 寄生功率2   数字（10,2）	kw 是
            public string extn09 { get; set; } //extn09 车速区间3（起）	数字（10,2）	km/h 是
            public string extn10 { get; set; } //extn10 车速区间3（止）	数字（10,2）	km/h 是
            public string extn11 { get; set; } //extn11 滑行时间3   数字（10,2）	s 是
            public string extn12 { get; set; } //extn12 寄生功率3   数字（10,2）	kw 是
            public string extn13 { get; set; } //extn13 车速区间4（起）	数字（10,2）	km/h 是
            public string extn14 { get; set; } //extn14 车速区间4（止）	数字（10,2）	km/h 是
            public string extn15 { get; set; } //extn15 滑行时间4   数字（10,2）	s 是
            public string extn16 { get; set; } //extn16 寄生功率4   数字（10,2）	kw 是
            public string extn17 { get; set; } //extn17 车速区间5（起）	数字（10,2）	km/h 是
            public string extn18 { get; set; } //extn18 车速区间5（止）	数字（10,2）	km/h 是
            public string extn19 { get; set; } //extn19 滑行时间5   数字（10,2）	s 是
            public string extn20 { get; set; } //extn20 寄生功率5   数字（10,2）	kw 是
        }

        public class uploadInspStmDeviceRecord废气标定
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public string lx { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<StmDeviceRecordDataItem废气标定> data { get; set; }
        }

        public class StmDeviceRecordDataItem废气标定
        {
            public List<废气标定bodyItem> body { get; set; }
        }
        public class 废气标定bodyItem
        {
            public string id { get; set; } //id 唯一代码    字符(32)  主键，32位GUID 是
            public string typecode { get; set; } //typecode 运行类型    字符(2)   2：标定 是
            public string TsNo { get; set; } //TsNo 检测机构编号  字符(16)      是
            public string TestLineNo { get; set; } //TestLineNo  检测线 字符(16)      是
            public string testdevicecode { get; set; } //testdevicecode  设备编号 字符(40)  参考3.7	是
            public string testdevicetype { get; set; } //testdevicetype  设备类型 字符(40)      是
            public string testdevicename { get; set; } //testdevicename  设备名称 字符(40)      是
            public string opcode { get; set; } //opcode  操作项 字符(40)  详见数据字典8.2序24 是
            public string Operator { get; set; } //operator	操作人 字符(40)  操作人姓名 是
            public string reporttime { get; set; } //reporttime 上报时间    日期格式 yyyyMMddHHmmss  是
            public string optime { get; set; } //optime  标定时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string opstime { get; set; } //opstime 标定开始时间  日期格式 yyyyMMddHHmmss.SSS  是
            public string opetime { get; set; } //opetime 标定结束时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string status { get; set; } //status 设备状态    字符(2)   详见数据字典8.2序9 是
            public string result { get; set; } //result 运行结果    字符(1)   1：标定成功；0：标定失败 是
            public string memo { get; set; } //memo 备注  字符(255) 正常为空 FALSE
            public string extnum { get; set; } //extnum 标定气浓度   数字（10,2）		
            public string exti01 { get; set; } //exti01 标定点数    字符(4)   第N次标定点数，第二次为一个新记录 是
            public string extn01 { get; set; } //extn01 HC设定值1  数字（10,2）		是
            public string extn02 { get; set; } //extn02  HC实测值1 数字（10,2）		是
            public string extn03 { get; set; } //extn03  HC偏差1 数字（10,2）	%	是
            public string extn04 { get; set; } //extn04  CO设定值1 数字（10,2）		是
            public string extn05 { get; set; } //extn05  CO实测值1 数字（10,2）		是
            public string extn06 { get; set; } //extn06  CO偏差1 数字（10,2）	%	是
            public string extn07 { get; set; } //extn07  CO2设定值1 数字（10,2）		是
            public string extn08 { get; set; } //extn08  CO2实测值1 数字（10,2）		是
            public string extn09 { get; set; } //extn09  CO2偏差1 数字（10,2）	%	是
            public string extn10 { get; set; } //extn10  NO设定值1 数字（10,2）		是
            public string extn11 { get; set; } //extn11  NO实测值1 数字（10,2）		是
            public string extn12 { get; set; } //extn12  NO偏差1 数字（10,2）	%	是
            public string extn13 { get; set; } //extn13  HC设定值2 数字（10,2）		是
            public string extn14 { get; set; } //extn14  HC实测值2 数字（10,2）		是
            public string extn15 { get; set; } //extn15  HC偏差2 数字（10,2）	%	是
        }

        public class uploadInspStmDeviceRecord烟度标定
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public string lx { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<StmDeviceRecordDataItem烟度标定> data { get; set; }
        }

        public class StmDeviceRecordDataItem烟度标定
        {
            public List<烟度标定bodyItem> body { get; set; }
        }
        public class 烟度标定bodyItem
        {
            public string id { get; set; } //id 唯一代码    字符(32)  主键，32位GUID 是
            public string typecode { get; set; } //typecode 运行类型    字符(2)   2：标定 是
            public string TsNo { get; set; } //TsNo 检测机构编号  字符(16)      是
            public string TestLineNo { get; set; } //TestLineNo  检测线 字符(16)      是
            public string testdevicecode { get; set; } //testdevicecode  设备编号 字符(40)  参考3.7	是
            public string testdevicetype { get; set; } //testdevicetype  设备类型 字符(40)      是
            public string testdevicename { get; set; } //testdevicename  设备名称 字符(40)      是
            public string opcode { get; set; } //opcode  操作项 字符(40)  详见数据字典8.2序24 是
            public string Operator { get; set; } //operator	操作人 字符(40)  操作人姓名 是
            public string reporttime { get; set; } //reporttime 上报时间    日期格式 yyyyMMddHHmmss  是
            public string optime { get; set; } //optime  标定时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string opstime { get; set; } //opstime 标定开始时间  日期格式 yyyyMMddHHmmss.SSS  是
            public string opetime { get; set; } //opetime 标定结束时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string status { get; set; } //status 设备状态    字符(2)   详见数据字典8.2序9 是
            public string result { get; set; } //result 运行结果    字符(1)   1：标定成功；0：标定失败 是
            public string memo { get; set; } //memo 备注  字符(255) 正常为空 FALSE
            public string exti01 { get; set; } //exti01 标定点数    字符(4)       是
            public string extn01 { get; set; } //extn01  不透光设定值1 数字（10,2）	%	是
            public string extn02 { get; set; } //extn02  不透光实测值1 数字（10,2）	%	是
            public string extn03 { get; set; } //extn03  不透光偏差1 数字（10,2）	%	是
            public string extn04 { get; set; } //extn04  光吸收数设定值1 数字（10,2）	m 是
            public string extn05 { get; set; } //extn05 光吸收数实测值1    数字（10,2）	m 是
            public string extn06 { get; set; } //extn06 光吸收数偏差1 数字（10,2）	%	是
            public string extn07 { get; set; } //extn07  不透光设定值2 数字（10,2）		是
            public string extn08 { get; set; } //extn08  不透光实测值2 数字（10,2）		是
            public string extn09 { get; set; } //extn09  不透光偏差2 数字（10,2）	%	是
            public string extn10 { get; set; } //extn10  光吸收数设定2 数字（10,2）		是
            public string extn11 { get; set; } //extn11  光吸收数实测2 数字（10,2）		是
            public string extn12 { get; set; } //extn12  光吸收偏差2 数字（10,2）		是
        }

        


        #endregion

        #region   设备检查
        public class uploadInspStmDeviceRecord加载滑行检查
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public string lx { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<StmDeviceRecordDataItem加载滑行检查> data { get; set; }
        }

        public class StmDeviceRecordDataItem加载滑行检查
        {
            public List<加载滑行检查bodyItem> body { get; set; }
        }
        public class 加载滑行检查bodyItem
        {
            public string id { get; set; } //id 唯一代码    字符(32)  主键，32位GUID 是
            public string typecode { get; set; } //typecode 运行类型    字符(2)   4：检测 是
            public string TsNo { get; set; } //TsNo 检测机构编号  字符(16)      是
            public string TestLineNo { get; set; } //TestLineNo  检测线 字符(16)      是
            public string testdevicecode { get; set; } //testdevicecode  设备编号 字符(40)  参考3.7	是
            public string testdevicetype { get; set; } //testdevicetype  设备类型 字符(40)  详见数据字典8.2序10 是
            public string testdevicename { get; set; } //testdevicename 设备名称    字符(40)      是
            public string opcode { get; set; } //opcode  操作项 字符(40)  详见数据字典8.2序24 是
            public string Operator { get; set; } //operator	操作人 字符(40)  操作人姓名 是
            public string reporttime { get; set; } //reporttime 上报时间    日期格式 yyyyMMddHHmmss  是
            public string optime { get; set; } //optime  操作时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string opstime { get; set; } //opstime 滑行检查开始时间    日期格式 开始时间是滚筒转速下降到48km/h开始的时间 是
            public string opetime { get; set; } //opetime 滑行检查结束时间    日期格式 yyyyMMddHHmmss.SSS  是
            public string status { get; set; } //status  设备状态 字符(2)   详见数据字典8.2序9 是
            public string result { get; set; } //result 检查结果    字符(1)   1：成功；0：失败 是
            public string memo { get; set; } //memo 不成功原因等  字符(255) 正常为空 FALSE
            public string extnum { get; set; } //extnum 基本惯性    数字(10,2)    Kg 是
            public string exti01 { get; set; } //exti01	48-32km/h滑行检查结果 字符(1)   0-不合格、1-合格 是
            public string exti02 { get; set; } //exti02	32-16km/h滑行检查结果 字符(1)   0-不合格、1-合格 是
            public string extn01 { get; set; } //extn01	48-32km/h实际滑行时间 数字(10,2)    ACDT40，ms 是
            public string extn02 { get; set; } //extn02	32-16km/h实际滑行时间 数字(10,2)    ACDT25，ms 是
            public string extn03 { get; set; } //extn03	40km/h时的内损 数字(10,2)    PLHP40，kW 是
            public string extn04 { get; set; } //extn04	25km/h时的内损 数字(10,2)    PLHP25，kW 是
            public string extn05 { get; set; } //extn05	48-32km/h名义滑行时间 数字(10,2)    CCDT40，ms 是
            public string extn06 { get; set; } //extn06	32-16km/h名义滑行时间 数字(10,2)    CCDT25，ms 是
            public string extn07 { get; set; } //extn07	48-32km/h滑行指示功率 数字(10,2)    IHP40，kW 是
            public string extn08 { get; set; } //extn08	32-16km/h滑行指示功率 数字(10,2)    IHP25，kW 是
        }

        public class uploadInspStmDeviceRecord附加功率损失检查
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public string lx { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<StmDeviceRecordDataItem附加功率损失检查> data { get; set; }
        }

        public class StmDeviceRecordDataItem附加功率损失检查
        {
            public List<附加功率损失检查bodyItem> body { get; set; }
        }
        public class 附加功率损失检查bodyItem
        {
            public string id { get; set; } //id 唯一代码    字符(32)  主键，32位GUID 是
            public string typecode { get; set; } //typecode 运行类型    字符(2)   4：检测 是
            public string TsNo { get; set; } //TsNo 检测机构编号  字符(16)      是
            public string TestLineNo { get; set; } //TestLineNo  检测线 字符(16)      是
            public string testdevicecode { get; set; } //testdevicecode  设备编号 字符(40)  参考3.7	是
            public string testdevicetype { get; set; } //testdevicetype  设备类型 字符(40)  详见数据字典8.2序10 是
            public string testdevicename { get; set; } //testdevicename 设备名称    字符(40)      是
            public string opcode { get; set; } //opcode  操作项 字符(40)  详见数据字典8.2序24 是
            public string Operator { get; set; } //operator	操作人 字符(40)  操作人姓名 是
            public string reporttime { get; set; } //reporttime 上报时间    日期格式 yyyyMMddHHmmss  是
            public string optime { get; set; } //optime  操作时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string opstime { get; set; } //opstime 滑行检查开始时间    日期格式 开始时间是滚筒转速下降到48km/h开始的时间 是
            public string opetime { get; set; } //opetime 滑行检查结束时间    日期格式 yyyyMMddHHmmss.SSS  是
            public string status { get; set; } //status  设备状态 字符(2)   详见数据字典8.2序9 是
            public string result { get; set; } //result 检查结果    字符(1)   1：成功；0：失败 是
            public string memo { get; set; } //memo 不成功原因等  字符(255) 正常为空 是
            public string extnum { get; set; } //extnum 基本惯量    数字(10,2)    Kg 是
            public string extn01 { get; set; } //extn01	48-32km/h实际滑行时间 字符(1)   ACDT40，ms 是
            public string extn02 { get; set; } //extn02	32-16km/h实际滑行时间 字符(1)   ACDT25，ms 是
            public string extn03 { get; set; } //extn03	40km/h时的内损 数字(10,2)    PLHP40，kW 是
            public string extn04 { get; set; } //extn04	25km/h时的内损 数字(10,2)    PLHP25，kW 是
        }

        public class uploadInspStmDeviceRecord分析仪检查
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public string lx { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<StmDeviceRecordDataItem分析仪检查> data { get; set; }
        }

        public class StmDeviceRecordDataItem分析仪检查
        {
            public List<分析仪检查bodyItem> body { get; set; }
        }
        public class 分析仪检查bodyItem
        {
            public string id { get; set; } //id 唯一代码    字符(32)  主键，32位GUID 是
            public string typecode { get; set; } //typecode 运行类型    字符(2)   4：检测 是
            public string TsNo { get; set; } //TsNo 检测机构编号  字符(16)      是
            public string TestLineNo { get; set; } //TestLineNo  检测线 字符(16)      是
            public string testdevicecode { get; set; } //testdevicecode  设备编号 字符(40)  参考3.7	是
            public string testdevicetype { get; set; } //testdevicetype  设备类型 字符(40)  详见数据字典8.2序10 是
            public string testdevicename { get; set; } //testdevicename 设备名称    字符(40)      是
            public string opcode { get; set; } //opcode  操作项 字符(40)  详见数据字典8.2序24 是
            public string Operator { get; set; } //operator	操作人 字符(40)  操作人姓名 是
            public string reporttime { get; set; } //reporttime 上报时间    日期格式 yyyyMMddHHmmss  是
            public string optime { get; set; } //optime  操作时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string opstime { get; set; } //opstime 检查开始时间  日期格式 开始时间是是从通气开始yyyyMMddHHmmss.SSS   是
            public string opetime { get; set; } //opetime 检查结束时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string status { get; set; } //status 设备状态    字符(2)   详见数据字典8.2序9 是
            public string result { get; set; } //result 检查结果    字符(1)   1：成功；0：失败 是
            public string memo { get; set; } //memo 不成功原因等  字符(255) 正常为空 FALSE
            public string extnum { get; set; } //extnum 检查类型    字符(2)   1-低浓度、2-中低浓度、3-中高浓度 4-高浓度、5-零度 是
            public string extn01 { get; set; } //extn01 标准气C3H8浓度   数字(10,2)    10-6	是
            public string extn02 { get; set; } //extn02  标准气CO浓度 数字(10,2)    %	是
            public string extn03 { get; set; } //extn03  标准气CO2浓度 数字(10,2)    %	是
            public string extn04 { get; set; } //extn04  标准气NO浓度 数字(10,2)    10-6	是
            public string extn05 { get; set; } //extn05  标准气O2浓度 数字(10,2)    %	是
            public string extn06 { get; set; } //extn06  HC检查结果值 数字(10,2)    10-6	是
            public string extn07 { get; set; } //extn07  CO检查结果值 数字(10,2)    %	是
            public string extn08 { get; set; } //extn08  CO2检查结果值 数字(10,2)    %	是
            public string extn09 { get; set; } //extn09  NO检查结果值 数字(10,2)    10-6	是
            public string extn10 { get; set; } //extn10  O2检查结果值 数字(10,2)    %	是
            public string extn11 { get; set; } //extn11  PEF值 数字(10,2)        是
        }


        public class uploadInspStmDeviceRecord泄露检查
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public string lx { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<StmDeviceRecordDataItem泄露检查> data { get; set; }
        }

        public class StmDeviceRecordDataItem泄露检查
        {
            public List<泄露检查bodyItem> body { get; set; }
        }
        public class 泄露检查bodyItem
        {
            public string id { get; set; } //id 唯一代码    字符(32)  主键，32位GUID 是
            public string typecode { get; set; } //typecode 运行类型    字符(2)   4：检测 是
            public string TsNo { get; set; } //TsNo 检测机构编号  字符(16)      是
            public string TestLineNo { get; set; } //TestLineNo  检测线 字符(16)      是
            public string testdevicecode { get; set; } //testdevicecode  设备编号 字符(40)  参考3.7	是
            public string testdevicetype { get; set; } //testdevicetype  设备类型 字符(40)  详见数据字典8.2序10 是
            public string testdevicename { get; set; } //testdevicename 设备名称    字符(40)      是
            public string opcode { get; set; } //opcode  操作项 字符(40)  详见数据字典8.2序24 是
            public string Operator { get; set; } //operator	操作人 字符(40)  操作人姓名 是
            public string reporttime { get; set; } //reporttime 上报时间    日期格式 yyyyMMddHHmmss  是
            public string optime { get; set; } //optime  操作时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string opstime { get; set; } //opstime 检查开始时间  日期格式 检查开始的时间yyyyMMddHHmmss.SSS   是
            public string opetime { get; set; } //opetime 检查结束时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string status { get; set; } //status 设备状态    字符(2)   详见数据字典8.2序9 是
            public string result { get; set; } //result 检查结果    字符(1)   1：成功；0：失败 是
            public string memo { get; set; } //memo 不成功原因等  字符(255) 正常为空 FALSE
        }

         public class uploadInspStmDeviceRecord分析仪氧量程检查
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public string lx { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<StmDeviceRecordDataItem分析仪氧量程检查> data { get; set; }
        }

        public class StmDeviceRecordDataItem分析仪氧量程检查
        {
            public List<分析仪氧量程检查bodyItem> body { get; set; }
        }
        public class 分析仪氧量程检查bodyItem
        {
            public string id { get; set; } //id 唯一代码    字符(32)  主键，32位GUID 是
            public string typecode { get; set; } //typecode 运行类型    字符(2)   4：检测 是
            public string TsNo { get; set; } //TsNo 检测机构编号  字符(16)      是
            public string TestLineNo { get; set; } //TestLineNo  检测线 字符(16)      是
            public string testdevicecode { get; set; } //testdevicecode  设备编号 字符(40)  参考3.7	是
            public string testdevicetype { get; set; } //testdevicetype  设备类型 字符(40)  详见数据字典8.2序10 是
            public string testdevicename { get; set; } //testdevicename 设备名称    字符(40)      是
            public string opcode { get; set; } //opcode  操作项 字符(40)  详见数据字典8.2序24 是
            public string Operator { get; set; } //Operator	操作人 字符(40)  操作人姓名 是
            public string reporttime { get; set; } //reporttime 上报时间    日期格式 yyyyMMddHHmmss  是
            public string optime { get; set; } //optime  操作时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string opstime { get; set; } //opstime 滑行检查开始时间    日期格式 开始时间是滚筒转速下降到48km/h开始的时间 是
            public string opetime { get; set; } //opetime 滑行检查结束时间    日期格式 yyyyMMddHHmmss.SSS  是
            public string status { get; set; } //status  设备状态 字符(2)   详见数据字典8.2序9 是
            public string result { get; set; } //result 检查结果    字符(1)   1：成功；0：失败 是
            public string memo { get; set; } //memo 不成功原因等  字符(255) 正常为空 FALSE
            public string extn01 { get; set; } //extn01 氧气量程标值下限    数字(10,2)        是
            public string extn02 { get; set; } //extn02  氧气量程标值上限 数字(10,2)        是
            public string extn03 { get; set; } //extn03  氧气量程测量值下限 数字(10,2)        是
            public string extn04 { get; set; } //extn04  氧气量程测量值上限 数字(10,2)        是
            public string extn05 { get; set; } //extn05  氧气量程下限误差 数字(10,2)        是
            public string extn06 { get; set; } //extn06  氧气量程上限误差 数字(10,2)        是
        }

        public class uploadInspStmDeviceRecord流量计检查
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public string lx { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<StmDeviceRecordDataItem流量计检查> data { get; set; }
        }

        public class StmDeviceRecordDataItem流量计检查
        {
            public List<流量计检查bodyItem> body { get; set; }
        }
        public class 流量计检查bodyItem
        {
            public string id { get; set; } //id 唯一代码    字符(32)  主键，32位GUID 是
            public string typecode { get; set; } //typecode 运行类型    字符(2)   4：检测 是
            public string TsNo { get; set; } //TsNo 检测机构编号  字符(16)      是
            public string TestLineNo { get; set; } //TestLineNo  检测线 字符(16)      是
            public string testdevicecode { get; set; } //testdevicecode  设备编号 字符(40)  参考3.7	是
            public string testdevicetype { get; set; } //testdevicetype  设备类型 字符(40)  详见数据字典8.2序10 是
            public string testdevicename { get; set; } //testdevicename 设备名称    字符(40)      是
            public string opcode { get; set; } //opcode  操作项 字符(40)  详见数据字典8.2序24 是
            public string Operator { get; set; } //operator	操作人 字符(40)  操作人姓名 是
            public string reporttime { get; set; } //reporttime 上报时间    日期格式 yyyyMMddHHmmss  是
            public string optime { get; set; } //optime  操作时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string opstime { get; set; } //opstime 检查开始时间  日期格式 yyyyMMddHHmmss.SSS  是
            public string opetime { get; set; } //opetime 检查结束时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string status { get; set; } //status 设备状态    字符(2)   详见数据字典8.2序9 是
            public string result { get; set; } //result 检查结果    字符(1)   1：成功；0：失败 是
            public string memo { get; set; } //memo 不成功原因等  字符(255) 正常为空	false
            public string extn01 { get; set; } //extn01 低量程标值   数字(10,2)        是
            public string extn02 { get; set; } //extn02  高量程标值 数字(10,2)        是
            public string extn03 { get; set; } //extn03  低量程测量值 数字(10,2)        是
            public string extn04 { get; set; } //extn04  高量程测量值 数字(10,2)        是
            public string extn05 { get; set; } //extn05  低量程误差 数字(10,2)        是
            public string extn06 { get; set; } //extn06  高量程误差 数字(10,2)        是
        }

        #endregion

        #region   设备时钟同步数据
        public class uploadInspStmDeviceRecord设备时钟同步数据
        {
            public string jkid { get; set; } //1	jkid 接口标识    是 接口标识，用于区分同一接口类型。
            public string jksqm { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            // public string lx { get; set; } //2	jksqm 验证码 是 验证码由8位长度数据组成。
            public List<StmDeviceRecordDataItem设备时钟同步数据> data { get; set; }
        }

        public class StmDeviceRecordDataItem设备时钟同步数据
        {
            public List<设备时钟同步数据bodyItem> body { get; set; }
        }
        public class 设备时钟同步数据bodyItem
        {
            public string id { get; set; } //id 唯一代码    字符(32)  主键，32位GUID 是
            public string typecode { get; set; } //typecode 运行类型    字符(2)   3：时钟同步 是
            public string TsNo { get; set; } //TsNo 检测机构编号  字符(16)      是
            public string TestLineNo { get; set; } //TestLineNo  检测线 字符(16)      是
            public string testdevicecode { get; set; } //testdevicecode  设备编号 字符(40)  参考3.7	是
            public string testdevicetype { get; set; } //testdevicetype  设备类型 字符(40)  详见数据字典8.2序10 是
            public string testdevicename { get; set; } //testdevicename 设备名称    字符(40)      是
            public string opcode { get; set; } //opcode  操作项 字符(40)  详见数据字典8.2序24 是
            public string Operator { get; set; } //operator	操作人 字符(40)  操作人姓名 是
            public string reporttime { get; set; } //reporttime 上报时间    日期格式 yyyyMMddHHmmss  是
            public string optime { get; set; } //optime  服务器时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string opstime { get; set; } //opstime 同步前本地时间 日期格式 yyyyMMddHHmmss.SSS  是
            public string opetime { get; set; } //opetime 同步后本地时间 日期格式    yyyyMMddHHmmss.SSS 是
            public string status { get; set; } //status 设备状态    字符(2)   详见数据字典8.2序9 是
            public string result { get; set; } //result 同步结果    字符(1)   1：成功；0：失败 是
            public string memo { get; set; } //memo 不成功原因等  字符(255) 正常为空 FALSE
        }

        #endregion

        #endregion




        #region   外观部分定义

        public class inspExterMenu
        {
            public string fuelType { get; set; }//1	fuelType 燃油类型    是	
            public string isNew { get; set; } //2	isNew 是否是新车   是 当车牌号为空时表示新车 1表示是新车 0表示非新车
            public string regId { get; set; }//3	regId 登记id    是	
            public string vehiclemodel { get; set; }//4	vehiclemodel 车辆型号    是
        }
        
        public class uploadInspExterResult
        {
            public string regId { get; set; }//1	regId	车辆登记id,数据唯一标识	是
    
        }
        
        public class Login
        {
            public string loginName { get; set; }//    1	loginName 登录帐号    是	
            public string password { get; set; }//2	password 密码  是	
        }


        public class reginfoList
        {
            public string tsNo { get; set; }//1	tsNo 检测站编号   是	
            public string auditStatus { get; set; }//2	auditStatus 数据类型    是 写死值为0，表示待检
            public string filterName { get; set; }//3	filterName 车牌号或者车架号    否 模糊查询
            public string pageNumber { get; set; }//4	pageNumber 页码  是	
            public string pageSize { get; set; }//5	pageSize 每页按多少条分页    是

        }


        public class uploadPicTemplate
        {
            public string fuelType { get; set; }//1	fuelType 燃油类型    是	
            public string isNew { get; set; }//2	isNew 是否是新车   是 当车牌号为空时表示新车1表示是新车0表示非新车
            public string regId { get; set; }//3	regId 登记id    是	
            public string vehiclemodel { get; set; }//4	vehiclemodel 车辆型号    是
        }


        public class submitInspExter
        {
            public string vin { get; set; }//1	vin 车架号 是 新车必填
            public string plateno { get; set; }//2	plateno 车牌号 是 在用车必填
            public string regId { get; set; }//3	regId 车辆登记id, 数据唯一标识   是	
            public string checkResult { get; set; }//4	checkResult 外检结果    是	0表示不合格 1表示合格
            public string userName { get; set; }//5	userName 外检人员姓名  是	
            public string userId { get; set; }//6	userId 外检人Id   是	
            public List<picListItem> picList { get; set; }//7	picList 已上传图片id集合   是 见如下示例
            public List<checkItemListItem> checkItemList { get; set; }//8	checkItemList 环保清单判定集合    是 见如下示例
            public string fueltype { get; set; }//9	fueltype 燃油类型    是 中文值，如：柴油、汽油
            public string regtime { get; set; }//10regtime 登记时间    是	
            public string vehiclemodel { get; set; }//11vehiclemodel 车辆型号    是	
            public string tsNo { get; set; }//12tsNo 检测站点编号  是	
            public string auditor { get; set; }//13auditor 外检审核人		
            public string auditdate { get; set; }//14auditdate 审核日期		
            public string auditopinion { get; set; }//15auditopinion 审核结果		0表示不合格 1表示合格
            public string repulseDesc { get; set; }//16repulseDesc 审核意见
           
        }

        public struct picListItem
        {

            // "code": "fdjpicture",
	        //"fileId": "490340988661792838",
	        //"fileUrl": "http://114.255.129.73:8888/jdcwqzhjg/webDownloadFile?fileId\u003d490340988661792838",
	        //"name": "发动机"
            public string code { get; set; } 
            public string fileId { get; set; }  
            public string fileUrl { get; set; }  
            public string name { get; set; }  
         
        }


        public struct checkItemListItem
        {
            //"code": "mechanicalcheck",
	        //"name": "车辆机械状况良好",
	        //"noProject": false,
	        //"selectedValue": 1,
	        //"type": 0

            public string code { get; set; }  
            public string name { get; set; }  
            public string noProject { get; set; }  
            public string selectedValue { get; set; }  
            public string type { get; set; }  
          
        }
















        #endregion

        #region 8.1 公安（GA）行业代码
        //以下代码会随着需求的变化做相应调整，放数据库中修改、读取更方便简单

        //8	数据字典(所有代码为字符类型)
        //8.1公安（GA）行业代码
        //1.号牌种类(GA24.7-2005_机动车登陆信息代码第7部分：号牌种类代码)

        //01	大型汽车号牌 黄底黑字(含02式号牌部分)
        //02	小型汽车号牌 蓝底白字(含02式号牌部分)
        //03	使馆汽车号牌 黑底白字、红“使”字
        //04	领馆汽车号牌 黑底白字、红“领”字
        //05	境外汽车号牌 黑底白/红字
        //06	外籍汽车号牌 黑底白字
        //07	两、三轮摩托车号牌 黄底黑字
        //08	轻便摩托车号牌 蓝底白字
        //09	使馆摩托车号牌 黑底白字、红“使”字
        //10	领馆摩托车号牌 黑底白字、红“领”字
        //11	境外摩托车号牌 黑底白字
        //12	外籍摩托车号牌 黑底白字
        //13	农用运输车号牌 黄底黑字黑框线，已按道《路交通安全法》取消农用运输车，不再发放
        //14	拖拉机号牌 黄底黑字
        //15	挂车号牌 黄底黑字黑框线
        //16	教练汽车号牌 黄底黑字黑框线
        //17	教练摩托车号牌 黄底黑字黑框线
        //18	试验汽车号牌	
        //19	试验摩托车号牌	
        //20	临时人境汽车号牌 白底红字黑“临时人境”
        //21	临时人境摩托车号牌 白底红字黑“临时人境”
        //22	临时行驶车号牌 白底黑字黑框线
        //23	警用汽车号牌	
        //24	警用摩托号牌	
        //99	其他号牌



        //2.国产/进口//GA24.12-2005_机动车登陆信息代码第12部分：国产-进口车辆代码 A   国产 指国产机动车
        //B 海关进口    指进口机动车
        //C   公安没收 指进口机动车
        //D 工商没收    指进口机动车
        //E   海关没收 指进口机动车
        //F 一车一证    指进口机动车
        //G   海关监管 指进口机动车
        //H 进口底盘    指进口底盘国产机动车
        //I   解除监管 指进口机动车
        //Z 其他



        //3.使用性质GA24.3-2005_机动车登陆信息代码第3部分：使用性质代码 A   非营运 个人或者单位不以获取运输利润为目的而使用的机动车

        //B 公路客运    专门从事公路旅客运输的、以获取利润为目的的机动车
        //C   公交客运 城市内专门从事公共交通客运的、以获取利润为目的的机动车
        //D   出租客运 以行驶里程和时间计费, 将乘客运载至其指定地点的、以获取利润为目的的机动车
        //E   旅游客运 专门运载游客的、以获取利润为目的的机动车
        //F   货运 专门从事货物运输的、以获取利润为目的的机动车
        //G   租赁 专门租赁给其他单位或者个人使用, 以租用时间或者租用里程计费的、以获取利润为目的的机动车
        //H   警用 a)公安机关用于侦查、警卫和治安、交通管理的巡逻车、勘察车、护卫车、囚车以及其他执行特别紧急任务的车辆
        //b)国家安全机关用于执行侦察和其他特殊任务的车辆
        //c)人民检察院用于侦查刑事犯罪案件的现场勘察车和押解人犯的囚车
        //d)人民法院用于押解刑事被告人或者罪犯、死刑执行、法医勘查、适用民事强制措施、强制执行以及执行其他紧急公务的囚车、刑场指挥车、死刑执行车、法医勘察车、强制执行用车和其他警务用车
        //e)司法行政机关用于押解罪犯、运送劳教人员的囚车或专用车辆和追辑逃犯的车辆
        //I   消防 公安消防部队和其他消防部门用于灭火的专用机动车和现场指挥机动车

        //J 救护  急救、医疗机构和卫生防疫部门用于抢救危重病人或处理紧急疫情的专用机动车;公共卫生事件中用于现场医疗救援和辅助现场医疗救援的专用车辆
        //K   工程抢险 防汛、水利、电力、矿山、城建、交通、铁道等部门用于抢修公用设施、抢救人民生命财产的专用机动车和现场指挥机动车
        //L   营转非 原为营运车辆, 现改为非营运车辆
        //M 出租转非    原为出租车,现改为非营运车辆
        //N   教练 营运，补充代码
        //O   幼儿校车 非营运，补充代码
        //P   小学生校车 非营运，补充代码
        //Q   其他校车 非营运，补充代码
        //R   危化品运输 营运，补充代码
        //Z   其他


        //4	所有权GA24.5-2005_机动车登陆信息代码第5部分：所有权代码	
        //1	单位 指机关、企业、事业单位和社会团体以及外国驻华使馆、领馆和外国驻华办事机构、国际组织驻华代表机构
        //2	个人 是指我国内地的居民和军人(含武警)以及香港、澳门特别行政区、台湾地区居民和外国人
        //9	其他



        //车辆类型
        //GA24.4-2005_机动车登陆信息代码第17部分：机动车车辆类型代码
        //本部分将机动车基于车辆大类按规格及结构进行分类，采用一位字母和两位阿拉伯数字表示机动车辆类型代码。
        //本编码由三部分组成：机动车大类（字母）+机动车规格（数字）+机动车结构（数字）
        //B11 重型普通半挂车
        //B12 重型厢式半挂车
        //B13 重型罐式半挂车
        //B14 重型平板半挂车
        //B15 重型集装箱半挂车
        //B16 重型自卸半挂车
        //B17 重型特殊结构半挂车
        //B18 重型仓栅式半挂车
        //B19 重型旅居半挂车
        //B1A 重型专项作业半挂车
        //B1B 重型低平板半挂车
        //B1C 重型车辆运输半挂车
        //B1D 重型罐式自卸半挂车
        //B1E 重型平板自卸半挂车
        //B1F 重型集装箱自卸半挂车
        //B1G 重型特殊结构自卸半挂车
        //B1H 重型仓栅式自卸半挂车
        //B1J 重型专项作业自卸半挂车
        //B1K 重型低平板自卸半挂车
        //B1U 重型中置轴旅居挂车
        //B1V 重型中置轴车辆运输车
        //B1W 重型中置轴普通挂车
        //B21 中型普通半挂车
        //B22 中型厢式半挂车
        //B23 中型罐式半挂车
        //B24 中型平板半挂车
        //B25 中型集装箱半挂车
        //B26 中型自卸半挂车
        //B27 中型特殊结构半挂车
        //B28 中型仓栅式半挂车
        //B29 中型旅居半挂车
        //B2A 中型专项作业半挂车
        //B2B 中型低平板半挂车
        //B2C 中型车辆运输半挂车
        //B2D 中型罐式自卸半挂车
        //B2E 中型平板自卸半挂车
        //B2F 中型集装箱自卸半挂车
        //B2G 中型特殊结构自卸半挂车
        //B2H 中型仓栅式自卸半挂车
        //B2J 中型专项作业自卸半挂车
        //B2K 中型低平板自卸半挂车
        //B2U 中型中置轴旅居挂车
        //B2V 中型中置轴车辆运输车
        //B2W 中型中置轴普通挂车
        //B31 轻型普通半挂车
        //B32 轻型厢式半挂车
        //B33 轻型罐式半挂车
        //B34 轻型平板半挂车
        //B35 轻型自卸半挂车
        //B36 轻型仓栅式半挂车
        //B37 轻型旅居半挂车
        //B38 轻型专项作业半挂车
        //B39 轻型低平板半挂车
        //B3C 轻型车辆运输半挂车
        //B3D 轻型罐式自卸半挂车
        //B3E 轻型平板自卸半挂车
        //B3F 轻型集装箱自卸半挂车
        //B3G 轻型特殊结构自卸半挂车
        //B3H 轻型仓栅式自卸半挂车
        //B3J 轻型专项作业自卸半挂车
        //B3K 轻型低平板自卸半挂车
        //B3U 轻型中置轴旅居挂车
        //B3V 轻型中置轴车辆运输车
        //B3W 轻型中置轴普通挂车
        //D11 无轨电车
        //D12 有轨电车
        //G11 重型普通全挂车
        //G12 重型厢式全挂车
        //G13 重型罐式全挂车
        //G14 重型平板全挂车
        //G15 重型集装箱全挂车
        //G16 重型自卸全挂车
        //G17 重型仓栅式全挂车
        //G18 重型旅居全挂车
        //G19 重型专项作业全挂车
        //G1A 重型厢式自卸全挂车
        //G1B 重型罐式自卸全挂车
        //G1C 重型平板自卸全挂车
        //G1D 重型集装箱自卸全挂车
        //G1E 重型仓栅式自卸全挂车
        //G1F 重型专项作业自卸全挂车
        //G21 中型普通全挂车
        //G22 中型厢式全挂车
        //G23 中型罐式全挂车
        //G24 中型平板全挂车
        //G25 中型集装箱全挂车
        //G26 中型自卸全挂车
        //G27 中型仓栅式全挂车
        //G28 中型旅居全挂车
        //G29 中型专项作业全挂车
        //G2A 中型厢式自卸全挂车
        //G2B 中型罐式自卸全挂车
        //G2C 中型平板自卸全挂车
        //G2D 中型集装箱自卸全挂车
        //G2E 中型仓栅式自卸全挂车
        //G2F 中型专项作业自卸全挂车
        //G31 轻型普通全挂车
        //G32 轻型厢式全挂车
        //G33 轻型罐式全挂车
        //G34 轻型平板全挂车
        //G35 轻型自卸全挂车
        //G36 轻型仓栅式全挂车
        //G37 轻型旅居全挂车
        //G38 轻型专项作业全挂车
        //G3A 轻型厢式自卸全挂车
        //G3B 轻型罐式自卸全挂车
        //G3C 轻型平板自卸全挂车
        //G3D 轻型集装箱自卸全挂车
        //G3E 轻型仓栅式自卸全挂车
        //G3F 轻型专项作业自卸全挂车
        //H11 重型普通货车
        //H12 重型厢式货车
        //H13 重型封闭货车
        //H14 重型罐式货车
        //H15 重型平板货车
        //H16 重型集装厢车
        //H17 重型自卸货车
        //H18 重型特殊结构货车
        //H19 重型仓栅式货车
        //H1A 重型车辆运输车
        //H1B 重型厢式自卸货车
        //H1C 重型罐式自卸货车
        //H1D 重型平板自卸货车
        //H1E 重型集装厢自卸货车
        //H1F 重型特殊结构自卸货车
        //H1G 重型仓栅式自卸货车
        //H21 中型普通货车
        //H22 中型厢式货车
        //H23 中型封闭货车
        //H24 中型罐式货车
        //H25 中型平板货车
        //H26 中型集装厢车
        //H27 中型自卸货车
        //H28 中型特殊结构货车
        //H29 中型仓栅式货车
        //H2A 中型车辆运输车
        //H2B 中型厢式自卸货车
        //H2C 中型罐式自卸货车
        //H2D 中型平板自卸货车
        //H2E 中型集装厢自卸货车
        //H2F 中型特殊结构自卸货车
        //H2G 中型仓栅式自卸货车
        //H31 轻型普通货车
        //H32 轻型厢式货车
        //H33 轻型封闭货车
        //H34 轻型罐式货车
        //H35 轻型平板货车
        //H37 轻型自卸货车
        //H38 轻型特殊结构货车
        //H39 轻型仓栅式货车
        //H3A 轻型车辆运输车
        //H3B 轻型厢式自卸货车
        //H3C 轻型罐式自卸货车
        //H3D 轻型平板自卸货车
        //H3F 轻型特殊结构自卸货车
        //H3G 轻型仓栅式自卸货车
        //H41 微型普通货车
        //H42 微型厢式货车
        //H43 微型封闭货车
        //H44 微型罐式货车
        //H45 微型自卸货车
        //H46 微型特殊结构货车
        //H47 微型仓栅式货车
        //H4A 微型车辆运输车
        //H4B 微型厢式自卸货车
        //H4C 微型罐式自卸货车
        //H4F 微型特殊结构自卸货车
        //H4G 微型仓栅式自卸货车
        //H51 普通低速货车
        //H52 厢式低速货车
        //H53 罐式低速货车
        //H54 自卸低速货车
        //H55 仓栅式低速货车
        //H5B 厢式自卸低速货车
        //H5C 罐式自卸低速货车
        //J11 轮式装载机械
        //J12 轮式挖掘机械
        //J13 轮式平地机械
        //K11 大型普通客车
        //K12 大型双层客车
        //K13 大型卧铺客车
        //K14 大型铰接客车
        //K15 大型越野客车
        //K16 大型轿车
        //K17 大型专用客车
        //K18 大型专用校车
        //K21 中型普通客车
        //K22 中型双层客车
        //K23 中型卧铺客车
        //K24 中型铰接客车
        //K25 中型越野客车
        //K26 中型轿车
        //K27 中型专用客车
        //K28 中型专用校车
        //K31 小型普通客车
        //K32 小型越野客车
        //K33 小型轿车
        //K34 小型专用客车
        //K38 小型专用校车
        //K39 小型面包车
        //K41 微型普通客车
        //K42 微型越野客车
        //K43 微型轿车
        //K49 微型面包车
        //M11 普通正三轮摩托车
        //M12 轻便正三轮摩托车
        //M13 正三轮载客摩托车
        //M14 正三轮载货摩托车
        //M15 侧三轮摩托车
        //M21 普通二轮摩托车
        //M22 轻便二轮摩托车
        //N11 三轮汽车
        //Q11 重型半挂牵引车
        //Q12 重型全挂牵引车
        //Q21 中型半挂牵引车
        //Q22 中型全挂牵引车
        //Q31 轻型半挂牵引车
        //Q32 轻型全挂牵引车
        //T11 大型轮式拖拉机
        //T21 小型轮式拖拉机
        //T22 手扶拖拉机
        //T23 手扶变形运输机
        //Z11 大型非载货专项作业车
        //Z12 大型载货专项作业车
        //Z21 中型非载货专项作业车
        //Z22 中型载货专项作业车
        //Z31 小型非载货专项作业车
        //Z32 小型载货专项作业车
        //Z41 微型非载货专项作业车
        //Z42 微型载货专项作业车
        //Z51 重型非载货专项作业车
        //Z52 重型载货专项作业车
        //Z71 轻型非载货专项作业车
        //Z72 轻型载货专项作业车
        //X99 其它

        public string GetCLXH(String CXLB)
        {
            string CXdm = "K33"; ;
            switch (CXLB)
            {
                case "重型普通半挂车":
                    CXdm = "B11";
                    break;
                case "重型厢式半挂车":
                    CXdm = "B12";
                    break;
                case "重型罐式半挂车":
                    CXdm = "B13";
                    break;
                case "重型平板半挂车":
                    CXdm = "B14";
                    break;
                case "重型集装箱半挂车":
                    CXdm = "B15";
                    break;
                case "重型自卸半挂车":
                    CXdm = "B16";
                    break;
                case "重型特殊结构半挂车":
                    CXdm = "B17";
                    break;
                case "重型仓栅式半挂车":
                    CXdm = "B18";
                    break;
                case "重型旅居半挂车":
                    CXdm = "B19";
                    break;
                case "重型专项作业半挂车":
                    CXdm = "B1A";
                    break;
                case "重型低平板半挂车":
                    CXdm = "B1B";
                    break;
                case "中型普通半挂车":
                    CXdm = "B21";
                    break;
                case "中型厢式半挂车":
                    CXdm = "B22";
                    break;
                case "中型罐式半挂车":
                    CXdm = "B23";
                    break;
                case "中型平板半挂车":
                    CXdm = "B24";
                    break;
                case "中型集装箱半挂车":
                    CXdm = "B25";
                    break;
                case "中型自卸半挂车":
                    CXdm = "B26";
                    break;
                case "中型特殊结构半挂车":
                    CXdm = "B27";
                    break;
                case "中型仓栅式半挂车":
                    CXdm = "B28";
                    break;
                case "中型旅居半挂车":
                    CXdm = "B29";
                    break;
                case "中型专项作业半挂车":
                    CXdm = "B2A";
                    break;
                case "中型低平板半挂车":
                    CXdm = "B2B";
                    break;
                case "轻型普通半挂车":
                    CXdm = "B31";
                    break;
                case "轻型厢式半挂车":
                    CXdm = "B32";
                    break;
                case "轻型罐式半挂车":
                    CXdm = "B33";
                    break;
                case "轻型平板半挂车":
                    CXdm = "B34";
                    break;
                case "轻型自卸半挂车":
                    CXdm = "B35";
                    break;
                case "轻型仓栅式半挂车":
                    CXdm = "B36";
                    break;
                case "轻型旅居半挂车":
                    CXdm = "B37";
                    break;
                case "轻型专项作业半挂车":
                    CXdm = "B38";
                    break;
                case "轻型低平板半挂车":
                    CXdm = "B39";
                    break;
                case "无轨电车":
                    CXdm = "D11";
                    break;
                case "有轨电车":
                    CXdm = "D12";
                    break;
                case "重型普通全挂车":
                    CXdm = "G11";
                    break;
                case "重型厢式全挂车":
                    CXdm = "G12";
                    break;
                case "重型罐式全挂车":
                    CXdm = "G13";
                    break;
                case "重型平板全挂车":
                    CXdm = "G14";
                    break;
                case "重型集装箱全挂车":
                    CXdm = "G15";
                    break;
                case "重型自卸全挂车":
                    CXdm = "G16";
                    break;
                case "重型仓栅式全挂车":
                    CXdm = "G17";
                    break;
                case "重型旅居全挂车":
                    CXdm = "G18";
                    break;
                case "重型专项作业全挂车":
                    CXdm = "G19";
                    break;
                case "中型普通全挂车":
                    CXdm = "G21";
                    break;
                case "中型厢式全挂车":
                    CXdm = "G22";
                    break;
                case "中型罐式全挂车":
                    CXdm = "G23";
                    break;
                case "中型平板全挂车":
                    CXdm = "G24";
                    break;
                case "中型集装箱全挂车":
                    CXdm = "G25";
                    break;
                case "中型自卸全挂车":
                    CXdm = "G26";
                    break;
                case "中型仓栅式全挂车":
                    CXdm = "G27";
                    break;
                case "中型旅居全挂车":
                    CXdm = "G28";
                    break;
                case "中型专项作业全挂车":
                    CXdm = "G29";
                    break;
                case "轻型普通全挂车":
                    CXdm = "G31";
                    break;
                case "轻型厢式全挂车":
                    CXdm = "G32";
                    break;
                case "轻型罐式全挂车":
                    CXdm = "G33";
                    break;
                case "轻型平板全挂车":
                    CXdm = "G34";
                    break;
                case "轻型自卸全挂车":
                    CXdm = "G35";
                    break;
                case "轻型仓栅式全挂车":
                    CXdm = "G36";
                    break;
                case "轻型旅居全挂车":
                    CXdm = "G37";
                    break;
                case "轻型专项作业全挂车":
                    CXdm = "G38";
                    break;
                case "重型普通货车":
                    CXdm = "H11";
                    break;
                case "重型厢式货车":
                    CXdm = "H12";
                    break;
                case "重型封闭货车":
                    CXdm = "H13";
                    break;
                case "重型罐式货车":
                    CXdm = "H14";
                    break;
                case "重型平板货车":
                    CXdm = "H15";
                    break;
                case "重型集装厢车":
                    CXdm = "H16";
                    break;
                case "重型自卸货车":
                    CXdm = "H17";
                    break;
                case "重型特殊结构货车":
                    CXdm = "H18";
                    break;
                case "重型仓栅式货车":
                    CXdm = "H19";
                    break;
                case "中型普通货车":
                    CXdm = "H21";
                    break;
                case "中型厢式货车":
                    CXdm = "H22";
                    break;
                case "中型封闭货车":
                    CXdm = "H23";
                    break;
                case "中型罐式货车":
                    CXdm = "H24";
                    break;
                case "中型平板货车":
                    CXdm = "H25";
                    break;
                case "中型集装厢车":
                    CXdm = "H26";
                    break;
                case "中型自卸货车":
                    CXdm = "H27";
                    break;
                case "中型特殊结构货车":
                    CXdm = "H28";
                    break;
                case "中型仓栅式货车":
                    CXdm = "H29";
                    break;
                case "轻型普通货车":
                    CXdm = "H31";
                    break;
                case "轻型厢式货车":
                    CXdm = "H32";
                    break;
                case "轻型封闭货车":
                    CXdm = "H33";
                    break;
                case "轻型罐式货车":
                    CXdm = "H34";
                    break;
                case "轻型平板货车":
                    CXdm = "H35";
                    break;
                case "轻型自卸货车":
                    CXdm = "H37";
                    break;
                case "轻型特殊结构货车":
                    CXdm = "H38";
                    break;
                case "轻仓栅式货车":
                    CXdm = "H39";
                    break;
                case "微型普通货车":
                    CXdm = "H41";
                    break;
                case "微型厢式货车":
                    CXdm = "H42";
                    break;
                case "微型封闭货车":
                    CXdm = "H43";
                    break;
                case "微型罐式货车":
                    CXdm = "H44";
                    break;
                case "微型自卸货车":
                    CXdm = "H45";
                    break;
                case "微型特殊结构货车":
                    CXdm = "H46";
                    break;
                case "微型仓栅式货车":
                    CXdm = "H47";
                    break;
                case "普通低速货车":
                    CXdm = "H51";
                    break;
                case "厢式低速货车":
                    CXdm = "H52";
                    break;
                case "罐式低速货车":
                    CXdm = "H53";
                    break;
                case "自卸低速货车":
                    CXdm = "H54";
                    break;
                case "仓栅式低速货车":
                    CXdm = "H55";
                    break;
                case "轮式装载机械":
                    CXdm = "J11";
                    break;
                case "轮式挖掘机械":
                    CXdm = "J12";
                    break;
                case "轮式平地机械":
                    CXdm = "J13";
                    break;
                case "大型普通客车":
                    CXdm = "K11";
                    break;
                case "大型双层客车":
                    CXdm = "K12";
                    break;
                case "大型卧铺客车":
                    CXdm = "K13";
                    break;
                case "大型铰接客车":
                    CXdm = "K14";
                    break;
                case "大型越野客车":
                    CXdm = "K15";
                    break;
                case "大型轿车":
                    CXdm = "K16";
                    break;
                case "大型专用客车":
                    CXdm = "K17";
                    break;
                case "中型普通客车":
                    CXdm = "K21";
                    break;
                case "中型双层客车":
                    CXdm = "K22";
                    break;
                case "中型卧铺客车":
                    CXdm = "K23";
                    break;
                case "中型铰接客车":
                    CXdm = "K24";
                    break;
                case "中型越野客车":
                    CXdm = "K25";
                    break;
                case "中型轿车":
                    CXdm = "K26";
                    break;
                case "中型专用客车":
                    CXdm = "K27";
                    break;
                case "小型普通客车":
                    CXdm = "K31";
                    break;
                case "小型越野客车":
                    CXdm = "K32";
                    break;
                case "小型轿车":
                case "轿车":
                    CXdm = "K33";
                    break;
                case "小型专用客车":
                    CXdm = "K34";
                    break;
                case "微型普通客车":
                    CXdm = "K41";
                    break;
                case "微型越野客车":
                    CXdm = "K42";
                    break;
                case "微型轿车":
                    CXdm = "K43";
                    break;
                case "大型专用校车":
                    CXdm = "K18";
                    break;
                case "中型专用校车":
                    CXdm = "K28";
                    break;
                case "小型专用校车":
                    CXdm = "K38";
                    break;
                case "小型面包车":
                    CXdm = "K39";
                    break;
                case "微型面包车":
                    CXdm = "K49";
                    break;
                case "普通正三轮摩托车":
                    CXdm = "M11";
                    break;
                case "轻便正三轮摩托车":
                    CXdm = "M12";
                    break;
                case "正三轮载客摩托车":
                    CXdm = "M13";
                    break;
                case "正三轮载货摩托车":
                    CXdm = "M14";
                    break;
                case "侧三轮摩托车":
                    CXdm = "M15";
                    break;
                case "普通二轮摩托车":
                    CXdm = "M21";
                    break;
                case "轻便二轮摩托车":
                    CXdm = "M22";
                    break;
                case "三轮汽车":
                    CXdm = "N11";
                    break;
                case "重型半挂牵引车":
                    CXdm = "Q11";
                    break;
                case "重型全挂牵引车":
                    CXdm = "Q12";
                    break;
                case "中型半挂牵引车":
                    CXdm = "Q21";
                    break;
                case "中型全挂牵引车":
                    CXdm = "Q22";
                    break;
                case "轻型半挂牵引车":
                    CXdm = "Q31";
                    break;
                case "轻型全挂牵引车":
                    CXdm = "Q32";
                    break;
                case "大型轮式拖拉机":
                    CXdm = "T11";
                    break;
                case "小型轮式拖拉机":
                    CXdm = "T21";
                    break;
                case "手扶拖拉机":
                    CXdm = "T22";
                    break;
                case "手扶变形运输机":
                    CXdm = "T23";
                    break;
                case "其它":
                    CXdm = "X99";
                    break;
                case "大型专项作业车":
                    CXdm = "Z11";
                    break;
                case "中型专项作业车":
                    CXdm = "Z21";
                    break;
                case "小型专项作业车":
                    CXdm = "Z31";
                    break;
                case "微型专项作业车":
                    CXdm = "Z41";
                    break;
                case "重型专项作业车":
                    CXdm = "Z51";
                    break;
                case "轻型专项作业车":
                    CXdm = "Z71";
                    break;
            }

            return CXdm;
        }

        //6.燃料(能源)种类代码
        //GA24.9-2005_机动车登陆信息代码第9部分：燃料(能源)种类代码 A   汽油
        //B   柴油
        //C   电 以电能驱动的汽车
        //D 混合油
        //E 天然气
        //F 液化石油气
        //L 甲醉
        //M 乙醇
        //N 太阳能
        //O 混合动力
        //Y 无   仅限全挂车等无动力的
        //Z   其它
        //AE  汽油天然气 双燃料车
        //BO 柴油混合动力
        //BC 柴油电动混合
        //AC 汽油电动混合
        //AO 汽油混合动力
        //A1 油改气 自定义



        //7.身份证明名称
        //GA24.20-2005_机动车登陆信息代码第20部分：身份证明名称代码
        //A   居民身份证 中华人民共和国居民身份证
        //B 组织机构代码证书    机关、企业、事业单位、社会团体、外商在华独资企业、外商驻华机构的身份证明
        //C   军官证 现役军官身份证明和警官证、文职证
        //D   士兵证 现役士兵身份证明
        //E 军官离退休证  退休军官身份证明
        //F   境外人员身份证明 指香港、澳门特别行政区、台湾地区居民和外国人人境的身份证明
        //G   外交人员身份证件 指 外国驻华使馆、领馆人员、国际组织驻华代表机构人员的身份证明，是外交部核发的有效身份证件
        //H   居民户口簿 中华人民共和国居民户口簿
        //J 单位注销证明  指 注销、撤销、破产企业由有关单位、组织出具的相关证明(注销的企业单位，是工商行政管理部门出具的注销证明; 已撤销的机关、事业单位，是其上级主管机关出具的有关证明;已破产的企业单位，是依法成立的财产清算机构出具的有关证明)
        //K 居住暂住证明  指在暂住地居住的内地居民，由公安机关核发的居住暂住证明
        //L   驻华机构证明 各国驻华使领馆和外国驻华办事机构的身份证明
        //Z 其他证件



        //8	车身颜色(GA)    
        //A 白
        //B 灰
        //C 黄
        //D 粉
        //E 红
        //F 紫
        //G 绿
        //H 蓝
        //I 棕
        //J 黑
        //Z 其他

        public string GetCPYSDM(String YS)
        {
            string DM = "1";
            switch (YS)
            {
                case "蓝":
                case "02":
                    DM = "1";
                    break;
                case "黄":
                case "01":
                    DM = "2";
                    break;
                case "黑":
                case "15":
                    DM = "3";
                    break;
                case "白":
                case "23":
                    DM = "4";
                    break;
                case "绿":
                case "13":
                    DM = "5";
                    break;
                case "其他":
                case "99":
                    DM = "9";
                    break;
            }
            return DM;
        }


        //7.2	车身颜色
        //代码  名称
        //A   白
        //B   灰
        //C   黄
        //D   粉
        //E   红
        //F   紫
        //G   绿
        //H   蓝
        //I   棕
        //J   黑
        //Y   白绿
        //Z   其他
        public string GetCSYSDM(String YS)
        {
            string DM = "A";
            switch (YS)
            {
                case "白":
                    DM = "A";
                    break;
                case "灰":
                    DM = "B";
                    break;
                case "黄":
                    DM = "C";
                    break;
                case "粉":
                    DM = "D";
                    break;
                case "红":
                    DM = "E";
                    break;
                case "紫":
                    DM = "F";
                    break;
                case "绿":
                    DM = "G";
                    break;
                case "蓝":
                    DM = "H";
                    break;
                case "棕":
                    DM = "I";
                    break;
                case "黑":
                    DM = "J";
                    break;
                case "白绿":
                    DM = "Y";
                    break;
                case "其他":
                    DM = "Z";
                    break;
            }
            return DM;
        }
        #endregion

        #region 8.2 环保（EP）行业代码（非行业标准）
        //8.2	环保（EP）行业代码（非行业标准）
        //1	环保车辆类型 M11 第一类轻型汽车
        //MN2 第二类轻型汽车
        //MN3 重型汽车	


        //2	环保使用性质	
        //0	非营运 对应公安A, H, I, J, K, L, M, O, P, Q
        //1	营运 对应公安B, C, D, E, F, G, N, R


        //3	检验类型	
        //1	定期检验	
        //2	抽检复检	
        //3	实验比对	
        //4	外地车委托检验	
        //5	外地车转入检验	
        //6	特殊检验 原外出检验
        //7	临时检验	
        //8	新车上牌前检验	
        //9	发动机变更检验	
        //0	其它	


        //4	检测方法	
        //1	怠速 怠速法 不用   
        //2	双怠速 双怠速法
        //3	滤纸烟度 自由加速度滤纸烟度法 不用
        //4	不透光 自由加速度不透光烟度法
        //5	简易稳态 简易稳态工况法(ASM)
        //6	简易瞬态 简易瞬态工况法(VMAS)
        //7	加载减速 加载减速工况法(LUGDOWN)
        //8	瞬态工况 瞬态工况法 不用
        //9	路视	不用
        //0	急加速 急加速法（环保部I急加速）不用


        //5	检测结果	
        //1	合格	
        //0	不合格	
        //9	未检	


        //6	进气方式	
        //1	自然吸气	
        //2	涡轮增压


        //7	驱动方式	
        //1	前驱	
        //2	后驱	
        //3	四驱	
        //4	全时四驱	


        //8	变速箱类型	
        //1	手动 	
        //2	自动 含手自一体、CVT无极变速等


        //9	设备状态	
        //1	预热 设备开机预热
        //2	空闲 设备空闲，随时可以进行检测
        //3	检测 设备忙，设备正在进行检测
        //4	关闭 设备已经关闭，或检测程序退出
        //5	到位	
        //6	下降	
        //7	检测中	
        //8	接转速	
        //9	插探头	
        //10	加速	
        //11	举升	
        //12	过车	
        //13	检测失败	
        //14	标定	
        //15	自检	
        //16	锁止	
        //17	故障	

        //10	检测设备类型
        //1	测功机	
        //2	五气分析仪	
        //3	烟度计	
        //4	电子环境信息仪	
        //5	发动机转速仪	
        //6	流量计	
        //7	滤纸烟度计	
        //8	工控机	

        //11	号牌颜色
        //1	蓝牌	
        //2	黄牌	
        //3	黑牌	
        //4	白牌	
        //5	绿牌	
        //0	未知	

        //12	检测登陆状态
        //0	预登陆	不用
        //1	待检	
        //2	任务已发布	不用
        //3	已检	

        //14	车辆类型(VECC)
        //K4 微型载客汽车
        //K3 小型载客汽车
        //K2 中型载客汽车
        //K1 大型载客汽车
        //H3 轻型载货汽车
        //H2 中型载货汽车
        //H4 微型载货汽车
        //H1 重型载货汽车
        //D1 低速载货汽车
        //D2 三轮低速载货汽车
        //M1 普通摩托车
        //M2 轻便摩托车	

        //15	排放标准(EP)
        //0	国0	
        //1	国Ⅰ	
        //2	国Ⅱ	
        //3	国Ⅲ	
        //4	国Ⅳ	
        //5	国Ⅴ	
        //6	国Ⅵ	

        //16	标志等级	
        //1	绿标	
        //2	黄标	

        //17	机动车状态(EP)  
        //1	在用	
        //0	非在用	

        //18	装配OBD	
        //1	有OBD	
        //0	无OBD	

        //19	技术鉴别状态	
        //1	鉴别通过	
        //2	鉴别未通过	

        //20	供油方式	
        //1		开环电喷	
        //2	闭环电喷	
        //3	直喷	
        //4	高压共轨	
        //5	泵喷式	
        //9	化油器	
        //0	其他	

        //21	排气处理装置	
        //1	汽油车有三元催化器	
        //2	柴油车有尾气过滤装置	

        //22	氧传感器	
        //1	有	
        //0	无	

        //23	特殊情况描述
        //1	混合动力车免上线检测	
        //2	电动车免上线检测	
        //3	改方法为双怠速	
        //4	改方法为不透光	
        //0	正常情况	

        //24	设备运行操作项	
        //0	自检	
        //1	车速标定	
        //2	扭力标定	
        //3	寄生功率标定	
        //4	加载滑行标定	
        //5	分析仪标定	
        //6	烟度计标定	
        //9	时钟同步	
        //11加载滑行检查	
        //12附加功率损失检查	
        //13五气分析仪检查	
        //14泄露检查	
        //15五气分析仪氧量程检查	
        //16低标气检查	
        //17流量计检查
        #endregion

        // 6.2.4 code定义 调用接口返回代码
        //（环保）中心系统会对每次上报的数据进行实时验证，若数据内容、格式等不符合规范要求时，系统会中止此次数据上报，并返回异常信息(错误代码+说明)。检测站系统需记录异常信息以便检测站工作人员查阅核实。以下是部分返回的异常信息： 
        //描述 返回结果JSON文档，code（大于0成功；小于等于0失败， message（描述信息）。
        //取值说明 code取值  说明
        //	1	数据请求成功/上传成功
        //	2	文件上传成功
        //	-1	数据请求失败
        //	-2	数据项格式不正确
        //	-3	数据项业务关联不正确
        //	-4	数据上传部分成功，异常数据被抛弃
        //	-9	Json解析异常
        //	-101	jksqm验证码为空
        //	-102	jksqm信息有误
        //	-103	Jkid为空
        //	-104	Jkid信息有误
        //	-105	Jkid关闭
        //	-106	TsNo信息为空
        //	-107	VIN信息为空
        //	-108		EngineNo信息为空
        //	-500	未找到匹配数据
        //	-10000	Id重复
        //	-20000	数据为空错误
        //	-20001	数据格式错误
        //	-20002	数据字段为空错误
        //	-20003	数据字段长度错误
        //	-20004	数据字段格式错误：不是数字类型
        //	-20005	数据字段格式错误：不符合正则表达式规定
        //	-20009	数据库保存异常
        //	-20020	上报检测结果数据未找到匹配的车辆信息
        //	-20021	重复上报检测结果数据，检测结果数据一旦上报不可更新
        //	-20022	inspregid或VIN 字段与车辆登录表的字段信息不匹配
        //	-20023	tesingid、inspregid字段与车辆检测结果的字段信息不匹配
        //	-20024	检测设备编号已存在
        //	-20025	检测设备不存在
        //	-20026	上传车辆外观检测图片失败，appearanceid 或 inspregid有误
        //	-20027	上传车辆外观检测图片失败，图片picType不是规定类型
        //	-20028	obdresultid、inspregid字段与车辆OBD检测结果的字段信息不匹配，请检查相关字段是否有误！
        //	-40000	上传文件为空
        //	-40001	不允许上传多个文件
        //	-40002	文件类型不符合规定格式
        //	-40003	文件大小超过规定值
        //	$E 系统异常

        #region 数据上报回执部分
        public class uploadAckResult
        {
            public string exchangeCode { set; get; }
            public string responseTime { set; get; }
            public string code { set; get; }
            public string message { set; get; }
            public string testingId { set; get; }
        }
        public class uploadAck
        {
            public string jkid { set; get; }
            public string jksqm { set; get; }
            public List<uploadAckResult> result { set; get; }
        }

        #endregion
        #region 检测报告单信息返回
        public class uploadTestingCheck
        {
            public string jkid { set; get; }
            public string jksqm { set; get; }
            public List<uploadAckResult> result { set; get; }
        }
        public class uploadTestingCheckResult
        {
            public List<uploadTestingBody> body;
        }
        public class uploadTestingBody
        {
            public uploadTestingBodyCheck testingCheck;
            public uploadTestingBodyResult testingResult;
        }
        public class uploadTestingBodyCheck
        {
            public string testingid;
            public string testno;
            public string tsno;
        }
        public class uploadTestingBodyResult
        {
            public string testtype;
            public string testJudge;//0,不合格，1，合格
            public string value01;
            public string value02;
            public string value03;
            public string value04;
            public string value05;
            public string value06;
            public string limit01;
            public string limit02;
            public string limit03;
            public string limit04;
            public string limit05;
            public string limit06;
            public string judge01;
            public string judge02;
            public string judge03;
            public string judge04;
            public string judge05;
            public string judge06;
            public string parame01;
            public string parame02;
            public string parame03;
            public string parame04;
            public string parame05;
            public string parame06;
        }
        #endregion
        public class uploadReg
        {
            public string jkid { set; get; }
            public string jksqm { set; get; }
            public List<uploadRegResult> result { set; get; }
        }
        public class uploadRegResult
        {
            public List<uploadRegBody> body;
        }
        public class uploadRegBody
        {
            public uploadRegInsp insp;
            public uploadRegInsp_Vehicle insp_vehicle;
        }
        public class uploadRegInsp
        {
            public string inspregid { set; get; }        //车辆检测登录id    如果epdesc值为ALLOW或为空，则返回inspregid信息。该字段是环保局检测登录id唯一编号。与该车辆检测相关数据必须包含该字段内容
            public string TsNo { set; get; }        // 检测站编号 由市环保局提供，检测站唯一编号。该检测站上报的所有数据都必须包含该字段内容
            public string epdesc { set; get; }        // 车辆是否可以在本检测站进行检测 如果值为ALLOW或为空则表示可以正常登陆检测。如果不为空，则表示该车辆被中心系统锁定，站内登录系统需把该<环保说明epdesc> 信息反馈给登录人员。（参看epdesc节点说明）。
            public string TestType { set; get; }        //常规检测方法  最终检测方法要检测人员根据车辆实际情况进行确定
            public string TestTypeDesc { set; get; }        //检测方法补充说明 对建议检测方法进行补充说明。
            public string TestLineNo { set; get; }        //检测线代码   yyyyMMddHHmmss
            public string inspfueltype { set; get; }        //  应检燃料类型 详见数据字典8.1序6
            public string RegisterTime { set; get; }        //  车辆登记时间
            public string Inspperiod { set; get; }        // 年检周期
            public string TestTimes { set; get; }        // 检验次数

        }
        public class uploadRegInsp_Vehicle
        {
            public string License { set; get; }        //号牌号码
            public string LicenseCode { set; get; }        //号牌种类(GA)    详见数据字典8.1序1
            public string LicenseType { set; get; }        // 号牌颜色 详见数据字典8.2序11
            public string VIN { set; get; }        //VIN 车辆识别代号
            public string vehicleid { set; get; }        //车辆id 系统内部使用
            public string Engine { set; get; }        //发动机型号
            public string EngineNo { set; get; }        //发动机编号
            public string VehicleModel { set; get; }        //车辆型号
            public string FuelType { set; get; }        //燃料种类(GA)    详见数据字典8.1序6
            public string Cylinders { set; get; }        //汽缸数
            public string Odometer { set; get; }        //里程表读数 KM
            public string Standard { set; get; }        //排放标准    参考8.2 序15
            public string RM { set; get; }        //基准质量 KG
            public string GVM { set; get; }        //最大总质量   KG
            public string ED { set; get; }        // 发动机排量 L
            public string EngineSpeed { set; get; }        //发动机额定转速
            public string EnginePower { set; get; }        // 发动额定机功率 Kw
            public string Passcap { set; get; }        //座位数
            public string Manuf { set; get; }        //车辆生产企业
            public string MDate { set; get; }        //车辆出厂日期 yyyyMMddHHmmss
            public string FuelWay { set; get; }        // 供油方式    详见数据字典8.2 序20
            public string DriveMode { set; get; }        // 驱动方式 详见数据字典8.2 序7
            public string Brand { set; get; }        // 品牌/型号
            public string Gear { set; get; }        // 变速器型式 详见数据字典8.2 序8
            public string AirIn { set; get; }        //进气方式 详见数据字典8.2 序6
            public string isOBD { set; get; }        //是否有OBD
            public string VehicleDesc { set; get; }        //特殊车况说明
            public string VehicleType { set; get; }        //车辆类型 详见数据字典8.1 序5
            public string Owner { set; get; }        //联系人
            public string OwnerAddr { set; get; }        //联系人地址
            public string OwnerTel { set; get; }        //联系人方式

        }
    }
}
