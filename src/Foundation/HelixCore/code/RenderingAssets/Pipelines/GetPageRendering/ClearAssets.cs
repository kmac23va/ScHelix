using Sitecore.Mvc.Pipelines.Response.GetPageRendering;
using ScHelix.Foundation.HelixCore.RenderingAssets.Repositories;

namespace ScHelix.Foundation.HelixCore.RenderingAssets.Pipelines.GetPageRendering {
    public class ClearAssets : GetPageRenderingProcessor {
        public override void Process(GetPageRenderingArgs args) => AssetRepository.Current.Clear();
    }
}
