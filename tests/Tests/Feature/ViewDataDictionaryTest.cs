using InertiaAdapter.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using Xunit;

namespace Tests.Feature
{
    public class ViewDataDictionaryTest
    {
        private readonly ActionContext _actionContext;

        public ViewDataDictionaryTest() =>
            _actionContext = new ActionContext { HttpContext = new DefaultHttpContext() };

        [Fact]
        public void NullDataReturnsEmptyList()
        {
            var vd = new ViewData(null, _actionContext, null);

            var dictionary = vd.ViewDataDictionary;

            Assert.Empty(vd.ViewDataDictionary);
            Assert.IsType<ViewDataDictionary>(dictionary);
        }

        [Fact]
        public void EmptyDataReturnsEmptyList()
        {
            var empty = new Dictionary<string, object>();

            var vd = new ViewData(null, _actionContext, empty);

            var dictionary = vd.ViewDataDictionary;

            Assert.Empty(vd.ViewDataDictionary);
            Assert.IsType<ViewDataDictionary>(dictionary);
        }

        [Fact]
        public void OneRecordReturnsEmptyList()
        {
            var one = new Dictionary<string, object> { { "Foo", "Bar" } };

            var vd = new ViewData(null, _actionContext, one);

            var dictionary = vd.ViewDataDictionary;

            Assert.Single(dictionary);
            Assert.IsType<ViewDataDictionary>(dictionary);
            Assert.Equal("Bar", dictionary["Foo"]);
        }

        [Fact]
        public void TwoRecordsReturnEmptyList()
        {
            var two = new Dictionary<string, object> { { "Foo1", "Bar1" }, { "Foo2", "Bar2" } };

            var vd = new ViewData(null, _actionContext, two);

            var dictionary = vd.ViewDataDictionary;

            Assert.Equal(2, dictionary.Count);
            Assert.IsType<ViewDataDictionary>(dictionary);
            Assert.Equal("Bar1", dictionary["Foo1"]);
            Assert.Equal("Bar2", dictionary["Foo2"]);
        }
    }
}