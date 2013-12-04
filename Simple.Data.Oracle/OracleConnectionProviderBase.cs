using System;
using System.Data;
using System.Data.Common;
using Simple.Data.Ado;
using Simple.Data.Ado.Schema;

namespace Simple.Data.Oracle
{
    public abstract class OracleConnectionProviderBase : IConnectionProvider
    {
        public abstract DbConnection CreateConnection(string connectionString);
        public abstract DbConnectionStringBuilder CreateConnectionStringBuilder(string connectionString);
        public abstract string UserIdOfConnection(string connectionString);
        public abstract DbParameter AddToParameterCollection(DbParameterCollection parameters, string parameterName, object parameterValue);

        public void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }

        IDbConnection IConnectionProvider.CreateConnection()
        {
            return CreateConnection(ConnectionString);
        }

        public ISchemaProvider GetSchemaProvider()
        {
            return new OracleSchemaProvider(this);
        }

        public string GetIdentityFunction()
        {
            return "ROWID";
        }

        public IProcedureExecutor GetProcedureExecutor(AdoAdapter adapter, ObjectName procedureName)
        {
            procedureName = new ObjectName(procedureName.Schema ?? UserOfConnection.ToUpperInvariant(), procedureName.Name);
            return new OracleProcedureExecutor(this, procedureName);
        }

        public string ConnectionString { get; private set; }

        bool IConnectionProvider.SupportsCompoundStatements
        {
            get { return false; }
        }

        bool IConnectionProvider.SupportsStoredProcedures
        {
            get { return true; }
        }

        internal DbConnection CreateOracleConnection()
        {
            return CreateConnection(ConnectionString);
        }

        internal string UserOfConnection
        {
            get
            {
                return ConnectionString != null ? UserIdOfConnection(ConnectionString) : null;
            }
        }
    }
}
