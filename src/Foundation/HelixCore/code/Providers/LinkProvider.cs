using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Links;

namespace ScHelix.Foundation.HelixCore.Providers {
    public class LinkProvider : Sitecore.Links.LinkProvider {
        public override string GetItemUrl(Item item, UrlOptions options) {
            if (item == null) {
                return string.Empty;
            }

            options.SiteResolving = Settings.Rendering.SiteResolving;
            string itemUrl = base.GetItemUrl(item, options);

            return itemUrl;
        }
    }
}
