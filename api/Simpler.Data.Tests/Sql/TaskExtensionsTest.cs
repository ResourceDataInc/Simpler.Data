using NUnit.Framework;
using Simpler;
using Simpler.Data.Tests.Tasks;
using Simpler.Data.Sql;

namespace Simpler.Data.Tests
{
    [TestFixture]
    public class TaskExtensionTest
    {
        [Test]
        public void should_find_sql_in_corresponding_sql_file()
        {
            var t = Task.New<SelectEverything>();
            Assert.That(t.Sql().Contains("select * from everything"));
        }
    }
}