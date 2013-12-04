using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Simple.Data.Oracle.Tests
{
    public class RegionWithGuid
    {
        public decimal RegionId { get; set; }
        public string RegionName { get; set; }
        public Guid RegionUid { get; set; }
    }

    [TestFixture(OracleClientProvider)]
    [TestFixture(OracleManagedDataAccessProvider)]
    [TestFixture(DevartClientProvider)]
    internal class GuidInsertionTests : OracleConnectivityContext
    {
        public GuidInsertionTests(string providerName) : base(providerName) { }

        [TestFixtureSetUp]
        public void Given()
        {
            if (_unavailableProviders.Contains(_providerName))
                return;

            InitDynamicDB();
        }

        [TearDown]
        public void Teardown()
        {
            if (_unavailableProviders.Contains(_providerName))
                return;

            _db.Regions.DeleteByRegionId(5);
        }

        [Test]
        public void Guid_is_insertable()
        {
            _db.Regions.Insert(RegionId: 5m, RegionName: "Antarctica", RegionUid: Guid.NewGuid());
        }

        [Test]
        public void Guid_is_retrievable()
        {
            var r = (RegionWithGuid)_db.Regions.Insert(RegionId: 5m, RegionName: "Antarctica", RegionUid: Guid.NewGuid());
            Assert.AreNotEqual(Guid.Empty, r.RegionUid);
        }

        [Test]
        public void Guid_can_be_used_for_retrieval()
        {
            var uid = Guid.NewGuid();
            var r = (RegionWithGuid)_db.Regions.Insert(RegionId: 5m, RegionName: "Antarctica", RegionUid: uid);
            IList<dynamic> result = _db.Regions.FindAllByRegionUid(uid.ToByteArray()).ToList<dynamic>();
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }
    }
}
