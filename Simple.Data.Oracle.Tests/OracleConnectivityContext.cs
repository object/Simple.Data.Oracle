using System;
using System.Configuration;
using System.Data.Common;
using NUnit.Framework;
using Simple.Data.Ado.Schema;
using System.Collections.Generic;
using System.Linq;

namespace Simple.Data.Oracle.Tests
{
    internal class OracleConnectivityContext
    {
        internal const string OracleClientProvider = "Oracle.DataAccess.Client";
        internal const string DevartClientProvider = "Devart.Data.Oracle";
        internal const string OracleManagedDataAccessProvider = "Oracle.ManagedDataAccess";

        private readonly Dictionary<string, Func<OracleConnectionProviderBase>> _connectionProviderFactories = 
            new Dictionary<string, Func<OracleConnectionProviderBase>>
        {
            { OracleClientProvider, () => new DataAccess.OracleConnectionProvider()},
            { DevartClientProvider, () => new Devart.OracleConnectionProvider()},
            { OracleManagedDataAccessProvider, () => new ManagedDataAccess.OracleConnectionProvider()},
        };

        protected readonly string[] _unavailableProviders = {OracleClientProvider};

        protected dynamic _db;
        protected string _connectionString;
        protected string _providerName;

        protected OracleConnectivityContext(string providerName)
        {
            _connectionString = ConfigurationManager.ConnectionStrings[providerName].ConnectionString;
            _providerName = ConfigurationManager.ConnectionStrings[providerName].ProviderName;
        }

        [SetUp]
        public void SetUp()
        {
            if (_unavailableProviders.Contains(_providerName))
                Assert.Ignore("Provider {0} is not available", _providerName);
        }

        protected void InitDynamicDB()
        {
            _db = Database.Opener.OpenConnection(_connectionString, _providerName);
        }

        protected List<Table> Tables { get; private set; }

        protected Table TableByName(string name)
        {
            return Tables.Single(t => t.ActualName.InvariantEquals(name));
        }

        protected OracleConnectionProviderBase GetConnectionProvider()
        {
            var p = _connectionProviderFactories[_providerName].Invoke();
            p.SetConnectionString(_connectionString);
            return p;
        }

        protected SqlReflection GetSqlReflection()
        {
            return new SqlReflection(GetConnectionProvider());
        }

        protected OracleSchemaProvider GetSchemaProvider()
        {
            var schemaProvider = new OracleSchemaProvider(GetConnectionProvider());
            Tables = schemaProvider.GetTables().ToList();
            return schemaProvider;
        }

        protected DbCommand GetCommand(string text)
        {
            var con = GetConnectionProvider().CreateConnection(_connectionString);
            var c = con.CreateCommand();
            c.CommandText = text;
            return c;
        }
    }
}