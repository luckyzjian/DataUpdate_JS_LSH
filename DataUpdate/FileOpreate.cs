using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.InteropServices;

namespace DataUpdate
{
    //文件相关操作（含ini文件及TXT文件）
    public class FileOpreate
    {
        private static string configpath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config.ini";
        private static string logpath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "logs";

        //数据库配置信息
        public static string DB_Address = "";
        public static string DB_Name = "";
        public static string DB_User = "";
        public static string DB_Pwd = "";
        //软件操作员配置信息（保存用户名密码时用）
        public static string User_Name = "";
        public static string User_Pwd = "";
        public static bool SaveUser = false;
        public static bool SavePwd = false;
        public static string NM_Zlkzr = "";
        public static string NM_Sqr = "";
        //联网配置信息
        public static string Interface_Address = "";
        public static string Interface_BdAddress = "";
        public static string Interface_Jkxlh = "";
        public static string Interface_JczID = "";
        public static string Interface_JczUser = "";
        public static string Interface_JczPass = "";
        public static string Interface_JczUnitid = "";
        public static string Interface_JcCityCode = "";
        public static bool getPicFromXcPDA = false;
        public static string xcPDAurl = "";
        public static Dictionary<string, string> Interface_LineidDic = new Dictionary<string, string>();
        public static Dictionary<string, DateTime> Interface_LineJdYxqDic = new Dictionary<string, DateTime>();
        public static int Interface_Area = 0;
        //软件配置
        public static int UpdateStyle = 0;
        public static int UpdateFailSovle = 0;
        public static int UpdateGetWaitListTime = 15;
        public static int UpdateAutoGetCarList = 1;
        
        public struct jcff
        {
            public static bool sds = false;
            public static bool asm = false;
            public static bool vmas = false;
            public static bool jzjs = false;
            public static bool zyjs = false;
        }

        

