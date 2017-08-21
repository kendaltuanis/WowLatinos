using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Data;

namespace DBAccess
{
    public class MySqlAccess : DBAccess
    {
        private MySqlConnection connection;
        private MySqlTransaction transaction;
        public MySqlAccess(string connectionString) : base(connectionString)
        {
            try
            {
                this.connection = new MySqlConnection(this.connectionString);
            }
            catch (MySqlException e)
            {
                this.ProcessException(e);
            }
            this.Connect();

            this.connection.InfoMessage += new MySqlInfoMessageEventHandler((object sender, MySqlInfoMessageEventArgs e) => {
                ProcessStoreProcedureException(e.errors.GetValue(0).ToString());
            });
        }
        public override void Connect()
        {
            if (this.connection.State == ConnectionState.Open) return;
            try
            {
                this.connection.Open();
            }
            catch (MySqlException e)
            {
                this.ProcessException(e);
            }
        }
        public override void Disconnect()
        {
            try
            {
                this.connection.Close();
            }
            catch (MySqlException ex)
            {
                this.ProcessException(ex);
            }
        }
        public override Dictionary<string, string> SqlQuery(string sql, IDictionary<string, object> parameters)
        {
            MySqlDataReader resultSet=null;
            Dictionary<string, string> dic= new Dictionary<string, string>();
            try
            {
                MySqlCommand cmd = this.AddParameters(sql, parameters);
                resultSet = cmd.ExecuteReader();

                while (resultSet.Read())
                {
                    for (int i = 0; i < resultSet.FieldCount; i++) {
                        object s=null;
                        try
                        {
                             s = resultSet.GetValue(i);
                        }
                        catch (MySqlConversionException e)
                        {
                            s = "Ninguna conexión";
                        }
                        
                        dic.Add(resultSet.GetName(i), (s==null) ? "" : s.ToString());
                    }
                }
            }
            catch (MySqlException e)
            {
                this.ProcessException(e);
            }

            finally
            {
                if (resultSet!=null) {
                    resultSet.Close();
                }
            }
            return dic;

        }

        public override List<string> SqlQueryList(string sql, IDictionary<string, object> parameters)
        {
            MySqlDataReader resultSet = null;
            List<string> list = new List<string>();
            try
            {
                MySqlCommand cmd = this.AddParameters(sql, parameters);
                resultSet = cmd.ExecuteReader();

                while (resultSet.Read())
                {
                    for (int i = 0; i < resultSet.FieldCount; i++)
                    {
                        list.Add(resultSet.GetString(i));
                    }
                }
            }
            catch (MySqlException e)
            {
                this.ProcessException(e);
            }
            finally
            {
               resultSet.Close();
            }
            return list;

        }
        public override object SqlScalar(string sql, IDictionary<string, object> parameters)
        {
            this.CleanStatus();
            object result = null;
            try
            {
                MySqlCommand cmd = this.AddParameters(sql, parameters);
                cmd.ExecuteNonQuery();
                result= cmd.LastInsertedId;
            }
            catch (MySqlException e)
            {
                this.ProcessException(e);
            }
            return result;
        }
        public override void SqlStatement(string pSql, IDictionary<string, object> parameters)
        {
            this.CleanStatus();
            try
            {
                MySqlCommand cmd = this.AddParameters(pSql, parameters);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                this.ProcessException(ex);
            }
        }
        private MySqlCommand AddParameters(string sql, IDictionary<string, object> parameters)
        {
            MySqlCommand cmd = new MySqlCommand(sql, this.connection);
            cmd.CommandType = CommandType.Text;
            foreach (var parameter in parameters)
            {
                cmd.Parameters.AddWithValue(parameter.Key, (parameter.Value) ?? DBNull.Value);
            }
            return cmd;
        }
        public override void BeginTransaction()
        {
            if (!inTransaction)
            {
                this.transaction = this.connection.BeginTransaction();
                this.inTransaction = true;
            }
        }
        public override void RollbackTransaction()
        {
            if (this.inTransaction)
            {
                this.transaction.Rollback();
                this.inTransaction = false;
            }
        }
        public override void CommitTransaction()
        {
            if (this.inTransaction)
            {
                this.transaction.Commit();
                this.inTransaction = false;
            }
        }
    }
}
