using System;

namespace Simpler
{
    public class BuildTypedException : Exception
    {
        public BuildTypedException() {}
        public BuildTypedException(string message) : base(message) {}
    }

    public class ResultsException : Exception
    {
        public ResultsException() { }
        public ResultsException(string message) : base(message) { }
    }

    public class ConnectException : Exception
    {
        public ConnectException() { }
        public ConnectException(string message) : base(message) { }
    }
}
