using System;
using System.Collections.Generic;
using System.Linq;
using WireMock.Net.ModelBuilders.Matchers;

namespace WireMock.Net.ModelBuilders
{
    /// <summary>
    /// Fluent builder for <see cref="Admin.Mappings.RequestModel" />.
    /// </summary>
    public class RequestModelBuilder
    {
        private readonly Admin.Mappings.RequestModel _requestModel;

        /// <summary>
        /// Creates an instance of the <see cref="RequestModelBuilder" />.
        /// </summary>
        public RequestModelBuilder()
        {
            _requestModel = new Admin.Mappings.RequestModel
            {
                Methods = new string[0],
                Headers = new List<Admin.Mappings.HeaderModel>(),
                Cookies = new List<Admin.Mappings.CookieModel>(),
                Params = new List<Admin.Mappings.ParamModel>()
            };
        }

        /// <summary>
        /// Begin building a <see cref="Admin.Mappings.RequestModel"/>.
        /// </summary>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public static RequestModelBuilder Create()
        {
            return new RequestModelBuilder();
        }

        /// <summary>
        /// Builds the <see cref="Admin.Mappings.RequestModel" />.
        /// </summary>
        /// <returns>The <see cref="Admin.Mappings.RequestModel" />.</returns>
        public Admin.Mappings.RequestModel Build()
        {
            return _requestModel;
        }
        
        /// <summary>
        /// Add HTTP Method matching on any method.
        /// </summary>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder UsingAnyMethod()
        {
            _requestModel.Methods = new string[0];

            return this;
        }

        /// <summary>
        /// Add HTTP Method matching on `delete`.
        /// </summary>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder UsingDelete()
        {
            _requestModel.Methods = _requestModel.Methods.Union(new string[] { "delete" }).ToArray();

            return this;
        }

        /// <summary>
        /// Add HTTP Method matching on `get`.
        /// </summary>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder UsingGet()
        {
            _requestModel.Methods = _requestModel.Methods.Union(new string[] { "get" }).ToArray();

            return this;
        }

        /// <summary>
        /// Add HTTP Method matching on `head`.
        /// </summary>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder UsingHead()
        {
            _requestModel.Methods = _requestModel.Methods.Union(new string[] { "head" }).ToArray();

            return this;
        }

        /// <summary>
        /// Add HTTP Method matching on any methods.
        /// </summary>
        /// <param name="methods">The methods.</param>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder UsingMethod(params string[] methods)
        {
            if (methods == null) throw new ArgumentNullException(nameof(methods));

            _requestModel.Methods = _requestModel.Methods.Union(methods).ToArray();

            return this;
        }

        /// <summary>
        /// Add HTTP Method matching on `patch`.
        /// </summary>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder UsingPatch()
        {
            _requestModel.Methods = _requestModel.Methods.Union(new string[] { "patch" }).ToArray();

            return this;
        }

        /// <summary>
        /// Add HTTP Method matching on `post`.
        /// </summary>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder UsingPost()
        {
            _requestModel.Methods = _requestModel.Methods.Union(new string[] { "post" }).ToArray();

            return this;
        }

        /// <summary>
        /// Add HTTP Method matching on `put`.
        /// </summary>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder UsingPut()
        {
            _requestModel.Methods = _requestModel.Methods.Union(new string[] { "put" }).ToArray();

            return this;
        }

        /// <summary>
        /// Wildcard string matching the body.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="ignoreCase">Ignore the case from the pattern.</param>
        /// <param name="rejectOnMatch">The match behaviour.</param>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder WithBody(string pattern, bool ignoreCase = true, bool rejectOnMatch = false)
        {
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            return WithBody(Matcher.Wildcard(ignoreCase, rejectOnMatch, pattern));
        }

        /// <summary>
        /// Body matcher.
        /// </summary>
        /// <param name="matcher">The matcher.</param>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder WithBody(IMatcherModelBuilder matcher)
        {
            if (matcher == null) throw new ArgumentNullException(nameof(matcher));

            _requestModel.Body = new Admin.Mappings.BodyModel()
            {
                Matcher = matcher.Build()
            };

            return this;
        }

        /// <summary>
        /// Add matching on clientIPs.
        /// </summary>
        /// <param name="clientIPs">The clientIPs.</param>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder WithClientIP(params string[] clientIPs)
        {
            return WithClientIP(false, clientIPs);
        }

        /// <summary>
        /// Add matching on clientIPs and matchBehaviour.
        /// </summary>
        /// <param name="rejectOnMatch">The match behaviour.</param>
        /// <param name="clientIPs">The clientIPs.</param>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder WithClientIP(bool rejectOnMatch = false, params string[] clientIPs)
        {
            if (clientIPs == null) throw new ArgumentNullException(nameof(clientIPs));

            return WithClientIP(clientIPs.Select(x => Matcher.Wildcard(true, rejectOnMatch, x)).ToArray());
        }

        /// <summary>
        /// Add matching on ClientIP matchers.
        /// </summary>
        /// <param name="matchers">The matchers.</param>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder WithClientIP(params IStringMatcherModelBuilder[] matchers)
        {
            if (matchers == null) throw new ArgumentNullException(nameof(matchers));

            _requestModel.ClientIP = new Admin.Mappings.ClientIPModel
            {
                Matchers = matchers.Select(x => x.Build()).ToArray()
            };

            return this;
        }

        /// <summary>
        /// Matching based on name, pattern, ignoreCase and rejectOnMatch.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="ignoreCase">Ignore the case from the pattern.</param>
        /// <param name="rejectOnMatch">The match behaviour.</param>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder WithCookie(string name, string pattern, bool ignoreCase = true, bool rejectOnMatch = false)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            return WithCookie(name, Matcher.Wildcard(ignoreCase, rejectOnMatch, pattern));
        }

