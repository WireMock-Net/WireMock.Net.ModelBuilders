using WireMock.Admin.Mappings;

namespace WireMock.Net.ModelBuilders
{
    /// <summary>
    /// IMatcherModelBuilder
    /// </summary>
    public interface IMatcherModelBuilder
    {
        /// <summary>
        /// Builds a <see cref="MatcherModel" />.
        /// </summary>
        /// <returns>The <see cref="MatcherModel" /></returns>
        MatcherModel Build();
    }
}