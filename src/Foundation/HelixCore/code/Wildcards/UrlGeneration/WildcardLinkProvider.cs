using System;
using System.Collections.Generic;
using System.Linq;
using ScHelix.Foundation.HelixCore.Wildcards.UrlGeneration.TokenValueExtraction;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Links;

namespace ScHelix.Foundation.HelixCore.Wildcards.UrlGeneration {
    public class WildcardLinkProvider : LinkProvider {
        protected string WildcardTokenizedString => Settings.GetSetting("WildcardTokenizedString", ",-w-,");

        public override string GetItemUrl(Item item, UrlOptions options) {
            if (item == null || options == null) {
                return base.GetItemUrl(item, options);
            }

            WildcardRouteItem route = WildcardManager.Current.GetWildcardRouteForLinkProvider(item, Context.Site);

            if (route == null || route.WildcardItemIds.Any(x => x == item.ID)) {
                return base.GetItemUrl(item, options);
            }

            string url = base.GetItemUrl(item, options);
            string routeUrl = GetMostSuitableRootItemUrl(route, options);

            return string.IsNullOrEmpty(routeUrl) ? url : ReplaceUrlTokens(routeUrl, item, route, options);
        }

        protected virtual string ReplaceUrlTokens(string routeUrl, Item item, WildcardRouteItem routeItem, UrlOptions options) {
            List<string> resultUrl = new List<string>();
            IDictionary<int, string> tokenValues = GetTokenValues(item, routeItem, options);
            string[] urlPattern = routeUrl.Split('/');
            int tokenCounter = 0;

            foreach (string segment in urlPattern) {
                string resultSegment = segment;

                if (segment.ToLower() == WildcardTokenizedString) {
                    resultSegment = tokenValues[tokenCounter++];
                }

                resultUrl.Add(resultSegment);
            }

            return string.Join("/", resultUrl);
        }

        protected virtual IDictionary<int, string> GetTokenValues(Item item, WildcardRouteItem routeItem, UrlOptions options) {
            Dictionary<int, string> ret = new Dictionary<int, string>();
            IDictionary<int, string> rules = routeItem.UrlGenerationRules;

            foreach (KeyValuePair<int, string> rule in rules) {
                string mapping = rule.Value.Trim();
                string tokenValue = UrlGenerationTokenValueExtractor.Current.ExtractTokenValue(mapping, item);

                if (options.LowercaseUrls) {
                    tokenValue = tokenValue.ToLower();
                }

                tokenValue = tokenValue.Replace(" ", "-");
                ret.Add(rule.Key, tokenValue);
            }

            return ret;
        }

        protected virtual string GetMostSuitableRootItemUrl(WildcardRouteItem routeItem, UrlOptions options) {
            string url = string.Empty;
            IEnumerable<Item> rootItems = routeItem.WildcardItems;
            Item currentSiteItem = rootItems.FirstOrDefault(x => x.Paths.FullPath.StartsWith(Context.Site.RootPath, StringComparison.InvariantCultureIgnoreCase));

            return currentSiteItem != null ? base.GetItemUrl(currentSiteItem, options) : url;
        }
    }
}
