using Adapter.Core;
using Adapter.Interfaces;
using System;

namespace Adapter
{
    public static class Inertia
    {
        private static IResultFactory _factory = new NullFactory();

        public static void Init(IResultFactory factory) => _factory = factory;

        public static Result Render(string component, object controller) =>
            _factory.Render(component, controller);

        public static string? GetVersion() => _factory.GetVersion();

        public static void SetRootView(string s) => _factory.SetRootView(s);

        public static void Version(string s) => _factory.Version(s);

        public static void Version(Func<string> s) => _factory.Version(s);

        public static object? Share
        {
            get => _factory.Share;
            set => _factory.Share = value;
        }
    }
}