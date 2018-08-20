using System;
using NFluent;
using WireMock.Net.ModelBuilders;
using WireMock.Net.ModelBuilders.Matchers;
using Xunit;

namespace WireMock.Net.ModelBuilders.UnitTests
{
    public class RequestModelTests
    {
        [Fact]
        public void Build_creates_responsemodel()
        {
            var model = RequestModelBuilder.Create().Build();

            Check.That(model).IsNotNull();
        }

        [Fact]
        public void UsingAnyMethod_should_reset_methods()
        {
            var model =
                RequestModelBuilder.Create()
                    .UsingDelete()
                    .UsingAnyMethod()
                    .Build();

            Check.That(model.Methods).CountIs(0);
        }

        [Fact]
        public void UsingDelete_should_set_method()
        {
            var model = RequestModelBuilder.Create().UsingDelete().Build();

            Check.That(model.Methods).ContainsExactly("delete");
        }

        [Fact]
        public void UsingGet_should_set_method()
        {
            var model = RequestModelBuilder.Create().UsingGet().Build();

            Check.That(model.Methods).ContainsExactly("get");
        }

        [Fact]
        public void UsingHead_should_set_method()
        {
            var model = RequestModelBuilder.Create().UsingHead().Build();

            Check.That(model.Methods).ContainsExactly("head");
        }

        [Fact]
        public void UsingPatch_should_set_method()
        {
            var model = RequestModelBuilder.Create().UsingPatch().Build();

            Check.That(model.Methods).ContainsExactly("patch");
        }

        [Fact]
        public void UsingPost_should_set_method()
        {
            var model = RequestModelBuilder.Create().UsingPost().Build();

            Check.That(model.Methods).ContainsExactly("post");
        }

        [Fact]
        public void UsingPut_should_set_method()
        {
            var model = RequestModelBuilder.Create().UsingPut().Build();

            Check.That(model.Methods).ContainsExactly("put");
        }

        [Fact]
        public void UsingMethod_should_set_methods()
        {
            var model = RequestModelBuilder.Create().UsingMethod("head", "get", "put").Build();

            Check.That(model.Methods).ContainsExactly("head", "get", "put");
        }


        [Fact]
        public void UsingMethod_should_add_to_existing_methods()
        {
            var model =
                RequestModelBuilder.Create()
                    .UsingPost()
                    .UsingMethod("head", "get", "put")
                    .Build();

            Check.That(model.Methods).ContainsExactly("post", "head", "get", "put");
        }

        [Fact]
        public void Using_multiple_methods_all_methods_should_be_set()
        {
            var model =
                RequestModelBuilder.Create()
                    .UsingDelete()
                    .UsingGet()
                    .UsingHead()
                    .UsingPatch()
                    .UsingPost()
                    .UsingPut()
                    .Build();

            Check.That(model.Methods).ContainsExactly("delete", "get", "head", "patch", "post", "put");
        }

        [Fact]
        public void WithBody_sets_correct_matchermodel()
        {
            var model = RequestModelBuilder.Create().WithBody("testPattern", false, true).Build();
            var matcher = model.Body.Matcher;

            Check.That(matcher.Name).IsEqualTo("WildcardMatcher");
            Check.That(matcher.IgnoreCase).IsEqualTo(false);
            Check.That(matcher.Patterns).ContainsExactly("testPattern");
            Check.That(matcher.RejectOnMatch).IsEqualTo(true);
        }

        [Fact]
        public void WithBody_and_matcher_sets_correct_matchermodel()
        {
            var matcher = Matcher.Json(new { test = true });
            var model = RequestModelBuilder.Create().WithBody(matcher).Build();

            Check.That(model.Body.Matcher).HasFieldsWithSameValues(matcher.Build());
        }

