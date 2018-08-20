using System;
using WireMock.Admin.Mappings;

namespace WireMock.Net.ModelBuilders.Matchers
{
    /// <summary>
    /// StringMatcherModelBuilder
    /// </summary>
    public class StringMatcherModelBuilder : IStringMatcherModelBuilder
    {
        private readonly Func<MatcherModel> _builder;

        /// <summary>
        /// Creates an instance of <see cref="StringMatcherModelBuilder" />
        /// </summary>
        /// <param name="builder">Function that creates the matcher model.</param>
        public StringMatcherModelBuilder(Func<MatcherModel> builder)
        {
            _builder = builder;
        }

        /// <inheritdoc cref="IMatcherModelBuilder.Build" />
        public MatcherModel Build()
        {
            return _builder();
        }
    }
}