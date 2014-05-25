using NUnit.Framework;
using Simpler;
using Simpler.Data.Sql;
using Easier.Tasks;

namespace Easier.Sql
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