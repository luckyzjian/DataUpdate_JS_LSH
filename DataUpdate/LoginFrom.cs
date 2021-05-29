using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DataUpdate
{
    public partial class LoginFrom : Form
    {
        private bool login_status;//指示是否登录成功
        public bool LoginStatus { get { return login_status; } }
        private DBControl userinfodb = new DBControl();//定义数据库操作

        public LoginFrom()
        {
            InitializeComponent();
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            panel_login.Location = new Point(51, 70);
            panel_config.Location = new Point(5, 40);
            panel_config.Visible = false;

            if (FileOpreate.SaveUser)
            {
                textBox_User_Name.Text = FileOpreate.User_Name;
                checkBox_save_name.Checked = true;
            }
            if (FileOpreate.SavePwd)
            {
                textBox_User_Pwd.Text = FileOpreate.User_Pwd;
                checkBox_save_pwd.Checked = true;
            }
        }

        private void button_change_config_Click(object sender, EventArgs e)
        {
            panel_login.Visible = false;
            panel_config.Visible = true;
            //数据库配置
            textBox_DB_address.Text = FileOpreate.DB_Address;
            textBox_DB_name.Text = FileOpreate.DB_Name;
            textBox_DB_user.Text = FileOpreate.DB_User;
            textBox_DB_pwd.Text = FileOpreate.DB_Pwd;
            //联网接口配置
            textBox_Interface_address.Text = FileOpreate.Interface_Address;
            textBox_Interface_bdaddress.Text = FileOpreate.Interface_BdAddress;
            textBox_Interface_jkxlh.Text = FileOpreate.Interface_Jkxlh;
            textBox_Interface_jczid.Text = FileOpreate.Interface_JczID;

            textBJsUser.Text = FileOpreate.Interface_JczUser;
            textBJsPass.Text = FileOpreate.Interface_JczPass;
            textBCityCode.Text = FileOpreate.Interface_JcCityCode;
            textBLine01.Text = FileOpreate.Interface_LineidDic["01"];
            textBLine02.Text = FileOpreate.Interface_LineidDic["02"];
            textBLine03.Text = FileOpreate.Interface_LineidDic["03"];
            textBLine04.Text = FileOpreate.Interface_LineidDic["04"];
            textBLine05.Text = FileOpreate.Interface_LineidDic["05"];
            textBLine06.Text = FileOpreate.Interface_LineidDic["06"];
            dateTimePicker1.Value = FileOpreate.Interface_LineJdYxqDic["01"];
            dateTimePicker2.Value = FileOpreate.Interface_LineJdYxqDic["02"];
            dateTimePicker3.Value = FileOpreate.Interface_LineJdYxqDic["03"];
            dateTimePicker4.Value = FileOpreate.Interface_LineJdYxqDic["04"];
            dateTimePicker5.Value = FileOpreate.Interface_LineJdYxqDic["05"];
            dateTimePicker6.Value = FileOpreate.Interface_LineJdYxqDic["06"];

            textBoxXcPDAUrl.Text = FileOpreate.xcPDAurl;
            checkBoxGetPicFromXcPDA.Checked = FileOpreate.getPicFromXcPDA;

            comboBox_Interface_Area.SelectedIndex = FileOpreate.Interface_Area;
            textBox_SQR.Text = FileOpreate.NM_Sqr;
            textBox_PZR.Text = FileOpreate.NM_Zlkzr;
            //联网上传设置
            comboBox_data_update_style.SelectedIndex = FileOpreate.UpdateStyle;
            comboBox_update_fail_solve.SelectedIndex = FileOpreate.UpdateFailSovle;
            numericUpDown_SetTimeOfGetWaitList.Value = FileOpreate.UpdateGetWaitListTime;
            checkBox_getCarListUsed.Checked = FileOpreate.UpdateAutoGetCarList == 1 ? true : false;

            //检测方法
            checkBox14.Checked = FileOpreate.jcff.sds;
            checkBox15.Checked = FileOpreate.jcff.asm;
            checkBox16.Checked = FileOpreate.jcff.vmas;
            checkBox17.Checked = FileOpreate.jcff.jzjs;
            checkBox18.Checked = FileOpreate.jcff.zyjs;
        }

        //退出数据库配置保存
        private void button_Exit_Config_Click(object sender, EventArgs e)
        {
            panel_login.Visible = true;
            panel_config.Visible = false;
        }

        //保存数据库配置
        private void button_SaveDbConfig_Click(object sender, EventArgs e)
        {
            try
            {
                FileOpreate.DB_Address = textBox_DB_address.Text;
                FileOpreate.DB_Name = textBox_DB_name.Text;
                FileOpreate.DB_User = textBox_DB_user.Text;
                FileOpreate.DB_Pwd = textBox_DB_pwd.Text;
                FileOpreate.WritePrivateProfileString("数据库", "服务器", textBox_DB_address.Text, @"./Config.ini");
                FileOpreate.WritePrivateProfileString("数据库", "数据库名", textBox_DB_name.Text, @"./Config.ini");
                FileOpreate.WritePrivateProfileString("数据库", "用户名", textBox_DB_user.Text, @"./Config.ini");
                FileOpreate.WritePrivateProfileString("数据库", "密码", textBox_DB_pwd.Text, @"./Config.ini");
                MessageBox.Show("数据库信息保存成功！", "保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("原因：" + ex.Message.ToString(), "数据库配置信息保存失败");
            }
        }

        //保存接口配置
        private void button_SaveInterfaceConfig_Click(object sender, EventArgs e)
        {
            try
            {
                
                FileOpreate.NM_Sqr = textBox_SQR.Text;
                FileOpreate.NM_Zlkzr = textBox_PZR.Text;
                FileOpreate.WritePrivateProfileString("用户", "内蒙联授权人", FileOpreate.NM_Sqr, @"./Config.ini");
                FileOpreate.WritePrivateProfileString("用户", "内蒙联网质量控制人", FileOpreate.NM_Zlkzr, @"./Config.ini");
                FileOpreate.Interface_Area = comboBox_Interface_Area.SelectedIndex;
                FileOpreate.Interface_Address = textBox_Interface_address.Text;
                FileOpreate.Interface_BdAddress = textBox_Interface_bdaddress.Text;
                FileOpreate.Interface_Jkxlh = textBox_Interface_jkxlh.Text;
                FileOpreate.Interface_JczID = textBox_Interface_jczid.Text;
                FileOpreate.WritePrivateProfileString("联网信息", "联网地区", comboBox_Interface_Area.SelectedIndex.ToString(), @"./Config.ini");
                FileOpreate.WritePrivateProfileString("联网信息", "标定接口地址", textBox_Interface_bdaddress.Text, @"./Config.ini");
                FileOpreate.WritePrivateProfileString("联网信息", "接口地址", textBox_Interface_address.Text, @"./Config.ini");
                FileOpreate.WritePrivateProfileString("联网信息", "接口序列号", textBox_Interface_jkxlh.Text, @"./Config.ini");
                FileOpreate.WritePrivateProfileString("联网信息", "检测站ID", textBox_Interface_jczid.Text, @"./Config.ini");
                FileOpreate.WritePrivateProfileString("联网信息", "用户名Js", textBJsUser.Text, @"./Config.ini");
                FileOpreate.WritePrivateProfileString("联网信息", "密码Js", textBJsPass.Text, @"./Config.ini");
                FileOpreate.WritePrivateProfileString("联网信息", "城市代码", textBCityCode.Text, @"./Config.ini");
                FileOpreate.WritePrivateProfileString("联网信息","01号线线号", textBLine01.Text, @"./Config.ini");
                FileOpreate.WritePrivateProfileString("联网信息", "02号线线号", textBLine02.Text, @"./Config.ini");
                FileOpreate.WritePrivateProfileString("联网信息", "03号线线号", textBLine03.Text, @"./Config.ini");
                FileOpreate.WritePrivateProfileString("联网信息", "04号线线号", textBLine04.Text, @"./Config.ini");
                FileOpreate.WritePrivateProfileString("联网信息", "05号线线号", textBLine05.Text, @"./Config.ini");
                FileOpreate.WritePrivateProfileString("联网信息", "06号线线号", textBLine06.Text, @"./Config.ini");
                FileOpreate.Interface_LineidDic["01"] = textBLine01.Text;
                FileOpreate.Interface_LineidDic["02"] = textBLine02.Text;
                FileOpreate.Interface_LineidDic["03"] = textBLine03.Text;
                FileOpreate.Interface_LineidDic["04"] = textBLine04.Text;
                FileOpreate.Interface_LineidDic["05"] = textBLine05.Text;
                FileOpreate.Interface_LineidDic["06"] = textBLine06.Text;
                FileOpreate.WritePrivateProfileString("联网信息", "01号线检定有效期", dateTimePicker1.Value.ToString("yyyy-MM-dd"), @"./Config.ini");
                FileOpreate.WritePrivateProfileString("联网信息", "02号线检定有效期", dateTimePicker2.Value.ToString("yyyy-MM-dd"), @"./Config.ini");
                FileOpreate.WritePrivateProfileString("联网信息", "03号线检定有效期", dateTimePicker3.Value.ToString("yyyy-MM-dd"), @"./Config.ini");
                FileOpreate.WritePrivateProfileString("联网信息", "04号线检定有效期", dateTimePicker4.Value.ToString("yyyy-MM-dd"), @"./Config.ini");
                FileOpreate.WritePrivateProfileString("联网信息", "05号线检定有效期", dateTimePicker5.Value.ToString("yyyy-MM-dd"), @"./Config.ini");
                FileOpreate.WritePrivateProfileString("联网信息", "06号线检定有效期", dateTimePicker6.Value.ToString("yyyy-MM-dd"), @"./Config.ini");
                FileOpreate.Interface_LineJdYxqDic["01"] = dateTimePicker1.Value;
                FileOpreate.Interface_LineJdYxqDic["02"] = dateTimePicker2.Value;
                FileOpreate.Interface_LineJdYxqDic["03"] = dateTimePicker3.Value;
                FileOpreate.Interface_LineJdYxqDic["04"] = dateTimePicker4.Value;
                FileOpreate.Interface_LineJdYxqDic["05"] = dateTimePicker5.Value;
                FileOpreate.Interface_LineJdYxqDic["06"] = dateTimePicker6.Value;
                MessageBox.Show("联网信息保存成功！", "保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("原因：" + ex.Message.ToString(), "接口配置保存错误");
            }
        }

        //退出接口配置保存
        private void button1_Click(object sender, EventArgs e)
        {
            panel_login.Visible = true;
            panel_config.Visible = false;
        }

        //保存软件上传设置
        private void button_SaveUpdateSet_Click(object sender, EventArgs e)
        {
            try
            {
                FileOpreate.UpdateStyle = comboBox_data_update_style.SelectedIndex;
                FileOpreate.UpdateFailSovle = comboBox_update_fail_solve.SelectedIndex;
                FileOpreate.UpdateGetWaitListTime = (int)numericUpDown_SetTimeOfGetWaitList.Value;
                FileOpreate.UpdateAutoGetCarList = checkBox_getCarListUsed.Checked ? 1 : 0;
                FileOpreate.WritePrivateProfileString("软件配置", "上传方式", FileOpreate.UpdateStyle.ToString(), @"./Config.ini");
                FileOpreate.WritePrivateProfileString("软件配置", "上传失败处理方式", FileOpreate.UpdateFailSovle.ToString(), @"./Config.ini");
                FileOpreate.WritePrivateProfileString("软件配置", "待检列表刷新间隔", FileOpreate.UpdateGetWaitListTime.ToString(), @"./Config.ini");
                FileOpreate.WritePrivateProfileString("软件配置", "是否开启自动获取待检列表", FileOpreate.UpdateAutoGetCarList.ToString(), @"./Config.ini");
                MessageBox.Show("接口配置信息保存成功！", "保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("原因：" + ex.Message, "软件配置保存失败");
            }
        }

        //退出上传设置配置保存
        private void button3_Click(object sender, EventArgs e)
        {
            panel_login.Visible = true;
            panel_config.Visible = false;
        }

        private void checkBox_save_name_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_save_name.Checked)
            {
                if (textBox_User_Name.Text != "")
                {
                    if (textBox_User_Name.Text != FileOpreate.User_Name)
                    {
                        FileOpreate.User_Name = textBox_User_Name.Text;
                        FileOpreate.WritePrivateProfileString("用户", "用户名", textBox_User_Name.Text, @"./Config.ini");
                    }
                    FileOpreate.WritePrivateProfileString("用户", "SU", "Y", @"./Config.ini");
                }
                else
                    checkBox_save_name.Checked = false;
            }
            else
            {
                textBox_User_Name.Text = "";
                FileOpreate.WritePrivateProfileString("用户", "用户名", "", @"./Config.ini");
                FileOpreate.WritePrivateProfileString("用户", "SU", "N", @"./Config.ini");
                checkBox_save_name.Checked = false;
            }
        }

        private void checkBox_save_pwd_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_save_name.Checked == true && checkBox_save_pwd.Checked == true)
            {
                if (textBox_User_Pwd.Text != "")
                {
                    if (textBox_User_Pwd.Text != FileOpreate.User_Pwd)
                    {
                        FileOpreate.User_Pwd = textBox_User_Pwd.Text;
                        FileOpreate.WritePrivateProfileString("用户", "密码", textBox_User_Pwd.Text, @"./Config.ini");
                    }
                    FileOpreate.WritePrivateProfileString("用户", "SP", "Y", @"./Config.ini");
                }
                else
                    checkBox_save_pwd.Checked = false;
            }
            else
            {
                textBox_User_Pwd.Text = "";
                FileOpreate.WritePrivateProfileString("用户", "密码", "", @"./Config.ini");
                FileOpreate.WritePrivateProfileString("用户", "SP", "N", @"./Config.ini");
                checkBox_save_pwd.Checked = false;
            }
        }

        private void button_login_Click(object sender, EventArgs e)
        {
            string username = textBox_User_Name.Text;
            string userpwd = textBox_User_Pwd.Text;
            if (username != "" && userpwd != "")
            {
                DataRow user_info = null;
                if (userinfodb.getUserInfoByName(username, out user_info) == true)
                {
                    if (user_info != null)
                    {
                        if (userpwd == user_info["PASSWORD"].ToString())
                        {
                            login_status = true;
                            this.Close();
                        }
                        else
                            MessageBox.Show("密码错误！");
                    }
                    else
                        MessageBox.Show("该用户不存在！");
                }
                else
                    MessageBox.Show("数据查询失败，请检查数据库连接！");
            }
            else
                MessageBox.Show("请输入用户名或密码后再点登录！");
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            login_status = false;
            this.Close();
        }

        private void LoginFrom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (panel_login.Visible == true)
                    button_login.PerformClick();
                if (panel_config.Visible == true)
                    button_SaveDbConfig.PerformClick();
            }
        }

        private void comboBox_Interface_Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileOpreate.Interface_Area = comboBox_Interface_Area.SelectedIndex;
            
        }

        private void buttonSaveTestLineInfo_Click(object sender, EventArgs e)
        {
            try
            {
                FileOpreate.jcff.sds = checkBox14.Checked;
                FileOpreate.jcff.asm = checkBox15.Checked;
                FileOpreate.jcff.vmas = checkBox16.Checked;
                FileOpreate.jcff.jzjs = checkBox17.Checked;
                FileOpreate.jcff.zyjs = checkBox18.Checked;
                FileOpreate.WritePrivateProfileString("检测方法", "SDS", FileOpreate.jcff.sds ? "Y" : "N", @"./Config.ini");
                FileOpreate.WritePrivateProfileString("检测方法", "ASM", FileOpreate.jcff.asm ? "Y" : "N", @"./Config.ini");
                FileOpreate.WritePrivateProfileString("检测方法", "VMAS", FileOpreate.jcff.vmas ? "Y" : "N", @"./Config.ini");
                FileOpreate.WritePrivateProfileString("检测方法", "JZJS", FileOpreate.jcff.jzjs ? "Y" : "N", @"./Config.ini");
                FileOpreate.WritePrivateProfileString("检测方法", "ZYJS", FileOpreate.jcff.zyjs ? "Y" : "N", @"./Config.ini");

                MessageBox.Show("可检方法设置保存成功！", "保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("原因：" + ex.Message, "可检方法保存失败");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel_login.Visible = true;
            panel_config.Visible = false;
        }

        private void comboBLine_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void LoginFrom_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// utf8码转换 解码
        /// </summary>
        /// <param name="str1"></param>
        /// <returns></returns>
        public string UrlDecode(string str1)
        {
            string cllx = System.Web.HttpUtility.UrlDecode(str1, Encoding.UTF8);
            return cllx;
        }
        private void label1_Click(object sender, EventArgs e)
        {
            return;

            DataTable dt = new DataTable();
            //江苏车辆编码是从平台获取的
            dt.Columns.Add("CLID");
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
            dt.Columns.Add("scqy");
            dt.Columns.Add("pp");
            dt.Columns.Add("xh");
            string strCon = "";
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            string cllx = UrlDecode(strCon);
            xmlDoc.LoadXml(UrlDecode(strCon));
            XmlNodeList xnList = xmlDoc.SelectNodes("//vehicleitem");
            foreach (XmlNode xn in xnList)
            {
                //无法使用xn["ActivityId"].InnerText  
                string VIN = (xn.SelectSingleNode("vin")).InnerText;//LJDHAA125A0008942
                string LSH = xn.SelectSingleNode("checkid").InnerText;//320900991905022304269504

                strCon = ModPublicJiangsu.coa.GetVehicle("", "", VIN);

                strCon = "<xml><status>true</status><errMsg></errMsg><VEHICLE_INFO><VEHICLE_INFO_CONTENT><DRIVE_MODE>3</DRIVE_MODE><IS_ELECTRONIC_CTRL>N</IS_ELECTRONIC_CTRL><SIGN_TYPE>1</SIGN_TYPE><OWNERADDRESS>江苏省滨海县蔡桥镇</OWNERADDRESS><NEAR_UNIT_ID>320900499</NEAR_UNIT_ID><CYLINDER>null</CYLINDER><CLXH>CC6460KM01</CLXH><FDJXH>4G64S4M</FDJXH><PHONE>13851110057</PHONE><FACTORY_NAME>长城汽车股份有限公司</FACTORY_NAME><NEAR_CHECK_RESULT>1</NEAR_CHECK_RESULT><STROKE>4</STROKE><SUPPLY_MODE>4</SUPPLY_MODE><OWNER>李文平</OWNER><VEHICLE_ID>32090017013406</VEHICLE_ID><ADMISSION>1</ADMISSION><MANUFACTURE_DATE>2006-01-01</MANUFACTURE_DATE><CHECK_PERIOD>12</CHECK_PERIOD><USAGE_NATURE>A</USAGE_NATURE><CHECK_METHOD>2</CHECK_METHOD><PERIOD_END_DATE>2020-01-01 00:00:00</PERIOD_END_DATE><HAS_ODB>N</HAS_ODB><CITY_CODE>320900</CITY_CODE><STDWEIGHT>1820</STDWEIGHT><CLSB>长城</CLSB><ORDAIN_REV>3000</ORDAIN_REV><REGISTER_DATE>2006-01-05</REGISTER_DATE><VEHICLE_TYPE>K31</VEHICLE_TYPE><STANDARD_ID>2</STANDARD_ID><DRIVE_FORM>3</DRIVE_FORM><SIGN_STATE>null</SIGN_STATE><ODO_METER>222547</ODO_METER><NEAR_CHECK_DATE>2020-01-01 14:01:04</NEAR_CHECK_DATE><EXHAUST_QUANTITY>2.4</EXHAUST_QUANTITY><SEAT_CAPACITY>5</SEAT_CAPACITY><MAXWEIGHT>2195</MAXWEIGHT><RATING_POWER>93</RATING_POWER><LOGIN_UNIT_ID>320900499</LOGIN_UNIT_ID><FUEL_TYPE>A</FUEL_TYPE><VIN>LGWEF3A535B005065</VIN><HAS_PURGE>N</HAS_PURGE><ENGINE_NO>SCG1507</ENGINE_NO><PLATE>苏JTJ482</PLATE><PLATE_COLOR>1</PLATE_COLOR><PLATE_TYPE>02</PLATE_TYPE></VEHICLE_INFO_CONTENT></VEHICLE_INFO></xml>";


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
                        dr["CLID"] = node4.SelectSingleNode("VEHICLE_ID").InnerText;
                        dr["jylsh"] = LSH;
                        dr["jycs"] = "1";// child1.insp.TestTimes;
                        string Hphm = node4.SelectSingleNode("PLATE").InnerText;
                        dr["hphm"] = node4.SelectSingleNode("PLATE").InnerText;
                        dr["hpys"] = "";// GetCpys(node4.SelectSingleNode("PLATE_COLOR").InnerText);
                        dr["hpzl"] = "";// GetHpzl(node4.SelectSingleNode("PLATE_TYPE").InnerText);
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
                        syxzstr = "";// GetSyxz(syxzcode);


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
                        dr["pfbz"] = "";
                        dr["bsqxs"] = "";// GetBsxxs(node4.SelectSingleNode("DRIVE_FORM").InnerText);
                        dr["jqfs"] = ""; //node4.SelectSingleNode("ADDMISSION").InnerText;

                        string ryzlcode = string.Empty;
                        string ryzlstr = string.Empty;
                        ryzlcode = node4.SelectSingleNode("FUEL_TYPE").InnerText;
                        ryzlstr = "";// GetRllx(ryzlcode);

                        dr["ryzl"] = ryzlstr;

                        string gyfs = node4.SelectSingleNode("SUPPLY_MODE").InnerText.ToString().Trim();
                        dr["gyfs"] = "";// GetGyfs(gyfs);

                        dr["qdfs"] = "";// GetQdxs(node4.SelectSingleNode("DRIVE_MODE").InnerText);
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

                        dr["jcff"] = "";// GetJcff(node4.SelectSingleNode("CHECK_METHOD").InnerText);

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

        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel_login.Visible = true;
            panel_config.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FileOpreate.xcPDAurl = textBoxXcPDAUrl.Text;
            FileOpreate.getPicFromXcPDA = checkBoxGetPicFromXcPDA.Checked;
            FileOpreate.WritePrivateProfileString("xcPDA", "接口地址", FileOpreate.xcPDAurl, @"./Config.ini");
            FileOpreate.WritePrivateProfileString("xcPDA", "是否使用", FileOpreate.getPicFromXcPDA?"Y":"N", @"./Config.ini");

            MessageBox.Show("PDA信息设置保存成功！", "保存成功");
        }
    }
}
