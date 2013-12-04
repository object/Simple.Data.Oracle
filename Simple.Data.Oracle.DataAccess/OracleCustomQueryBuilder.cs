using System.ComponentModel.Composition;
using Simple.Data.Ado;

namespace Simple.Data.Oracle.DataAccess
{
    [Export(typeof(ICustomQueryBuilder))]
    public class OracleCustomQueryBuilder : OracleCustomQueryBuilderBase
    {
    }
}
