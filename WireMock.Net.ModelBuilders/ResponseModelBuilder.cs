using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace WireMock.Net.ModelBuilders
{
    /// <summary>
    /// Fluent builder for <see cref="Admin.Mappings.ResponseModel" />
    /// </summary>
    public class ResponseModelBuilder
    {
        private readonly Admin.Mappings.ResponseModel _responseModel;

        /// <summary>
        /// Creates an instance of the <see cref="ResponseModelBuilder" />
        /// </summary>

        public ResponseModelBuilder()
        {
            _responseModel = new Admin.Mappings.ResponseModel
            {
                Headers = new Dictionary<string, object>(),
                StatusCode = 200
            };
        }

        /// <summary>
        /// Begin building a <see cref="Admin.Mappings.ResponseModel"/>.
        /// </summary>
        /// <returns>The <see cref="ResponseModelBuilder"/>.</returns>
        public static ResponseModelBuilder Create()
        {
            return new ResponseModelBuilder();
        }

        /// <summary>
        /// Builds the <see cref="Admin.Mappings.ResponseModel" />
        /// </summary>
        /// <returns>The <see cref="Admin.Mappings.ResponseModel" />.</returns>
        public Admin.Mappings.ResponseModel Build()
        {
            return _responseModel;
        }

        /// <summary>
        /// The response status code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>A <see cref="ResponseModelBuilder"/>.</returns>
        public ResponseModelBuilder WithStatusCode(int code)
        {
            _responseModel.StatusCode = code;

            return this;
        }

        /// <summary>
        /// The response status code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>A <see cref="ResponseModelBuilder"/>.</returns>
        public ResponseModelBuilder WithStatusCode(HttpStatusCode code)
        {
            return WithStatusCode((int)code);
        }

        /// <summary>
        /// With Success status code (200).
        /// </summary>
        /// <returns>A <see cref="ResponseModelBuilder"/>.</returns>
        public ResponseModelBuilder WithSuccess()
        {
            return WithStatusCode((int)HttpStatusCode.OK);
        }

        /// <summary>
        /// With NotFound status code (404).
        /// </summary>
        /// <returns>The <see cref="ResponseModelBuilder"/>.</returns>
        public ResponseModelBuilder WithNotFound()
        {
            return WithStatusCode((int)HttpStatusCode.NotFound);
        }

        /// <summary>
        /// With header.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        /// <returns>The <see cref="ResponseModelBuilder"/>.</returns>
        public ResponseModelBuilder WithHeader(string name, params string[] values)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (values == null) throw new ArgumentNullException(nameof(values));

            _responseModel.Headers[name] = _responseModel.Headers.TryGetValue(name, out object val) ?
                ((string[])val).Union(values) :
                values;

            return this;
        }

        /// <summary>
        /// With headers.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <returns>The <see cref="ResponseModelBuilder"/>.</returns>
        public ResponseModelBuilder WithHeaders(IDictionary<string, string> headers)
        {
            if (headers == null) throw new ArgumentNullException(nameof(headers));

            _responseModel.Headers = headers.ToDictionary(x => x.Key, x => (object)new string[] { x.Value });
            return this;
        }

        /// <summary>
        /// With headers.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <returns>The <see cref="ResponseModelBuilder"/>.</returns>
        public ResponseModelBuilder WithHeaders(IDictionary<string, string[]> headers)
        {
            if (headers == null) throw new ArgumentNullException(nameof(headers));

            _responseModel.Headers = headers.ToDictionary(x => x.Key, x => (object)x.Value);

            return this;
        }

        /// <summary>
        /// With body from file.
        /// </summary>
        /// <param name="filename">The path to the file.</param>
        /// <returns>The <see cref="ResponseModelBuilder" />.</returns>
        public ResponseModelBuilder WithBodyFromFile(string filename)
        {
            if (filename == null) throw new ArgumentNullException(nameof(filename));

            ResetBody();
            _responseModel.BodyAsBytes = File.ReadAllBytes(filename);

            return this;
        }

        /// <summary>
        /// Create a response based on a remote File.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>A <see cref="ResponseModelBuilder"/>.</returns>
        public ResponseModelBuilder WithBodyFromRemoteFile(string filename)
        {
            if (filename == null) throw new ArgumentNullException(nameof(filename));

            ResetBody();
            _responseModel.BodyAsFile = filename;

            return this;
        }

        /// <summary>
        /// Create a response based on a string body.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>A <see cref="ResponseModelBuilder" />.</returns>
        public ResponseModelBuilder WithBody(string body, Encoding encoding = null)
        {
            if (body == null) throw new ArgumentNullException(nameof(body));

            encoding = encoding ?? Encoding.UTF8;

            ResetBody();
            _responseModel.Body = body;
            _responseModel.BodyEncoding = CreateEncodingModel(encoding);

            return this;
        }

        /// <summary>
        /// Creates a based on a body as bytes.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>A <see cref="ResponseModelBuilder" />.</returns>
        public ResponseModelBuilder WithBodyAsBytes(byte[] body, Encoding encoding = null)
        {
            if (body == null) throw new ArgumentNullException(nameof(body));

            encoding = encoding ?? Encoding.UTF8;

            _responseModel.BodyDestination = null;
            _responseModel.BodyAsBytes = body;
            _responseModel.BodyEncoding = CreateEncodingModel(encoding);

            return this;
        }

        /// <summary>
        /// Create a string response based on a object (which will be converted to a JSON string).
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="encoding">The body encoding.</param>
        /// <param name="indented">Use JSON indented.</param>
        /// <returns>A <see cref="ResponseModelBuilder"/>.</returns>
        public ResponseModelBuilder WithBodyAsJson(object body, Encoding encoding = null, bool? indented = null)
        {
            if (body == null) throw new ArgumentNullException(nameof(body));

            encoding = encoding ?? Encoding.UTF8;

            ResetBody();
            _responseModel.BodyAsJson = body;
            _responseModel.BodyAsJsonIndented = indented;
            _responseModel.BodyEncoding = CreateEncodingModel(encoding);

            return this;
        }

        /// <summary>
        /// Create a string response based on a object (which will be converted to a JSON string).
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="indented">Use JSON indented.</param>
        /// <returns>A <see cref="ResponseModelBuilder"/>.</returns>
        public ResponseModelBuilder WithBodyAsJson(object body, bool indented)
        {
            return WithBodyAsJson(body, null, indented);
        }

        /// <summary>
        /// Use a transformer.
        /// </summary>
        /// <returns>
        /// The <see cref="ResponseModelBuilder"/>.
        /// </returns>
        public ResponseModelBuilder WithTransformer()
        {
            _responseModel.UseTransformer = true;

            return this;
        }

        /// <summary>
        /// The response delay.
        /// </summary>
        /// <param name="delay">The TimeSpan to delay.</param>
        /// <returns>The <see cref="ResponseModelBuilder"/>.</returns>
        public ResponseModelBuilder WithDelay(TimeSpan delay)
        {
            _responseModel.Delay = (int)delay.TotalMilliseconds;

            return this;
        }

        /// <summary>
        /// The response delay.
        /// </summary>
        /// <param name="milliseconds">The milliseconds to delay.</param>
        /// <returns>The <see cref="ResponseModelBuilder"/>.</returns>
        public ResponseModelBuilder WithDelay(int milliseconds)
        {
            return WithDelay(TimeSpan.FromMilliseconds(milliseconds));
        }

        /// <summary>
        /// Proxy URL using Client X509Certificate2.
        /// </summary>
        /// <param name="proxyUrl">The proxy url.</param>
        /// <param name="clientX509Certificate2ThumbprintOrSubjectName">The X509Certificate2 file to use for client authentication.</param>
        /// <returns>A <see cref="ResponseModelBuilder"/>.</returns>
        public ResponseModelBuilder WithProxy(string proxyUrl, string clientX509Certificate2ThumbprintOrSubjectName = null)
        {
            _responseModel.ProxyUrl = proxyUrl;
            _responseModel.X509Certificate2ThumbprintOrSubjectName = clientX509Certificate2ThumbprintOrSubjectName;

            return this;
        }

        private Admin.Mappings.EncodingModel CreateEncodingModel(Encoding encoding)
        {
            return new Admin.Mappings.EncodingModel
            {
                CodePage = encoding.CodePage,
                EncodingName = encoding.EncodingName,
                WebName = encoding.WebName
            };
        }

        private void ResetBody()
        {
            _responseModel.Body = null;
            _responseModel.BodyAsBytes = null;
            _responseModel.BodyAsFile = null;
            _responseModel.BodyAsFileIsCached = null;
            _responseModel.BodyAsJson = null;
            _responseModel.BodyAsJsonIndented = null;
            _responseModel.BodyDestination = null;
            _responseModel.BodyEncoding = null;
            _responseModel.BodyFromBase64 = null;
        }
    }
}