using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

/* 例子：
    using (TransactionHelper tran = new TransactionHelper(OracleHelper.GetConnection())) 
    {
        try
        {
            tran.BeginTransaction();
            tran.ExecuteNonQuery("", CommandType.Text);
            tran.ExecuteNonQuery("", CommandType.Text);
            tran.Commit();
        }
        catch (Exception ex)
        {
            tran.Rollback();
            throw ex;
        }                
    }
 */

namespace DailyReport.Utils
{
    public class TransactionHelper : IDisposable
    {
        private OracleConnection conn;
        private OracleCommand cmd;
        private OracleTransaction tran;

        public TransactionHelper(OracleConnection conn)
        {
            this.conn = conn;
            conn.Open();
        }

        public void BeginTransaction()
        {
            tran = conn.BeginTransaction();
        }

        public int ExecuteNonQuery(string cmdText, CommandType type, params DbParameter[] paras)
        {
            cmd = new OracleCommand(cmdText, conn);
            if (paras != null)
            {
                cmd.Parameters.AddRange(paras);
            }
            cmd.CommandType = type;
            cmd.Transaction = tran;
            int result = cmd.ExecuteNonQuery();
            return result;
        }

        public void Commit()
        {
            if (tran != null)
            {
                tran.Commit();
                tran.Dispose();
                tran = null;
            }
        }

        public void Rollback()
        {
            if (tran != null)
            {
                tran.Rollback();
                tran.Dispose();
                tran = null;
            }
        }

        public void Dispose()
        {
            if (tran != null)
            {
                tran.Dispose();
                tran = null;
            }
            if (cmd != null)
            {
                cmd.Dispose();
                cmd = null;
            }
            if (conn != null)
            {
                conn.Dispose();
                conn = null;
            }
        }

    }
}