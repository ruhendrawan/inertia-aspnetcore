using Adapter.Core;
using System;

namespace Adapter.Interfaces
{
    internal class NullFactory : IResultFactory
    {
        public object? Share { get; set; }

        public void SetRootView(string s) => throw new NotImplementedException();

        public void Version(string version) => throw new NotImplementedException();

        public void Version(Func<string> version) => throw new NotImplementedException();

        public string GetVersion() => throw new NotImplementedException();

        public Result Render(string component, object controller) => throw new NotImplementedException();
    }
}