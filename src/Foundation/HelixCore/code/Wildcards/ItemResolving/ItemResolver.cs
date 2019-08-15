using System.Collections.Generic;
using System.Configuration.Provider;
using System.Web;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Sites;

namespace ScHelix.Foundation.HelixCore.Wildcards.ItemResolving {
    public class ItemResolver : ProviderBase {
        protected string WildcardTokenizedString => Settings.GetSetting("WildcardTokenizedString", ",-w-,");

        public virtual Item ResolveItem(Item item, WildcardRouteItem routeItem) => null;

        protected virtual IDictionary<string, string> GetTokenValues(Item item, WildcardRouteItem routeItem) {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            string[] itemUrl;

            using (new SiteContextSwitcher(Context.Site)) {
                UrlOptions options = UrlOptions.DefaultOptions;
                options.AlwaysIncludeServerUrl = false;
                string url = LinkManager.GetItemUrl(item, options);
                itemUrl = url.Split('/');
            }

            string[] requestUrl = HttpContext.Current.Request.Url.LocalPath.Split('/');
            IDictionary<int, string> rules = routeItem.ItemResolvingRules;
            int ruleIndex = 0;
            int index = 0;

            foreach (string rurl in requestUrl) {
                if (itemUrl[index] == WildcardTokenizedString) {
                    if (rules.ContainsKey(ruleIndex)) {
                        string mapping = rules[ruleIndex];

                        if (mapping == null) {
                            throw new WildcardException("Can't resolve wildcards by index " + (ruleIndex));
                        }

                        mapping = HttpUtility.UrlDecode(mapping);

                        if (!string.IsNullOrEmpty(mapping)) {
                            if (!ret.ContainsKey(mapping)) {
                                ret.Add(mapping, rurl);
                            }
                        }
                    }

                    ruleIndex++;
                }

                index++;
            }

            return ret;
        }
    }
}
