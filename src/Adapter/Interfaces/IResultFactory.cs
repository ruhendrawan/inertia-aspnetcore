using Adapter.Core;
using System;

namespace Adapter.Interfaces
{
    public interface IResultFactory
    {
        object? Share { get; set; }

        void SetRootView(string s);

        void Version(string version);

        void Version(Func<string> version);

        string? GetVersion();

        Result Render(string component, object controller);
    }
}