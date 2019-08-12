using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Sitecore;
using ScHelix.Foundation.HelixCore.RenderingAssets.Models;
using ScHelix.Foundation.HelixCore.RenderingAssets.Repositories;

namespace ScHelix.Foundation.HelixCore.RenderingAssets.Services {
    /// <summary>
    ///     A service which helps add the required JavaScript at the end of a page, and CSS at the top of a page.
    ///     In component based architecture it ensures references and inline scripts are only added once.
    /// </summary>
    public class RenderAssetsService {
        private static RenderAssetsService _current;
        public static RenderAssetsService Current => _current ?? (_current = new RenderAssetsService());

        public HtmlString RenderScripts() {
            IEnumerable<Asset> assets = AssetRepository.Current.Items.Where(asset => (asset.Type == AssetType.JavaScript || asset.Type == AssetType.Raw) && IsForContextSite(asset));
            StringBuilder sb = new StringBuilder();

            foreach (Asset item in assets) {
                if (item.Type == AssetType.Raw) {
                    sb.Append(item.Content).AppendLine();
                } else {
                    sb.AppendFormat("<script src=\"{0}\"></script>", item.Content).AppendLine();
                }
            }

            return new HtmlString(sb.ToString());
        }

        public HtmlString RenderStyles() {
            StringBuilder sb = new StringBuilder();

            foreach (Asset item in AssetRepository.Current.Items.Where(asset => asset.Type == AssetType.Css && IsForContextSite(asset))) {
                sb.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" />", item.Content).AppendLine();
            }

            return new HtmlString(sb.ToString());
        }

        private static bool IsForContextSite(Asset asset) {
            if (asset.Site == null) {
                return true;
            }

            foreach (string part in asset.Site.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries)) {
                string siteWildcard = part.Trim().ToLowerInvariant();

                if (siteWildcard == "*" || Context.Site.Name.Equals(siteWildcard, StringComparison.InvariantCultureIgnoreCase)) {
                    return true;
                }
            }

            return false;
        }
    }
}
