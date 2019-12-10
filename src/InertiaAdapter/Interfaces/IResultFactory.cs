using InertiaAdapter.Core;
using Microsoft.AspNetCore.Html;
using System;

namespace InertiaAdapter.Interfaces
{
    public interface IResultFactory
    {
        object? Share { get; set; }

        void SetRootView(string s);

        void Version(string version);

        void Version(Func<string> version);

        string? GetVersion();

        IHtmlContent Html(dynamic model);

        Result Render(string component, object controller);
    }
}