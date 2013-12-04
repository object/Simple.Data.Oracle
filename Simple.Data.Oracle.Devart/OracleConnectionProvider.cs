using System.ComponentModel.Composition;
using System.Data.Common;
using Simple.Data.Ado;

namespace Simple.Data.Oracle.Devart
{
    [Export("Devart.Data.Oracle", typeof(IConnectionProvider))]
    public class OracleConnectionProvider : OracleConnectionProviderBase
    {
        public override DbConnection CreateConnection(string connectionString)
        {
            return new global::Devart.Data.Oracle.OracleConnection(connectionString);
        }

        public override DbConnectionStringBuilder CreateConnectionStringBuilder(string connectionString)
        {
            return new global::Devart.Data.Oracle.OracleConnectionStringBuilder(connectionString);
        }

        public override string UserIdOfConnection(string connectionString)
        {
            return new global::Devart.Data.Oracle.OracleConnectionStringBuilder(connectionString).UserId.ToUpperInvariant();
        }

        public override DbParameter AddToParameterCollection(DbParameterCollection parameters, string parameterName, object parameterValue)
        {
            return (parameters as global::Devart.Data.Oracle.OracleParameterCollection).Add(parameterName, parameterValue);
        }
    }
}
