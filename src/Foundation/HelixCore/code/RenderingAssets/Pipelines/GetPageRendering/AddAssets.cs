using System.Collections.Generic;
using Sitecore.Mvc.Pipelines.Response.GetPageRendering;
using ScHelix.Foundation.HelixCore.RenderingAssets.Models;
using ScHelix.Foundation.HelixCore.RenderingAssets.Repositories;

namespace ScHelix.Foundation.HelixCore.RenderingAssets.Pipelines.GetPageRendering {
    public class AddAssets : GetPageRenderingProcessor {
        private IList<Asset> _siteAssets;
        private IEnumerable<Asset> SiteAssets => _siteAssets ?? (_siteAssets = new List<Asset>());

        public override void Process(GetPageRenderingArgs args) {
            foreach (Asset asset in SiteAssets) {
                AssetRepository.Current.Add(asset, true);
            }
        }
    }
}
