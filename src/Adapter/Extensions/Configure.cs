using Adapter.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Adapter.Extensions
{
    public static class Configure
    {
        public static IApplicationBuilder UseInertia(this IApplicationBuilder app)
        {
            Inertia.Init(app.NotNull().ApplicationServices.GetRequiredService<IResultFactory>());


            app.UseWhen(context => context.IsInertiaRequest()
                                   && context.Request.Method == "GET" &&
                                   context.Request.Headers["X-Inertia-Version"] != Inertia.GetVersion(),
                Handler1());

            app.UseWhen(context => context.IsInertiaRequest()
                                   && new[] { "PATCH", "PUT", "DELETE" }.Contains(context.Request.Method), Handler2);


            return app;
        }

        private static Action<IApplicationBuilder> Handler1()
        {
            return app => app.Run(async context =>
            {
                var temData = app.ApplicationServices.GetService<ITempDataDictionaryFactory>()
                    .GetTempData(context);

                if (temData.Count > 0)
                    temData.Keep();

                context.Response.Headers.Add("X-Inertia-Location", context.Request.GetDisplayUrl());
                context.Response.StatusCode = 409;

                await context.Response.CompleteAsync();
            });
        }

        private static void Handler2(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.Response.OnStarting(() =>
                {
                    if (new[] { 302, 301 }.Contains(context.Response.StatusCode))
                        context.Response.StatusCode = 303;
                    return Task.CompletedTask;
                });

                await next.Invoke();
            });
        }
    }
}