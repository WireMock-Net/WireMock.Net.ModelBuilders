using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using NFluent;
using WireMock.Net.ModelBuilders;
using Xunit;

namespace WireMock.Net.ModelBuilders.UnitTests
{
    public class ResponseModelTests
    {
        [Fact]
        public void Build_creates_responsemodel()
        {
            var model = ResponseModelBuilder.Create().Build();

            Check.That(model).IsNotNull();
        }

        [Fact]
        public void WithBody_sets_body()
        {
            var model = ResponseModelBuilder.Create().WithBody("body").Build();

            Check.That(model.Body).IsEqualTo("body");
            CheckEncoding(model.BodyEncoding, Encoding.UTF8);
        }

        [Fact]
        public void WithBodyAsBytes_sets_bodyasbytes()
        {
            var testBytes = Encoding.UTF32.GetBytes("test");
            var model = ResponseModelBuilder.Create().WithBodyAsBytes(testBytes, Encoding.UTF32).Build();

            Check.That(model.BodyAsBytes).IsEqualTo(testBytes);
            CheckEncoding(model.BodyEncoding, Encoding.UTF32);
        }

        [Fact]
        public void WithBodyAsJson_sets_bodyasjson()
        {
            var testObj = new { test = true };
            var model = ResponseModelBuilder.Create().WithBodyAsJson(testObj).Build();

            Check.That(model.BodyAsJson).IsEqualTo(testObj);
            CheckEncoding(model.BodyEncoding, Encoding.UTF8);
        }

        [Fact]
        public void WithBodyFromRemoteFile_sets_bodyasfile()
        {
            var model = ResponseModelBuilder.Create().WithBodyFromRemoteFile("test.txt").Build();

            Check.That(model.BodyAsFile).IsEqualTo("test.txt");
        }

        [Fact]
        public void WithDelay_timespan_should_set_delay()
        {
            var model = ResponseModelBuilder.Create().WithDelay(TimeSpan.FromMilliseconds(2000)).Build();

            Check.That(model.Delay).IsEqualTo(2000);
        }

        [Fact]
        public void WithDelay_ms_should_set_delay()
        {
            var model = ResponseModelBuilder.Create().WithDelay(5000).Build();

            Check.That(model.Delay).IsEqualTo(5000);
        }

        [Fact]
        public void WithHeader_sets_header()
        {
            var model = ResponseModelBuilder.Create().WithHeader("headerName", "headerValue").Build();

            Check.That(model.Headers).HasOneElementOnly();
            Check.That(model.Headers).ContainsPair("headerName", new string[] { "headerValue" });
        }

        [Fact]
        public void WithHeader_with_multiple_values_sets_header()
        {
            var model = ResponseModelBuilder.Create().WithHeader("headerName", "headerValue1", "headerValue2").Build();

            Check.That(model.Headers)
                .HasOneElementOnly()
                .And.ContainsPair("headerName", new string[] { "headerValue1", "headerValue2" });
        }

        [Fact]
        public void WithHeader_multiple_sets_headers()
        {
            var model =
                ResponseModelBuilder.Create()
                    .WithHeader("headerName1", "headerValue1")
                    .WithHeader("headerName2", "headerValue2")
                    .Build();

            Check.That(model.Headers).CountIs(2);
            Check.That(model.Headers)
                .ContainsPair("headerName1", new string[] { "headerValue1" })
                .And.ContainsPair("headerName2", new string[] { "headerValue2" });
        }

        [Fact]
        public void WithHeaders_sets_headers()
        {
            var headers = new Dictionary<string, string> { { "headerName", "headerValue" } };
            var model = ResponseModelBuilder.Create().WithHeaders(headers).Build();

            Check.That(model.Headers).HasOneElementOnly();
            Check.That(model.Headers).ContainsPair("headerName", new string[] { "headerValue" });
        }

        [Fact]
        public void WithHeaders_and_values_sets_headers_and_values()
        {
            var headers = new Dictionary<string, string[]> { { "headerName", new string[] { "headerValue1", "headerValue2" } }};
            var model = ResponseModelBuilder.Create().WithHeaders(headers).Build();

            Check.That(model.Headers).HasOneElementOnly();
            Check.That(model.Headers).ContainsPair("headerName", new string[] { "headerValue1", "headerValue2" });
        }

        [Fact]
        public void WithNotFound_sets_statuscode_404()
        {
            var model = ResponseModelBuilder.Create().WithNotFound().Build();

            Check.That(model.StatusCode).IsEqualTo(404);
        }

        [Fact]
        public void WithProxy_sets_proxyurl_and_x509thumbprint()
        {
            var model = ResponseModelBuilder.Create().WithProxy("proxyUrl", "thumbprint").Build();

            Check.That(model.ProxyUrl).IsEqualTo("proxyUrl");
            Check.That(model.X509Certificate2ThumbprintOrSubjectName).IsEqualTo("thumbprint");
        }

        [Fact]
        public void WithStatusCode_int_sets_statuscode()
        {
            var model = ResponseModelBuilder.Create().WithStatusCode(500).Build();

            Check.That(model.StatusCode).IsEqualTo(500);
        }

        [Fact]
        public void WithStatusCode_httpstatuscode_set_statuscode()
        {
            var model = ResponseModelBuilder.Create().WithStatusCode(HttpStatusCode.Accepted).Build();

            Check.That(model.StatusCode).IsEqualTo(202);
        }

        [Fact]
        public void WithSuccess_sets_statuscode_200()
        {
            var model = ResponseModelBuilder.Create().WithSuccess().Build();

            Check.That(model.StatusCode).IsEqualTo(200);
        }

        [Fact]
        public void WithTransformer_sets_usetransformer_true()
        {
            var model = ResponseModelBuilder.Create().WithTransformer().Build();

            Check.That(model.UseTransformer).IsTrue();
        }

        private void CheckEncoding(Admin.Mappings.EncodingModel encodingModel, Encoding encoding)
        {
            Check.That(encodingModel.CodePage).IsEqualTo(encoding.CodePage);
            Check.That(encodingModel.EncodingName).IsEqualTo(encoding.EncodingName);
            Check.That(encodingModel.WebName).IsEqualTo(encoding.WebName);
        }
    }
}