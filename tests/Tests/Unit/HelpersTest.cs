using Adapter.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace Tests.Unit
{
    public class HelpersTest
    {
        private readonly ActionContext _actionContext;

        public HelpersTest()
        {
            _actionContext = new ActionContext { HttpContext = new DefaultHttpContext() };
        }

        [Fact]
        public void CheckNotNull()
        {
            const string originalNotNull = "NotNull";

            const string? originalNull = null;

            var valueNotNull = originalNotNull.NotNull();

            Assert.Throws<ArgumentNullException>(() => originalNull.NotNull());

            Assert.Equal(originalNotNull, valueNotNull);
        }

        [Fact]
        public void CheckActionContextIsInertiaRequest()
        {
            _actionContext.HttpContext.Request.Headers.Add("X-Inertia", "true");

            Assert.True(_actionContext.IsInertiaRequest());
        }

        [Fact]
        public void CheckHttpContextIsInertiaRequest()
        {
            _actionContext.HttpContext.Request.Headers.Add("X-Inertia", "true");

            Assert.True(_actionContext.HttpContext.IsInertiaRequest());
        }

        [Fact]
        public void CheckIsNotInertiaRequest()
        {
            Assert.False(_actionContext.IsInertiaRequest());
        }

        [Fact]
        public void ReturnValueFromString()
        {
            var obj = new { Name = "Foo" };

            Assert.Equal("Foo", obj.Value("Name"));

            Assert.Throws<NullReferenceException>(() => obj.Value("NotValidKey"));
        }
    }
}