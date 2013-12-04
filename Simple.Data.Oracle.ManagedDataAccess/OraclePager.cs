using System.ComponentModel.Composition;
using Simple.Data.Ado;

namespace Simple.Data.Oracle.ManagedDataAccess
{
    [Export(typeof(IQueryPager))]
    public class OraclePager : OraclePagerBase
    {
    }
}
