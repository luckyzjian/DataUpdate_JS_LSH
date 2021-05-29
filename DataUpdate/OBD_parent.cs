using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace JbtNetLibrary
{
    public abstract class OBD_parent
    {        
        public string getIUPRRate(double a,double b)
        {
            if (b > 1)
                return (a * 100 / b).ToString("0.0");
            else
                return "0";
        }
        public int rllx_intest = 0;
        public string vin = "";
        public int index = 1;
        public string DPF = "";

        public enum ENUM_STARTTEST_ITEM { STARTTEST_OBDTEST,STARTTEST_REALDATA,STARTTEST_NEWCAROBDTEST};
        public struct outlookPic
        {
            public string jylsh;
            public string jccs;
            public string photocode;
            public DateTime pssj;
            public string photo;
        }
        /// <summary>
        /// 0-等待检测中 1-扫描接口 2-扫描完毕 3-获取车辆信息 4-获取ECU信息 5-获取故障代码 6-获取就绪状态
        /// </summary>
        public int wait_process = 0;
        public ENUM_STARTTEST_ITEM itemNow;
        public bool VCIisConnected = false;

        #region 实现OBD接口
        /// <summary>
        /// 终止OBD服务，在车辆检测退出事件时调用
        /// </summary>
        /// <returns></returns>
        //public abstract bool closeServer();
        /// <summary>
        /// 开始OBD服务，初始化OBD实例后调用
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public abstract bool startService(out string msg);
        /// <summary>
        /// 停止OBD服务，无需调用
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public abstract bool stopService(out string msg);
        
        public virtual bool getOutlookPic(string jylsh, string jccs, string photocode, out string code, out string msg, out outlookPic pic)
        {
            code = "-1";
            msg = "暂不提供该接口";
            pic = new outlookPic();
            return false;
        }
        public virtual bool getVersion(out string versionnumber, out string code, out string msg)
        {
            versionnumber = "";
            code = "-1";
            msg = "暂不提供该接口";
            return false;
        }
        #endregion
    }
}
