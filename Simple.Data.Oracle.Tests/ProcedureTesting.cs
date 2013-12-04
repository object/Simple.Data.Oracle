using System;
using System.Linq;
using NUnit.Framework;

namespace Simple.Data.Oracle.Tests
{
    [TestFixture(OracleClientProvider)]
    [TestFixture(OracleManagedDataAccessProvider)]
    [TestFixture(DevartClientProvider)]
    internal class ProcedureTesting : OracleConnectivityContext
    {
        public ProcedureTesting(string providerName) : base(providerName) { }

        [TestFixtureSetUp]
        public void Given()
        {
            if (_unavailableProviders.Contains(_providerName))
                return;

            InitDynamicDB();
        }

        [Test]
        public void employee_count_in_department_is_callable()
        {
            var result = _db.Employee_Count_Department("Marketing");
            var @return = result.ReturnValue;
            Assert.IsAssignableFrom<decimal>(@return);
            Assert.AreEqual(2, @return);
        }

        [Test]
        public void accessing_package_function_with_double_underscore()
        {
            var result = _db.Department__Department_Count();
            Assert.AreEqual(27, result.ReturnValue);
        }

        [Test]
        public void accessing_second_package_function()
        {
            var result = _db.Department__Manager_Of_Department("Marketing");
            var @return = result.ReturnValue;
            Assert.IsAssignableFrom<string>(@return);
            Assert.AreEqual("Hartstein", @return);
        }

        [Test]
        public void accessing_stored_proc_with_output_values()
        {
            var result = _db.Department__Manager_And_Count("Marketing");
            Assert.AreEqual("Hartstein", result.OutputValues["P_MANAGER"]);
            Assert.AreEqual(2, result.OutputValues["P_COUNT"]);
        }
    }
}