        [Fact]
        public void WithClientIP_sets_correct_matchermodels()
        {
            var model = RequestModelBuilder.Create().WithClientIP("127.0.0.1", "192.168.0.1").Build();

            Check.That(model.ClientIP).IsInstanceOf<Admin.Mappings.ClientIPModel>();

            var clientIp = (Admin.Mappings.ClientIPModel)model.ClientIP;
            var matchers = clientIp.Matchers;

            Check.That(matchers).CountIs(2);
            Check.That(matchers[0]).HasFieldsWithSameValues(new Admin.Mappings.MatcherModel
            {
                Name = "WildcardMatcher",
                Patterns = new string[] { "127.0.0.1" },
                IgnoreCase = true,
                RejectOnMatch = false
            });
            Check.That(matchers[1]).HasFieldsWithSameValues(new Admin.Mappings.MatcherModel
            {
                Name = "WildcardMatcher",
                Patterns = new string[] { "192.168.0.1" },
                IgnoreCase = true,
                RejectOnMatch = false
            });
        }

        [Fact]
        public void WithClientIP_and_matcher_sets_correct_matchermodel()
        {
            var matcher = Matcher.Regex("testPattern");
            var model = RequestModelBuilder.Create().WithClientIP(matcher).Build();

            Check.That(model.ClientIP).IsInstanceOf<Admin.Mappings.ClientIPModel>();

            var clientIp = (Admin.Mappings.ClientIPModel)model.ClientIP;
            var matchers = clientIp.Matchers;

            Check.That(matchers).HasOneElementOnly()
                .Which.HasFieldsWithSameValues(matcher.Build());

        }

        [Fact]
        public void WithCookie_sets_correct_matchermodel()
        {
            var model = RequestModelBuilder.Create().WithCookie("cookieName", "cookieValue").Build();

            Check.That(model.Cookies).HasOneElementOnly();

            var cookieModel = model.Cookies[0];
            Check.That(cookieModel.Name).IsEqualTo("cookieName");

            var matchers = cookieModel.Matchers;
            Check.That(matchers).HasOneElementOnly().Which.HasFieldsWithSameValues(new Admin.Mappings.MatcherModel
            {
                Name = "WildcardMatcher",
                Patterns = new string[] { "cookieValue" },
                IgnoreCase = true,
                RejectOnMatch = false
            });
        }

        [Fact]
        public void WithCookie_and_matchers_sets_correct_matchermodels()
        {
            var matcher = Matcher.Exact("exactValue");
            var model = RequestModelBuilder.Create().WithCookie("cookieName", matcher, matcher).Build();

            Check.That(model.Cookies).HasOneElementOnly();

            var cookieModel = model.Cookies[0];
            Check.That(cookieModel.Name).IsEqualTo("cookieName");

            var matchers = cookieModel.Matchers;
            Check.That(matchers).CountIs(2);
            Check.That(matchers).HasElementAt(0).Which.HasFieldsWithSameValues(matcher.Build());
            Check.That(matchers).HasElementAt(1).Which.HasFieldsWithSameValues(matcher.Build());
        }

        [Fact]
        public void WithHeader_sets_correct_matchermodel()
        {
            var model = RequestModelBuilder.Create().WithHeader("headerName", "headerValue").Build();

            Check.That(model.Headers).HasOneElementOnly();
            var headerModel = model.Headers[0];

            Check.That(headerModel.Name).IsEqualTo("headerName");
            Check.That(headerModel.Matchers).HasOneElementOnly()
                .Which.HasFieldsWithSameValues(new Admin.Mappings.MatcherModel
                {
                    Name = "WildcardMatcher",
                    Patterns = new string[] { "headerValue" },
                    IgnoreCase = true,
                    RejectOnMatch = false
                });
        }

        [Fact]
        public void WithHeader_and_matcher_sets_correct_matchermodel()
        {
            var matcher = Matcher.Regex("regex");
            var model = RequestModelBuilder.Create().WithHeader("headerName", matcher).Build();

            Check.That(model.Headers).HasOneElementOnly();
            var headerModel = model.Headers[0];

            Check.That(headerModel.Name).IsEqualTo("headerName");
            Check.That(headerModel.Matchers).HasOneElementOnly()
                .Which.HasFieldsWithSameValues(matcher.Build());
        }

