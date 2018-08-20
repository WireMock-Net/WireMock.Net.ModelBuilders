using SimMetrics.Net;
using WireMock.Admin.Mappings;
using WireMock.Net.ModelBuilders.Matchers;

namespace WireMock.Net.ModelBuilders
{
    /// <summary>
    /// Create matcher models.
    /// </summary>
    public static class Matcher
    {
        /// <summary>
        /// Exact matcher.
        /// </summary>
        /// <param name="rejectOnMatch">The matching behaviour.</param>
        /// <param name="patterns">The patterns.</param>
        public static IStringMatcherModelBuilder Exact(bool rejectOnMatch = false, params string[] patterns)
        {
            return new StringMatcherModelBuilder(() =>
                new MatcherModel
                {
                    Name = "ExactMatcher",
                    Patterns = patterns,
                    RejectOnMatch = rejectOnMatch
                });
        }

        /// <summary>
        /// Exact matcher.
        /// </summary>
        /// <param name="patterns">The patterns.</param>
        public static IStringMatcherModelBuilder Exact(params string[] patterns)
        {
            return Exact(false, patterns);
        }

        /// <summary>
        /// Json matcher.
        /// </summary>
        /// <param name="json">The the object or JSON string to deep match on.</param>
        /// <param name="rejectOnMatch">The matching behaviour.</param>
        public static IMatcherModelBuilder Json(object json, bool rejectOnMatch = false)
        {
            return new MatcherModelBuilder(() =>
                new MatcherModel
                {
                    Name = "JsonMatcher",
                    Pattern = json,
                    RejectOnMatch = rejectOnMatch
                });
        }

        /// <summary>
        /// JsonPath matcher.
        /// </summary>
        /// <param name="rejectOnMatch">The matching behaviour.</param>
        /// <param name="patterns">The patterns.</param>
        public static IStringMatcherModelBuilder JsonPath(bool rejectOnMatch = false, params string[] patterns)
        {
            return new StringMatcherModelBuilder(() =>
                new MatcherModel
                {
                    Name = "JsonPathMatcher",
                    Patterns = patterns,
                    RejectOnMatch = rejectOnMatch
                });
        }

        /// <summary>
        /// JsonPath matcher.
        /// </summary>
        /// <param name="patterns">The patterns.</param>
        public static IStringMatcherModelBuilder JsonPath(params string[] patterns)
        {
            return JsonPath(false, patterns);
        }

        /// <summary>
        /// Regex matcher.
        /// </summary>
        /// <param name="ignoreCase">Ignore the case from the pattern.</param>
        /// <param name="rejectOnMatch">The matching behaviour.</param>
        /// <param name="patterns">The patterns.</param>
        public static IStringMatcherModelBuilder Regex(bool ignoreCase = true, bool rejectOnMatch = false, params string[] patterns)
        {
            return new StringMatcherModelBuilder(() =>
                new MatcherModel
                {
                    IgnoreCase = ignoreCase,
                    Name = "RegexMatcher",
                    Patterns = patterns,
                    RejectOnMatch = rejectOnMatch
                });
        }

        /// <summary>
        /// Regex matcher.
        /// </summary>
        /// <param name="patterns">The patterns.</param>
        public static IStringMatcherModelBuilder Regex(params string[] patterns)
        {
            return Regex(true, false, patterns);
        }

        /// <summary>
        /// SimMetrics matcher.
        /// </summary>
        /// <param name="type">The SimMetrics type.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="rejectOnMatch">The matching behaviour.</param>
        public static IMatcherModelBuilder SimMetrics(SimMetricType type, string pattern, bool rejectOnMatch = false)
        {
            var matcher = $"SimMetricsMatcher.{type.ToString()}";
            return new MatcherModelBuilder(() =>
                new MatcherModel
                {
                    Name = matcher,
                    Pattern = pattern,
                    RejectOnMatch = rejectOnMatch
                });
        }

        /// <summary>
        /// Wildcard matcher.
        /// </summary>
        /// <param name="ignoreCase">Ignore the case from the pattern.</param>
        /// <param name="rejectOnMatch">The matching behaviour.</param>
        /// <param name="patterns">The patterns.</param>
        public static IStringMatcherModelBuilder Wildcard(bool ignoreCase = true, bool rejectOnMatch = false, params string[] patterns)
        {
            return new StringMatcherModelBuilder(() =>
                new MatcherModel
                {
                    IgnoreCase = ignoreCase,
                    Name = "WildcardMatcher",
                    Patterns = patterns,
                    RejectOnMatch = rejectOnMatch
                });
        }

        /// <summary>
        /// Wildcard matcher.
        /// </summary>
        /// <param name="patterns">The patterns.</param>
        public static IStringMatcherModelBuilder Wildcard(params string[] patterns)
        {
            return Wildcard(true, false, patterns);
        }

        /// <summary>
        /// XPath matcher.
        /// </summary>
        /// <param name="xpath">The xpath.</param>
        /// <param name="rejectOnMatch">The matching behaviour.</param>
        public static IStringMatcherModelBuilder XPath(string xpath, bool rejectOnMatch = false)
        {
            return new StringMatcherModelBuilder(() =>
                new MatcherModel
                {
                    Name = "XPathMatcher",
                    Pattern = xpath,
                    RejectOnMatch = rejectOnMatch
                });
        }
    }
}