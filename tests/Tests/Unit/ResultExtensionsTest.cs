using InertiaAdapter.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests.Unit
{
    public class ResultExtensionsTest
    {
        private readonly ActionContext _actionContext;

        public ResultExtensionsTest()
        {
            _actionContext = new ActionContext { HttpContext = new DefaultHttpContext() };
        }

        [Fact]
        public void ReturnComponentName()
        {
            _actionContext.HttpContext.Request.Headers.Add("X-Inertia-Partial-Component", "Component");

            Assert.Equal("Component", _actionContext.ComponentName());

            Assert.NotEqual("WrongComponent", _actionContext.ComponentName());
        }

        [Fact]
        public void SetCorrectResponseForJson()
        {
            _actionContext.ConfigureResponse();

            Assert.Equal("Accept", _actionContext.HttpContext.Response.Headers["Vary"]);
            Assert.Equal("true", _actionContext.HttpContext.Response.Headers["X-Inertia"]);
            Assert.Equal(200, _actionContext.HttpContext.Response.StatusCode);
        }


        [Fact]
        public void SetCorrect409Response()
        {
            _actionContext.HttpContext.Configure409Response();

            Assert.NotEmpty(_actionContext.HttpContext.Response.Headers["X-Inertia-Location"]);
            Assert.Equal(409, _actionContext.HttpContext.Response.StatusCode);
        }

        [Fact]
        public void CheckObjectIsLazy()
        {
            var notLazy = new { Id = 1 };

            var lazy = new Lazy<object>(() => new { Id = 1 });

            Assert.True(lazy.IsLazy());

            Assert.False(notLazy.IsLazy());
        }

        [Theory]
        [InlineData("foo,bar", new[] { "foo", "bar" })]
        [InlineData("foo,bar,baz", new[] { "foo", "bar", "baz" })]
        [InlineData("foo", new[] { "foo" })]
        [InlineData(null, new string[] { })]
        public void GetPartialData(string str, IList<string> ex)
        {
            _actionContext.HttpContext.Request.Headers.Add("X-Inertia-Partial-Data", str);

            Assert.Equal(ex, _actionContext.GetPartialData());
        }

        [Fact]
        public void GetSharedListStringFromObjectProperties()
        {
            var obj = new { Id = 1, Name = "Foo" };
            object? nullObject = null;
            var list = new List<string> { "Name" };
            var emptyList = new List<string>();

            Assert.Equal(new List<string> { "Name" }, obj.Only(list));
            Assert.Equal(new List<string>(), obj.Only(emptyList));
            Assert.Equal(new List<string>(), nullObject.Only(emptyList));
        }

        [Theory]
        [InlineData("https://example.com/Home?q=search")]
        [InlineData("https://example.com/Home?q=search&s=test")]
        [InlineData("https://example.com/Home")]
        [InlineData("https://example.com/Home/Index")]
        public void GetRequestedUri(string str)
        {
            var url = new Uri(str);

            var pathAndQuery = url.PathAndQuery;
            _actionContext.HttpContext.Request.Path = pathAndQuery;

            Assert.Equal(pathAndQuery, _actionContext.RequestedUri());
        }
    }
}