        [Fact]
        public void WithParam_sets_correct_matchermodel()
        {
            var model = RequestModelBuilder.Create().WithParam("paramName").Build();

            Check.That(model.Params).HasOneElementOnly();
            var paramModel = model.Params[0];

            Check.That(paramModel.Name).IsEqualTo("paramName");
            Check.That(paramModel.Matchers).HasOneElementOnly()
                .Which.HasFieldsWithSameValues(new Admin.Mappings.MatcherModel
                {
                    Name = "WildcardMatcher",
                    Patterns = new string[] { "*" },
                    IgnoreCase = true,
                    RejectOnMatch = false
                });
        }

        [Fact]
        public void WithParam_and_value_sets_correct_matchermodel()
        {
            var model = RequestModelBuilder.Create().WithParam("paramName", "paramValue").Build();

            Check.That(model.Params).HasOneElementOnly();
            var paramModel = model.Params[0];

            Check.That(paramModel.Name).IsEqualTo("paramName");
            Check.That(paramModel.Matchers).HasOneElementOnly()
                .Which.HasFieldsWithSameValues(new Admin.Mappings.MatcherModel
                {
                    Name = "ExactMatcher",
                    Patterns = new string[] { "paramValue" },
                    IgnoreCase = null,
                    RejectOnMatch = false
                });
        }

        [Fact]
        public void WithParam_and_matcher_sets_correct_matchermodel()
        {
            var matcher = Matcher.Wildcard("paramValue");
            var model = RequestModelBuilder.Create().WithParam("paramName", matcher).Build();

            Check.That(model.Params).HasOneElementOnly();
            var paramModel = model.Params[0];

            Check.That(paramModel.Name).IsEqualTo("paramName");
            Check.That(paramModel.Matchers).HasOneElementOnly()
                .Which.HasFieldsWithSameValues(matcher.Build());
        }

        [Fact]
        public void WithPath_sets_correct_matchermodel()
        {
            var model = RequestModelBuilder.Create().WithPath("/testPath").Build();

            Check.That(model.Path).IsInstanceOf<Admin.Mappings.PathModel>();

            var pathModel = (Admin.Mappings.PathModel)model.Path;
            Check.That(pathModel.Matchers).HasOneElementOnly()
                .Which.HasFieldsWithSameValues(new Admin.Mappings.MatcherModel
                {
                    Name = "WildcardMatcher",
                    Patterns = new string[] { "/testPath" },
                    IgnoreCase = true,
                    RejectOnMatch = false
                });
        }

        [Fact]
        public void WithPath_and_matcher_sets_correct_matchermodel()
        {
            var matcher = Matcher.Exact("/testPath");
            var model = RequestModelBuilder.Create().WithPath(matcher).Build();

            Check.That(model.Path).IsInstanceOf<Admin.Mappings.PathModel>();

            var pathModel = (Admin.Mappings.PathModel)model.Path;
            Check.That(pathModel.Matchers).HasOneElementOnly()
                .Which.HasFieldsWithSameValues(matcher.Build());
        }

        [Fact]
        public void WithUrl_sets_correct_matchermodel()
        {
            var model = RequestModelBuilder.Create().WithUrl("testUrl").Build();

            Check.That(model.Url).IsInstanceOf<Admin.Mappings.UrlModel>();

            var urlModel = (Admin.Mappings.UrlModel)model.Url;
            Check.That(urlModel.Matchers).HasOneElementOnly()
                .Which.HasFieldsWithSameValues(new Admin.Mappings.MatcherModel
                {
                    Name = "WildcardMatcher",
                    Patterns = new string[] { "testUrl" },
                    IgnoreCase = true,
                    RejectOnMatch = false
                });
        }

        [Fact]
        public void WithUrl()
        {
            var matcher = Matcher.Exact("testUrl");
            var model = RequestModelBuilder.Create().WithUrl(matcher).Build();

            Check.That(model.Url).IsInstanceOf<Admin.Mappings.UrlModel>();

            var urlModel = (Admin.Mappings.UrlModel)model.Url;
            Check.That(urlModel.Matchers).HasOneElementOnly()
                .Which.HasFieldsWithSameValues(matcher.Build());
        }
    }
}
