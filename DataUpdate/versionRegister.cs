using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DataUpdate
{
    class versionRegister
    {
        [DllImport("syncauth.dll", EntryPoint = "lastError")]
        /**
         * 
         * @return 错误码为0时,可以调用本接口观察错误具体原因， 返回的是错误原因的gbk编码内容，内存由dll进行管理，c#这边不要删除该内存。
         */
        public static extern IntPtr lastError();


        [DllImport("syncauth.dll", EntryPoint = "doAuth")]

        /**
         * 
         * @param programeWorkDir 程序的工作目录gbk编码,doAuth需要在本目录下加载valid.txt文件,如果找不到会报1101错误。 内存由C#这边管理
         * @param appVersion     app的版本号gbk编码，  内存由C#这边管理
         * @param szReturnStr    透传auth返回的结果，gbk编码。 内存由C#这边提前分配好，传给dll
         * @return  错误码,0表示成功,1 - 99 是webservice调用协议错误, 1001及以后错误码是业务错误
         */
        public static extern int doAuth([MarshalAs(UnmanagedType.LPArray)] byte[] programeWorkDir, [MarshalAs(UnmanagedType.LPArray)] byte[] appVersion, byte[] szReturnStr);

        private static string PtrToString(IntPtr ptr) // aPtr is nul-terminated
        {
            if (ptr == IntPtr.Zero)
                return "";
            int len = 0;
            while (System.Runtime.InteropServices.Marshal.ReadByte(ptr, len) != 0)
                len++;
            if (len == 0)
                return "";
            byte[] array = new byte[len];
            System.Runtime.InteropServices.Marshal.Copy(ptr, array, 0, len);
            return System.Text.Encoding.Default.GetString(array);
        }

        private static string BufferToString(byte[] buff) // buff is nul-terminated
        {

            int len = buff.Length;
            int i = 0;
            for (; i < len; i++)
            {
                if (buff[i] != 0)
                {
                    //continue
                }
                else
                {
                    break;
                }
            }
            return System.Text.Encoding.Default.GetString(buff, 0, i);
        }

        public static bool registerVersion(string workDir, string version)
        {
            byte[] returnBuffer = new byte[1024 * 8];

            //string version = "csharpDemoV1.0";
            byte[] byteInputVersion = System.Text.Encoding.Default.GetBytes(version);


            //string workDir = ".";

            byte[] byteInputWorkDir = System.Text.Encoding.Default.GetBytes(workDir);
            DataUpdate.FileOpreate.SaveLog(string.Format("doAuth({0},{1})", workDir, version), "[版本注册]", 1);// + authCode + ", returnStr:" + returnStr + ", lastError:" + PtrToString(lastError()), "[版本注册]", 1);

            int authCode = doAuth(byteInputWorkDir, byteInputVersion, returnBuffer);

            string returnStr = BufferToString(returnBuffer);


            DataUpdate.FileOpreate.SaveLog("authCode:" + authCode + ", returnStr:" + returnStr + ", lastError:" + PtrToString(lastError()),"[版本注册]",1);


            //Console.ReadKey();
            return authCode==0;

        }
    }
}
