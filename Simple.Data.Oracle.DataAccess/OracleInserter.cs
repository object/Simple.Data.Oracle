using System.ComponentModel.Composition;
using Simple.Data.Ado;

namespace Simple.Data.Oracle.DataAccess
{
    [Export(typeof(ICustomInserter))]
    public class OracleInserter : OracleInserterBase
    {
    }
}