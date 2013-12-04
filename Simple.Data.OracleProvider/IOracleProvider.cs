using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Simple.Data.OracleProvider
{
    public interface IOracleProvider
    {
        DbConnection CreateConnection(string connectionString);
        DbConnectionStringBuilder CreateConnectionStringBuilder(string connectionString);
        string UserIdOfConnection(string connectionString);
        DbParameter AddToParameterCollection(DbParameterCollection parameters, string parameterName, object parameterValue);
    }
}
