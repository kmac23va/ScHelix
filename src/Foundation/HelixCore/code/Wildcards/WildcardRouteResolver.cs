﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ScHelix.Foundation.HelixCore.Wildcards.Extensions;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Sites;

namespace ScHelix.Foundation.HelixCore.Wildcards {
    public class WildcardRouteResolver : RouteResolver {
        private static readonly object InitSyncRoot = new object();
        private static ICollection<WildcardRouteItem> _routes;
        protected static ConcurrentDictionary<string, WildcardRouteItem> ItemResolverRoutesCache = new ConcurrentDictionary<string, WildcardRouteItem>();
        protected static ConcurrentDictionary<string, WildcardRouteItem> LinkProviderRoutesCache = new ConcurrentDictionary<string, WildcardRouteItem>();
        protected virtual string RoutesPath => Settings.GetSetting("WildcardProvider.RoutesPath", "/sitecore/system/modules/wildcards/routes");

        public override WildcardRouteItem GetWildcardRouteForItemResolver(Item item, SiteContext site) {
            Assert.ArgumentNotNull(item, "item");
            string cacheKey = $"cache{item.ID}_{item.Language.Name}_{site.Name}";

            if (ItemResolverRoutesCache.ContainsKey(cacheKey)) {
                return ItemResolverRoutesCache[cacheKey];
            }

            WildcardRouteItem route = Routes?.FirstOrDefault(x => x.WildcardItemIds.Contains(item.ID));

            if (route == null) {
                return null;
            }

            ItemResolverRoutesCache.TryAdd(cacheKey, route);

            return route;
        }

        public override WildcardRouteItem GetWildcardRouteForLinkProvider(Item item, SiteContext site) {
            Assert.ArgumentNotNull(item, "item");
            string cacheKey = $"cache{item.TemplateID}_{item.Language.Name}_{site.Name}";

            if (LinkProviderRoutesCache.ContainsKey(cacheKey)) {
                return LinkProviderRoutesCache[cacheKey];
            }

            WildcardRouteItem route = Routes?.FirstOrDefault(x => x.ItemTemplates.Contains(new TemplateID(item.TemplateID)));

            if (route == null) {
                return null;
            }

            LinkProviderRoutesCache.TryAdd(cacheKey, route);

            return route;
        }

        public override void ClearCache() {
            ItemResolverRoutesCache.Clear();
            LinkProviderRoutesCache.Clear();

            _routes = null;
        }

        protected virtual IEnumerable<WildcardRouteItem> Routes {
            get {
                if (Context.Database == null) {
                    return null;
                }

                if (_routes != null && _routes.Any()) {
                    return _routes;
                }

                lock (InitSyncRoot) {
                    if (_routes != null && _routes.Any()) {
                        return _routes;
                    }

                    Item routesRootItem = Context.Database.GetItem(RoutesPath);

                    if (routesRootItem == null) {
                        return null;
                    }

                    _routes = routesRootItem
                        .GetChildrenReccursively(WildcardRouteItem.TemplateId.Guid)
                        .Select(x => new WildcardRouteItem(x))
                        .ToArray();
                }

                return _routes;
            }
        }
    }
}
