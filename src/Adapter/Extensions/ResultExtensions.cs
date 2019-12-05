using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adapter.Extensions
{
    internal static class ResultExtensions
    {
        internal static string ComponentName(this ActionContext? ac) =>
            ac.NotNull().HttpContext.Request.Headers["X-Inertia-Partial-Component"];

        internal static IList<string> Only(this object? obj, IList<string> list) =>
            obj?.GetType().GetProperties().Select(c => c.Name).Intersect(list).ToList() ??
            new List<string>();

        internal static bool IsLazy(this object obj) =>
            obj.GetType().IsGenericType && obj.GetType().GetGenericTypeDefinition() == typeof(Lazy<>);

        internal static void ConfigureResponse(this ActionContext? ac)
        {
            ac.NotNull().HttpContext.Response.Headers.Add("Vary", "Accept");
            ac.NotNull().HttpContext.Response.Headers.Add("X-Inertia", "true");
            ac.NotNull().HttpContext.Response.StatusCode = 200;
        }

        internal static string RequestedUrl(this ActionContext? ac) =>
            Uri.UnescapeDataString(ac.NotNull().HttpContext.Request.GetEncodedPathAndQuery());


        internal static IList<string> GetPartialData(this ActionContext? ac) =>
            ac
                .NotNull()
                .HttpContext
                .Request
                .Headers["X-Inertia-Partial-Data"]
                .FirstOrDefault()?
                .Split(",")
                .Where(c => !string.IsNullOrEmpty(c))
                .ToList() ?? new List<string>();
    }
}