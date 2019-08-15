using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;

namespace ScHelix.Foundation.HelixCore.Wildcards.ItemResolving {
    public class ContentSearchWildcardItemResolver : ItemResolver {
        private IDictionary<string, string> encodeNameReplacements;
        protected virtual IDictionary<string, string> EncodeNameReplacements => encodeNameReplacements ?? (encodeNameReplacements = GetEncodeNameReplacements());
        protected static bool WildcardWrapSearchTermsInEncodedQuotes => Settings.GetBoolSetting("WildcardWrapSearchTermsInEncodedQuotes", false);
        protected static bool WildcardTokenizeSearchTerms => Settings.GetBoolSetting("WildcardTokenizeSearchTerms", false);

        public override Item ResolveItem(Item item, WildcardRouteItem routeItem) {
            if (routeItem == null) {
                return item;
            }

            IDictionary<string, string> tokenValues = GetTokenValues(item, routeItem);
            Item rootItem = routeItem.ItemSearchRootNode;

            using (IProviderSearchContext searchContext = ContentSearchManager.CreateSearchContext((SitecoreIndexableItem) item)) {
                IQueryable<SearchResultItem> queryable = searchContext.GetQueryable<SearchResultItem>();
                Expression<Func<SearchResultItem, bool>> predicate = PredicateBuilder.True<SearchResultItem>();
                predicate = routeItem.ItemTemplates.Aggregate(predicate, (current, itemTemplateId) => current.Or(p => p.TemplateId == itemTemplateId.ID));

                queryable = queryable
                    .Where(predicate)
                    .Where(resultItem => resultItem.Language == Context.Language.Name);

                if (rootItem != null) {
                    queryable = queryable.Where(resultItem => resultItem.Paths.Contains(rootItem.ID));
                }

                queryable = tokenValues.Aggregate(queryable, (current, tokenRule) => AddTokenPredicates(tokenRule, current));
                SearchResultItem result = queryable.FirstOrDefault();

                if (result == null) {
                    return item;
                }

                Item resolvedItem = result.GetItem();

                if (resolvedItem != null) {
                    return resolvedItem;
                }
            }

            return item;
        }

        protected IQueryable<SearchResultItem> AddTokenPredicates(KeyValuePair<string, string> tokenRule, IQueryable<SearchResultItem> queryable) {
            Expression<Func<SearchResultItem, bool>> predicate = PredicateBuilder.True<SearchResultItem>();
            string indexFieldName = GetPropertyIndexName(tokenRule.Key);
            string searchTerm = GetTokenSearchibleValue(tokenRule.Value);
            predicate = EncodeNameReplacements.Select(replacement => replacement.Value).Where(value => searchTerm.ToLower().Contains(value.ToLower())).Aggregate(predicate, (current1, value) => EncodeNameReplacements.Where(x => x.Value == value).Select(name => searchTerm.Replace(name.Value, name.Key)).Aggregate(current1, (current, searchTermVariant) => current.Or(p => p[indexFieldName] == searchTermVariant)));
            predicate = predicate.Or(second: p => p[indexFieldName] == searchTerm);

            return queryable.Where(predicate);
        }

        protected virtual string GetTokenSearchibleValue(string value) {
            if (WildcardTokenizeSearchTerms) {
                value = value
                    .Replace("-", "")
                    .Replace("+", "")
                    .Replace(" ", "")
                    .ToLower();
            }

            if (WildcardWrapSearchTermsInEncodedQuotes) {
                return "%22" + value + "%22";
            }

            return value;
        }

        protected virtual string GetPropertyIndexName(string key) => key.ToLower().Replace(" ", "_").Replace("-", "_");

        protected virtual IDictionary<string, string> GetEncodeNameReplacements() {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            XmlNode nameReplacements = Factory.GetConfigNode("encodeNameReplacements", false);

            if (nameReplacements == null) {
                return ret;
            }

            foreach (XmlNode replacementNode in nameReplacements.ChildNodes) {
                if (replacementNode.Attributes == null) {
                    continue;
                }

                XmlAttribute modeAttr = replacementNode.Attributes["mode"];
                XmlAttribute findAttr = replacementNode.Attributes["find"];
                XmlAttribute replaceWithAttr = replacementNode.Attributes["replaceWith"];

                if (modeAttr == null || findAttr == null || replaceWithAttr == null) {
                    continue;
                }

                string mode = modeAttr.Value;
                string find = findAttr.Value;
                string replaceWith = replaceWithAttr.Value;

                if ("on".Equals(mode, StringComparison.InvariantCultureIgnoreCase) && !ret.ContainsKey(find) && !string.IsNullOrEmpty(find)) {
                    ret.Add(find, replaceWith);
                }
            }

            return ret;
        }
    }
}
