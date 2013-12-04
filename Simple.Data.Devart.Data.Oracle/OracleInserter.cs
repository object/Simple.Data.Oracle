using System.ComponentModel.Composition;
using Simple.Data.Ado;
using Simple.Data.Oracle;

namespace Simple.Data.Devart.Data.Oracle
{
    [Export(typeof(ICustomInserter))]
    public class OracleInserter : OracleInserterBase
    {
    }
}