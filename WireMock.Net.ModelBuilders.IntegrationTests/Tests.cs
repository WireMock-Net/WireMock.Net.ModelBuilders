using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NFluent;
using WireMock.Admin.Mappings;
using WireMock.Net.ModelBuilders;
using WireMock.Net.ModelBuilders.Matchers;
using Xunit;

namespace WireMock.Net.ModelBuilders.IntegrationTests
{
    public class Tests
    {
        private readonly Fixture _f;

        public Tests()
        {
            _f = Fixture.Instance;
        }

        [Fact]
        public async Task Get_with_response_body()
        {
            var id = Guid.NewGuid();

            var mapping = new MappingModel
            {
                Request = RequestModelBuilder.Create()
                    .UsingGet()
                    .WithPath($"/test-get/{id}")
                    .Build(),
                Response = ResponseModelBuilder.Create()
                    .WithSuccess()
                    .WithBody(id.ToString())
                    .Build()
            };

            await _f.TestMock.PostMappingAsync(mapping);

            var resp = await _f.TestMockClient.GetAsync($"/test-get/{id}");
            var respBody = await resp.Content.ReadAsStringAsync();
            Check.That(resp.StatusCode).IsEqualTo(HttpStatusCode.OK);
            Check.That(respBody).IsEqualTo(id);
        }

        [Fact]
        public async Task Post_with_response_body()
        {
            var id = Guid.NewGuid();

            var mapping = new MappingModel
            {
                Request = RequestModelBuilder.Create()
                    .UsingPost()
                    .WithPath($"/test-post/{id}")
                    .WithBody($"*{id}*")
                    .Build(),
                Response = ResponseModelBuilder.Create()
                    .WithBody(id.ToString())
                    .WithSuccess()
                    .Build()
            };

            await _f.TestMock.PostMappingAsync(mapping);

            var resp = await _f.TestMockClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, $"/test-post/{id}")
            {
                Content = new StringContent($"{{ \"id'\": \"${id}\" }}", Encoding.UTF8, "application/json")
            });

            var respBody = await resp.Content.ReadAsStringAsync();
            Check.That(resp.StatusCode).IsEqualTo(HttpStatusCode.OK);
            Check.That(respBody).IsEqualTo(id);
        }

        [Fact]
        public async Task Put_with_exact_body_matcher()
        {
            var id = Guid.NewGuid();

            var mapping = new MappingModel
            {
                Request = RequestModelBuilder.Create()
                    .UsingPut()
                    .WithPath($"/test-put/{id}")
                    .WithBody(Matcher.Exact($"test-{id}"))
                    .Build(),
                Response = ResponseModelBuilder.Create()
                    .WithSuccess()
                    .Build()
            };

            await _f.TestMock.PostMappingAsync(mapping);

            var resp = await _f.TestMockClient.SendAsync(new HttpRequestMessage(HttpMethod.Put, $"/test-put/{id}")
            {
                Content = new StringContent($"test-{id}", Encoding.UTF8, "application/json")
            });

            var respBody = await resp.Content.ReadAsStringAsync();
            Check.That(resp.StatusCode).IsEqualTo(HttpStatusCode.OK);
        }
    }
}
