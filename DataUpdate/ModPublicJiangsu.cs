using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataUpdate
{
    class ModPublicJiangsu
    {
        public static string webAddress;  //接口地址
        public static string xcPDAurl;//PDAservice
        public static bool getPicFromXcPDA = false;
        public static string bdWebAddress;
        public static string unitid;//机构编码
        public static string LwUserName;//用户名
        public static string LwUserPass;//用户名密码
        public static string CityCode;//用户名密码
        public static Dictionary<string,string> DicLineid;
        public static Dictionary<string, DateTime> DicLineYxq;
        public static JsInterfaceInspection.VehicleServiceClient coa;//= new JsInterfaceInspection.VehicleServiceClient();
        public static JsInterfaceBd.BdServiceClient cobd;// = new JsInterfaceBd.BdServiceClient();
        public static JbtNetLibrary.OBD_lr wjserver ;

    }
}
