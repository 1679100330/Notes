using System;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace DailyReport.Utils
{
    public class OracleHelper
    {
        private string connStr;

        public OracleHelper(string key)
        {
            connStr = ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }
 
        public OracleConnection GetConnection()
        {
            return new OracleConnection(connStr);;
        }

        public int ExecuteNonQuery(string cmdText, CommandType type, params DbParameter[] paras)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                using (OracleCommand cmd = new OracleCommand(cmdText, conn))
                {
                    if (paras != null)
                    {
                        cmd.Parameters.AddRange(paras);
                    }
                    cmd.CommandType = type;
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// string cmdText = "SELECT COUNT(*) from SFIS1.DAILY_REPORT_USER WHERE USERNAME=:USERNAME and PASSWORD = :PASSWORD";
        ///    int count = Convert.ToInt32(helper.ExecuteScalar(cmdText, CommandType.Text
        ///        , new OracleParameter("USERNAME", user.username)
        ///        , new OracleParameter("PASSWORD", user.password)));
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="type"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public object ExecuteScalar(string cmdText, CommandType type, params DbParameter[] paras)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                using (OracleCommand cmd = new OracleCommand(cmdText, conn))
                {
                    if (paras != null)
                    {
                        cmd.Parameters.AddRange(paras);
                    }
                    cmd.CommandType = type;
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        public DbDataReader ExecuteReader(string cmdText, CommandType type, params DbParameter[] paras)
        {
            OracleConnection conn = new OracleConnection(connStr);
            using (OracleCommand cmd = new OracleCommand(cmdText, conn))
            {
                if (paras != null)
                {
                    cmd.Parameters.AddRange(paras);
                }
                cmd.CommandType = type;
                conn.Open();
                try
                {
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch (Exception ex)
                {
                    conn.Dispose();
                    throw ex;
                }
            }
        }

        public DataSet GetDataSet(string cmdText, CommandType type, params DbParameter[] paras)
        {
            DataSet ds = new DataSet();
            OracleConnection conn = new OracleConnection(connStr);
            using (OracleDataAdapter oda = new OracleDataAdapter(cmdText, conn))
            {
                if (paras != null)
                {
                    oda.SelectCommand.Parameters.AddRange(paras);
                }
                oda.SelectCommand.CommandType = type;
                oda.Fill(ds);
            }
            return ds;
        }

        public int ExecuteNonQuery(string cmdText, params DbParameter[] paras)
        {
            return ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        public object ExecuteScalar(string cmdText, params DbParameter[] paras)
        {
            return ExecuteScalar(cmdText, CommandType.Text, paras);
        }

        public DbDataReader ExecuteReader(string cmdText, params DbParameter[] paras)
        {
            return ExecuteReader(cmdText, CommandType.Text, paras);
        }

        public DataSet GetDataSet(string cmdText, params DbParameter[] paras)
        {
            return GetDataSet(cmdText, CommandType.Text, paras);
        }

    }
}