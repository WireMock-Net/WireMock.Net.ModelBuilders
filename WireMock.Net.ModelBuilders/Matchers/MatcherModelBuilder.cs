using System;
using WireMock.Admin.Mappings;

namespace WireMock.Net.ModelBuilders.Matchers
{
    /// <summary>
    /// MatcherModelBuilder
    /// </summary>
    public class MatcherModelBuilder : IMatcherModelBuilder
    {
        private readonly Func<MatcherModel> _builder;

        /// <summary>
        /// Creates an instance of <see cref="MatcherModelBuilder" />
        /// </summary>
        /// <param name="builder">Function that creates the matcher model.</param>
        public MatcherModelBuilder(Func<MatcherModel> builder)
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