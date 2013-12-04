using System.ComponentModel.Composition;
using Simple.Data.Ado;

namespace Simple.Data.Oracle.ManagedDataAccess
{
    [Export(typeof(ICustomInserter))]
    public class OracleInserter : OracleInserterBase
    {
    }
}