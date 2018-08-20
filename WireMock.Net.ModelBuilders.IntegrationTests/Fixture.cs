using System;
using System.Net.Http;
using WireMock.Client;

namespace WireMock.Net.ModelBuilders.IntegrationTests
{
    public class Fixture
    {
        private static Lazy<Fixture> _instance = new Lazy<Fixture>(() => new Fixture());

        public static Fixture Instance = _instance.Value;

        /// <summary>
        /// True if the integration tests are running inside docker
        /// </summary>
        public bool IsDocker { get; }
        public IFluentMockServerAdmin TestMock { get; }
        public HttpClient TestMockClient { get; }

        public Fixture()
        {
            IsDocker = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOCKER"));

            var testMockUrl = EnvSwitch("http://test-mock", "http://localhost:5010");
            TestMock = RestEase.RestClient.For<IFluentMockServerAdmin>(testMockUrl);
            TestMockClient = new HttpClient() { BaseAddress = new Uri(testMockUrl) };
        }

        private T EnvSwitch<T>(T isDocker, T isLocal)
        {
            return IsDocker ? isDocker : isLocal;
        }
    }
}