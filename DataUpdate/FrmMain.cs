using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections;

namespace DataUpdate
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }
        private DataTable dt_AlreadyUpload = null;//已上传表
        private DataTable dt_UploadFailed = null;//已上传表
        private DataTable dt_ReadyUpload = null;//待上传、上传失败表
        private DataTable dt_ZjBdUpload = null;

        public static ConnForSQL conforsql = new ConnForSQL();
        private DBControl carinfobd = new DBControl();//定义数据库操作

        private Interface InterfaceUpload = null;//定义联网操作
        private interfaceJinhua interfaceUploadJh = null;
        private interfacejiangsu interfacejiangsu = null;
        private bool _auto_upload = false;//初始定义上传模式为手动
        private bool _whether_Connected = false;//定义联网是否可以正常工作
        private bool _clearcarlistat500 = true;//用于判断程序是否在6点左右执行了一次定时清理待检列表
        private int Choosedindex = 0;//手动上传时被选中行的行号
        private string ChoosedJylsh = "";//删除待检列表时 选中的流水号
        private Dictionary<string, string> JcZt = new Dictionary<string, string>();
        private Dictionary<string, string> Dic_BDZJ = new Dictionary<string, string>();
        private Dictionary<string, string> Dic_ZJLX = new Dictionary<string, string>();

        #region 更新上传过程状态表显示
        public delegate void udgv(string jylsh, string jycs, string column_name, string zt);
        /// <summary>
        /// 更新软件界面待上传车辆显示状态
        /// </summary>
        /// <param name="jylsh">检验流水号</param>
        /// <param name="jycs">检验次数</param>
        /// <param name="column_name">列名</param>
        /// <param name="zt">状态</param>
        public void ShowStatus(string jylsh, string jycs, string column_name, string zt)
        {
            try
            {
                if (dt_ReadyUpload != null)
                {
                    DataRow[] temp_dr = dt_ReadyUpload.Select("检验流水号 = '" + jylsh + "' and 检验次数 = '" + jycs + "'");
                    if (temp_dr.Count() > 0)
                    {
                        dt_ReadyUpload.Rows[dt_ReadyUpload.Rows.IndexOf(temp_dr[0])][column_name] = zt;
                    }
                }
            }
            catch { }
        }
        //用于线程中更新检测状态到备注
        public void UpdateStatus(string jylsh, string jycs, string column_name, string zt)
        {
            try
            {
                BeginInvoke(new udgv(ShowStatus), jylsh, jycs, column_name, zt);
            }
            catch { }
        }
        #endregion
        private void test()
        {
            Hashtable hstb = new Hashtable();
            hstb.Add("CLHP", "川A12345");
            hstb.Add("CPYS", "蓝");
            hstb.Add("CLLX", "K34");

            if (conforsql.InsertOrUpdate("车辆信息", " where CLHP='" + hstb["CLHP"] + "' and CPYS='" + hstb["CPYS"] + "'", hstb) > 0)
            {
                FileOpreate.SaveLog(hstb["CLHP"] + "车辆信息更新成功", "DBWrite1", 1);//3、更新待检列表信息
            }
            else
                FileOpreate.SaveLog(hstb["CLHP"] + "更新车辆信息表失败，无法加入待检列表", "DBError2", 1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clid"></param>
        /// <param name="bgbh">检测报告编号（存字段 LWH)</param>
        /// <param name="qcrode">加密二维码串（存字段 YWLSH)</param>
        /// <returns></returns>
        private bool updateTestingID(string clid, string bgbh,string qcrode)
        {
            Hashtable hstb = new Hashtable();
            hstb.Add("LSH", bgbh);
            hstb.Add("YWLSH", qcrode);

            if (conforsql.InsertOrUpdate("已检车辆信息", " where CLID='" + clid + "'", hstb) > 0)
            {
                FileOpreate.SaveLog(clid + "已检车辆信息 testingid[YWLSH]更新成功", "DBWrite1", 1);//3、更新待检列表信息
                return true;
            }
            else
            {
                FileOpreate.SaveLog(clid + "已检车辆信息 testingid[YWLSH]更新失败", "DBError2", 1);
                return false;
            }

        }
        private bool updateJCBGBH(string clid, string jcbgbh)
        {
            Hashtable hstb = new Hashtable();
            hstb.Add("LSH", jcbgbh);

            if (conforsql.InsertOrUpdate("已检车辆信息", " where CLID='" + clid + "'", hstb) > 0)
            {
                FileOpreate.SaveLog(clid + "已检车辆信息 检测报告编号[LSH]更新成功", "DBWrite1", 1);//3、更新待检列表信息
                return true;
            }
            else
            {
                FileOpreate.SaveLog(clid + "已检车辆信息 检测报告编号[LSH]更新失败", "DBError2", 1);
                return false;
            }

        }
        string version = "1.0";
        private void GetVersion()
        {
            version = System.Configuration.ConfigurationManager.AppSettings["Version"].ToString();
            /*try
            {
                // 创建一个 StreamReader 的实例来读取文件 
                // using 语句也能关闭 StreamReader
                string updatesjson = "";
                using (StreamReader sr = new StreamReader(Application.StartupPath+"/updates.json"))
                {
                    string line;

                    // 从文件读取并显示行，直到文件的末尾 
                    while ((line = sr.ReadLine()) != null)
                    {
                        updatesjson += line;
                        //Console.WriteLine(line);
                    }
                }
                updateInf updateinf = Newtonsoft.Json.JsonConvert.DeserializeObject<updateInf>(updatesjson);
                version = updateinf.latestversion;
            }
            catch (Exception e)
            {
                version = "3.0.0";
            }*/
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            #region 获取配置信息

            if (FileOpreate.GetDBConfig() == false)
            {
                MessageBox.Show("获取数据库配置信息失败！");
                return;
            }
            if (FileOpreate.GetInterfaceconfig() == false)
            {
                MessageBox.Show("获取联网配置信息失败！");
                return;
            }
            //ModPublicJiangsu.webAddress = FileOpreate.Interface_Address;
            //ModPublicJiangsu.bdWebAddress = FileOpreate.Interface_BdAddress;
            //ModPublicJiangsu.xcPDAurl = FileOpreate.xcPDAurl;
            //ModPublicJiangsu.getPicFromXcPDA = FileOpreate.getPicFromXcPDA;
            //ModPublicJiangsu.LwUserName = FileOpreate.Interface_JczUser;
            //ModPublicJiangsu.LwUserPass = FileOpreate.Interface_JczPass;
            //ModPublicJiangsu.unitid = FileOpreate.Interface_JczID;
            //ModPublicJiangsu.CityCode = FileOpreate.Interface_JcCityCode;
            //ModPublicJiangsu.DicLineid = FileOpreate.Interface_LineidDic;
            //ModPublicJHHB.jksqm = FileOpreate.Interface_Jkxlh;
            //ModPublicJHHB.TsNo = FileOpreate.Interface_JczID;

            if (FileOpreate.GetSoftconfig() == false)
            {
                MessageBox.Show("获取软件配置信息失败！");
                return;
            }
            if (FileOpreate.GetUserconfig() == false)
            {
                MessageBox.Show("登录初始化失败！");
                return;
            }
            #endregion

            #region 功能测试区
            InitJcZt();
            #endregion

            #region 显示登录界面
            LoginFrom loginfrom = new LoginFrom();
            loginfrom.ShowDialog();
            if (loginfrom.LoginStatus == false)
            {
                this.Close();
                return;
            }
            ModPublicJiangsu.webAddress = FileOpreate.Interface_Address;
            ModPublicJiangsu.bdWebAddress = FileOpreate.Interface_BdAddress;
            ModPublicJiangsu.xcPDAurl = FileOpreate.xcPDAurl;
            ModPublicJiangsu.getPicFromXcPDA = FileOpreate.getPicFromXcPDA;
            ModPublicJiangsu.LwUserName = FileOpreate.Interface_JczUser;
            ModPublicJiangsu.LwUserPass = FileOpreate.Interface_JczPass;
            ModPublicJiangsu.unitid = FileOpreate.Interface_JczID;
            ModPublicJiangsu.CityCode = FileOpreate.Interface_JcCityCode;
            ModPublicJiangsu.DicLineid = FileOpreate.Interface_LineidDic;
            ModPublicJiangsu.DicLineYxq = FileOpreate.Interface_LineJdYxqDic;

            ModPublicJHHB.jksqm = FileOpreate.Interface_Jkxlh;
            ModPublicJHHB.TsNo = FileOpreate.Interface_JczID;
            //test();
            FileOpreate.SaveLog("软件登录成功", "当前状态", 1);
            #endregion
            #region 注册版本号
            GetVersion();
            this.Text = "江苏环检上传[Version:" + version + "]";
            
            if (!versionRegister.registerVersion(System.Environment.CurrentDirectory, version))
            {
                MessageBox.Show("注册版本号失败！");
                //return;
            }
            #endregion

            #region 控件初始位置设置

            dataGridView_AlreadyUpload.Location = new Point(1, 28);
            dataGridView_Ready_And_Failed.Location = new Point(1, 28);
            dataGridView_BDZJ.Location = new Point(1, 28);
            dataGridView_UploadFailed.Location = new Point(1, 28);

            dataGridViewWaitList.Location = new Point(1, 28);

            dataGridView_AlreadyUpload.Width = this.Size.Width - 20;
            dataGridView_Ready_And_Failed.Width = this.Size.Width - 20;
            dataGridView_BDZJ.Width = this.Size.Width - 20;
            dataGridView_UploadFailed.Width = this.Size.Width - 20;
            dataGridViewWaitList.Width = this.Size.Width - 20;

            dataGridView_Ready_And_Failed.BringToFront();
            #endregion

            #region 软件界面控件初始化
            _auto_upload = FileOpreate.UpdateStyle == 1 ? true : false;//数据上传方式，自动还是手动
            button_UpdateByMan.Visible = FileOpreate.UpdateFailSovle == 1 ? true : false;//上传失败处理方式，是否允许手动再次上传
            timerGetWaitCarList.Interval = FileOpreate.UpdateGetWaitListTime * 1000;//设置获取待检列表时间间隔

            #region 已上传表初始化并绑定数据源
            dt_AlreadyUpload = new DataTable();
            dt_AlreadyUpload.Columns.Add("检验流水号");
            dt_AlreadyUpload.Columns.Add("车辆号牌");
            dt_AlreadyUpload.Columns.Add("号牌种类");
            dt_AlreadyUpload.Columns.Add("线号");
            dt_AlreadyUpload.Columns.Add("检验次数");
            dt_AlreadyUpload.Columns.Add("检验时间");
            dt_AlreadyUpload.Columns.Add("检测方法");
            dt_AlreadyUpload.Columns.Add("上传成功时间");
            dataGridView_AlreadyUpload.DataSource = dt_AlreadyUpload;
            //dataGridView_AlreadyUpload.Columns["检验流水号"].Width = 160;
            //dataGridView_AlreadyUpload.Columns["车辆号牌"].Width = 100;
            //dataGridView_AlreadyUpload.Columns["号牌种类"].Width = 100;
            //dataGridView_AlreadyUpload.Columns["线号"].Width = 80;
            //dataGridView_AlreadyUpload.Columns["检验次数"].Width = 80;
            //dataGridView_AlreadyUpload.Columns["检验时间"].Width = 160;
            //dataGridView_AlreadyUpload.Columns["检测方法"].Width = 100;
            //dataGridView_AlreadyUpload.Columns["上传成功时间"].Width = 160;
            #endregion

            #region 待上传表初始化并绑定数据源
            dt_ReadyUpload = new DataTable();
            dt_ReadyUpload.Columns.Add("检验流水号");
            dt_ReadyUpload.Columns.Add("车辆号牌");
            dt_ReadyUpload.Columns.Add("号牌种类");
            dt_ReadyUpload.Columns.Add("线号");
            dt_ReadyUpload.Columns.Add("检验次数");
            dt_ReadyUpload.Columns.Add("检验时间");
            dt_ReadyUpload.Columns.Add("检测方法");
            dt_ReadyUpload.Columns.Add("当前状态");
            dt_ReadyUpload.Columns.Add("已处理状态");
            dt_ReadyUpload.Columns.Add("备注");
            dataGridView_Ready_And_Failed.DataSource = dt_ReadyUpload;
            //dataGridView_Ready_And_Failed.Columns["检验流水号"].Width = 100;
            //dataGridView_Ready_And_Failed.Columns["车辆号牌"].Width = 100;
            //dataGridView_Ready_And_Failed.Columns["号牌种类"].Width = 80;
            //dataGridView_Ready_And_Failed.Columns["线号"].Width = 60;
            //dataGridView_Ready_And_Failed.Columns["检验次数"].Width = 80;
            //dataGridView_Ready_And_Failed.Columns["检验时间"].Width = 100;
            //dataGridView_Ready_And_Failed.Columns["检测方法"].Width = 80;
            //dataGridView_Ready_And_Failed.Columns["当前状态"].Width = 80;
            //dataGridView_Ready_And_Failed.Columns["已处理状态"].Width = 100;
            //dataGridView_Ready_And_Failed.Columns["备注"].Width = 100;
            #endregion
            #region 上传失败表初始化并绑定数据源
            dt_UploadFailed = new DataTable();
            dt_UploadFailed.Columns.Add("检验流水号");
            dt_UploadFailed.Columns.Add("车辆号牌");
            dt_UploadFailed.Columns.Add("号牌种类");
            dt_UploadFailed.Columns.Add("线号");
            dt_UploadFailed.Columns.Add("检验次数");
            dt_UploadFailed.Columns.Add("检验时间");
            dt_UploadFailed.Columns.Add("检测方法");
            dt_UploadFailed.Columns.Add("当前状态");
            dt_UploadFailed.Columns.Add("已处理状态");
            dt_UploadFailed.Columns.Add("上传失败原因");
            dataGridView_UploadFailed.DataSource = dt_UploadFailed;
            //dataGridView_UploadFailed.Columns["检验流水号"].Width = 100;
            //dataGridView_UploadFailed.Columns["车辆号牌"].Width = 100;
            //dataGridView_UploadFailed.Columns["号牌种类"].Width = 80;
            //dataGridView_UploadFailed.Columns["线号"].Width = 60;
            //dataGridView_UploadFailed.Columns["检验次数"].Width = 80;
            //dataGridView_UploadFailed.Columns["检验时间"].Width = 100;
            //dataGridView_UploadFailed.Columns["检测方法"].Width = 80;
            //dataGridView_UploadFailed.Columns["当前状态"].Width = 80;
            //dataGridView_UploadFailed.Columns["已处理状态"].Width = 100;
            //dataGridView_UploadFailed.Columns["上传失败原因"].Width = 100;
            #endregion
            #region 自检标定记录表初始化
            dt_ZjBdUpload = new DataTable();
            dt_ZjBdUpload.Columns.Add("ID");
            dt_ZjBdUpload.Columns.Add("内容");
            dt_ZjBdUpload.Columns.Add("线号");
            dt_ZjBdUpload.Columns.Add("项目");
            dt_ZjBdUpload.Columns.Add("结果");
            dt_ZjBdUpload.Columns.Add("状态");
            dt_ZjBdUpload.Columns.Add("上传次数");
            dt_ZjBdUpload.Columns.Add("操作员");
            dataGridView_BDZJ.DataSource = dt_ZjBdUpload;

            #endregion

            #endregion

            #region 初始化PDA service
            if (FileOpreate.xcPDAurl != "" && FileOpreate.getPicFromXcPDA)
            {
                if (FileOpreate.xcPDAurl == "")
                {
                    MessageBox.Show("PDA配置信息不全，请完善接口配置后重试！");
                    return;
                }
                try
                {
                    ModPublicJiangsu.wjserver = new JbtNetLibrary.OBD_lr(FileOpreate.xcPDAurl, "");
                    string wjmsg = "";
                    if (!ModPublicJiangsu.wjserver.startWjService(out wjmsg))
                    {
                        MessageBox.Show("PDA启动失败，失败信息：" + wjmsg);
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show("初始化PDA失败，错误原因：" + er.Message);
                    return;
                }
            }
            #endregion

            #region 联网初始化
            if (FileOpreate.Interface_Address == "" || FileOpreate.Interface_JczID == "" || FileOpreate.Interface_Jkxlh == "")
            {
                MessageBox.Show("联网接口配置信息不全，请完善接口配置后重试！");
                return;
            }
            else
            {

                #region 直接返回时间查询成功与否及时间结果
                try
                {
                    //江苏地址暂不同步时间
                    _whether_Connected = true;
                    //ModPublicJiangsu.coa. = ModPublicJiangsu.webAddress;
                    ModPublicJiangsu.coa = new JsInterfaceInspection.VehicleServiceClient("IVehicleServicePort", ModPublicJiangsu.webAddress);
                    ModPublicJiangsu.cobd = new JsInterfaceBd.BdServiceClient("IBdServicePort", ModPublicJiangsu.bdWebAddress);

                    interfacejiangsu = new interfacejiangsu();
                    timerGetWaitCarList_Tick(null, null);
                    timerGetWaitCarList.Enabled = true;
                    timerGetWaitCarList.Start();
                }
                catch (Exception er)
                {
                    _whether_Connected = false;
                    FileOpreate.SaveLog("联网接口初始化错误，原因：" + er.Message, "接口初始化", 1);
                    MessageBox.Show("联网接口初始化错误，错误原因：" + er.Message);
                    return;
                }
                #endregion
            }
            #endregion

        }

        #region 软件设置各按钮
        //窗口大小变化时强制重绘窗体
        private void FrmMain_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void 开始自动上传ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_whether_Connected == true)
            {
                timerUploadTestData.Enabled = true;
                timerRefreshFaileData.Enabled = true;
                timerBdZjUpdate.Enabled = true;
                tSSL_Status.Text = "状态：已开启数据自动上传！";
            }
            else
                MessageBox.Show("请先正常联网并上传标定和自检数据后再试！");
        }

        private void 关闭自动上传ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timerUploadTestData.Enabled = false;
            timerRefreshFaileData.Enabled = false;
            timerBdZjUpdate.Enabled = false;
            tSSL_Status.Text = "状态：数据自动上传关闭！";
        }

        private void 待上传数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView_UploadFailed.Visible = false;
            dataGridView_AlreadyUpload.Visible = false;
            dataGridView_BDZJ.Visible = false;
            dataGridViewWaitList.Visible = false;
            dataGridView_Ready_And_Failed.Visible = true;
            tSSL_Status.Text = "状态：当前显示正在上传或上传失败数据列表！";
        }

        private void 已上传数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView_UploadFailed.Visible = false;
            dataGridView_AlreadyUpload.Visible = true;
            dataGridView_Ready_And_Failed.Visible = false;
            dataGridViewWaitList.Visible = false;
            tSSL_Status.Text = "状态：当前显示已上传数据列表！";
        }

        //手动上传按钮
        private void button_UpdateByMan_Click(object sender, EventArgs e)
        {
            if (!_whether_Connected)//若同步时间失败则不上传
            {
                MessageBox.Show("接口未正常初始化，不能手动上传！");
                return;
            }

            try
            {
                string jylsh = dt_ReadyUpload.Rows[Choosedindex]["检验流水号"].ToString();
                string jycs = dt_ReadyUpload.Rows[Choosedindex]["检验次数"].ToString();
                string lineid = dt_ReadyUpload.Rows[Choosedindex]["线号"].ToString();
                DataRow CarTestStatusRow = carinfobd.getCarTestStatusByLsh(jylsh, jycs);

                CarTestStatusRow["HPZL"] = InterfaceUpload.get_hpzl_code[CarTestStatusRow["HPZL"].ToString()];
                if (CarTestStatusRow != null)
                {
                    string tempZT = CarTestStatusRow["ZT"].ToString();
                    string tempYCLZT = CarTestStatusRow["YCLZT"].ToString();

                    //3为联网验证未通过，5为检测未完成，当前状态不为1且当前状态等于已处理状态，以上不执行
                    //其他都要执行（不能正常执行的重新开始检测后zt会自动更新为1）
                    if (tempZT == "3" || tempZT == "5" || tempZT == tempYCLZT)
                        return;
                    //正常开始之后关闭检查软件导致ZT与YCLZT相同的还原
                    if (tempZT == "1" && tempYCLZT == "1")
                        carinfobd.UpdateCarTestStatusYCLZT("0", jylsh, jycs);
                    try
                    {
                        switch (CarTestStatusRow["JCFF"].ToString())
                        {
                            case "ASM":
                                if (tempZT == "1" || tempZT == "21" || tempZT == "22" || tempZT == "23" || tempZT == "24" || tempZT == "6")
                                {
                                    Thread AsmUploadThread = new Thread(new ParameterizedThreadStart(UploadASMData));//新建一个过程的上传线程
                                    AsmUploadThread.Start(CarTestStatusRow);//开始上传线程
                                    //carinfobd.UpdateCarTestStatusYCLZT(tempZT, jylsh, jycs);
                                    ShowStatus(jylsh, jycs, "备注", "ASM手动上传中...");
                                    FileOpreate.SaveLog("ASM数据手动上传线程已开始", "检测数据上传", 1);
                                }
                                break;
                            /*
                        case "VMAS":
                            if (tempZT == "1" || tempZT == "31" || tempZT == "32" || tempZT == "33" || tempZT == "6")
                            {
                                Thread VmasUploadThread = new Thread(new ParameterizedThreadStart(UploadVMASData));
                                VmasUploadThread.Start(CarTestStatusRow);

                                ShowStatus(jylsh, jycs, "备注", "VMAS手动上传中...");
                                FileOpreate.SaveLog("VMAS数据手动上传线程已开始", "检测数据上传", 1);
                            }
                            break;
                            */
                            case "SDS":
                                if (tempZT == "1" || tempZT == "11" || tempZT == "12" || tempZT == "6")
                                {
                                    Thread SdsUploadThread = new Thread(new ParameterizedThreadStart(UploadSDSData));
                                    SdsUploadThread.Start(CarTestStatusRow);
                                    //carinfobd.UpdateCarTestStatusYCLZT(tempZT, jylsh, jycs);
                                    ShowStatus(jylsh, jycs, "备注", "SDS手动上传中...");
                                    FileOpreate.SaveLog("SDS数据手动上传线程已开始", "检测数据上传", 1);
                                }
                                break;
                            case "SDSM":
                                if (tempZT == "1" || tempZT == "81" || tempZT == "82" || tempZT == "6")
                                {
                                    Thread SdsMUploadThread = new Thread(new ParameterizedThreadStart(UploadSDSMData));
                                    SdsMUploadThread.Start(CarTestStatusRow);
                                    //carinfobd.UpdateCarTestStatusYCLZT(tempZT, jylsh, jycs);
                                    ShowStatus(jylsh, jycs, "备注", "SDSM手动上传中...");
                                    FileOpreate.SaveLog("SDSM数据手动上传线程已开始", "检测数据上传", 1);
                                }
                                break;
                            case "JZJS":
                                if (tempZT == "1" || tempZT == "41" || tempZT == "42" || tempZT == "43" || tempZT == "6")
                                {
                                    Thread JzjsUploadThread = new Thread(new ParameterizedThreadStart(UploadJZJSData));
                                    JzjsUploadThread.Start(CarTestStatusRow);
                                    //carinfobd.UpdateCarTestStatusYCLZT(tempZT, jylsh, jycs);
                                    ShowStatus(jylsh, jycs, "备注", "JZJS手动上传中...");
                                    FileOpreate.SaveLog("JZJS数据手动上传线程已开始", "检测数据上传", 1);
                                }
                                break;
                            case "ZYJS":
                                if (tempZT == "1" || tempZT == "51" || tempZT == "52" || tempZT == "53" || tempZT == "6")
                                {
                                    Thread ZyjsUploadThread = new Thread(new ParameterizedThreadStart(UploadZYJSData));
                                    ZyjsUploadThread.Start(CarTestStatusRow);
                                    //carinfobd.UpdateCarTestStatusYCLZT(tempZT, jylsh, jycs);
                                    ShowStatus(jylsh, jycs, "备注", "ZYJS手动上传中...");
                                    FileOpreate.SaveLog("ZYJS数据手动上传线程已开始", "检测数据上传", 1);
                                }
                                break;
                        }
                    }
                    catch (Exception er)
                    {
                        FileOpreate.SaveLog("开启线程失败，原因：" + er.Message, "检测数据上传", 1);
                    }
                    button_UpdateByMan.Enabled = false;
                }
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("失败原因：" + er.Message, "手动上传错误", 1);
                MessageBox.Show(er.Message, "手动上传错误");
            }
        }

        //删除选中项目
        private void 删除上传失败记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView_Ready_And_Failed.Visible == true && MessageBox.Show("您确定要删除该行数据吗？", "请注意", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                try
                {
                    string jylsh = dt_ReadyUpload.Rows[Choosedindex]["检验流水号"].ToString();
                    string jycs = dt_ReadyUpload.Rows[Choosedindex]["检验次数"].ToString();

                    //待检列表、检测状态表、上传软件界面都删一次操作
                    string strDeleteResult = "状态：流水号" + jylsh + "，检验次数" + jycs;
                    if (carinfobd.deleteCarInWaitlist(jylsh, jycs))//删除本地待检列表
                        strDeleteResult += "本地待检列表删除成功|";
                    else
                        strDeleteResult += "本地待检列表删除失败|";
                    if (carinfobd.DeleteCarTestStatus(jylsh, jycs))//删除车辆检测状态表
                        strDeleteResult += "本地车辆检测状态表删除成功|";
                    else
                        strDeleteResult += "本地车辆检测状态表删除失败|";
                    DataRow[] ReadyToDelete = dt_ReadyUpload.Select("检验流水号 = '" + jylsh + "' and 检验次数 = '" + jycs + "'");
                    if (ReadyToDelete.Count() > 0)//软件界面有这个检验流水号及检验次数的记录就删除
                    {
                        for (int j = 0; j < ReadyToDelete.Count(); j++)
                        {
                            dt_ReadyUpload.Rows.Remove(ReadyToDelete[j]);
                        }
                        strDeleteResult += "上传软件界面记录删除成功";
                    }
                    strDeleteResult += "上传软件界面无记录";
                    tSSL_Status.Text = strDeleteResult;
                    FileOpreate.SaveLog(strDeleteResult, "待检车辆信息删除", 1);
                }
                catch (Exception er)
                {
                    tSSL_Status.Text = "数据删除错误：该行数据删除失败，原因：" + er.Message;
                    FileOpreate.SaveLog("删除失败记录失败，原因：" + er.Message, "主界面按钮", 1);
                }
            }
        }

        //选中行数发生变化时修改设置的行号
        private void dataGridView_Ready_And_Failed_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Choosedindex = e.RowIndex;
            button_UpdateByMan.Enabled = true;
        }

        //查询车辆数据上传状态
        private void button_SeachByCarNum_Click(object sender, EventArgs e)
        {
            try
            {
                //设置选中行dataGridView_Ready_And_Failed.Rows[1].Selected = true;
                if (dataGridView_Ready_And_Failed.Visible == true && dt_ReadyUpload != null)
                {
                    foreach (DataGridViewRow dgvr in dataGridView_Ready_And_Failed.Rows)
                    {
                        dgvr.Selected = false;
                    }
                    DataRow[] temp_dr = null;
                    int a = 0;
                    if (int.TryParse(textBox_CarNum.Text, out a))
                    {
                        temp_dr = dt_ReadyUpload.Select("车辆号牌 like '%" + a + "%'");
                    }
                    else if (textBox_CarNum.Text.Length <= 7 && !System.Text.RegularExpressions.Regex.IsMatch(textBox_CarNum.Text, "[\u4e00-\u9fbb]"))
                    {
                        temp_dr = dt_ReadyUpload.Select("车辆号牌 like '%" + textBox_CarNum.Text + "%'");
                    }
                    else if (textBox_CarNum.Text.Length == 7 || textBox_CarNum.Text.Length == 8)
                    {
                        temp_dr = dt_ReadyUpload.Select("车辆号牌 = '" + textBox_CarNum.Text + "'");
                    }
                    else
                    {
                        MessageBox.Show("输入车牌格式不对，请正确输入车牌号或直接输入英文加数字部分！");
                        return;
                    }
                    if (temp_dr.Count() > 0)
                    {
                        dataGridView_Ready_And_Failed.Rows[dt_ReadyUpload.Rows.IndexOf(temp_dr[0])].Selected = true;
                        dataGridView_Ready_And_Failed.CurrentCell = dataGridView_Ready_And_Failed.Rows[dt_ReadyUpload.Rows.IndexOf(temp_dr[0])].Cells[1];
                    }
                    else
                        MessageBox.Show("待上传或上传失败数据中未找到相应车辆信息");
                }
                else if (dataGridView_AlreadyUpload.Visible == true)
                {
                    foreach (DataGridViewRow dgvr in dataGridView_AlreadyUpload.Rows)
                    {
                        dgvr.Selected = false;
                    }
                    DataRow[] temp_dr = null;
                    int a = 0;
                    if (int.TryParse(textBox_CarNum.Text, out a))
                    {
                        temp_dr = dt_AlreadyUpload.Select("车辆号牌 like '%" + a + "%'");
                    }
                    else if (textBox_CarNum.Text.Length <= 7 && !System.Text.RegularExpressions.Regex.IsMatch(textBox_CarNum.Text, "[\u4e00-\u9fbb]"))
                    {
                        temp_dr = dt_AlreadyUpload.Select("车辆号牌 like '%" + textBox_CarNum.Text + "%'");
                    }
                    else if (textBox_CarNum.Text.Length == 7 || textBox_CarNum.Text.Length == 8)
                    {
                        temp_dr = dt_AlreadyUpload.Select("车辆号牌 = '" + textBox_CarNum.Text + "'");
                    }
                    else
                    {
                        MessageBox.Show("输入车牌格式不对，请正确输入车牌号或直接输入英文加数字部分！");
                        return;
                    }
                    if (temp_dr.Count() > 0)
                    {
                        dataGridView_AlreadyUpload.Rows[dt_AlreadyUpload.Rows.IndexOf(temp_dr[0])].Selected = true;
                        dataGridView_AlreadyUpload.CurrentCell = dataGridView_AlreadyUpload.Rows[dt_AlreadyUpload.Rows.IndexOf(temp_dr[0])].Cells[1];
                    }
                    else
                        MessageBox.Show("已上传数据中未找到相应车辆信息");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "上传记录查询失败");
            }
        }

        private void 上传自检数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable SelfCheckDataTable = carinfobd.getSelfCheckData(maxUploadTimes);
                if (SelfCheckDataTable != null && SelfCheckDataTable.Rows.Count > 0)
                {
                    FileOpreate.SaveLog("手动开始进行自检数据上传，总共有：" + SelfCheckDataTable.Rows.Count + "行数据！", "自检数据上传", 1);
                    for (int i = 0; i < SelfCheckDataTable.Rows.Count; i++)
                    {

                        UpdateZj(SelfCheckDataTable.Rows[i]);
                        /*
                        Thread JZHXSelfCheckUploadThread = new Thread(new ParameterizedThreadStart(UpdateZj));//新建一个标定数据上传线程
                        JZHXSelfCheckUploadThread.Start(SelfCheckDataTable.Rows[i]);//开始上传线程
                        */

                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "自检数据手动上传失败原因");
            }
        }
        private int maxUploadTimes = 3;
        private void 上传标定数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable BDDataTable = carinfobd.getBDData(maxUploadTimes);//获取当天未上传标定数据
                if (BDDataTable != null && BDDataTable.Rows.Count > 0)
                {
                    FileOpreate.SaveLog("手动开始进行标定数据上传，总共有：" + BDDataTable.Rows.Count + "行数据！", "标定数据上传", 1);
                    for (int i = 0; i < BDDataTable.Rows.Count; i++)
                    {
                        //object obj = (object)(BDDataTable.Rows[i]);
                        /*
                        Thread SpeedBDUploadThread = new Thread(new ParameterizedThreadStart(UpdateBD));//新建一个标定数据上传线程
                        SpeedBDUploadThread.Start(BDDataTable.Rows[i]);//开始上传线程*/
                        UpdateBD(BDDataTable.Rows[i]);//不放在进程里面，以免上传时间长重复进入上传进程
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "标定数据手动上传失败原因");
            }
        }
        #endregion
        #region 定时器：获取待检列表定时器、自动上传定时器、界面刷新定时器
        //自动上传定时器
        private void timerBdZjUpdate_Tick(object sender, EventArgs e)
        {
            #region 自检数据上传
            timerUploadBdzjCount++;//
            if (timerUploadBdzjCount > 10)
                timerUploadBdzjCount = 0;
            toolStripProgressBar3.Value = timerUploadBdzjCount;
            //= 0, timerUploadBdzjCount = 0,timerGetWaitListCount = 0;
            DataTable SelfCheckDataTable = carinfobd.getSelfCheckData(maxUploadTimes);

            DataTable BDDataTable = carinfobd.getBDData(maxUploadTimes);//获取当天未上传标定数据
            dt_ZjBdUpload.Clear();
            if (SelfCheckDataTable != null && SelfCheckDataTable.Rows.Count > 0)
            {
                for (int i = 0; i < SelfCheckDataTable.Rows.Count; i++)
                {
                    DataRow dr = dt_ZjBdUpload.NewRow();
                    dr["ID"] = SelfCheckDataTable.Rows[i]["ID"].ToString();
                    dr["内容"] = "自检";
                    dr["线号"] = SelfCheckDataTable.Rows[i]["JCGWH"].ToString();
                    dr["项目"] = Dic_ZJLX.GetValue(SelfCheckDataTable.Rows[i]["ZJLX"].ToString(), "其他");
                    dr["结果"] = SelfCheckDataTable.Rows[i]["ZJJG"].ToString() == "1" ? "合格" : "不合格";
                    dr["状态"] = SelfCheckDataTable.Rows[i]["ZT"].ToString() == "1" ? "已上传" : "未上传";
                    dr["上传次数"] = SelfCheckDataTable.Rows[i]["BY1"].ToString();
                    dr["操作员"] = SelfCheckDataTable.Rows[i]["CZY"].ToString();
                    dt_ZjBdUpload.Rows.Add(dr);
                }

            }
            if (BDDataTable != null && BDDataTable.Rows.Count > 0)
            {
                for (int i = 0; i < BDDataTable.Rows.Count; i++)
                {
                    DataRow dr = dt_ZjBdUpload.NewRow();
                    dr["ID"] = BDDataTable.Rows[i]["ID"].ToString();
                    dr["内容"] = "标定";
                    dr["线号"] = BDDataTable.Rows[i]["JCGWH"].ToString();
                    dr["项目"] = Dic_BDZJ.GetValue(BDDataTable.Rows[i]["LX"].ToString(), "");
                    dr["结果"] = BDDataTable.Rows[i]["BDJG"].ToString();
                    dr["状态"] = BDDataTable.Rows[i]["ZT"].ToString() == "1" ? "已上传" : "未上传";
                    dr["上传次数"] = BDDataTable.Rows[i]["BY1"].ToString();
                    dr["操作员"] = BDDataTable.Rows[i]["CZY"].ToString();
                    dt_ZjBdUpload.Rows.Add(dr);
                }

            }
            dataGridView_BDZJ.DataSource = dt_ZjBdUpload;
            try
            {

                if (SelfCheckDataTable != null && SelfCheckDataTable.Rows.Count > 0)
                {
                    FileOpreate.SaveLog("自动开始进行自检数据上传，总共有：" + SelfCheckDataTable.Rows.Count + "行数据！", "自检数据上传", 1);
                    for (int i = 0; i < SelfCheckDataTable.Rows.Count; i++)
                    {
                        if (SelfCheckDataTable.Rows[i]["ZT"].ToString() == "0" && int.Parse(SelfCheckDataTable.Rows[i]["BY1"].ToString()) < maxUploadTimes)
                            UpdateZj(SelfCheckDataTable.Rows[i]);
                        /*
                        Thread JZHXSelfCheckUploadThread = new Thread(new ParameterizedThreadStart(UpdateZj));//新建一个标定数据上传线程
                        JZHXSelfCheckUploadThread.Start(SelfCheckDataTable.Rows[i]);//开始上传线程
                        */

                    }
                }
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("错误原因：" + er.Message, "设备自检数据上传错误", 1);
            }
            try
            {
                if (BDDataTable != null && BDDataTable.Rows.Count > 0)
                {
                    FileOpreate.SaveLog("手动开始进行标定数据上传，总共有：" + BDDataTable.Rows.Count + "行数据！", "标定数据上传", 1);
                    for (int i = 0; i < BDDataTable.Rows.Count; i++)
                    {
                        //object obj = (object)(BDDataTable.Rows[i]);
                        /*
                        Thread SpeedBDUploadThread = new Thread(new ParameterizedThreadStart(UpdateBD));//新建一个标定数据上传线程
                        SpeedBDUploadThread.Start(BDDataTable.Rows[i]);//开始上传线程*/

                        if (BDDataTable.Rows[i]["ZT"].ToString() == "0" && int.Parse(BDDataTable.Rows[i]["BY1"].ToString()) < maxUploadTimes)
                            UpdateBD(BDDataTable.Rows[i]);//不放在进程里面，以免上传时间长重复进入上传进程
                    }
                }
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("错误原因：" + er.Message, "设备标定数据上传错误", 1);
            }
            #endregion
        }
        public static DataTable dtWaitCarListInNet = new DataTable();
        // 待检列表获取
        private void timerGetWaitCarList_Tick(object sender, EventArgs e)
        {
            #region 连续不间断运行时在05：00-05：30间进行一次定时清理待检列表
            try
            {
                if (_clearcarlistat500 && DateTime.Now.CompareTo(Convert.ToDateTime("05:30")) < 0 && DateTime.Now.CompareTo(Convert.ToDateTime("05:00")) > 0)
                {
                    //在5:00到5:30之间，进行一次清理，并将_clearcarlistat500状态置为fales，保存清理时间
                    carinfobd.clearCarWaitList();
                    carinfobd.clearCarTestStatus();
                    _clearcarlistat500 = false;
                    FileOpreate.SaveClearTime(System.DateTime.Now.ToString("yyyy-MM-dd"));
                }
                if (!_clearcarlistat500 && DateTime.Now.CompareTo(Convert.ToDateTime("05:30")) > 0)
                    _clearcarlistat500 = true;
            }
            catch { }
            #endregion

            #region 获取平台待检列表
            try
            {
                timerGetWaitListCount++;//
                if (timerGetWaitListCount > 10)
                    timerGetWaitListCount = 0;
                toolStripProgressBar2.Value = timerGetWaitListCount;
                //= 0, timerUploadBdzjCount = 0,timerGetWaitListCount = 0;
                string error_info1 = "";
                dtWaitCarListInNet = interfacejiangsu.GetCheckList("", ModPublicJiangsu.unitid);//联网获取待检车辆信息
                if (dtWaitCarListInNet == null)
                {
                    FileOpreate.SaveLog("获取错误,原因：" + error_info1, "获取平台待检列表", 1);
                    return;
                }
                if (dtWaitCarListInNet.Rows.Count > 0)
                {
                    DataTable dtWaitCarList = carinfobd.getCarInWaitList("Y");//获取待检列表中的联网待检车辆信息
                    if (dtWaitCarList == null)
                    {
                        FileOpreate.SaveLog("获取错误,原因：" + error_info1, "获取本地待检列表", 1);
                        return;
                    }
                    if (true)
                    {
                        #region 通用联网
                        //删除本地待检列表在联网待检列表中不存在的项 
                        for (int i = 0; i < dtWaitCarList.Rows.Count; i++)
                        {
                            string check_jylsh = dtWaitCarList.Rows[i]["JYLSH"].ToString();
                            string check_jycs = dtWaitCarList.Rows[i]["JCCS"].ToString();
                            DataRow[] dr_check_alive_waitlist = dtWaitCarListInNet.Select("jylsh='" + check_jylsh + "' and jycs='" + check_jycs + "'");
                            if (dr_check_alive_waitlist == null || dr_check_alive_waitlist.Count() == 0)
                            {
                                //本机待检车辆信息在网上查不到，则删除本地已有待检车辆信息
                                try
                                {
                                    string strDeleteResult = "状态：流水号" + check_jylsh + "，检验次数" + check_jycs;
                                    if (carinfobd.deleteCarInWaitlist(check_jylsh, check_jycs))//删除本地待检列表
                                        strDeleteResult += "本地待检列表删除成功|";
                                    else
                                        strDeleteResult += "本地待检列表删除失败|";
                                    if (carinfobd.DeleteCarTestStatus(check_jylsh, check_jycs))//删除车辆检测状态表
                                        strDeleteResult += "本地车辆检测状态表删除成功|";
                                    else
                                        strDeleteResult += "本地车辆检测状态表删除失败|";
                                    DataRow[] ReadyToDelete = dt_ReadyUpload.Select("检验流水号 = '" + check_jylsh + "' and 检验次数 = '" + check_jycs + "'");
                                    if (ReadyToDelete.Count() > 0)//软件界面有这个检验流水号及检验次数的记录就删除
                                    {
                                        for (int j = 0; j < ReadyToDelete.Count(); j++)
                                        {
                                            dt_ReadyUpload.Rows.Remove(ReadyToDelete[j]);
                                        }
                                        strDeleteResult += "数据上传软件界面记录删除成功";
                                    }
                                    strDeleteResult += "数据上传软件界面无记录";
                                    tSSL_Status.Text = strDeleteResult;
                                    FileOpreate.SaveLog(strDeleteResult, "待检车辆信息删除", 1);
                                }
                                catch (Exception ex)
                                {
                                    FileOpreate.SaveLog("删除NET上不存在待检车辆信息错误" + ex.Message, "待检列表同步错误", 4);
                                }
                            }
                        }
                        //同步待检列表
                        try
                        {
                            for (int i = 0; i < dtWaitCarListInNet.Rows.Count; i++)
                            {
                                string jcff = dtWaitCarListInNet.Rows[i]["jcff"].ToString();
                                //320900001911012043529060
                                string jylsh = dtWaitCarListInNet.Rows[i]["jylsh"].ToString();
                                string jycs = dtWaitCarListInNet.Rows[i]["jycs"].ToString();
                                string hphm = dtWaitCarListInNet.Rows[i]["hphm"].ToString();

                                DataRow[] dr_CheckWaitList = dtWaitCarList.Select("JYLSH = '" + jylsh + "' and JCCS = '" + jycs + "'");//查询待检列表中是否存在该车
                                DataRow[] dr_CheckAlreadUpload = dt_AlreadyUpload.Select("检验流水号 = '" + jylsh + "' and 检验次数 = '" + jycs + "'");//上传已成功表是否存在该车

                                if (dr_CheckWaitList.Count() == 0 && dr_CheckAlreadUpload.Count() == 0)//说明：待检车辆没有该车信息，先更新该车信息、再更新待检列表信息
                                {
                                    //1、获取车辆信
                                    //2、更新车辆信息
                                    Hashtable hstb = new Hashtable();
                                    hstb.Add("CLHP", dtWaitCarListInNet.Rows[i]["hphm"].ToString());
                                    hstb.Add("CPYS", dtWaitCarListInNet.Rows[i]["hpys"].ToString());
                                    hstb.Add("HPZL", dtWaitCarListInNet.Rows[i]["hpzl"].ToString());
                                    hstb.Add("CLLX", dtWaitCarListInNet.Rows[i]["cllx"].ToString());
                                    hstb.Add("XH", dtWaitCarListInNet.Rows[i]["cpxh"].ToString());
                                    hstb.Add("PP", dtWaitCarListInNet.Rows[i]["cpmc"].ToString());
                                    hstb.Add("CLSBM", dtWaitCarListInNet.Rows[i]["clsbm"].ToString());
                                    hstb.Add("SCQY", dtWaitCarListInNet.Rows[i]["clscqy"].ToString());
                                    hstb.Add("FDJXH", dtWaitCarListInNet.Rows[i]["fdjxh"].ToString());
                                    hstb.Add("FDJSCQY", dtWaitCarListInNet.Rows[i]["fdjscqy"].ToString());
                                    hstb.Add("FDJHM", dtWaitCarListInNet.Rows[i]["fdjh"].ToString());
                                    hstb.Add("FDJPL", dtWaitCarListInNet.Rows[i]["fdjpl"].ToString());
                                    hstb.Add("EDZS", dtWaitCarListInNet.Rows[i]["fdjedzs"].ToString());
                                    hstb.Add("EDGL", dtWaitCarListInNet.Rows[i]["fdjedgl"].ToString());
                                    hstb.Add("SYXZ", dtWaitCarListInNet.Rows[i]["syxz"].ToString());
                                    hstb.Add("ZCRQ", DateTime.Parse(dtWaitCarListInNet.Rows[i]["ccdjrq"].ToString()));
                                    hstb.Add("SCRQ", DateTime.Parse(dtWaitCarListInNet.Rows[i]["ccrq"].ToString()));
                                    hstb.Add("CZ", dtWaitCarListInNet.Rows[i]["czmc"].ToString());
                                    hstb.Add("LXDH", dtWaitCarListInNet.Rows[i]["lxdh"].ToString());
                                    hstb.Add("CZDZ", dtWaitCarListInNet.Rows[i]["lxdz"].ToString());
                                    hstb.Add("ZXBZ", dtWaitCarListInNet.Rows[i]["pfbz"].ToString());
                                    hstb.Add("BSQXS", dtWaitCarListInNet.Rows[i]["bsqxs"].ToString());
                                    hstb.Add("JQFS", dtWaitCarListInNet.Rows[i]["jqfs"].ToString());
                                    hstb.Add("RLZL", dtWaitCarListInNet.Rows[i]["ryzl"].ToString());
                                    hstb.Add("GYFS", dtWaitCarListInNet.Rows[i]["gyfs"].ToString());
                                    hstb.Add("QDXS", dtWaitCarListInNet.Rows[i]["qdfs"].ToString());
                                    hstb.Add("ZZL", dtWaitCarListInNet.Rows[i]["zdzzl"].ToString());
                                    hstb.Add("JZZL", dtWaitCarListInNet.Rows[i]["jzzl"].ToString());
                                    hstb.Add("ZBZL", dtWaitCarListInNet.Rows[i]["zbzl"].ToString());
                                    hstb.Add("QGS", dtWaitCarListInNet.Rows[i]["qgs"].ToString());
                                    hstb.Add("OBD", dtWaitCarListInNet.Rows[i]["obd"].ToString());
                                    hstb.Add("QDLTQY", dtWaitCarListInNet.Rows[i]["qdltqy"].ToString());
                                    hstb.Add("JHZZ", dtWaitCarListInNet.Rows[i]["chzhq"].ToString());
                                    hstb.Add("RYPH", dtWaitCarListInNet.Rows[i]["rygg"].ToString());
                                    hstb.Add("HDZK", dtWaitCarListInNet.Rows[i]["sjcys"].ToString());
                                    hstb.Add("SSXQ", dtWaitCarListInNet.Rows[i]["ssxq"].ToString());
                                    hstb.Add("DWS", dtWaitCarListInNet.Rows[i]["dws"].ToString());
                                    hstb.Add("JCLB", dtWaitCarListInNet.Rows[i]["jclx"].ToString());
                                    string Clid = dtWaitCarListInNet.Rows[i]["CLID"].ToString();

                                    //"3600501900023&320900991905031345372908"
                                    Clid = Clid + "&" + jylsh;
                                    //Clid = Clid.Substring(0, Clid.IndexOf("&"));

                                    if (conforsql.InsertOrUpdate("车辆信息", " where CLHP='" + dtWaitCarListInNet.Rows[i]["hphm"].ToString() + "' and HPZL='" + dtWaitCarListInNet.Rows[i]["hpzl"].ToString() + "'", hstb) > 0)//CPYS
                                    {
                                        FileOpreate.SaveLog(hphm + "车辆信息更新成功", "DBWrite1", 1);//3、更新待检列表信息
                                        if (carinfobd.addCarToWaitList(Clid, jylsh, jycs, hphm, dtWaitCarListInNet.Rows[i]["hpys"].ToString(), dtWaitCarListInNet.Rows[i]["hpzl"].ToString(), dtWaitCarListInNet.Rows[i]["ljxslc"].ToString(), jcff, dtWaitCarListInNet.Rows[i]["jczbh"].ToString(), dtWaitCarListInNet.Rows[i]["dlsj"].ToString(), ""))
                                            FileOpreate.SaveLog(hphm + "/" + jcff + "方法待检车辆信息更新成功", "DBWrite2", 1);
                                        else
                                            FileOpreate.SaveLog(hphm + "/" + jcff + "方法待检车辆信息更新失败", "DBError1", 1);
                                    }
                                    else
                                        FileOpreate.SaveLog(hphm + "更新车辆信息表失败，无法加入待检列表", "DBError2", 1);
                                }
                            }
                        }
                        catch (Exception er)
                        {
                            FileOpreate.SaveLog("错误原因：" + er.Message, "更新车辆信息及待检列表信息错误", 1);
                        }
                        #endregion
                    }
                }
                else
                {
                    tSSL_Status.Text = "联网平台无待检车辆！";
                    FileOpreate.SaveLog("联网平台无待检车辆", "待检车辆信息获取", 1);
                }
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog(er.Message, "systemError1", 1);
            }
            #endregion

        }
        int timerUploadTestDataCount = 0, timerUploadBdzjCount = 0, timerGetWaitListCount = 0;
        //数据上传定时器
        private void timerUploadTestData_Tick(object sender, EventArgs e)
        {
            //if (!_whether_Connected)//若同步时间失败则不上传
            //    return;

            try
            {
                timerUploadTestDataCount++;//
                if (timerUploadTestDataCount > 10)
                    timerUploadTestDataCount = 0;
                toolStripProgressBar1.Value = timerUploadTestDataCount;
                //= 0, timerUploadBdzjCount = 0,timerGetWaitListCount = 0;
                //tsl_inf.Text = "上次刷新：" + DateTime.Now.ToString();
                DataTable CarTestStatusTable = carinfobd.getCarTestStatus();//获取车辆检测状态表
                #region 显示 
                dt_ReadyUpload.Clear();
                if (CarTestStatusTable != null && CarTestStatusTable.Rows.Count > 0)
                {
                    for (int i = 0; i < CarTestStatusTable.Rows.Count; i++)
                    {
                        DataRow dr = dt_ReadyUpload.NewRow();
                        dr["检验流水号"] = CarTestStatusTable.Rows[i]["JYLSH"].ToString();
                        string Hphm = CarTestStatusTable.Rows[i]["CLHP"].ToString();
                        dr["车辆号牌"] = CarTestStatusTable.Rows[i]["CLHP"].ToString();
                        dr["号牌种类"] = CarTestStatusTable.Rows[i]["HPZL"].ToString();
                        dr["线号"] = CarTestStatusTable.Rows[i]["LINEID"].ToString();
                        dr["检验次数"] = CarTestStatusTable.Rows[i]["JYCS"].ToString();
                        dr["检验时间"] = CarTestStatusTable.Rows[i]["JCSJ"].ToString();
                        dr["检测方法"] = CarTestStatusTable.Rows[i]["JCFF"].ToString();
                        dr["当前状态"] = JcZt[CarTestStatusTable.Rows[i]["ZT"].ToString()];
                        dr["已处理状态"] = CarTestStatusTable.Rows[i]["YCLZT"].ToString() == "6" ? "上传过程/结果数据" : JcZt[CarTestStatusTable.Rows[i]["YCLZT"].ToString()];
                        dr["备注"] = CarTestStatusTable.Rows[i]["BZ"].ToString();
                        dt_ReadyUpload.Rows.Add(dr);
                    }
                }
                dataGridView_Ready_And_Failed.DataSource = dt_ReadyUpload;
                #endregion
                if (CarTestStatusTable != null && CarTestStatusTable.Rows.Count > 0)
                {
                    //逐行判断车辆检测状态是否变化
                    for (int i = 0; i < CarTestStatusTable.Rows.Count; i++)
                    {
                        //提取数据准备启动数据上传线程
                        DataTable dt_jczt = CarTestStatusTable.Clone();
                        dt_jczt.ImportRow(CarTestStatusTable.Rows[i]);

                        //处理提取数据
                        //dt_jczt.Rows[0]["HPZL"] = InterfaceUpload.get_hpzl_code[dt_jczt.Rows[0]["HPZL"].ToString()];
                        string tempZT = dt_jczt.Rows[0]["ZT"].ToString();
                        string tempYCLZT = dt_jczt.Rows[0]["YCLZT"].ToString();
                        string jylsh = dt_jczt.Rows[0]["JYLSH"].ToString();
                        string jycs = dt_jczt.Rows[0]["JYCS"].ToString();

                        if (tempZT == "3" || tempZT == "5" || ((tempZT != "1") && (tempZT == tempYCLZT)))//3为联网验证未通过，5为检测未完成退出，tempZT、tempYCLZT相等时不执行上传操作
                            continue;
                        try
                        {
                            string A = dt_jczt.Rows[0]["JCFF"].ToString();
                            switch (dt_jczt.Rows[0]["JCFF"].ToString())
                            {

                                case "ASM":
                                    if (tempZT == "1" || tempZT == "21" || tempZT == "22" || tempZT == "23" || tempZT == "24" || tempZT == "6")
                                    {
                                        if (AsmUploadThread != null && AsmUploadThread.IsAlive)
                                            continue;
                                        AsmUploadThread = new Thread(new ParameterizedThreadStart(UploadASMData));//新建一个过程的上传线程
                                        AsmUploadThread.Start(dt_jczt);//开始上传线程
                                        //carinfobd.UpdateCarTestStatusYCLZT(tempZT, jylsh, jycs);//临时更新已处理状态到当前状态
                                        //ShowStatus(jylsh, jycs, "备注", "ASM数据自动上传中...");
                                        FileOpreate.SaveLog("jylsh:" + jylsh + "|ASM数据自动上传线程已开始", "检测数据上传", 1);
                                    }
                                    break;

                                /*
                            case "VMAS":
                                if (tempZT == "1" || tempZT == "31" || tempZT == "32" || tempZT == "33" || tempZT == "6")
                                {
                                    if (VmasUploadThread != null && VmasUploadThread.IsAlive)
                                        continue;
                                    VmasUploadThread = new Thread(new ParameterizedThreadStart(UploadVMASData));
                                    VmasUploadThread.Start(dt_jczt);
                                    //carinfobd.UpdateCarTestStatusYCLZT(tempZT, jylsh, jycs);
                                    //ShowStatus(jylsh, jycs, "备注", "VMAS数据自动上传中...");
                                    FileOpreate.SaveLog("VMAS数据自动上传线程已开始", "检测数据上传", 1);
                                }
                                break;
                                */
                                case "SDS":
                                    if (tempZT == "1" || tempZT == "11" || tempZT == "12" || tempZT == "6")
                                    {
                                        if (SdsUploadThread != null && SdsUploadThread.IsAlive)
                                            continue;
                                        SdsUploadThread = new Thread(new ParameterizedThreadStart(UploadSDSData));
                                        SdsUploadThread.Start(dt_jczt);
                                        //carinfobd.UpdateCarTestStatusYCLZT(tempZT, jylsh, jycs);
                                        // ShowStatus(jylsh, jycs, "备注", "SDS数据自动上传中...");
                                        FileOpreate.SaveLog("jylsh:" + jylsh + "|SDS数据自动上传线程已开始", "检测数据上传", 1);
                                    }
                                    break;
                                case "SDSM":
                                    if (tempZT == "1" || tempZT == "81" || tempZT == "82" || tempZT == "6")
                                    {
                                        if (SdsMUploadThread != null && SdsMUploadThread.IsAlive)
                                            continue;
                                        SdsMUploadThread = new Thread(new ParameterizedThreadStart(UploadSDSMData));
                                        SdsMUploadThread.Start(dt_jczt);
                                        ////carinfobd.UpdateCarTestStatusYCLZT(tempZT, jylsh, jycs);
                                        // ShowStatus(jylsh, jycs, "备注", "SDSM数据自动上传中...");
                                        FileOpreate.SaveLog("SDSM数据自动上传线程已开始", "检测数据上传", 1);
                                    }
                                    break;
                                case "JZJS":
                                    if (tempZT == "1" || tempZT == "41" || tempZT == "42" || tempZT == "43" || tempZT == "6")
                                    {
                                        if (JzjsUploadThread != null && JzjsUploadThread.IsAlive)
                                            continue;
                                        JzjsUploadThread = new Thread(new ParameterizedThreadStart(UploadJZJSData));
                                        JzjsUploadThread.Start(dt_jczt);
                                        //carinfobd.UpdateCarTestStatusYCLZT(tempZT, jylsh, jycs);
                                        ShowStatus(jylsh, jycs, "备注", "JZJS数据自动上传中...");
                                        FileOpreate.SaveLog("JZJS数据自动上传线程已开始", "检测数据上传", 1);
                                    }
                                    break;
                                case "ZYJS":
                                    if (tempZT == "1" || tempZT == "51" || tempZT == "52" || tempZT == "53" || tempZT == "6")
                                    {
                                        if (ZyjsUploadThread != null && ZyjsUploadThread.IsAlive)
                                            continue;
                                        ZyjsUploadThread = new Thread(new ParameterizedThreadStart(UploadZYJSData));
                                        ZyjsUploadThread.Start(dt_jczt);
                                        // carinfobd.UpdateCarTestStatusYCLZT(tempZT, jylsh, jycs);
                                        // ShowStatus(jylsh, jycs, "备注", "ZYJS数据自动上传中...");
                                        FileOpreate.SaveLog("ZYJS数据自动上传线程已开始", "检测数据上传", 1);
                                    }
                                    break;
                            }
                        }
                        catch (Exception er)
                        {
                            FileOpreate.SaveLog("开启线程失败，原因：" + er.Message, "检测数据上传", 1);
                        }
                    }
                    CarTestStatusTable.Clear();
                }
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("失败原因：" + er.Message, "自动上传错误", 1);
            }
        }
        Thread AsmUploadThread = null;
        Thread VmasUploadThread = null;
        Thread SdsUploadThread = null;
        Thread SdsMUploadThread = null;
        Thread JzjsUploadThread = null;
        Thread ZyjsUploadThread = null;

        #endregion

        #region 数据上报线程，当ZT与YCLZT不等时执行一次上传
        /// <summary>
        /// 稳态
        /// </summary>
        /// <param name="obj"></param>
        private void UploadASMData(object obj)
        {
            try
            {
                DataTable dt_test_status = ((DataTable)obj).Copy();

                if (dt_test_status.Rows.Count <= 0)
                {
                    //carinfobd.UpdateCarTestStatusYCLZT(dr_temp["YCLZT"].ToString(), dr_temp["JYLSH"].ToString(), dr_temp["JYCS"].ToString());
                    FileOpreate.SaveLog("ASM上传线程传入数据为空" + obj.ToString(), "ASM上传线程执行失败", 1);
                    return;
                }
                string jylsh = dt_test_status.Rows[0]["JYLSH"].ToString();
                string jycs = dt_test_status.Rows[0]["JYCS"].ToString();
                string zt = dt_test_status.Rows[0]["ZT"].ToString();
                string yclzt = dt_test_status.Rows[0]["YCLZT"].ToString();
                DataRow waitrow= FrmMain.dtWaitCarListInNet.Select("jylsh='" + jylsh + "' and jycs='" + jycs + "'").FirstOrDefault();
                if(waitrow==null)
                {
                    FileOpreate.SaveLog("未找到该条记录的待检记录，不能继续上传", "ASM上传线程执行失败", 1);
                    return;
                }
                string realjylsh = waitrow["reallsh"].ToString();
                //DataRow obj_car = null;
                //carinfobd.getVehicleData("已检车辆信息", jylsh, jycs, out obj_car);
                try
                {
                    FileOpreate.SaveLog("流水号：" + jylsh + "数据上传准备", "ASM上传过程", 1);
                    if (zt == "1")
                    {
                        //向平台发送开始信息
                        //interfacejiangsu. BeginCheck(obj_car);
                        //发请求开始——KS001，解析开始是否同意，更新检测状态，并更新YCLZT到1
                        string error_info = "";
                        if (UpdateProjectStart(dt_test_status.Rows[0],realjylsh, out error_info))
                        {
                            if (carinfobd.UpdateCarTestStatusZT("2", jylsh, jycs, error_info))
                            {
                                //yclzt = "2";
                                carinfobd.UpdateCarTestStatusYCLZT("2", jylsh, jycs);//将该条记录置为上传失败记录
                                FileOpreate.SaveLog("流水号：" + jylsh + "项目开始发送成功", "ASM上传过程", 1);
                            }
                            else
                                FileOpreate.SaveLog("流水号：" + jylsh + "更新允许开始状态失败", "ASM上传过程", 1);
                        }
                        else
                        {
                            carinfobd.UpdateCarTestStatusZT("3", jylsh, jycs, error_info);
                            carinfobd.UpdateCarTestStatusYCLZT("0", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "不允许开始，原因：" + error_info, "ASM上传过程", 1);
                            return;
                        }
                    }
                    if ((zt == "21" || zt == "22" || zt == "23" || zt == "24" || zt == "6") && yclzt == "2")
                    {
                        //发21照片，更新YCLZT到21
                        if (UpdateCapturePicture(dt_test_status.Rows[0], "21"))
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("21", jylsh, jycs);
                            //yclzt = "21";
                            FileOpreate.SaveLog("流水号：" + jylsh + "21发送成功", "ASM上传过程", 1);
                        }
                        else
                            FileOpreate.SaveLog("流水号：" + jylsh + "21发送失败", "ASM上传过程", 1);
                    }
                    if ((zt == "22" || zt == "23" || zt == "24" || zt == "6") && yclzt == "21")
                    {
                        //发22，更新YCLZT到22
                        if (UpdateCapturePicture(dt_test_status.Rows[0], "22"))
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("22", jylsh, jycs);
                            //yclzt = "22";
                            FileOpreate.SaveLog("流水号：" + jylsh + "22发送成功", "ASM上传过程", 1);
                        }
                        else
                            FileOpreate.SaveLog("流水号：" + jylsh + "22发送失败", "ASM上传过程", 1);
                    }
                    if ((zt == "23" || zt == "24" || zt == "6") && yclzt == "22")
                    {
                        //发23，更新YCLZT到23
                        if (UpdateCapturePicture(dt_test_status.Rows[0], "23"))
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("23", jylsh, jycs);
                            //yclzt = "23";
                            FileOpreate.SaveLog("流水号：" + jylsh + "23发送成功", "ASM上传过程", 1);
                        }
                        else
                            FileOpreate.SaveLog("流水号：" + jylsh + "23发送失败", "ASM上传过程", 1);
                    }
                    if ((zt == "24" || zt == "6") && yclzt == "23")
                    {
                        //发24，更新YCLZT到24
                        if (UpdateCapturePicture(dt_test_status.Rows[0], "24"))
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("24", jylsh, jycs);
                            //yclzt = "24";
                            FileOpreate.SaveLog("流水号：" + jylsh + "24发送成功", "ASM上传过程", 1);
                        }
                        else
                            FileOpreate.SaveLog("流水号：" + jylsh + "24发送失败", "ASM上传过程", 1);
                    }
                    if (zt == "6" && yclzt == "24")
                    {
                        string test_result = "";
                        //上传外检
                        if(!UpdateResult("outlook", jylsh, jycs,realjylsh, out test_result))
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs, test_result);
                            FileOpreate.SaveLog("流水号：" + jylsh + "外检数据上传失败", "ASM上传过程", 1);
                        }
                        //上传结果
                        if (UpdateResult("ASM", jylsh, jycs,realjylsh, out test_result))
                        {
                            FileOpreate.SaveLog("流水号：" + jylsh + "结果数据发送成功", "ASM上传过程", 1);
                            if (UpdateProjectStop(dt_test_status.Rows[0], test_result))
                            {
                                UpdateStatus(jylsh, jycs, "已处理状态", "上传完毕");
                                FileOpreate.SaveLog("流水号：" + jylsh + "项目结束指令发送成功，所有数据上传完毕", "ASM上传过程", 1);
                                //将该条数据添加到已上传数据中，删除车辆检测状态表中该条数据，记录上传成功日志
                                DataRow dr = dt_AlreadyUpload.NewRow();
                                dr["检验流水号"] = dt_test_status.Rows[0]["JYLSH"].ToString();
                                dr["车辆号牌"] = dt_test_status.Rows[0]["CLHP"].ToString();
                                dr["号牌种类"] = dt_test_status.Rows[0]["HPZL"].ToString();
                                dr["线号"] = dt_test_status.Rows[0]["LINEID"].ToString();
                                dr["检验次数"] = dt_test_status.Rows[0]["JYCS"].ToString();
                                dr["检验时间"] = dt_test_status.Rows[0]["JCSJ"].ToString();
                                dr["检测方法"] = dt_test_status.Rows[0]["JCFF"].ToString();
                                dr["上传成功时间"] = System.DateTime.Now.ToString();
                                dt_AlreadyUpload.Rows.Add(dr);
                                carinfobd.DeleteCarTestStatus(jylsh, jycs);
                                carinfobd.deleteCarInWaitlist(jylsh, jycs);
                            }
                            else
                            {
                                carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs, test_result);
                                FileOpreate.SaveLog("流水号：" + jylsh + "检测结束上传失败", "ASM上传过程", 1);
                            }
                        }
                        else
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs, test_result);
                            FileOpreate.SaveLog("流水号：" + jylsh + "上传失败", "ASM上传过程", 1);
                        }

                    }

                    UpdateStatus(jylsh, jycs, "备注", "当前数据上传完");
                }
                catch (Exception er)
                {
                    //更新YCLZT状态到实际执行到的位置
                    carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs, er.Message);
                    FileOpreate.SaveLog("数据上传错误原因：" + er.Message, "ASM上传错误", 1);
                }
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("线程运行错误原因：" + er.Message, "ASM上传线程错误", 1);
            }
        }

        /*瞬态工况
        private void UploadVMASData(object obj)
        {
            try
            {
                DataTable dt_test_status = ((DataTable)obj).Copy();

                if (dt_test_status.Rows.Count <= 0)
                {
                    //carinfobd.UpdateCarTestStatusYCLZT(dr_temp["YCLZT"].ToString(), dr_temp["JYLSH"].ToString(), dr_temp["JYCS"].ToString());
                    FileOpreate.SaveLog("VMAS上传线程传入数据为空" + obj.ToString(), "VAMS上传线程执行失败", 1);
                    return;
                }
                string jylsh = dt_test_status.Rows[0]["JYLSH"].ToString();
                string jycs = dt_test_status.Rows[0]["JYCS"].ToString();
                string zt = dt_test_status.Rows[0]["ZT"].ToString();
                string yclzt = dt_test_status.Rows[0]["YCLZT"].ToString();

                try
                {
                    FileOpreate.SaveLog("流水号：" + jylsh + "VMAS数据上传准备", "VMAS上传过程", 1);
                    if (zt == "1")
                    {
                        //发请求开始——KS001，解析开始是否同意，更新检测状态，并更新YCLZT到1
                        string error_info = "";
                        if (UpdateProjectStart(dt_test_status.Rows[0], out error_info))
                        {
                            if (carinfobd.UpdateCarTestStatusZT("2", jylsh, jycs, error_info))
                            {
                                carinfobd.UpdateCarTestStatusYCLZT("2", jylsh, jycs);
                                FileOpreate.SaveLog("流水号：" + jylsh + "项目开始发送成功", "VMAS上传过程", 1);
                            }
                            else
                                FileOpreate.SaveLog("流水号：" + jylsh + "更新允许开始状态失败", "VMAS上传过程", 1);
                        }
                        else
                        {
                            carinfobd.UpdateCarTestStatusZT("3", jylsh, jycs, error_info);
                            carinfobd.UpdateCarTestStatusYCLZT("0", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "不允许开始，原因：" + error_info, "VMAS上传过程", 1);
                            return;
                        }
                    }
                    if ((zt == "31" || zt == "32" || zt == "33" || zt == "6") && yclzt == "2")
                    {
                        //发31照片，更新YCLZT到31
                        if (UpdateCapturePicture(dt_test_status.Rows[0], "31"))
                        {
                           // yclzt = "31";
                            carinfobd.UpdateCarTestStatusYCLZT("31", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "31发送成功", "VMAS上传过程", 1);
                        }
                        else
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs);//将该条记录置为上传失败记录
                            FileOpreate.SaveLog("流水号：" + jylsh + "31发送失败", "VMAS上传过程", 1);
                        }
                    }
                    if ((zt == "32" || zt == "33" || zt == "6") && yclzt == "31")
                    {
                        //发32
                        if (UpdateCapturePicture(dt_test_status.Rows[0], "32"))
                        {
                            //yclzt = "32";
                            carinfobd.UpdateCarTestStatusYCLZT("32", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "32发送成功", "VMAS上传过程", 1);
                        }
                        else
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs);//将该条记录置为上传失败记录
                            FileOpreate.SaveLog("流水号：" + jylsh + "32发送失败", "VMAS上传过程", 1);
                        }
                        //更新YCLZT到32
                    }
                    if ((zt == "33" || zt == "6") && yclzt == "32")
                    {
                        //发33
                        if (UpdateCapturePicture(dt_test_status.Rows[0], "33"))
                        {
                            //yclzt = "33";
                            carinfobd.UpdateCarTestStatusYCLZT("33", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "33发送成功", "VMAS上传过程", 1);
                        }
                        else
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs);//将该条记录置为上传失败记录
                            FileOpreate.SaveLog("流水号：" + jylsh + "33发送失败", "VMAS上传过程", 1);
                        }
                        //更新YCLZT到33
                    }
                    if (zt == "6" && yclzt == "33")
                    {
                        string test_result = "";
                        if (UpdateResult("VMAS",jylsh, jycs, out test_result))
                        {
                            FileOpreate.SaveLog("流水号：" + jylsh + "结果数据发送成功", "VMAS上传过程", 1);
                            if (UpdateProjectStop(dt_test_status.Rows[0], test_result))
                            {
                                UpdateStatus(jylsh, jycs, "已处理状态", "上传完毕");
                                FileOpreate.SaveLog("流水号：" + jylsh + "项目结束发送成功，所有数据上传完毕", "VMAS上传过程", 1);
                                //更新其他状态
                                DataRow dr = dt_AlreadyUpload.NewRow();
                                dr["检验流水号"] = dt_test_status.Rows[0]["JYLSH"].ToString();
                                dr["车辆号牌"] = dt_test_status.Rows[0]["CLHP"].ToString();
                                dr["号牌种类"] = dt_test_status.Rows[0]["HPZL"].ToString();
                                dr["线号"] = dt_test_status.Rows[0]["LINEID"].ToString();
                                dr["检验次数"] = dt_test_status.Rows[0]["JYCS"].ToString();
                                dr["检验时间"] = dt_test_status.Rows[0]["JCSJ"].ToString();
                                dr["检测方法"] = dt_test_status.Rows[0]["JCFF"].ToString();
                                dr["上传成功时间"] = System.DateTime.Now.ToString();
                                dt_AlreadyUpload.Rows.Add(dr);
                                carinfobd.DeleteCarTestStatus(jylsh, jycs);
                                carinfobd.deleteCarInWaitlist(jylsh, jycs);
                            }
                            else
                            {
                                carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs, test_result);//将该条记录置为上传失败记录
                                FileOpreate.SaveLog("流水号：" + jylsh + "检测结束上传失败", "VMAS上传过程", 1);
                            }
                        }
                        else
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs,test_result);//将该条记录置为上传失败记录
                            FileOpreate.SaveLog("流水号：" + jylsh + "上传失败", "VMAS上传过程", 1);
                        }
                    }

                    //更新YCLZT状态到实际执行到的位置
                    UpdateStatus(jylsh, jycs, "备注", "当前数据上传完");
                }
                catch (Exception er)
                {
                    //更新YCLZT状态到实际执行到的位置
                    carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs,er.Message);
                    FileOpreate.SaveLog("数据上传错误原因：" + er.Message, "Vmas上传错误", 1);
                }
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("线程运行错误原因：" + er.Message, "Vmas上传线程错误", 1);
            }
        }*/

        /// <summary>
        /// 双怠速
        /// </summary>
        /// <param name="obj"></param>
        private void UploadSDSData(object obj)
        {
            try
            {
                DataTable dt_test_status = ((DataTable)obj).Copy();

                if (dt_test_status.Rows.Count <= 0)
                {
                    //carinfobd.UpdateCarTestStatusYCLZT(dr_temp["YCLZT"].ToString(), dr_temp["JYLSH"].ToString(), dr_temp["JYCS"].ToString());
                    FileOpreate.SaveLog("SDS上传线程传入数据为空" + obj.ToString(), "SDS上传线程执行失败", 1);
                    return;
                }
                string jylsh = dt_test_status.Rows[0]["JYLSH"].ToString();
                string jycs = dt_test_status.Rows[0]["JYCS"].ToString();
                string zt = dt_test_status.Rows[0]["ZT"].ToString();
                string yclzt = dt_test_status.Rows[0]["YCLZT"].ToString();
                DataRow waitrow = FrmMain.dtWaitCarListInNet.Select("jylsh='" + jylsh + "' and jycs='" + jycs + "'").FirstOrDefault();
                if (waitrow == null)
                {
                    FileOpreate.SaveLog("未找到该条记录的待检记录，不能继续上传", "ASM上传线程执行失败", 1);
                    return;
                }
                string realjylsh = waitrow["reallsh"].ToString();
                //DataRow obj_car = null;
                //carinfobd.getVehicleData("已检车辆信息", jylsh, jycs, out obj_car);

                try
                {
                    FileOpreate.SaveLog("流水号：" + jylsh + "SDS数据上传准备", "SDS上传过程", 1);
                    if (zt == "1")
                    {
                        //向平台发送开始信息
                        //interfacejiangsu.BeginCheck(obj_car);
                        //发请求开始——KS001，解析开始是否同意，更新检测状态，并更新YCLZT到1
                        string error_info = "";
                        if (UpdateProjectStart(dt_test_status.Rows[0],realjylsh, out error_info))
                        {
                            if (carinfobd.UpdateCarTestStatusZT("2", jylsh, jycs, error_info))
                            {
                                carinfobd.UpdateCarTestStatusYCLZT("2", jylsh, jycs);
                                FileOpreate.SaveLog("流水号：" + jylsh + "项目开始发送成功", "SDS上传过程", 1);
                            }
                            else
                                FileOpreate.SaveLog("流水号：" + jylsh + "更新允许开始状态失败", "SDS上传过程", 1);
                        }
                        else
                        {
                            carinfobd.UpdateCarTestStatusZT("3", jylsh, jycs, error_info);
                            carinfobd.UpdateCarTestStatusYCLZT("0", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "不允许开始，原因：" + error_info, "SDS上传过程", 1);
                            return;
                        }
                    }
                    if ((zt == "11" || zt == "14" || zt == "12" || zt == "13" || zt == "6") && yclzt == "2")
                    {
                        //发11照片
                        if (UpdateCapturePicture(dt_test_status.Rows[0], "11"))
                        {
                            //yclzt = "11";
                            carinfobd.UpdateCarTestStatusYCLZT("11", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "11发送成功", "SDS上传过程", 1);
                        }
                        else
                            FileOpreate.SaveLog("流水号：" + jylsh + "11发送失败", "SDS上传过程", 1);
                        //更新YCLZT到11
                    }
                    if ((zt == "14" || zt == "12" || zt == "13" || zt == "6") && yclzt == "11")
                    {
                        //发11照片
                        if (UpdateCapturePicture(dt_test_status.Rows[0], "14"))
                        {
                            //yclzt = "11";
                            carinfobd.UpdateCarTestStatusYCLZT("14", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "11发送成功", "SDS上传过程", 1);
                        }
                        else
                            FileOpreate.SaveLog("流水号：" + jylsh + "11发送失败", "SDS上传过程", 1);
                        //更新YCLZT到11
                    }
                    if ((zt == "12" || zt == "13" || zt == "6") && yclzt == "14")
                    {
                        //发11照片
                        if (UpdateCapturePicture(dt_test_status.Rows[0], "12"))
                        {
                            //yclzt = "11";
                            carinfobd.UpdateCarTestStatusYCLZT("12", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "11发送成功", "SDS上传过程", 1);
                        }
                        else
                            FileOpreate.SaveLog("流水号：" + jylsh + "11发送失败", "SDS上传过程", 1);
                        //更新YCLZT到11
                    }
                    if ((zt == "13" || zt == "6") && yclzt == "12")
                    {
                        //发11照片
                        if (UpdateCapturePicture(dt_test_status.Rows[0], "13"))
                        {
                            //yclzt = "11";
                            carinfobd.UpdateCarTestStatusYCLZT("13", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "11发送成功", "SDS上传过程", 1);
                        }
                        else
                            FileOpreate.SaveLog("流水号：" + jylsh + "11发送失败", "SDS上传过程", 1);
                        //更新YCLZT到11
                    }
                    if (zt == "6" && yclzt == "13")
                    {
                        string test_result = "";
                        //上传外检
                        //上传外检
                        if (!UpdateResult("outlook", jylsh, jycs, realjylsh, out test_result))
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs, test_result);
                            FileOpreate.SaveLog("流水号：" + jylsh + "外检数据上传失败", "ASM上传过程", 1);
                        }
                        if (UpdateResult("SDS", jylsh, jycs,realjylsh, out test_result))
                        {
                            FileOpreate.SaveLog("流水号：" + jylsh + "结果数据发送成功", "SDS上传过程", 1);
                            if (UpdateProjectStop(dt_test_status.Rows[0], test_result))
                            {
                                UpdateStatus(jylsh, jycs, "已处理状态", "上传完毕");
                                FileOpreate.SaveLog("流水号：" + jylsh + "项目结束命令发送成功", "SDS上传过程", 1);
                                //更新其他状态
                                DataRow dr = dt_AlreadyUpload.NewRow();
                                dr["检验流水号"] = dt_test_status.Rows[0]["JYLSH"].ToString();
                                dr["车辆号牌"] = dt_test_status.Rows[0]["CLHP"].ToString();
                                dr["号牌种类"] = dt_test_status.Rows[0]["HPZL"].ToString();
                                dr["线号"] = dt_test_status.Rows[0]["LINEID"].ToString();
                                dr["检验次数"] = dt_test_status.Rows[0]["JYCS"].ToString();
                                dr["检验时间"] = dt_test_status.Rows[0]["JCSJ"].ToString();
                                dr["检测方法"] = dt_test_status.Rows[0]["JCFF"].ToString();
                                dr["上传成功时间"] = System.DateTime.Now.ToString();
                                dt_AlreadyUpload.Rows.Add(dr);
                                FileOpreate.SaveLog("流水号：" + jylsh + "所有数据上传完毕", "SDS上传过程", 1);
                                carinfobd.DeleteCarTestStatus(jylsh, jycs);
                                carinfobd.deleteCarInWaitlist(jylsh, jycs);
                            }
                            else
                            {
                                carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs, test_result);//将该条记录置为上传失败记录
                                FileOpreate.SaveLog("流水号：" + jylsh + "检测结束上传失败", "SDS上传过程", 1);
                            }

                        }
                        else
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs, test_result);//将该条记录置为上传失败记录
                            FileOpreate.SaveLog("流水号：" + jylsh + "上传失败", "SDS上传过程", 1);
                        }
                        //更新YCLZT到6
                    }

                    //更新YCLZT状态到实际执行到的位置
                    //carinfobd.UpdateCarTestStatusYCLZT(yclzt, jylsh, jycs);
                    UpdateStatus(jylsh, jycs, "备注", "当前数据上传完");
                }
                catch (Exception er)
                {
                    //更新YCLZT状态到实际执行到的位置
                    carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs, er.Message);
                    FileOpreate.SaveLog("数据上传错误原因：" + er.Message, "SDS上传错误", 1);
                }
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("线程运行错误原因：" + er.Message, "SDS上传线程错误", 1);
            }
        }

        private void UploadSDSMData(object obj)
        {
            try
            {
                DataTable dt_test_status = ((DataTable)obj).Copy();

                if (dt_test_status.Rows.Count <= 0)
                {
                    //carinfobd.UpdateCarTestStatusYCLZT(dr_temp["YCLZT"].ToString(), dr_temp["JYLSH"].ToString(), dr_temp["JYCS"].ToString());
                    FileOpreate.SaveLog("SDSM上传线程传入数据为空" + obj.ToString(), "SDSM上传线程执行失败", 1);
                    return;
                }
                string jylsh = dt_test_status.Rows[0]["JYLSH"].ToString();
                string jycs = dt_test_status.Rows[0]["JYCS"].ToString();
                string zt = dt_test_status.Rows[0]["ZT"].ToString();
                string yclzt = dt_test_status.Rows[0]["YCLZT"].ToString();
                DataRow waitrow = FrmMain.dtWaitCarListInNet.Select("jylsh='" + jylsh + "' and jycs='" + jycs + "'").FirstOrDefault();
                if (waitrow == null)
                {
                    FileOpreate.SaveLog("未找到该条记录的待检记录，不能继续上传", "ASM上传线程执行失败", 1);
                    return;
                }
                string realjylsh = waitrow["reallsh"].ToString();
                //DataRow obj_car = null;
                //carinfobd.getVehicleData("已检车辆信息", jylsh, jycs, out obj_car);
                try
                {
                    FileOpreate.SaveLog("流水号：" + jylsh + "SDSM数据上传准备", "SDSM上传过程", 1);
                    if (zt == "1")
                    {
                        //向平台发送开始信息
                        //interfacejiangsu.BeginCheck(obj_car);
                        //发请求开始——KS001，解析开始是否同意，更新检测状态，并更新YCLZT到1
                        string error_info = "";
                        if (UpdateProjectStart(dt_test_status.Rows[0],realjylsh, out error_info))
                        {
                            if (carinfobd.UpdateCarTestStatusZT("2", jylsh, jycs, error_info))
                            {
                                yclzt = "2";
                                FileOpreate.SaveLog("流水号：" + jylsh + "项目开始发送成功", "SDS上传过程", 1);
                            }
                            else
                                FileOpreate.SaveLog("流水号：" + jylsh + "更新允许开始状态失败", "SDS上传过程", 1);
                        }
                        else
                        {
                            carinfobd.UpdateCarTestStatusZT("3", jylsh, jycs, error_info);
                            carinfobd.UpdateCarTestStatusYCLZT("0", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "不允许开始，原因：" + error_info, "SDS上传过程", 1);
                            return;
                        }
                    }
                    if ((zt == "81" || zt == "82" || zt == "6") && yclzt == "2")
                    {
                        //发11照片
                        if (UpdateCapturePicture(dt_test_status.Rows[0], "81"))
                        {
                            //yclzt = "81";
                            carinfobd.UpdateCarTestStatusYCLZT("81", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "81发送成功", "SDSM上传过程", 1);
                        }
                        else
                            FileOpreate.SaveLog("流水号：" + jylsh + "81发送失败", "SDSM上传过程", 1);
                        //更新YCLZT到81
                    }
                    if ((zt == "82" || zt == "6") && yclzt == "81")
                    {
                        //发82
                        if (UpdateCapturePicture(dt_test_status.Rows[0], "82"))
                        {
                            //yclzt = "82";
                            carinfobd.UpdateCarTestStatusYCLZT("82", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "82发送成功", "SDSM上传过程", 1);
                        }
                        else
                            FileOpreate.SaveLog("流水号：" + jylsh + "82发送失败", "SDSM上传过程", 1);
                        //更新YCLZT到82
                    }
                    if (zt == "6" && yclzt == "82")
                    {
                        string test_result = "";
                        //上传外检
                        //上传外检
                        if (!UpdateResult("outlook", jylsh, jycs, realjylsh, out test_result))
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs, test_result);
                            FileOpreate.SaveLog("流水号：" + jylsh + "外检数据上传失败", "ASM上传过程", 1);
                        }

                        if (UpdateResult("SDS", jylsh, jycs,realjylsh, out test_result))
                        {
                            FileOpreate.SaveLog("流水号：" + jylsh + "结果数据发送成功", "SDSM上传过程", 1);
                            if (UpdateProjectStop(dt_test_status.Rows[0], test_result))
                            {
                                UpdateStatus(jylsh, jycs, "已处理状态", "上传完毕");
                                FileOpreate.SaveLog("流水号：" + jylsh + "项目结束命令发送成功", "SDSM上传过程", 1);
                                //更新其他状态
                                DataRow dr = dt_AlreadyUpload.NewRow();
                                dr["检验流水号"] = dt_test_status.Rows[0]["JYLSH"].ToString();
                                dr["车辆号牌"] = dt_test_status.Rows[0]["CLHP"].ToString();
                                dr["号牌种类"] = dt_test_status.Rows[0]["HPZL"].ToString();
                                dr["线号"] = dt_test_status.Rows[0]["LINEID"].ToString();
                                dr["检验次数"] = dt_test_status.Rows[0]["JYCS"].ToString();
                                dr["检验时间"] = dt_test_status.Rows[0]["JCSJ"].ToString();
                                dr["检测方法"] = dt_test_status.Rows[0]["JCFF"].ToString();
                                dr["上传成功时间"] = System.DateTime.Now.ToString();
                                dt_AlreadyUpload.Rows.Add(dr);
                                FileOpreate.SaveLog("流水号：" + jylsh + "所有数据上传完毕", "SDSM上传过程", 1);
                                carinfobd.DeleteCarTestStatus(jylsh, jycs);
                                carinfobd.deleteCarInWaitlist(jylsh, jycs);
                            }
                            else
                            {
                                carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs, test_result);//将该条记录置为上传失败记录
                                FileOpreate.SaveLog("流水号：" + jylsh + "检测结束上传失败", "SDSM上传过程", 1);
                            }
                        }
                        else
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs, test_result);//将该条记录置为上传失败记录
                            FileOpreate.SaveLog("流水号：" + jylsh + "上传失败", "SDSM上传过程", 1);
                        }
                        //更新YCLZT到6
                    }

                    //更新YCLZT状态到实际执行到的位置
                    //carinfobd.UpdateCarTestStatusYCLZT(yclzt, jylsh, jycs);
                    UpdateStatus(jylsh, jycs, "备注", "当前数据上传完");
                }
                catch (Exception er)
                {
                    //更新YCLZT状态到实际执行到的位置
                    carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs, er.Message);
                    FileOpreate.SaveLog("数据上传错误原因：" + er.Message, "SDS上传错误", 1);
                }
            }
            catch (Exception er)
            {
                //carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs, er.Message);
                FileOpreate.SaveLog("线程运行错误原因：" + er.Message, "ASM上传线程错误", 1);
            }
        }

        /// <summary>
        /// 加载减速
        /// </summary>
        /// <param name="obj"></param>
        private void UploadJZJSData(object obj)
        {
            try
            {
                DataTable dt_test_status = ((DataTable)obj).Copy();

                if (dt_test_status.Rows.Count <= 0)
                {
                    //carinfobd.UpdateCarTestStatusYCLZT(dr_temp["YCLZT"].ToString(), dr_temp["JYLSH"].ToString(), dr_temp["JYCS"].ToString());
                    FileOpreate.SaveLog("JZJS上传线程传入数据为空" + obj.ToString(), "JZJS上传线程执行失败", 1);
                    return;
                }
                string jylsh = dt_test_status.Rows[0]["JYLSH"].ToString();
                string jycs = dt_test_status.Rows[0]["JYCS"].ToString();
                string zt = dt_test_status.Rows[0]["ZT"].ToString();
                string yclzt = dt_test_status.Rows[0]["YCLZT"].ToString();
                DataRow waitrow = FrmMain.dtWaitCarListInNet.Select("jylsh='" + jylsh + "' and jycs='" + jycs + "'").FirstOrDefault();
                if (waitrow == null)
                {
                    FileOpreate.SaveLog("未找到该条记录的待检记录，不能继续上传", "ASM上传线程执行失败", 1);
                    return;
                }
                string realjylsh = waitrow["reallsh"].ToString();
                //DataRow obj_car = null;
                //carinfobd.getVehicleData("已检车辆信息", jylsh, jycs, out obj_car);
                try
                {
                    FileOpreate.SaveLog("流水号：" + jylsh + "JZJS数据上传准备", "JZJS上传过程", 1);
                    if (zt == "1")
                    {
                        //向平台发送开始信息
                        //interfacejiangsu.BeginCheck(obj_car);
                        //发请求开始——KS001，解析开始是否同意，更新检测状态，并更新YCLZT到1
                        string error_info = "";
                        if (UpdateProjectStart(dt_test_status.Rows[0],realjylsh, out error_info))
                        {
                            if (carinfobd.UpdateCarTestStatusZT("2", jylsh, jycs, error_info))
                            {
                                //yclzt = "2";
                                carinfobd.UpdateCarTestStatusYCLZT("2", jylsh, jycs);
                                FileOpreate.SaveLog("流水号：" + jylsh + "项目开始发送成功", "JZJS上传过程", 1);
                            }
                            else
                                FileOpreate.SaveLog("流水号：" + jylsh + "更新允许开始状态失败", "JZJS上传过程", 1);
                        }
                        else
                        {
                            carinfobd.UpdateCarTestStatusZT("3", jylsh, jycs, error_info);
                            carinfobd.UpdateCarTestStatusYCLZT("0", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "不允许开始，原因：" + error_info, "JZJS上传过程", 1);
                            return;
                        }
                    }
                    if ((zt == "41" || zt == "42" || zt == "43" || zt == "44" || zt == "6") && yclzt == "2")
                    {
                        //发41照片
                        if (UpdateCapturePicture(dt_test_status.Rows[0], "41"))
                        {
                            //yclzt = "41";
                            carinfobd.UpdateCarTestStatusYCLZT("41", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "41发送成功", "JZJS上传过程", 1);
                        }
                        else
                            FileOpreate.SaveLog("流水号：" + jylsh + "41发送失败", "JZJS上传过程", 1);
                        //更新YCLZT到41
                    }
                    if ((zt == "42" || zt == "43" || zt == "44" || zt == "6") && yclzt == "41")
                    {
                        //发42
                        if (UpdateCapturePicture(dt_test_status.Rows[0], "42"))
                        {
                            //yclzt = "42";
                            carinfobd.UpdateCarTestStatusYCLZT("42", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "42发送成功", "JZJS上传过程", 1);
                        }
                        else
                            FileOpreate.SaveLog("流水号：" + jylsh + "42发送失败", "JZJS上传过程", 1);
                        //更新YCLZT到42
                    }
                    if ((zt == "43" || zt == "44" || zt == "6") && yclzt == "42")
                    {
                        //发43
                        if (UpdateCapturePicture(dt_test_status.Rows[0], "43"))
                        {
                            // yclzt = "43";
                            carinfobd.UpdateCarTestStatusYCLZT("43", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "43发送成功", "JZJS上传过程", 1);
                        }
                        else
                            FileOpreate.SaveLog("流水号：" + jylsh + "43发送失败", "JZJS上传过程", 1);
                        //更新YCLZT到43
                    }
                    if ((zt == "44" || zt == "6") && yclzt == "43")
                    {
                        //发43
                        if (UpdateCapturePicture(dt_test_status.Rows[0], "44"))
                        {
                            // yclzt = "43";
                            carinfobd.UpdateCarTestStatusYCLZT("44", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "44发送成功", "JZJS上传过程", 1);
                        }
                        else
                            FileOpreate.SaveLog("流水号：" + jylsh + "43发送失败", "JZJS上传过程", 1);
                        //更新YCLZT到43
                    }
                    if (zt == "6" && yclzt == "44")
                    {
                        //发过程数据、结果数据及检测结束命令——JS001

                        string test_result = "";
                        //上传外检
                        //上传外检
                        if (!UpdateResult("outlook", jylsh, jycs, realjylsh, out test_result))
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs, test_result);
                            FileOpreate.SaveLog("流水号：" + jylsh + "外检数据上传失败", "ASM上传过程", 1);
                        }

                        if (UpdateResult("JZJS", jylsh, jycs,realjylsh, out test_result))
                        {
                            FileOpreate.SaveLog("流水号：" + jylsh + "结果数据发送成功", "JZJS上传过程", 1);
                            if (UpdateProjectStop(dt_test_status.Rows[0], test_result))
                            {
                                UpdateStatus(jylsh, jycs, "已处理状态", "上传完毕");
                                FileOpreate.SaveLog("流水号：" + jylsh + "项目结束命令发送成功，所有数据上传完毕", "JZJS上传过程", 1);
                                //更新其他状态
                                DataRow dr = dt_AlreadyUpload.NewRow();
                                dr["检验流水号"] = dt_test_status.Rows[0]["JYLSH"].ToString();
                                dr["车辆号牌"] = dt_test_status.Rows[0]["CLHP"].ToString();
                                dr["号牌种类"] = dt_test_status.Rows[0]["HPZL"].ToString();
                                dr["线号"] = dt_test_status.Rows[0]["LINEID"].ToString();
                                dr["检验次数"] = dt_test_status.Rows[0]["JYCS"].ToString();
                                dr["检验时间"] = dt_test_status.Rows[0]["JCSJ"].ToString();
                                dr["检测方法"] = dt_test_status.Rows[0]["JCFF"].ToString();
                                dr["上传成功时间"] = System.DateTime.Now.ToString();

                                dt_AlreadyUpload.Rows.Add(dr);
                                carinfobd.DeleteCarTestStatus(jylsh, jycs);
                                carinfobd.deleteCarInWaitlist(jylsh, jycs);
                            }
                            else
                            {
                                carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs, test_result);//将该条记录置为上传失败记录
                                FileOpreate.SaveLog("流水号：" + jylsh + "检测结束上传失败", "JZJS上传过程", 1);
                            }
                        }
                        else
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs, test_result);//将该条记录置为上传失败记录
                            FileOpreate.SaveLog("流水号：" + jylsh + "上传失败", "JZJS上传过程", 1);
                        }
                        //更新YCLZT到6
                    }

                    //更新YCLZT状态到实际执行到的位置
                    //carinfobd.UpdateCarTestStatusYCLZT(yclzt, jylsh, jycs);
                    UpdateStatus(jylsh, jycs, "备注", "当前数据上传完");
                }
                catch (Exception er)
                {
                    //更新YCLZT状态到实际执行到的位置
                    carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs, er.Message);
                    FileOpreate.SaveLog("数据上传错误原因：" + er.Message, "JZJS上传错误", 1);
                }
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("线程运行错误原因：" + er.Message, "JZJS上传线程错误", 1);
            }
        }

        /// <summary>
        /// 自由加速
        /// </summary>
        /// <param name="obj"></param>
        private void UploadZYJSData(object obj)
        {
            try
            {
                DataTable dt_test_status = ((DataTable)obj).Copy();

                if (dt_test_status.Rows.Count <= 0)
                {
                    //carinfobd.UpdateCarTestStatusYCLZT(dr_temp["YCLZT"].ToString(), dr_temp["JYLSH"].ToString(), dr_temp["JYCS"].ToString());
                    FileOpreate.SaveLog("ZYJS上传线程传入数据为空" + obj.ToString(), "ZYJS上传线程执行失败", 1);
                    return;
                }
                string jylsh = dt_test_status.Rows[0]["JYLSH"].ToString();
                string jycs = dt_test_status.Rows[0]["JYCS"].ToString();
                string zt = dt_test_status.Rows[0]["ZT"].ToString();
                string yclzt = dt_test_status.Rows[0]["YCLZT"].ToString();
                DataRow waitrow = FrmMain.dtWaitCarListInNet.Select("jylsh='" + jylsh + "' and jycs='" + jycs + "'").FirstOrDefault();
                if (waitrow == null)
                {
                    FileOpreate.SaveLog("未找到该条记录的待检记录，不能继续上传", "ASM上传线程执行失败", 1);
                    return;
                }
                string realjylsh = waitrow["reallsh"].ToString();
                //DataRow obj_car = null;
                //carinfobd.getVehicleData("已检车辆信息", jylsh, jycs, out obj_car);
                try
                {
                    FileOpreate.SaveLog("流水号：" + jylsh + "ZYJS数据上传准备", "ZYJS上传过程", 1);
                    if (zt == "1")
                    {
                        //向平台发送开始信息
                        //interfacejiangsu.BeginCheck(obj_car);
                        //发请求开始——KS001，解析开始是否同意，更新检测状态，并更新YCLZT到1
                        string error_info = "";
                        if (UpdateProjectStart(dt_test_status.Rows[0],realjylsh, out error_info))
                        {
                            if (carinfobd.UpdateCarTestStatusZT("2", jylsh, jycs, error_info))
                            {
                                carinfobd.UpdateCarTestStatusYCLZT("2", jylsh, jycs);
                                FileOpreate.SaveLog("流水号：" + jylsh + "项目开始发送成功", "ZYJS上传过程", 1);
                            }
                            else
                            {
                                FileOpreate.SaveLog("流水号：" + jylsh + "更新允许开始状态失败", "ZYJS上传过程", 1);
                            }
                        }
                        else
                        {
                            carinfobd.UpdateCarTestStatusZT("3", jylsh, jycs, error_info);
                            carinfobd.UpdateCarTestStatusYCLZT("0", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "不允许开始，原因：" + error_info, "ZYJS上传过程", 1);
                            return;
                        }
                    }

                    if ((zt == "54" || zt == "51" || zt == "52" || zt == "53" || zt == "6") && yclzt == "2")
                    {
                        //发51照片
                        if (UpdateCapturePicture(dt_test_status.Rows[0], "54"))
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("54", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "54发送成功", "ZYJS上传过程", 1);
                        }
                        else
                        {
                            FileOpreate.SaveLog("流水号：" + jylsh + "54发送失败", "ZYJS上传过程", 1);
                        }
                        //更新YCLZT到51
                    }
                    if ((zt == "51" || zt == "52" || zt == "53" || zt == "6") && yclzt == "54")
                    {
                        //发51照片
                        if (UpdateCapturePicture(dt_test_status.Rows[0], "51"))
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("51", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "51发送成功", "ZYJS上传过程", 1);
                        }
                        else
                        {
                            FileOpreate.SaveLog("流水号：" + jylsh + "51发送失败", "ZYJS上传过程", 1);
                        }
                        //更新YCLZT到51
                    }
                    if ((zt == "52" || zt == "53" || zt == "6") && yclzt == "51")
                    {
                        //发52
                        if (UpdateCapturePicture(dt_test_status.Rows[0], "52"))
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("52", jylsh, jycs);
                            FileOpreate.SaveLog("流水号：" + jylsh + "52发送成功", "ZYJS上传过程", 1);
                        }
                        else
                            FileOpreate.SaveLog("流水号：" + jylsh + "52发送失败", "ZYJS上传过程", 1);
                        //更新YCLZT到52
                    }
                    if ((zt == "53" || zt == "6") && yclzt == "52")
                    {
                        //发53
                        if (UpdateCapturePicture(dt_test_status.Rows[0], "53"))
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("53", jylsh, jycs);
                            //yclzt = "53";
                            FileOpreate.SaveLog("流水号：" + jylsh + "53发送成功", "ZYJS上传过程", 1);
                        }
                        else
                            FileOpreate.SaveLog("流水号：" + jylsh + "53发送失败", "ZYJS上传过程", 1);
                        //更新YCLZT到53
                    }
                    if (zt == "6" && yclzt == "53")
                    {
                        string test_result = "";
                        //上传外检
                        //上传外检
                        if (!UpdateResult("outlook", jylsh, jycs, realjylsh, out test_result))
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs, test_result);
                            FileOpreate.SaveLog("流水号：" + jylsh + "外检数据上传失败", "ASM上传过程", 1);
                        }

                        if (UpdateResult("ZYJS", jylsh, jycs,realjylsh, out test_result))
                        {
                            FileOpreate.SaveLog("流水号：" + jylsh + "结果数据发送成功", "ZYJS上传过程", 1);
                            if (UpdateProjectStop(dt_test_status.Rows[0], test_result))
                            {
                                UpdateStatus(jylsh, jycs, "已处理状态", "上传完毕");
                                FileOpreate.SaveLog("流水号：" + jylsh + "项目结束发送成功，所有数据上传完毕", "ZYJS上传过程", 1);
                                //更新其他状态
                                DataRow dr = dt_AlreadyUpload.NewRow();
                                dr["检验流水号"] = dt_test_status.Rows[0]["JYLSH"].ToString();
                                dr["车辆号牌"] = dt_test_status.Rows[0]["CLHP"].ToString();
                                dr["号牌种类"] = dt_test_status.Rows[0]["HPZL"].ToString();
                                dr["线号"] = dt_test_status.Rows[0]["LINEID"].ToString();
                                dr["检验次数"] = dt_test_status.Rows[0]["JYCS"].ToString();
                                dr["检验时间"] = dt_test_status.Rows[0]["JCSJ"].ToString();
                                dr["检测方法"] = dt_test_status.Rows[0]["JCFF"].ToString();
                                dr["上传成功时间"] = System.DateTime.Now.ToString();
                                dt_AlreadyUpload.Rows.Add(dr);
                                carinfobd.DeleteCarTestStatus(jylsh, jycs);
                                carinfobd.deleteCarInWaitlist(jylsh, jycs);
                            }
                            else
                            {
                                carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs, test_result);
                                FileOpreate.SaveLog("流水号：" + jylsh + "检测结束上传失败", "ZYJS上传过程", 1);
                            }

                        }
                        else
                        {
                            carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs, test_result);
                            FileOpreate.SaveLog("流水号：" + jylsh + "过程数据上传失败", "ZYJS上传过程", 1);
                        }
                        //更新YCLZT到6
                    }

                    //更新YCLZT状态到实际执行到的位置
                    //carinfobd.UpdateCarTestStatusYCLZT(yclzt, jylsh, jycs);
                    UpdateStatus(jylsh, jycs, "备注", "当前数据上传完");
                }
                catch (Exception er)
                {
                    //更新YCLZT状态到实际执行到的位置
                    carinfobd.UpdateCarTestStatusYCLZT("-1", jylsh, jycs);
                    FileOpreate.SaveLog("数据上传错误原因：" + er.Message, "ZYJS上传错误", 1);
                }
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("线程运行错误原因：" + er.Message, "ZYJS上传线程错误", 1);
            }
        }
        #endregion

        #region 上报数据发送操作，以流水号及检验测试查询数据
        //项目开始发送
        private bool UpdateProjectStart(DataRow TestStatus,string lsh, out string error_info)
        {
            try
            {
                error_info = "";
                string jycs = TestStatus["JYCS"].ToString();
                string jylsh = TestStatus["JYLSH"].ToString();
                
                DataTable dt = new DataTable();
                if (ModPublicJiangsu.getPicFromXcPDA)
                {
                    DataRow obj_car = null;
                    carinfobd.getCarWaitInfoByLsh( jylsh, jycs, out obj_car);
                    if(obj_car==null)
                    {
                        error_info = "查询对应待车辆信息时失败";
                        //return false;
                        return false;
                    }
                    string outlookid= obj_car["OutlookCheckID"].ToString();
                    dt = carinfobd.getXcOutLookCheck(outlookid);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        error_info = "未找到该车外检记录，请先进行外检";
                        //return false;
                        return false;
                    }
                    else
                    {
                        string checkResult = dt.Rows[0]["CheckResult"].ToString();
                        if (checkResult != "Y")
                        {
                            error_info = "该车外检记录不合格，请检查后重新进行外检";
                            return false;
                        }
                        else
                            return true;
                    }
                }
                else
                {
                    dt = carinfobd.getOutLookCheck(jylsh);

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        error_info = "未找到该车外检记录，请先进行外检";
                        //return false;
                        return true;
                    }
                    else
                    {
                        string checkResult = dt.Rows[0]["CheckResult"].ToString();
                        if (checkResult != "Y")
                        {
                            error_info = "该车外检记录不合格，请检查后重新进行外检";
                            return false;
                        }
                        string id = dt.Rows[0]["ID"].ToString();
                        Hashtable ht = new Hashtable();
                        ht.Add("IsOutlookChecked", "Y");
                        ht.Add("OutLookCheckID", id);
                        if (conforsql.Update("待检车辆", "where JYLSH='" + jylsh + "' and JCCS='" + jycs + "'", ht) > 0)
                        {
                            return true;
                        }
                        else
                        {
                            error_info = "更新待检车辆外检信息失败";
                            return false;
                        }
                    }
                }
            }
            catch (Exception ER)
            {
                FileOpreate.SaveLog("更新待检车辆信息异常:" + ER.Message, "请求开始", 1);
                error_info = "更新待检车辆信息异常";
                return false;
            }
        }

        //项目结束发送
        private bool UpdateProjectStop(DataRow TestStatus, string test_result)
        {

            test_result = "";
            return true;
        }

        //项目照片发送
        private bool UpdateCapturePicture(DataRow TestStatus, string photo_num)
        {
            photo_num = "";
            return true;
        }

        //上传检测结果
        private bool UpdateResult(string jcff, string jylsh, string jycs,string reallsh, out string test_result)
        {
            //int zt = int.Parse(YCLZT);
            test_result = "";
            try
            {
                DataRow obj_result = null;
                DataRow obj_process = null;
                DataRow obj_car = null;
                DataRow obj_obd = null;
                if (jcff != "outlook")
                {
                    carinfobd.getTestResult(jcff == "ZYJS" ? "Zyjs_Btg" : jcff, jylsh, jycs, out obj_result);
                    carinfobd.getProcessData(jcff + "_DATASECONDS", jylsh, jycs, out obj_process);
                }
                carinfobd.getVehicleData("已检车辆信息", jylsh, jycs, out obj_car);

                string clid = obj_car["CLID"].ToString();
                carinfobd.getOBDResult(clid, out obj_obd);

                if (jcff != "outlook")
                {
                    if (obj_result == null)
                    {
                        test_result = "没有找到结果数据";
                        FileOpreate.SaveLog("没有找到结果数据", "[" + jylsh + "]上传过程错误", 1);
                        return false;
                    }
                    if (obj_process == null)
                    {
                        test_result = "没有找到过程数据";
                        FileOpreate.SaveLog("没有找到过程数据", "[" + jylsh + "]上传过程错误", 1);
                        return false;
                    }
                }

                if (obj_car == null)
                {
                    test_result = "没有找到车辆数据";
                    FileOpreate.SaveLog("没有找到车辆数据", "[" + jylsh + "]上传过程错误", 1);
                    return false;
                }
                
                string bgbh, qcrode;
                if (!interfacejiangsu.UploadInspectionResult(obj_car, obj_result, obj_process, obj_obd, jcff,reallsh,out bgbh,out qcrode))
                {
                    test_result = "发送" + jcff + "结果数据失败";
                    FileOpreate.SaveLog("发送" + jcff + "结果数据失败", "[" + jylsh + "]上传过程错误", 1);
                    return false;
                }

                if (jcff != "outlook")
                {
                    updateTestingID(clid, bgbh,qcrode);//更新检测报告编号
                    FileOpreate.SaveLog("发送结果数据成功", "[" + jylsh + "]上传过程", 1);
                }
                else
                {
                    FileOpreate.SaveLog("发送外检结果数据成功", "[" + jylsh + "]上传外检", 1);
                }

                return true;
            }
            catch (Exception er)
            {

                test_result = "上传过程异常" + er.Message;
                FileOpreate.SaveLog("执行" + jcff + "结果数据发送失败，原因：" + er.Message, "[" + jylsh + "]上传过程异常", 1);
                return false;
            }
        }


        #endregion
        #region 设备标定数据上传
        private void UpdateBD(object BDData)
        {
            try
            {
                DataRow temp_dr = (DataRow)BDData;
                DataTable temp_dt_bddata = temp_dr.Table.Clone();
                temp_dt_bddata.ImportRow(temp_dr);
                string lineid = temp_dt_bddata.Rows[0]["JCGWH"].ToString();
                int uploadTimes = int.Parse(temp_dt_bddata.Rows[0]["BY1"].ToString());
                string id = temp_dt_bddata.Rows[0]["ID"].ToString();
                string lx = temp_dt_bddata.Rows[0]["LX"].ToString();
                try
                {
                    if (interfacejiangsu.uploadBd(temp_dt_bddata.Rows[0]))
                    {
                        FileOpreate.SaveLog("【设备标定数据】|ID=" + id + "|LX=" + lx + "|数据上传成功", "标定数据发送", 1);

                        //Update_ZjBd_Log(lineid, int.Parse(temp_dt_bddata.Rows[0]["LX"].ToString()), "已上传");
                        carinfobd.UpdateBDZT("1", id, ++uploadTimes);
                    }
                    else
                    {
                        FileOpreate.SaveLog("！！！！【设备标定数据】|ID=" + id + "|LX=" + lx + "|数据上传失败！！！！", "标定数据发送", 1);
                        //上传不成功则将状态改回0
                        carinfobd.UpdateBDZT("0", id, ++uploadTimes);
                    }
                }
                catch (Exception er)
                {
                    FileOpreate.SaveLog("！！！！【设备标定数据】|ID=" + id + "|LX=" + lx + "|数据上传发生异常：" + er.Message + "！！！！", "标定数据发送", 1);
                    carinfobd.UpdateBDZT("0", id, ++uploadTimes);
                }
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("！！！！【设备标定数据】|取标定数据发生异常：" + er.Message + "！！！！", "标定数据发送", 1);
            }
        }
        #endregion

        #region 设备自检数据上传
        private void UpdateZj(object BDData)
        {
            try
            {
                DataRow temp_dr = (DataRow)BDData;
                DataTable temp_dt_bddata = temp_dr.Table.Clone();
                temp_dt_bddata.ImportRow(temp_dr);
                string lineid = temp_dt_bddata.Rows[0]["JCGWH"].ToString();
                int uploadTimes = int.Parse(temp_dt_bddata.Rows[0]["BY1"].ToString());
                string id = temp_dt_bddata.Rows[0]["ID"].ToString();
                string lx = temp_dt_bddata.Rows[0]["LX"].ToString();
                try
                {
                    DataRow processdata = carinfobd.getZjBdProcessData(id);
                    if (interfacejiangsu.uploadZj(temp_dt_bddata.Rows[0],processdata))
                    {
                        FileOpreate.SaveLog("【设备自检数据】|ID=" + id + "|LX=" + lx + "|数据上传成功", "标定数据发送", 1);
                        // Update_ZjBd_Log(lineid, int.Parse(temp_dt_bddata.Rows[0]["LX"].ToString()), "已上传");
                        carinfobd.UpdateSelfCheckZT("1", id, ++uploadTimes);
                    }
                    else
                    {
                        FileOpreate.SaveLog("！！！！【设备自检数据】|ID=" + id + "|LX=" + lx + "|数据上传失败！！！！", "标定数据发送", 1);
                        //上传不成功则将状态改回0
                        carinfobd.UpdateSelfCheckZT("0", id, ++uploadTimes);
                    }
                }
                catch (Exception er)
                {
                    FileOpreate.SaveLog("！！！！【设备自检数据】|ID=" + id + "|LX=" + lx + "|数据上传发生异常：" + er.Message + "！！！！", "标定数据发送", 1);
                    carinfobd.UpdateSelfCheckZT("0", id, ++uploadTimes);
                }
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("！！！！【设备自检数据】|取标定数据发生异常：" + er.Message + "！！！！", "标定数据发送", 1);
            }
        }
        #endregion

        private void 标定自检上传记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            dataGridView_UploadFailed.Visible = false;
            dataGridView_Ready_And_Failed.Visible = false;
            dataGridView_AlreadyUpload.Visible = false;
            dataGridViewWaitList.Visible = false;
            dataGridView_BDZJ.Visible = true;
            dataGridView_BDZJ.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //显示标定自检上传记录表

            tSSL_Status.Text = "状态：当前显示标定自检数据上传记录！";
        }

        private void 数据库升级ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //alter table [车辆检测状态] alter column BZ varchar(max)
            if (carinfobd.updateSQL())
                MessageBox.Show("数据库升级成功！");
            else
                MessageBox.Show("数据库升级失败！");
        }
        #region 其他功能函数
        /// <summary>
        /// 初始化状态代码
        /// </summary>
        private void InitJcZt()
        {
            JcZt.Add("-1", "上传失败");
            JcZt.Add("", "空状态");
            JcZt.Add("0", "检测准备");
            JcZt.Add("1", "请求开始");
            JcZt.Add("2", "允许检测");
            JcZt.Add("3", "不允许检测");
            JcZt.Add("4", "开始检测");
            JcZt.Add("5", "未完成退出");
            JcZt.Add("6", "检测完成");
            JcZt.Add("11", "SDS高怠速中");
            JcZt.Add("12", "SDS低怠速中");
            JcZt.Add("14", "SDS高怠速结束");
            JcZt.Add("13", "SDS低怠速结束");
            JcZt.Add("21", "5025开始");
            JcZt.Add("22", "ASM5025结束");
            JcZt.Add("23", "ASM2540开始");
            JcZt.Add("24", "ASM2540结束");
            JcZt.Add("31", "Vmas15km/h中");
            JcZt.Add("32", "Vmas32km/h中");
            JcZt.Add("33", "Vmas50km/h中");
            JcZt.Add("41", "JZJS100Vel");
            JcZt.Add("42", "JZJS90Vel");
            JcZt.Add("43", "JZJS80Vel");
            JcZt.Add("51", "ZYJS第一次");
            JcZt.Add("52", "ZYJS第二次");
            JcZt.Add("53", "ZYJS第三次");

            Dic_ZJLX.Add("2", "加载滑行-汽油");
            Dic_ZJLX.Add("2cy", "加载滑行-柴油");
            Dic_ZJLX.Add("3", "寄生功率-汽油");
            Dic_ZJLX.Add("3cy", "寄生功率-柴油");
            Dic_ZJLX.Add("4", "废气仪常规");
            Dic_ZJLX.Add("5", "废气仪低点");
            Dic_ZJLX.Add("6", "烟度计检查");
            Dic_ZJLX.Add("11_l", "废气仪单点-低标气");
            Dic_ZJLX.Add("11_h", "废气仪单点-高标气");
            Dic_ZJLX.Add("11_z", "废气仪单点-零气");
            Dic_ZJLX.Add("11_x", "废气仪单点-响应时间");
            Dic_ZJLX.Add("12_l", "废气仪五点-低标气");
            Dic_ZJLX.Add("12_ml", "废气仪五点-中低标气");
            Dic_ZJLX.Add("12_mh", "废气仪五点-中高气");
            Dic_ZJLX.Add("12_h", "废气仪五点-高标气");
            Dic_ZJLX.Add("12_z", "废气仪五点-零气");
            Dic_ZJLX.Add("13_l", "氮氧分析仪单点-低标气");
            Dic_ZJLX.Add("13_h", "氮氧分析仪单点-高标气");
            Dic_ZJLX.Add("13_z", "氮氧分析仪单点-零气");
            Dic_ZJLX.Add("13_cr", "氮氧分析仪单点-NOx转换率");
            Dic_ZJLX.Add("14_l", "氮氧分析仪五点-低标气");
            Dic_ZJLX.Add("14_ml", "氮氧分析仪五点-中低标气");
            Dic_ZJLX.Add("14_mh", "氮氧分析仪五点-中高气");
            Dic_ZJLX.Add("14_h", "氮氧分析仪五点-高标气");
            Dic_ZJLX.Add("14_z", "氮氧分析仪五点-零气");

            Dic_BDZJ.Add("1", "测功机自检");
            Dic_BDZJ.Add("2", "附加功率损失");
            Dic_BDZJ.Add("3", "五气分析仪自检");
            Dic_BDZJ.Add("4", "泄漏检查");
            Dic_BDZJ.Add("5", "分析仪氧量程检查");
            Dic_BDZJ.Add("6", "低标气检查");
            Dic_BDZJ.Add("7", "流量计检查");
            Dic_BDZJ.Add("8", "转速计");
            Dic_BDZJ.Add("9", "电子环境检查");
            Dic_BDZJ.Add("10", "NOx分析仪");
            Dic_BDZJ.Add("11", "烟度计检查");
            Dic_BDZJ.Add("12", "加载滑行检查");
            Dic_BDZJ.Add("13", "五气分析仪五点检查");
            Dic_BDZJ.Add("14", "NOx分析仪五点检查");
            Dic_BDZJ.Add("15", "预热");
            Dic_BDZJ.Add("20", "加载滑行标定");
            Dic_BDZJ.Add("21", "寄生功率校准");
            Dic_BDZJ.Add("22", "车速校准");
            Dic_BDZJ.Add("23", "扭力校准");
            Dic_BDZJ.Add("24", "废气仪标准");
            Dic_BDZJ.Add("25", "烟度计校准");
            Dic_BDZJ.Add("26", "NOx分析仪校准");
            Dic_BDZJ.Add("27", "气象站校准");
            Dic_BDZJ.Add("28", "惯量");
            Dic_BDZJ.Add("29", "变载荷");
            Dic_BDZJ.Add("30", "响应时间");
            Dic_BDZJ.Add("31", "流量计标定");
            Dic_BDZJ.Add("40", "同时时间");
        }

        private void Update_ZjBd_Log(string line_id, int xm_index, string result)
        {
            DataRow[] dr_change_temp = dt_ZjBdUpload.Select("线号='" + line_id + "'");
            if (dr_change_temp.Length > 0)
                dr_change_temp[0][xm_index] = result;
        }
        #endregion

        private void 上传失败记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            dataGridView_UploadFailed.Visible = true;
            dataGridView_Ready_And_Failed.Visible = false;
            dataGridView_AlreadyUpload.Visible = false;
            dataGridView_BDZJ.Visible = false;
            dataGridViewWaitList.Visible = false;
            tSSL_Status.Text = "状态：当前显示上传失败数据列表！";
        }
        /// <summary>
        /// 刷新失败列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerRefreshFaileData_Tick(object sender, EventArgs e)
        {
            if (!_whether_Connected)//若同步时间失败则不上传
                return;

            try
            {
                DataTable CarTestStatusTable = carinfobd.getCarUploadFailed();//获取车辆检测状态表
                #region 显示 
                dt_UploadFailed.Clear();
                if (CarTestStatusTable != null && CarTestStatusTable.Rows.Count > 0)
                {
                    for (int i = 0; i < CarTestStatusTable.Rows.Count; i++)
                    {
                        DataRow dr = dt_UploadFailed.NewRow();
                        dr["检验流水号"] = CarTestStatusTable.Rows[i]["JYLSH"].ToString();
                        dr["车辆号牌"] = CarTestStatusTable.Rows[i]["CLHP"].ToString();
                        dr["号牌种类"] = CarTestStatusTable.Rows[i]["HPZL"].ToString();
                        dr["线号"] = CarTestStatusTable.Rows[i]["LINEID"].ToString();
                        dr["检验次数"] = CarTestStatusTable.Rows[i]["JYCS"].ToString();
                        dr["检验时间"] = CarTestStatusTable.Rows[i]["JCSJ"].ToString();
                        dr["检测方法"] = CarTestStatusTable.Rows[i]["JCFF"].ToString();
                        dr["当前状态"] = JcZt[CarTestStatusTable.Rows[i]["ZT"].ToString()];
                        dr["已处理状态"] = CarTestStatusTable.Rows[i]["YCLZT"].ToString() == "6" ? "上传过程/结果数据" : JcZt[CarTestStatusTable.Rows[i]["YCLZT"].ToString()];
                        dr["上传失败原因"] = CarTestStatusTable.Rows[i]["BZ"].ToString();
                        dt_UploadFailed.Rows.Add(dr);
                    }
                }
                dataGridView_UploadFailed.DataSource = dt_UploadFailed;
                #endregion
            }
            catch (Exception er)
            {
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dataGridView_UploadFailed.Visible = false;
            dataGridView_AlreadyUpload.Visible = false;
            dataGridView_BDZJ.Visible = false;
            dataGridView_BDZJ.Visible = false;
            dataGridView_Ready_And_Failed.Visible = false;
            dataGridView_Ready_And_Failed.Visible = false;
            dataGridViewWaitList.Visible = true;
            tSSL_Status.Text = "状态：当前显示待检车辆列表！";
        }

        private void 删除选中的待检信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewWaitList.Visible == true && MessageBox.Show("您确定要删除该行数据吗？", "请注意", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                try
                {
                    //待检列表
                    string strDeleteResult = "状态：流水号" + ChoosedJylsh;
                    if (carinfobd.deleteCarInWaitlist(ChoosedJylsh, "1"))//删除本地待检列表
                        strDeleteResult += "本地待检列表删除成功|";
                    else
                        strDeleteResult += "本地待检列表删除失败|";

                    FileOpreate.SaveLog(strDeleteResult, "待检车辆信息删除", 1);
                }
                catch (Exception er)
                {
                    tSSL_Status.Text = "数据删除错误：该行数据删除失败，原因：" + er.Message;
                    FileOpreate.SaveLog("待检车辆信息删除失败，原因：" + er.Message, "主界面按钮", 1);
                }
            }
        }

        private void 待检列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    DBControl DHC = new DBControl();
            //    dataGridView_UploadFailed.Visible = false;
            //    dataGridView_Ready_And_Failed.Visible = false;
            //    dataGridView_AlreadyUpload.Visible = false;
            //    dataGridView_BDZJ.Visible = false;
            //    dataGridViewWaitList.Visible = true;
            //    dataGridViewWaitList.DataSource = DHC.getCarInWaitList("Y");
            //}
            //catch { }
        }

        private void dataGridViewWaitList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Choosedindex = e.RowIndex;
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            int r = this.dataGridViewWaitList.CurrentRow.Index;
            ChoosedJylsh = dataGridViewWaitList.Rows[r].Cells[20].Value.ToString();

        }

        private void 待检列表ToolStripMenuItem_DoubleClick(object sender, EventArgs e)
        {

        }

        private void 刷新待检列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DBControl DHC = new DBControl();
                dataGridView_UploadFailed.Visible = false;
                dataGridView_Ready_And_Failed.Visible = false;
                dataGridView_AlreadyUpload.Visible = false;
                dataGridView_BDZJ.Visible = false;
                dataGridViewWaitList.Visible = true;
                dataGridViewWaitList.DataSource = DHC.getCarInWaitList("Y");
            }
            catch { }
        }

        private void 测试发送开始ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dttest = new DataTable();
            dttest.Columns.Add("JYLSH");
            dttest.Columns.Add("JCCS");
            dttest.Columns.Add("CLHP");
            dttest.Columns.Add("CLSBM");
            dttest.Columns.Add("LINEID");
            DataRow dr = dttest.NewRow();
            //("JYLSH","JCCS","CLHP","CLSBM","LINEID");
            dr["JYLSH"]= "川A22222T20200921111624";
            dr["JCCS"] = "1";
            dr["CLHP"] = "川A22222";
            dr["CLSBM"] = "12345678901234567";
            dr["LINEID"] = "01";
            string[] piclx = { "1", "2", "3" };
            string[] piccontent = { "", "", "" };
            for (int i = 0; i < piclx.Length; i++)
            {
                string code, msg;
                JbtNetLibrary.OBD_lr.outlookPic pic;
                if (ModPublicJiangsu.wjserver.getOutlookPic(dr["JYLSH"].ToString(), dr["JCCS"].ToString(), piclx[i], out code, out msg, out pic))
                    piccontent[i] = pic.photo;
            }
            interfacejiangsu.BeginCheck(dr, piccontent[0], piccontent[1], piccontent[2]);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timerBdZjUpdate.Enabled = checkBox1.Checked;
            timerRefreshFaileData.Enabled = checkBox1.Checked;
            timerUploadTestData.Enabled = checkBox1.Checked;
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }
    }

    #region 设置系统时间
    public class SetSystemDateTime
    {
        [DllImport("Kernel32.dll")]
        public static extern bool SetLocalTime(ref SystemTime sysTime);

        public static bool SetLocalTimeByStr(string timestr)
        {
            bool flag = false;
            SystemTime sysTime = new SystemTime();
            DateTime dt = Convert.ToDateTime(timestr);
            sysTime.wYear = Convert.ToUInt16(dt.Year);
            sysTime.wMonth = Convert.ToUInt16(dt.Month);
            sysTime.wDay = Convert.ToUInt16(dt.Day);
            sysTime.wHour = Convert.ToUInt16(dt.Hour);
            sysTime.wMinute = Convert.ToUInt16(dt.Minute);
            sysTime.wSecond = Convert.ToUInt16(dt.Second);
            sysTime.wMiliseconds = Convert.ToUInt16(dt.Millisecond);
            try
            {
                flag = SetSystemDateTime.SetLocalTime(ref sysTime);
            }
            catch (Exception e)
            {
                FileOpreate.SaveLog("SetSystemDateTime函数执行异常" + e.Message, "SetSystemTimeError", 1);
                MessageBox.Show("设置系统时间失败:" + e.Message);
                return false;
            }
            return true;
        }
    }

    public struct SystemTime
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMiliseconds;
    }
    #endregion
}