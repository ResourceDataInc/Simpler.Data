using System;
using System.Data;
using Simpler.Data.Tasks;

namespace Simpler.Data
{
    public class Results : IDisposable
    {
        public Results(IDataReader reader)
        {
            Reader = reader;
        }

        public IDataReader Reader { get; set; }

        public T[] Read<T>()
        {
            if (Reader == null) throw new ResultsException("There are no more results.");

            var buildObjects = Task.New<BuildObjects<T>>();
            buildObjects.In.Reader = Reader;
            buildObjects.Execute();

            if (!Reader.NextResult())
            {
                Reader.Dispose();
                Reader = null;
            }

            return buildObjects.Out.Objects;
        }

        public void Dispose()
        {
            if (Reader != null)
            {
                Reader.Dispose();
                Reader = null;
            }
        }
    }
}