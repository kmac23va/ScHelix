using System.Collections.Generic;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.Response.GetPageRendering;
using Sitecore.Mvc.Presentation;
using ScHelix.Foundation.HelixCore.RenderingAssets.Repositories;
using ScHelix.Foundation.HelixCore.Extensions;

namespace ScHelix.Foundation.HelixCore.RenderingAssets.Pipelines.GetPageRendering {
    public class AddRenderingAssets : GetPageRenderingProcessor {
        public override void Process(GetPageRenderingArgs args) => AddAssets(args.PageContext.PageDefinition.Renderings);

        private void AddAssets(IEnumerable<Rendering> renderings) {
            foreach (Rendering rendering in renderings) {
                Item renderingItem = GetRenderingItem(rendering);

                if (renderingItem == null) {
                    return;
                }

                AddAssetsFromItem(renderingItem);
            }
        }

        protected static void AddAssetsFromItem(Item renderingItem) {
            if (!renderingItem.IsDerived(Templates.RenderingAssets.Id)) {
                return;
            }

            AddScriptAssetsFromRendering(renderingItem);
            AddStylingAssetsFromRendering(renderingItem);
        }

        private static void AddStylingAssetsFromRendering(Item renderingItem) {
            string cssAssets = renderingItem[Templates.RenderingAssets.Fields.StylingFiles];

            foreach (string cssAsset in cssAssets.Split(';', ',', '\n')) {
                AssetRepository.Current.AddStylingFile(cssAsset, true);
            }
        }

        private static void AddScriptAssetsFromRendering(Item renderingItem) {
            string javaScriptAssets = renderingItem[Templates.RenderingAssets.Fields.ScriptFiles];

            foreach (string javaScriptAsset in javaScriptAssets.Split(';', ',', '\n')) {
                AssetRepository.Current.AddScriptFile(javaScriptAsset, true);
            }
        }

        private Item GetRenderingItem(Rendering rendering) {
            if (rendering.RenderingItem == null) {
                Log.Warn($"rendering.RenderingItem is null for {rendering.RenderingItemPath}", this);
                return null;
            }

            if (Context.PageMode.IsNormal && rendering.Caching.Cacheable) {
                AssetRepository.Current.Add(rendering.RenderingItem.ID);
            }

            return rendering.RenderingItem.InnerItem;
        }
    }
}
