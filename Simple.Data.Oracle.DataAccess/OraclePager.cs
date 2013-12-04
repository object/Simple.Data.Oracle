using System.ComponentModel.Composition;
using Simple.Data.Ado;

namespace Simple.Data.Oracle.DataAccess
{
    [Export(typeof(IQueryPager))]
    public class OraclePager : OraclePagerBase
    {
    }
}
