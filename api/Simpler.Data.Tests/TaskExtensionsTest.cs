using NUnit.Framework;
using Simpler;
using Simpler.Data;

namespace Simpler.Data.Tests.Tasks
{
    public class SelectEverything : Task
    {
        public override void Execute() { }
    }
}

namespace Simpler.Data.Tests
{
    using Simpler.Data.Tests.Tasks;

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