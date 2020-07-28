using InertiaAdapter.Core;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using System.Text.Json;

namespace Tests.Feature
{
    public class RazorHtmlTest
    {
        private readonly ITestOutputHelper output;

        public RazorHtmlTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        private void Dump(object o)
        {
            string json = JsonSerializer.Serialize(o);
            output.WriteLine("Dump: {0}", o);
        }

        public static IEnumerable<object[]> GetData()
        {
            yield return new object[] { new { Id = 1 }, 
                "<div id=\"app\" data-page=\"{&quot;component&quot;:&quot;test&quot;,&quot;version&quot;:null,&quot;url&quot;:null,&quot;props&quot;:{&quot;id&quot;:1}}\"></div>" };
            yield return new object[] { new { Id = "Andy" }, 
                "<div id=\"app\" data-page=\"{&quot;component&quot;:&quot;test&quot;,&quot;version&quot;:null,&quot;url&quot;:null,&quot;props&quot;:{&quot;id&quot;:&quot;Andy&quot;}}\"></div>" };                
        }

        public static IEnumerable<object[]> GetDataFull()
        {
            yield return new object[] { 
                new { 
                    Page = "Dashboard",
                    Data = new [] {
                        new {
                            id = 95,
                            name = "Abbott",
                        },
                        new {
                            id = 105,
                            name = "Adams",
                        },
                    }
                },
                new {
                    Id = 1,
                    Username = "Andy",
                },
                new {
                    CustomData = "Boy"
                },
                "<div id=\"app\" data-page=\"{&quot;component&quot;:&quot;test&quot;,&quot;version&quot;:null,&quot;url&quot;:null,&quot;props&quot;:{&quot;page&quot;:&quot;Dashboard&quot;,&quot;data&quot;:[{&quot;id&quot;:95,&quot;name&quot;:&quot;Abbott&quot;},{&quot;id&quot;:105,&quot;name&quot;:&quot;Adams&quot;}],&quot;id&quot;:1,&quot;username&quot;:&quot;Andy&quot;,&quot;customData&quot;:&quot;Boy&quot;}}\"></div>" 
            };
            yield return new object[] { 
                new { 
                    Page = "Dashboard",
                },
                new {
                    Id = 1,
                    Username = "Andy",
                },
                new {
                    CustomData = "Boy"
                },
                "<div id=\"app\" data-page=\"{&quot;component&quot;:&quot;test&quot;,&quot;version&quot;:null,&quot;url&quot;:null,&quot;props&quot;:{&quot;page&quot;:&quot;Dashboard&quot;,&quot;id&quot;:1,&quot;username&quot;:&quot;Andy&quot;,&quot;customData&quot;:&quot;Boy&quot;}}\"></div>"
            };
            yield return new object[] { 
                new { 
                    Page = "Dashboard"
                },
                new {},
                new {}, 
                "<div id=\"app\" data-page=\"{&quot;component&quot;:&quot;test&quot;,&quot;version&quot;:null,&quot;url&quot;:null,&quot;props&quot;:{&quot;page&quot;:&quot;Dashboard&quot;}}\"></div>"
            };
            yield return new object[] { 
                new { 
                    id = 1, 
                    status = new {
                        errors = new {},
                        flash = new {},
                    },
                    auth = new {
                        user = new {
                            
                        }
                    }, 
                    link = new {
                        organization = new {
                            data = "organization/data"
                        }
                    }
                },
                new {},
                new {},
                "<div id=\"app\" data-page=\"{&quot;component&quot;:&quot;test&quot;,&quot;version&quot;:null,&quot;url&quot;:null,&quot;props&quot;:{&quot;id&quot;:1,&quot;status&quot;:{&quot;errors&quot;:{},&quot;flash&quot;:{}},&quot;auth&quot;:{&quot;user&quot;:{}},&quot;link&quot;:{&quot;organization&quot;:{&quot;data&quot;:&quot;organization/data&quot;}}}}\"></div>"
            };
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void ItGeneratesRazorHtmlContent(dynamic model, string expected)
        {
            ResultFactory rf = new ResultFactory();
            Result res = rf.Render("test", model);
            var actual = rf.Html(res.GetMergedPage()).ToString();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(GetDataFull))]
        public void ItGeneratesRazorHtmlContentFull(dynamic modelController, dynamic modelShare, dynamic modelWith, string expected)
        {
            ResultFactory rf = new ResultFactory();
            rf.Share = modelShare;
            Result res = rf.Render("test", modelController);
            res.With(modelWith);
            var actual = rf.Html(res.GetMergedPage()).ToString();

            Assert.Equal(expected, actual);
        }

    }
}