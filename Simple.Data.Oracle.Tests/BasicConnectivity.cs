using System;
using System.Linq;
using NUnit.Framework;

namespace Simple.Data.Oracle.Tests
{
    [TestFixture(OracleClientProvider)]
    [TestFixture(OracleManagedDataAccessProvider)]
    [TestFixture(DevartClientProvider)]
    internal class BasicConnectivity : OracleConnectivityContext
    {
        public BasicConnectivity(string providerName) : base(providerName) { }

        [Test]
        public void Basic_connection_to_region()
        {
            Assert.IsNotNull(_db);
            var regions = _db.Regions.All().ToList<Region>();
            Assert.IsNotNull(regions);
            foreach (Region r in regions)
            {
                Assert.IsNotNull(r);
                Assert.IsNotNull(r.RegionId);

            }
        }

        [TestFixtureSetUp]
        public void Given()
        {
            if (_unavailableProviders.Contains(_providerName))
                return;

            InitDynamicDB();
        }
    }
}
