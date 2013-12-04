using System.ComponentModel.Composition;
using Simple.Data.Ado;

namespace Simple.Data.Oracle.ManagedDataAccess
{
    [Export(typeof(ICustomQueryBuilder))]
    public class OracleCustomQueryBuilder : OracleCustomQueryBuilderBase
    {
    }
}