        /// <summary>
        /// Matching based on name and matchers.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="matchers">The matchers.</param>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder WithCookie(string name, params IStringMatcherModelBuilder[] matchers)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (matchers == null) throw new ArgumentNullException(nameof(matchers));

            _requestModel.Cookies.Add(new Admin.Mappings.CookieModel
            {
                Name = name,
                Matchers = matchers.Select(x => x.Build()).ToList()
            });

            return this;
        }

        /// <summary>
        /// Matching based on name, patterns.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="patterns">The patterns.</param>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder WithHeader(string name, params string[] patterns)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (patterns == null) throw new ArgumentNullException(nameof(patterns));

            return WithHeader(name, true, false, patterns);
        }

        /// <summary>
        /// Matching based on name, patterns, ignoreCase and rejectOnMatch.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="ignoreCase">Ignore the case from the pattern.</param>
        /// <param name="rejectOnMatch">The match behaviour.</param>
        /// <param name="patterns">The patterns.</param>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder WithHeader(string name, bool ignoreCase = true, bool rejectOnMatch = false, params string[] patterns)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (patterns == null) throw new ArgumentNullException(nameof(patterns));

            return WithHeader(name, patterns.Select(x => Matcher.Wildcard(ignoreCase, rejectOnMatch, x)).ToArray());
        }

        /// <summary>
        /// Matching based on name and matchers.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="matchers">The matchers.</param>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder WithHeader(string name, params IStringMatcherModelBuilder[] matchers)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (matchers == null) throw new ArgumentNullException(nameof(matchers));

            _requestModel.Headers.Add(new Admin.Mappings.HeaderModel
            {
                Name = name,
                Matchers = matchers.Select(x => x.Build()).ToArray()
            });

            return this;
        }

        /// <summary>
        /// Matching on key only.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="rejectOnMatch">The match behaviour (optional).</param>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder WithParam(string key, bool rejectOnMatch = false)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            return WithParam(key, Matcher.Wildcard(true, rejectOnMatch, "*"));
        }

        /// <summary>
        /// Matching on key and values.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder WithParam(string key, params string[] values)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (values == null) throw new ArgumentNullException(nameof(values));

            return WithParam(key, false, values);
        }

        /// <summary>
        /// Matching on key, values and matchBehaviour.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <param name="rejectOnMatch">The match behaviour.</param>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder WithParam(string key, bool rejectOnMatch = false, params string[] values)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (values == null) throw new ArgumentNullException(nameof(values));

            return WithParam(key, values.Select(x => Matcher.Exact(rejectOnMatch, x)).ToArray());
        }

        /// <summary>
        /// Matching on key, matchers and rejectOnMatch.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="matchers">The matchers.</param>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder WithParam(string key, params IStringMatcherModelBuilder[] matchers)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (matchers == null) throw new ArgumentNullException(nameof(matchers));

            _requestModel.Params.Add(new Admin.Mappings.ParamModel
            {
                Name = key,
                Matchers = matchers.Select(x => x.Build()).ToArray()
            });

            return this;
        }

        /// <summary>
        /// Add path matching based on paths.
        /// </summary>
        /// <param name="paths">The matchers.</param>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder WithPath(params string[] paths)
        {
            return WithPath(false, paths);
        }

        /// <summary>
        /// Add path matching based on urls and rejectOnMatch.
        /// </summary>
        /// <param name="rejectOnMatch">The match behaviour.</param>
        /// <param name="paths">The paths.</param>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder WithPath(bool rejectOnMatch, params string[] paths)
        {
            if (paths == null) throw new ArgumentException(nameof(paths));

            return WithPath(paths.Select(x => Matcher.Wildcard(true, rejectOnMatch, x)).ToArray());
        }

        /// <summary>
        /// Add path matching based on matchers.
        /// </summary>
        /// <param name="matchers">The matchers.</param>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder WithPath(params IStringMatcherModelBuilder[] matchers)
        {
            if (matchers == null) throw new ArgumentNullException(nameof(matchers));

            _requestModel.Path = new Admin.Mappings.PathModel
            {
                Matchers = matchers.Select(x => x.Build()).ToArray()
            };

            return this;
        }

        /// <summary>
        /// Add url matching based on urls.
        /// </summary>
        /// <param name="urls">The urls.</param>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder WithUrl(params string[] urls)
        {
            if (urls == null) throw new ArgumentNullException(nameof(urls));

            return WithUrl(false, urls);
        }

        /// <summary>
        /// Add url matching based on urls.
        /// </summary>
        /// <param name="rejectOnMatch">The match behaviour.</param>
        /// <param name="urls">The urls.</param>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder WithUrl(bool rejectOnMatch = false, params string[] urls)
        {
            if (urls == null) throw new ArgumentNullException(nameof(urls));

            return WithUrl(urls.Select(x => Matcher.Wildcard(true, rejectOnMatch, x)).ToArray());
        }

        /// <summary>
        /// Add url matching based on matchers.
        /// </summary>
        /// <param name="matchers">The matchers.</param>
        /// <returns>The <see cref="RequestModelBuilder"/>.</returns>
        public RequestModelBuilder WithUrl(params IStringMatcherModelBuilder[] matchers)
        {
            if (matchers == null) throw new ArgumentNullException(nameof(matchers));

            _requestModel.Url = new Admin.Mappings.UrlModel
            {
                Matchers = matchers.Select(x => x.Build()).ToArray()
            };

            return this;
        }
    }
}