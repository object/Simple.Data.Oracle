using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Simple.Data.Ado;

namespace Simple.Data.Oracle
{
    public class OracleCustomQueryBuilderBase : ICustomQueryBuilder
    {
        public ICommandBuilder Build(AdoAdapter adapter, int bulkIndex, SimpleQuery query, out IEnumerable<SimpleQueryClauseBase> unhandledClauses)
        {
            return new OracleQueryBuilder(adapter, bulkIndex, new OracleFunctionNameConverter()).Build(query, out unhandledClauses);
        }
    }
}
