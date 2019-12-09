using Adapter.Interfaces;
using Adapter.Models;
using System;

namespace Adapter.Core
{
    internal class ResultFactory : IResultFactory
    {
        public object? Share { get; set; }
        private string _rootView = "Views/App.cshtml";
        private object? _version;

        public void SetRootView(string s) => _rootView = s;

        public void Version(string version) => _version = version;

        public void Version(Func<string> version) => _version = version;

        public string? GetVersion() =>
            _version switch
            {
                Func<string> func => func(),
                string s => s,
                _ => null
            };

        public Result Render(string component, object controller) =>
            new Result(new Props { Controller = controller, Share = Share }, component, _rootView, GetVersion());
    }
}