        /// <summary>
        /// 获取数据库配置信息
        /// </summary>
        /// <returns>是否获取成功</returns>
        public static bool GetDBConfig()
        {
            if (File.Exists(configpath))
            {
                try
                {
                    StringBuilder temp = new StringBuilder();
                    temp.Length = 2048;
                    GetPrivateProfileString("数据库", "服务器", "192.168.0.123", temp, 2048, @".\Config.ini");
                    DB_Address = temp.ToString();
                    GetPrivateProfileString("数据库", "数据库名", "SX_ASMV30", temp, 2048, @".\Config.ini");
                    DB_Name = temp.ToString();
                    GetPrivateProfileString("数据库", "用户名", "sa", temp, 2048, @".\Config.ini");
                    DB_User = temp.ToString();
                    GetPrivateProfileString("数据库", "密码", "123456", temp, 2048, @".\Config.ini");
                    DB_Pwd = temp.ToString();

                    temp.Clear();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return false;
        }

        /// <summary>
        /// 获取软件人员信息
        /// </summary>
        /// <returns>是否获取成功</returns>
        public static bool GetUserconfig()
        {
            if (File.Exists(configpath))
            {
                try
                {
                    StringBuilder temp = new StringBuilder();
                    temp.Length = 2048;
                    GetPrivateProfileString("用户", "用户名", "", temp, 2048, @".\Config.ini");
                    User_Name = temp.ToString();
                    GetPrivateProfileString("用户", "密码", "", temp, 2048, @".\Config.ini");
                    User_Pwd = temp.ToString();
                    GetPrivateProfileString("用户", "SU", "N", temp, 2048, @".\Config.ini");
                    SaveUser = temp.ToString() == "Y" ? true : false;
                    GetPrivateProfileString("用户", "SP", "N", temp, 2048, @".\Config.ini");
                    SavePwd = temp.ToString() == "Y" ? true : false;
                    GetPrivateProfileString("用户", "内蒙联网质量控制人", "", temp, 2048, @".\Config.ini");
                    NM_Zlkzr = temp.ToString();
                    GetPrivateProfileString("用户", "内蒙联授权人", "", temp, 2048, @".\Config.ini");
                    NM_Sqr = temp.ToString();
                    temp.Clear();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return false;
        }

        /// <summary>
        /// 获取联网配置信息
        /// </summary>
        /// <returns>是否获取成功</returns>
        public static bool GetInterfaceconfig()
        {
            if (File.Exists(configpath))
            {
                try
                {
                    StringBuilder temp = new StringBuilder();
                    temp.Length = 2048;
                    GetPrivateProfileString("联网信息", "接口地址", "", temp, 2048, @".\Config.ini");
                    Interface_Address = temp.ToString();
                    GetPrivateProfileString("联网信息", "标定接口地址", "", temp, 2048, @".\Config.ini");
                    Interface_BdAddress = temp.ToString();
                    GetPrivateProfileString("联网信息", "接口序列号", "", temp, 2048, @".\Config.ini");
                    Interface_Jkxlh = temp.ToString();
                    GetPrivateProfileString("联网信息", "检测站ID", "", temp, 2048, @".\Config.ini");
                    Interface_JczID = temp.ToString();
                    GetPrivateProfileString("联网信息", "用户名Js", "", temp, 2048, @".\Config.ini");
                    Interface_JczUser = temp.ToString();
                    GetPrivateProfileString("联网信息", "密码Js", "", temp, 2048, @".\Config.ini");
                    Interface_JczPass = temp.ToString();
                    GetPrivateProfileString("联网信息", "城市代码", "", temp, 2048, @".\Config.ini");
                    Interface_JcCityCode = temp.ToString();
                    GetPrivateProfileString("检测方法", "SDS", "N", temp, 2048, @".\Config.ini");
                    jcff.sds = temp.ToString() == "Y" ? true : false;
                    GetPrivateProfileString("检测方法", "ASM", "N", temp, 2048, @".\Config.ini");
                    jcff.asm = temp.ToString() == "Y" ? true : false;
                    GetPrivateProfileString("检测方法", "VMAS", "N", temp, 2048, @".\Config.ini");
                    jcff.vmas = temp.ToString() == "Y" ? true : false;
                    GetPrivateProfileString("检测方法", "JZJS", "N", temp, 2048, @".\Config.ini");
                    jcff.jzjs = temp.ToString() == "Y" ? true : false;
                    GetPrivateProfileString("检测方法", "ZYJS", "N", temp, 2048, @".\Config.ini");
                    jcff.zyjs = temp.ToString() == "Y" ? true : false;
                    for(int i=1;i<=6;i++)
                    {
                        string lineid = i.ToString("00");
                        GetPrivateProfileString("联网信息", lineid + "号线线号", "", temp, 2048, @".\Config.ini");
                        Interface_LineidDic.Add(lineid,temp.ToString());
                        GetPrivateProfileString("联网信息", lineid + "号线检定有效期", "2020-01-01", temp, 2048, @".\Config.ini");
                        DateTime tempdate=DateTime.Now;
                        DateTime.TryParse(temp.ToString(), out tempdate);
                        Interface_LineJdYxqDic.Add(lineid, tempdate);
                    }
                    GetPrivateProfileString("xcPDA", "接口地址", "", temp, 2048, @".\Config.ini");
                    xcPDAurl = temp.ToString();
                    GetPrivateProfileString("xcPDA", "是否使用", "N", temp, 2048, @".\Config.ini");
                    getPicFromXcPDA = temp.ToString() == "Y" ? true : false;
                    temp.Clear();
                    Interface_Area = GetPrivateProfileInt("联网信息", "联网地区", 0, @".\Config.ini");
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return false;
        }

        /// <summary>
        /// 获取软件配置
        /// </summary>
        /// <returns>是否获取成功</returns>
        public static bool GetSoftconfig()
        {
            if (File.Exists(configpath))
            {
                try
                {
                    UpdateStyle = GetPrivateProfileInt("软件配置", "上传方式", 0, @".\Config.ini");
                    UpdateFailSovle = GetPrivateProfileInt("软件配置", "上传失败处理方式", 0, @".\Config.ini");
                    UpdateGetWaitListTime = GetPrivateProfileInt("软件配置", "待检列表刷新间隔", 15, @".\Config.ini");
                    if (UpdateGetWaitListTime < 15)
                        UpdateGetWaitListTime = 15;
                    UpdateAutoGetCarList = GetPrivateProfileInt("软件配置", "是否开启自动获取待检列表", 1, @".\Config.ini");
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return false;
        }

        /// <summary>
        /// 获取配置文件保存的软件上次启动的时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetClearTime()
        {
            if (File.Exists(configpath))
            {
                try
                {
                    StringBuilder temp = new StringBuilder();
                    temp.Length = 2048;
                    GetPrivateProfileString("软件配置", "软件启动时间", "", temp, 2048, @".\Config.ini");
                    return Convert.ToDateTime(temp.ToString());
                }
                catch { return DateTime.Now; }
            }
            else
                return DateTime.Now;
        }
        

        public static void SaveClearTime(string time_now)
        {
            try
            {
                WritePrivateProfileString("软件配置", "软件启动时间", time_now, @"./Config.ini");
            }
            catch{}
        }
        
        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="logs">日志内容</param>
        /// <param name="log_name">日志名称，如SocketLog、ErrorLog</param>
        /// <param name="log_kind">日志类别，1:runlog；2:dblog；3:netlog；4:errorlog；其他直接保存在log目录下</param>
        /// <returns>是否保存成功</returns>
        public static void SaveLog(string logs, string log_name, int log_kind)
        {
            try
            {
                string today_logpath = logpath + "\\" + System.DateTime.Now.ToString("yyyy-MM-dd");
                if (Directory.Exists(today_logpath) == false)
                    Directory.CreateDirectory(today_logpath);
                //switch (log_kind)
                //{
                //    case 1://运行日志
                //        today_logpath = today_logpath + "\\" + System.DateTime.Now.Hour.ToString() + "_Runninglog.txt";
                //        break;
                //    case 2://数据库日志
                //        today_logpath = today_logpath + "\\" + System.DateTime.Now.Hour.ToString() + "_DBlog.txt";
                //        break;
                //    case 3://联网日志
                //        today_logpath = today_logpath + "\\" + System.DateTime.Now.Hour.ToString() + "_Netlog.txt";
                //        break;
                //    default://
                //        today_logpath = today_logpath + "\\" + System.DateTime.Now.Hour.ToString() + "_Otherlog.txt";
                //        break;
                //}
                today_logpath = today_logpath + "\\" + System.DateTime.Now.Hour.ToString() + "_log.txt";
                FileStream fs = new FileStream(today_logpath, FileMode.Append, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(System.DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "——" + log_name + " ：" + "\r\n" + logs + " \r\n ");
                sw.Close();
            }
            catch{}
        }

        #region INI读写DLL
        /// <summary>
        /// 写配置文件
        /// </summary>
        /// <param name="strAppName">段名</param>
        /// <param name="strKeyName">字段名</param>
        /// <param name="strString">内容</param>
        /// <param name="strFileName">文件路径包括名字</param>
        /// <returns>bool</returns>
        [DllImport("Kernel32.dll")]
        public static extern bool WritePrivateProfileString(string strAppName, string strKeyName, string strString, string strFileName);//写配置文件（段名，字段，字段值，路径）

        /// <summary>
        /// 以字符串形式读配置文件
        /// </summary>
        /// <param name="strAppName">段名</param>
        /// <param name="strKeyName">字段名</param>
        /// <param name="strDefault">默认值</param>
        /// <param name="sbReturnString">StringBuilder</param>
        /// <param name="nSize">StringBuilder大小</param>
        /// <param name="strFileName">文件路径包括名字</param>
        /// <returns>int</returns>
        [DllImport("Kernel32.dll")]
        public static extern int GetPrivateProfileString(string strAppName, string strKeyName, string strDefault, StringBuilder sbReturnString, int nSize, string strFileName);//读配置文件 string（段名，字段，默认值，保存的strbuilder，大小，路径）

        /// <summary>
        /// 以int形式读配置文件
        /// </summary>
        /// <param name="strAppName">段名</param>
        /// <param name="strKeyName">字段名</param>
        /// <param name="nDefault">默认值</param>
        /// <param name="strFileName">文件路径包括名字</param>
        /// <returns>int</returns>
        [DllImport("Kernel32.dll")]
        public static extern int GetPrivateProfileInt(string strAppName, string strKeyName, int nDefault, string strFileName);//读配置文件 int（段名，字段，默认值，路径）
        #endregion
    }
}