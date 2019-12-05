using System;
using Microsoft.AspNetCore.Mvc;

namespace Adapter.Interfaces
{
    internal class NullFactory : IResultFactory
    {
        public object? Share { get; set; }

        public void SetRootView(string s) => throw new NotImplementedException();

        public void Version(string version) => throw new NotImplementedException();

        public void Version(Func<string> version) => throw new NotImplementedException();

        public string GetVersion() => throw new NotImplementedException();

        public IActionResult Render(string component, object controller) => throw new NotImplementedException();
    }
}