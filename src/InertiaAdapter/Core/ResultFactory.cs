﻿using InertiaAdapter.Interfaces;
using InertiaAdapter.Models;
using Microsoft.AspNetCore.Html;
using System;
using System.Text.Json;
using System.Web;

namespace InertiaAdapter.Core
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

        public IHtmlContent Html(dynamic model)
        {  
            string data = 
                HttpUtility.HtmlEncode(
                    JsonSerializer.Serialize(
                        model,
                        new JsonSerializerOptions {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    })
                );

            return new HtmlString($"<div id=\"app\" data-page=\"{data}\"></div>");
        }

        public Result Render(string component, object controller) =>
            new Result(
                new Props { 
                    Controller = controller, 
                    Share = Share 
                }, 
                component, 
                _rootView, 
                GetVersion()
            );
    }
}