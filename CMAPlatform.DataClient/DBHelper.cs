using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace CMAPlatform.DataClient
{
    public class DBHelper
    {
        protected static string m_ConnectionString =
            "server=localhost;port=3306;database=cmaplatform;user id=root;password=frontfree;";

        protected static MySqlConnection m_Conn;

        static DBHelper()
        {
            //var data = DataManager.Instance;
            m_ConnectionString = DataManager.m_ConnectionString;

            m_Conn = new MySqlConnection(m_ConnectionString);
            m_Conn.Open();
        }

        public static string GetConnectionString(int i)
        {
            return m_ConnectionString;
        }

        public static void ExecuteSql(string sql)
        {
            var conn = new MySqlConnection(m_ConnectionString);
            conn.Open();
            try
            {
                var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 60000; //10分钟   
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                Debug.Write("--------------------------------------------");
                Debug.WriteLine(msg);
                Debug.WriteLine(sql.Substring(0, 200));
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public static void ExecuteSql(string sql, string connectstr)
        {
            var conn = new MySqlConnection(connectstr);
            conn.Open();
            try
            {
                var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 60000; //10分钟   
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                Debug.Write("--------------------------------------------");
                Debug.WriteLine(msg);
                Debug.WriteLine(sql.Substring(0, 200));
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        #region PrepareCommand

        /// <summary>
        ///     Command预处理
        /// </summary>
        /// <param name="conn">MySqlConnection对象</param>
        /// <param name="trans">MySqlTransaction对象，可为null</param>
        /// <param name="cmd">MySqlCommand对象</param>
        /// <param name="cmdType">CommandType，存储过程或命令行</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <param name="cmdParms">MySqlCommand参数数组，可为null</param>
        private static void PrepareCommand(MySqlConnection conn, MySqlTransaction trans, MySqlCommand cmd,
            CommandType cmdType, string cmdText, MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (var parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        #endregion

        /// <summary>
        ///     执行命令或存储过程，返回DataSet对象
        /// </summary>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string cmdText)
        {
            using (var conn = new MySqlConnection(m_ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 60000; //10分钟   
                cmd.CommandText = cmdText;
                var da = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);
                conn.Close();
                //cmd.Parameters.Clear();
                return ds;
            }
        }


        /// <summary>
        ///     mySql批量执行
        /// </summary>
        /// <param name="SQLStringList"></param>
        public static bool ExecuteSqlBatch(List<string> SQLStringList)
        {
            var IsBoll = false;
            using (var conn = new MySqlConnection(m_ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand();
                cmd.Connection = conn;
                var tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (var n = 0; n < SQLStringList.Count; n++)
                    {
                        var strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            var resultCount = cmd.ExecuteNonQuery();
                        }
                        if (n%500 == 0 || n == SQLStringList.Count - 1)
                        {
                            tx.Commit();
                            tx = conn.BeginTransaction();
                        }
                    }
                    IsBoll = true;
                }
                catch (SqlException e)
                {
                    tx.Rollback();
                    throw new Exception(e.Message);
                }
            }
            return IsBoll;
        }

        /// <summary>
        ///     mySql批量执行
        /// </summary>
        /// <param name="SQLStringList"></param>
        public static void ExecuteSqlBatch(List<string> SQLStringList, string connectstr)
        {
            using (var conn = new MySqlConnection(connectstr))
            {
                conn.Open();
                var cmd = new MySqlCommand();
                cmd.Connection = conn;
                var tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (var n = 0; n < SQLStringList.Count; n++)
                    {
                        var strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                        if (n%500 == 0 || n == SQLStringList.Count - 1)
                        {
                            tx.Commit();
                            tx = conn.BeginTransaction();
                        }
                    }
                }
                catch (SqlException e)
                {
                    tx.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }
    }
}