using System.ComponentModel.Composition;
using System.Data.Common;
using Simple.Data.Ado;
using OracleClient = Oracle.ManagedDataAccess.Client;

namespace Simple.Data.Oracle.ManagedDataAccess
{
    [Export("Oracle.ManagedDataAccess", typeof(IConnectionProvider))]
    public class OracleConnectionProvider : OracleConnectionProviderBase
    {
        private static AssemblyResolver _assemblyResolver = new AssemblyResolver();

        public override DbConnection CreateConnection(string connectionString)
        {
            return new OracleClient.OracleConnection(connectionString);
        }

        public override DbConnectionStringBuilder CreateConnectionStringBuilder(string connectionString)
        {
            return new OracleClient.OracleConnectionStringBuilder(connectionString);
        }

        public override string UserIdOfConnection(string connectionString)
        {
            return new OracleClient.OracleConnectionStringBuilder(connectionString).UserID.ToUpperInvariant();
        }

        public override DbParameter AddToParameterCollection(DbParameterCollection parameters, string parameterName, object parameterValue)
        {
            return (parameters as OracleClient.OracleParameterCollection).Add(parameterName, parameterValue);
        }
    }
}
