using System;
using System.ComponentModel.Composition;
using System.Data.Common;
using Simple.Data.Ado;
using Simple.Data.Oracle;
using OracleClient = Devart.Data.Oracle;

namespace Simple.Data.Devart.Data.Oracle
{
    [Export("Devart.Data.Oracle", typeof(IConnectionProvider))]
    public class OracleConnectionProvider : OracleConnectionProviderBase
    {
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
            return new OracleClient.OracleConnectionStringBuilder(connectionString).UserId.ToUpperInvariant();
        }

        public override DbParameter AddToParameterCollection(DbParameterCollection parameters, string parameterName, object parameterValue)
        {
            return (parameters as OracleClient.OracleParameterCollection).Add(parameterName, parameterValue);
        }
    }
}
