using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace DataUpdate
{
    //数据库相关操作
    public class DBControl
    {
        #region 用户及检测线信息
        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="Users">用户信息</param>
        /// <returns></returns>
        public bool getUserInfoByName(string username,out DataRow Users)
        {
            Users = null;
            string sql= "select * from [员工] where NAME = '" + username + "'";
            try
            {
                DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                if (dt.Rows.Count > 0)
                    Users = dt.Rows[0];
                return true;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行查询错误，原因：" + er.Message, "用户查询", 2);
                return false;
            }
        }

        /// <summary>
        /// 获取检测线信息
        /// </summary>
        /// <returns></returns>
        public DataTable getLineInfo()
        {
            string sql = "select * from [stationLine]";
            try
            {
                DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                return dt;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行查询错误，原因：" + er.Message, "检测线信息查询", 2);
                return null;
            }
        }
        #endregion

        #region 待检车辆信息表操作
        /// <summary>
        /// 清空待检列表
        /// </summary>
        /// <returns></returns>
        public bool clearCarWaitList()
        {
            string sql = "truncate table [待检车辆]";
            try
            {
                DBHelperSQL.Execute(sql);
                return true;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行清空待检列表失败，原因：" + er.Message, "待检车辆", 2);
                return false;
            }
        }

        /// <summary>
        /// 根据车牌号查询待检车辆表中是否存在车辆信息
        /// </summary>
        /// <param name="clhp">车牌号</param>
        /// <returns></returns>
        public bool getCarInWaitListByCLHP(string clhp)
        {
            string sql = "select * from [待检车辆] where CLHP='" + clhp + "'";
            try
            {
                DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                if (dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行查询待检车辆失败，原因：" + er.Message, "待检车辆", 2);
                return false;
            }
        }

        /// <summary>
        /// 获取整个待检车辆表
        /// </summary>
        /// <param name="isNet">是否为联网检测（Y为联网检测，N为单机）</param>
        /// <returns></returns>
        public DataTable getCarInWaitList( string isNet)
        {
            string sql = "select * from [待检车辆] where TEST = '" + isNet + "' order by DLSJ desc";
            DataTable dt = null;
            try
            {
                dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                return dt;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行获取待检列表失败，原因：" + er.Message, "待检车辆", 2);
                return null;
            }
        }
        public DataTable getXcOutLookCheck(string id)
        {
            string sql = "select * from OutLookCheck where ID = '" + id + "' order by CheckTime desc";
            DataTable dt = null;
            try
            {
                dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                return dt;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行获取外检记录失败，原因：" + er.Message, "外检记录", 2);
                return null;
            }
        }
        public DataTable getOutLookCheck(string jylsh)
        {
            string sql = "select * from OutLookCheck where BY1 = '" + jylsh + "' order by CheckTime desc";
            DataTable dt = null;
            try
            {
                dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                return dt;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行获取外检记录失败，原因：" + er.Message, "外检记录", 2);
                return null;
            }
        }
        /// <summary>
        /// 根据流水号及检验次数获取待检车辆表中数据
        /// </summary>
        /// <param name="jylsh">流水号</param>
        /// <param name="jccs">检验次数</param>
        /// <param name="CarTestStatus">查到的车辆信息</param>
        /// <returns></returns>
        public bool getCarWaitInfoByLsh(string jylsh, string jccs, out DataRow CarTestStatus)
        {
            CarTestStatus = null;
            string sql = "select * from [待检车辆] where JYLSH='" + jylsh + "' and JCCS='" + jccs + "'";
            try
            {
                DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                if (dt.Rows.Count > 0)
                {
                    CarTestStatus = dt.Rows[0];
                    return true;
                }
                else
                    return false;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行获取待检车辆信息失败，原因：" + er.Message, "待检车辆", 2);
                return false;
            }
        }

        /// <summary>
        /// 向待检车辆表中插入一行
        /// </summary>
        /// <param name="CLID">车辆检测ID号</param>
        /// <param name="JCZBH">检测站编号</param>
        /// <param name="JYLSH">检验流水号</param>
        /// <param name="JCCS">检测次数</param>
        /// <param name="CLHP">车牌号</param>
        /// <param name="HPZL">号牌种类</param>
        /// <param name="JCFF">检测方法</param>
        /// <param name="DLSJ">登录时间</param>
        /// <param name="ZT">状态</param>
        /// <returns></returns>
        public bool addCarToWaitList(string CLID, string JYLSH, string JCCS, string CLHP, string CPYS, string HPZL, string XSLC, string JCFF, string JCZBH, string DLSJ, string DLY)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into [待检车辆] (");
                strSql.Append("CLID,");//clph+T+time
                strSql.Append("JYLSH,");
                strSql.Append("JCCS,");
                strSql.Append("CLHP,");
                strSql.Append("CPYS,");
                strSql.Append("HPZL,");
                strSql.Append("XSLC,");
                strSql.Append("JCFF,");
                strSql.Append("JCZBH,");
                strSql.Append("DLSJ,");
                strSql.Append("JCRQ,");
                strSql.Append("WXSJ,");
                strSql.Append("DLY,");
                strSql.Append("ZT,");
                strSql.Append("IsOutlookChecked,");
                strSql.Append("TEST)");

                strSql.Append("values (@CLID,@JYLSH,@JCCS,@CLHP,@CPYS,@HPZL,@XSLC,@JCFF,@JCZBH,@DLSJ,@JCRQ,@WXSJ,@DLY,@ZT,@IsOutlookChecked,@TEST)");
                SqlParameter[] parameters = {
                    new SqlParameter("@CLID", SqlDbType.VarChar,50),
                    new SqlParameter("@JYLSH",SqlDbType.VarChar,50),
                    new SqlParameter("@JCCS", SqlDbType.VarChar,50),
                    new SqlParameter("@CLHP", SqlDbType.VarChar,50),
                    new SqlParameter("@CPYS", SqlDbType.VarChar,50),
                    new SqlParameter("@HPZL", SqlDbType.VarChar,50),
                    new SqlParameter("@XSLC", SqlDbType.VarChar,50),
                    new SqlParameter("@JCFF", SqlDbType.VarChar,50),
                    new SqlParameter("@JCZBH", SqlDbType.VarChar,50),
                    new SqlParameter("@DLSJ", SqlDbType.DateTime),
                    new SqlParameter("@JCRQ", SqlDbType.DateTime),
                    new SqlParameter("@WXSJ", SqlDbType.DateTime),
                    new SqlParameter("@DLY", SqlDbType.VarChar,50),
                    new SqlParameter("@ZT", SqlDbType.VarChar,50),
                    new SqlParameter("@IsOutlookChecked", SqlDbType.VarChar,50),
                    new SqlParameter("@TEST", SqlDbType.VarChar,50)};
                parameters[0].Value = CLID;
                parameters[1].Value = JYLSH;
                parameters[2].Value = JCCS;
                parameters[3].Value = CLHP;
                parameters[4].Value = CPYS;
                parameters[5].Value = HPZL;
                parameters[6].Value = XSLC;
                parameters[7].Value = JCFF;
                parameters[8].Value = JCZBH;
                parameters[9].Value = DateTime.Parse(DLSJ);
                parameters[10].Value = DateTime.Now;
                parameters[11].Value = DateTime.Now;
                parameters[12].Value = DLY;
                parameters[13].Value = 0;
                parameters[14].Value = "N";//江苏检外检
                parameters[15].Value = "Y";
                if (DBHelperSQL.Execute(strSql.ToString(), parameters) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行待检列表更新失败，原因：" + er.Message, "待检车辆", 2);
                return false;
            }
        }

        /// <summary>
        /// 删除待检车辆表中数据
        /// </summary>
        /// <param name="jylsh">流水号</param>
        /// <returns></returns>
        public bool deleteCarInWaitlist(string jylsh, string jycs)
        {
            string sql_check = "select * from [待检车辆] where JYLSH='" + jylsh + "' and JCCS='" + jycs + "'";
            string sql_delete = "delete from [待检车辆] where JYLSH='" + jylsh + "' and JCCS='" + jycs + "'";
            try
            {
                DataTable dt = DBHelperSQL.GetDataTable(sql_check, CommandType.Text);
                if (dt.Rows.Count > 0)
                {
                    int rows = DBHelperSQL.Execute(sql_delete);
                    if (rows > 0)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行待检列表数据删除失败，原因：" + er.Message, "待检车辆", 2);
                return false;
            }
        }

        #endregion

        #region 车辆信息表操作
        //更新车辆信息表（有时更新、无时插入）
        public bool UpdateOrInsertCarInf(CARINF model)
        {
            string sql = "select * from [车辆信息] where CLHP='" + model.CLHP + "'";
            try
            {
                DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                if (dt.Rows.Count > 0)
                {
                    if (UpdateCarInfo(model))
                        return true;
                    else
                        return false;
                }
                else
                {
                    if (insertCarInfbyPlate(model))
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行插入或更新车辆信息失败，原因：" + er.Message, "车辆信息", 2);
                return false;
            }
        }

        //更新
        public bool UpdateCarInfo(CARINF model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update [车辆信息] set ");
                strSql.Append("CPYS=@CPYS,");
                strSql.Append("CLLX=@CLLX,");
                strSql.Append("CZ=@CZ,");
                strSql.Append("SYXZ=@SYXZ,");
                strSql.Append("PP=@PP,");
                strSql.Append("XH=@XH,");
                strSql.Append("CLSBM=@CLSBM,");
                strSql.Append("FDJHM=@FDJHM,");
                strSql.Append("FDJXH=@FDJXH,");
                strSql.Append("SCQY=@SCQY,");
                strSql.Append("HDZK=@HDZK,");
                strSql.Append("JSSZK=@JSSZK,");
                strSql.Append("ZZL=@ZZL,");
                strSql.Append("HDZZL=@HDZZL,");
                strSql.Append("ZBZL=@ZBZL,");
                strSql.Append("JZZL=@JZZL,");
                strSql.Append("ZCRQ=@ZCRQ,");
                strSql.Append("SCRQ=@SCRQ,");
                strSql.Append("FDJPL=@FDJPL,");
                strSql.Append("RLZL=@RLZL,");
                strSql.Append("EDGL=@EDGL,");
                strSql.Append("EDZS=@EDZS,");
                strSql.Append("BSQXS=@BSQXS,");
                strSql.Append("DWS=@DWS,");
                strSql.Append("GYFS=@GYFS,");
                strSql.Append("DPFS=@DPFS,");
                strSql.Append("JQFS=@JQFS,");
                strSql.Append("QGS=@QGS,");
                strSql.Append("QDXS=@QDXS,");
                strSql.Append("CHZZ=@CHZZ,");
                strSql.Append("DLSP=@DLSP,");
                strSql.Append("SFSRL=@SFSRL,");
                strSql.Append("JHZZ=@JHZZ,");
                strSql.Append("OBD=@OBD,");
                strSql.Append("CCS=@CCS,");
                strSql.Append("DKGYYB=@DKGYYB,");
                strSql.Append("CZDZ=@CZDZ,");
                strSql.Append("JCFS=@JCFS,");
                strSql.Append("JCLB=@JCLB,");
                strSql.Append("CLZL=@CLZL,");
                strSql.Append("SSXQ=@SSXQ,");
                strSql.Append("SFWDZR=@SFWDZR,");
                strSql.Append("SFYQBF=@SFYQBF,");
                strSql.Append("FDJSCQY=@FDJSCQY,");
                strSql.Append("QDLTQY=@QDLTQY,");
                strSql.Append("RYPH=@RYPH,");
                strSql.Append("CSYS=@CSYS,");
                strSql.Append("HPZL=@HPZL,");
                strSql.Append("ZXBZ=@ZXBZ,");
                strSql.Append("ZDJGL=@ZDJGL,");
                strSql.Append("LXDH=@LXDH");
                strSql.Append(" where CLHP='" + model.CLHP + "'");

                SqlParameter[] parameters = {
                    new SqlParameter("@CPYS", SqlDbType.VarChar,50),
                    new SqlParameter("@CLLX",SqlDbType.VarChar,50),
                    new SqlParameter("@CZ",SqlDbType.VarChar,100),
                    new SqlParameter("@SYXZ", SqlDbType.VarChar,50),
                    new SqlParameter("@PP", SqlDbType.VarChar,50),
                    new SqlParameter("@XH", SqlDbType.VarChar,50),
                    new SqlParameter("@CLSBM", SqlDbType.VarChar,50),
                    new SqlParameter("@FDJHM", SqlDbType.VarChar,50),
                    new SqlParameter("@FDJXH", SqlDbType.VarChar,50),
                    new SqlParameter("@SCQY", SqlDbType.VarChar,100),
                    new SqlParameter("@HDZK", SqlDbType.VarChar,50),
                    new SqlParameter("@JSSZK", SqlDbType.VarChar,50),
                    new SqlParameter("@ZZL", SqlDbType.VarChar,50),
                    new SqlParameter("@HDZZL", SqlDbType.VarChar,50),
                    new SqlParameter("@ZBZL", SqlDbType.VarChar,50),
                    new SqlParameter("@JZZL", SqlDbType.VarChar,50),
                    new SqlParameter("@ZCRQ", SqlDbType.DateTime),
                    new SqlParameter("@SCRQ", SqlDbType.DateTime),
                    new SqlParameter("@FDJPL", SqlDbType.VarChar,50),
                    new SqlParameter("@RLZL", SqlDbType.VarChar,50),
                    new SqlParameter("@EDGL", SqlDbType.VarChar,50),
                    new SqlParameter("@EDZS", SqlDbType.VarChar,50),
                    new SqlParameter("@BSQXS",SqlDbType.VarChar,50),
                    new SqlParameter("@DWS", SqlDbType.VarChar,50),
                    new SqlParameter("@GYFS", SqlDbType.VarChar,50),
                    new SqlParameter("@DPFS", SqlDbType.VarChar,50),
                    new SqlParameter("@JQFS", SqlDbType.VarChar,50),
                    new SqlParameter("@QGS", SqlDbType.VarChar,50),
                    new SqlParameter("@QDXS", SqlDbType.VarChar,50),
                    new SqlParameter("@CHZZ", SqlDbType.VarChar,50),
                    new SqlParameter("@DLSP", SqlDbType.VarChar,50),
                    new SqlParameter("@SFSRL", SqlDbType.VarChar,50),
                    new SqlParameter("@JHZZ",SqlDbType.VarChar,50),
                    new SqlParameter("@OBD",SqlDbType.VarChar,50),
                    new SqlParameter("@CCS",SqlDbType.VarChar,50),
                    new SqlParameter("@DKGYYB",SqlDbType.VarChar,50),
                    new SqlParameter("@CZDZ",SqlDbType.VarChar,100),
                    new SqlParameter("@JCFS",SqlDbType.VarChar,50),
                    new SqlParameter("@JCLB",SqlDbType.VarChar,50),
                    new SqlParameter("@CLZL",SqlDbType.VarChar,50),
                    new SqlParameter("@SSXQ",SqlDbType.VarChar,50),
                    new SqlParameter("@SFWDZR",SqlDbType.VarChar,50),
                    new SqlParameter("@SFYQBF",SqlDbType.VarChar,50),
                    new SqlParameter("@FDJSCQY",SqlDbType.VarChar,100),
                    new SqlParameter("@QDLTQY",SqlDbType.VarChar,50),
                    new SqlParameter("@RYPH",SqlDbType.VarChar,50),
                    new SqlParameter("@CSYS",SqlDbType.VarChar,50),
                    new SqlParameter("@HPZL",SqlDbType.VarChar,50),
                    new SqlParameter("@ZXBZ",SqlDbType.VarChar,50),
                    new SqlParameter("@ZDJGL",SqlDbType.VarChar,50),
                    new SqlParameter("@LXDH",SqlDbType.VarChar,50)};
                int i = 0;
                parameters[i++].Value = model.CPYS;
                parameters[i++].Value = model.CLLX;
                parameters[i++].Value = model.CZ;
                parameters[i++].Value = model.SYXZ;
                parameters[i++].Value = model.PP;
                parameters[i++].Value = model.XH;
                parameters[i++].Value = model.CLSBM;
                parameters[i++].Value = model.FDJHM;
                parameters[i++].Value = model.FDJXH;
                parameters[i++].Value = model.SCQY;
                parameters[i++].Value = model.HDZK;
                parameters[i++].Value = "1";
                parameters[i++].Value = model.ZZL;
                parameters[i++].Value = "0";
                parameters[i++].Value = model.ZBZL;
                parameters[i++].Value = model.JZZL;
                parameters[i++].Value = model.ZCRQ;
                parameters[i++].Value = model.SCRQ;
                parameters[i++].Value = model.FDJPL;
                parameters[i++].Value = model.RLZL;
                parameters[i++].Value = model.EDGL;
                parameters[i++].Value = model.EDZS;
                parameters[i++].Value = model.BSQXS;
                parameters[i++].Value = model.DWS;
                parameters[i++].Value = model.GYFS;
                parameters[i++].Value = "";
                parameters[i++].Value = model.JQFS;
                parameters[i++].Value = model.QGS;
                parameters[i++].Value = model.QDXS;
                parameters[i++].Value = model.CHZZ;
                parameters[i++].Value = "否";
                parameters[i++].Value = "否";
                parameters[i++].Value = model.JHZZ;
                parameters[i++].Value = model.OBD;
                parameters[i++].Value = model.CCS;
                parameters[i++].Value = "否";
                parameters[i++].Value = model.CZDZ;
                parameters[i++].Value = "0";
                parameters[i++].Value = model.JCLB;
                parameters[i++].Value = "";
                parameters[i++].Value = model.SSXQ;
                parameters[i++].Value = "否";
                parameters[i++].Value = "否";
                parameters[i++].Value = model.FDJSCQY;
                parameters[i++].Value = model.QDLTQY;
                parameters[i++].Value = model.RYPH;
                parameters[i++].Value = "";
                parameters[i++].Value = model.HPZL;
                parameters[i++].Value = model.ZXBZ;
                parameters[i++].Value = model.ZDJGL;
                parameters[i++].Value = model.LXDH;

                int rows = DBHelperSQL.Execute(strSql.ToString(), parameters);
                if (rows > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行更新车辆信息失败，原因：" + er.Message, "车辆信息", 2);
                return false;
            }
        }

        //插入
        public bool insertCarInfbyPlate(CARINF model)
        {
            try
            {
                int i = 0;
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into [车辆信息] (");
                strSql.Append("CLHP,");
                strSql.Append("CPYS,");
                strSql.Append("CLLX,");
                strSql.Append("CZ,");
                strSql.Append("SYXZ,");
                strSql.Append("PP,");
                strSql.Append("XH,");
                strSql.Append("CLSBM,");
                strSql.Append("FDJHM,");
                strSql.Append("FDJXH,");
                strSql.Append("SCQY,");
                strSql.Append("HDZK,");
                strSql.Append("JSSZK,");
                strSql.Append("ZZL,");
                strSql.Append("HDZZL,");
                strSql.Append("ZBZL,");
                strSql.Append("JZZL,");
                strSql.Append("ZCRQ,");
                strSql.Append("SCRQ,");
                strSql.Append("FDJPL,");
                strSql.Append("RLZL,");
                strSql.Append("EDGL,");
                strSql.Append("EDZS,");
                strSql.Append("BSQXS,");
                strSql.Append("DWS,");
                strSql.Append("GYFS,");
                strSql.Append("DPFS,");
                strSql.Append("JQFS,");
                strSql.Append("QGS,");
                strSql.Append("QDXS,");
                strSql.Append("CHZZ,");
                strSql.Append("DLSP,");
                strSql.Append("SFSRL,");
                strSql.Append("JHZZ,");
                strSql.Append("OBD,");
                strSql.Append("CCS,");
                strSql.Append("DKGYYB,");
                strSql.Append("CZDZ,");
                strSql.Append("JCFS,");
                strSql.Append("JCLB,");
                strSql.Append("CLZL,");
                strSql.Append("SSXQ,");
                strSql.Append("SFWDZR,");
                strSql.Append("SFYQBF,");
                strSql.Append("FDJSCQY,");
                strSql.Append("QDLTQY,");
                strSql.Append("RYPH,");
                strSql.Append("ZXBZ,");
                strSql.Append("HPZL,");
                strSql.Append("ZDJGL,");
                strSql.Append("LXDH)");
                strSql.Append("values (@CLHP,@CPYS,@CLLX,@CZ,@SYXZ,@PP,@XH,@CLSBM,@FDJHM,@FDJXH,@SCQY,@HDZK,@JSSZK,@ZZL,@HDZZL,@ZBZL,@JZZL,@ZCRQ,@SCRQ,@FDJPL,@RLZL,@EDGL,@EDZS,@BSQXS,@DWS,@GYFS,@DPFS,@JQFS,@QGS,@QDXS,@CHZZ,@DLSP,@SFSRL,@JHZZ,@OBD,@CCS,@DKGYYB,@CZDZ,@JCFS,@JCLB,@CLZL,@SSXQ,@SFWDZR,@SFYQBF,@FDJSCQY,@QDLTQY,@RYPH,@ZXBZ,@HPZL,@ZDJGL,@LXDH)");
                SqlParameter[] parameters = {
                    new SqlParameter("@CLHP", SqlDbType.VarChar,50),
                    new SqlParameter("@CPYS", SqlDbType.VarChar,50),
                    new SqlParameter("@CLLX",SqlDbType.VarChar,50),
                    new SqlParameter("@CZ",SqlDbType.VarChar,100),
                    new SqlParameter("@SYXZ", SqlDbType.VarChar,50),
                    new SqlParameter("@PP", SqlDbType.VarChar,50),
                    new SqlParameter("@XH", SqlDbType.VarChar,50),
                    new SqlParameter("@CLSBM", SqlDbType.VarChar,50),
                    new SqlParameter("@FDJHM", SqlDbType.VarChar,50),
                    new SqlParameter("@FDJXH", SqlDbType.VarChar,50),
                    new SqlParameter("@SCQY", SqlDbType.VarChar,100),
                    new SqlParameter("@HDZK", SqlDbType.VarChar,50),
                    new SqlParameter("@JSSZK", SqlDbType.VarChar,50),
                    new SqlParameter("@ZZL", SqlDbType.VarChar,50),
                    new SqlParameter("@HDZZL", SqlDbType.VarChar,50),
                    new SqlParameter("@ZBZL", SqlDbType.VarChar,50),
                    new SqlParameter("@JZZL", SqlDbType.VarChar,50),
                    new SqlParameter("@ZCRQ", SqlDbType.DateTime),
                    new SqlParameter("@SCRQ", SqlDbType.DateTime),
                    new SqlParameter("@FDJPL", SqlDbType.VarChar,50),
                    new SqlParameter("@RLZL", SqlDbType.VarChar,50),
                    new SqlParameter("@EDGL", SqlDbType.VarChar,50),
                    new SqlParameter("@EDZS", SqlDbType.VarChar,50),
                    new SqlParameter("@BSQXS",SqlDbType.VarChar,50),
                    new SqlParameter("@DWS", SqlDbType.VarChar,50),
                    new SqlParameter("@GYFS", SqlDbType.VarChar,50),
                    new SqlParameter("@DPFS", SqlDbType.VarChar,50),
                    new SqlParameter("@JQFS", SqlDbType.VarChar,50),
                    new SqlParameter("@QGS", SqlDbType.VarChar,50),
                    new SqlParameter("@QDXS", SqlDbType.VarChar,50),
                    new SqlParameter("@CHZZ", SqlDbType.VarChar,50),
                    new SqlParameter("@DLSP", SqlDbType.VarChar,50),
                    new SqlParameter("@SFSRL", SqlDbType.VarChar,50),
                    new SqlParameter("@JHZZ",SqlDbType.VarChar,50),
                    new SqlParameter("@OBD",SqlDbType.VarChar,50),
                    new SqlParameter("@CCS",SqlDbType.VarChar,50),
                    new SqlParameter("@DKGYYB",SqlDbType.VarChar,50),
                    new SqlParameter("@CZDZ",SqlDbType.VarChar,100),
                    new SqlParameter("@JCFS",SqlDbType.VarChar,50),
                    new SqlParameter("@JCLB",SqlDbType.VarChar,50),
                    new SqlParameter("@CLZL",SqlDbType.VarChar,50),
                    new SqlParameter("@SSXQ",SqlDbType.VarChar,50),
                    new SqlParameter("@SFWDZR",SqlDbType.VarChar,50),
                    new SqlParameter("@SFYQBF",SqlDbType.VarChar,50),
                    new SqlParameter("@FDJSCQY",SqlDbType.VarChar,100),
                    new SqlParameter("@QDLTQY",SqlDbType.VarChar,50),
                    new SqlParameter("@RYPH",SqlDbType.VarChar,50),
                    new SqlParameter("@ZXBZ",SqlDbType.VarChar,50),
                    new SqlParameter("@HPZL",SqlDbType.VarChar,50),
                    new SqlParameter("@ZDJGL",SqlDbType.VarChar,50),
                    new SqlParameter("@LXDH",SqlDbType.VarChar,50)};
                parameters[i++].Value = model.CLHP;
                parameters[i++].Value = model.CPYS;
                parameters[i++].Value = model.CLLX;
                parameters[i++].Value = model.CZ;
                parameters[i++].Value = model.SYXZ;
                parameters[i++].Value = model.PP;
                parameters[i++].Value = model.XH;
                parameters[i++].Value = model.CLSBM;
                parameters[i++].Value = model.FDJHM;
                parameters[i++].Value = model.FDJXH;
                parameters[i++].Value = model.SCQY;
                parameters[i++].Value = model.HDZK;
                parameters[i++].Value = "1";
                parameters[i++].Value = model.ZZL;
                parameters[i++].Value = "0";
                parameters[i++].Value = model.ZBZL;
                parameters[i++].Value = model.JZZL;
                parameters[i++].Value = model.ZCRQ;
                parameters[i++].Value = model.SCRQ;
                parameters[i++].Value = model.FDJPL;
                parameters[i++].Value = model.RLZL;
                parameters[i++].Value = model.EDGL;
                parameters[i++].Value = model.EDZS;
                parameters[i++].Value = model.BSQXS;
                parameters[i++].Value = model.DWS;
                parameters[i++].Value = model.GYFS;
                parameters[i++].Value = "";
                parameters[i++].Value = model.JQFS;
                parameters[i++].Value = model.QGS;
                parameters[i++].Value = model.QDXS;
                parameters[i++].Value = model.CHZZ;
                parameters[i++].Value = "否";
                parameters[i++].Value = "否";
                parameters[i++].Value = model.JHZZ;
                parameters[i++].Value = model.OBD;
                parameters[i++].Value = model.CCS;
                parameters[i++].Value = "否";
                parameters[i++].Value = model.CZDZ;
                parameters[i++].Value = "0";
                parameters[i++].Value = model.JCLB;
                parameters[i++].Value = "";
                parameters[i++].Value = model.SSXQ;
                parameters[i++].Value = "否";
                parameters[i++].Value = "否";
                parameters[i++].Value = model.FDJSCQY;
                parameters[i++].Value = model.QDLTQY;
                parameters[i++].Value = model.RYPH;
                parameters[i++].Value = model.ZXBZ;
                parameters[i++].Value = model.HPZL;
                parameters[i++].Value = model.ZDJGL;
                parameters[i++].Value = model.LXDH;

                if (DBHelperSQL.Execute(strSql.ToString(), parameters) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行插入车辆信息失败，原因：" + er.Message, "车辆信息", 2);
                return false;
            }
        }

        //校验车辆信息表中是否已存在该车
        public bool checkCarInfIsAlive(string plateNumber)
        {
            try
            {
                string sql = "select * from [车辆信息] where CLHP='" + plateNumber + "'";
                DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                if (dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行查询车辆信息失败，原因：" + er.Message, "车辆信息", 2);
                return false;
            }
        }
        #endregion

        #region 车辆检测状态表操作
        /// <summary>
        /// 清空车辆检测状态
        /// </summary>
        /// <returns></returns>
        public bool clearCarTestStatus()
        {
            try
            {
                string sql = "truncate table [车辆检测状态]";
                DBHelperSQL.Execute(sql);
                return true;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行清空车辆检测状态失败，原因：" + er.Message, "车辆检测状态", 2);
                return false;
            }
        }

        /// <summary>
        /// 根据检验流水号及检验次数查询车辆检测状态
        /// </summary>
        /// <param name="jylsh">流水号</param>
        /// <param name="CarTestStatus">输出车辆检测状态</param>
        /// <returns>是否成功</returns>
        public DataRow getCarTestStatusByLsh(string jylsh, string jycs)
        {
            try
            {
                string sql = "select * from [车辆检测状态] where JYLSH='" + jylsh + "' and JYCS ='" + jycs + "'";
                DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0];
                }
                else
                    return null;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行查询车辆检测状态失败，原因：" + er.Message, "车辆检测状态", 2);
                return null;
            }
        }
        
        /// <summary>
        /// 获取需要上传车辆的数据 即获取YCLZT不为-1的记录
        /// </summary>
        /// <returns></returns>
        public DataTable getCarTestStatus()
        {
            try
            {
                string sql = "select * from [车辆检测状态] where YCLZT != '-1' or YCLZT is null order by JCSJ desc";
                DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                return dt;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行获取全部车辆检测状态失败，原因：" + er.Message, "车辆检测状态", 2);
                return null;
            }
        }
        /// <summary>
        /// 获取上传失败的数据，即获取YCLZT为-1的记录
        /// </summary>
        /// <returns></returns>
        public DataTable getCarUploadFailed()
        {
            try
            {
                string sql = "select * from [车辆检测状态] where YCLZT='-1' order by JCSJ desc";
                DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                return dt;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行获取全部车辆检测状态失败，原因：" + er.Message, "车辆检测状态", 2);
                return null;
            }
        }

        /// <summary>
        /// 根据检验流水号、检验次数更新车辆检测状态表中的已处理状态列
        /// </summary>
        /// <param name="yclzt">更新后的已处理状态值</param>
        /// <param name="jylsh">检验流水号</param>
        /// <param name="jycs">检验次数</param>
        /// <returns>是否成功</returns>
        public bool UpdateCarTestStatusYCLZT(string yclzt, string jylsh, string jycs)
        {
            try
            {
                string sql = "update [车辆检测状态] set YCLZT='" + yclzt + "' where JYLSH='" + jylsh + "' and JYCS='" + jycs + "'";
                int rows = DBHelperSQL.Execute(sql);
                if (rows > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行更新YCLZT车辆检测状态失败，原因：" + er.Message, "车辆检测状态", 2);
                return false;
            }
        }
        public bool UpdateCarTestStatusYCLZT(string yclzt, string jylsh, string jycs,string bz)
        {
            try
            {
                string sql = "update [车辆检测状态] set YCLZT='" + yclzt + "',BZ='"+bz+"' where JYLSH='" + jylsh + "' and JYCS='" + jycs + "'";
                int rows = DBHelperSQL.Execute(sql);
                if (rows > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行更新YCLZT车辆检测状态失败，原因：" + er.Message, "车辆检测状态", 2);
                return false;
            }
        }
        /// <summary>
        /// 根据检验流水号、检验次数更新车辆检测状态表中的状态列
        /// </summary>
        /// <param name="zt">更新后的状态值</param>
        /// <param name="jylsh">检验流水号</param>
        /// <param name="jycs">检验次数</param>
        /// <param name="errorinfo">备注信息</param>
        /// <returns>是否成功</returns>
        public bool UpdateCarTestStatusZT(string zt, string jylsh, string jycs, string errorinfo)
        {
            try
            {
                string sql = "update [车辆检测状态] set ZT='" + zt + "' , BZ='" + errorinfo + "' where JYLSH='" + jylsh + "' and JYCS ='" + jycs + "'";

                if (DBHelperSQL.Execute(sql) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行更新ZT车辆检测状态失败" + zt + "|" + jylsh + "|" + jycs + "|" + errorinfo + "，原因：" + er.Message, "车辆检测状态", 2);
                return false;
            }
        }

        /// <summary>
        /// 检验完成后根据检验流水号及检验次数删除一条数据
        /// </summary>
        /// <param name="jylsh">流水号</param>
        /// <param name="jycs">检验次数</param>
        /// <returns>是否成功</returns>
        public bool DeleteCarTestStatus(string jylsh, string jycs)
        {
            try
            {
                string sql = "delete from [车辆检测状态] where JYLSH='" + jylsh + "' and JYCS='" + jycs + "'";
                
                if (DBHelperSQL.Execute(sql) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行删除车辆检测状态数据失败，原因：" + er.Message, "车辆检测状态", 2);
                return false;
            }
        }

        /// <summary>
        /// 清空检测状态表
        /// </summary>
        /// <returns></returns>
        public bool EmptyCarTestStatus()
        {
            try
            {
                string sql = "truncate table [车辆检测状态]";
                
                if (DBHelperSQL.Execute(sql) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行清空车辆检测状态数据失败，原因：" + er.Message, "车辆检测状态", 2);
                return false;
            }
        }
        #endregion

        #region 检测结果及过程数据获取
        /// <summary>
        /// 根据检验流水号及检验次数获取检测结果
        /// </summary>
        /// <param name="jcff_table">要查询的检测方法表（字符串），ASM、JZJS、SDS、VMAS、Zyjs_Btg</param>
        /// <param name="jylsh">检验流水号</param>
        /// <param name="TestResult">查询结果</param>
        /// <returns></returns>
        public bool getTestResult(string table_name, string jylsh, string jycs, out DataRow TestResult)
        {
            TestResult = null;
            try
            {
                string sql = "select * from [" + table_name + "] where JYLSH='" + jylsh + "' and JYCS='" + jycs + "' order by JCRQ desc";
                DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                if (dt.Rows.Count > 0)
                {
                    TestResult = dt.Rows[0];
                    return true;
                }
                else
                    return false;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行"+ table_name + "结果数据查询失败，原因：" + er.Message, table_name, 2);
                return false;
            }
        }
        public bool getProcessData(string table_name, string jylsh, string jycs, out DataRow TestResult)
        {
            TestResult = null;
            try
            {
                string sql = "select * from [" + table_name + "] where JYLSH='" + jylsh + "' and JYCS='" + jycs + "' order by JCSJ desc";
                DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                if (dt.Rows.Count > 0)
                {
                    TestResult = dt.Rows[0];
                    return true;
                }
                else
                    return false;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行" + table_name + "结果数据查询失败，原因：" + er.Message, table_name, 2);
                return false;
            }
        }
        public bool getVehicleData(string table_name, string jylsh, string jycs, out DataRow TestResult)
        {
            TestResult = null;
            try
            {
                string sql = "select * from [" + table_name + "] where JYLSH='" + jylsh + "' and JCCS='" + jycs + "' order by JCSJ desc";
                DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                if (dt.Rows.Count > 0)
                {
                    TestResult = dt.Rows[0];
                    return true;
                }
                else
                    return false;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行" + table_name + "结果数据查询失败，原因：" + er.Message, table_name, 2);
                return false;
            }
        }
        /// <summary>
        /// 获取 OBD检测记录
        /// </summary>
        /// <param name="clid"></param>
        /// <param name="TestResult"></param>
        /// <returns></returns>
        public bool getOBDResult(string clid, out DataRow TestResult)
        {
            TestResult = null;
            try
            {
                string sql = "select * from OBD where CLID='" + clid+"'";
                DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                if (dt.Rows.Count > 0)
                {
                    TestResult = dt.Rows[0];
                    return true;
                }
                else
                    return false;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("获取OBD结果数据查询失败，原因：" + er.Message, "OBD", 2);
                return false;
            }
        }

        /// <summary>
        /// 根据流水号和次数获取车辆ID
        /// </summary>
        /// <param name="clid"></param>
        /// <param name="TestResult"></param>
        /// <returns></returns>
        public string GetClid(string slh, string jycs)
        {
            try
            {
                string sql = "select * from 已检车辆信息 where SLH='" + slh + "' and JCCS='"+ jycs + "' ";
                DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["CLID"].ToString().Trim();
                }
                else
                    return "";
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("获取车辆id数据查询失败，原因：" + er.Message, "CLID", 2);
                return "";
            }
        }
        public bool UpdateJCBGBH(string reallsh,string lsh, string jycs)
        {
            try
            {
                string sql = "update [已检车辆信息] set LSH='"+reallsh+"' where JYLSH='"+lsh + "' and JCCS='" + jycs + "'";
                DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("更新["+lsh+"|"+jycs+"]报告单编号失败，原因：" + er.Message,"更新报告单编号失败",1);
                return false;
            }
        }


        /// <summary>
        /// 根据检验流水号及检验次数获取车辆检测过程数据
        /// </summary>
        /// <param name="table_name">过程数据表</param>
        /// <param name="jylsh">车辆ID</param>
        /// <param name="jycs">车辆ID</param>
        /// <param name="ProcessData">过程数据</param>
        /// <returns></returns>
        /*public bool getProcessData(string table_name, string jylsh, string jycs, out DataRow ProcessData)
        {
            ProcessData = null;
            try
            {
                string sql = "select * from [" + table_name + "] where JYLSH='" + jylsh + "' and JYCS='" + jycs + "' order by JCSJ desc";
                DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                if (dt.Rows.Count > 0)
                {
                    ProcessData = dt.Rows[0];
                    return true;
                }
                else
                    return false;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行" + table_name + "过程数据查询失败，原因：" + er.Message, table_name, 2);
                return false;
            }
        }*/
        #endregion

        #region 标定结果及自检结果表操作
        //获取标定数据
        public DataTable getBDData(int maxUploadTimes)
        {
            DataTable dt = null;
            try
            {
                string today_date = System.DateTime.Now.ToString("yyyy-MM-dd");
                string sql = "select * from [设备标定数据] where BY2='1' and BDSJ between '" + today_date + " 00:00:00.000' and '" + today_date + " 23:59:59.999'" + " order by BDSJ asc";
                dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                return dt;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行获取设备标定状态失败，原因：" + er.Message, "设备标定", 2);
                return null;
            }
        }
        //更新标定数据上传状态
        public bool UpdateBDZT(string zt, string id,int uploadTimes)
        {
            try
            {
                string sql = "update [设备标定数据] set ZT = '" + zt + "', BY1='"+uploadTimes.ToString()+"' where ID = '" + id + "'";
                if (DBHelperSQL.Execute(sql) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行更新设备标定状态失败，原因：" + er.Message, "设备标定", 2);
                return false;
            }
        }
        //获取自检数据
        public DataTable getSelfCheckData(int maxUploadTimes)
        {
            try
            {
                string today_date = System.DateTime.Now.ToString("yyyy-MM-dd");
                string sql = "select * from [设备自检数据] where BY2='1' and ZJSJ between '" + today_date + " 00:00:00.000' and '" + today_date + " 23:59:59.999'" + " order by ZJSJ asc";
                DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                return dt;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行获取设备自检状态失败，原因：" + er.Message, "设备自检", 2);
                return null;
            }
        }
        //更新自检数据上传状态
        public bool UpdateSelfCheckZT(string zt, string id, int uploadTimes)
        {
            try
            {
                string sql = "update [设备自检数据] set ZT = '" + zt + "', BY1='" + uploadTimes.ToString() + "' where ID = '" + id+ "'";
                if (DBHelperSQL.Execute(sql) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行更新设备自检状态失败，原因：" + er.Message, "设备自检", 2);
                return false;
            }
        }
        public DataRow getZjBdProcessData(string id)
        {
            try
            {
                string today_date = System.DateTime.Now.ToString("yyyy-MM-dd");
                string sql = "select * from [设备自检过程数据] where ID='" + id + "'";
                DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                if (dt != null && dt.Rows.Count > 0)
                    return dt.Rows[0];
                else
                    return null;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行获取设备自检过程数据，原因：" + er.Message, "设备自检", 2);
                return null;
            }
        }
        #endregion

        #region 升级数据库
        public bool updateSQL()
        {
            try
            {
                string sql = "alter table[车辆检测状态] alter column BZ varchar(max)";
                DBHelperSQL.Execute(sql);
                return true;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog("执行数据库升级失败，原因：" + er.Message, "数据库升级", 2);
                return false;
            }
        }
        #endregion
    }

    /// ConnDbForOracle 的摘要说明。
    public class ConnForSQL
    {
        SqlConnection Connection;

        /// <summary>
        /// 初始化数据库连接字符串
        /// </summary>
        /// <returns>string</returns>
        public static string Init_DBlink()
        {
            string constr_temp = "";
            if (FileOpreate.DB_Address == "" || FileOpreate.DB_Name == "" || FileOpreate.DB_Pwd == "" || FileOpreate.DB_User == "")
            {
                if (FileOpreate.GetDBConfig() == false || FileOpreate.DB_Address == "" || FileOpreate.DB_Name == "" || FileOpreate.DB_Pwd == "" || FileOpreate.DB_User == "")
                    throw new Exception("数据库服务器配置不正确或未配置");
            }
            constr_temp = "Data Source=" + FileOpreate.DB_Address + ";Initial Catalog=" + FileOpreate.DB_Name + ";Persist Security Info=True;User ID=" + FileOpreate.DB_User + ";password=" + FileOpreate.DB_Pwd;
            //DB_Link_Test_DAL(constr_temp);
            return constr_temp;
        }

        
        #region 带参数的构造函数 
        /// 带参数的构造函数
        /// 数据库联接字符串
        public ConnForSQL()
        {
            string connStr;
            connStr = Init_DBlink();
            Connection = new SqlConnection();
            Connection.ConnectionString = connStr;
            //Connection.Open();
        }
        #endregion

        #region 打开数据库 
        /// 打开数据库
        public void OpenConn()
        {
            if (this.Connection.State != ConnectionState.Open)
                this.Connection.Open();
        }
        #endregion
        #region 关闭数据库联接 
        /// 关闭数据库联接
        public void CloseConn()
        {
            if (Connection.State == ConnectionState.Open)
                Connection.Close();
        }
        #endregion

        #region 执行SQL语句，返回数据到DataSet中
        /// 执行SQL语句，返回数据到DataSet中
        /// sql语句
        /// 自定义返回的DataSet表名
        /// 返回DataSet
        public DataSet ReturnDataSet(string sql, string DataSetName)
        {
            DataSet dataSet = new DataSet();
            OpenConn();
            SqlDataAdapter OraDA = new SqlDataAdapter(sql, Connection);
            OraDA.Fill(dataSet, DataSetName);
            CloseConn();
            return dataSet;
        }
        #endregion

        #region 执行SQL语句，返回数据到DataSet中
        /// 执行SQL语句，返回数据到DataSet中
        /// sql语句
        /// 自定义返回的DataSet表名
        /// 返回DataSet
        public DataSet ReturnDataSet(string sql)
        {
            DataSet dataSet = new DataSet();
            OpenConn();
            SqlDataAdapter OraDA = new SqlDataAdapter(sql, Connection);
            OraDA.Fill(dataSet);
            CloseConn();
            return dataSet;
        }
        #endregion

        #region 执行Sql语句,返回带分页功能的dataset 
        /// 执行Sql语句,返回带分页功能的dataset
        /// Sql语句
        /// 每页显示记录数
        /// <当前页/param>
        /// 返回dataset表名
        /// 返回DataSet
        public DataSet ReturnDataSet(string sql, int PageSize, int CurrPageIndex, string DataSetName)
        {
            DataSet dataSet = new DataSet();
            OpenConn();
            SqlDataAdapter OraDA = new SqlDataAdapter(sql, Connection);
            OraDA.Fill(dataSet, PageSize * (CurrPageIndex - 1), PageSize, DataSetName);
            CloseConn();
            return dataSet;
        }
        #endregion

        #region 执行SQL语句，返回 DataReader,用之前一定要先.read()打开,然后才能读到数据
        /// 执行SQL语句，返回 DataReader,用之前一定要先.read()打开,然后才能读到数据 
        /// sql语句
        /// 返回一个OracleDataReader
        public string ReturnDataReader(String sql)
        {

            string datareaderstring = "";
            try
            {
                OpenConn();
                SqlCommand command = new SqlCommand(sql, Connection);
                SqlDataReader datareader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (datareader.Read())
                {
                    datareaderstring = System.Text.Encoding.GetEncoding("GB2312").GetString((byte[])datareader[0]);
                }
                CloseConn();
            }
            catch (Exception er)
            {
            }
            return datareaderstring;
        }
        #endregion


        #region 执行SQL语句，返回记录总数数
        /// 执行SQL语句，返回记录总数数
        /// sql语句
        /// 返回记录总条数
        public int GetRecordCount(string sql)
        {
            int recordCount = 0;
            OpenConn();
            SqlCommand command = new SqlCommand(sql, Connection);
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                recordCount++;
            }
            dataReader.Close();
            CloseConn();
            return recordCount;
        }
        #endregion

        #region 取当前序列,条件为seq.nextval或seq.currval
        /// 
        /// 取当前序列
        public decimal GetSeq(string seqstr)
        {
            decimal seqnum = 0;
            string sql = "select " + seqstr + " from dual";
            OpenConn();
            SqlCommand command = new SqlCommand(sql, Connection);
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                seqnum = decimal.Parse(dataReader[0].ToString());
            }
            dataReader.Close();
            CloseConn();
            return seqnum;
        }
        #endregion
        #region 执行SQL语句,返回所影响的行数
        /// 执行SQL语句,返回所影响的行数
        public int ExecuteSQL(string sql)
        {
            int Cmd = 0;
            OpenConn();
            SqlCommand command = new SqlCommand(sql, Connection);
            try
            {
                Cmd = command.ExecuteNonQuery();
            }
            catch
            {
            }
            finally
            {
                CloseConn();
            }
            return Cmd;
        }
        #endregion
        #region 执行SQL select 行数
        /// 执行SQL语句,返回所影响的行数
        public int ExecuteSQLSelect(string sql)
        {
            int Cmd = 0;
            OpenConn();
            SqlCommand command = new SqlCommand(sql, Connection);
            try
            {
                Cmd = (int)command.ExecuteScalar();
            }
            catch(Exception er)
            {
            }
            finally
            {
                CloseConn();
            }
            return Cmd;
        }
        #endregion
        public bool checkIsAlive(string TableName, string ht_Where)
        {
            try
            {
                string sql = "select count(*) from " + TableName + " " + ht_Where;
                int val = ExecuteSQLSelect(sql);
                if (val > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog(er.Message, "checkIsAlive Exception", 2);
                return false;
            }
        }
        public int InsertOrUpdate(string TableName,string ht_Where, Hashtable ht)
        {
            try
            {
                string sql = "select count(*) from " + TableName + " " + ht_Where;
                int val = ExecuteSQLSelect(sql);
                if(val>0)
                {
                    int cmd=Update(TableName, ht_Where, ht);
                    return cmd;
                }
                else
                {
                    return Insert(TableName, ht);
                }
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog(er.Message, "Insert Exception", 2);
                return 0;
            }
        }
        //＝＝用hashTable对数据库进行insert,update,del操作,注意此时只能用默认的数据库连接"connstr"
        
        #region 根据表名及哈稀表自动插入数据库 用法：Insert("test",ht)
        public int Insert(string TableName, Hashtable ht)
        {
            try
            {
                SqlParameter[] Parms = new SqlParameter[ht.Count];
                IDictionaryEnumerator et = ht.GetEnumerator();
                DataTable dt = GetTabType(TableName);

                SqlDbType otype;
                int size = 0;
                int i = 0;
                while (et.MoveNext()) // 作哈希表循环
                {
                    GetoType(et.Key.ToString(), dt, out otype, out size);
                    SqlParameter op = MakeParam(":" + et.Key.ToString(), otype, size, et.Value.ToString());
                    Parms[i] = op; // 添加SqlParameter对象
                    i = i + 1;
                }
                string str_Sql = GetInsertSqlbyHt(TableName, ht); // 获得插入sql语句
                int val = ExecuteNonQuery(str_Sql);
                FileOpreate.SaveLog(str_Sql,"Insert",2);
                return val;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog(er.Message, "Insert Exception", 2);
                return 0;
            }
        }
        #endregion

        #region 根据相关条件对数据库进行更新操作 用法：Update("test","Id=:Id",ht); 
        public int Update(string TableName, string ht_Where, Hashtable ht)
        {
            try
            {
                SqlParameter[] Parms = new SqlParameter[ht.Count];
                IDictionaryEnumerator et = ht.GetEnumerator();
                DataTable dt = GetTabType(TableName);
                SqlDbType otype;
                int size = 0;
                int i = 0;
                // 作哈希表循环
                while (et.MoveNext())
                {
                    GetoType(et.Key.ToString().ToUpper(), dt, out otype, out size);
                    SqlParameter op = MakeParam(":" + et.Key.ToString(), otype, size, et.Value.ToString());
                    Parms[i] = op; // 添加SqlParameter对象
                    i = i + 1;
                }
                string str_Sql = GetUpdateSqlbyHt(TableName, ht_Where, ht); // 获得插入sql语句
                int val = ExecuteNonQuery(str_Sql);
                FileOpreate.SaveLog(str_Sql, "Update", 2);
                return val;

            }
            catch (Exception er)
            {
                FileOpreate.SaveLog(er.Message, "Update Exception", 2);
                return 0;
            }
        }
        #endregion

        #region del操作,注意此处条件个数与hash里参数个数应该一致 用法：Del("test","Id=:Id",ht)
        public int Del(string TableName, string ht_Where, Hashtable ht)
        {
            try
            {
                SqlParameter[] Parms = new SqlParameter[ht.Count];
                IDictionaryEnumerator et = ht.GetEnumerator();
                DataTable dt = GetTabType(TableName);
                SqlDbType otype;
                int i = 0;
                int size = 0;
                // 作哈希表循环
                while (et.MoveNext())
                {
                    GetoType(et.Key.ToString().ToUpper(), dt, out otype, out size);
                    SqlParameter op = MakeParam(":" + et.Key.ToString(), et.Value.ToString());
                    Parms[i] = op; // 添加SqlParameter对象
                    i = i + 1;
                }
                string str_Sql = GetDelSqlbyHt(TableName, ht_Where, ht); // 获得删除sql语句
                int val = ExecuteNonQuery(str_Sql, Parms);
                return val;
            }
            catch (Exception er)
            {
                FileOpreate.SaveLog(er.Message, "Delete Exception", 2);
                //ini.INIIO.saveLogInf("Del异常:" + er.Message);
                return 0;
            }
        }
        #endregion

        //　＝＝＝＝＝＝＝＝上面三个操作的内部调用函数＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝

        #region 根据哈稀表及表名自动生成相应insert语句(参数类型的)
        /// 根据哈稀表及表名自动生成相应insert语句 
        /// 要插入的表名
        /// 哈稀表
        /// 返回sql语句
        public static string GetInsertSqlbyHt(string TableName, Hashtable ht)
        {
            string str_Sql = "";
            int i = 0;
            int ht_Count = ht.Count; // 哈希表个数
            IDictionaryEnumerator myEnumerator = ht.GetEnumerator();
            string before = "";
            string behide = "";
            while (myEnumerator.MoveNext())
            {
                if (i == 0)
                {
                    before = "(" + myEnumerator.Key;
                }
                else if (i + 1 == ht_Count)
                {
                    before = before + "," + myEnumerator.Key + ")";
                }
                else
                {
                    before = before + "," + myEnumerator.Key;
                }
                i = i + 1;
            }
            myEnumerator = ht.GetEnumerator();
            i = 0;
            while (myEnumerator.MoveNext())
            {
                if (i == 0)
                {
                    behide = "('" + myEnumerator.Value + "'";
                }
                else if (i + 1 == ht_Count)
                {
                    behide = behide + ",'" + myEnumerator.Value + "')";
                }
                else
                {
                    behide = behide + ",'" + myEnumerator.Value + "'";
                }
                i = i + 1;
            }
            behide = behide.Replace("'DateTimeS", "to_date('").Replace("DateTimeE'", "','yyyy-MM-dd hh24:mi:ss')");
            behide = " Values" + behide;
            str_Sql = "Insert into " + TableName + before + behide;
            return str_Sql;
        }
        #endregion

        #region 根据表名，where条件，哈稀表自动生成更新语句(参数类型的)
        public static string GetUpdateSqlbyHt(string Table, string ht_Where, Hashtable ht)
        {
            string str_Sql = "";
            int i = 0;
            int ht_Count = ht.Count; // 哈希表个数
            IDictionaryEnumerator myEnumerator = ht.GetEnumerator();
            while (myEnumerator.MoveNext())
            {
                if (i == 0)
                {
                    if (ht_Where.ToString().ToLower().IndexOf((myEnumerator.Key + "=:" + myEnumerator.Key).ToLower()) == -1)
                    {
                        str_Sql = myEnumerator.Key + "='" + myEnumerator.Value+"'";
                    }
                }
                else
                {
                    if (ht_Where.ToString().ToLower().IndexOf((":" + myEnumerator.Key + " ").ToLower()) == -1)
                    {
                        str_Sql = str_Sql + "," + myEnumerator.Key + "='" + myEnumerator.Value+"'";
                    }
                }
                i = i + 1;
            }
            str_Sql = str_Sql.Replace("'DateTimeS", "to_date('").Replace("DateTimeE'", "','yyyy-MM-dd hh24:mi:ss')");
            if (ht_Where == null || ht_Where.Replace(" ", "") == "") // 更新时候没有条件
            {
                str_Sql = "update " + Table + " set " + str_Sql;
            }
            else
            {
                str_Sql = "update " + Table + " set " + str_Sql  + ht_Where;
            }
            //str_Sql = str_Sql.Replace("set ,", "set ").Replace("update ,", "update ");
            return str_Sql;
        }
        #endregion

        #region 根据表名，where条件，哈稀表自动生成del语句(参数类型的)
        public static string GetDelSqlbyHt(string Table, string ht_Where, Hashtable ht)
        {
            string str_Sql = "";
            int i = 0;

            int ht_Count = ht.Count; // 哈希表个数
            IDictionaryEnumerator myEnumerator = ht.GetEnumerator();
            while (myEnumerator.MoveNext())
            {
                if (i == 0)
                {
                    if (ht_Where.ToString().ToLower().IndexOf((myEnumerator.Key + "=:" + myEnumerator.Key).ToLower()) == -1)
                    {
                        str_Sql = myEnumerator.Key + "=:" + myEnumerator.Key;
                    }
                }
                else
                {
                    if (ht_Where.ToString().ToLower().IndexOf((":" + myEnumerator.Key + " ").ToLower()) == -1)
                    {
                        str_Sql = str_Sql + "," + myEnumerator.Key + "=:" + myEnumerator.Key;
                    }
                }
                i = i + 1;
            }
            if (ht_Where == null || ht_Where.Replace(" ", "") == "") // 更新时候没有条件
            {
                str_Sql = "Delete " + Table;
            }
            else
            {
                str_Sql = "Delete " + Table + " where " + ht_Where;
            }
            return str_Sql;
        }
        #endregion

        #region 生成oracle参数
        /// 
        /// 生成oracle参数
        /// 字段名
        /// 数据类型
        /// 数据大小
        /// 值
        public static SqlParameter MakeParam(string ParamName, SqlDbType otype, int size, Object Value)
        {
            SqlParameter para = new SqlParameter(ParamName, Value);
            para.SqlDbType = otype;
            para.Size = size;
            return para;
        }
        #endregion
        #region 生成oracle参数
        public static SqlParameter MakeParam(string ParamName, string Value)
        {
            return new SqlParameter(ParamName, Value);
        }
        #endregion
        #region 根据表结构字段的类型和长度拼装oracle sql语句参数
        public static void GetoType(string key, DataTable dt, out SqlDbType otype, out int size)
        {
            try
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = "column_name='" + key + "'";
                string fType = dv[0]["data_type"].ToString().ToUpper();
                switch (fType)
                {
                    case "DATE":
                        otype = SqlDbType.Date;
                        size = int.Parse(dv[0]["data_length"].ToString());
                        break;
                    case "CHAR":
                        otype = SqlDbType.Char;
                        size = int.Parse(dv[0]["data_length"].ToString());
                        break;
                    case "LONG":
                        otype = SqlDbType.Int;
                        size = int.Parse(dv[0]["data_length"].ToString());
                        break;
                    case "NVARCHAR2":
                        otype = SqlDbType.NVarChar;
                        size = int.Parse(dv[0]["data_length"].ToString());
                        break;
                    case "VARCHAR2":
                        otype = SqlDbType.VarChar;
                        size = int.Parse(dv[0]["data_length"].ToString());
                        break;
                    default:
                        otype = SqlDbType.VarChar;
                        size = 100;
                        break;
                }
            }
            catch
            {
                otype = SqlDbType.VarChar;
                size = 100;
            }
        }
        #endregion
        #region 动态 取表里字段的类型和长度,此处没有动态用到connstr,是默认的！by/文少
        public System.Data.DataTable GetTabType(string tabnale)
        {
            string sql = "SELECT * FROM INFORMATION_SCHEMA.columns WHERE TABLE_NAME='" + tabnale.ToUpper() + "'";
            OpenConn();
            return (ReturnDataSet(sql, "dv")).Tables[0];
        }
        #endregion
        #region 执行sql语句 
        public int ExecuteNonQuery(string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            OpenConn();
            cmd.Connection = Connection;
            cmd.CommandText = cmdText;
            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            CloseConn();
            return val;
        }
        #endregion
    }

    public class DBHelperSQL
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private static string constr = Init_DBlink();
        public static SqlConnection conn_test = new SqlConnection(constr);
        public static bool DB_Status = false;

        /// <summary>
        /// 初始化数据库连接字符串
        /// </summary>
        /// <returns>string</returns>
        private static string Init_DBlink()
        {
            string constr_temp = "";
            if (FileOpreate.DB_Address == "" || FileOpreate.DB_Name == "" || FileOpreate.DB_Pwd == "" || FileOpreate.DB_User == "")
            {
                if (FileOpreate.GetDBConfig() == false || FileOpreate.DB_Address == "" || FileOpreate.DB_Name == "" || FileOpreate.DB_Pwd == "" || FileOpreate.DB_User == "")
                    throw new Exception("数据库服务器配置不正确或未配置");
            }
            constr_temp = "Data Source=" + FileOpreate.DB_Address + ";Initial Catalog=" + FileOpreate.DB_Name + ";Persist Security Info=True;User ID=" + FileOpreate.DB_User + ";password=" + FileOpreate.DB_Pwd;
            DB_Link_Test_DAL(constr_temp);
            return constr_temp;
        }

        /// <summary>
        /// 测试数据库连接状态DBHelper用
        /// </summary>
        private static void DB_Link_Test_DAL(string connstr)
        {
            using (SqlConnection conn_test = new SqlConnection(connstr))
            {
                try
                {
                    DataTable test_dt = new DataTable();
                    SqlDataAdapter test_adp = new SqlDataAdapter();
                    conn_test.Open();
                    SqlCommand cmd = conn_test.CreateCommand();
                    cmd.CommandText = "select * from SYSConfig";        //查询这张表 如果没有异常则连接正常
                    test_adp = new SqlDataAdapter(cmd);
                    test_adp.Fill(test_dt);
                    conn_test.Close();
                    DB_Status = true;
                }
                catch (Exception er)
                {
                    string x = er.Message;
                    conn_test.Close();
                    DB_Status = false;
                }
            }
        }

        /// <summary>
        /// 通用方法 查询返回（带参数）
        /// </summary>
        /// <param name="sql">查询语句或存储过程名</param>
        /// <param name="cmdtype">执行类型</param>
        /// <param name="spr">参数 数组</param>
        /// <returns>查询出来的数据表</returns>
        public static DataTable GetDataTable(string sql, CommandType cmdtype, SqlParameter[] spr)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = sql;
                    cmd.CommandType = cmdtype;
                    cmd.Parameters.AddRange(spr);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    try
                    {
                        conn.Open();
                        adp.Fill(dt);
                        conn.Close();
                        return dt;

                    }
                    catch (Exception e)
                    {
                        conn.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 通用方法 查询返回（不带参数）
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="cmdtype">查询语句类型</param>
        /// <returns>数据表</returns>
        public static DataTable GetDataTable(string sql, CommandType cmdtype)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = sql;
                        cmd.CommandType = cmdtype;
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        conn.Open();
                        adp.Fill(dt);
                        conn.Close();
                        return dt;
                    }
                    catch (Exception e)
                    {
                        conn.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 通用方法 查询返回（不带参数）
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="cmdtype">查询语句类型</param>
        /// <returns>数据表</returns>
        public static DataSet GetDataSet(string sql, CommandType cmdtype)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = sql;
                    cmd.CommandType = cmdtype;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataSet dt = new DataSet();
                    try
                    {
                        conn.Open();
                        adp.Fill(dt);
                        conn.Close();
                        return dt;
                    }
                    catch (Exception e)
                    {
                        conn.Close();
                        throw e;
                    }
                }
            }
        }

        public static DataTable GetDataTable(string sql)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = sql; ;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    try
                    {
                        conn.Open();

                        adp.Fill(dt);
                        conn.Close();
                        return dt;
                    }
                    catch (Exception e)
                    {
                        conn.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 通用方法  查询返回第一行第一列 Object类型
        /// </summary>
        /// <param name="sql">查询语句或存储过程名</param>
        /// <param name="cmdtype">执行类型</param>
        /// <param name="spr">参数 数组</param>
        /// <returns>查询出来的第一行第一列  Object类型</returns>
        public static object GetOnline(string sql, CommandType cmdtype, SqlParameter[] spr)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // conn.Open();
                    cmd.CommandText = sql;
                    cmd.CommandType = cmdtype;
                    cmd.Parameters.AddRange(spr);
                    try
                    {
                        conn.Open();
                        Object obj = cmd.ExecuteScalar();
                        conn.Close();
                        return obj;

                    }
                    catch (Exception e)
                    {
                        conn.Close();
                        throw e;
                    }

                }

            }
        }

        /// <summary>
        /// 通用方法  查询返回第一行第一列 Object类型
        /// </summary>
        /// <param name="sql">查询语句或存储过程名</param>
        /// <returns>查询出来的第一行第一列  Object类型</returns>
        public static object GetOnline(string sql)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    try
                    {
                        conn.Open();
                        Object obj = cmd.ExecuteScalar();
                        conn.Close();
                        return obj;

                    }
                    catch (Exception e)
                    {
                        conn.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行sql语句并返回受影响的行数
        /// </summary>
        /// <param name="sql">查询语句或存储过程名</param>
        /// <param name="cmdtype">执行类型</param>
        /// <param name="spr">参数 数组</param>
        /// <returns>受影响的行数</returns>
        public static int Execute(string sql, CommandType cmdtype, SqlParameter[] spr)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = cmdtype;
                    cmd.Parameters.AddRange(spr);

                    try
                    {
                        conn.Open();
                        int i = cmd.ExecuteNonQuery();
                        conn.Close();
                        return i;
                    }
                    catch (Exception e)
                    {
                        conn.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行sql语句并返回受影响的行数
        /// </summary>
        /// <param name="sql">查询语句或存储过程名</param>
        /// <param name="spr">参数 数组</param>
        /// <returns>受影响的行数</returns>
        public static int Execute(string sql, SqlParameter[] spr)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(spr);
                    try
                    {
                        conn.Open();
                        int i = cmd.ExecuteNonQuery();
                        conn.Close();
                        return i;
                    }
                    catch (Exception e)
                    {
                        cmd.Parameters.Clear();
                        conn.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行sql语句并返回受影响的行数
        /// </summary>
        /// <param name="sql">查询语句或存储过程名</param>
        /// <returns>受影响的行数</returns>
        public static int Execute(string sql)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    try
                    {
                        conn.Open();
                        int i = cmd.ExecuteNonQuery();
                        conn.Close();
                        return i;
                    }
                    catch (Exception e)
                    {
                        conn.Close();
                        throw e;
                    }
                }
            }
        }
        /// <summary>
        /// 执行sql语句并返回受影响的行数
        /// </summary>
        /// <param name="sql">查询语句或存储过程名</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteCount(string sql)
        {
            int i = 0;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    try
                    {
                        conn.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            i = int.Parse(dr["number"].ToString());
                        }
                        else
                        {
                            i = 0;
                        }
                        dr.Close();
                        //int i = cmd.ExecuteNonQuery();
                        conn.Close();
                        return i;
                    }
                    catch (Exception e)
                    {
                        conn.Close();
                        throw e;
                    }
                }
            }
        }
    }
}