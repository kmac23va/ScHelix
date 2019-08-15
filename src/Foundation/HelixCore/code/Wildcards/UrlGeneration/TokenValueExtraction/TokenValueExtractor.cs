using System.Configuration.Provider;
using Sitecore.Data.Items;

namespace ScHelix.Foundation.HelixCore.Wildcards.UrlGeneration.TokenValueExtraction {
    public class TokenValueExtractor : ProviderBase {
        public virtual string ExtractTokenValue(string tokenPattern, Item item) => string.Empty;

        public virtual string HelpText => "This token extractor does not have explanation text.";
    }
}
