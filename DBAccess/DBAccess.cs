using System;
using System.Collections.Generic;
using System.Data;

namespace DBAccess
{
    public abstract class DBAccess : ErrorHandler
    {
        protected string connectionString;
        protected bool inTransaction;
        public DBAccess(string connectionString)
        {
            this.connectionString = connectionString;
            this.inTransaction = false;
        }
        public abstract void Connect();
        public abstract void Disconnect();
        public abstract Dictionary<string, string> SqlQuery(string sql, IDictionary<string, Object> parameters);
        public abstract List<string> SqlQueryList(string sql, IDictionary<string, Object> parameters);
        public abstract object SqlScalar(string sql, IDictionary<string, Object> parameters);
        public abstract void SqlStatement(string sql, IDictionary<string, Object> parameters);
        public abstract void BeginTransaction();
        public abstract void RollbackTransaction();
        public abstract void CommitTransaction();
    }
}
