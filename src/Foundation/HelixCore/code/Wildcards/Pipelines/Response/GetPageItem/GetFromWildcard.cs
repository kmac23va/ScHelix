using System.Web;
using ScHelix.Foundation.HelixCore.Wildcards.ItemResolving;
using Sitecore;
using Sitecore.Abstractions;
using Sitecore.Data.Items;
using Sitecore.Mvc.Pipelines.Response.GetPageItem;

namespace ScHelix.Foundation.HelixCore.Wildcards.Pipelines.Response.GetPageItem {
    public class GetFromWildcard : GetPageItemProcessor {
        private const string OriginalItemCacheKey = "Wildcards.OriginalItem";

        public GetFromWildcard(BaseClient baseClient) : base(baseClient) {
        }

        public override void Process(GetPageItemArgs args) {
            if (!WildvardResolvingPossible(args)) {
                return;
            }

            HttpContext.Current.Items[OriginalItemCacheKey] = args.Result;
            Item resolvedItem = ResolveItem(args);

            if (resolvedItem != null) {
                args.Result = resolvedItem;
            }
        }

        protected virtual bool WildvardResolvingPossible(GetPageItemArgs args) {
            if (args?.Result == null || Context.Site == null || Context.Database == null || Context.Database.Name == "core") {
                return false;
            }

            return WildcardManager.Current.HasWildcardsPath(args.Result);
        }

        protected virtual Item ResolveItem(GetPageItemArgs args) {
            if (args.Result == null) {
                return null;
            }

            WildcardRouteItem route = WildcardManager.Current.GetWildcardRouteForItemResolver(args.Result, Context.Site);
            return WildcardItemResolver.Current.ResolveItem(args.Result, route);
        }
    }
}
