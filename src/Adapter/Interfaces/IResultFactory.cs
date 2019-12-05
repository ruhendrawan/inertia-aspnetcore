using System;
using Microsoft.AspNetCore.Mvc;

namespace Adapter.Interfaces
{
    public interface IResultFactory
    {
        object? Share { get; set; }

        void SetRootView(string s);
        
        void Version(string version);
        
        void Version(Func<string> version);
        
        string? GetVersion();
        
        IActionResult Render(string component, object controller);
    }
}